using clase7_controller.DTOs;
using clase7_controller.Validators.Common;
using FluentValidation;

namespace clase7_controller.Validators.DTOs;

public class UpdateProductPartialRequestDtoValidator
    : AbstractValidator<UpdateProductPartialRequestDto>
{
    public UpdateProductPartialRequestDtoValidator()
    {
        RuleFor(dto => dto)
            .Custom(
                (dto, context) =>
                {
                    if (!HasAtLeastOneValue(dto))
                    {
                        context.AddFailure(
                            "request",
                            "Debes enviar al menos un campo para actualizar."
                        );
                    }
                }
            );

        When(dto => dto.Name is not null, () => RuleFor(dto => dto.Name!).ApplyOptionalNameRules());

        When(
            dto => dto.Brand is not null,
            () => RuleFor(dto => dto.Brand!).ApplyOptionalBrandRules()
        );

        When(
            dto => dto.Price.HasValue,
            () =>
                RuleFor(dto => dto.Price!.Value)
                    .ApplyOptionalPriceRules()
                    .OverridePropertyName("Price")
        );
    }

    private static bool HasAtLeastOneValue(UpdateProductPartialRequestDto dto)
    {
        return dto.Name is not null || dto.Brand is not null || dto.Price.HasValue;
    }
}
