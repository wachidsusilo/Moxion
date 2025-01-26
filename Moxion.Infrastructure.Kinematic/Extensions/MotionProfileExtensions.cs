using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;
using Moxion.Domain.Kinematic;
using Moxion.Extensions;

namespace Moxion.Infrastructure.Kinematic.Extensions;

public static class MotionProfileExtensions
{
  public static MotionPhase CalculatePhase( this MotionProfile profile, Time time )
  {
    if (time.IsNegative || time > profile.GetTotalDuration())
    {
      return MotionPhase.None;
    }

    if (time <= profile.TimeProfile.CalculateTotalDuration( MotionPhase.AccelerationWithPositiveJerk ))
    {
      return MotionPhase.AccelerationWithPositiveJerk;
    }

    if (time <= profile.TimeProfile.CalculateTotalDuration( MotionPhase.ConstantAcceleration ))
    {
      return MotionPhase.ConstantAcceleration;
    }

    if (time <= profile.TimeProfile.CalculateTotalDuration( MotionPhase.AccelerationWithNegativeJerk ))
    {
      return MotionPhase.AccelerationWithNegativeJerk;
    }

    if (time <= profile.TimeProfile.CalculateTotalDuration( MotionPhase.ConstantVelocity ))
    {
      return MotionPhase.ConstantVelocity;
    }

    if (time <= profile.TimeProfile.CalculateTotalDuration( MotionPhase.DecelerationWithNegativeJerk ))
    {
      return MotionPhase.DecelerationWithNegativeJerk;
    }

    return time <= profile.TimeProfile.CalculateTotalDuration( MotionPhase.ConstantDeceleration )
      ? MotionPhase.ConstantDeceleration
      : MotionPhase.DecelerationWithPositiveJerk;
  }

  public static Jerk CalculateJerk( this MotionProfile profile, MotionPhase phase )
  {
    return phase switch
    {
      MotionPhase.AccelerationWithPositiveJerk or MotionPhase.DecelerationWithPositiveJerk => profile.Jerk,
      MotionPhase.AccelerationWithNegativeJerk or MotionPhase.DecelerationWithNegativeJerk => -profile.Jerk,
      _ => Jerk.Zero
    };
  }
}