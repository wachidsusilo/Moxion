using Microsoft.Extensions.Logging;
using Moxion.Application.Extensions;
using Moxion.Application.Feature.Kinematic.QueryResults;
using Moxion.Common.Enumerations;
using Moxion.Domain.Units;
using Moxion.Presentation.Abstractions.Factories;
using Moxion.Presentation.Features.Kinematic.Responses;

namespace Moxion.Presentation.Factories;

internal class KinematicResponseFactory : IKinematicResponseFactory
{
  private readonly IMotionProfileFactory _motionProfileFactory;
  private readonly IMotionDataFactory _motionDataFactory;
  private readonly ILogger<KinematicResponseFactory> _logger;

  public KinematicResponseFactory(
    IMotionProfileFactory motionProfileFactory,
    IMotionDataFactory motionDataFactory,
    ILogger<KinematicResponseFactory> logger
  )
  {
    _motionProfileFactory = motionProfileFactory;
    _motionDataFactory = motionDataFactory;
    _logger = logger;
  }

  public async Task<KinematicSimulateResponse> Create(
    SimulateMotionQueryResult result,
    KinematicUnitInfo sourceUnit,
    KinematicUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var response = await CreateInternal( result, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( response.ErrorCode );

    return response;
  }

  private async Task<KinematicSimulateResponse> CreateInternal(
    SimulateMotionQueryResult result,
    KinematicUnitInfo sourceUnit,
    KinematicUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    if (result.ErrorCode != ErrorCode.NoError)
    {
      return new KinematicSimulateResponse( result.ErrorCode, null, null );
    }

    if (result.Profile is null || result.Data is null)
    {
      return new KinematicSimulateResponse( ErrorCode.UnexpectedNullData, null, null );
    }

    var motionProfileResult = await _motionProfileFactory.Create(
      result.Profile.Value,
      sourceUnit,
      destinationUnit,
      cancellationToken
    );

    if (motionProfileResult.HasError)
    {
      return new KinematicSimulateResponse( motionProfileResult.ErrorCode, null, null );
    }

    var motionDataResult = await _motionDataFactory.Create(
      result.Data,
      sourceUnit,
      destinationUnit,
      cancellationToken
    );

    if (motionDataResult.HasError)
    {
      return new KinematicSimulateResponse( motionDataResult.ErrorCode, null, null );
    }

    var response = new KinematicSimulateResponse(
      motionDataResult.ErrorCode,
      motionProfileResult.Data,
      motionDataResult.Data
    );

    return response;
  }
}