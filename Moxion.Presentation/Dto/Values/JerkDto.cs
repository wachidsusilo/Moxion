using Moxion.Common.Units;
using Moxion.Presentation.Abstractions;
using Moxion.Presentation.Abstractions.Dto;

namespace Moxion.Presentation.Dto.Values;

public readonly record struct JerkDto(
  double Value,
  PositionUnit PositionUnit,
  TimeUnit TimeUnit
) : IValueDto<JerkDto>;