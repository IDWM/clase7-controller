using System.Globalization;
using Bogus;
using clase7_controller.Exceptions;
using clase7_controller.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AppValidationException = clase7_controller.Exceptions.ValidationException;

namespace clase7_controller.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext dbContext,
        IValidator<Product> productValidator
    )
    {
        bool hasProducts = await dbContext.Products.AnyAsync();
        if (hasProducts)
        {
            return;
        }

        Randomizer.Seed = new Random();

        Faker<Product> productFaker = new Faker<Product>()
            .RuleFor(product => product.Name, faker => faker.Commerce.ProductName())
            .RuleFor(product => product.Brand, faker => faker.Company.CompanyName())
            .RuleFor(
                product => product.Price,
                faker =>
                    decimal.Parse(faker.Commerce.Price(1000, 500000), CultureInfo.InvariantCulture)
            );

        List<Product> products = productFaker.Generate(1000);
        foreach (Product product in products)
        {
            FluentValidation.Results.ValidationResult validationResult =
                await productValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult
                    .Errors.GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        grouping => grouping.Key,
                        grouping =>
                            grouping.Select(error => error.ErrorMessage).Distinct().ToArray()
                    );

                throw new AppValidationException(
                    "Se encontraron errores de validación en el seed.",
                    errors
                );
            }
        }

        await dbContext.Products.AddRangeAsync(products);
        int rowsAffected = await dbContext.SaveChangesAsync();
        if (rowsAffected == 0)
        {
            throw new PersistenceException(
                "Error al inicializar los productos en la base de datos."
            );
        }
    }
}
