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

internal class MotionProfileFactory : IMotionProfileFactory
{
  private readonly IUnitConverter _unitConverter;
  private readonly ILogger<MotionProfileFactory> _logger;

  public MotionProfileFactory(
    IUnitConverter unitConverter,
    ILogger<MotionProfileFactory> logger
  )
  {
    _unitConverter = unitConverter;
    _logger = logger;
  }

  public async Task<Result<MotionProfileDto>> Create(
    MotionProfile value,
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

  public async Task<Result<MotionProfileDto[]>> Create(
    IReadOnlyList<MotionProfile> values,
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

  private Task<Result<MotionProfileDto[]>> CreateArrayInternal(
    IReadOnlyList<MotionProfile> values,
    KinematicUnitInfo sourceUnit,
    KinematicUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    return Task.Run( () =>
      {
        var result = new List<MotionProfileDto>( values.Count );

        foreach (var motionProfile in values)
        {
          var createInternalResult =
            CreateInternal( motionProfile, sourceUnit, destinationUnit, cancellationToken ).Result;

          if (createInternalResult.HasError)
          {
            return Task.FromResult( Result.Error<MotionProfileDto[]>( createInternalResult.ErrorCode, [] ) );
          }

          if (cancellationToken.IsCancellationRequested)
          {
            return Task.FromResult( Result.Error<MotionProfileDto[]>( ErrorCode.OperationCancelled, [] ) );
          }

          result.Add( createInternalResult.Data );
        }

        return Task.FromResult( Result.Success( result.ToArray() ) );
      },
      CancellationToken.None
    );
  }

  private async Task<Result<MotionProfileDto>> CreateInternal(
    MotionProfile profile,
    KinematicUnitInfo sourceUnit,
    KinematicUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    var displacementResult = await _unitConverter.Convert(
      profile.Displacement,
      sourceUnit.Displacement.Unit,
      destinationUnit.Displacement.Unit
    );

    if (displacementResult.HasError)
    {
      return Result.Error<MotionProfileDto>( displacementResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var velocityResult = await _unitConverter.Convert(
      profile.Velocity,
      sourceUnit.Velocity.PositionUnit,
      sourceUnit.Velocity.TimeUnit,
      destinationUnit.Velocity.PositionUnit,
      destinationUnit.Velocity.TimeUnit
    );

    if (velocityResult.HasError)
    {
      return Result.Error<MotionProfileDto>( velocityResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var accelerationResult = await _unitConverter.Convert(
      profile.Acceleration,
      sourceUnit.Acceleration.PositionUnit,
      sourceUnit.Acceleration.TimeUnit,
      destinationUnit.Acceleration.PositionUnit,
      destinationUnit.Acceleration.TimeUnit
    );

    if (accelerationResult.HasError)
    {
      return Result.Error<MotionProfileDto>( accelerationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var jerkResult = await _unitConverter.Convert(
      profile.Jerk,
      sourceUnit.Jerk.PositionUnit,
      sourceUnit.Jerk.TimeUnit,
      destinationUnit.Jerk.PositionUnit,
      destinationUnit.Jerk.TimeUnit
    );

    if (jerkResult.HasError)
    {
      return Result.Error<MotionProfileDto>( jerkResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var jerkDurationResult = await _unitConverter.Convert(
      profile.JerkDuration,
      sourceUnit.Duration.Unit,
      destinationUnit.Duration.Unit
    );

    if (jerkDurationResult.HasError)
    {
      return Result.Error<MotionProfileDto>( jerkDurationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var accelerationDurationResult = await _unitConverter.Convert(
      profile.AccelerationDuration,
      sourceUnit.Duration.Unit,
      destinationUnit.Duration.Unit
    );

    if (accelerationDurationResult.HasError)
    {
      return Result.Error<MotionProfileDto>( accelerationDurationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var steadyMotionDurationResult = await _unitConverter.Convert(
      profile.SteadyMotionDuration,
      sourceUnit.Duration.Unit,
      destinationUnit.Duration.Unit
    );

    if (steadyMotionDurationResult.HasError)
    {
      return Result.Error<MotionProfileDto>( steadyMotionDurationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var totalDurationResult = await _unitConverter.Convert(
      profile.GetTotalDuration(),
      sourceUnit.Duration.Unit,
      destinationUnit.Duration.Unit
    );

    if (totalDurationResult.HasError)
    {
      return Result.Error<MotionProfileDto>( totalDurationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var motionProfileType = profile.GetProfileType();
    var displacementDto = new PositionDto( displacementResult.Data.ToDouble(), destinationUnit.Displacement.Unit );

    var velocityDto = new VelocityDto(
      velocityResult.Data.ToDouble(),
      destinationUnit.Velocity.PositionUnit,
      destinationUnit.Velocity.TimeUnit
    );

    var accelerationDto = new AccelerationDto(
      accelerationResult.Data.ToDouble(),
      destinationUnit.Acceleration.PositionUnit,
      destinationUnit.Acceleration.TimeUnit
    );

    var jerkDto = new JerkDto(
      jerkResult.Data.ToDouble(),
      destinationUnit.Jerk.PositionUnit,
      destinationUnit.Jerk.TimeUnit
    );

    var jerkDurationDto = new TimeDto( jerkDurationResult.Data.ToDouble(), destinationUnit.Duration.Unit );

    var accelerationDurationDto = new TimeDto(
      accelerationDurationResult.Data.ToDouble(),
      destinationUnit.Duration.Unit
    );

    var steadyMotionDurationDto = new TimeDto(
      steadyMotionDurationResult.Data.ToDouble(),
      destinationUnit.Duration.Unit
    );

    var totalDurationDto = new TimeDto( totalDurationResult.Data.ToDouble(), destinationUnit.Duration.Unit );

    var result = new MotionProfileDto(
      motionProfileType,
      displacementDto,
      velocityDto,
      accelerationDto,
      jerkDto,
      jerkDurationDto,
      accelerationDurationDto,
      steadyMotionDurationDto,
      totalDurationDto
    );

    return Result.Success( result );
  }
}