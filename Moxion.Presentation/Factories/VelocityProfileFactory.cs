using Microsoft.Extensions.Logging;
using Moxion.Application.Extensions;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Domain.Kinematic;
using Moxion.Domain.Units;
using Moxion.Presentation.Abstractions.Converters;
using Moxion.Presentation.Abstractions.Factories;
using Moxion.Presentation.Dto.Kinematic;
using Moxion.Presentation.Dto.Values;

namespace Moxion.Presentation.Factories;

internal class VelocityProfileFactory : IVelocityProfileFactory
{
  private readonly IUnitConverter _unitConverter;
  private readonly ILogger<VelocityProfileFactory> _logger;

  public VelocityProfileFactory( IUnitConverter unitConverter, ILogger<VelocityProfileFactory> logger )
  {
    _unitConverter = unitConverter;
    _logger = logger;
  }

  public async Task<Result<VelocityProfileDto>> Create(
    VelocityProfile value,
    VelocityUnitInfo sourceUnit,
    VelocityUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var createInternalResult = await CreateInternal( value, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( createInternalResult );

    return createInternalResult;
  }

  public async Task<Result<VelocityProfileDto[]>> Create(
    IReadOnlyList<VelocityProfile> values,
    VelocityUnitInfo sourceUnit,
    VelocityUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var result = await CreateArrayInternal( values, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( result );

    return result;
  }

  private Task<Result<VelocityProfileDto[]>> CreateArrayInternal(
    IReadOnlyList<VelocityProfile> profiles,
    VelocityUnitInfo sourceUnit,
    VelocityUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    return Task.Run( () =>
      {
        var result = new List<VelocityProfileDto>( profiles.Count );

        foreach (var motionProfile in profiles)
        {
          var createInternalResult =
            CreateInternal( motionProfile, sourceUnit, destinationUnit, cancellationToken ).Result;

          if (createInternalResult.HasError)
          {
            return Task.FromResult( Result.Error<VelocityProfileDto[]>( createInternalResult.ErrorCode, [] ) );
          }

          if (cancellationToken.IsCancellationRequested)
          {
            return Task.FromResult( Result.Error<VelocityProfileDto[]>( ErrorCode.OperationCancelled, [] ) );
          }

          result.Add( createInternalResult.Data );
        }

        return Task.FromResult( Result.Success( result.ToArray() ) );
      },
      CancellationToken.None
    );
  }

  private async Task<Result<VelocityProfileDto>> CreateInternal(
    VelocityProfile profile,
    VelocityUnitInfo sourceUnit,
    VelocityUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    var positiveJerkMaxVelocityResult = await _unitConverter.Convert(
      profile.PositiveJerkMaxVelocity,
      sourceUnit.PositionUnit,
      sourceUnit.TimeUnit,
      destinationUnit.PositionUnit,
      destinationUnit.TimeUnit
    );

    if (positiveJerkMaxVelocityResult.HasError)
    {
      return Result.Error<VelocityProfileDto>( positiveJerkMaxVelocityResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<VelocityProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var constantAccelerationMaxVelocityResult = await _unitConverter.Convert(
      profile.ConstantAccelerationMaxVelocity,
      sourceUnit.PositionUnit,
      sourceUnit.TimeUnit,
      destinationUnit.PositionUnit,
      destinationUnit.TimeUnit
    );

    if (constantAccelerationMaxVelocityResult.HasError)
    {
      return Result.Error<VelocityProfileDto>( constantAccelerationMaxVelocityResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<VelocityProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var negativeJerkMaxVelocityResult = await _unitConverter.Convert(
      profile.NegativeJerkMaxVelocity,
      sourceUnit.PositionUnit,
      sourceUnit.TimeUnit,
      destinationUnit.PositionUnit,
      destinationUnit.TimeUnit
    );

    if (negativeJerkMaxVelocityResult.HasError)
    {
      return Result.Error<VelocityProfileDto>( negativeJerkMaxVelocityResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<VelocityProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var positiveJerkMaxVelocity = new VelocityDto(
      positiveJerkMaxVelocityResult.Data.ToDouble(),
      destinationUnit.PositionUnit,
      destinationUnit.TimeUnit
    );

    var constantAccelerationMaxVelocity = new VelocityDto(
      constantAccelerationMaxVelocityResult.Data.ToDouble(),
      destinationUnit.PositionUnit,
      destinationUnit.TimeUnit
    );

    var negativeJerkMaxVelocity = new VelocityDto(
      negativeJerkMaxVelocityResult.Data.ToDouble(),
      destinationUnit.PositionUnit,
      destinationUnit.TimeUnit
    );

    var velocityProfileDto = new VelocityProfileDto(
      positiveJerkMaxVelocity,
      constantAccelerationMaxVelocity,
      negativeJerkMaxVelocity
    );

    return Result.Success( velocityProfileDto );
  }
}