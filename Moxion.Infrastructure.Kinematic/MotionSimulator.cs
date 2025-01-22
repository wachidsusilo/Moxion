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
using Moxion.Common.Values.Derived;
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
  private readonly IMotionJerkCalculator _jerkCalculator;
  private readonly IMotionPhaseCalculator _phaseCalculator;
  private readonly ILogger<MotionSimulator> _logger;

  public MotionSimulator(
    IMotionProfileGenerator profileGenerator,
    IMotionDisplacementCalculator displacementCalculator,
    IMotionVelocityCalculator velocityCalculator,
    IMotionAccelerationCalculator accelerationCalculator,
    IMotionJerkCalculator jerkCalculator,
    IMotionPhaseCalculator phaseCalculator,
    ILogger<MotionSimulator> logger
  )
  {
    _profileGenerator = profileGenerator;
    _displacementCalculator = displacementCalculator;
    _velocityCalculator = velocityCalculator;
    _accelerationCalculator = accelerationCalculator;
    _jerkCalculator = jerkCalculator;
    _phaseCalculator = phaseCalculator;
    _logger = logger;
  }

  public async Task<Result<MotionSimulationResult>> Execute(
    MotionSimulationParam param,
    CancellationToken cancellationToken
  )
  {
    _logger.LogStart( param.DataCount );

    var result = await ExecuteInternal( param, cancellationToken );

    _logger.LogEnd( result, result.Data.MotionData.Length );

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
    var motionData = new MotionData[param.DataCount];
    var errorCode = ErrorCode.NoError;

    await Parallel.ForAsync( 0, param.DataCount, cancellationToken, async ( i, cancelToken ) =>
      {
        if (errorCode != ErrorCode.NoError || cancellationToken.IsCancellationRequested)
        {
          return;
        }

        var time = totalDuration * i / ( param.DataCount - 1.0 );

        var motionParam = new MotionCalculationParam( time, profile );
        var phaseResult = await _phaseCalculator.Execute( motionParam, cancelToken );

        if (phaseResult.HasError)
        {
          errorCode = phaseResult.ErrorCode;
          return;
        }

        var positionResult = await _displacementCalculator.Execute( motionParam, cancelToken );

        if (positionResult.HasError)
        {
          errorCode = positionResult.ErrorCode;
          return;
        }

        var velocityResult = await _velocityCalculator.Execute( motionParam, cancelToken );

        if (velocityResult.HasError)
        {
          errorCode = velocityResult.ErrorCode;
          return;
        }

        var accelerationResult = await _accelerationCalculator.Execute( motionParam, cancelToken );

        if (accelerationResult.HasError)
        {
          errorCode = accelerationResult.ErrorCode;
          return;
        }

        var jerkResult = await _jerkCalculator.Execute( motionParam, cancelToken );

        if (jerkResult.HasError)
        {
          errorCode = jerkResult.ErrorCode;
          return;
        }

        var phase = phaseResult.Data.MotionPhase;
        var position = positionResult.Data.Displacement;
        var velocity = velocityResult.Data.Velocity;
        var acceleration = accelerationResult.Data.Acceleration;
        var jerk = jerkResult.Data.Jerk;

        motionData[i] = new MotionData( time, position, velocity, acceleration, jerk, phase );
        // motionData[i] = new MotionData( time, Position.Zero, velocity, Acceleration.Zero, Jerk.Zero, phase );
      }
    );

    var result = new MotionSimulationResult( profile, motionData );
    return Result.Create( errorCode, result );
  }
}