using Moxion.Application.Feature.Kinematic.Queries;
using Moxion.Domain.Units;
using Moxion.Presentation.Factories.Data;
using Moxion.Presentation.Features.Kinematic.Requests;

namespace Moxion.Presentation.Abstractions.Factories;

internal interface IKinematicQueryFactory
  : IQueryFactory<SimulateMotionQuery, KinematicUnitInfo, SimulateMotionQueryFactoryData?, KinematicSimulateRequest>;