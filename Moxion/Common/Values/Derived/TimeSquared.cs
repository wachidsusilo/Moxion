using Moxion.Abstractions;

namespace Moxion.Common.Values.Derived;

public readonly record struct TimeSquared( Number Value ) : IValue<TimeSquared>
{
  public bool IsZero => Number.IsZero( Value );
  public bool IsPositive => Number.IsPositive( Value );
  public bool IsNegative => Number.IsNegative( Value );
  public bool IsValid => Value >= Number.Zero;

  public static TimeSquared Zero => new( Number.Zero );

  public Time SquareRoot()
  {
    throw new NotImplementedException();
  }

  #region Operators

  public static TimeCubed operator *( TimeSquared left, Time right )
  {
    return new TimeCubed( left.Value * right.Value );
  }

  public static Time operator /( TimeSquared left, Time right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Time( left.Value / right.Value );
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

  public int CompareTo( TimeSquared other )
  {
    return Value.CompareTo( other.Value );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( TimeSquared other )
  {
    return CompareTo( other ) == 0;
  }

  #endregion

  #region IValue Implementation Operators

  public static bool operator <( TimeSquared left, TimeSquared right )
  {
    return left.CompareTo( right ) < 0;
  }

  public static bool operator >( TimeSquared left, TimeSquared right )
  {
    return left.CompareTo( right ) > 0;
  }

  public static bool operator <=( TimeSquared left, TimeSquared right )
  {
    return left.CompareTo( right ) <= 0;
  }

  public static bool operator >=( TimeSquared left, TimeSquared right )
  {
    return left.CompareTo( right ) >= 0;
  }

  public static TimeSquared operator +( TimeSquared left, TimeSquared right )
  {
    return new TimeSquared( left.Value + right.Value );
  }

  public static TimeSquared operator +( TimeSquared left, Number right )
  {
    return new TimeSquared( left.Value + right );
  }

  public static TimeSquared operator +( Number left, TimeSquared right )
  {
    return new TimeSquared( left + right.Value );
  }

  public static TimeSquared operator -( TimeSquared left, TimeSquared right )
  {
    return new TimeSquared( left.Value - right.Value );
  }

  public static TimeSquared operator -( TimeSquared left, Number right )
  {
    return new TimeSquared( left.Value - right );
  }

  public static TimeSquared operator -( Number left, TimeSquared right )
  {
    return new TimeSquared( left - right.Value );
  }

  public static TimeSquared operator *( TimeSquared left, Number right )
  {
    return new TimeSquared( left.Value * right );
  }

  public static TimeSquared operator *( Number left, TimeSquared right )
  {
    return new TimeSquared( left * right.Value );
  }

  public static Number operator /( TimeSquared left, TimeSquared right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return left.Value / right.Value;
  }

  public static TimeSquared operator /( TimeSquared left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new TimeSquared( left.Value / right );
  }

  public static TimeSquared operator %( TimeSquared left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new TimeSquared( left.Value % right );
  }

  public static TimeSquared operator +( TimeSquared value )
  {
    return value;
  }

  public static TimeSquared operator -( TimeSquared value )
  {
    return new TimeSquared( -value.Value );
  }

  #endregion
}