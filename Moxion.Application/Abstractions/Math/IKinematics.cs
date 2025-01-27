using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Application.Abstractions.Math;

internal interface IKinematics
{
  #region Linear Motion

  Position CalculateLinearPosition( Time time, Velocity velocity );
  Time CalculateLinearTime( Position position, Velocity velocity );

  #endregion

  #region Quadratic Motion

  Position CalculateQuadraticPosition( Time time, Acceleration acceleration );
  Position CalculateQuadraticPosition( Time time, Velocity initialVelocity, Acceleration acceleration );

  Position CalculateQuadraticPosition(
    Time time,
    Position initialPosition,
    Velocity initialVelocity,
    Acceleration acceleration
  );

  Time CalculateQuadraticTime( Position position, Acceleration acceleration );
  Time CalculateQuadraticTime( Position position, Velocity initialVelocity, Acceleration acceleration );
  Time CalculateQuadraticTime( Velocity velocity, Acceleration acceleration );
  Velocity CalculateQuadraticVelocity( Time time, Acceleration acceleration );

  #endregion

  #region Cubic Motion

  Position CalculateCubicPosition( Time time, Jerk jerk );
  Position CalculateCubicPosition( Time time, Velocity initialVelocity, Jerk jerk );
  Position CalculateCubicPosition( Time time, Velocity initialVelocity, Acceleration initialAcceleration, Jerk jerk );
  Time CalculateCubicTime( Position position, Jerk jerk );
  Time CalculateCubicTime( Acceleration acceleration, Jerk jerk );
  Time CalculateCubicTime( Velocity velocity, Jerk jerk );
  Velocity CalculateCubicVelocity( Time time, Jerk jerk );
  Velocity CalculateCubicVelocity( Time time, Acceleration initialAcceleration, Jerk jerk );
  Acceleration CalculateCubicAcceleration( Time time, Jerk jerk );

  /// <summary>
  /// 
  /// </summary>
  /// <param name="totalDisplacement">The total displacement of the motion.</param>
  /// <param name="jerk">The rate of change of acceleration of the motion.</param>
  /// <returns>The duration of the jerk phase.</returns>
  /// <remarks>
  /// The duration of the jerk phase is calculated based on the following assumptions:
  /// <list type="number">
  /// <item>The duration of positive and negative jerk phases are symmetrical</item>
  /// <item>The contribution of positive and negative jerks to the velocity are identical</item>
  /// </list>
  /// Phase 1 - Positive Jerk Phase:
  /// <list type="bullet">
  /// <item>Initial position is zero</item>
  /// <item>Initial velocity is zero</item>
  /// <item>Initial acceleration is zero</item>
  /// </list>
  /// The acceleration at the end of positive jerk phase is given by:
  /// <code>
  /// a = a₀ + jt
  /// a₁ = j₁t₁
  /// </code>
  /// The velocity at the end of positive jerk phase is given by:
  /// <code>
  /// v = v₀ + a₀t + ½jt²
  /// v₁ = ½j₁(t₁)²
  /// </code>
  /// The position at the end of positive jerk phase is given by:
  /// <code>
  /// s = s₀ + v₀t + ½a₀t² + ⅙jt³
  /// s₁ = ⅙j₁(t₁)³
  /// </code>
  /// Phase 2 - Negative Jerk Phase:
  /// <list type="bullet">
  /// <item>Initial position is <c>s₁</c></item>
  /// <item>Initial velocity is <c>v₁</c></item>
  /// <item>Initial acceleration is <c>a₁</c></item>
  /// </list>
  /// The position at the end of negative jerk phase is given by:
  /// <code>
  /// s = s₀ + v₀t + ½a₀t² + ⅙jt³
  /// s₂ = s₁ + v₁t₂ + ½a₁t₂² + ⅙j(t₂)³
  /// s₂ = ⅙j₁(t₁)³ + (½j₁(t₁)²)t₂ + ½(j₁t₁)t₂² + ⅙j₂(t₂)³
  /// </code>
  /// Since the duration of positive and negative jerk are symmetrical, we can assume that
  /// <c>t₁ = t₂ = t</c>, therefore the previous equation can be simplified further:
  /// <code>
  /// s₂ = ⅙j₁(t₂)³ + (½j₁(t₂)²)t₂ + ½(j₁t₂)t₂² + ⅙j₂(t₂)³
  /// s₂ = ⅙j₁(t₂)³ + ½j₁(t₂)³ + ½j₁(t₂)³ + ⅙j₂(t₂)³
  /// </code>
  /// The positive and negative jerk has the same magnitude but with opposite direction,
  /// so we can say that <c>j₂ = -j₁</c>, therefore the previous equation can be expressed
  /// as of the following:
  /// <code>
  /// s₂ = ⅙j₁(t₂)³ + ½j₁(t₂)³ + ½j₁(t₂)³ - ⅙j₁(t₂)³
  /// s₂ = ½j₁(t₂)³ + ½j₁(t₂)³
  /// s₂ = j₁(t₂)³
  /// </code>
  /// The duration of the positive jerk phase is the same as the duration of the negative
  /// jerk. Therefore, the jerk duration is given by:
  /// <code>
  /// t₁ = t₂ = ∛(s₂/j₁)
  /// where,
  /// s₂ is the displacement of the acceleration phase (including positive and negative jerk phases)
  /// j₁ is the rate of change of acceleration
  /// </code>
  /// </remarks>
  Time CalculateCubicJerkDuration( Position totalDisplacement, Jerk jerk );

  /// <summary>
  /// Calculates the jerk duration based on the specified maximum velocity.
  /// </summary>
  /// <param name="maxVelocity">The maximum velocity of the motion.</param>
  /// <param name="jerk">The rate of change of acceleration of the motion.</param>
  /// <returns>The duration of the jerk phase.</returns>
  /// <remarks>
  /// The duration of the jerk phase is calculated based on the following assumptions:
  /// <list type="number">
  /// <item>The duration of positive and negative jerk phases are symmetrical</item>
  /// <item>The contribution of positive and negative jerks to the velocity are identical</item>
  /// </list>
  /// Phase 1 - Positive Jerk Phase:
  /// <list type="bullet">
  /// <item>Initial velocity is zero</item>
  /// <item>Initial acceleration is zero</item>
  /// </list>
  /// The velocity at the end of positive jerk phase is given by:
  /// <code>
  /// v = v₀ + a₀t + ½jt²
  /// v₁ = ½j(t₁)²
  /// </code>
  /// While the acceleration at the end of positive jerk phase is given by:
  /// <code>
  /// a = a₀ + jt
  /// a₁ = jt₁
  /// </code>
  /// Phase 2 - Negative Jerk Phase:
  /// <list type="bullet">
  /// <item>Initial velocity is <c>v₁</c></item>
  /// <item>Initial acceleration is <c>a₁</c></item>
  /// </list>
  /// In the negative jerk phase, the acceleration at any given point in time is given by:
  /// <code>
  /// a(t) = a₁ - jt = jt₁ - jt
  /// </code>
  /// The velocity during the negative jerk phase is given by:
  /// <code>
  /// v₂ = v₁ + ∫a(t)dt
  /// v₂ = v₁ + ∫(jt₁ - jt)dt
  /// v₂ = v₁ + ∫(jt₁)dt - ∫(jt)dt
  /// </code>
  /// By applying finite integration from 0 to <c>t₂</c>, we got:
  /// <code>
  /// v₂ = v₁ + jt₁t₂ - ½j(t₂)²
  /// v₂ = ½j(t₁)² + jt₁t₂ - ½j(t₂)²
  /// </code>
  /// At the end of negative jerk phase, the velocity is at maximum.
  /// Therefore, <c>v₂</c> is equal to the maximum velocity of the motion.
  /// We assume that the duration for positive and negative jerk phases are symmetric,
  /// which means <c>t₁</c> is equal to <c>t₂</c>.
  /// By substituting <c>t₁ = t₂</c> to the previous equation, we got:
  /// <code>
  /// v₂ = ½j(t₂)² + j(t₂)² - ½j(t₂)²
  /// v₂ = j(t₂)²
  /// t₁ = t₂ = √(v₂/j)
  /// </code>
  /// </remarks>
  Time CalculateCubicJerkDuration( Velocity maxVelocity, Jerk jerk );

  #endregion
}