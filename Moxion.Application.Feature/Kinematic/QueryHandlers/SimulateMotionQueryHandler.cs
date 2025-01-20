using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Moxion.Application.Abstractions.Simulators.Kinematic;
using Moxion.Application.Extensions;
using Moxion.Application.Shared.Simulators.Params;
using Moxion.Application.Feature.Abstractions;
using Moxion.Application.Feature.Kinematic.Queries;
using Moxion.Application.Feature.Kinematic.QueryResults;

namespace Moxion.Application.Feature.Kinematic.QueryHandlers;

internal class SimulateMotionQueryHandler : IQueryHandler<SimulateMotionQuery, SimulateMotionQueryResult>
{
  private readonly IMotionSimulator _motionSimulator;
  private readonly ILogger<SimulateMotionQueryHandler> _logger;

  public SimulateMotionQueryHandler( IMotionSimulator motionSimulator, ILogger<SimulateMotionQueryHandler> logger )
  {
    _motionSimulator = motionSimulator;
    _logger = logger;
  }

  public async Task<SimulateMotionQueryResult> Handle(
    SimulateMotionQuery request,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var param = new MotionSimulationParam(
      request.Displacement,
      request.Velocity,
      request.Acceleration,
      request.Jerk,
      request.DataCount
    );

    var result = await _motionSimulator.Execute( param, cancellationToken );

    _logger.LogEnd( result );

    return new SimulateMotionQueryResult( result.ErrorCode, result.Data?.Profile, result.Data?.MotionData );
  }
}