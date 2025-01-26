using Moxion.Common.Enumerations;
using Moxion.Common.Values.Derived;
using Moxion.Domain.Kinematic;

namespace Moxion.Infrastructure.Kinematic.Extensions;

public static class VelocityProfileExtensions
{
  public static Velocity CalculateVelocity( this VelocityProfile profile, MotionPhase phase )
  {
    return phase switch
    {
      MotionPhase.AccelerationWithPositiveJerk => profile.PositiveJerkMaxVelocity,
      MotionPhase.ConstantAcceleration => profile.ConstantAccelerationMaxVelocity,
      MotionPhase.AccelerationWithNegativeJerk or MotionPhase.ConstantVelocity => profile.NegativeJerkMaxVelocity,
      MotionPhase.DecelerationWithNegativeJerk => profile.NegativeJerkMaxVelocity - profile.PositiveJerkMaxVelocity,
      MotionPhase.ConstantDeceleration => profile.NegativeJerkMaxVelocity
                                          - profile.ConstantAccelerationMaxVelocity,
      _ => Velocity.Zero
    };
  }
}