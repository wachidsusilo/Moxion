using Moxion.Abstractions;
using Moxion.Common.Values;

namespace Moxion.Domain.Kinematic;

public readonly record struct TimeProfile(
  Time JerkDuration,
  Time ConstantAccelerationDuration,
  Time ConstantVelocityDuration
) : IValueObject<TimeProfile>;