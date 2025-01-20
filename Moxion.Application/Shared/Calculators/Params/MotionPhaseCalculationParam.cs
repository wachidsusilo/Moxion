using Moxion.Application.Abstractions.Calculators;
using Moxion.Common.Values;
using Moxion.Domain.Kinematic;

namespace Moxion.Application.Shared.Calculators.Params;

internal readonly record struct MotionPhaseCalculationParam( MotionProfile Profile, Time Time ) : ICalculationParam;