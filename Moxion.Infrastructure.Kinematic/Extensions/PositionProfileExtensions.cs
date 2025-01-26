using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Domain.Kinematic;

namespace Moxion.Infrastructure.Kinematic.Extensions;

public static class PositionProfileExtensions
{
  public static Position CalculateTotalDisplacement( this PositionProfile profile, MotionPhase phase )
  {
    return phase switch
    {
      MotionPhase.AccelerationWithPositiveJerk => profile.PositiveJerkDisplacement,
      MotionPhase.ConstantAcceleration => profile.PositiveJerkDisplacement + profile.ConstantAccelerationDisplacement,
      MotionPhase.AccelerationWithNegativeJerk => profile.PositiveJerkDisplacement
                                                  + profile.ConstantAccelerationDisplacement
                                                  + profile.NegativeJerkDisplacement,
      MotionPhase.ConstantVelocity => profile.PositiveJerkDisplacement
                                      + profile.ConstantAccelerationDisplacement
                                      + profile.NegativeJerkDisplacement
                                      + profile.ConstantVelocityDisplacement,
      MotionPhase.DecelerationWithNegativeJerk => profile.PositiveJerkDisplacement
                                                  + profile.ConstantAccelerationDisplacement
                                                  + 2 * profile.NegativeJerkDisplacement
                                                  + profile.ConstantVelocityDisplacement,
      MotionPhase.ConstantDeceleration => profile.PositiveJerkDisplacement
                                          + 2 * profile.ConstantAccelerationDisplacement
                                          + 2 * profile.NegativeJerkDisplacement
                                          + profile.ConstantVelocityDisplacement,
      _ => 2 * profile.PositiveJerkDisplacement
           + 2 * profile.ConstantAccelerationDisplacement
           + 2 * profile.NegativeJerkDisplacement
           + profile.ConstantVelocityDisplacement
    };
  }
}