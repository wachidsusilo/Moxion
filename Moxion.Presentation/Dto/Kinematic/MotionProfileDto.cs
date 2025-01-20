using Moxion.Common.Enumerations;
using Moxion.Presentation.Abstractions;
using Moxion.Presentation.Abstractions.Dto;
using Moxion.Presentation.Dto.Values;

namespace Moxion.Presentation.Dto.Kinematic;

public readonly record struct MotionProfileDto(
  MotionProfileType ProfileType,
  PositionDto Displacement,
  VelocityDto Velocity,
  AccelerationDto Acceleration,
  JerkDto Jerk,
  TimeDto JerkDuration,
  TimeDto AccelerationDuration,
  TimeDto SteadyMotionDuration,
  TimeDto TotalDuration
) : IValueObjectDto<MotionProfileDto>;