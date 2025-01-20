using Moxion.Abstractions;
using Moxion.Common.Values.Derived;

namespace Moxion.Common.Values;

public readonly record struct Time( Number Value ) : IValue<Time>
{
  public bool IsZero => Number.IsZero( Value );
  public bool IsPositive => Number.IsPositive( Value );
  public bool IsNegative => Number.IsNegative( Value );
  public bool IsValid => Value >= Number.Zero;

  public static Time Zero => new( Number.Zero );

  public TimeSquared Squared()
  {
    return new TimeSquared( Value * Value );
  }

  public TimeCubed Cubed()
  {
    return new TimeCubed( Value * Value * Value );
  }

  #region Operators

  public static TimeSquared operator *( Time left, Time right )
  {
    return new TimeSquared( left.Value * right.Value );
  }

  #endregion

  #region IValue Implementation Methods

  public sbyte ToSbyte()
  {
    return Value.ToSbyte();
  }

  public byte ToByte()
  {
    return Value.ToByte();
  }

  public short ToShort()
  {
    return Value.ToShort();
  }

  public ushort ToUShort()
  {
    return Value.ToUShort();
  }

  public int ToInt()
  {
    return Value.ToInt();
  }

  public uint ToUInt()
  {
    return Value.ToUInt();
  }

  public long ToLong()
  {
    return Value.ToLong();
  }

  public ulong ToULong()
  {
    return Value.ToULong();
  }

  public float ToFloat()
  {
    return Value.ToFloat();
  }

  public double ToDouble()
  {
    return Value.ToDouble();
  }

  public decimal ToDecimal()
  {
    return Value.ToDecimal();
  }

  public int CompareTo( Time other )
  {
    return Value.CompareTo( other.Value );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( Time other )
  {
    return CompareTo( other ) == 0;
  }

  #endregion

  #region IValue Implementation Operators

  public static bool operator <( Time left, Time right )
  {
    return left.CompareTo( right ) < 0;
  }

  public static bool operator >( Time left, Time right )
  {
    return left.CompareTo( right ) > 0;
  }

  public static bool operator <=( Time left, Time right )
  {
    return left.CompareTo( right ) <= 0;
  }

  public static bool operator >=( Time left, Time right )
  {
    return left.CompareTo( right ) >= 0;
  }

  public static Time operator +( Time left, Time right )
  {
    return new Time( left.Value + right.Value );
  }

  public static Time operator +( Time left, Number right )
  {
    return new Time( left.Value + right );
  }

  public static Time operator +( Number left, Time right )
  {
    return new Time( left + right.Value );
  }

  public static Time operator -( Time left, Time right )
  {
    return new Time( left.Value - right.Value );
  }

  public static Time operator -( Time left, Number right )
  {
    return new Time( left.Value - right );
  }

  public static Time operator -( Number left, Time right )
  {
    return new Time( left - right.Value );
  }

  public static Time operator *( Time left, Number right )
  {
    return new Time( left.Value * right );
  }

  public static Time operator *( Number left, Time right )
  {
    return new Time( left * right.Value );
  }

  public static Number operator /( Time left, Time right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return left.Value / right.Value;
  }

  public static Time operator /( Time left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Time( left.Value / right );
  }

  public static Time operator %( Time left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Time( left.Value % right );
  }

  public static Time operator +( Time value )
  {
    return value;
  }

  public static Time operator -( Time value )
  {
    return new Time( -value.Value );
  }

  #endregion
}