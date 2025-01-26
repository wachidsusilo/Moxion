using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Values;
using Moxion.Domain.Kinematic;

namespace Moxion.Application.Shared.Calculators.Params;

public readonly record struct MotionCalculationParam(
  IReadOnlyList<Time> TimeSlices,
  MotionProfile Profile
) : ICalculationParam;