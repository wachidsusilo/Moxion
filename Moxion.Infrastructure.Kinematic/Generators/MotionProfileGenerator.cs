using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Moxion.Application.Abstractions.Generators.Kinematic;
using Moxion.Application.Abstractions.Math;
using Moxion.Application.Extensions;
using Moxion.Application.Shared.Generators.Params;
using Moxion.Application.Shared.Generators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;
using Moxion.Domain.Kinematic;

namespace Moxion.Infrastructure.Kinematic.Generators;

[SuppressMessage( "ReSharper", "ConvertToPrimaryConstructor" )]
internal class MotionProfileGenerator : IMotionProfileGenerator
{
  private readonly IKinematics _kinematic;
  private readonly ILogger<MotionProfileGenerator> _logger;

  public MotionProfileGenerator( IKinematics kinematics, ILogger<MotionProfileGenerator> logger )
  {
    _kinematic = kinematics;
    _logger = logger;
  }

  public Task<Result<MotionProfileGenerationResult?>> Execute(
    MotionProfileGenerationParam param,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var result = ExecuteInternal( param, cancellationToken );

    _logger.LogEnd( result );

    return Task.FromResult( result );
  }

  private Result<MotionProfileGenerationResult?> ExecuteInternal(
    MotionProfileGenerationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.OperationCancelled );
    }

    if (param.Acceleration.IsZero)
    {
      // Constant Velocity
      var steadyMotionDuration = _kinematic.CalculateTime( param.Displacement, param.Velocity );

      if (!steadyMotionDuration.IsPositive)
      {
        return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
      }

      var profile = new MotionProfile(
        param.Displacement,
        param.Velocity,
        Acceleration.Zero,
        Jerk.Zero,
        Time.Zero,
        Time.Zero,
        steadyMotionDuration
      );

      return Result.Success<MotionProfileGenerationResult?>( new MotionProfileGenerationResult( profile ) );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.OperationCancelled );
    }

    if (param.Jerk.IsZero)
    {
      // Constant Acceleration
      var halfDisplacement = param.Displacement / 2;
      var timeAtMaxVelocity = param.Velocity / param.Acceleration;
      var displacementAtMaxVelocity = _kinematic.CalculatePosition( timeAtMaxVelocity, param.Acceleration );
      var accelerationDuration = _kinematic.CalculateTime( halfDisplacement, param.Acceleration );

      if (!accelerationDuration.IsPositive)
      {
        return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
      }

      if (param.Displacement < 2 * displacementAtMaxVelocity)
      {
        // Triangular
        var profile = new MotionProfile(
          param.Displacement,
          param.Velocity,
          param.Acceleration,
          Jerk.Zero,
          Time.Zero,
          accelerationDuration,
          Time.Zero
        );

        return Result.Success<MotionProfileGenerationResult?>( new MotionProfileGenerationResult( profile ) );
      }

      // Trapezoid
      var steadyMotionDisplacement = param.Displacement - 2 * displacementAtMaxVelocity;
      var steadyMotionDuration = _kinematic.CalculateTime( steadyMotionDisplacement, param.Velocity );

      if (!steadyMotionDuration.IsPositive)
      {
        return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
      }

      var motionProfile = new MotionProfile(
        param.Displacement,
        param.Velocity,
        param.Acceleration,
        Jerk.Zero,
        Time.Zero,
        timeAtMaxVelocity,
        steadyMotionDuration
      );

      return Result.Success<MotionProfileGenerationResult?>( new MotionProfileGenerationResult( motionProfile ) );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.OperationCancelled );
    }

    // Constant Jerk
    var quarterDisplacement = param.Displacement / 4;
    var timeAtMaxAcceleration = _kinematic.CalculateTime( param.Acceleration, param.Jerk );
    var displacementAtMaxAcceleration = _kinematic.CalculatePosition( timeAtMaxAcceleration, param.Jerk );

    if (!timeAtMaxAcceleration.IsPositive)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
    }

    if (param.Displacement < 4 * displacementAtMaxAcceleration)
    {
      // Target Displacement reached before Maximum Acceleration
      // Guaranteed to not have an acceleration phase
      var timeAtQuarterDisplacement = _kinematic.CalculateTime( quarterDisplacement, param.Jerk );
      var velocityAtQuarterDisplacement = _kinematic.CalculateVelocity( timeAtQuarterDisplacement, param.Jerk );

      if (!timeAtQuarterDisplacement.IsPositive)
      {
        return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
      }

      if (param.Velocity > 2 * velocityAtQuarterDisplacement)
      {
        // Jerk Only
        var profile = new MotionProfile(
          param.Displacement,
          Velocity.Zero,
          Acceleration.Zero,
          param.Jerk,
          timeAtQuarterDisplacement,
          Time.Zero,
          Time.Zero
        );

        return Result.Success<MotionProfileGenerationResult?>( new MotionProfileGenerationResult( profile ) );
      }

      // Jerk with Steady State
      var jerkDuration = _kinematic.CalculateJerkDuration( param.Velocity, param.Jerk );
      var jerkDisplacement = _kinematic.CalculatePosition( jerkDuration, param.Jerk );

      if (!jerkDuration.IsPositive)
      {
        return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
      }

      var steadyMotionDisplacement = param.Displacement - 4 * jerkDisplacement;
      var steadyMotionDuration = _kinematic.CalculateTime( steadyMotionDisplacement, param.Velocity );

      if (!steadyMotionDuration.IsPositive)
      {
        return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
      }

      var motionProfile = new MotionProfile(
        param.Displacement,
        param.Velocity,
        Acceleration.Zero,
        param.Jerk,
        jerkDuration,
        Time.Zero,
        steadyMotionDuration
      );

      return Result.Success<MotionProfileGenerationResult?>( new MotionProfileGenerationResult( motionProfile ) );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.OperationCancelled );
    }

    // Target Displacement reached after Maximum Acceleration
    // Guaranteed to have sufficient displacement to execute 4 jerk phases with maximum acceleration
    var velocityAtMaxAcceleration = _kinematic.CalculateVelocity( timeAtMaxAcceleration, param.Jerk );

    if (param.Velocity < 2 * velocityAtMaxAcceleration)
    {
      // Maximum Velocity reached before Maximum Acceleration
      // Guaranteed to not have an acceleration phase
      // Guaranteed to have a steady motion phase
      var jerkDuration = _kinematic.CalculateJerkDuration( param.Velocity, param.Jerk );
      var jerkDisplacement = _kinematic.CalculatePosition( jerkDuration, param.Jerk );

      if (!jerkDuration.IsPositive)
      {
        return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
      }

      var steadyMotionDisplacement = param.Displacement - 4 * jerkDisplacement;
      var steadyMotionDuration = _kinematic.CalculateTime( steadyMotionDisplacement, param.Velocity );

      if (!steadyMotionDuration.IsPositive)
      {
        return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
      }

      var profile = new MotionProfile(
        param.Displacement,
        param.Velocity,
        Acceleration.Zero,
        param.Jerk,
        jerkDuration,
        Time.Zero,
        steadyMotionDuration
      );

      return Result.Success<MotionProfileGenerationResult?>( new MotionProfileGenerationResult( profile ) );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.OperationCancelled );
    }

    // Maximum Velocity reached after Maximum Acceleration
    // Guaranteed to have an acceleration phase
    var velocityAtConstantAcceleration = param.Velocity - 2 * velocityAtMaxAcceleration;
    var accelerationPhaseDuration = _kinematic.CalculateTime( velocityAtConstantAcceleration, param.Acceleration );

    if (!accelerationPhaseDuration.IsPositive)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
    }

    var accelerationDisplacement =
      _kinematic.CalculatePosition( accelerationPhaseDuration, velocityAtMaxAcceleration, param.Acceleration );

    var steadyPhaseDisplacement =
      param.Displacement - ( 4 * displacementAtMaxAcceleration + 2 * accelerationDisplacement );

    var steadyPhaseDuration = _kinematic.CalculateTime( steadyPhaseDisplacement, param.Velocity );

    if (!steadyPhaseDuration.IsPositive)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.NegativeTimeResult );
    }

    var profileResult = new MotionProfile(
      param.Displacement,
      param.Velocity,
      Acceleration.Zero,
      param.Jerk,
      timeAtMaxAcceleration,
      accelerationPhaseDuration,
      steadyPhaseDuration
    );

    return Result.Success<MotionProfileGenerationResult?>( new MotionProfileGenerationResult( profileResult ) );
  }
}