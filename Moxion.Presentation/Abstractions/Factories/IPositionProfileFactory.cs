using Moxion.Domain.Kinematic;
using Moxion.Domain.Units;
using Moxion.Presentation.Dto.Kinematic;

namespace Moxion.Presentation.Abstractions.Factories;

internal interface IPositionProfileFactory
  : IValueObjectFactory<PositionProfileDto, PositionUnitInfo, PositionProfile>;