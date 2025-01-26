using Microsoft.Extensions.Logging;
using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Abstractions.Generators.Kinematic;
using Moxion.Application.Abstractions.Simulators.Kinematic;
using Moxion.Application.Extensions;
using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Generators.Params;
using Moxion.Application.Shared.Simulators.Params;
using Moxion.Application.Shared.Simulators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Domain.Kinematic;
using Moxion.Extensions;
using Moxion.Infrastructure.Kinematic.Extensions;

namespace Moxion.Infrastructure.Kinematic;

internal class MotionSimulator : IMotionSimulator
{
  private readonly IMotionProfileGenerator _profileGenerator;
  private readonly IMotionDisplacementCalculator _displacementCalculator;
  private readonly IMotionVelocityCalculator _velocityCalculator;
  private readonly IMotionAccelerationCalculator _accelerationCalculator;
  private readonly ILogger<MotionSimulator> _logger;

  public MotionSimulator(
    IMotionProfileGenerator profileGenerator,
    IMotionDisplacementCalculator displacementCalculator,
    IMotionVelocityCalculator velocityCalculator,
    IMotionAccelerationCalculator accelerationCalculator,
    ILogger<MotionSimulator> logger
  )
  {
    _profileGenerator = profileGenerator;
    _displacementCalculator = displacementCalculator;
    _velocityCalculator = velocityCalculator;
    _accelerationCalculator = accelerationCalculator;
    _logger = logger;
  }

  public async Task<Result<MotionSimulationResult>> Execute(
    MotionSimulationParam param,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart( param.DataCount );

    var result = await ExecuteInternal( param, cancellationToken );

    _logger.LogEnd( result, result.Data.MotionData?.Count );

    return result;
  }

  private async Task<Result<MotionSimulationResult>> ExecuteInternal(
    MotionSimulationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionSimulationResult>( ErrorCode.OperationCancelled );
    }

    var profileParam = new MotionProfileGenerationParam(
      param.Displacement,
      param.Velocity,
      param.Acceleration,
      param.Jerk
    );

    var profileResult = await _profileGenerator.Execute( profileParam, cancellationToken );

    if (profileResult.HasError)
    {
      return Result.Error<MotionSimulationResult>( profileResult.ErrorCode );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionSimulationResult>( ErrorCode.OperationCancelled );
    }

    var profile = profileResult.Data.MotionProfile;
    var totalDuration = profile.GetTotalDuration();
    var timeSlices = new Time[param.DataCount];
    var motionData = new MotionData[param.DataCount];

    for (var i = 0; i < timeSlices.Length; i++)
    {
      timeSlices[i] = totalDuration * i / ( param.DataCount - 1.0 );
    }

    var motionParam = new MotionCalculationParam( timeSlices, profile );
    var positionTask = _displacementCalculator.Execute( motionParam, cancellationToken );
    var velocityTask = _velocityCalculator.Execute( motionParam, cancellationToken );
    var accelerationTask = _accelerationCalculator.Execute( motionParam, cancellationToken );

    var positionResult = await positionTask;
    var velocityResult = await velocityTask;
    var accelerationResult = await accelerationTask;

    if (positionResult.HasError)
    {
      return Result.Error<MotionSimulationResult>( positionResult.ErrorCode );
    }

    if (velocityResult.HasError)
    {
      return Result.Error<MotionSimulationResult>( velocityResult.ErrorCode );
    }

    if (accelerationResult.HasError)
    {
      return Result.Error<MotionSimulationResult>( accelerationResult.ErrorCode );
    }

    var positionList = positionResult.Data.Displacement;
    var velocityList = velocityResult.Data.Velocity;
    var accelerationList = accelerationResult.Data.Acceleration;

    if (positionList.Count != motionData.Length)
    {
      return Result.Error<MotionSimulationResult>( ErrorCode.DataLengthMismatch );
    }

    if (velocityList.Count != motionData.Length)
    {
      return Result.Error<MotionSimulationResult>( ErrorCode.DataLengthMismatch );
    }

    if (accelerationList.Count != motionData.Length)
    {
      return Result.Error<MotionSimulationResult>( ErrorCode.DataLengthMismatch );
    }

    for (var i = 0; i < motionData.Length; i++)
    {
      var phase = profile.CalculatePhase( timeSlices[i] );

      motionData[i] = new MotionData(
        timeSlices[i],
        positionList[i],
        velocityList[i],
        accelerationList[i],
        profile.CalculateJerk( phase ),
        phase
      );
    }

    var result = new MotionSimulationResult( profile, motionData );
    return Result.Success( result );
  }
}