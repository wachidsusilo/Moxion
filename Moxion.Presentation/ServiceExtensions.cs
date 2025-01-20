using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moxion.Application.Feature;
using Moxion.Infrastructure.Diagnostic;
using Moxion.Infrastructure.Kinematic;
using Moxion.Presentation.Abstractions.Converters;
using Moxion.Presentation.Abstractions.Extractors;
using Moxion.Presentation.Abstractions.Factories;
using Moxion.Presentation.Abstractions.Validators;
using Moxion.Presentation.Converters;
using Moxion.Presentation.Extractors;
using Moxion.Presentation.Factories;
using Moxion.Presentation.Validators.Requests;

namespace Moxion.Presentation;

public static class ServiceExtensions
{
  public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration configuration )
  {
    return services
      .AddDiagnostic( configuration )
      .AddKinematicServices()
      .AddFeatures()
      .AddConverters()
      .AddValidators()
      .AddExtractors()
      .AddParamFactories()
      .AddParamFactories()
      .AddResponseFactories()
      .AddValueObjectFactories()
      .AddMediatR( config => config.RegisterServicesFromAssembly( AssemblyReference.Assembly ) );
  }

  private static IServiceCollection AddConverters( this IServiceCollection services )
  {
    return services
      .AddSingleton<IUnitConverter, UnitConverter>();
  }

  private static IServiceCollection AddValidators( this IServiceCollection services )
  {
    return services
      .AddSingleton<IKinematicSimulateRequestValidator, KinematicSimulateRequestValidator>();
  }

  private static IServiceCollection AddExtractors( this IServiceCollection services )
  {
    return services
      .AddSingleton<IKinematicUnitExtractor, KinematicUnitExtractor>();
  }

  private static IServiceCollection AddParamFactories( this IServiceCollection services )
  {
    return services
      .AddSingleton<IKinematicQueryFactory, KinematicQueryFactory>();
  }

  private static IServiceCollection AddResponseFactories( this IServiceCollection services )
  {
    return services
      .AddSingleton<IKinematicResponseFactory, KinematicResponseFactory>();
  }

  private static IServiceCollection AddValueObjectFactories( this IServiceCollection services )
  {
    return services
      .AddSingleton<IMotionDataFactory, MotionDataFactory>()
      .AddSingleton<IMotionProfileFactory, MotionProfileFactory>();
  }
}