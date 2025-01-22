using Moxion.Abstractions;

namespace Moxion.Common.Values.Derived;

public readonly record struct TimeCubed( Number Value ) : IValue<TimeCubed>
{
  public bool IsZero => Number.IsZero( Value );
  public bool IsPositive => Number.IsPositive( Value );
  public bool IsNegative => Number.IsNegative( Value );
  public bool IsValid => Value >= Number.Zero;

  public static TimeCubed Zero => new( Number.Zero );

  public Time CubeRoot()
  {
    return new Time( Value.Pow( 1m / 3m ) );
  }

  #region Operators

  public static Time operator /( TimeCubed left, TimeSquared right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Time( left.Value / right.Value );
  }

  public static TimeSquared operator /( TimeCubed left, Time right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new TimeSquared( left.Value / right.Value );
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

  public int CompareTo( TimeCubed other )
  {
    return Value.CompareTo( other.Value );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( TimeCubed other )
  {
    return CompareTo( other ) == 0;
  }

  #endregion

  #region IValue Implementation Operators

  public static bool operator <( TimeCubed left, TimeCubed right )
  {
    return left.CompareTo( right ) < 0;
  }

  public static bool operator >( TimeCubed left, TimeCubed right )
  {
    return left.CompareTo( right ) > 0;
  }

  public static bool operator <=( TimeCubed left, TimeCubed right )
  {
    return left.CompareTo( right ) <= 0;
  }

  public static bool operator >=( TimeCubed left, TimeCubed right )
  {
    return left.CompareTo( right ) >= 0;
  }

  public static TimeCubed operator +( TimeCubed left, TimeCubed right )
  {
    return new TimeCubed( left.Value + right.Value );
  }

  public static TimeCubed operator +( TimeCubed left, Number right )
  {
    return new TimeCubed( left.Value + right );
  }

  public static TimeCubed operator +( Number left, TimeCubed right )
  {
    return new TimeCubed( left + right.Value );
  }

  public static TimeCubed operator -( TimeCubed left, TimeCubed right )
  {
    return new TimeCubed( left.Value - right.Value );
  }

  public static TimeCubed operator -( TimeCubed left, Number right )
  {
    return new TimeCubed( left.Value - right );
  }

  public static TimeCubed operator -( Number left, TimeCubed right )
  {
    return new TimeCubed( left - right.Value );
  }

  public static TimeCubed operator *( TimeCubed left, Number right )
  {
    return new TimeCubed( left.Value * right );
  }

  public static TimeCubed operator *( Number left, TimeCubed right )
  {
    return new TimeCubed( left * right.Value );
  }

  public static Number operator /( TimeCubed left, TimeCubed right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return left.Value / right.Value;
  }

  public static TimeCubed operator /( TimeCubed left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new TimeCubed( left.Value / right );
  }

  public static TimeCubed operator %( TimeCubed left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new TimeCubed( left.Value % right );
  }

  public static TimeCubed operator +( TimeCubed value )
  {
    return value;
  }

  public static TimeCubed operator -( TimeCubed value )
  {
    return new TimeCubed( -value.Value );
  }

  #endregion
}