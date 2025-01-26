using Moxion.Common.Units;
using Moxion.Presentation.Abstractions.Dto;

namespace Moxion.Presentation.Dto.Values;

public readonly record struct VelocityDto(
  double Value,
  PositionUnit PositionUnit,
  TimeUnit TimeUnit
) : IValueDto<VelocityDto>;