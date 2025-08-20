using Microsoft.Extensions.Options;
using ShopAppApi.Infrastructure.DataStorage;
using ShopAppApi.Infrastructure.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Common.Configurations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices( this IServiceCollection services, IConfiguration configuration )
    {
        services.AddSingleton<ITokenService>(
            s => new TokenService( configuration.GetValue<string>( "FullTokenKey" )!,
                                   s.GetRequiredService<ILogger<TokenService>>() ) );

        services.AddSingleton<IInternalDataStorage, InternalDataStorage>();

        return services;
    }

    public static IServiceCollection AddCustomAuthentication( this IServiceCollection services, IConfiguration configuration )
    {
        string publicKey = configuration.GetSection( "ApiConfiguration" ).GetValue<string>( "PublicTokenKey" )!;
        RSA rsa = RSA.Create();
        rsa.FromXmlString( publicKey );

        services.AddAuthentication( options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                } )
                .AddJwtBearer( options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "ShopAppIssuer",
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey( rsa )
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if( context.Exception.GetType() == typeof(SecurityTokenExpiredException) )
                            {
                                context.Response.Headers.Add( "Token-Expired", "true" );
                            }

                            return Task.CompletedTask;
                        }
                    };
                } );

        return services;
    }

    #region Swagger

    public static IServiceCollection AddApiVersioningAndApiExplorer( this IServiceCollection services )
    {
        services.AddApiVersioning( options =>
        {
            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;
        } );

        services.AddVersionedApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            } );

        return services;
    }

    public static IServiceCollection AddSwagger( this IServiceCollection services )
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        // comments path for the Swagger JSON and UI.
        string[] xmlFiles = new[]
        {
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
        };
        IEnumerable<string> xmlFilePaths = xmlFiles.Select( fileName => Path.Combine( AppContext.BaseDirectory, fileName ) );

        services.AddSwaggerGen(
            options =>
            {
                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();
                // integrate xml comments
                foreach( string filePath in xmlFilePaths ) options.IncludeXmlComments( filePath );
            } );

        return services;
    }
}

internal class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
    public ConfigureSwaggerOptions( IApiVersionDescriptionProvider provider )
    {
        _provider = provider;
    }

    /// <inheritdoc />
    public void Configure( SwaggerGenOptions options )
    {
        // add a swagger document for each discovered API version
        foreach( ApiVersionDescription description in _provider.ApiVersionDescriptions )
        {
            options.SwaggerDoc( description.GroupName, CreateInfoForApiVersion( description ) );
#if DEBUG // Use Swagger with bearer token

            options.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            } );
            options.AddSecurityRequirement( new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            } );

#endif
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion( ApiVersionDescription description )
    {
        OpenApiInfo info = new()
        {
            Title = "Shop App API",
            Version = description.ApiVersion.ToString(),
            Description = "Shop App API documentation"
        };

        if( description.IsDeprecated ) info.Description += " This API version has been deprecated.";

        return info;
    }
}

internal class SwaggerDefaultValues : IOperationFilter
{
    public void Apply( OpenApiOperation operation, OperationFilterContext context )
    {
        ApiDescription apiDescription = context.ApiDescription;

        operation.Deprecated = apiDescription.IsDeprecated();

        if( operation.Parameters == null ) return;

        foreach( OpenApiParameter parameter in operation.Parameters )
        {
            ApiParameterDescription description = apiDescription.ParameterDescriptions.First( p => p.Name == parameter.Name );

            parameter.Description ??= description.ModelMetadata.Description;

            parameter.Required |= description.IsRequired;
        }
    }
}

#endregion