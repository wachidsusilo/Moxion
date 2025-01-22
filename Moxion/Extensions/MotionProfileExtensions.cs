using Moxion.Common.Enumerations;
using Moxion.Common.Values;
using Moxion.Domain.Kinematic;

namespace Moxion.Extensions;

public static class MotionProfileExtensions
{
  public static Time GetTotalDuration( this MotionProfile profile )
  {
    return 4 * profile.JerkDuration + 2 * profile.AccelerationDuration + profile.SteadyMotionDuration;
  }

  public static MotionProfileType GetProfileType( this MotionProfile profile )
  {
    var hasJerk = !profile.JerkDuration.IsZero;
    var hasAcceleration = !profile.AccelerationDuration.IsZero;
    var hasSteadyMotion = !profile.SteadyMotionDuration.IsZero;

    if (hasSteadyMotion && !hasAcceleration && !hasJerk)
    {
      return MotionProfileType.Linear;
    }

    if (hasSteadyMotion && hasAcceleration && !hasJerk)
    {
      return MotionProfileType.Trapezoid;
    }

    if (!hasSteadyMotion && hasAcceleration && !hasJerk)
    {
      return MotionProfileType.Triangular;
    }

    if (!hasSteadyMotion && !hasAcceleration && hasJerk)
    {
      return MotionProfileType.JerkOnly;
    }

    if (hasSteadyMotion && !hasAcceleration && hasJerk)
    {
      return MotionProfileType.JerkWithSteadyState;
    }

    if (hasSteadyMotion && hasAcceleration && hasJerk)
    {
      return MotionProfileType.SCurve;
    }

    if (!hasSteadyMotion && hasAcceleration && hasJerk)
    {
      // This is not possible
      // A cubic motion without steady motion phase will not have acceleration phase
    }

    return MotionProfileType.None;
  }
}