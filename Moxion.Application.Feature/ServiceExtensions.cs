using Microsoft.Extensions.DependencyInjection;

namespace Moxion.Application.Feature;

public static class ServiceExtensions
{
  public static IServiceCollection AddFeatures( this IServiceCollection services )
  {
    return services
      .AddMediatR( configuration => configuration.RegisterServicesFromAssembly( AssemblyReference.Assembly ) );
  }
}