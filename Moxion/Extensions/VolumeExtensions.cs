using Moxion.Common.Units;

namespace Moxion.Extensions;

public static class VolumeExtensions
{
  public static bool IsValid( this VolumeUnit unit )
  {
    return Enum.IsDefined( unit ) && unit != VolumeUnit.None;
  }
}