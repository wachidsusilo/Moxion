using Moxion.Application.Abstractions.Generators;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Application.Shared.Generators.Params;

internal readonly record struct MotionProfileGenerationParam(
  Position Displacement,
  Velocity Velocity,
  Acceleration Acceleration,
  Jerk Jerk
) : IGenerationParam;