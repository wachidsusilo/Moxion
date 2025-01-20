using Moxion.Application.Abstractions.Simulators;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Application.Shared.Simulators.Params;

internal readonly record struct MotionSimulationParam(
  Position Displacement,
  Velocity Velocity,
  Acceleration Acceleration,
  Jerk Jerk,
  int DataCount
) : ISimulationParam;