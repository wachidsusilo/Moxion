using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Calculators.Results;

namespace Moxion.Application.Abstractions.Calculators.Kinematic;

internal interface IMotionPhaseCalculator
  : ICalculator<MotionPhaseCalculationParam, MotionPhaseCalculationResult>;