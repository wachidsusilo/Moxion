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

internal class PositionProfileFactory : IPositionProfileFactory
{
  private readonly IUnitConverter _unitConverter;
  private readonly ILogger<PositionProfileFactory> _logger;

  public PositionProfileFactory( IUnitConverter unitConverter, ILogger<PositionProfileFactory> logger )
  {
    _unitConverter = unitConverter;
    _logger = logger;
  }

  public async Task<Result<PositionProfileDto>> Create(
    PositionProfile value,
    PositionUnitInfo sourceUnit,
    PositionUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var createInternalResult = await CreateInternal( value, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( createInternalResult );

    return createInternalResult;
  }

  public async Task<Result<PositionProfileDto[]>> Create(
    IReadOnlyList<PositionProfile> values,
    PositionUnitInfo sourceUnit,
    PositionUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart();

    var result = await CreateArrayInternal( values, sourceUnit, destinationUnit, cancellationToken );

    _logger.LogEnd( result );

    return result;
  }

  private Task<Result<PositionProfileDto[]>> CreateArrayInternal(
    IReadOnlyList<PositionProfile> profiles,
    PositionUnitInfo sourceUnit,
    PositionUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    return Task.Run( () =>
      {
        var result = new List<PositionProfileDto>( profiles.Count );

        foreach (var motionProfile in profiles)
        {
          var createInternalResult =
            CreateInternal( motionProfile, sourceUnit, destinationUnit, cancellationToken ).Result;

          if (createInternalResult.HasError)
          {
            return Task.FromResult( Result.Error<PositionProfileDto[]>( createInternalResult.ErrorCode, [] ) );
          }

          if (cancellationToken.IsCancellationRequested)
          {
            return Task.FromResult( Result.Error<PositionProfileDto[]>( ErrorCode.OperationCancelled, [] ) );
          }

          result.Add( createInternalResult.Data );
        }

        return Task.FromResult( Result.Success( result.ToArray() ) );
      },
      CancellationToken.None
    );
  }

  private async Task<Result<PositionProfileDto>> CreateInternal(
    PositionProfile profile,
    PositionUnitInfo sourceUnit,
    PositionUnitInfo destinationUnit,
    CancellationToken cancellationToken
  )
  {
    var positiveJerkDisplacementResult = await _unitConverter.Convert(
      profile.PositiveJerkDisplacement,
      sourceUnit.Unit,
      destinationUnit.Unit
    );

    if (positiveJerkDisplacementResult.HasError)
    {
      return Result.Error<PositionProfileDto>( positiveJerkDisplacementResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<PositionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var negativeJerkDisplacementResult = await _unitConverter.Convert(
      profile.NegativeJerkDisplacement,
      sourceUnit.Unit,
      destinationUnit.Unit
    );

    if (negativeJerkDisplacementResult.HasError)
    {
      return Result.Error<PositionProfileDto>( negativeJerkDisplacementResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<PositionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var constantAccelerationDisplacementResult = await _unitConverter.Convert(
      profile.ConstantAccelerationDisplacement,
      sourceUnit.Unit,
      destinationUnit.Unit
    );

    if (constantAccelerationDisplacementResult.HasError)
    {
      return Result.Error<PositionProfileDto>( constantAccelerationDisplacementResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<PositionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var constantVelocityDisplacementResult = await _unitConverter.Convert(
      profile.ConstantVelocityDisplacement,
      sourceUnit.Unit,
      destinationUnit.Unit
    );

    if (constantVelocityDisplacementResult.HasError)
    {
      return Result.Error<PositionProfileDto>( constantVelocityDisplacementResult.ErrorCode, default );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<PositionProfileDto>( ErrorCode.OperationCancelled, default );
    }

    var positiveJerkDisplacement = new PositionDto(
      positiveJerkDisplacementResult.Data.ToDouble(),
      destinationUnit.Unit
    );

    var negativeJerkDisplacement = new PositionDto(
      negativeJerkDisplacementResult.Data.ToDouble(),
      destinationUnit.Unit
    );

    var constantAccelerationDisplacement = new PositionDto(
      constantAccelerationDisplacementResult.Data.ToDouble(),
      destinationUnit.Unit
    );

    var constantVelocityDisplacement = new PositionDto(
      constantVelocityDisplacementResult.Data.ToDouble(),
      destinationUnit.Unit
    );

    var positionProfileDto = new PositionProfileDto(
      positiveJerkDisplacement,
      negativeJerkDisplacement,
      constantAccelerationDisplacement,
      constantVelocityDisplacement
    );

    return Result.Success( positionProfileDto );
  }
}