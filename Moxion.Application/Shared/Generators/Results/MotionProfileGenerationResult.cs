using Moxion.Application.Abstractions.Generators;
using Moxion.Domain.Kinematic;

namespace Moxion.Application.Shared.Generators.Results;

internal readonly record struct MotionProfileGenerationResult( MotionProfile MotionProfile ) : IGenerationResult;