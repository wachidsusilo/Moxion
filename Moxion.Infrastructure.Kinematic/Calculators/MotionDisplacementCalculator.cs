using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Abstractions.Math;
using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Calculators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Infrastructure.Kinematic.Extensions;

namespace Moxion.Infrastructure.Kinematic.Calculators;

internal class MotionDisplacementCalculator : IMotionDisplacementCalculator
{
  private readonly IKinematics _kinematics;
  private readonly IMotionVelocityCalculator _velocityCalculator;
  private readonly IMotionAccelerationCalculator _accelerationCalculator;
  private readonly IMotionPhaseCalculator _motionPhaseCalculator;

  public MotionDisplacementCalculator(
    IKinematics kinematics,
    IMotionVelocityCalculator velocityCalculator,
    IMotionAccelerationCalculator accelerationCalculator,
    IMotionPhaseCalculator motionPhaseCalculator
  )
  {
    _kinematics = kinematics;
    _velocityCalculator = velocityCalculator;
    _accelerationCalculator = accelerationCalculator;
    _motionPhaseCalculator = motionPhaseCalculator;
  }

  public async Task<Result<MotionDisplacementCalculationResult>> Execute(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    var phaseResult = await _motionPhaseCalculator.Execute( param, cancellationToken );

    if (phaseResult.HasError)
    {
      return Result.Error<MotionDisplacementCalculationResult>( phaseResult.ErrorCode );
    }

    var phase = phaseResult.Data.MotionPhase;
    var phaseDuration = param.Profile.CalculateDuration( param.Time, phase );

    if (phase == MotionPhase.AccelerationWithPositiveJerk)
    {
      return new MotionDisplacementCalculationResult(
        _kinematics.CalculatePosition( phaseDuration, param.Profile.Jerk )
      );
    }

    var jerkDisplacement = _kinematics.CalculatePosition( param.Profile.JerkDuration, param.Profile.Jerk );
    var totalDisplacement = jerkDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.ConstantAcceleration)
    {
      return new MotionDisplacementCalculationResult(
        totalDisplacement + _kinematics.CalculatePosition( phaseDuration, param.Profile.Acceleration )
      );
    }

    var accelerationDisplacement =
      _kinematics.CalculatePosition( param.Profile.AccelerationDuration, param.Profile.Acceleration );

    totalDisplacement += accelerationDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.AccelerationWithNegativeJerk)
    {
      var duration = param.Profile.CalculateTotalDuration( MotionPhase.ConstantAcceleration );
      var motionParam = param with { Time = duration };
      var velocityResult = await _velocityCalculator.Execute( motionParam, cancellationToken );

      if (velocityResult.HasError)
      {
        return Result.Error<MotionDisplacementCalculationResult>( velocityResult.ErrorCode );
      }

      var accelerationResult = await _accelerationCalculator.Execute( motionParam, cancellationToken );

      if (accelerationResult.HasError)
      {
        return Result.Error<MotionDisplacementCalculationResult>( accelerationResult.ErrorCode );
      }

      var velocity = velocityResult.Data.Velocity;
      var acceleration = accelerationResult.Data.Acceleration;

      return new MotionDisplacementCalculationResult(
        totalDisplacement +
        _kinematics.CalculatePosition( phaseDuration, velocity, acceleration, -param.Profile.Jerk )
      );
    }

    totalDisplacement += jerkDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.ConstantVelocity)
    {
      return new MotionDisplacementCalculationResult(
        totalDisplacement + _kinematics.CalculatePosition( phaseDuration, param.Profile.Velocity )
      );
    }

    totalDisplacement += _kinematics.CalculatePosition( param.Profile.SteadyMotionDuration, param.Profile.Velocity );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.DecelerationWithNegativeJerk)
    {
      var duration = param.Profile.CalculateTotalDuration( MotionPhase.ConstantVelocity );
      var motionParam = param with { Time = duration };
      var velocityResult = await _velocityCalculator.Execute( motionParam, cancellationToken );

      if (velocityResult.HasError)
      {
        return Result.Error<MotionDisplacementCalculationResult>( velocityResult.ErrorCode );
      }

      var velocity = velocityResult.Data.Velocity;

      return new MotionDisplacementCalculationResult(
        totalDisplacement + _kinematics.CalculatePosition( phaseDuration, velocity, -param.Profile.Jerk )
      );
    }

    totalDisplacement += jerkDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (phase == MotionPhase.ConstantDeceleration)
    {
      var duration = param.Profile.CalculateTotalDuration( MotionPhase.DecelerationWithNegativeJerk );
      var velocityParam = param with { Time = duration };
      var velocityResult = await _velocityCalculator.Execute( velocityParam, cancellationToken );

      if (velocityResult.HasError)
      {
        return Result.Error<MotionDisplacementCalculationResult>( velocityResult.ErrorCode );
      }

      var velocity = velocityResult.Data.Velocity;

      return new MotionDisplacementCalculationResult(
        totalDisplacement + _kinematics.CalculatePosition( phaseDuration, velocity, -param.Profile.Acceleration )
      );
    }

    totalDisplacement += accelerationDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    var totalDuration = param.Profile.CalculateTotalDuration( MotionPhase.ConstantDeceleration );
    var motionCalculationParam = param with { Time = totalDuration };
    var initialVelocityResult = await _velocityCalculator.Execute( motionCalculationParam, cancellationToken );

    if (initialVelocityResult.HasError)
    {
      return Result.Error<MotionDisplacementCalculationResult>( initialVelocityResult.ErrorCode );
    }

    var initialAccelerationResult = await _accelerationCalculator.Execute( motionCalculationParam, cancellationToken );

    if (initialAccelerationResult.HasError)
    {
      return Result.Error<MotionDisplacementCalculationResult>( initialAccelerationResult.ErrorCode );
    }

    var initialVelocity = initialVelocityResult.Data.Velocity;
    var initialAcceleration = initialAccelerationResult.Data.Acceleration;

    return new MotionDisplacementCalculationResult(
      totalDisplacement +
      _kinematics.CalculatePosition( phaseDuration, initialVelocity, initialAcceleration, param.Profile.Jerk )
    );
  }
}