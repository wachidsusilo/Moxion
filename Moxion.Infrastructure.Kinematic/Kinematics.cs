using Moxion.Application.Abstractions.Math;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Infrastructure.Kinematic;

internal class Kinematics : IKinematics
{
  #region Linear Motion

  // a = a₀ + jt
  // v = v₀ + a₀t + ½jt²
  // s = s₀ + v₀t + ½a₀t² + ⅙jt³

  public Position CalculatePosition( Time time, Velocity velocity )
  {
    // s = s₀ + vt
    // where s₀ is zero
    // s = vt
    return velocity * time;
  }

  public Time CalculateTime( Position position, Velocity velocity )
  {
    // s = s₀ + vt
    // where s₀ is zero
    // t = s/v
    return position / velocity;
  }

  #endregion

  #region Quadratic Motion

  public Position CalculatePosition( Time time, Acceleration acceleration )
  {
    // s = s₀ + v₀t + ½at²
    // where s₀ and v₀ are zero
    // s = ½at²
    return ( ( acceleration * time * time ) / 2 );
  }

  public Position CalculatePosition( Time time, Velocity initialVelocity, Acceleration initialAcceleration )
  {
    // s = s₀ + v₀t + ½at²
    // where s₀ is zero
    // s = v₀t + ½at²
    return ( initialVelocity * time ) + ( ( initialAcceleration * time * time ) / 2 );
  }

  public Position CalculatePosition(
    Time time,
    Position initialPosition,
    Velocity initialVelocity,
    Acceleration acceleration
  )
  {
    // s = s₀ + v₀t + ½at²
    return initialPosition + ( initialVelocity * time ) + ( ( acceleration * time.Squared() ) / 2 );
  }

  public Time CalculateTime( Position position, Acceleration acceleration )
  {
    // s = s₀ + v₀t + ½at²
    // where s₀ and v₀ are zero
    // s = ½at²
    // t = √(2s/a)
    return ( 2 * position / acceleration ).SquareRoot();
  }

  public Time CalculateTime( Velocity velocity, Acceleration acceleration )
  {
    // v = v₀ + at
    // where v₀ is zero
    // v = at
    // t = v/a
    return velocity / acceleration;
  }

  public Velocity CalculateVelocity( Time time, Acceleration acceleration )
  {
    // v = v₀ + at
    // where v₀ is zero
    // v = at
    return acceleration * time;
  }

  #endregion

  #region Cubic Motion

  public Position CalculatePosition( Time time, Jerk jerk )
  {
    // s = s₀ + v₀t + ½a₀t² + ⅙jt³
    // where s₀, v₀ and a₀ are zero
    // s = ⅙jt³
    return jerk * time.Cubed() / 6;
  }

  public Position CalculatePosition( Time time, Velocity initialVelocity, Jerk jerk )
  {
    // s = s₀ + v₀t + ½a₀t² + ⅙jt³
    // where s₀ and a₀ are zero
    // s = v₀t + ⅙jt³
    return ( initialVelocity * time ) + ( jerk * time.Cubed() / 6 );
  }

  public Position CalculatePosition( Time time, Velocity initialVelocity, Acceleration initialAcceleration, Jerk jerk )
  {
    // s = s₀ + v₀t + ½a₀t² + ⅙jt³
    // s = v₀t + ½a₀t² + ⅙jt³
    return ( initialVelocity * time )
           + ( ( initialAcceleration * time.Squared() ) / 2 )
           + ( jerk * time.Cubed() / 6 );
  }

  public Time CalculateTime( Position position, Jerk jerk )
  {
    // s = s₀ + v₀t + ½a₀t² + ⅙jt³
    // where s₀, v₀ and a₀ are zero
    // s = ⅙jt³
    // t = ∛(6s/j)
    return ( 6 * position / jerk ).CubeRoot();
  }

  public Time CalculateTime( Acceleration acceleration, Jerk jerk )
  {
    // a = a₀ + jt
    // where a₀ is zero
    // a = jt
    // t = a/j
    return acceleration / jerk;
  }

  public Velocity CalculateVelocity( Time time, Jerk jerk )
  {
    // v = v₀ + a₀t + ½jt²
    // where v₀ and a₀ are zero
    // v = ½jt²
    return jerk * time.Squared() / 2;
  }

  public Velocity CalculateVelocity( Time time, Acceleration initialAcceleration, Jerk jerk )
  {
    // v = v₀ + a₀t + ½jt²
    // where v₀ is zero
    // v = a₀t + ½jt²
    return ( initialAcceleration * time ) + ( jerk * time.Squared() / 2 );
  }

  public Acceleration CalculateAcceleration( Time time, Jerk jerk )
  {
    // v = v₀ + a₀t + ½jt²
    // where v₀ is zero
    // v = a₀t + ½jt²
    return jerk * time;
  }

  public Time CalculateJerkDuration( Velocity maxVelocity, Jerk jerk )
  {
    // Assumptions:
    // - The duration of positive and negative jerks are equal
    // - The contribution of positive and negative jerks to the velocity are identical
    // 
    // Positive Jerk phase:
    //   Conditions:
    //   - Initial position is zero
    //   - Initial velocity is zero
    //   - Initial acceleration is zero
    //
    //   Therefore, the velocity at the end of positive jerk phase is given by:
    //     v₁ = ½j(t₁)²
    //   While the acceleration at the end of positive jerk phase is given by:
    //     a₁ = jt₁
    //
    // Negative Jerk phase:
    //   Conditions:
    //   - Initial velocity is v₁
    //   - Initial acceleration is a₁
    //
    //   In the negative jerk phase, the acceleration at any given point in time is given by:
    //     a(t) = a₁ - jt = jt₁ - jt
    //   The velocity during the negative jerk phase is given by:
    //     v₂ = v₁ + ∫a(t)dt
    //     v₂ = v₁ + ∫(jt₁ - jt)dt
    //     v₂ = v₁ + ∫(jt₁)dt - ∫(jt)dt
    //   By applying finite integration from 0 to t₂, we got:
    //     v₂ = v₁ + jt₁t₂ - ½j(t₂)²
    //     v₂ = ½j(t₁)² + jt₁t₂ - ½j(t₂)²
    //   At the end of negative jerk phase, the velocity is maximum.
    //   Therefore, v₂ is the maximum velocity of the motion.
    //   We assume that the time is symmetric, which means t₁ is equal to t₂.
    //   By substituting t₁ = t₂ to the previous equation, we got:
    //    v₂ = ½j(t₂)² + j(t₂)² - ½j(t₂)²
    //    v₂ = j(t₂)²
    //    t₁ = t₂ = √(v₂/j)
    return ( maxVelocity / jerk ).SquareRoot();
  }

  #endregion
}