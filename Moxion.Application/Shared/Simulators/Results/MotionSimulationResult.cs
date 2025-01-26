using Moxion.Application.Abstractions.Simulators;
using Moxion.Domain.Kinematic;

namespace Moxion.Application.Shared.Simulators.Results;

internal readonly record struct MotionSimulationResult(
  MotionProfile Profile,
  IReadOnlyList<MotionData>? MotionData
) : ISimulationResult;