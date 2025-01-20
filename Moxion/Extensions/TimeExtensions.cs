using Moxion.Common.Units;

namespace Moxion.Extensions;

public static class TimeExtensions
{
  public static bool IsValid( this TimeUnit unit )
  {
    return Enum.IsDefined( unit ) && unit != TimeUnit.None;
  }
}