using Moxion.Abstractions;

namespace Moxion.Common.Values.Derived;

public readonly record struct Jerk( Number Value ) : IValue<Jerk>
{
  public bool IsZero => Number.IsZero( Value );
  public bool IsPositive => Number.IsPositive( Value );
  public bool IsNegative => Number.IsNegative( Value );
  public bool IsValid => Value >= Number.Zero;

  public static Jerk Zero => new( Number.Zero );

  #region Operators

  public static Position operator *( Jerk left, TimeCubed right )
  {
    return new Position( left.Value * right.Value );
  }

  public static Velocity operator *( Jerk left, TimeSquared right )
  {
    return new Velocity( left.Value * right.Value );
  }

  public static Acceleration operator *( Jerk left, Time right )
  {
    return new Acceleration( left.Value * right.Value );
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

  public int CompareTo( Jerk other )
  {
    return Value.CompareTo( other.Value );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( Jerk other )
  {
    return CompareTo( other ) == 0;
  }

  #endregion

  #region IValue Implementation Operators

  public static bool operator <( Jerk left, Jerk right )
  {
    return left.CompareTo( right ) < 0;
  }

  public static bool operator >( Jerk left, Jerk right )
  {
    return left.CompareTo( right ) > 0;
  }

  public static bool operator <=( Jerk left, Jerk right )
  {
    return left.CompareTo( right ) <= 0;
  }

  public static bool operator >=( Jerk left, Jerk right )
  {
    return left.CompareTo( right ) >= 0;
  }

  public static Jerk operator +( Jerk left, Jerk right )
  {
    return new Jerk( left.Value + right.Value );
  }

  public static Jerk operator +( Jerk left, Number right )
  {
    return new Jerk( left.Value + right );
  }

  public static Jerk operator +( Number left, Jerk right )
  {
    return new Jerk( left + right.Value );
  }

  public static Jerk operator -( Jerk left, Jerk right )
  {
    return new Jerk( left.Value - right.Value );
  }

  public static Jerk operator -( Jerk left, Number right )
  {
    return new Jerk( left.Value - right );
  }

  public static Jerk operator -( Number left, Jerk right )
  {
    return new Jerk( left - right.Value );
  }

  public static Jerk operator *( Jerk left, Number right )
  {
    return new Jerk( left.Value * right );
  }

  public static Jerk operator *( Number left, Jerk right )
  {
    return new Jerk( left * right.Value );
  }

  public static Number operator /( Jerk left, Jerk right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return left.Value / right.Value;
  }

  public static Jerk operator /( Jerk left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Jerk( left.Value / right );
  }

  public static Jerk operator %( Jerk left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Jerk( left.Value % right );
  }

  public static Jerk operator +( Jerk value )
  {
    return value;
  }

  public static Jerk operator -( Jerk value )
  {
    return new Jerk( -value.Value );
  }

  #endregion
}