using Microsoft.Extensions.Logging;
using Moxion.Application.Extensions;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Extensions;
using Moxion.Presentation.Abstractions.Validators;
using Moxion.Presentation.Features.Kinematic.Requests;

namespace Moxion.Presentation.Validators.Requests;

internal class KinematicSimulateRequestValidator : IKinematicSimulateRequestValidator
{
  private readonly ILogger<KinematicSimulateRequestValidator> _logger;

  public KinematicSimulateRequestValidator( ILogger<KinematicSimulateRequestValidator> logger )
  {
    _logger = logger;
  }

  public async Task<Result> Validate( KinematicSimulateRequest request, CancellationToken cancellationToken )
  {
    _logger.LogStart();

    var result = await ValidateInternal( request, cancellationToken );

    _logger.LogEnd( result );

    return result;
  }

  private static Task<Result> ValidateInternal( KinematicSimulateRequest request, CancellationToken cancellationToken )
  {
    if (!request.Displacement.Unit.IsValid())
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidPositionUnit ) );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error( ErrorCode.OperationCancelled ) );
    }

    if (!request.Velocity.PositionUnit.IsValid() || !request.Velocity.TimeUnit.IsValid())
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidVelocityUnit ) );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error( ErrorCode.OperationCancelled ) );
    }

    if (!request.Acceleration.PositionUnit.IsValid() || !request.Acceleration.TimeUnit.IsValid())
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidAccelerationUnit ) );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error( ErrorCode.OperationCancelled ) );
    }

    if (!request.Jerk.PositionUnit.IsValid() || !request.Jerk.TimeUnit.IsValid())
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidJerkUnit ) );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error( ErrorCode.OperationCancelled ) );
    }

    // TODO: _ws Put data count limit into config.
    if (request.DataCount is < 1 or > 1000)
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDataCount ) );
    }

    return Task.FromResult( Result.Error( ErrorCode.NoError ) );
  }
}