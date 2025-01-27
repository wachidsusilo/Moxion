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
    var jerk = param.Jerk;

    // Assumes that the motion does not have constant acceleration and constant velocity phases
    var positiveJerkMaxAcceleration = param.Acceleration;
    var jerkDuration = _kinematics.CalculateCubicTime( positiveJerkMaxAcceleration, jerk );
    var constantAccelerationDuration = Time.Zero;
    var constantVelocityDuration = Time.Zero;

    if (jerkDuration.IsNegative)
    {
      return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
    }

    var positiveJerkMaxVelocity = _kinematics.CalculateCubicVelocity( jerkDuration, jerk );
    var constantAccelerationMaxVelocity = positiveJerkMaxVelocity;
    var negativeJerkMaxVelocity = 2 * positiveJerkMaxVelocity;

    if (negativeJerkMaxVelocity < param.Velocity)
    {
      // If the velocity at the end of negative jerk phase is smaller than the requested velocity,
      // it means that the motion should have constant acceleration phase.
      // In this case, we need to recalculate the velocity at the end of constant acceleration phase
      // and the velocity at the end of negative jerk phase.
      var constantAccelerationVelocity = param.Velocity - negativeJerkMaxVelocity;

      constantAccelerationDuration = _kinematics.CalculateQuadraticTime(
        constantAccelerationVelocity,
        positiveJerkMaxAcceleration
      );

      if (constantAccelerationDuration.IsNegative)
      {
        return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
      }

      constantAccelerationMaxVelocity = positiveJerkMaxVelocity
                                        + _kinematics.CalculateQuadraticVelocity(
                                          constantAccelerationDuration,
                                          positiveJerkMaxAcceleration
                                        );

      negativeJerkMaxVelocity = constantAccelerationMaxVelocity + positiveJerkMaxVelocity;
    }
    else if (negativeJerkMaxVelocity > param.Velocity)
    {
      // On the other hand, if the velocity at the end of negative jerk phase is larger than the requested
      // velocity, it means that the motion with the given parameters cannot achieve maximum acceleration.
      // In this case, we need to recalculate the velocity contributed by the positive and negative jerk phases.
      positiveJerkMaxVelocity = param.Velocity / 2;
      constantAccelerationMaxVelocity = positiveJerkMaxVelocity;
      negativeJerkMaxVelocity = 2 * positiveJerkMaxVelocity;

      jerkDuration = _kinematics.CalculateCubicTime( positiveJerkMaxVelocity, jerk );

      if (jerkDuration.IsNegative)
      {
        return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
      }

      positiveJerkMaxAcceleration = _kinematics.CalculateCubicAcceleration( jerkDuration, jerk );
    }

    var positiveJerkDisplacement = _kinematics.CalculateCubicPosition( jerkDuration, jerk );
    var constantAccelerationDisplacement = _kinematics.CalculateQuadraticPosition(
      constantAccelerationDuration,
      positiveJerkMaxVelocity,
      positiveJerkMaxAcceleration
    );

    var constantVelocityDisplacement = Position.Zero;
    var negativeJerkDisplacement = _kinematics.CalculateCubicPosition(
      jerkDuration,
      constantAccelerationMaxVelocity,
      positiveJerkMaxAcceleration,
      -jerk
    );

    var calculatedTotalDisplacement = constantVelocityDisplacement
                                      + 2 * ( positiveJerkDisplacement + negativeJerkDisplacement )
                                      + 2 * constantAccelerationDisplacement;

    if (totalDisplacement < calculatedTotalDisplacement)
    {
      // If the requested total displacement is smaller than the calculated total displacement,
      // it means that the motion with the given parameters cannot achieve maximum velocity.
      // In this case, we need to collapse the constant velocity phase and squeeze down
      // the acceleration and deceleration phases.

      // First, try stripping the constant acceleration phase off
      constantAccelerationMaxVelocity = positiveJerkMaxVelocity;
      negativeJerkMaxVelocity = 2 * positiveJerkMaxVelocity;

      constantAccelerationDuration = Time.Zero;
      constantAccelerationDisplacement = Position.Zero;
      negativeJerkDisplacement = _kinematics.CalculateCubicPosition(
        jerkDuration,
        constantAccelerationMaxVelocity,
        positiveJerkMaxAcceleration,
        -jerk
      );

      calculatedTotalDisplacement = constantVelocityDisplacement
                                    + 2 * ( positiveJerkDisplacement + negativeJerkDisplacement );

      if (totalDisplacement < calculatedTotalDisplacement)
      {
        // If removing constant acceleration phase is still insufficient,
        // we need to squeeze down the positive and negative jerk phases.
        jerkDuration = _kinematics.CalculateCubicJerkDuration( totalDisplacement, jerk );

        if (jerkDuration.IsNegative)
        {
          return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
        }

        positiveJerkMaxAcceleration = _kinematics.CalculateCubicAcceleration( jerkDuration, jerk );

        positiveJerkMaxVelocity = _kinematics.CalculateCubicVelocity( jerkDuration, jerk );
        constantAccelerationMaxVelocity = positiveJerkMaxVelocity;
        negativeJerkMaxVelocity = 2 * positiveJerkMaxVelocity;

        positiveJerkDisplacement = _kinematics.CalculateCubicPosition( jerkDuration, jerk );
        negativeJerkDisplacement = _kinematics.CalculateCubicPosition(
          jerkDuration,
          constantAccelerationMaxVelocity,
          positiveJerkMaxAcceleration,
          -jerk
        );
      }
      else if (totalDisplacement > calculatedTotalDisplacement)
      {
        // If removing constant acceleration phase is resulted in a leftover of the displacement,
        // it means that the motion should have acceleration phase but with reduced duration.
        //
        // NOTE: Calculating constant acceleration duration is a bit tricky since it caused circular
        //       dependency. The calculation of constant acceleration duration is depended on the negative
        //       jerk displacement, while the calculation of negative jerk displacement is depended on
        //       the velocity at the end of constant acceleration phase.
        //
        // Currently, we have the following variables:
        // - sₕ  : the half displacement of the motion
        // - s₁ : the displacement of positive jerk phase (known)
        // - s₂ : the displacement of constant acceleration phase (unknown)
        // - s₃ : the displacement of negative jerk phase (unknown)
        // - t₁ : the duration of positive jerk phase (known)
        // - t₂ : the duration of constant acceleration phase (unknown)
        // - t₃ : the duration of negative jerk phase (known, is equal to t₁)
        // 
        // The sum of the displacement of the mentioned phases should be equal to the half displacement
        // of the motion, we can say that:
        //   sₕ = s₁ + s₂ + s₃
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
        //   sₕ = s₁ + s₂ + s₃
        //   sₕ = s₁ + (v₁t₂ + ½a₁(t₂)²) + (v₁t₃ + a₁t₂t₃ + ½a₁(t₃)² - ⅙j(t₃)³)
        //   sₕ = s₁ + v₁t₂ + ½a₁(t₂)² + v₁t₃ + a₁t₂t₃ + ½a₁(t₃)² - ⅙j(t₃)³
        //
        // Since all variables except t₂ is known, we got quadratic equation in the function of t₂:
        //   ½a₁(t₂)² + (v₁ + a₁t₃)t₂ + (s₁ + v₁t₃ + ½a₁(t₃)² - ⅙j(t₃)³ - sₕ) = 0
        //
        // The discriminant, d, is given by:
        //   d = b² - 4ac
        // where, a = ½a₁
        //        b = v₁ + a₁t₃
        //        c = s₁ + v₁t₃ + ½a₁(t₃)² - ⅙j(t₃)³ - sₕ
        //
        // The root of the quadratic formula is given by:
        //   t₂ = (-b ± √d) / 2a
        //
        // Because time cannot be negative, we only care about the positive result:
        //   t₂ = (-b + √d) / 2a
        //
        var halfDisplacement = totalDisplacement / 2;

        var a = positiveJerkMaxAcceleration / 2;
        var b = positiveJerkMaxVelocity + positiveJerkMaxAcceleration * jerkDuration;
        var c = positiveJerkDisplacement
                + positiveJerkMaxVelocity * jerkDuration
                + positiveJerkMaxAcceleration * jerkDuration * jerkDuration / 2
                - jerk * jerkDuration * jerkDuration * jerkDuration / 6
                - halfDisplacement;

        var d = ( b * b ) - ( 4 * a * c );

        if (d.IsNegative)
        {
          return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
        }

        constantAccelerationDuration = ( -b + d.SquareRoot() ) / ( 2 * a );

        if (constantAccelerationDuration.IsNegative)
        {
          return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
        }

        constantAccelerationDisplacement = _kinematics.CalculateQuadraticPosition(
          constantAccelerationDuration,
          positiveJerkMaxVelocity,
          positiveJerkMaxAcceleration
        );

        constantAccelerationMaxVelocity = positiveJerkMaxVelocity
                                          + _kinematics.CalculateQuadraticVelocity(
                                            constantAccelerationDuration,
                                            positiveJerkMaxAcceleration
                                          );

        negativeJerkMaxVelocity = constantAccelerationMaxVelocity + positiveJerkMaxVelocity;
        negativeJerkDisplacement = _kinematics.CalculateCubicPosition(
          jerkDuration,
          constantAccelerationMaxVelocity,
          positiveJerkMaxAcceleration,
          -jerk
        );
      }
    }
    else if (totalDisplacement > calculatedTotalDisplacement)
    {
      // If the requested total displacement is larger than the calculated total displacement,
      // it means that the motion should have constant velocity phase.
      constantVelocityDisplacement = totalDisplacement - calculatedTotalDisplacement;
      constantVelocityDuration = _kinematics.CalculateLinearTime(
        constantVelocityDisplacement,
        negativeJerkMaxVelocity
      );

      if (constantAccelerationDuration.IsNegative)
      {
        return Result.Error<MotionProfile>( ErrorCode.NegativeTimeResult );
      }
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
      positiveJerkMaxVelocity,
      constantAccelerationMaxVelocity,
      negativeJerkMaxVelocity
    );

    var accelerationProfile = new AccelerationProfile( positiveJerkMaxAcceleration );

    var motionProfile = new MotionProfile(
      totalDisplacement,
      negativeJerkMaxVelocity,
      positiveJerkMaxAcceleration,
      jerk,
      timeProfile,
      displacementProfile,
      velocityProfile,
      accelerationProfile
    );

    return Result.Success( motionProfile );
  }
}