using Moxion.Common.Enumerations;
using Moxion.Domain.Units;
using Moxion.Presentation.Abstractions.Transport;
using Moxion.Presentation.Dto.Kinematic;

namespace Moxion.Presentation.Features.Kinematic.Responses;

public record KinematicSimulateResponse(
  ErrorCode ErrorCode,
  MotionProfileDto? Profile,
  MotionDataDto[]? Data,
  KinematicUnitInfo? UnitInfo
) : IResponse;