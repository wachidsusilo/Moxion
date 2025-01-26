using Moxion.Abstractions;
using Moxion.Common.Units;

namespace Moxion.Domain.Units;

public readonly record struct KinematicUnitInfo(
  PositionUnitInfo Position,
  VelocityUnitInfo Velocity,
  AccelerationUnitInfo Acceleration,
  JerkUnitInfo Jerk,
  TimeUnitInfo Time
) : IUnitInfo<KinematicUnitInfo>
{
  public static KinematicUnitInfo Create( PositionUnit positionUnit, TimeUnit timeUnit )
  {
    return new KinematicUnitInfo(
      new PositionUnitInfo( positionUnit ),
      new VelocityUnitInfo( positionUnit, timeUnit ),
      new AccelerationUnitInfo( positionUnit, timeUnit ),
      new JerkUnitInfo( positionUnit, timeUnit ),
      new TimeUnitInfo( timeUnit )
    );
  }
}