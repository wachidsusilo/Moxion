using Moxion.Abstractions;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Domain.Kinematic;

public readonly record struct MotionProfile(
  Position Displacement,
  Velocity Velocity,
  Acceleration Acceleration,
  Jerk Jerk,
  Time JerkDuration,
  Time AccelerationDuration,
  Time SteadyMotionDuration
) : IValueObject<MotionProfile>;