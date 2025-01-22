using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Values.Derived;

namespace Moxion.Application.Shared.Calculators.Results;

internal readonly record struct MotionAccelerationCalculationResult( Acceleration Acceleration ) : ICalculationResult;