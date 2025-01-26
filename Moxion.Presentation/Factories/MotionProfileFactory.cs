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
  private readonly ITimeProfileFactory _timeProfileFactory;
  private readonly IPositionProfileFactory _positionProfileFactory;
  private readonly IVelocityProfileFactory _velocityProfileFactory;
  private readonly IAccelerationProfileFactory _accelerationProfileFactory;
  private readonly ILogger<MotionProfileFactory> _logger;

  public MotionProfileFactory(
    IUnitConverter unitConverter,
    ITimeProfileFactory timeProfileFactory,
    IPositionProfileFactory positionProfileFactory,
    IVelocityProfileFactory velocityProfileFactory,
    IAccelerationProfileFactory accelerationProfileFactory,
    ILogger<MotionProfileFactory> logger
  )
  {
    _unitConverter = unitConverter;
    _timeProfileFactory = timeProfileFactory;
    _positionProfileFactory = positionProfileFactory;
    _velocityProfileFactory = velocityProfileFactory;
    _accelerationProfileFactory = accelerationProfileFactory;
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
    IReadOnlyList<MotionProfile> profiles,
    KinematicUnitInfo sourceUnit,
    KinematicUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    return Task.Run( () =>
      {
        var result = new List<MotionProfileDto>( profiles.Count );

        foreach (var motionProfile in profiles)
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
      profile.TotalDisplacement,
      sourceUnit.Position.Unit,
      destinationUnit.Position.Unit
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
      profile.MaxVelocity,
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
      profile.MaxAcceleration,
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

    var timeProfileResult = await _timeProfileFactory.Create(
      profile.TimeProfile,
      sourceUnit.Time,
      destinationUnit.Time,
      cancellationToken
    );

    if (timeProfileResult.HasError)
    {
      return Result.Error<MotionProfileDto>( timeProfileResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var positionProfileResult = await _positionProfileFactory.Create(
      profile.PositionProfile,
      sourceUnit.Position,
      destinationUnit.Position,
      cancellationToken
    );

    if (positionProfileResult.HasError)
    {
      return Result.Error<MotionProfileDto>( positionProfileResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var velocityProfileResult = await _velocityProfileFactory.Create(
      profile.VelocityProfile,
      sourceUnit.Velocity,
      destinationUnit.Velocity,
      cancellationToken
    );

    if (velocityProfileResult.HasError)
    {
      return Result.Error<MotionProfileDto>( velocityProfileResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var accelerationProfileResult = await _accelerationProfileFactory.Create(
      profile.AccelerationProfile,
      sourceUnit.Acceleration,
      destinationUnit.Acceleration,
      cancellationToken
    );

    if (accelerationProfileResult.HasError)
    {
      return Result.Error<MotionProfileDto>( accelerationProfileResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var motionProfileType = profile.GetProfileType();
    var displacementDto = new PositionDto( displacementResult.Data.ToDouble(), destinationUnit.Position.Unit );

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

    var result = new MotionProfileDto(
      motionProfileType,
      displacementDto,
      velocityDto,
      accelerationDto,
      jerkDto,
      timeProfileResult.Data,
      positionProfileResult.Data,
      velocityProfileResult.Data,
      accelerationProfileResult.Data
    );

    return Result.Success( result );
  }
}