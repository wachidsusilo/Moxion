using Moxion.Common.Enumerations;

namespace Moxion.Common;

public readonly record struct Result
{
  public ErrorCode ErrorCode { get; }
  public Exception? Exception { get; }
  public bool HasError => ErrorCode != ErrorCode.NoError;
  public bool IsCancelled => ErrorCode == ErrorCode.OperationCancelled;

  private Result( ErrorCode errorCode, Exception? exception )
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

  public static Result<TData?> Error<TData>( ErrorCode errorCode ) =>
    new( errorCode, default, null );

  public static Result<TData> Error<TData>( ErrorCode errorCode, TData data ) => new( errorCode, data, null );

  public static Result<TData> Error<TData>( ErrorCode errorCode, TData data, Exception exception ) =>
    new( errorCode, data, exception );
}

public readonly record struct Result<TData>
{
  public ErrorCode ErrorCode { get; }
  public TData Data { get; }
  public Exception? Exception { get; }
  public bool HasError => ErrorCode != ErrorCode.NoError;
  public bool IsCancelled => ErrorCode == ErrorCode.OperationCancelled;

  internal Result( ErrorCode errorCode, TData data, Exception? exception )
  {
    ErrorCode = errorCode;
    Data = data;
    Exception = exception;
  }

  public static implicit operator Result<TData>( TData data ) => new( ErrorCode.NoError, data, null );

  public static implicit operator TData( Result<TData> data ) => data.Data;
}