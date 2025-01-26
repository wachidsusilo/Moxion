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

internal class AccelerationProfileFactory : IAccelerationProfileFactory
{
  private readonly IUnitConverter _unitConverter;
  private readonly ILogger<AccelerationProfileFactory> _logger;

  public AccelerationProfileFactory( IUnitConverter unitConverter, ILogger<AccelerationProfileFactory> logger )
  {
    _unitConverter = unitConverter;
    _logger = logger;
  }

  public async Task<Result<AccelerationProfileDto>> Create(
    AccelerationProfile value,
    AccelerationUnitInfo sourceUnit,
    AccelerationUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var createInternalResult = await CreateInternal( value, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( createInternalResult );

    return createInternalResult;
  }

  public async Task<Result<AccelerationProfileDto[]>> Create(
    IReadOnlyList<AccelerationProfile> values,
    AccelerationUnitInfo sourceUnit,
    AccelerationUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var result = await CreateArrayInternal( values, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( result );

    return result;
  }

  private Task<Result<AccelerationProfileDto[]>> CreateArrayInternal(
    IReadOnlyList<AccelerationProfile> profiles,
    AccelerationUnitInfo sourceUnit,
    AccelerationUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    return Task.Run( () =>
      {
        var result = new List<AccelerationProfileDto>( profiles.Count );

        foreach (var motionProfile in profiles)
        {
          var createInternalResult =
            CreateInternal( motionProfile, sourceUnit, destinationUnit, cancellationToken ).Result;

          if (createInternalResult.HasError)
          {
            return Task.FromResult( Result.Error<AccelerationProfileDto[]>( createInternalResult.ErrorCode, [] ) );
          }

          if (cancellationToken.IsCancellationRequested)
          {
            return Task.FromResult( Result.Error<AccelerationProfileDto[]>( ErrorCode.OperationCancelled, [] ) );
          }

          result.Add( createInternalResult.Data );
        }

        return Task.FromResult( Result.Success( result.ToArray() ) );
      },
      CancellationToken.None
    );
  }

  private async Task<Result<AccelerationProfileDto>> CreateInternal(
    AccelerationProfile profile,
    AccelerationUnitInfo sourceUnit,
    AccelerationUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    var positiveJerkMaxAccelerationResult = await _unitConverter.Convert(
      profile.PositiveJerkMaxAcceleration,
      sourceUnit.PositionUnit,
      sourceUnit.TimeUnit,
      destinationUnit.PositionUnit,
      destinationUnit.TimeUnit
    );

    if (positiveJerkMaxAccelerationResult.HasError)
    {
      return Result.Error<AccelerationProfileDto>( positiveJerkMaxAccelerationResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<AccelerationProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var positiveJerkMaxAcceleration = new AccelerationDto(
      positiveJerkMaxAccelerationResult.Data.ToDouble(),
      destinationUnit.PositionUnit,
      destinationUnit.TimeUnit
    );

    var accelerationProfileDto = new AccelerationProfileDto( positiveJerkMaxAcceleration );

    return Result.Success( accelerationProfileDto );
  }
}