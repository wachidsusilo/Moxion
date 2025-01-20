using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Enumerations;
using Moxion.Domain.Kinematic;

namespace Moxion.Application.Shared.Calculators.Params;

internal readonly record struct MotionJerkCalculationParam( MotionProfile Profile, MotionPhase Phase )
  : ICalculationParam;