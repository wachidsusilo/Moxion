using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Abstractions.Math;
using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Calculators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Common.Values.Derived;

namespace Moxion.Infrastructure.Kinematic.Calculators;

internal class MotionAccelerationCalculator : IMotionAccelerationCalculator
{
  private readonly IKinematics _kinematics;

  public MotionAccelerationCalculator( IKinematics kinematics )
  {
    _kinematics = kinematics;
  }

  public Task<Result<MotionAccelerationCalculationResult>> Execute(
    MotionAccelerationCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Phase == MotionPhase.AccelerationWithPositiveJerk)
    {
      return Task.FromResult(
        Result.Success(
          new MotionAccelerationCalculationResult(
            _kinematics.CalculateAcceleration( param.PhaseDuration, param.Profile.Jerk )
          )
        )
      );
    }

    var lastPhaseAcceleration = _kinematics.CalculateAcceleration( param.Profile.JerkDuration, param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Phase == MotionPhase.ConstantAcceleration)
    {
      return Task.FromResult(
        Result.Success(
          new MotionAccelerationCalculationResult( param.Profile.Acceleration )
        )
      );
    }

    lastPhaseAcceleration = param.Profile.AccelerationDuration.IsPositive
      ? param.Profile.Acceleration
      : lastPhaseAcceleration;

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Phase == MotionPhase.AccelerationWithNegativeJerk)
    {
      return Task.FromResult(
        Result.Success(
          new MotionAccelerationCalculationResult(
            lastPhaseAcceleration + _kinematics.CalculateAcceleration( param.PhaseDuration, -param.Profile.Jerk )
          )
        )
      );
    }

    lastPhaseAcceleration += _kinematics.CalculateAcceleration( param.Profile.JerkDuration, -param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Phase == MotionPhase.ConstantVelocity)
    {
      return Task.FromResult(
        Result.Success(
          new MotionAccelerationCalculationResult( Acceleration.Zero )
        )
      );
    }

    lastPhaseAcceleration = param.Profile.SteadyMotionDuration.IsPositive
      ? Acceleration.Zero
      : lastPhaseAcceleration;

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Phase == MotionPhase.DecelerationWithNegativeJerk)
    {
      return Task.FromResult(
        Result.Success(
          new MotionAccelerationCalculationResult(
            lastPhaseAcceleration + _kinematics.CalculateAcceleration( param.PhaseDuration, -param.Profile.Jerk )
          )
        )
      );
    }

    lastPhaseAcceleration += _kinematics.CalculateAcceleration( param.Profile.JerkDuration, -param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Phase == MotionPhase.ConstantDeceleration)
    {
      return Task.FromResult(
        Result.Success(
          new MotionAccelerationCalculationResult( -param.Profile.Acceleration )
        )
      );
    }

    lastPhaseAcceleration = param.Profile.AccelerationDuration.IsPositive
      ? -param.Profile.Acceleration
      : lastPhaseAcceleration;

    return Task.FromResult(
      Result.Success(
        new MotionAccelerationCalculationResult(
          lastPhaseAcceleration + _kinematics.CalculateAcceleration( param.PhaseDuration, param.Profile.Jerk )
        )
      )
    );
  }
}