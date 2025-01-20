using Moxion.Application.Feature.Kinematic.Queries;
using Moxion.Domain.Units;
using Moxion.Presentation.Abstractions.Factories;

namespace Moxion.Presentation.Factories.Data;

internal record SimulateMotionQueryFactoryData(
  SimulateMotionQuery Query,
  KinematicUnitInfo UnitInfo
) : IQueryFactoryData<SimulateMotionQuery, KinematicUnitInfo>;