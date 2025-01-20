using Microsoft.Extensions.DependencyInjection;
using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Abstractions.Generators.Kinematic;
using Moxion.Application.Abstractions.Math;
using Moxion.Application.Abstractions.Simulators.Kinematic;
using Moxion.Infrastructure.Kinematic.Calculators;
using Moxion.Infrastructure.Kinematic.Generators;

namespace Moxion.Infrastructure.Kinematic;

public static class ServiceExtensions
{
  public static IServiceCollection AddKinematicServices( this IServiceCollection services )
  {
    return services
      .AddTransient<IKinematics, Kinematics>()
      .AddTransient<IMotionProfileGenerator, MotionProfileGenerator>()
      .AddTransient<IMotionDisplacementCalculator, MotionDisplacementCalculator>()
      .AddTransient<IMotionVelocityCalculator, MotionVelocityCalculator>()
      .AddTransient<IMotionAccelerationCalculator, MotionAccelerationCalculator>()
      .AddTransient<IMotionJerkCalculator, MotionJerkCalculator>()
      .AddTransient<IMotionPhaseCalculator, MotionPhaseCalculator>()
      .AddTransient<IMotionSimulator, MotionSimulator>();
  }
}