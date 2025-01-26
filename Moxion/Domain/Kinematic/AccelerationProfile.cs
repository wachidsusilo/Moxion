using Moxion.Abstractions;
using Moxion.Common.Values.Derived;

namespace Moxion.Domain.Kinematic;

public readonly record struct AccelerationProfile(
  Acceleration PositiveJerkMaxAcceleration
) : IValueObject<AccelerationProfile>;