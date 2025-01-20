using Moxion.Abstractions;

namespace Moxion.Common.Values.Derived;

public readonly record struct Area( Number Value ) : IValue<Area>
{
  public bool IsZero => Number.IsZero( Value );
  public bool IsPositive => Number.IsPositive( Value );
  public bool IsNegative => Number.IsNegative( Value );
  public bool IsValid => Value >= Number.Zero;

  public static Area Zero => new( Number.Zero );

  public Position SquareRoot()
  {
    throw new NotImplementedException();
  }

  #region Operators

  public static Volume operator *( Area left, Position right )
  {
    return new Volume( left.Value * right.Value );
  }

  public static Position operator /( Area left, Position right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Position( left.Value / right.Value );
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

  public int CompareTo( Area other )
  {
    return Value.CompareTo( other.Value );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( Area other )
  {
    return CompareTo( other ) == 0;
  }

  #endregion

  #region IValue Implementation Operators

  public static bool operator <( Area left, Area right )
  {
    return left.CompareTo( right ) < 0;
  }

  public static bool operator >( Area left, Area right )
  {
    return left.CompareTo( right ) > 0;
  }

  public static bool operator <=( Area left, Area right )
  {
    return left.CompareTo( right ) <= 0;
  }

  public static bool operator >=( Area left, Area right )
  {
    return left.CompareTo( right ) >= 0;
  }

  public static Area operator +( Area left, Area right )
  {
    return new Area( left.Value + right.Value );
  }

  public static Area operator +( Area left, Number right )
  {
    return new Area( left.Value + right );
  }

  public static Area operator +( Number left, Area right )
  {
    return new Area( left + right.Value );
  }

  public static Area operator -( Area left, Area right )
  {
    return new Area( left.Value - right.Value );
  }

  public static Area operator -( Area left, Number right )
  {
    return new Area( left.Value - right );
  }

  public static Area operator -( Number left, Area right )
  {
    return new Area( left - right.Value );
  }

  public static Area operator *( Area left, Number right )
  {
    return new Area( left.Value * right );
  }

  public static Area operator *( Number left, Area right )
  {
    return new Area( left * right.Value );
  }

  public static Number operator /( Area left, Area right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return left.Value / right.Value;
  }

  public static Area operator /( Area left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Area( left.Value / right );
  }

  public static Area operator %( Area left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Area( left.Value % right );
  }

  public static Area operator +( Area value )
  {
    return value;
  }

  public static Area operator -( Area value )
  {
    return new Area( -value.Value );
  }

  #endregion
}