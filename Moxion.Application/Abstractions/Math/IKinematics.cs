using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Application.Abstractions.Math;

internal interface IKinematics
{
  #region Linear Motion

  Position CalculatePosition( Time time, Velocity velocity );
  Time CalculateTime( Position position, Velocity velocity );

  #endregion

  #region Quadratic Motion

  Position CalculatePosition( Time time, Acceleration acceleration );
  Position CalculatePosition( Time time, Velocity initialVelocity, Acceleration initialAcceleration );

  Position CalculatePosition(
    Time time,
    Position initialPosition,
    Velocity initialVelocity,
    Acceleration acceleration
  );

  Time CalculateTime( Position position, Acceleration acceleration );
  Time CalculateTime( Velocity velocity, Acceleration acceleration );
  Velocity CalculateVelocity( Time time, Acceleration acceleration );

  #endregion

  #region Cubic Motion

  Position CalculatePosition( Time time, Jerk jerk );
  Position CalculatePosition( Time time, Velocity initialVelocity, Jerk jerk );
  Position CalculatePosition( Time time, Velocity initialVelocity, Acceleration initialAcceleration, Jerk jerk );
  Time CalculateTime( Position position, Jerk jerk );
  Time CalculateTime( Acceleration acceleration, Jerk jerk );
  Velocity CalculateVelocity( Time time, Jerk jerk );
  Velocity CalculateVelocity( Time time, Acceleration initialAcceleration, Jerk jerk );
  Acceleration CalculateAcceleration( Time time, Jerk jerk );
  Time CalculateJerkDuration( Velocity maxVelocity, Jerk jerk );

  #endregion
}