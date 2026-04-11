using clase7_controller.DTOs;
using clase7_controller.Models;
using Mapster;

namespace clase7_controller.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateProductRequestDto, Product>.NewConfig();
        TypeAdapterConfig<Product, ProductResponseDto>.NewConfig();
    }
}
