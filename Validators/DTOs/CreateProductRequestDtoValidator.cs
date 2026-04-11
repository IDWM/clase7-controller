using clase7_controller.DTOs;
using clase7_controller.Validators.Common;
using FluentValidation;

namespace clase7_controller.Validators.DTOs;

public class CreateProductRequestDtoValidator : AbstractValidator<CreateProductRequestDto>
{
    public CreateProductRequestDtoValidator()
    {
        RuleFor(dto => dto.Name).ApplyRequiredNameRules();
        RuleFor(dto => dto.Brand).ApplyRequiredBrandRules();
        RuleFor(dto => dto.Price).ApplyRequiredPriceRules();
    }
}
