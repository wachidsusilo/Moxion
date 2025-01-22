using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Abstractions.Math;
using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Calculators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Infrastructure.Kinematic.Extensions;

namespace Moxion.Infrastructure.Kinematic.Calculators;

internal class MotionVelocityCalculator : IMotionVelocityCalculator
{
  private readonly IKinematics _kinematics;
  private readonly IMotionAccelerationCalculator _accelerationCalculator;
  private readonly IMotionPhaseCalculator _motionPhaseCalculator;

  public MotionVelocityCalculator(
    IKinematics kinematics,
    IMotionAccelerationCalculator accelerationCalculator,
    IMotionPhaseCalculator motionPhaseCalculator
  )
  {
    _kinematics = kinematics;
    _accelerationCalculator = accelerationCalculator;
    _motionPhaseCalculator = motionPhaseCalculator;
  }

  public async Task<Result<MotionVelocityCalculationResult>> Execute(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult>( ErrorCode.OperationCancelled );
    }

    var phaseResult = await _motionPhaseCalculator.Execute( param, cancellationToken );

    if (phaseResult.HasError)
    {
      return Result.Error<MotionVelocityCalculationResult>( phaseResult.ErrorCode );
    }

    var phase = phaseResult.Data.MotionPhase;
    var phaseDuration = param.Profile.CalculateDuration( param.Time, phase );

    if (phase == MotionPhase.AccelerationWithPositiveJerk)
    {
      return new MotionVelocityCalculationResult(
        _kinematics.CalculateVelocity( phaseDuration, param.Profile.Jerk )
      );
    }

    var lastPhaseVelocity = _kinematics.CalculateVelocity( param.Profile.JerkDuration, param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.ConstantAcceleration)
    {
      return new MotionVelocityCalculationResult(
        lastPhaseVelocity + _kinematics.CalculateVelocity( phaseDuration, param.Profile.Acceleration )
      );
    }

    lastPhaseVelocity +=
      _kinematics.CalculateVelocity( param.Profile.AccelerationDuration, param.Profile.Acceleration );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult>( ErrorCode.OperationCancelled );
    }

    var accelerationParam = param with
    {
      Time = param.Profile.CalculateTotalDuration( MotionPhase.ConstantAcceleration )
    };

    var accelerationResult = await _accelerationCalculator.Execute( accelerationParam, cancellationToken );

    if (accelerationResult.HasError)
    {
      return Result.Error<MotionVelocityCalculationResult>( accelerationResult.ErrorCode );
    }

    var acceleration = accelerationResult.Data.Acceleration;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.AccelerationWithNegativeJerk)
    {
      return new MotionVelocityCalculationResult(
        lastPhaseVelocity + _kinematics.CalculateVelocity( phaseDuration, acceleration, -param.Profile.Jerk )
      );
    }

    lastPhaseVelocity += _kinematics.CalculateVelocity( param.Profile.JerkDuration, acceleration, -param.Profile.Jerk );

    if (phase == MotionPhase.ConstantVelocity)
    {
      return new MotionVelocityCalculationResult( param.Profile.Velocity );
    }

    lastPhaseVelocity = !param.Profile.SteadyMotionDuration.IsZero
      ? param.Profile.Velocity
      : lastPhaseVelocity;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.DecelerationWithNegativeJerk)
    {
      return new MotionVelocityCalculationResult(
        lastPhaseVelocity + _kinematics.CalculateVelocity( phaseDuration, -param.Profile.Jerk )
      );
    }

    lastPhaseVelocity += _kinematics.CalculateVelocity( param.Profile.JerkDuration, -param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.ConstantDeceleration)
    {
      return new MotionVelocityCalculationResult(
        lastPhaseVelocity + _kinematics.CalculateVelocity( phaseDuration, -param.Profile.Acceleration )
      );
    }

    lastPhaseVelocity +=
      _kinematics.CalculateVelocity( param.Profile.AccelerationDuration, -param.Profile.Acceleration );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult>( ErrorCode.OperationCancelled );
    }

    var decelerationParam = param with
    {
      Time = param.Profile.CalculateTotalDuration( MotionPhase.ConstantDeceleration )
    };

    var decelerationResult = await _accelerationCalculator.Execute( decelerationParam, cancellationToken );

    if (decelerationResult.HasError)
    {
      return Result.Error<MotionVelocityCalculationResult>( decelerationResult.ErrorCode );
    }

    var deceleration = decelerationResult.Data.Acceleration;

    return new MotionVelocityCalculationResult(
      lastPhaseVelocity + _kinematics.CalculateVelocity( phaseDuration, deceleration, param.Profile.Jerk )
    );
  }
}