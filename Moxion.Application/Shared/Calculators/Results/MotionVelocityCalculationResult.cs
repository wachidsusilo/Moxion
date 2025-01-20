using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Values.Derived;

namespace Moxion.Application.Shared.Calculators.Results;

internal record MotionVelocityCalculationResult( Velocity Velocity ) : ICalculationResult;