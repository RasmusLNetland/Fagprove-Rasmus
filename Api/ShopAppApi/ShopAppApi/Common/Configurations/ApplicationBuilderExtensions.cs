using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;
using System.Text.Json;
using ShopAppApi.Infrastructure.Common;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Common.Configurations;

public static class ApplicationBuilderExtensions
{
    public static void ConfigureExceptionHandler( this IApplicationBuilder app, ILoggerFactory loggerFactory )
    {
        ILogger logger = loggerFactory.CreateLogger( "GlobalExceptionHandlerLogger" );
        app.UseExceptionHandler( appError =>
        {
            appError.Run( async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if( contextFeature != null )
                {
                    logger.LogError( $"Something went wrong: {contextFeature.Error}" );
                    string message;
                    int statusCode;
                    switch( contextFeature.Error )
                    {
                        case BadRequestException:
                            statusCode = (int)HttpStatusCode.BadRequest;
                            message = $"{contextFeature.Error.Message}";
                            break;
                        case NotFoundException:
                            statusCode = (int)HttpStatusCode.NotFound;
                            message = $"{contextFeature.Error.Message}";
                            break;
                        case UnprocessableEntityException:
                            statusCode = (int)HttpStatusCode.UnprocessableEntity;
                            message = $"Unprocessable entity. Error is : {contextFeature.Error.Message}";
                            break;
                        default:
                            statusCode = (int)HttpStatusCode.InternalServerError;
                            message = "Internal Server Error.";
                            break;
                    }

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsJsonAsync( new ErrorDetails()
                    {
                        StatusCode = statusCode,
                        Message = message
                    } );
                }
            } );
        } );
    }

    internal class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonSerializer.Serialize( this );
        }
    }

    #region Swagger

    public static IApplicationBuilder UseCustomSwagger( this IApplicationBuilder builder, IApiVersionDescriptionProvider provider )
    {
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        builder.UseSwagger( c => c.SerializeAsV2 = true );

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
        // specifying the Swagger JSON endpoint.
        builder.UseSwaggerUI( options =>
        {
            // build a swagger endpoint for each discovered API version
            foreach( ApiVersionDescription description in provider.ApiVersionDescriptions )
                options.SwaggerEndpoint( $"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant() );

            options.SupportedSubmitMethods(); // Disable "Try it Out" functionality, not needed.
        } );

        return builder;
    }

    #endregion
}