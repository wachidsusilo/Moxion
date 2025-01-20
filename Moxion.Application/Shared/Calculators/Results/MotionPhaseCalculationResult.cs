using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Enumerations;

namespace Moxion.Application.Shared.Calculators.Results;

internal record MotionPhaseCalculationResult( MotionPhase MotionPhase ) : ICalculationResult;