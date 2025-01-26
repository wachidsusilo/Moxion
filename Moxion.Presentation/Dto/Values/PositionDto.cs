using Moxion.Common.Units;
using Moxion.Presentation.Abstractions.Dto;

namespace Moxion.Presentation.Dto.Values;

public readonly record struct PositionDto(
  double Value,
  PositionUnit Unit
) : IValueDto<PositionDto>;