using Microsoft.Extensions.Logging;
using Moxion.Application.Extensions;
using Moxion.Application.Feature.Kinematic.Queries;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;
using Moxion.Domain.Units;
using Moxion.Presentation.Abstractions.Converters;
using Moxion.Presentation.Abstractions.Factories;
using Moxion.Presentation.Factories.Data;
using Moxion.Presentation.Features.Kinematic.Requests;

namespace Moxion.Presentation.Factories;

internal class KinematicQueryFactory : IKinematicQueryFactory
{
  private readonly IUnitConverter _unitConverter;
  private readonly ILogger<KinematicQueryFactory> _logger;

  public KinematicQueryFactory(
    IUnitConverter unitConverter,
    ILogger<KinematicQueryFactory> logger
  )
  {
    _unitConverter = unitConverter;
    _logger = logger;
  }

  public async Task<Result<SimulateMotionQueryFactoryData?>> Create(
    KinematicSimulateRequest request,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var result = await CreateInternal( request, cancellationToken );

    _logger.LogEnd( result );

    return result;
  }

  private async Task<Result<SimulateMotionQueryFactoryData?>> CreateInternal(
    KinematicSimulateRequest request,
    CancellationToken cancellationToken
  )
  {
    // TODO: _ws Create algorithm to determine which kinematic units to use.
    var targetPositionUnit = request.Displacement.Unit;
    var targetTimeUnit = request.Velocity.TimeUnit;

    var displacementResult = await _unitConverter.Convert(
      new Position( request.Displacement.Value ),
      request.Displacement.Unit,
      targetPositionUnit
    );

    if (displacementResult.HasError)
    {
      return Result.Error<SimulateMotionQueryFactoryData?>( displacementResult.ErrorCode, null );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<SimulateMotionQueryFactoryData?>( ErrorCode.OperationCancelled, null );
    }

    var velocityResult = await _unitConverter.Convert(
      new Velocity( request.Velocity.Value ),
      request.Velocity.PositionUnit,
      request.Velocity.TimeUnit,
      targetPositionUnit,
      targetTimeUnit
    );

    if (velocityResult.HasError)
    {
      return Result.Error<SimulateMotionQueryFactoryData?>( velocityResult.ErrorCode, null );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<SimulateMotionQueryFactoryData?>( ErrorCode.OperationCancelled, null );
    }

    var accelerationResult = await _unitConverter.Convert(
      new Acceleration( request.Acceleration.Value ),
      request.Acceleration.PositionUnit,
      request.Acceleration.TimeUnit,
      targetPositionUnit,
      targetTimeUnit
    );

    if (accelerationResult.HasError)
    {
      return Result.Error<SimulateMotionQueryFactoryData?>( accelerationResult.ErrorCode, null );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<SimulateMotionQueryFactoryData?>( ErrorCode.OperationCancelled, null );
    }

    var jerkResult = await _unitConverter.Convert(
      new Jerk( request.Acceleration.Value ),
      request.Jerk.PositionUnit,
      request.Jerk.TimeUnit,
      targetPositionUnit,
      targetTimeUnit
    );

    if (jerkResult.HasError)
    {
      return Result.Error<SimulateMotionQueryFactoryData?>( jerkResult.ErrorCode, null );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<SimulateMotionQueryFactoryData?>( ErrorCode.OperationCancelled, null );
    }

    var query = new SimulateMotionQuery(
      displacementResult.Data,
      velocityResult.Data,
      accelerationResult.Data,
      jerkResult.Data,
      request.DataCount
    );

    var unitInfo = KinematicUnitInfo.Create( targetPositionUnit, targetTimeUnit );
    var result = new SimulateMotionQueryFactoryData( query, unitInfo );

    return result;
  }
}