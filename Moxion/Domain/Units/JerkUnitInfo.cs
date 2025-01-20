using Moxion.Abstractions;
using Moxion.Common.Units;

namespace Moxion.Domain.Units;

public readonly record struct JerkUnitInfo(
  PositionUnit PositionUnit,
  TimeUnit TimeUnit
) : IUnitInfo<JerkUnitInfo>;