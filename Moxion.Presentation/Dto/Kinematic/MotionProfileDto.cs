using Moxion.Common.Enumerations;
using Moxion.Presentation.Abstractions.Dto;
using Moxion.Presentation.Dto.Values;

namespace Moxion.Presentation.Dto.Kinematic;

public readonly record struct MotionProfileDto(
  MotionProfileType ProfileType,
  PositionDto TotalDisplacement,
  VelocityDto MaxVelocity,
  AccelerationDto MaxAcceleration,
  JerkDto Jerk,
  TimeProfileDto TimeProfile,
  PositionProfileDto PositionProfile,
  VelocityProfileDto VelocityProfile,
  AccelerationProfileDto AccelerationProfile
) : IValueObjectDto<MotionProfileDto>;