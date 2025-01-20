using Moxion.Application.Feature.Kinematic.QueryResults;
using Moxion.Domain.Units;
using Moxion.Presentation.Features.Kinematic.Responses;

namespace Moxion.Presentation.Abstractions.Factories;

internal interface IKinematicResponseFactory
  : IResponseFactory<KinematicSimulateResponse, SimulateMotionQueryResult, KinematicUnitInfo>;