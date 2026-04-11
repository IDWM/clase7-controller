using clase7_controller.Models;
using clase7_controller.Validators.Common;
using FluentValidation;

namespace clase7_controller.Validators.Models;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(product => product.Name).ApplyRequiredNameRules();
        RuleFor(product => product.Brand).ApplyRequiredBrandRules();
        RuleFor(product => product.Price).ApplyRequiredPriceRules();
    }
}
