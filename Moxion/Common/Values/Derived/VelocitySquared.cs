using Moxion.Abstractions;

namespace Moxion.Common.Values.Derived;

public readonly record struct VelocitySquared(Number Value): IValue<VelocitySquared>
{
  public bool IsZero => Number.IsZero( Value );
  public bool IsPositive => Number.IsPositive( Value );
  public bool IsNegative => Number.IsNegative( Value );
  public bool IsValid => Value >= Number.Zero;

  public static VelocitySquared Zero => new( Number.Zero );

  public Velocity SquareRoot()
  {
    return new Velocity( Value.SquareRoot() );
  }

  #region Operators

  public static Velocity operator /( VelocitySquared left, Velocity right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Velocity( left.Value / right.Value );
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

  public int CompareTo( VelocitySquared other )
  {
    return Value.CompareTo( other.Value );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( VelocitySquared other )
  {
    return CompareTo( other ) == 0;
  }

  #endregion

  #region IValue Implementation Operators

  public static bool operator <( VelocitySquared left, VelocitySquared right )
  {
    return left.CompareTo( right ) < 0;
  }

  public static bool operator >( VelocitySquared left, VelocitySquared right )
  {
    return left.CompareTo( right ) > 0;
  }

  public static bool operator <=( VelocitySquared left, VelocitySquared right )
  {
    return left.CompareTo( right ) <= 0;
  }

  public static bool operator >=( VelocitySquared left, VelocitySquared right )
  {
    return left.CompareTo( right ) >= 0;
  }

  public static VelocitySquared operator +( VelocitySquared left, VelocitySquared right )
  {
    return new VelocitySquared( left.Value + right.Value );
  }

  public static VelocitySquared operator +( VelocitySquared left, Number right )
  {
    return new VelocitySquared( left.Value + right );
  }

  public static VelocitySquared operator +( Number left, VelocitySquared right )
  {
    return new VelocitySquared( left + right.Value );
  }

  public static VelocitySquared operator -( VelocitySquared left, VelocitySquared right )
  {
    return new VelocitySquared( left.Value - right.Value );
  }

  public static VelocitySquared operator -( VelocitySquared left, Number right )
  {
    return new VelocitySquared( left.Value - right );
  }

  public static VelocitySquared operator -( Number left, VelocitySquared right )
  {
    return new VelocitySquared( left - right.Value );
  }

  public static VelocitySquared operator *( VelocitySquared left, Number right )
  {
    return new VelocitySquared( left.Value * right );
  }

  public static VelocitySquared operator *( Number left, VelocitySquared right )
  {
    return new VelocitySquared( left * right.Value );
  }

  public static Number operator /( VelocitySquared left, VelocitySquared right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return left.Value / right.Value;
  }

  public static VelocitySquared operator /( VelocitySquared left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new VelocitySquared( left.Value / right );
  }

  public static VelocitySquared operator %( VelocitySquared left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new VelocitySquared( left.Value % right );
  }

  public static VelocitySquared operator +( VelocitySquared value )
  {
    return value;
  }

  public static VelocitySquared operator -( VelocitySquared value )
  {
    return new VelocitySquared( -value.Value );
  }

  #endregion
}