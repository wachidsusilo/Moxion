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

internal class MotionAccelerationCalculator : IMotionAccelerationCalculator
{
  private readonly IKinematics _kinematics;
  private readonly ILogger<MotionAccelerationCalculator> _logger;

  public MotionAccelerationCalculator(
    IKinematics kinematics,
    ILogger<MotionAccelerationCalculator> logger
  )
  {
    _kinematics = kinematics;
    _logger = logger;
  }

  public async Task<Result<MotionAccelerationCalculationResult>> Execute(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart( param.TimeSlices.Count );

    var result = await ExecuteInternal( param, cancellationToken );

    _logger.LogEnd( result, param.TimeSlices.Count );

    return result;
  }

  private Task<Result<MotionAccelerationCalculationResult>> ExecuteInternal(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    return Task.Run( () =>
      {
        var accelerationData = new List<Acceleration>( param.TimeSlices.Count );

        foreach (var time in param.TimeSlices)
        {
          if (cancellationToken.IsCancellationRequested)
          {
            return Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled );
          }

          var phase = param.Profile.CalculatePhase( time );
          var phaseDuration = param.Profile.TimeProfile.CalculateDuration( time, phase );

          if (phase == MotionPhase.AccelerationWithPositiveJerk)
          {
            accelerationData.Add( _kinematics.CalculateCubicAcceleration( phaseDuration, param.Profile.Jerk ) );
            continue;
          }

          if (phase == MotionPhase.ConstantAcceleration)
          {
            accelerationData.Add( param.Profile.MaxAcceleration );
            continue;
          }

          if (phase == MotionPhase.AccelerationWithNegativeJerk)
          {
            accelerationData.Add(
              param.Profile.MaxAcceleration
              + _kinematics.CalculateCubicAcceleration( phaseDuration, -param.Profile.Jerk )
            );

            continue;
          }

          if (phase == MotionPhase.ConstantVelocity)
          {
            accelerationData.Add( Acceleration.Zero );
            continue;
          }

          if (phase == MotionPhase.DecelerationWithNegativeJerk)
          {
            accelerationData.Add( _kinematics.CalculateCubicAcceleration( phaseDuration, -param.Profile.Jerk ) );
            continue;
          }

          if (phase == MotionPhase.ConstantDeceleration)
          {
            accelerationData.Add( -param.Profile.MaxAcceleration );
            continue;
          }

          accelerationData.Add(
            -param.Profile.MaxAcceleration
            + _kinematics.CalculateCubicAcceleration( phaseDuration, param.Profile.Jerk )
          );
        }

        var result = new MotionAccelerationCalculationResult( accelerationData );
        return Result.Success( result );
      },
      CancellationToken.None
    );
  }
}