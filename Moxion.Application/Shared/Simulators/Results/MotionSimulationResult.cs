using Moxion.Application.Abstractions.Simulators;
using Moxion.Domain.Kinematic;

namespace Moxion.Application.Shared.Simulators.Results;

internal record MotionSimulationResult(
  MotionProfile Profile,
  MotionData[] MotionData
) : ISimulationResult;