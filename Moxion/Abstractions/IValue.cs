using Moxion.Common.Values;

namespace Moxion.Abstractions;

public interface IValue<TSelf> : IEquatable<TSelf>, IComparable<TSelf> where TSelf : struct, IValue<TSelf>
{
  Number Value { get; }

  bool IsZero { get; }
  bool IsPositive { get; }
  bool IsNegative { get; }
  bool IsValid { get; }

  sbyte ToSbyte();
  byte ToByte();
  short ToShort();
  ushort ToUShort();
  int ToInt();
  uint ToUInt();
  long ToLong();
  ulong ToULong();
  float ToFloat();
  double ToDouble();
  decimal ToDecimal();

  static abstract TSelf Zero { get; }

  static abstract bool operator <( TSelf left, TSelf right );
  static abstract bool operator >( TSelf left, TSelf right );
  static abstract bool operator <=( TSelf left, TSelf right );
  static abstract bool operator >=( TSelf left, TSelf right );
  static abstract TSelf operator +( TSelf left, TSelf right );
  static abstract TSelf operator +( TSelf left, Number right );
  static abstract TSelf operator +( Number left, TSelf right );
  static abstract TSelf operator -( TSelf left, TSelf right );
  static abstract TSelf operator -( TSelf left, Number right );
  static abstract TSelf operator -( Number left, TSelf right );
  static abstract TSelf operator *( TSelf left, Number right );
  static abstract TSelf operator *( Number left, TSelf right );
  static abstract Number operator /( TSelf left, TSelf right );
  static abstract TSelf operator /( TSelf left, Number right );
  static abstract TSelf operator %( TSelf left, Number right );
  static abstract TSelf operator +( TSelf value );
  static abstract TSelf operator -( TSelf value );
}