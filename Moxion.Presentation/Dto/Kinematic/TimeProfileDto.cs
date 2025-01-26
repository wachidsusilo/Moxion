using Moxion.Presentation.Abstractions.Dto;
using Moxion.Presentation.Dto.Values;

namespace Moxion.Presentation.Dto.Kinematic;

public readonly record struct TimeProfileDto(
  TimeDto JerkDuration,
  TimeDto ConstantAccelerationDuration,
  TimeDto ConstantVelocityDuration,
  TimeDto TotalDuration
) : IValueObjectDto<TimeProfileDto>;