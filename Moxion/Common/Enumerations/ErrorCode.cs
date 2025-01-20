namespace Moxion.Common.Enumerations;

public enum ErrorCode
{
  #region General

  NoError,
  UnknownError,
  UnexpectedNullData,
  OperationCancelled,

  #endregion

  #region Calculation

  NegativeTimeResult,

  #endregion

  #region Validation

  InvalidTime,
  InvalidPositionUnit,
  InvalidVelocityUnit,
  InvalidAccelerationUnit,
  InvalidJerkUnit,
  InvalidDataCount,

  #endregion

  #region Conversion

  InvalidSourcePositionUnit,
  InvalidDestinationPositionUnit,
  InvalidSourceTimeUnit,
  InvalidDestinationTimeUnit,
  InvalidSourceVolumeUnit,
  InvalidDestinationVolumeUnit,

  #endregion
}