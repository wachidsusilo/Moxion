using Moxion.Abstractions;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Domain.Kinematic;

public readonly record struct MotionProfile(
  Position TotalDisplacement,
  Velocity MaxVelocity,
  Acceleration MaxAcceleration,
  Jerk Jerk,
  TimeProfile TimeProfile,
  PositionProfile PositionProfile,
  VelocityProfile VelocityProfile,
  AccelerationProfile AccelerationProfile
) : IValueObject<MotionProfile>;