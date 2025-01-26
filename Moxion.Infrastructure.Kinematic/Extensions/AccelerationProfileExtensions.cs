using Moxion.Common.Enumerations;
using Moxion.Common.Values.Derived;
using Moxion.Domain.Kinematic;

namespace Moxion.Infrastructure.Kinematic.Extensions;

public static class AccelerationProfileExtensions
{
  public static Acceleration CalculateAcceleration( this AccelerationProfile profile, MotionPhase phase )
  {
    return phase switch
    {
      MotionPhase.AccelerationWithPositiveJerk => profile.PositiveJerkMaxAcceleration,
      MotionPhase.ConstantAcceleration => profile.PositiveJerkMaxAcceleration,
      MotionPhase.AccelerationWithNegativeJerk or MotionPhase.ConstantVelocity => Acceleration.Zero,
      MotionPhase.DecelerationWithNegativeJerk => -profile.PositiveJerkMaxAcceleration,
      MotionPhase.ConstantDeceleration => -profile.PositiveJerkMaxAcceleration,
      _ => Acceleration.Zero
    };
  }
}