using Moxion.Abstractions;
using Moxion.Common.Values.Derived;

namespace Moxion.Common.Values;

public readonly record struct Position( Number Value ) : IValue<Position>
{
  public bool IsZero => Number.IsZero( Value );
  public bool IsPositive => Number.IsPositive( Value );
  public bool IsNegative => Number.IsNegative( Value );
  public bool IsValid => true;

  public static Position Zero => new( Number.Zero );

  public Area Squared()
  {
    return new Area( Value * Value );
  }

  public Volume Cubed()
  {
    return new Volume( Value * Value * Value );
  }

  #region Operators

  public static Area operator *( Position left, Position right )
  {
    return new Area( left.Value * right.Value );
  }

  public static Volume operator *( Position left, Area right )
  {
    return new Volume( left.Value * right.Value );
  }

  public static Velocity operator /( Position left, Time right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Velocity( left.Value / right.Value );
  }

  public static Time operator /( Position left, Velocity right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Time( left.Value / right.Value );
  }

  public static TimeSquared operator /( Position left, Acceleration right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new TimeSquared( left.Value / right.Value );
  }

  public static TimeCubed operator /( Position left, Jerk right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new TimeCubed( left.Value / right.Value );
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

  public int CompareTo( Position other )
  {
    return Value.CompareTo( other.Value );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( Position other )
  {
    return CompareTo( other ) == 0;
  }

  #endregion

  #region IValue Implementation Operators

  public static bool operator <( Position left, Position right )
  {
    return left.CompareTo( right ) < 0;
  }

  public static bool operator >( Position left, Position right )
  {
    return left.CompareTo( right ) > 0;
  }

  public static bool operator <=( Position left, Position right )
  {
    return left.CompareTo( right ) <= 0;
  }

  public static bool operator >=( Position left, Position right )
  {
    return left.CompareTo( right ) >= 0;
  }

  public static Position operator +( Position left, Position right )
  {
    return new Position( left.Value + right.Value );
  }

  public static Position operator +( Position left, Number right )
  {
    return new Position( left.Value + right );
  }

  public static Position operator +( Number left, Position right )
  {
    return new Position( left + right.Value );
  }

  public static Position operator -( Position left, Position right )
  {
    return new Position( left.Value - right.Value );
  }

  public static Position operator -( Position left, Number right )
  {
    return new Position( left.Value - right );
  }

  public static Position operator -( Number left, Position right )
  {
    return new Position( left - right.Value );
  }

  public static Position operator *( Position left, Number right )
  {
    return new Position( left.Value * right );
  }

  public static Position operator *( Number left, Position right )
  {
    return new Position( left * right.Value );
  }

  public static Number operator /( Position left, Position right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return left.Value / right.Value;
  }

  public static Position operator /( Position left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Position( left.Value / right );
  }

  public static Position operator %( Position left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Position( left.Value % right );
  }

  public static Position operator +( Position value )
  {
    return value;
  }

  public static Position operator -( Position value )
  {
    return new Position( -value.Value );
  }

  #endregion
}