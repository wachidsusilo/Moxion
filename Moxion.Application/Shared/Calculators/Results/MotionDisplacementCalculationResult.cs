using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Values;

namespace Moxion.Application.Shared.Calculators.Results;

internal record MotionDisplacementCalculationResult( Position Displacement ) : ICalculationResult;