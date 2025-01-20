using Moxion.Common;

namespace Moxion.Presentation.Abstractions.Validators;

internal interface IRequestValidator<in TRequest>
{
  Task<Result> Validate( TRequest request, CancellationToken cancellationToken );
}