using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Domain.Kinematic;

namespace Moxion.Extensions;

public static class MotionProfileExtensions
{
  public static Time GetTotalDuration( this MotionProfile profile )
  {
    return profile.TimeProfile.GetTotalDuration();
  }

  public static MotionProfileType GetProfileType( this MotionProfile profile )
  {
    var hasJerk = !profile.TimeProfile.JerkDuration.IsZero;
    var hasAcceleration = !profile.TimeProfile.ConstantAccelerationDuration.IsZero;
    var hasSteadyMotion = !profile.TimeProfile.ConstantVelocityDuration.IsZero;

    if (hasSteadyMotion && !hasAcceleration && !hasJerk)
    {
      return MotionProfileType.Linear;
    }

    if (hasSteadyMotion && hasAcceleration && !hasJerk)
    {
      return MotionProfileType.Trapezoidal;
    }

    if (!hasSteadyMotion && hasAcceleration && !hasJerk)
    {
      return MotionProfileType.Triangular;
    }

    if (!hasSteadyMotion && !hasAcceleration && hasJerk)
    {
      return MotionProfileType.JerkDriven;
    }

    if (hasSteadyMotion && !hasAcceleration && hasJerk)
    {
      return MotionProfileType.JerkWithConstantVelocity;
    }

    if (hasSteadyMotion && hasAcceleration && hasJerk)
    {
      return MotionProfileType.SCurve;
    }

    if (!hasSteadyMotion && hasAcceleration && hasJerk)
    {
      return MotionProfileType.JerkWithConstantAcceleration;
    }

    return MotionProfileType.None;
  }
}