using Microsoft.Extensions.Logging;
using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Abstractions.Math;
using Moxion.Application.Extensions;
using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Calculators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Infrastructure.Kinematic.Extensions;

namespace Moxion.Infrastructure.Kinematic.Calculators;

internal class MotionDisplacementCalculator : IMotionDisplacementCalculator
{
  private readonly IKinematics _kinematics;
  private readonly ILogger<MotionDisplacementCalculator> _logger;

  public MotionDisplacementCalculator(
    IKinematics kinematics,
    ILogger<MotionDisplacementCalculator> logger
  )
  {
    _kinematics = kinematics;
    _logger = logger;
  }

  public async Task<Result<MotionDisplacementCalculationResult>> Execute(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var result = await ExecuteInternal( param, cancellationToken );

    _logger.LogEnd( result );

    return result;
  }

  private Task<Result<MotionDisplacementCalculationResult>> ExecuteInternal(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    return Task.Run( () =>
      {
        var positionData = new List<Position>( param.TimeSlices.Count );

        var accelerationWithPositiveJerkDisplacement =
          param.Profile.PositionProfile.CalculateTotalDisplacement( MotionPhase.AccelerationWithPositiveJerk );

        var constantAccelerationDisplacement =
          param.Profile.PositionProfile.CalculateTotalDisplacement( MotionPhase.ConstantAcceleration );

        var accelerationWithNegativeJerkDisplacement =
          param.Profile.PositionProfile.CalculateTotalDisplacement( MotionPhase.AccelerationWithNegativeJerk );

        var constantVelocityDisplacement =
          param.Profile.PositionProfile.CalculateTotalDisplacement( MotionPhase.ConstantVelocity );

        var decelerationWithNegativeJerkDisplacement =
          param.Profile.PositionProfile.CalculateTotalDisplacement( MotionPhase.DecelerationWithNegativeJerk );

        var constantDecelerationDisplacement =
          param.Profile.PositionProfile.CalculateTotalDisplacement( MotionPhase.ConstantDeceleration );

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
            return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
          }

          var phase = param.Profile.CalculatePhase( time );
          var phaseDuration = param.Profile.TimeProfile.CalculateDuration( time, phase );

          if (phase == MotionPhase.None)
          {
            return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.InvalidMotionPhase );
          }

          if (phase == MotionPhase.AccelerationWithPositiveJerk)
          {
            positionData.Add( _kinematics.CalculateCubicPosition( phaseDuration, param.Profile.Jerk ) );
            continue;
          }

          if (phase == MotionPhase.ConstantAcceleration)
          {
            positionData.Add(
              accelerationWithPositiveJerkDisplacement
              + _kinematics.CalculateQuadraticPosition(
                phaseDuration,
                accelerationWithPositiveJerkMaxVelocity,
                param.Profile.MaxAcceleration
              )
            );

            continue;
          }

          if (phase == MotionPhase.AccelerationWithNegativeJerk)
          {
            positionData.Add(
              constantAccelerationDisplacement
              + _kinematics.CalculateCubicPosition(
                phaseDuration,
                constantAccelerationMaxVelocity,
                param.Profile.MaxAcceleration,
                -param.Profile.Jerk
              )
            );

            continue;
          }

          if (phase == MotionPhase.ConstantVelocity)
          {
            positionData.Add(
              accelerationWithNegativeJerkDisplacement
              + _kinematics.CalculateLinearPosition( phaseDuration, param.Profile.MaxVelocity )
            );

            continue;
          }

          if (phase == MotionPhase.DecelerationWithNegativeJerk)
          {
            positionData.Add(
              constantVelocityDisplacement
              + _kinematics.CalculateCubicPosition(
                phaseDuration,
                param.Profile.MaxVelocity,
                -param.Profile.Jerk
              )
            );

            continue;
          }

          if (phase == MotionPhase.ConstantDeceleration)
          {
            positionData.Add(
              decelerationWithNegativeJerkDisplacement
              + _kinematics.CalculateQuadraticPosition(
                phaseDuration,
                decelerationWithNegativeJerkMaxVelocity,
                -param.Profile.MaxAcceleration
              )
            );

            continue;
          }

          positionData.Add(
            constantDecelerationDisplacement
            + _kinematics.CalculateCubicPosition(
              phaseDuration,
              constantDecelerationMaxVelocity,
              -param.Profile.MaxAcceleration,
              param.Profile.Jerk
            )
          );
        }

        var result = new MotionDisplacementCalculationResult( positionData );
        return Result.Success( result );
      },
      CancellationToken.None
    );
  }
}