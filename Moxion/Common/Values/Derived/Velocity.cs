using Moxion.Abstractions;

namespace Moxion.Common.Values.Derived;

public readonly record struct Velocity( Number Value ) : IValue<Velocity>
{
  public bool IsZero => Number.IsZero( Value );
  public bool IsPositive => Number.IsPositive( Value );
  public bool IsNegative => Number.IsNegative( Value );
  public bool IsValid => Value >= Number.Zero;

  public static Velocity Zero => new( Number.Zero );

  #region Operators

  public static Position operator *( Velocity left, Time right )
  {
    return new Position( left.Value * right.Value );
  }

  public static Time operator /( Velocity left, Acceleration right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Time( left.Value / right.Value );
  }

  public static TimeSquared operator /( Velocity left, Jerk right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new TimeSquared( left.Value / right.Value );
  }

  public static Acceleration operator /( Velocity left, Time right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Acceleration( left.Value / right.Value );
  }

  public static Jerk operator /( Velocity left, TimeSquared right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Jerk( left.Value / right.Value );
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

  public int CompareTo( Velocity other )
  {
    return Value.CompareTo( other.Value );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( Velocity other )
  {
    return CompareTo( other ) == 0;
  }

  #endregion

  #region IValue Implementation Operators

  public static bool operator <( Velocity left, Velocity right )
  {
    return left.CompareTo( right ) < 0;
  }

  public static bool operator >( Velocity left, Velocity right )
  {
    return left.CompareTo( right ) > 0;
  }

  public static bool operator <=( Velocity left, Velocity right )
  {
    return left.CompareTo( right ) <= 0;
  }

  public static bool operator >=( Velocity left, Velocity right )
  {
    return left.CompareTo( right ) >= 0;
  }

  public static Velocity operator +( Velocity left, Velocity right )
  {
    return new Velocity( left.Value + right.Value );
  }

  public static Velocity operator +( Velocity left, Number right )
  {
    return new Velocity( left.Value + right );
  }

  public static Velocity operator +( Number left, Velocity right )
  {
    return new Velocity( left + right.Value );
  }

  public static Velocity operator -( Velocity left, Velocity right )
  {
    return new Velocity( left.Value - right.Value );
  }

  public static Velocity operator -( Velocity left, Number right )
  {
    return new Velocity( left.Value - right );
  }

  public static Velocity operator -( Number left, Velocity right )
  {
    return new Velocity( left - right.Value );
  }

  public static Velocity operator *( Velocity left, Number right )
  {
    return new Velocity( left.Value * right );
  }

  public static Velocity operator *( Number left, Velocity right )
  {
    return new Velocity( left * right.Value );
  }

  public static Number operator /( Velocity left, Velocity right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return left.Value / right.Value;
  }

  public static Velocity operator /( Velocity left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Velocity( left.Value / right );
  }

  public static Velocity operator %( Velocity left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Velocity( left.Value % right );
  }

  public static Velocity operator +( Velocity value )
  {
    return value;
  }

  public static Velocity operator -( Velocity value )
  {
    return new Velocity( -value.Value );
  }

  #endregion
}