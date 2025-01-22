using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Moxion.Common.Values;

public readonly record struct Number( decimal Value ) : INumber<Number>
{
  public static int Radix => 10;
  public static Number One => new( 1 );
  public static Number Zero => new( 0 );
  public static Number AdditiveIdentity => new( 0 );
  public static Number MultiplicativeIdentity => new( 1 );

  public sbyte ToSbyte()
  {
    return Convert.ToSByte( Value );
  }

  public byte ToByte()
  {
    return Convert.ToByte( Value );
  }

  public short ToShort()
  {
    return Convert.ToInt16( Value );
  }

  public ushort ToUShort()
  {
    return Convert.ToUInt16( Value );
  }

  public int ToInt()
  {
    return Convert.ToInt32( Value );
  }

  public uint ToUInt()
  {
    return Convert.ToUInt32( Value );
  }

  public long ToLong()
  {
    return Convert.ToInt64( Value );
  }

  public ulong ToULong()
  {
    return Convert.ToUInt64( Value );
  }

  public float ToFloat()
  {
    return Convert.ToSingle( Value );
  }

  public double ToDouble()
  {
    return Convert.ToDouble( Value );
  }

  public decimal ToDecimal()
  {
    return Value;
  }

  public Number Sqrt()
  {
    // TODO: _ws Find better implementation
    return Math.Sqrt( ToDouble() );
  }

  public Number Pow( Number exponent )
  {
    // TODO: _ws Find better implementation
    return Math.Pow( ToDouble(), exponent.ToDouble() );
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public bool Equals( Number other )
  {
    return Value.Equals( other.Value );
  }

  public int CompareTo( object? obj )
  {
    if (obj is not Number other)
    {
      throw new ArgumentException( $"Object must be of type {nameof(Number)}." );
    }

    return Value.CompareTo( other.Value );
  }

  public int CompareTo( Number other )
  {
    return Value.CompareTo( other.Value );
  }

  public string ToString( string? format, IFormatProvider? formatProvider )
  {
    return Value.ToString( format, formatProvider );
  }

  public static Number Parse( string s, IFormatProvider? provider )
  {
    return new Number( decimal.Parse( s, provider ) );
  }

  public static Number Parse( string s, NumberStyles style, IFormatProvider? provider )
  {
    return new Number( decimal.Parse( s, style, provider ) );
  }

  public static Number Parse( ReadOnlySpan<char> s, IFormatProvider? provider )
  {
    return new Number( decimal.Parse( s, provider ) );
  }

  public static Number Parse( ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider )
  {
    return new Number( decimal.Parse( s, style, provider ) );
  }

  public static bool TryParse( [NotNullWhen( true )] string? s, IFormatProvider? provider, out Number result )
  {
    if (!decimal.TryParse( s, NumberStyles.Number, provider, out var resultValue ))
    {
      result = default;
      return false;
    }

    result = new Number( resultValue );
    return true;
  }

  public static bool TryParse(
    [NotNullWhen( true )] string? s,
    NumberStyles style,
    IFormatProvider? provider,
    out Number result
  )
  {
    if (!decimal.TryParse( s, style, provider, out var resultValue ))
    {
      result = default;
      return false;
    }

    result = new Number( resultValue );
    return true;
  }

  public static bool TryParse( ReadOnlySpan<char> s, IFormatProvider? provider, out Number result )
  {
    if (!decimal.TryParse( s, NumberStyles.Number, provider, out var resultValue ))
    {
      result = default;
      return false;
    }

    result = new Number( resultValue );
    return true;
  }

  public static bool TryParse( ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Number result )
  {
    if (!decimal.TryParse( s, style, provider, out var resultValue ))
    {
      result = default;
      return false;
    }

    result = new Number( resultValue );
    return true;
  }

  public bool TryFormat(
    Span<char> destination,
    out int charsWritten,
    ReadOnlySpan<char> format,
    IFormatProvider? provider
  )
  {
    return Value.TryFormat( destination, out charsWritten, format, provider );
  }

  public static bool operator >( Number left, Number right )
  {
    return left.Value > right.Value;
  }

  public static bool operator >=( Number left, Number right )
  {
    return left.Value >= right.Value;
  }

  public static bool operator <( Number left, Number right )
  {
    return left.Value < right.Value;
  }

  public static bool operator <=( Number left, Number right )
  {
    return left.Value <= right.Value;
  }

  public static Number operator +( Number left, Number right )
  {
    return new Number( left.Value + right.Value );
  }

  public static Number operator -( Number left, Number right )
  {
    return new Number( left.Value - right.Value );
  }

  public static Number operator *( Number left, Number right )
  {
    return new Number( left.Value * right.Value );
  }

  public static Number operator /( Number left, Number right )
  {
    if (right.Value == 0)
    {
      throw new DivideByZeroException();
    }

    return new Number( left.Value / right.Value );
  }

  public static Number operator %( Number left, Number right )
  {
    if (right.Value == 0)
    {
      throw new DivideByZeroException();
    }

    return new Number( left.Value % right.Value );
  }

  public static Number operator ++( Number value )
  {
    return new Number( value.Value + 1 );
  }

  public static Number operator --( Number value )
  {
    return new Number( value.Value - 1 );
  }

  public static Number operator +( Number value )
  {
    return value;
  }

  public static Number operator -( Number value )
  {
    return new Number( -value.Value );
  }

  public static Number Abs( Number value )
  {
    return new Number( Math.Abs( value.Value ) );
  }

  public static bool IsCanonical( Number value )
  {
    return true;
  }

  public static bool IsComplexNumber( Number value )
  {
    return false;
  }

  public static bool IsEvenInteger( Number value )
  {
    return decimal.IsEvenInteger( value.Value );
  }

  public static bool IsFinite( Number value )
  {
    return false;
  }

  public static bool IsImaginaryNumber( Number value )
  {
    return false;
  }

  public static bool IsInfinity( Number value )
  {
    return false;
  }

  public static bool IsInteger( Number value )
  {
    return decimal.IsInteger( value.Value );
  }

  public static bool IsNaN( Number value )
  {
    return false;
  }

  public static bool IsNegative( Number value )
  {
    return decimal.IsNegative( value.Value );
  }

  public static bool IsNegativeInfinity( Number value )
  {
    return false;
  }

  public static bool IsNormal( Number value )
  {
    return value.Value != 0;
  }

  public static bool IsOddInteger( Number value )
  {
    return decimal.IsOddInteger( value.Value );
  }

  public static bool IsPositive( Number value )
  {
    return decimal.IsPositive( value.Value );
  }

  public static bool IsPositiveInfinity( Number value )
  {
    return false;
  }

  public static bool IsRealNumber( Number value )
  {
    return true;
  }

  public static bool IsSubnormal( Number value )
  {
    return false;
  }

  public static bool IsZero( Number value )
  {
    return value.Value == 0;
  }

  public static Number MaxMagnitude( Number x, Number y )
  {
    return new Number( decimal.MaxMagnitude( x.Value, y.Value ) );
  }

  public static Number MaxMagnitudeNumber( Number x, Number y )
  {
    return new Number( decimal.MaxMagnitude( x.Value, y.Value ) );
  }

  public static Number MinMagnitude( Number x, Number y )
  {
    return new Number( decimal.MinMagnitude( x.Value, y.Value ) );
  }

  public static Number MinMagnitudeNumber( Number x, Number y )
  {
    return new Number( decimal.MinMagnitude( x.Value, y.Value ) );
  }

  public static bool TryConvertFromChecked<TOther>( TOther value, out Number result ) where TOther : INumberBase<TOther>
  {
    try
    {
      result = new Number( decimal.CreateChecked( value ) );
      return true;
    }
    catch (Exception)
    {
      result = default;
      return false;
    }
  }

  public static bool TryConvertFromSaturating<TOther>( TOther value, out Number result )
    where TOther : INumberBase<TOther>
  {
    try
    {
      result = new Number( decimal.CreateSaturating( value ) );
      return true;
    }
    catch (Exception)
    {
      result = default;
      return false;
    }
  }

  public static bool TryConvertFromTruncating<TOther>( TOther value, out Number result )
    where TOther : INumberBase<TOther>
  {
    try
    {
      result = new Number( decimal.CreateTruncating( value ) );
      return true;
    }
    catch (Exception)
    {
      result = default;
      return false;
    }
  }

  public static bool TryConvertToChecked<TOther>( Number value, [MaybeNullWhen( false )] out TOther result )
    where TOther : INumberBase<TOther>
  {
    return TOther.TryConvertFromChecked( value.Value, out result );
  }

  public static bool TryConvertToSaturating<TOther>( Number value, [MaybeNullWhen( false )] out TOther result )
    where TOther : INumberBase<TOther>
  {
    return TOther.TryConvertFromSaturating( value.Value, out result );
  }

  public static bool TryConvertToTruncating<TOther>( Number value, [MaybeNullWhen( false )] out TOther result )
    where TOther : INumberBase<TOther>
  {
    return TOther.TryConvertFromTruncating( value.Value, out result );
  }

  public static implicit operator Number( int value ) => new( value );
  public static implicit operator Number( long value ) => new( new decimal( value ) );
  public static implicit operator Number( float value ) => new( new decimal( value ) );
  public static implicit operator Number( double value ) => new( new decimal( value ) );
  public static implicit operator Number( decimal value ) => new( value );
}