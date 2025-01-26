using Microsoft.Extensions.Logging;
using Moxion.Application.Extensions;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Domain.Kinematic;
using Moxion.Domain.Units;
using Moxion.Presentation.Abstractions.Converters;
using Moxion.Presentation.Abstractions.Factories;
using Moxion.Presentation.Dto.Kinematic;

namespace Moxion.Presentation.Factories;

internal class MotionDataFactory : IMotionDataFactory
{
  private readonly IUnitConverter _unitConverter;
  private readonly ILogger<MotionDataFactory> _logger;

  public MotionDataFactory(
    IUnitConverter unitConverter,
    ILogger<MotionDataFactory> logger
  )
  {
    _unitConverter = unitConverter;
    _logger = logger;
  }

  public async Task<Result<MotionDataDto>> Create(
    MotionData value,
    KinematicUnitInfo sourceUnit,
    KinematicUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var createInternalResult = await CreateInternal( value, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( createInternalResult );

    return createInternalResult;
  }

  public async Task<Result<MotionDataDto[]>> Create(
    IReadOnlyList<MotionData> values,
    KinematicUnitInfo sourceUnit,
    KinematicUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var result = await CreateArrayInternal( values, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( result );

    return result;
  }

  private Task<Result<MotionDataDto[]>> CreateArrayInternal(
    IReadOnlyList<MotionData> values,
    KinematicUnitInfo sourceUnit,
    KinematicUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    return Task.Run( () =>
      {
        var result = new List<MotionDataDto>( values.Count );

        foreach (var motionProfile in values)
        {
          var createInternalResult =
            CreateInternal( motionProfile, sourceUnit, destinationUnit, cancellationToken ).Result;

          if (createInternalResult.HasError)
          {
            return Task.FromResult( Result.Error<MotionDataDto[]>( createInternalResult.ErrorCode, [] ) );
          }

          if (cancellationToken.IsCancellationRequested)
          {
            return Task.FromResult( Result.Error<MotionDataDto[]>( ErrorCode.OperationCancelled, [] ) );
          }

          result.Add( createInternalResult.Data );
        }

        return Task.FromResult( Result.Success( result.ToArray() ) );
      },
      CancellationToken.None
    );
  }

  private async Task<Result<MotionDataDto>> CreateInternal(
    MotionData motionData,
    KinematicUnitInfo sourceUnit,
    KinematicUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    var timeResult = await _unitConverter.Convert(
      motionData.Time,
      sourceUnit.Time.Unit,
      destinationUnit.Time.Unit
    );

    if (timeResult.HasError)
    {
      return Result.Error<MotionDataDto>( timeResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDataDto>( ErrorCode.OperationCancelled, default );
    }

    var positionResult = await _unitConverter.Convert(
      motionData.Position,
      sourceUnit.Position.Unit,
      destinationUnit.Position.Unit
    );

    if (positionResult.HasError)
    {
      return Result.Error<MotionDataDto>( positionResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDataDto>( ErrorCode.OperationCancelled, default );
    }

    var velocityResult = await _unitConverter.Convert(
      motionData.Velocity,
      sourceUnit.Velocity.PositionUnit,
      sourceUnit.Velocity.TimeUnit,
      destinationUnit.Velocity.PositionUnit,
      destinationUnit.Velocity.TimeUnit
    );

    if (velocityResult.HasError)
    {
      return Result.Error<MotionDataDto>( velocityResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDataDto>( ErrorCode.OperationCancelled, default );
    }

    var accelerationResult = await _unitConverter.Convert(
      motionData.Acceleration,
      sourceUnit.Acceleration.PositionUnit,
      sourceUnit.Acceleration.TimeUnit,
      destinationUnit.Acceleration.PositionUnit,
      destinationUnit.Acceleration.TimeUnit
    );

    if (accelerationResult.HasError)
    {
      return Result.Error<MotionDataDto>( accelerationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDataDto>( ErrorCode.OperationCancelled, default );
    }

    var jerkResult = await _unitConverter.Convert(
      motionData.Jerk,
      sourceUnit.Jerk.PositionUnit,
      sourceUnit.Jerk.TimeUnit,
      destinationUnit.Jerk.PositionUnit,
      destinationUnit.Jerk.TimeUnit
    );

    if (jerkResult.HasError)
    {
      return Result.Error<MotionDataDto>( jerkResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionDataDto>( ErrorCode.OperationCancelled, default );
    }

    var result = new MotionDataDto(
      timeResult.Data.ToDouble(),
      positionResult.Data.ToDouble(),
      velocityResult.Data.ToDouble(),
      accelerationResult.Data.ToDouble(),
      jerkResult.Data.ToDouble(),
      motionData.Phase
    );

    return Result.Success( result );
  }
}