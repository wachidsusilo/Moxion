using Moxion.Abstractions;
using Moxion.Common.Values.Derived;

namespace Moxion.Domain.Kinematic;

public readonly record struct VelocityProfile(
  Velocity PositiveJerkMaxVelocity,
  Velocity ConstantAccelerationMaxVelocity,
  Velocity NegativeJerkMaxVelocity
) : IValueObject<VelocityProfile>;