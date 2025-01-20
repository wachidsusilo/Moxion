namespace Moxion.Common.Enumerations;

public enum MotionPhase
{
  None,
  AccelerationWithPositiveJerk,
  ConstantAcceleration,
  AccelerationWithNegativeJerk,
  ConstantVelocity,
  DecelerationWithNegativeJerk,
  ConstantDeceleration,
  DecelerationWithPositiveJerk,
}
