using Moxion.Abstractions;
using Moxion.Common;
using Moxion.Common.Units;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;

namespace Moxion.Presentation.Abstractions.Converters;

internal interface IUnitConverter
  : ISingleRepresentationUnitConverter<Position, PositionUnit>,
    ISingleRepresentationUnitConverter<Area, PositionUnit>,
    IMultiRepresentationUnitConverter<Volume, PositionUnit, VolumeUnit>,
    ISingleRepresentationUnitConverter<TimeSquared, TimeUnit>,
    ISingleRepresentationUnitConverter<TimeCubed, TimeUnit>,
    ISingleRepresentationUnitConverter<Time, TimeUnit>,
    ISingleRepresentationUnitConverter<Velocity, PositionUnit, TimeUnit>,
    ISingleRepresentationUnitConverter<Acceleration, PositionUnit, TimeUnit>,
    ISingleRepresentationUnitConverter<Jerk, PositionUnit, TimeUnit>;

internal interface ISingleRepresentationUnitConverter<TValue, in TUnit>
  where TValue : struct, IValue<TValue>
  where TUnit : Enum
{
  Task<Result<TValue>> Convert( TValue value, TUnit fromUnit, TUnit toUnit );
}

internal interface ISingleRepresentationUnitConverter<TValue, in TFirstUnit, in TSecondUnit>
  where TValue : struct, IValue<TValue>
  where TFirstUnit : Enum
  where TSecondUnit : Enum
{
  Task<Result<TValue>> Convert( TValue value, TFirstUnit fromUnit, TFirstUnit toUnit );
  Task<Result<TValue>> Convert( TValue value, TSecondUnit fromUnit, TSecondUnit toUnit );

  Task<Result<TValue>> Convert(
    TValue value,
    TFirstUnit fromFirstUnit,
    TSecondUnit fromSecondUnit,
    TFirstUnit toFirstUnit,
    TSecondUnit toSecondUnit
  );
}

internal interface IMultiRepresentationUnitConverter<TValue, in TFirstUnit, in TSecondUnit>
  where TValue : struct, IValue<TValue>
  where TFirstUnit : Enum
{
  Task<Result<TValue>> Convert( TValue value, TFirstUnit fromUnit, TFirstUnit toUnit );
  Task<Result<TValue>> Convert( TValue value, TSecondUnit fromUnit, TSecondUnit toUnit );
  Task<Result<TValue>> Convert( TValue value, TFirstUnit fromUnit, TSecondUnit toUnit );
  Task<Result<TValue>> Convert( TValue value, TSecondUnit fromUnit, TFirstUnit toUnit );
  bool AreEquivalent( TFirstUnit firstUnit, TSecondUnit secondUnit );
}