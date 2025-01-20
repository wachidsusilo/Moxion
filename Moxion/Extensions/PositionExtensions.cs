using Moxion.Common.Units;

namespace Moxion.Extensions;

public static class PositionExtensions
{
  public static bool IsValid( this PositionUnit unit )
  {
    return Enum.IsDefined( unit ) && unit != PositionUnit.None;
  }
}