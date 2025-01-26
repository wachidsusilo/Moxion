using Moxion.Abstractions;
using Moxion.Common.Units;

namespace Moxion.Domain.Units;

public readonly record struct PositionUnitInfo( PositionUnit Unit ) : IUnitInfo<PositionUnitInfo>;