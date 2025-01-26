using Moxion.Presentation.Abstractions.Dto;
using Moxion.Presentation.Dto.Values;

namespace Moxion.Presentation.Dto.Kinematic;

public readonly record struct PositionProfileDto(
  PositionDto PositiveJerkDisplacement,
  PositionDto NegativeJerkDisplacement,
  PositionDto ConstantAccelerationDisplacement,
  PositionDto ConstantVelocityDisplacement
) : IValueObjectDto<PositionProfileDto>;