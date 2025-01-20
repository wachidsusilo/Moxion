using MediatR;
using Microsoft.Extensions.Logging;
using Moxion.Application.Extensions;
using Moxion.Common.Enumerations;
using Moxion.Presentation.Abstractions.Extractors;
using Moxion.Presentation.Abstractions.Factories;
using Moxion.Presentation.Abstractions.Validators;
using Moxion.Presentation.Features.Kinematic.Requests;
using Moxion.Presentation.Features.Kinematic.Responses;

namespace Moxion.Presentation.Features.Kinematic.Handlers;

internal class KinematicSimulateRequestHandler
  : IRequestHandler<KinematicSimulateRequest, KinematicSimulateResponse>
{
  private readonly ISender _sender;
  private readonly IKinematicQueryFactory _kinematicQueryFactory;
  private readonly IKinematicSimulateRequestValidator _simulateRequestValidator;
  private readonly IKinematicResponseFactory _responseFactory;
  private readonly IKinematicUnitExtractor _kinematicUnitExtractor;
  private readonly ILogger<KinematicSimulateRequestHandler> _logger;

  public KinematicSimulateRequestHandler(
    ISender sender,
    IKinematicQueryFactory kinematicQueryFactory,
    IKinematicSimulateRequestValidator simulateRequestValidator,
    IKinematicResponseFactory responseFactory,
    IKinematicUnitExtractor kinematicUnitExtractor,
    ILogger<KinematicSimulateRequestHandler> logger
  )
  {
    _sender = sender;
    _kinematicQueryFactory = kinematicQueryFactory;
    _simulateRequestValidator = simulateRequestValidator;
    _responseFactory = responseFactory;
    _kinematicUnitExtractor = kinematicUnitExtractor;
    _logger = logger;
  }

  public async Task<KinematicSimulateResponse> Handle(
    KinematicSimulateRequest request,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var response = await HandleInternal( request, cancellationToken );

    _logger.LogEnd( response.ErrorCode );

    return response;
  }

  private async Task<KinematicSimulateResponse> HandleInternal(
    KinematicSimulateRequest request,
    CancellationToken cancellationToken
  )
  {
    var validationResult = await _simulateRequestValidator.Validate( request, cancellationToken );

    if (validationResult.HasError)
    {
      return new KinematicSimulateResponse( validationResult.ErrorCode, null, null );
    }

    var queryResult = await _kinematicQueryFactory.Create( request, cancellationToken );

    if (queryResult.HasError)
    {
      return new KinematicSimulateResponse( queryResult.ErrorCode, null, null );
    }

    if (queryResult.Data is null)
    {
      return new KinematicSimulateResponse( ErrorCode.UnexpectedNullData, null, null );
    }

    var result = await _sender.Send( queryResult.Data.Query, cancellationToken );

    if (result.ErrorCode != ErrorCode.NoError)
    {
      return new KinematicSimulateResponse( result.ErrorCode, null, null );
    }

    var unitResult = await _kinematicUnitExtractor.Extract( request, CancellationToken.None );

    var response = await _responseFactory.Create(
      result,
      queryResult.Data.UnitInfo,
      unitResult.Data,
      cancellationToken
    );

    return response;
  }
}