using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Domain.Kinematic;

namespace Moxion.Infrastructure.Kinematic.Extensions;

public static class TimeProfileExtensions
{
  public static Time CalculateDuration( this TimeProfile profile, Time time, MotionPhase phase )
  {
    return phase switch
    {
      MotionPhase.AccelerationWithPositiveJerk => time,
      MotionPhase.ConstantAcceleration =>
        time - CalculateTotalDuration( profile, MotionPhase.AccelerationWithPositiveJerk ),
      MotionPhase.AccelerationWithNegativeJerk =>
        time - CalculateTotalDuration( profile, MotionPhase.ConstantAcceleration ),
      MotionPhase.ConstantVelocity =>
        time - CalculateTotalDuration( profile, MotionPhase.AccelerationWithNegativeJerk ),
      MotionPhase.DecelerationWithNegativeJerk =>
        time - CalculateTotalDuration( profile, MotionPhase.ConstantVelocity ),
      MotionPhase.ConstantDeceleration =>
        time - CalculateTotalDuration( profile, MotionPhase.DecelerationWithNegativeJerk ),
      _ => time - CalculateTotalDuration( profile, MotionPhase.ConstantDeceleration ),
    };
  }

  public static Time CalculateTotalDuration( this TimeProfile profile, MotionPhase phase )
  {
    return phase switch
    {
      MotionPhase.AccelerationWithPositiveJerk => profile.JerkDuration,
      MotionPhase.ConstantAcceleration => profile.JerkDuration + profile.ConstantAccelerationDuration,
      MotionPhase.AccelerationWithNegativeJerk => 2 * profile.JerkDuration + profile.ConstantAccelerationDuration,
      MotionPhase.ConstantVelocity => 2 * profile.JerkDuration
                                      + profile.ConstantAccelerationDuration
                                      + profile.ConstantVelocityDuration,
      MotionPhase.DecelerationWithNegativeJerk => 3 * profile.JerkDuration
                                                  + profile.ConstantAccelerationDuration
                                                  + profile.ConstantVelocityDuration,
      MotionPhase.ConstantDeceleration => 3 * profile.JerkDuration
                                          + 2 * profile.ConstantAccelerationDuration
                                          + profile.ConstantVelocityDuration,
      _ => 4 * profile.JerkDuration
           + 2 * profile.ConstantAccelerationDuration
           + profile.ConstantVelocityDuration,
    };
  }
}