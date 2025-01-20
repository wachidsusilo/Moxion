using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Domain.Kinematic;

namespace Moxion.Application.Shared.Calculators.Params;

internal readonly record struct MotionDisplacementCalculationParam(
  MotionProfile Profile,
  MotionPhase Phase,
  Time PhaseDuration
) : ICalculationParam;