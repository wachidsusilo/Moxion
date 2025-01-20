using Moxion.Abstractions;
using Moxion.Common.Units;

namespace Moxion.Domain.Units;

public readonly record struct AccelerationUnitInfo(
  PositionUnit PositionUnit,
  TimeUnit TimeUnit
) : IUnitInfo<AccelerationUnitInfo>;