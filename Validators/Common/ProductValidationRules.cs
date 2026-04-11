using FluentValidation;

namespace clase7_controller.Validators.Common;

public static class ProductValidationRules
{
    public static IRuleBuilderOptions<T, string> ApplyRequiredNameRules<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("El nombre no puede contener solo espacios.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres.");
    }

    public static IRuleBuilderOptions<T, string> ApplyRequiredBrandRules<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("La marca es obligatoria.")
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("La marca no puede contener solo espacios.")
            .MaximumLength(100)
            .WithMessage("La marca no puede superar los 100 caracteres.");
    }

    public static IRuleBuilderOptions<T, decimal> ApplyRequiredPriceRules<T>(
        this IRuleBuilder<T, decimal> ruleBuilder
    )
    {
        return ruleBuilder.GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
    }

    public static IRuleBuilderOptions<T, string> ApplyOptionalNameRules<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Si envías nombre, no puede venir vacío.")
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Si envías nombre, no puede contener solo espacios.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres.");
    }

    public static IRuleBuilderOptions<T, string> ApplyOptionalBrandRules<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Si envías marca, no puede venir vacía.")
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Si envías marca, no puede contener solo espacios.")
            .MaximumLength(100)
            .WithMessage("La marca no puede superar los 100 caracteres.");
    }

    public static IRuleBuilderOptions<T, decimal> ApplyOptionalPriceRules<T>(
        this IRuleBuilder<T, decimal> ruleBuilder
    )
    {
        return ruleBuilder.GreaterThan(0).WithMessage("Si envías precio, debe ser mayor a 0.");
    }
}
