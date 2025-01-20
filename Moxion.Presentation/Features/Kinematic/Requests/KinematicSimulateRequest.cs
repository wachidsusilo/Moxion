using MediatR;
using Moxion.Common.Units;
using Moxion.Presentation.Dto.Values;
using Moxion.Presentation.Features.Kinematic.Responses;

namespace Moxion.Presentation.Features.Kinematic.Requests;

public record KinematicSimulateRequest(
  PositionDto Displacement,
  VelocityDto Velocity,
  AccelerationDto Acceleration,
  JerkDto Jerk,
  TimeUnit TimeIntervalUnit,
  int DataCount
) : IRequest<KinematicSimulateResponse>;