using Moxion.Presentation.Abstractions.Dto;
using Moxion.Presentation.Dto.Values;

namespace Moxion.Presentation.Dto.Kinematic;

public readonly record struct AccelerationProfileDto(
  AccelerationDto PositiveJerkMaxAcceleration
) : IValueObjectDto<AccelerationProfileDto>;