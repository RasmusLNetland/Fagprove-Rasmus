using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using ShopAppApi.Common.Configurations;

#pragma warning disable CS8604 // Possible null reference argument.

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

// Serilog
builder.Host.UseSerilog( ( hostingContext, loggerConfiguration ) =>
                             loggerConfiguration.WriteTo.File(
                                 path: Path.Combine(
                                     string.IsNullOrWhiteSpace( hostingContext.Configuration.GetValue<string>( "LogFileBaseDirectory" ) )
                                         ? AppContext.BaseDirectory
                                         : hostingContext.Configuration.GetValue<string>( "LogFileBaseDirectory" ), "Serilog",
                                     "ShopAppApiLog.txt" ),
                                 shared: true, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 28,
                                 flushToDiskInterval: TimeSpan.FromSeconds( 5 ) ) );

builder.Services.AddCustomAuthentication( builder.Configuration );
builder.Services.AddControllers();

builder.Services.AddApiVersioningAndApiExplorer()
       .AddSwagger()
       .AddMediatR( cfg => cfg.RegisterServicesFromAssembly( Assembly.GetExecutingAssembly() ) )
       .AddServices( builder.Configuration.GetSection( "ApiConfiguration" ) )
       .AddHttpContextAccessor();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

IApiVersionDescriptionProvider apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseCustomSwagger( apiVersionDescriptionProvider );

app.ConfigureExceptionHandler( app.Services.GetRequiredService<ILoggerFactory>() );

app.UseRouting();
app.UseAuthentication().UseAuthorization();
app.UseEndpoints( endpoints => endpoints.MapControllers() );

app.Run();