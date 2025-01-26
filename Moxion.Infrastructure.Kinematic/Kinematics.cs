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

  public Position CalculateLinearPosition( Time time, Velocity velocity )
  {
    // s = s₀ + vt
    // where s₀ is zero
    // s = vt
    return velocity * time;
  }

  public Time CalculateLinearTime( Position position, Velocity velocity )
  {
    // s = s₀ + vt
    // where s₀ is zero
    // t = s/v
    return position / velocity;
  }

  #endregion

  #region Quadratic Motion

  public Position CalculateQuadraticPosition( Time time, Acceleration acceleration )
  {
    // s = s₀ + v₀t + ½at²
    // where s₀ and v₀ are zero
    // s = ½at²
    return ( ( acceleration * time * time ) / 2 );
  }

  public Position CalculateQuadraticPosition( Time time, Velocity initialVelocity, Acceleration acceleration )
  {
    // s = s₀ + v₀t + ½at²
    // where s₀ is zero
    // s = v₀t + ½at²
    return ( initialVelocity * time ) + ( ( acceleration * time * time ) / 2 );
  }

  public Position CalculateQuadraticPosition(
    Time time,
    Position initialPosition,
    Velocity initialVelocity,
    Acceleration acceleration
  )
  {
    // s = s₀ + v₀t + ½at²
    return initialPosition + ( initialVelocity * time ) + ( ( acceleration * time.Squared() ) / 2 );
  }

  public Time CalculateQuadraticTime( Position position, Acceleration acceleration )
  {
    // s = s₀ + v₀t + ½at²
    // where s₀ and v₀ are zero
    // s = ½at²
    // t = √(2s/a)
    return ( 2 * position / acceleration ).SquareRoot();
  }

  public Time CalculateQuadraticTime( Position position, Velocity initialVelocity, Acceleration acceleration )
  {
    // s = s₀ + v₀t + ½at²
    // where s₀ is zero
    // s = v₀t + ½at²
    // ½at² + v₀t - s = 0
    // at² + 2v₀t - 2s = 0
    // find the root of a quadratic equation:
    // y = (-b ± √(b² - 4ac)) / 2a
    // t = (-2v₀ ± √((2v₀)² - 4a(-2s))) / 2a
    // t = (-2v₀ ± √(4(v₀)² + 8as)) / 2a
    // t = (-v₀ ± √((v₀)² + 2as)) / a
    // time cannot be negative, so we only care about the positive result:
    // t = (-v₀ + √((v₀)² + 2as)) / a
    var discriminant = ( initialVelocity * initialVelocity ) + ( 2 * acceleration * position );

    return ( -initialVelocity + discriminant.SquareRoot() ) / acceleration;
  }

  public Time CalculateQuadraticTime( Velocity velocity, Acceleration acceleration )
  {
    // v = v₀ + at
    // where v₀ is zero
    // v = at
    // t = v/a
    return velocity / acceleration;
  }

  public Velocity CalculateQuadraticVelocity( Time time, Acceleration acceleration )
  {
    // v = v₀ + at
    // where v₀ is zero
    // v = at
    return acceleration * time;
  }

  #endregion

  #region Cubic Motion

  public Position CalculateCubicPosition( Time time, Jerk jerk )
  {
    // s = s₀ + v₀t + ½a₀t² + ⅙jt³
    // where s₀, v₀ and a₀ are zero
    // s = ⅙jt³
    return jerk * time.Cubed() / 6;
  }

  public Position CalculateCubicPosition( Time time, Velocity initialVelocity, Jerk jerk )
  {
    // s = s₀ + v₀t + ½a₀t² + ⅙jt³
    // where s₀ and a₀ are zero
    // s = v₀t + ⅙jt³
    return ( initialVelocity * time ) + ( jerk * time.Cubed() / 6 );
  }

  public Position CalculateCubicPosition( Time time, Velocity initialVelocity, Acceleration initialAcceleration,
    Jerk jerk )
  {
    // s = s₀ + v₀t + ½a₀t² + ⅙jt³
    // s = v₀t + ½a₀t² + ⅙jt³
    return ( initialVelocity * time )
           + ( ( initialAcceleration * time.Squared() ) / 2 )
           + ( jerk * time.Cubed() / 6 );
  }

  public Time CalculateCubicTime( Position position, Jerk jerk )
  {
    // s = s₀ + v₀t + ½a₀t² + ⅙jt³
    // where s₀, v₀ and a₀ are zero
    // s = ⅙jt³
    // t = ∛(6s/j)
    return ( 6 * position / jerk ).CubeRoot();
  }

  public Time CalculateCubicTime( Velocity velocity, Jerk jerk )
  {
    // v = v₀ + a₀t + ½jt²
    // where v₀ and a₀ are zero
    // v = ½jt²
    // t = √(2v/j)
    return ( 2 * velocity / jerk ).SquareRoot();
  }

  public Time CalculateCubicTime( Acceleration acceleration, Jerk jerk )
  {
    // a = a₀ + jt
    // where a₀ is zero
    // a = jt
    // t = a/j
    return acceleration / jerk;
  }

  public Velocity CalculateCubicVelocity( Time time, Jerk jerk )
  {
    // v = v₀ + a₀t + ½jt²
    // where v₀ and a₀ are zero
    // v = ½jt²
    return jerk * time.Squared() / 2;
  }

  public Velocity CalculateCubicVelocity( Time time, Acceleration initialAcceleration, Jerk jerk )
  {
    // v = v₀ + a₀t + ½jt²
    // where v₀ is zero
    // v = a₀t + ½jt²
    return ( initialAcceleration * time ) + ( jerk * time.Squared() / 2 );
  }

  public Acceleration CalculateCubicAcceleration( Time time, Jerk jerk )
  {
    // v = v₀ + a₀t + ½jt²
    // where v₀ is zero
    // v = a₀t + ½jt²
    return jerk * time;
  }

  /// <inheritdoc/>
  public Time CalculateCubicJerkDuration( Position totalDisplacement, Jerk jerk )
  {
    var halfDisplacement = totalDisplacement / 2;

    return ( halfDisplacement / jerk ).CubeRoot();
  }

  /// <inheritdoc/>
  public Time CalculateCubicJerkDuration( Velocity maxVelocity, Jerk jerk )
  {
    return ( maxVelocity / jerk ).SquareRoot();
  }

  #endregion
}