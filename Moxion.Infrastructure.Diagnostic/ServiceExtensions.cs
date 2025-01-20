using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moxion.Infrastructure.Diagnostic.Enrichers;
using Serilog;

namespace Moxion.Infrastructure.Diagnostic;

public static class ServiceExtensions
{
  public static IServiceCollection AddDiagnostic( this IServiceCollection services, IConfiguration configuration )
  {
    Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration( configuration )
      .Enrich.With<ClassNameEnricher>()
      .CreateLogger();

    return services
      .AddLogging( builder =>
        {
          builder.ClearProviders();
          builder.AddSerilog();
        }
      );
  }
}