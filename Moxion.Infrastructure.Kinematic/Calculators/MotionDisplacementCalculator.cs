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

  public MotionDisplacementCalculator(
    IKinematics kinematics,
    IMotionVelocityCalculator velocityCalculator,
    IMotionAccelerationCalculator accelerationCalculator
  )
  {
    _kinematics = kinematics;
    _velocityCalculator = velocityCalculator;
    _accelerationCalculator = accelerationCalculator;
  }

  public async Task<Result<MotionDisplacementCalculationResult>> Execute(
    MotionDisplacementCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.AccelerationWithPositiveJerk)
    {
      return new MotionDisplacementCalculationResult(
        _kinematics.CalculatePosition( param.PhaseDuration, param.Profile.Jerk )
      );
    }

    var jerkDisplacement = _kinematics.CalculatePosition( param.Profile.JerkDuration, param.Profile.Jerk );
    var totalDisplacement = jerkDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.ConstantAcceleration)
    {
      return new MotionDisplacementCalculationResult(
        totalDisplacement + _kinematics.CalculatePosition( param.PhaseDuration, param.Profile.Acceleration )
      );
    }

    var accelerationDisplacement =
      _kinematics.CalculatePosition( param.Profile.AccelerationDuration, param.Profile.Acceleration );

    totalDisplacement += accelerationDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.AccelerationWithNegativeJerk)
    {
      var duration = param.Profile.CalculateTotalDuration( MotionPhase.ConstantAcceleration );
      var velocityParam = new MotionVelocityCalculationParam( param.Profile, param.Phase, duration );
      var velocityResult = await _velocityCalculator.Execute( velocityParam, cancellationToken );

      if (velocityResult.HasError)
      {
        return Result.Error<MotionDisplacementCalculationResult>( velocityResult.ErrorCode );
      }

      var accelerationParam = new MotionAccelerationCalculationParam( param.Profile, param.Phase, duration );
      var accelerationResult = await _accelerationCalculator.Execute( accelerationParam, cancellationToken );

      if (accelerationResult.HasError)
      {
        return Result.Error<MotionDisplacementCalculationResult>( accelerationResult.ErrorCode );
      }

      var velocity = velocityResult.Data.Velocity;
      var acceleration = accelerationResult.Data.Acceleration;

      return new MotionDisplacementCalculationResult(
        totalDisplacement +
        _kinematics.CalculatePosition( param.PhaseDuration, velocity, acceleration, -param.Profile.Jerk )
      );
    }

    totalDisplacement += jerkDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.ConstantVelocity)
    {
      return new MotionDisplacementCalculationResult(
        totalDisplacement + _kinematics.CalculatePosition( param.PhaseDuration, param.Profile.Velocity )
      );
    }

    totalDisplacement += _kinematics.CalculatePosition( param.Profile.SteadyMotionDuration, param.Profile.Velocity );

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.DecelerationWithNegativeJerk)
    {
      var duration = param.Profile.CalculateTotalDuration( MotionPhase.ConstantVelocity );
      var velocityParam = new MotionVelocityCalculationParam( param.Profile, param.Phase, duration );
      var velocityResult = await _velocityCalculator.Execute( velocityParam, cancellationToken );

      if (velocityResult.HasError)
      {
        return Result.Error<MotionDisplacementCalculationResult>( velocityResult.ErrorCode );
      }

      var velocity = velocityResult.Data.Velocity;

      return new MotionDisplacementCalculationResult(
        totalDisplacement + _kinematics.CalculatePosition( param.PhaseDuration, velocity, -param.Profile.Jerk )
      );
    }

    totalDisplacement += jerkDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    if (param.Phase == MotionPhase.ConstantDeceleration)
    {
      var duration = param.Profile.CalculateTotalDuration( MotionPhase.DecelerationWithNegativeJerk );
      var velocityParam = new MotionVelocityCalculationParam( param.Profile, param.Phase, duration );
      var velocityResult = await _velocityCalculator.Execute( velocityParam, cancellationToken );

      if (velocityResult.HasError)
      {
        return Result.Error<MotionDisplacementCalculationResult>( velocityResult.ErrorCode );
      }

      var velocity = velocityResult.Data.Velocity;

      return new MotionDisplacementCalculationResult(
        totalDisplacement + _kinematics.CalculatePosition( param.PhaseDuration, velocity, -param.Profile.Acceleration )
      );
    }

    totalDisplacement += accelerationDisplacement;

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDisplacementCalculationResult>( ErrorCode.OperationCancelled );
    }

    var totalDuration = param.Profile.CalculateTotalDuration( MotionPhase.ConstantDeceleration );
    var initialVelocityParam = new MotionVelocityCalculationParam( param.Profile, param.Phase, totalDuration );
    var initialVelocityResult = await _velocityCalculator.Execute( initialVelocityParam, cancellationToken );

    if (initialVelocityResult.HasError)
    {
      return Result.Error<MotionDisplacementCalculationResult>( initialVelocityResult.ErrorCode );
    }

    var initialAccelerationParam = new MotionAccelerationCalculationParam( param.Profile, param.Phase, totalDuration );
    var initialAccelerationResult =
      await _accelerationCalculator.Execute( initialAccelerationParam, cancellationToken );

    if (initialAccelerationResult.HasError)
    {
      return Result.Error<MotionDisplacementCalculationResult>( initialAccelerationResult.ErrorCode );
    }

    var initialVelocity = initialVelocityResult.Data.Velocity;
    var initialAcceleration = initialAccelerationResult.Data.Acceleration;

    return new MotionDisplacementCalculationResult(
      totalDisplacement +
      _kinematics.CalculatePosition( param.PhaseDuration, initialVelocity, initialAcceleration, param.Profile.Jerk )
    );
  }
}