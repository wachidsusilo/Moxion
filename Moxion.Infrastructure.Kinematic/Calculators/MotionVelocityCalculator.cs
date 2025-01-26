using Microsoft.Extensions.Logging;
using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Abstractions.Math;
using Moxion.Application.Extensions;
using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Calculators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Common.Values.Derived;
using Moxion.Infrastructure.Kinematic.Extensions;

namespace Moxion.Infrastructure.Kinematic.Calculators;

internal class MotionVelocityCalculator : IMotionVelocityCalculator
{
  private readonly IKinematics _kinematics;
  private readonly ILogger<MotionVelocityCalculator> _logger;

  public MotionVelocityCalculator(
    IKinematics kinematics,
    ILogger<MotionVelocityCalculator> logger
  )
  {
    _kinematics = kinematics;
    _logger = logger;
  }

  public async Task<Result<MotionVelocityCalculationResult>> Execute(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart( param.TimeSlices.Count );

    var result = await ExecuteInternal( param, cancellationToken );

    _logger.LogEnd( result, param.TimeSlices.Count );

    return result;
  }

  private Task<Result<MotionVelocityCalculationResult>> ExecuteInternal(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionVelocityCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    return Task.Run( () =>
      {
        var velocityData = new List<Velocity>( param.TimeSlices.Count );

        var accelerationWithPositiveJerkMaxVelocity =
          param.Profile.VelocityProfile.CalculateVelocity( MotionPhase.AccelerationWithPositiveJerk );

        var constantAccelerationMaxVelocity =
          param.Profile.VelocityProfile.CalculateVelocity( MotionPhase.ConstantAcceleration );

        var decelerationWithNegativeJerkMaxVelocity =
          param.Profile.VelocityProfile.CalculateVelocity( MotionPhase.DecelerationWithNegativeJerk );

        var constantDecelerationMaxVelocity =
          param.Profile.VelocityProfile.CalculateVelocity( MotionPhase.ConstantDeceleration );

        foreach (var time in param.TimeSlices)
        {
          if (cancellationToken.IsCancellationRequested)
          {
            return Result.Error<MotionVelocityCalculationResult>( ErrorCode.OperationCancelled );
          }

          var phase = param.Profile.CalculatePhase( time );
          var phaseDuration = param.Profile.TimeProfile.CalculateDuration( time, phase );

          if (phase == MotionPhase.AccelerationWithPositiveJerk)
          {
            velocityData.Add( _kinematics.CalculateCubicVelocity( phaseDuration, param.Profile.Jerk ) );
            continue;
          }

          if (phase == MotionPhase.ConstantAcceleration)
          {
            velocityData.Add(
              accelerationWithPositiveJerkMaxVelocity
              + _kinematics.CalculateQuadraticVelocity( phaseDuration, param.Profile.MaxAcceleration )
            );

            continue;
          }

          if (phase == MotionPhase.AccelerationWithNegativeJerk)
          {
            velocityData.Add(
              constantAccelerationMaxVelocity
              + _kinematics.CalculateCubicVelocity(
                phaseDuration,
                param.Profile.MaxAcceleration,
                -param.Profile.Jerk
              )
            );

            continue;
          }

          if (phase == MotionPhase.ConstantVelocity)
          {
            velocityData.Add( param.Profile.MaxVelocity );
            continue;
          }

          if (phase == MotionPhase.DecelerationWithNegativeJerk)
          {
            velocityData.Add(
              param.Profile.MaxVelocity + _kinematics.CalculateCubicVelocity( phaseDuration, -param.Profile.Jerk )
            );

            continue;
          }

          if (phase == MotionPhase.ConstantDeceleration)
          {
            velocityData.Add(
              decelerationWithNegativeJerkMaxVelocity
              + _kinematics.CalculateQuadraticVelocity( phaseDuration, -param.Profile.MaxAcceleration )
            );

            continue;
          }

          velocityData.Add(
            constantDecelerationMaxVelocity
            + _kinematics.CalculateCubicVelocity(
              phaseDuration,
              -param.Profile.MaxAcceleration,
              param.Profile.Jerk
            )
          );
        }

        var result = new MotionVelocityCalculationResult( velocityData );
        return Result.Success( result );
      },
      CancellationToken.None
    );
  }
}