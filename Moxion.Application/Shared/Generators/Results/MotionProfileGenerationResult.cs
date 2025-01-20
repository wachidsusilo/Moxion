using Moxion.Application.Abstractions.Generators;
using Moxion.Domain.Kinematic;

namespace Moxion.Application.Shared.Generators.Results;

internal record MotionProfileGenerationResult( MotionProfile MotionProfile ) : IGenerationResult;