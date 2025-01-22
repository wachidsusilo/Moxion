using Moxion.Application.Abstractions.Calculators.Kinematic;
using Moxion.Application.Shared.Calculators.Params;
using Moxion.Application.Shared.Calculators.Results;
using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Common.Values.Derived;

namespace Moxion.Infrastructure.Kinematic.Calculators;

internal class MotionJerkCalculator : IMotionJerkCalculator
{
  public Task<Result<MotionJerkCalculationResult>> Execute(
    MotionJerkCalculationParam param,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested)
    {
      return Task.FromResult( Result.Error<MotionJerkCalculationResult>( ErrorCode.OperationCancelled ) );
    }

    return Task.FromResult(
      Result.Success(
        new MotionJerkCalculationResult(
          param.Phase switch
          {
            MotionPhase.AccelerationWithPositiveJerk or MotionPhase.DecelerationWithPositiveJerk => param.Profile.Jerk,
            MotionPhase.AccelerationWithNegativeJerk or MotionPhase.DecelerationWithNegativeJerk => -param.Profile.Jerk,
            _ => Jerk.Zero
          }
        )
      )
    );
  }
}