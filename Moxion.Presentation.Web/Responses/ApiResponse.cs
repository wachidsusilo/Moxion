using Microsoft.AspNetCore.Mvc;
using Moxion.Presentation.Abstractions;
using Moxion.Presentation.Abstractions.Transport;

namespace Moxion.Presentation.Web.Responses;

public static class ApiResponse
{
  public static ActionResult<TResponse> Ok<TResponse>( TResponse response ) where TResponse : IResponse
  {
    return new ObjectResult( response )
    {
      StatusCode = StatusCodes.Status200OK
    };
  }

  public static ActionResult<TResponse> BadRequest<TResponse>( TResponse response ) where TResponse : IResponse
  {
    return new ObjectResult( response )
    {
      StatusCode = StatusCodes.Status400BadRequest
    };
  }

  public static ActionResult<TResponse> InternalServerError<TResponse>( TResponse response ) where TResponse : IResponse
  {
    return new ObjectResult( response )
    {
      StatusCode = StatusCodes.Status500InternalServerError
    };
  }
}