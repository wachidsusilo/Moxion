using Moxion.Abstractions;
using Moxion.Common.Values;

namespace Moxion.Domain.Kinematic;

public readonly record struct PositionProfile(
  Position PositiveJerkDisplacement,
  Position NegativeJerkDisplacement,
  Position ConstantAccelerationDisplacement,
  Position ConstantVelocityDisplacement
) : IValueObject<PositionProfile>;