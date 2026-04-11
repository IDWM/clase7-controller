using clase7_controller.DTOs;
using FluentValidation;

namespace clase7_controller.Validators.DTOs;

public class GetProductsQueryDtoValidator : AbstractValidator<GetProductsQueryDto>
{
    public GetProductsQueryDtoValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThan(0)
            .WithMessage("El número de página debe ser mayor a 0.");

        RuleFor(query => query.PageSize)
            .GreaterThan(0)
            .WithMessage("El tamaño de página debe ser mayor a 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("El tamaño de página no puede superar 100.");
    }
}
