using Microsoft.Extensions.Logging;
using Moxion.Application.Extensions;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Domain.Units;
using Moxion.Presentation.Abstractions.Extractors;
using Moxion.Presentation.Features.Kinematic.Requests;

namespace Moxion.Presentation.Extractors;

internal class KinematicUnitExtractor : IKinematicUnitExtractor
{
  private readonly ILogger<KinematicUnitExtractor> _logger;

  public KinematicUnitExtractor( ILogger<KinematicUnitExtractor> logger )
  {
    _logger = logger;
  }

  public Task<Result<KinematicUnitInfo>> Extract(
    KinematicSimulateRequest request,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    if (cancellationToken.IsCancellationRequested)
    {
      _logger.LogEnd( ErrorCode.OperationCancelled );
      return Task.FromResult( Result.Error<KinematicUnitInfo>( ErrorCode.OperationCancelled, default ) );
    }

    var result = new KinematicUnitInfo(
      new PositionUnitInfo( request.Displacement.Unit ),
      new VelocityUnitInfo( request.Velocity.PositionUnit, request.Velocity.TimeUnit ),
      new AccelerationUnitInfo( request.Acceleration.PositionUnit, request.Acceleration.TimeUnit ),
      new JerkUnitInfo( request.Jerk.PositionUnit, request.Jerk.TimeUnit ),
      new TimeUnitInfo( request.TimeIntervalUnit )
    );

    _logger.LogEnd( ErrorCode.NoError );
    return Task.FromResult<Result<KinematicUnitInfo>>( result );
  }
}