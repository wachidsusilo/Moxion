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
  private readonly IKinematics _kinematics;
  private readonly ILogger<MotionProfileGenerator> _logger;

  public MotionProfileGenerator( IKinematics kinematics, ILogger<MotionProfileGenerator> logger )
  {
    _kinematics = kinematics;
    _logger = logger;
  }

  public Task<Result<MotionProfileGenerationResult>> Execute(
    MotionProfileGenerationParam param,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var result = ExecuteInternal( param, cancellationToken );

    _logger.LogEnd( result );

    return Task.FromResult( result );
  }

  private Result<MotionProfileGenerationResult> ExecuteInternal(
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
      var linearMotionProfileResult = GenerateLinearMotionProfile( param );

      return Result.Create(
        linearMotionProfileResult.ErrorCode,
        new MotionProfileGenerationResult( linearMotionProfileResult.Data )
      );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.OperationCancelled );
    }

    if (param.Jerk.IsZero)
    {
      var quadraticMotionProfileResult = GenerateQuadraticMotionProfile( param );

      return Result.Create(
        quadraticMotionProfileResult.ErrorCode,
        new MotionProfileGenerationResult( quadraticMotionProfileResult.Data )
      );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileGenerationResult>( ErrorCode.OperationCancelled );
    }

    var cubicMotionProfileResult = GenerateCubicMotionProfile( param );

    return Result.Create(
      cubicMotionProfileResult.ErrorCode,
      new MotionProfileGenerationResult( cubicMotionProfileResult.Data )
    );
  }

  /// <summary>
  /// Generates a linear motion profile.
  /// </summary>
  /// <param name="param">The parameters of the motion.</param>
  /// <returns>
  /// A <see cref="Result{TData}"/> object with <see cref="MotionProfile"/> as the data.
  /// The result contains one of the following error codes:
  /// <list type="bullet">
  /// <item><see cref="ErrorCode.NoError"/> - Indicates that the operation was successful.</item>
  /// <item>
  /// <see cref="ErrorCode.NegativeTimeResult"/>
  /// - Indicates that the calculation of the specified motion parameter resulted in a negative time.
  /// </item>
  /// </list>
  /// </returns>
  /// <remarks>
  /// A linear motion only has constant velocity phase.
  /// </remarks>
  private Result<MotionProfile> GenerateLinearMotionProfile( MotionProfileGenerationParam param )
  {
    var constantVelocityDuration = _kinematics.CalculateLinearTime( param.Displacement, param.Velocity );

    if (constantVelocityDuration.IsNegative)
    {
      return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
    }

    var timeProfile = new TimeProfile(
      Time.Zero,
      Time.Zero,
      constantVelocityDuration
    );

    var displacementProfile = new PositionProfile(
      Position.Zero,
      Position.Zero,
      Position.Zero,
      param.Displacement
    );

    var velocityProfile = new VelocityProfile(
      Velocity.Zero,
      Velocity.Zero,
      Velocity.Zero
    );

    var accelerationProfile = new AccelerationProfile( Acceleration.Zero );

    var motionProfile = new MotionProfile(
      param.Displacement,
      param.Velocity,
      Acceleration.Zero,
      Jerk.Zero,
      timeProfile,
      displacementProfile,
      velocityProfile,
      accelerationProfile
    );

    return Result.Success( motionProfile );
  }

  /// <summary>
  /// Generates a quadratic motion profile.
  /// </summary>
  /// <param name="param">The parameters of the motion.</param>
  /// <returns>
  /// A <see cref="Result{TData}"/> object with <see cref="MotionProfile"/> as the data.
  /// The result contains one of the following error codes:
  /// <list type="bullet">
  /// <item><see cref="ErrorCode.NoError"/> - Indicates that the operation was successful.</item>
  /// <item>
  /// <see cref="ErrorCode.NegativeTimeResult"/>
  /// - Indicates that the calculation of the specified motion parameter resulted in a negative time.
  /// </item>
  /// </list>
  /// </returns>
  /// <remarks>
  /// A quadratic motion has 3 phases:
  /// <list type="number">
  /// <item>Constant acceleration</item>
  /// <item>Constant velocity</item>
  /// <item>Constant deceleration</item>
  /// </list>
  /// The characteristics of the motion are listed below:
  /// <list type="number">
  /// <item>The displacement for acceleration and deceleration phases are symmetrical.</item>
  /// <item>The duration for acceleration and deceleration phases are symmetrical.</item>
  /// </list>
  /// The possible motion profile for a quadratic motion is described below:
  /// <list type="number">
  /// <item>
  /// Trapezoidal Profile
  /// The displacement is sufficient to contains all 3 phases.
  /// </item>
  /// <item>
  /// Triangular Profile
  /// The half-displacement is insufficient to reach the maximum velocity.
  /// In this case, the motion will only contain constant-acceleration and
  /// constant-deceleration phases.
  /// </item>
  /// </list>
  /// The implementation of this method assume that the acceleration and the deceleration
  /// are required for the motion. Therefore, a quadratic motion will at least have 2 phases:
  /// <list type="number">
  /// <item>Constant acceleration</item>
  /// <item>Constant deceleration</item>
  /// </list>
  /// </remarks>
  private Result<MotionProfile> GenerateQuadraticMotionProfile( MotionProfileGenerationParam param )
  {
    var totalDisplacement = param.Displacement;
    var maxVelocity = param.Velocity;
    var maxAcceleration = param.Acceleration;

    var accelerationDuration = maxVelocity / maxAcceleration;
    var accelerationDisplacement =
      _kinematics.CalculateQuadraticPosition( accelerationDuration, maxAcceleration );

    if (param.Displacement < 2 * accelerationDisplacement)
    {
      // The total displacement is insufficient to reach maximum velocity.
      // We need to adjust the maximum velocity.
      accelerationDisplacement = totalDisplacement / 2;
      accelerationDuration = _kinematics.CalculateQuadraticTime( accelerationDisplacement, param.Acceleration );

      if (accelerationDuration.IsNegative)
      {
        return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
      }

      maxVelocity = _kinematics.CalculateQuadraticVelocity( accelerationDuration, param.Acceleration );
    }

    var constantVelocityDisplacement = maxVelocity < param.Velocity
      ? Position.Zero
      : param.Displacement - 2 * accelerationDisplacement;

    var constantVelocityDuration = _kinematics.CalculateLinearTime( constantVelocityDisplacement, param.Velocity );

    if (constantVelocityDuration.IsNegative)
    {
      return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
    }

    var timeProfile = new TimeProfile(
      Time.Zero,
      accelerationDuration,
      constantVelocityDuration
    );

    var displacementProfile = new PositionProfile(
      Position.Zero,
      Position.Zero,
      accelerationDisplacement,
      constantVelocityDisplacement
    );

    var velocityProfile = new VelocityProfile(
      Velocity.Zero,
      maxVelocity,
      Velocity.Zero
    );

    var accelerationProfile = new AccelerationProfile( Acceleration.Zero );

    var motionProfile = new MotionProfile(
      param.Displacement,
      maxVelocity,
      param.Acceleration,
      Jerk.Zero,
      timeProfile,
      displacementProfile,
      velocityProfile,
      accelerationProfile
    );

    return Result.Success( motionProfile );
  }

  /// <summary>
  /// Generates a cubic motion profile.
  /// </summary>
  /// <param name="param">The parameters of the motion.</param>
  /// <returns>
  /// A <see cref="Result{TData}"/> object with <see cref="MotionProfile"/> as the data.
  /// The result contains one of the following error codes:
  /// <list type="bullet">
  /// <item><see cref="ErrorCode.NoError"/> - Indicates that the operation was successful.</item>
  /// <item>
  /// <see cref="ErrorCode.NegativeTimeResult"/>
  /// - Indicates that the calculation of the specified motion parameter resulted in a negative time.
  /// </item>
  /// </list>
  /// </returns>
  /// <remarks>
  /// A cubic motion has 7 phases:
  /// <list type="number">
  /// <item>Acceleration with positive jerk</item>
  /// <item>Constant acceleration</item>
  /// <item>Acceleration with negative jerk</item>
  /// <item>Constant velocity</item>
  /// <item>Deceleration with negative jerk</item>
  /// <item>Constant deceleration</item>
  /// <item>Deceleration with positive jerk</item>
  /// </list>
  /// The characteristics of the motion are listed below:
  /// <list type="number">
  /// <item>The displacement for acceleration and deceleration phases are symmetrical.</item>
  /// <item>The displacement for positive and negative jerk phases are not symmetrical.</item>
  /// <item>The displacement for constant-acceleration and constant-deceleration phases are symmetrical.</item>
  /// <item>The duration for acceleration and deceleration phases are symmetrical.</item>
  /// <item>The duration for positive and negative jerk phases are symmetrical.</item>
  /// <item>The duration for constant-acceleration and constant-deceleration phases are symmetrical.</item>
  /// </list>
  /// The possible motion profile for a cubic motion is described below:
  /// <list type="number">
  /// <item>
  /// Full Trapezoidal S-Curve.
  /// The displacement is sufficient to contains all 7 phases.
  /// </item>
  /// <item>
  /// Partial Trapezoidal S-Curve.
  /// The maximum velocity is reached before maximum acceleration.
  /// In this case, the motion does not have constant-acceleration and constant-deceleration phases.
  /// </item>
  /// <item>
  /// Full Triangular S-Curve.
  /// The half-displacement is just enough to reach the maximum velocity.
  /// In this case, the motion does not have constant-velocity phase.
  /// </item>
  /// <item>
  /// Partial Triangular S-Curve.
  /// The quarter-duration is not sufficient to reach the maximum acceleration.
  /// In this case, the motion does not have constant-acceleration, constant-velocity
  /// and constant-deceleration phases.
  /// </item>
  /// </list>
  /// The implementation of this method assume that the positive and negative jerks
  /// are required for the motion. Therefore, a cubic motion will at least have 4 phases:
  /// <list type="number">
  /// <item>Acceleration with positive jerk</item>
  /// <item>Acceleration with negative jerk</item>
  /// <item>Deceleration with negative jerk</item>
  /// <item>Deceleration with positive jerk</item>
  /// </list>
  /// </remarks>
  private Result<MotionProfile> GenerateCubicMotionProfile( MotionProfileGenerationParam param )
  {
    var totalDisplacement = param.Displacement;
    var maxAcceleration = param.Acceleration;
    var jerk = param.Jerk;

    var jerkDuration = _kinematics.CalculateCubicTime( maxAcceleration, jerk );

    if (jerkDuration.IsNegative)
    {
      return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
    }

    var positiveJerkVelocity = _kinematics.CalculateCubicVelocity( jerkDuration, jerk );
    var positiveJerkDisplacement = _kinematics.CalculateCubicPosition( jerkDuration, jerk );
    var negativeJerkDisplacement =
      _kinematics.CalculateCubicPosition( jerkDuration, positiveJerkVelocity, maxAcceleration, jerk );

    var maxVelocity = param.Velocity > 2 * positiveJerkVelocity
      ? param.Velocity
      : 2 * positiveJerkVelocity;

    if (totalDisplacement <= 2 * ( positiveJerkDisplacement + negativeJerkDisplacement ))
    {
      // The total displacement is insufficient to perform previously calculated jerk phases.
      // We need to adjust the parameters accordingly.
      jerkDuration = _kinematics.CalculateCubicJerkDuration( totalDisplacement, jerk );

      if (jerkDuration.IsNegative)
      {
        return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
      }

      maxAcceleration = _kinematics.CalculateCubicAcceleration( jerkDuration, jerk );
      positiveJerkVelocity = _kinematics.CalculateCubicVelocity( jerkDuration, jerk );
      positiveJerkDisplacement = _kinematics.CalculateCubicPosition( jerkDuration, jerk );

      negativeJerkDisplacement = _kinematics.CalculateCubicPosition(
        jerkDuration,
        positiveJerkVelocity,
        maxAcceleration,
        -jerk
      );

      maxVelocity = param.Velocity > 2 * positiveJerkVelocity
        ? param.Velocity
        : 2 * positiveJerkVelocity;
    }

    if (param.Velocity <= maxVelocity)
    {
      // The maximum velocity is achieved before the end of negative jerk phase.
      // We need to adjust the parameters accordingly.
      jerkDuration = _kinematics.CalculateCubicJerkDuration( param.Velocity, jerk );

      if (jerkDuration.IsNegative)
      {
        return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
      }

      maxAcceleration = _kinematics.CalculateCubicAcceleration( jerkDuration, jerk );
      positiveJerkVelocity = _kinematics.CalculateCubicVelocity( jerkDuration, jerk );
      positiveJerkDisplacement = _kinematics.CalculateCubicPosition( jerkDuration, jerk );

      negativeJerkDisplacement = _kinematics.CalculateCubicPosition(
        jerkDuration,
        positiveJerkVelocity,
        maxAcceleration,
        -jerk
      );

      maxVelocity = param.Velocity;
    }

    var constantAccelerationDuration = param.Velocity > maxVelocity
      ? Time.Zero
      : _kinematics.CalculateQuadraticTime( maxVelocity - 2 * positiveJerkVelocity, maxAcceleration );

    if (constantAccelerationDuration.IsNegative)
    {
      return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
    }

    var constantAccelerationVelocity = positiveJerkVelocity + _kinematics.CalculateQuadraticVelocity(
      constantAccelerationDuration,
      maxAcceleration
    );

    if (!constantAccelerationDuration.IsZero)
    {
      // If the motion has constant acceleration phase,
      // we need to recalculate the negative jerk displacement.
      negativeJerkDisplacement = _kinematics.CalculateCubicPosition(
        jerkDuration,
        constantAccelerationVelocity,
        maxAcceleration,
        -jerk
      );
    }

    var constantAccelerationDisplacement = _kinematics.CalculateQuadraticPosition(
      constantAccelerationDuration,
      positiveJerkVelocity,
      maxAcceleration
    );

    var constantVelocityDisplacement = maxVelocity < param.Velocity
      ? Position.Zero
      : totalDisplacement
        - 2 * ( positiveJerkDisplacement + negativeJerkDisplacement )
        - 2 * constantAccelerationDisplacement;

    if (!constantAccelerationDisplacement.IsZero && constantVelocityDisplacement.IsNegative)
    {
      // if the motion has constant acceleration phase but the constant velocity displacement is negative,
      // it means that the total displacement is sufficient to perform full acceleration phase
      // but the magnitude of constant acceleration is not large enough to ramp up the velocity
      // so that the maximum velocity is achieved before half of the total displacement.
      // In this case, the motion cannot achieve maximum velocity, so we need to adjust
      // the parameters accordingly.
      //
      // NOTE: Calculating constant acceleration duration is a bit tricky since it caused circular
      //       dependency. The calculation of constant acceleration duration is depended on the negative
      //       jerk displacement, while the calculation of negative jerk displacement is depended on
      //       the velocity at the end of constant acceleration phase.
      //
      // Currently, we have the following variables:
      // - s  : the half displacement of the motion
      // - s₁ : the displacement of positive jerk phase (known)
      // - s₂ : the displacement of constant acceleration phase (unknown)
      // - s₃ : the displacement of negative jerk phase (unknown)
      // - t₁ : the duration of positive jerk phase (known)
      // - t₂ : the duration of constant acceleration phase (unknown)
      // - t₃ : the duration of negative jerk phase (known, is equal to t₁)
      // 
      // The sum of the displacement of the mentioned phases should be equal to the half displacement
      // of the motion, we can say that:
      //   s = s₁ + s₂ + s₃
      // 
      // The displacement of constant acceleration phase is given by:
      //   s₂ = v₁t₂ + ½a₁(t₂)²
      // where, v₁ is the velocity at the end of positive jerk phase
      //        a₁ is the acceleration at the end of positive jerk phase (max acceleration)
      //        t₂ is the duration of constant acceleration phase
      //
      // The velocity at the end of constant acceleration phase is given by:
      //   v₂ = v₁ + a₁t₂
      // where, v₂ is the velocity at the end of constant acceleration phase 
      //
      // The displacement of negative jerk phase is given by:
      //   s₃ = v₂t₃ + ½a₁(t₃)² - ⅙j(t₃)³
      //
      // By substituting v₂ into s₃, we got:
      //   s₃ = (v₁ + a₁t₂)t₃ + ½a₁(t₃)² - ⅙j(t₃)³
      //   s₃ = v₁t₃ + a₁t₂t₃ + ½a₁(t₃)² - ⅙j(t₃)³
      //
      // Then we can rewrite the equation of the half displacement as of the following:
      //   s = s₁ + s₂ + s₃
      //   s = s₁ + (v₁t₂ + ½a₁(t₂)²) + (v₁t₃ + a₁t₂t₃ + ½a₁(t₃)² - ⅙j(t₃)³)
      //   s = s₁ + v₁t₂ + ½a₁(t₂)² + v₁t₃ + a₁t₂t₃ + ½a₁(t₃)² - ⅙j(t₃)³
      //
      // Since all variables except t₂ is known, we got quadratic equation in the function of t₂:
      //   ½a₁(t₂)² + (v₁ + a₁t₃)t₂ + (s₁ + v₁t₃ + ½a₁(t₃)² - ⅙j(t₃)³ - s) = 0
      //
      // The discriminant, d, is given by:
      //   d = b² - 4ac
      // where, a = ½a₁
      //        b = v₁ + a₁t₃
      //        c = s₁ + v₁t₃ + ½a₁(t₃)² - ⅙j(t₃)³ - s
      //
      // The root of the quadratic formula is given by:
      //   t₂ = (-b ± √d) / 2a
      //
      // Because time cannot be negative, we only care about the positive result:
      //   t₂ = (-b + √d) / 2a
      //
      var halfDisplacement = totalDisplacement / 2;

      var a = maxAcceleration / 2;
      var b = positiveJerkVelocity + maxAcceleration * jerkDuration;
      var c = positiveJerkDisplacement
              + positiveJerkVelocity * jerkDuration
              + maxAcceleration * jerkDuration * jerkDuration / 2
              - jerk * jerkDuration * jerkDuration * jerkDuration / 6
              - halfDisplacement;

      var d = ( b * b ) - ( 4 * a * c );

      constantAccelerationDuration = ( -b + d.SquareRoot() ) / ( 2 * a );

      if (constantAccelerationDuration.IsNegative)
      {
        return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
      }

      constantAccelerationDisplacement = _kinematics.CalculateQuadraticPosition(
        constantAccelerationDuration,
        positiveJerkVelocity,
        maxAcceleration
      );

      maxVelocity = 2 * positiveJerkVelocity + _kinematics.CalculateQuadraticVelocity(
        constantAccelerationDuration,
        maxAcceleration
      );

      constantAccelerationVelocity = positiveJerkVelocity + _kinematics.CalculateQuadraticVelocity(
        constantAccelerationDuration,
        maxAcceleration
      );

      negativeJerkDisplacement = _kinematics.CalculateCubicPosition(
        jerkDuration,
        constantAccelerationVelocity,
        maxAcceleration,
        -jerk
      );

      constantVelocityDisplacement = Position.Zero;
    }

    var constantVelocityDuration = _kinematics.CalculateLinearTime( constantVelocityDisplacement, maxVelocity );

    if (constantVelocityDuration.IsNegative)
    {
      return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
    }

    var timeProfile = new TimeProfile(
      jerkDuration,
      constantAccelerationDuration,
      constantVelocityDuration
    );

    var displacementProfile = new PositionProfile(
      positiveJerkDisplacement,
      negativeJerkDisplacement,
      constantAccelerationDisplacement,
      constantVelocityDisplacement
    );

    var velocityProfile = new VelocityProfile(
      positiveJerkVelocity,
      constantAccelerationVelocity,
      maxVelocity
    );

    var accelerationProfile = new AccelerationProfile( maxAcceleration );

    var motionProfile = new MotionProfile(
      totalDisplacement,
      maxVelocity,
      maxAcceleration,
      jerk,
      timeProfile,
      displacementProfile,
      velocityProfile,
      accelerationProfile
    );

    return Result.Success( motionProfile );
  }
}