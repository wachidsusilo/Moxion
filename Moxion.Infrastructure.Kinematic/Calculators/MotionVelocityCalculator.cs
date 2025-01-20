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

  public MotionVelocityCalculator(
    IKinematics kinematics,
    IMotionAccelerationCalculator accelerationCalculator
  )
  {
    _kinematics = kinematics;
    _accelerationCalculator = accelerationCalculator;
  }

  public async Task<Result<MotionVelocityCalculationResult?>> Execute(
    MotionVelocityCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult?>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.AccelerationWithPositiveJerk)
    {
      return new MotionVelocityCalculationResult(
        _kinematics.CalculateVelocity( param.PhaseDuration, param.Profile.Jerk )
      );
    }

    var lastPhaseVelocity = _kinematics.CalculateVelocity( param.Profile.JerkDuration, param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult?>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.ConstantAcceleration)
    {
      return new MotionVelocityCalculationResult(
        lastPhaseVelocity + _kinematics.CalculateVelocity( param.PhaseDuration, param.Profile.Acceleration )
      );
    }

    lastPhaseVelocity += _kinematics.CalculateVelocity( param.Profile.AccelerationDuration, param.Profile.Acceleration );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult?>( ErrorCode.OperationCancelled );
    }

    var accelerationParam = new MotionAccelerationCalculationParam(
      param.Profile,
      param.Phase,
      param.Profile.CalculateTotalDuration( MotionPhase.ConstantAcceleration )
    );

    var accelerationResult = await _accelerationCalculator.Execute( accelerationParam, cancellationToken );

    if (accelerationResult.HasError)
    {
      return Result.Error<MotionVelocityCalculationResult?>( accelerationResult.ErrorCode );
    }

    if (accelerationResult.Data is null)
    {
      return Result.Error<MotionVelocityCalculationResult?>( ErrorCode.UnexpectedNullData );
    }

    var acceleration = accelerationResult.Data.Acceleration;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult?>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.AccelerationWithNegativeJerk)
    {
      return new MotionVelocityCalculationResult(
        lastPhaseVelocity + _kinematics.CalculateVelocity( param.PhaseDuration, acceleration, -param.Profile.Jerk )
      );
    }

    lastPhaseVelocity += _kinematics.CalculateVelocity( param.Profile.JerkDuration, acceleration, -param.Profile.Jerk );

    if (param.Phase == MotionPhase.ConstantVelocity)
    {
      return new MotionVelocityCalculationResult( param.Profile.Velocity );
    }

    lastPhaseVelocity = param.Profile.SteadyMotionDuration.IsPositive
      ? param.Profile.Velocity
      : lastPhaseVelocity;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult?>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.DecelerationWithNegativeJerk)
    {
      return new MotionVelocityCalculationResult(
        lastPhaseVelocity + _kinematics.CalculateVelocity( param.PhaseDuration, -param.Profile.Jerk )
      );
    }

    lastPhaseVelocity += _kinematics.CalculateVelocity( param.Profile.JerkDuration, -param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult?>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.ConstantDeceleration)
    {
      return new MotionVelocityCalculationResult(
        lastPhaseVelocity + _kinematics.CalculateVelocity( param.PhaseDuration, -param.Profile.Acceleration )
      );
    }

    lastPhaseVelocity +=
      _kinematics.CalculateVelocity( param.Profile.AccelerationDuration, -param.Profile.Acceleration );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionVelocityCalculationResult?>( ErrorCode.OperationCancelled );
    }

    var decelerationParam = new MotionAccelerationCalculationParam(
      param.Profile,
      param.Phase,
      param.Profile.CalculateTotalDuration( MotionPhase.ConstantDeceleration )
    );

    var decelerationResult = await _accelerationCalculator.Execute( decelerationParam, cancellationToken );

    if (decelerationResult.HasError)
    {
      return Result.Error<MotionVelocityCalculationResult?>( decelerationResult.ErrorCode );
    }

    if (decelerationResult.Data is null)
    {
      return Result.Error<MotionVelocityCalculationResult?>( ErrorCode.UnexpectedNullData );
    }

    var deceleration = decelerationResult.Data.Acceleration;

    return new MotionVelocityCalculationResult(
      lastPhaseVelocity + _kinematics.CalculateVelocity( param.PhaseDuration, deceleration, param.Profile.Jerk )
    );
  }
}