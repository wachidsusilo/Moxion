using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Values;

namespace Moxion.Application.Shared.Calculators.Results;

internal readonly record struct MotionDisplacementCalculationResult( IReadOnlyList<Position> Displacement )
  : ICalculationResult;