using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Moxion.Common;

namespace Moxion.Application.Extensions;

public static class LoggerExtensions
{
  private record LogContext( Guid ExecutionId, long StartTime );

  private static readonly ConditionalWeakTable<ILogger, LogContext> ContextTable = new();

  public static void LogStart(
    this ILogger logger,
    [CallerFilePath] in string sourceFilePath = "",
    [CallerMemberName] in string methodName = ""
  )
  {
    var className = Path.GetFileNameWithoutExtension( sourceFilePath );
    var context = new LogContext( Guid.NewGuid(), Stopwatch.GetTimestamp() );

    logger.LogInformation(
      "Execution of {ClassName}::{MethodName} started with Execution ID {ExecutionId}.",
      className, methodName, context.ExecutionId
    );

    ContextTable.AddOrUpdate( logger, context );
  }

  public static void LogStart(
    this ILogger logger,
    int? dataCount,
    [CallerFilePath] in string sourceFilePath = "",
    [CallerMemberName] in string methodName = ""
  )
  {
    var className = Path.GetFileNameWithoutExtension( sourceFilePath );
    var context = new LogContext( Guid.NewGuid(), Stopwatch.GetTimestamp() );

    logger.LogInformation(
      "Execution of {ClassName}::{MethodName} started with Execution ID {ExecutionId}. The input contains {DataCount} items.",
      className, methodName, context.ExecutionId, dataCount
    );

    ContextTable.AddOrUpdate( logger, context );
  }

  public static void LogStart<TInput>(
    this ILogger logger,
    in string inputName,
    in TInput? input,
    [CallerFilePath] in string sourceFilePath = "",
    [CallerMemberName] in string methodName = ""
  )
  {
    var className = Path.GetFileNameWithoutExtension( sourceFilePath );
    var context = new LogContext( Guid.NewGuid(), Stopwatch.GetTimestamp() );

    logger.LogInformation(
      "Execution of {ClassName}::{MethodName} started with Execution ID {Id}. InputName: {InputName}, InputData: {@Input}.",
      className, methodName, context.ExecutionId, inputName, input
    );

    ContextTable.AddOrUpdate( logger, context );
  }

  public static void LogCheckPoint<TData>(
    this ILogger logger,
    in string dataName,
    in TData? data,
    [CallerFilePath] in string sourceFilePath = "",
    [CallerMemberName] in string methodName = ""
  )
  {
    var className = Path.GetFileNameWithoutExtension( sourceFilePath );
    var executionId = new Guid?();

    if (ContextTable.TryGetValue( logger, out var context ))
    {
      executionId = context.ExecutionId;
    }

    if (executionId is null)
    {
      logger.LogInformation(
        "Checkpoint reached in {ClassName}::{MethodName}. DataName={DataName}, Data={@Data}",
        className, methodName, dataName, data
      );

      return;
    }

    logger.LogInformation(
      "Checkpoint reached in {ClassName}::{MethodName} with Execution ID {id}. DataName={DataName}, Data={@Data}",
      className, methodName, executionId, dataName, data
    );
  }

  public static void LogEnd(
    this ILogger logger,
    in Result result,
    [CallerFilePath] in string sourceFilePath = "",
    [CallerMemberName] in string methodName = ""
  )
  {
    var className = Path.GetFileNameWithoutExtension( sourceFilePath );
    var elapsedMilliseconds = new long?();
    var executionId = new Guid?();

    if (ContextTable.TryGetValue( logger, out var context ))
    {
      elapsedMilliseconds = Stopwatch.GetElapsedTime( context.StartTime ).Milliseconds;
      executionId = context.ExecutionId;
      ContextTable.Remove( logger );
    }

    if (result.IsCancelled)
    {
      logger.LogWarning(
        "Execution of {ClassName}::{MethodName} was cancelled with Execution ID {Id} in {ExecutionTime} ms. ErrorCode: {ErrorCode}.",
        className, methodName, executionId, elapsedMilliseconds, result.ErrorCode
      );
    }

    if (result.HasError)
    {
      logger.LogError(
        "Execution of {ClassName}::{MethodName} failed with Execution ID {Id} in {ExecutionTime} ms. ErrorCode: {ErrorCode}.",
        className, methodName, executionId, elapsedMilliseconds, result.ErrorCode
      );
    }

    logger.LogInformation(
      "Execution of {ClassName}::{MethodName} completed with Execution ID {Id} in {ExecutionTime} ms. ErrorCode: {ErrorCode}.",
      className, methodName, executionId, elapsedMilliseconds, result.ErrorCode
    );
  }

  public static void LogEnd(
    this ILogger logger,
    in Result result,
    int? dataCount,
    [CallerFilePath] in string sourceFilePath = "",
    [CallerMemberName] in string methodName = ""
  )
  {
    var className = Path.GetFileNameWithoutExtension( sourceFilePath );
    var elapsedMilliseconds = new long?();
    var executionId = new Guid?();

    if (ContextTable.TryGetValue( logger, out var context ))
    {
      elapsedMilliseconds = Stopwatch.GetElapsedTime( context.StartTime ).Milliseconds;
      executionId = context.ExecutionId;
      ContextTable.Remove( logger );
    }

    if (result.IsCancelled)
    {
      logger.LogWarning(
        "Execution of {ClassName}::{MethodName} was cancelled with Execution ID {Id} in {ExecutionTime} ms. ProcessedItems: {DataCount}, ErrorCode: {ErrorCode}.",
        className, methodName, executionId, elapsedMilliseconds, dataCount, result.ErrorCode
      );
    }

    if (result.HasError)
    {
      logger.LogError(
        "Execution of {ClassName}::{MethodName} failed with Execution ID {Id} in {ExecutionTime} ms. ProcessedItems: {DataCount}, ErrorCode: {ErrorCode}.",
        className, methodName, executionId, elapsedMilliseconds, dataCount, result.ErrorCode
      );
    }

    logger.LogInformation(
      "Execution of {ClassName}::{MethodName} completed with Execution ID {Id} in {ExecutionTime} ms. ProcessedItems: {DataCount}, ErrorCode: {ErrorCode}.",
      className, methodName, executionId, elapsedMilliseconds, dataCount, result.ErrorCode
    );
  }

  public static void LogEnd<TOutput>(
    this ILogger logger,
    in Result result,
    in string outputName,
    in TOutput output,
    [CallerFilePath] in string sourceFilePath = "",
    [CallerMemberName] in string methodName = ""
  )
  {
    var className = Path.GetFileNameWithoutExtension( sourceFilePath );
    var elapsedMilliseconds = new long?();
    var executionId = new Guid?();

    if (ContextTable.TryGetValue( logger, out var context ))
    {
      elapsedMilliseconds = Stopwatch.GetElapsedTime( context.StartTime ).Milliseconds;
      executionId = context.ExecutionId;
      ContextTable.Remove( logger );
    }

    if (result.IsCancelled)
    {
      logger.LogWarning(
        "Execution of {ClassName}::{MethodName} was cancelled with Execution ID {Id} in {ExecutionTime} ms. OutputName: {OutputName}, OutputData={@OutputData}, ErrorCode: {ErrorCode}.",
        className, methodName, executionId, elapsedMilliseconds, outputName, output, result.ErrorCode
      );
    }

    if (result.HasError)
    {
      logger.LogError(
        "Execution of {ClassName}::{MethodName} failed with Execution ID {Id} in {ExecutionTime} ms. OutputName: {OutputName}, OutputData={@OutputData}, ErrorCode: {ErrorCode}.",
        className, methodName, executionId, elapsedMilliseconds, outputName, output, result.ErrorCode
      );
    }

    logger.LogInformation(
      "Execution of {ClassName}::{MethodName} completed with Execution ID {Id} in {ExecutionTime} ms. OutputName: {OutputName}, OutputData={@OutputData}, ErrorCode: {ErrorCode}.",
      className, methodName, executionId, elapsedMilliseconds, outputName, output, result.ErrorCode
    );
  }
}