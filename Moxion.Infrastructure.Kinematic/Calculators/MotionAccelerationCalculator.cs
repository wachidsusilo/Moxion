using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Abstractions.Math;
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
  private readonly IMotionPhaseCalculator _motionPhaseCalculator;

  public MotionAccelerationCalculator( IKinematics kinematics, IMotionPhaseCalculator motionPhaseCalculator )
  {
    _kinematics = kinematics;
    _motionPhaseCalculator = motionPhaseCalculator;
  }

  public async Task<Result<MotionAccelerationCalculationResult>> Execute(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled );
    }

    var phaseResult = await _motionPhaseCalculator.Execute( param, cancellationToken );

    if (phaseResult.HasError)
    {
      return Result.Error<MotionAccelerationCalculationResult>( phaseResult.ErrorCode );
    }

    var phase = phaseResult.Data.MotionPhase;
    var phaseDuration = param.Profile.CalculateDuration( param.Time, phase );

    if (phase == MotionPhase.AccelerationWithPositiveJerk)
    {
      return Result.Success(
        new MotionAccelerationCalculationResult(
          _kinematics.CalculateAcceleration( phaseDuration, param.Profile.Jerk )
        )
      );
    }

    var lastPhaseAcceleration = _kinematics.CalculateAcceleration( param.Profile.JerkDuration, param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.ConstantAcceleration)
    {
      return Result.Success(
        new MotionAccelerationCalculationResult( param.Profile.Acceleration )
      );
    }

    lastPhaseAcceleration = !param.Profile.AccelerationDuration.IsZero
      ? param.Profile.Acceleration
      : lastPhaseAcceleration;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.AccelerationWithNegativeJerk)
    {
      return Result.Success(
        new MotionAccelerationCalculationResult(
          lastPhaseAcceleration + _kinematics.CalculateAcceleration( phaseDuration, -param.Profile.Jerk )
        )
      );
    }

    lastPhaseAcceleration += _kinematics.CalculateAcceleration( param.Profile.JerkDuration, -param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.ConstantVelocity)
    {
      return Result.Success(
        new MotionAccelerationCalculationResult( Acceleration.Zero )
      );
    }

    lastPhaseAcceleration = !param.Profile.SteadyMotionDuration.IsZero
      ? Acceleration.Zero
      : lastPhaseAcceleration;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.DecelerationWithNegativeJerk)
    {
      return Result.Success(
        new MotionAccelerationCalculationResult(
          lastPhaseAcceleration + _kinematics.CalculateAcceleration( phaseDuration, -param.Profile.Jerk )
        )
      );
    }

    lastPhaseAcceleration += _kinematics.CalculateAcceleration( param.Profile.JerkDuration, -param.Profile.Jerk );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionAccelerationCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.ConstantDeceleration)
    {
      return Result.Success(
        new MotionAccelerationCalculationResult( -param.Profile.Acceleration )
      );
    }

    lastPhaseAcceleration = !param.Profile.AccelerationDuration.IsZero
      ? -param.Profile.Acceleration
      : lastPhaseAcceleration;

    return Result.Success(
      new MotionAccelerationCalculationResult(
        lastPhaseAcceleration + _kinematics.CalculateAcceleration( phaseDuration, param.Profile.Jerk )
      )
    );
  }
}