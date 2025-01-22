using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Enumerations;

namespace Moxion.Application.Shared.Calculators.Results;

internal readonly record struct MotionPhaseCalculationResult( MotionPhase MotionPhase ) : ICalculationResult;