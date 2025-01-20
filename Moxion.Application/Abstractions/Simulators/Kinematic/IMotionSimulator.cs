using Moxion.Application.Shared.Simulators.Params;
using Moxion.Application.Shared.Simulators.Results;

namespace Moxion.Application.Abstractions.Simulators.Kinematic;

internal interface IMotionSimulator : ISimulator<MotionSimulationParam, MotionSimulationResult?>;