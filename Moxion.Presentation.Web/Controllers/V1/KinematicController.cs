using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moxion.Application.Extensions;
using Moxion.Extensions;
using Moxion.Presentation.Features.Kinematic.Requests;
using Moxion.Presentation.Features.Kinematic.Responses;
using Moxion.Presentation.Web.Responses;

namespace Moxion.Presentation.Web.Controllers.V1;

[ApiVersion( "1.0" )]
[Route( "api/v{version:apiVersion}/[controller]" )]
public class KinematicController : ControllerBase
{
  private readonly ISender _sender;
  private readonly ILogger<KinematicController> _logger;

  public KinematicController(
    ISender sender,
    ILogger<KinematicController> logger
  )
  {
    _sender = sender;
    _logger = logger;
  }

  [HttpPost]
  public async Task<ActionResult<KinematicSimulateResponse>> Simulate( [FromBody] KinematicSimulateRequest request )
  {
    _logger.LogStart( nameof(KinematicSimulateRequest), request );

    var response = await _sender.Send( request );

    if (response.ErrorCode.IsInternalError())
    {
      _logger.LogEnd( response.ErrorCode );
      return ApiResponse.InternalServerError( response );
    }

    if (!response.ErrorCode.IsSuccess() && !response.ErrorCode.IsCancellation())
    {
      _logger.LogEnd( response.ErrorCode );
      return ApiResponse.BadRequest( response );
    }

    _logger.LogEnd( response.ErrorCode, nameof(KinematicSimulateResponse), response with { Data = null } );
    return Ok( response );
  }
}