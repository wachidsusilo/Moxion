using Microsoft.Extensions.Logging;
using Moxion.Application.Extensions;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Domain.Kinematic;
using Moxion.Domain.Units;
using Moxion.Extensions;
using Moxion.Presentation.Abstractions.Converters;
using Moxion.Presentation.Abstractions.Factories;
using Moxion.Presentation.Dto.Kinematic;
using Moxion.Presentation.Dto.Values;

namespace Moxion.Presentation.Factories;

internal class TimeProfileFactory : ITimeProfileFactory
{
  private readonly IUnitConverter _unitConverter;
  private readonly ILogger<TimeProfileFactory> _logger;

  public TimeProfileFactory( IUnitConverter unitConverter, ILogger<TimeProfileFactory> logger )
  {
    _unitConverter = unitConverter;
    _logger = logger;
  }

  public async Task<Result<TimeProfileDto>> Create(
    TimeProfile value,
    TimeUnitInfo sourceUnit,
    TimeUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var createInternalResult = await CreateInternal( value, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( createInternalResult );

    return createInternalResult;
  }

  public async Task<Result<TimeProfileDto[]>> Create(
    IReadOnlyList<TimeProfile> values,
    TimeUnitInfo sourceUnit,
    TimeUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var result = await CreateArrayInternal( values, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( result );

    return result;
  }

  private Task<Result<TimeProfileDto[]>> CreateArrayInternal(
    IReadOnlyList<TimeProfile> profiles,
    TimeUnitInfo sourceUnit,
    TimeUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    return Task.Run( () =>
      {
        var result = new List<TimeProfileDto>( profiles.Count );

        foreach (var motionProfile in profiles)
        {
          var createInternalResult =
            CreateInternal( motionProfile, sourceUnit, destinationUnit, cancellationToken ).Result;

          if (createInternalResult.HasError)
          {
            return Task.FromResult( Result.Error<TimeProfileDto[]>( createInternalResult.ErrorCode, [] ) );
          }

          if (cancellationToken.IsCancellationRequested)
          {
            return Task.FromResult( Result.Error<TimeProfileDto[]>( ErrorCode.OperationCancelled, [] ) );
          }

          result.Add( createInternalResult.Data );
        }

        return Task.FromResult( Result.Success( result.ToArray() ) );
      },
      CancellationToken.None
    );
  }

  private async Task<Result<TimeProfileDto>> CreateInternal(
    TimeProfile profile,
    TimeUnitInfo sourceUnit,
    TimeUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    var jerkDurationResult = await _unitConverter.Convert(
      profile.JerkDuration,
      sourceUnit.Unit,
      destinationUnit.Unit
    );

    if (jerkDurationResult.HasError)
    {
      return Result.Error<TimeProfileDto>( jerkDurationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<TimeProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var constantAccelerationDurationResult = await _unitConverter.Convert(
      profile.ConstantAccelerationDuration,
      sourceUnit.Unit,
      destinationUnit.Unit
    );

    if (constantAccelerationDurationResult.HasError)
    {
      return Result.Error<TimeProfileDto>( constantAccelerationDurationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<TimeProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var constantVelocityDurationResult = await _unitConverter.Convert(
      profile.ConstantVelocityDuration,
      sourceUnit.Unit,
      destinationUnit.Unit
    );

    if (constantVelocityDurationResult.HasError)
    {
      return Result.Error<TimeProfileDto>( constantVelocityDurationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<TimeProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var totalDurationResult = await _unitConverter.Convert(
      profile.GetTotalDuration(),
      sourceUnit.Unit,
      destinationUnit.Unit
    );

    if (totalDurationResult.HasError)
    {
      return Result.Error<TimeProfileDto>( totalDurationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<TimeProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var jerkDurationDto = new TimeDto( jerkDurationResult.Data.ToDouble(), destinationUnit.Unit );

    var constantAccelerationDurationDto = new TimeDto(
      constantAccelerationDurationResult.Data.ToDouble(),
      destinationUnit.Unit
    );

    var constantVelocityDurationDto = new TimeDto(
      constantVelocityDurationResult.Data.ToDouble(),
      destinationUnit.Unit
    );

    var totalDurationDto = new TimeDto( totalDurationResult.Data.ToDouble(), destinationUnit.Unit );

    var timeProfileDto = new TimeProfileDto(
      jerkDurationDto,
      constantAccelerationDurationDto,
      constantVelocityDurationDto,
      totalDurationDto
    );

    return Result.Success( timeProfileDto );
  }
}