using Moxion.Abstractions;
using Moxion.Common.Units;

namespace Moxion.Domain.Units;

public readonly record struct KinematicUnitInfo(
  DisplacementUnitInfo Displacement,
  VelocityUnitInfo Velocity,
  AccelerationUnitInfo Acceleration,
  JerkUnitInfo Jerk,
  DurationUnitInfo Duration
) : IUnitInfo<KinematicUnitInfo>
{
  public static KinematicUnitInfo Create( PositionUnit positionUnit, TimeUnit timeUnit )
  {
    return new KinematicUnitInfo(
      new DisplacementUnitInfo( positionUnit ),
      new VelocityUnitInfo( positionUnit, timeUnit ),
      new AccelerationUnitInfo( positionUnit, timeUnit ),
      new JerkUnitInfo( positionUnit, timeUnit ),
      new DurationUnitInfo( timeUnit )
    );
  }
}