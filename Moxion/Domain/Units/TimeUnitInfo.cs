using Moxion.Abstractions;
using Moxion.Common.Units;

namespace Moxion.Domain.Units;

public readonly record struct TimeUnitInfo( TimeUnit Unit ) : IUnitInfo<TimeUnitInfo>;