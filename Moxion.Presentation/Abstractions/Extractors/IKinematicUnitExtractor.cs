using Moxion.Domain.Units;
using Moxion.Presentation.Features.Kinematic.Requests;

namespace Moxion.Presentation.Abstractions.Extractors;

internal interface IKinematicUnitExtractor : IUnitExtractor<KinematicSimulateRequest, KinematicUnitInfo>;