using Moxion.Common.Enumerations;
using Moxion.Presentation.Abstractions;
using Moxion.Presentation.Abstractions.Dto;
using Moxion.Presentation.Dto.Values;

namespace Moxion.Presentation.Dto.Kinematic;

public readonly record struct MotionDataDto(
  TimeDto Time,
  PositionDto Position,
  VelocityDto Velocity,
  AccelerationDto Acceleration,
  JerkDto Jerk,
  MotionPhase Phase
) : IValueObjectDto<MotionDataDto>;