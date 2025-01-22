using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Calculators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Common.Values.Derived;

namespace Moxion.Infrastructure.Kinematic.Calculators;

internal class MotionJerkCalculator : IMotionJerkCalculator
{
  private readonly IMotionPhaseCalculator _motionPhaseCalculator;

  public MotionJerkCalculator( IMotionPhaseCalculator motionPhaseCalculator )
  {
    _motionPhaseCalculator = motionPhaseCalculator;
  }

  public async Task<Result<MotionJerkCalculationResult>> Execute(
    MotionCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Result.Error<MotionJerkCalculationResult>( ErrorCode.OperationCancelled );
    }

    var phaseResult = await _motionPhaseCalculator.Execute( param, cancellationToken );

    if (phaseResult.HasError)
    {
      return Result.Error<MotionJerkCalculationResult>( phaseResult.ErrorCode );
    }

    return Result.Success(
      new MotionJerkCalculationResult(
        phaseResult.Data.MotionPhase switch
        {
          MotionPhase.AccelerationWithPositiveJerk or MotionPhase.DecelerationWithPositiveJerk => param.Profile.Jerk,
          MotionPhase.AccelerationWithNegativeJerk or MotionPhase.DecelerationWithNegativeJerk => -param.Profile.Jerk,
          _ => Jerk.Zero
        }
      )
    );
  }
}