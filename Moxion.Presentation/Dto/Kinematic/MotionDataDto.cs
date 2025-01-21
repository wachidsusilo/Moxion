using Moxion.Common.Enumerations;
using Moxion.Presentation.Abstractions;
using Moxion.Presentation.Abstractions.Dto;
using Moxion.Presentation.Dto.Values;

namespace Moxion.Presentation.Dto.Kinematic;

public readonly record struct MotionDataDto(
  double Time,
  double Position,
  double Velocity,
  double Acceleration,
  double Jerk,
  MotionPhase Phase
) : IValueObjectDto<MotionDataDto>;