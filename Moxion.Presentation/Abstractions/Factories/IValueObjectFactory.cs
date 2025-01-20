using Moxion.Abstractions;
using Moxion.Common;
using Moxion.Presentation.Abstractions.Dto;

namespace Moxion.Presentation.Abstractions.Factories;

internal interface IValueObjectFactory<TValueObjectDto, in TUnitInfo, in TValueObject>
  where TValueObjectDto : IValueObjectDto<TValueObjectDto>
  where TUnitInfo : IUnitInfo<TUnitInfo>
  where TValueObject : IValueObject<TValueObject>
{
  Task<Result<TValueObjectDto>> Create(
    TValueObject value,
    TUnitInfo sourceUnit,
    TUnitInfo destinationUnit,
    CancellationToken cancellationToken
  );

  Task<Result<TValueObjectDto[]>> Create(
    IReadOnlyList<TValueObject> values,
    TUnitInfo sourceUnit,
    TUnitInfo destinationUnit,
    CancellationToken cancellationToken
  );
}