using Moxion.Application.Feature.Abstractions;
using Moxion.Common.Enumerations;
using Moxion.Domain.Kinematic;

namespace Moxion.Application.Feature.Kinematic.QueryResults;

internal record SimulateMotionQueryResult(
  ErrorCode ErrorCode,
  MotionProfile? Profile,
  MotionData[]? Data
) : IQueryResult;