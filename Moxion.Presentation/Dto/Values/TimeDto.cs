using Moxion.Common.Units;
using Moxion.Presentation.Abstractions.Dto;

namespace Moxion.Presentation.Dto.Values;

public readonly record struct TimeDto(
  double Value,
  TimeUnit Unit
) : IValueDto<TimeDto>;