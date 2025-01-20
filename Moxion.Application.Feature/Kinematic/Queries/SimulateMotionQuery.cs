using Moxion.Application.Feature.Abstractions;
using Moxion.Application.Feature.Kinematic.QueryResults;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Application.Feature.Kinematic.Queries;

internal record SimulateMotionQuery(
  Position Displacement,
  Velocity Velocity,
  Acceleration Acceleration,
  Jerk Jerk,
  int DataCount
) : IQuery<SimulateMotionQueryResult>;