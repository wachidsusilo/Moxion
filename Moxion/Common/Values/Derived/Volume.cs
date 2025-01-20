using Moxion.Abstractions;

namespace Moxion.Common.Values.Derived;

public readonly record struct Volume( Number Value ) : IValue<Volume>
{
  public bool IsZero => Number.IsZero( Value );
  public bool IsPositive => Number.IsPositive( Value );
  public bool IsNegative => Number.IsNegative( Value );
  public bool IsValid => Value >= Number.Zero;

  public static Volume Zero => new( Number.Zero );

  public Position CubeRoot()
  {
    throw new NotImplementedException();
  }

  #region Operators

  public static Position operator /( Volume left, Area right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Position( left.Value / right.Value );
  }

  public static Area operator /( Volume left, Position right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return new Area( left.Value / right.Value );
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

  public int CompareTo( Volume other )
  {
    return Value.CompareTo( other.Value );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( Volume other )
  {
    return CompareTo( other ) == 0;
  }

  #endregion

  #region IValue Implementation Operators

  public static bool operator <( Volume left, Volume right )
  {
    return left.CompareTo( right ) < 0;
  }

  public static bool operator >( Volume left, Volume right )
  {
    return left.CompareTo( right ) > 0;
  }

  public static bool operator <=( Volume left, Volume right )
  {
    return left.CompareTo( right ) <= 0;
  }

  public static bool operator >=( Volume left, Volume right )
  {
    return left.CompareTo( right ) >= 0;
  }

  public static Volume operator +( Volume left, Volume right )
  {
    return new Volume( left.Value + right.Value );
  }

  public static Volume operator +( Volume left, Number right )
  {
    return new Volume( left.Value + right );
  }

  public static Volume operator +( Number left, Volume right )
  {
    return new Volume( left + right.Value );
  }

  public static Volume operator -( Volume left, Volume right )
  {
    return new Volume( left.Value - right.Value );
  }

  public static Volume operator -( Volume left, Number right )
  {
    return new Volume( left.Value - right );
  }

  public static Volume operator -( Number left, Volume right )
  {
    return new Volume( left - right.Value );
  }

  public static Volume operator *( Volume left, Number right )
  {
    return new Volume( left.Value * right );
  }

  public static Volume operator *( Number left, Volume right )
  {
    return new Volume( left * right.Value );
  }

  public static Number operator /( Volume left, Volume right )
  {
    if (right.IsZero)
    {
      throw new DivideByZeroException();
    }

    return left.Value / right.Value;
  }

  public static Volume operator /( Volume left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Volume( left.Value / right );
  }

  public static Volume operator %( Volume left, Number right )
  {
    if (Number.IsZero( right ))
    {
      throw new DivideByZeroException();
    }

    return new Volume( left.Value % right );
  }

  public static Volume operator +( Volume value )
  {
    return value;
  }

  public static Volume operator -( Volume value )
  {
    return new Volume( -value.Value );
  }

  #endregion
}