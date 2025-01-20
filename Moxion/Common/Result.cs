using Moxion.Common.Enumerations;

namespace Moxion.Common;

public record Result
{
  public ErrorCode ErrorCode { get; }
  public Exception? Exception { get; }
  public bool HasError => ErrorCode != ErrorCode.NoError;
  public bool IsCancelled => ErrorCode == ErrorCode.OperationCancelled;

  protected Result( ErrorCode errorCode, Exception? exception )
  {
    ErrorCode = errorCode;
    Exception = exception;
  }

  public static implicit operator Result( ErrorCode errorCode ) => new( errorCode, null );

  public static implicit operator Result( Exception? exception ) => new( ErrorCode.UnknownError, exception );

  public static implicit operator ErrorCode( Result result ) => result.ErrorCode;

  public static implicit operator Exception?( Result result ) => result.Exception;

  public static Result Create( ErrorCode errorCode, Exception? exception = null ) => new( errorCode, exception );

  public static Result<TData> Create<TData>( ErrorCode errorCode, TData data, Exception? exception = null ) =>
    new( errorCode, data, exception );

  public static Result Success() => new( ErrorCode.NoError, null );

  public static Result<TData> Success<TData>( TData data ) => new( ErrorCode.NoError, data, null );

  public static Result Error( ErrorCode errorCode ) => new( errorCode, null );

  public static Result Error( ErrorCode errorCode, Exception exception ) => new( errorCode, exception );

  public static Result<TData?> Error<TData>( ErrorCode errorCode ) where TData : class? =>
    new( errorCode, null, null );

  public static Result<TData> Error<TData>( ErrorCode errorCode, TData data ) => new( errorCode, data, null );

  public static Result<TData> Error<TData>( ErrorCode errorCode, TData data, Exception exception ) =>
    new( errorCode, data, exception );
}

public record Result<TData> : Result
{
  public TData Data { get; }

  internal Result( ErrorCode errorCode, TData data, Exception? exception ) : base( errorCode, exception )
  {
    Data = data;
  }

  public static implicit operator Result<TData>( TData data ) => new( ErrorCode.NoError, data, null );

  public static implicit operator TData( Result<TData> data ) => data.Data;
}