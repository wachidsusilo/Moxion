using Moxion.Common;
using Moxion.Common.Enumerations;
using Moxion.Common.Units;
using Moxion.Common.Values;
using Moxion.Common.Values.Derived;
using Moxion.Presentation.Abstractions.Converters;

namespace Moxion.Presentation.Converters;

internal class UnitConverter : IUnitConverter
{
  #region Position

  public Task<Result<Position>> Convert( Position value, PositionUnit fromUnit, PositionUnit toUnit )
  {
    if (fromUnit is PositionUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Position.Zero ) );
    }

    if (toUnit is PositionUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Position.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Position( value.Value * PositionMeterMultipliers[toUnit] / PositionMeterMultipliers[fromUnit] )
      )
    );
  }

  #endregion

  #region Area

  public Task<Result<Area>> Convert( Area value, PositionUnit fromUnit, PositionUnit toUnit )
  {
    if (fromUnit is PositionUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Area.Zero ) );
    }

    if (toUnit is PositionUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Area.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Area( value.Value * AreaMeterMultipliers[toUnit] / AreaMeterMultipliers[fromUnit] )
      )
    );
  }

  #endregion

  #region Volume

  public Task<Result<Volume>> Convert( Volume value, PositionUnit fromUnit, PositionUnit toUnit )
  {
    if (fromUnit is PositionUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Volume.Zero ) );
    }

    if (toUnit is PositionUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Volume.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Volume( value.Value * VolumeMeterMultipliers[toUnit] / VolumeMeterMultipliers[fromUnit] )
      )
    );
  }

  public Task<Result<Volume>> Convert( Volume value, VolumeUnit fromUnit, VolumeUnit toUnit )
  {
    if (fromUnit is VolumeUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceVolumeUnit, Volume.Zero ) );
    }

    if (toUnit is VolumeUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationVolumeUnit, Volume.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Volume( value.Value * VolumeLiterMultipliers[toUnit] / VolumeLiterMultipliers[fromUnit] )
      )
    );
  }

  public Task<Result<Volume>> Convert( Volume value, PositionUnit fromUnit, VolumeUnit toUnit )
  {
    if (fromUnit is PositionUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Volume.Zero ) );
    }

    if (toUnit is VolumeUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationVolumeUnit, Volume.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        AreEquivalent( fromUnit, toUnit )
          ? value
          : new Volume( value.Value * VolumeLiterMultipliers[toUnit] / VolumeMeterMultipliers[fromUnit] )
      )
    );
  }

  public Task<Result<Volume>> Convert( Volume value, VolumeUnit fromUnit, PositionUnit toUnit )
  {
    if (fromUnit is VolumeUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceVolumeUnit, Volume.Zero ) );
    }

    if (toUnit is PositionUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Volume.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        AreEquivalent( toUnit, fromUnit )
          ? value
          : new Volume( value.Value * VolumeMeterMultipliers[toUnit] / VolumeLiterMultipliers[fromUnit] )
      )
    );
  }

  public bool AreEquivalent( PositionUnit positionUnit, VolumeUnit volumeUnit )
  {
    return ( positionUnit == PositionUnit.Decimeter && volumeUnit == VolumeUnit.Liter )
           || ( positionUnit == PositionUnit.Centimeter && volumeUnit == VolumeUnit.Milliliter )
           || ( positionUnit == PositionUnit.Millimeter && volumeUnit == VolumeUnit.Microliter );
  }

  #endregion

  #region Time

  public Task<Result<Time>> Convert( Time value, TimeUnit fromUnit, TimeUnit toUnit )
  {
    if (fromUnit is TimeUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceTimeUnit, Time.Zero ) );
    }

    if (toUnit is TimeUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationTimeUnit, Time.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Time( value.Value * TimeSecondMultipliers[toUnit] / TimeSecondMultipliers[fromUnit] )
      )
    );
  }

  #endregion

  #region Time Squared

  public Task<Result<TimeSquared>> Convert( TimeSquared value, TimeUnit fromUnit, TimeUnit toUnit )
  {
    if (fromUnit is TimeUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceTimeUnit, TimeSquared.Zero ) );
    }

    if (toUnit is TimeUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationTimeUnit, TimeSquared.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new TimeSquared( value.Value * TimeSquaredSecondMultipliers[toUnit] / TimeSquaredSecondMultipliers[fromUnit] )
      )
    );
  }

  #endregion

  #region Time Cubed

  public Task<Result<TimeCubed>> Convert( TimeCubed value, TimeUnit fromUnit, TimeUnit toUnit )
  {
    if (fromUnit is TimeUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceTimeUnit, TimeCubed.Zero ) );
    }

    if (toUnit is TimeUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationTimeUnit, TimeCubed.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new TimeCubed( value.Value * TimeCubedSecondMultipliers[toUnit] / TimeCubedSecondMultipliers[fromUnit] )
      )
    );
  }

  #endregion

  #region Velocity

  public Task<Result<Velocity>> Convert( Velocity value, PositionUnit fromUnit, PositionUnit toUnit )
  {
    if (fromUnit is PositionUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Velocity.Zero ) );
    }

    if (toUnit is PositionUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Velocity.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Velocity( value.Value * PositionMeterMultipliers[toUnit] / PositionMeterMultipliers[fromUnit] )
      )
    );
  }

  public Task<Result<Velocity>> Convert( Velocity value, TimeUnit fromUnit, TimeUnit toUnit )
  {
    if (fromUnit is TimeUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceTimeUnit, Velocity.Zero ) );
    }

    if (toUnit is TimeUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationTimeUnit, Velocity.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Velocity( value.Value * TimeSecondMultipliers[fromUnit] / TimeSecondMultipliers[toUnit] )
      )
    );
  }

  public Task<Result<Velocity>> Convert(
    Velocity value,
    PositionUnit fromPositionUnit,
    TimeUnit fromTimeUnit,
    PositionUnit toPositionUnit,
    TimeUnit toTimeUnit
  )
  {
    if (fromPositionUnit is PositionUnit.None || !Enum.IsDefined( fromPositionUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Velocity.Zero ) );
    }

    if (fromTimeUnit is TimeUnit.None || !Enum.IsDefined( fromTimeUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceTimeUnit, Velocity.Zero ) );
    }

    if (toPositionUnit is PositionUnit.None || !Enum.IsDefined( toPositionUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Velocity.Zero ) );
    }

    if (toTimeUnit is TimeUnit.None || !Enum.IsDefined( toTimeUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationTimeUnit, Velocity.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromPositionUnit == toPositionUnit && fromTimeUnit == toTimeUnit
          ? value
          : new Velocity(
            ( value.Value * PositionMeterMultipliers[toPositionUnit] / PositionMeterMultipliers[fromPositionUnit] )
            * TimeSecondMultipliers[fromTimeUnit] / TimeSecondMultipliers[toTimeUnit]
          )
      )
    );
  }

  #endregion

  #region Acceleration

  public Task<Result<Acceleration>> Convert( Acceleration value, PositionUnit fromUnit, PositionUnit toUnit )
  {
    if (fromUnit is PositionUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Acceleration.Zero ) );
    }

    if (toUnit is PositionUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Acceleration.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Acceleration( value.Value * PositionMeterMultipliers[toUnit] / PositionMeterMultipliers[fromUnit] )
      )
    );
  }

  public Task<Result<Acceleration>> Convert( Acceleration value, TimeUnit fromUnit, TimeUnit toUnit )
  {
    if (fromUnit is TimeUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceTimeUnit, Acceleration.Zero ) );
    }

    if (toUnit is TimeUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationTimeUnit, Acceleration.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Acceleration( value.Value * TimeSecondMultipliers[fromUnit] / TimeSecondMultipliers[toUnit] )
      )
    );
  }

  public Task<Result<Acceleration>> Convert(
    Acceleration value,
    PositionUnit fromPositionUnit,
    TimeUnit fromTimeUnit,
    PositionUnit toPositionUnit,
    TimeUnit toTimeUnit
  )
  {
    if (fromPositionUnit is PositionUnit.None || !Enum.IsDefined( fromPositionUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Acceleration.Zero ) );
    }

    if (fromTimeUnit is TimeUnit.None || !Enum.IsDefined( fromTimeUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceTimeUnit, Acceleration.Zero ) );
    }

    if (toPositionUnit is PositionUnit.None || !Enum.IsDefined( toPositionUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Acceleration.Zero ) );
    }

    if (toTimeUnit is TimeUnit.None || !Enum.IsDefined( toTimeUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationTimeUnit, Acceleration.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromPositionUnit == toPositionUnit && fromTimeUnit == toTimeUnit
          ? value
          : new Acceleration(
            ( value.Value * PositionMeterMultipliers[toPositionUnit] / PositionMeterMultipliers[fromPositionUnit] )
            * TimeSecondMultipliers[fromTimeUnit] / TimeSecondMultipliers[toTimeUnit]
          )
      )
    );
  }

  #endregion

  #region Jerk

  public Task<Result<Jerk>> Convert( Jerk value, PositionUnit fromUnit, PositionUnit toUnit )
  {
    if (fromUnit is PositionUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Jerk.Zero ) );
    }

    if (toUnit is PositionUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Jerk.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Jerk( value.Value * PositionMeterMultipliers[toUnit] / PositionMeterMultipliers[fromUnit] )
      )
    );
  }

  public Task<Result<Jerk>> Convert( Jerk value, TimeUnit fromUnit, TimeUnit toUnit )
  {
    if (fromUnit is TimeUnit.None || !Enum.IsDefined( fromUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceTimeUnit, Jerk.Zero ) );
    }

    if (toUnit is TimeUnit.None || !Enum.IsDefined( toUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationTimeUnit, Jerk.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromUnit == toUnit
          ? value
          : new Jerk( value.Value * TimeSecondMultipliers[fromUnit] / TimeSecondMultipliers[toUnit] )
      )
    );
  }

  public Task<Result<Jerk>> Convert(
    Jerk value,
    PositionUnit fromPositionUnit,
    TimeUnit fromTimeUnit,
    PositionUnit toPositionUnit,
    TimeUnit toTimeUnit
  )
  {
    if (fromPositionUnit is PositionUnit.None || !Enum.IsDefined( fromPositionUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourcePositionUnit, Jerk.Zero ) );
    }

    if (fromTimeUnit is TimeUnit.None || !Enum.IsDefined( fromTimeUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidSourceTimeUnit, Jerk.Zero ) );
    }

    if (toPositionUnit is PositionUnit.None || !Enum.IsDefined( toPositionUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationPositionUnit, Jerk.Zero ) );
    }

    if (toTimeUnit is TimeUnit.None || !Enum.IsDefined( toTimeUnit ))
    {
      return Task.FromResult( Result.Error( ErrorCode.InvalidDestinationTimeUnit, Jerk.Zero ) );
    }

    return Task.FromResult(
      Result.Success(
        fromPositionUnit == toPositionUnit && fromTimeUnit == toTimeUnit
          ? value
          : new Jerk(
            ( value.Value * PositionMeterMultipliers[toPositionUnit] / PositionMeterMultipliers[fromPositionUnit] )
            * TimeSecondMultipliers[fromTimeUnit] / TimeSecondMultipliers[toTimeUnit]
          )
      )
    );
  }

  #endregion

  #region Multipliers

  private static readonly IReadOnlyDictionary<PositionUnit, Number> PositionMeterMultipliers =
    new Dictionary<PositionUnit, Number>
    {
      [PositionUnit.Kilometer] = 1e3m,
      [PositionUnit.Hectometer] = 1e2m,
      [PositionUnit.Decameter] = 1e1m,
      [PositionUnit.Meter] = 1,
      [PositionUnit.Decimeter] = 1e-1m,
      [PositionUnit.Centimeter] = 1e-2m,
      [PositionUnit.Millimeter] = 1e-3m
    };

  private static readonly IReadOnlyDictionary<PositionUnit, Number> AreaMeterMultipliers =
    new Dictionary<PositionUnit, Number>
    {
      [PositionUnit.Kilometer] = 1e6m,
      [PositionUnit.Hectometer] = 1e4m,
      [PositionUnit.Decameter] = 1e2m,
      [PositionUnit.Meter] = 1,
      [PositionUnit.Decimeter] = 1e-2m,
      [PositionUnit.Centimeter] = 1e-4m,
      [PositionUnit.Millimeter] = 1e-6m
    };

  private static readonly IReadOnlyDictionary<PositionUnit, Number> VolumeMeterMultipliers =
    new Dictionary<PositionUnit, Number>
    {
      [PositionUnit.Kilometer] = 1e9m,
      [PositionUnit.Hectometer] = 1e6m,
      [PositionUnit.Decameter] = 1e3m,
      [PositionUnit.Meter] = 1,
      [PositionUnit.Decimeter] = 1e-3m,
      [PositionUnit.Centimeter] = 1e-6m,
      [PositionUnit.Millimeter] = 1e-9m
    };

  private static readonly IReadOnlyDictionary<VolumeUnit, Number> VolumeLiterMultipliers =
    new Dictionary<VolumeUnit, Number>
    {
      [VolumeUnit.Liter] = 1e-3m,
      [VolumeUnit.Milliliter] = 1e-6m,
      [VolumeUnit.Microliter] = 1e-9m,
      [VolumeUnit.Nanoliter] = 1e-12m
    };

  private static readonly IReadOnlyDictionary<TimeUnit, Number> TimeSecondMultipliers =
    new Dictionary<TimeUnit, Number>
    {
      [TimeUnit.Second] = 1,
      [TimeUnit.Millisecond] = 1e-3m,
      [TimeUnit.Microsecond] = 1e-6m,
      [TimeUnit.Nanosecond] = 1e-9m
    };

  private static readonly IReadOnlyDictionary<TimeUnit, Number> TimeSquaredSecondMultipliers =
    new Dictionary<TimeUnit, Number>
    {
      [TimeUnit.Second] = 1,
      [TimeUnit.Millisecond] = 1e-6m,
      [TimeUnit.Microsecond] = 1e-12m,
      [TimeUnit.Nanosecond] = 1e-18m
    };

  private static readonly IReadOnlyDictionary<TimeUnit, Number> TimeCubedSecondMultipliers =
    new Dictionary<TimeUnit, Number>
    {
      [TimeUnit.Second] = 1,
      [TimeUnit.Millisecond] = 1e-9m,
      [TimeUnit.Microsecond] = 1e-18m,
      [TimeUnit.Nanosecond] = 1e-27m
    };

  #endregion
}