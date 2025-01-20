using Moxion.Abstractions;
using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Domain.Kinematic;

public readonly record struct MotionData(
  Time Time,
  Position Position,
  Velocity Velocity,
  Acceleration Acceleration,
  Jerk Jerk,
  MotionPhase Phase
) : IValueObject<MotionData>;