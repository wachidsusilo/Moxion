using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Calculators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Extensions;
using Moxion.Infrastructure.Kinematic.Extensions;

namespace Moxion.Infrastructure.Kinematic.Calculators;

internal class MotionPhaseCalculator : IMotionPhaseCalculator
{
  public Task<Result<MotionPhaseCalculationResult>> Execute(
    MotionPhaseCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionPhaseCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    var profileType = param.Profile.GetProfileType();

    if (param.Time.IsNegative)
    {
      return Task.FromResult( Result.Error<MotionPhaseCalculationResult>( ErrorCode.InvalidTime ) );
    }

    if (profileType is MotionProfileType.Linear)
    {
      return Task.FromResult(
        Result.Success(
          new MotionPhaseCalculationResult( MotionPhase.ConstantVelocity )
        )
      );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionPhaseCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (profileType is MotionProfileType.Triangular)
    {
      return Task.FromResult(
        Result.Success(
          new MotionPhaseCalculationResult(
            param.Time <= param.Profile.CalculateTotalDuration( MotionPhase.ConstantAcceleration )
              ? MotionPhase.ConstantAcceleration
              : MotionPhase.ConstantDeceleration
          )
        )
      );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionPhaseCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (profileType is MotionProfileType.Trapezoid)
    {
      return Task.FromResult(
        Result.Success(
          new MotionPhaseCalculationResult(
            param.Time <= param.Profile.CalculateTotalDuration( MotionPhase.ConstantAcceleration )
              ? MotionPhase.ConstantAcceleration
              : param.Time <= param.Profile.CalculateTotalDuration( MotionPhase.ConstantVelocity )
                ? MotionPhase.ConstantVelocity
                : MotionPhase.ConstantDeceleration
          )
        )
      );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionPhaseCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Time <= param.Profile.CalculateTotalDuration( MotionPhase.AccelerationWithPositiveJerk ))
    {
      return Task.FromResult(
        Result.Success(
          new MotionPhaseCalculationResult( MotionPhase.AccelerationWithPositiveJerk )
        )
      );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionPhaseCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Time <= param.Profile.CalculateTotalDuration( MotionPhase.ConstantAcceleration ))
    {
      return Task.FromResult(
        Result.Success(
          new MotionPhaseCalculationResult( MotionPhase.ConstantAcceleration )
        )
      );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionPhaseCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Time <= param.Profile.CalculateTotalDuration( MotionPhase.AccelerationWithNegativeJerk ))
    {
      return Task.FromResult(
        Result.Success(
          new MotionPhaseCalculationResult( MotionPhase.AccelerationWithNegativeJerk )
        )
      );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionPhaseCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Time <= param.Profile.CalculateTotalDuration( MotionPhase.ConstantVelocity ))
    {
      return Task.FromResult(
        Result.Success(
          new MotionPhaseCalculationResult( MotionPhase.ConstantVelocity )
        )
      );
    }

    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionPhaseCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    if (param.Time <= param.Profile.CalculateTotalDuration( MotionPhase.DecelerationWithNegativeJerk ))
    {
      return Task.FromResult(
        Result.Success(
          new MotionPhaseCalculationResult( MotionPhase.DecelerationWithNegativeJerk )
        )
      );
    }

    return Task.FromResult(
      Result.Success(
        new MotionPhaseCalculationResult(
          param.Time <= param.Profile.CalculateTotalDuration( MotionPhase.ConstantDeceleration )
            ? MotionPhase.ConstantDeceleration
            : MotionPhase.DecelerationWithPositiveJerk
        )
      )
    );
  }
}