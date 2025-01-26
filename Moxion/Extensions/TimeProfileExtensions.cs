using Moxion.Common.Values;
using Moxion.Domain.Kinematic;

namespace Moxion.Extensions;

public static class TimeProfileExtensions
{
  public static Time GetTotalDuration( this TimeProfile profile )
  {
    return 4 * profile.JerkDuration + 2 * profile.ConstantAccelerationDuration + profile.ConstantVelocityDuration;
  }
}