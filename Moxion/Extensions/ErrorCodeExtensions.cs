using Moxion.Common.Enumerations;

namespace Moxion.Extensions;

public static class ErrorCodeExtensions
{
  public static bool IsSuccess( this ErrorCode errorCode )
  {
    return errorCode == ErrorCode.NoError;
  }

  public static bool IsCancellation( this ErrorCode errorCode )
  {
    return errorCode == ErrorCode.OperationCancelled;
  }

  public static bool IsInternalError( this ErrorCode errorCode )
  {
    return errorCode
      is ErrorCode.UnknownError
      or ErrorCode.UnexpectedNullData;
  }
}