using Moxion.Domain.Kinematic;
using Moxion.Domain.Units;
using Moxion.Presentation.Dto.Kinematic;

namespace Moxion.Presentation.Abstractions.Factories;

internal interface IMotionDataFactory : IValueObjectFactory<MotionDataDto, KinematicUnitInfo, MotionData>;