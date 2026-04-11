using clase7_controller.DTOs;

namespace clase7_controller.Services.Interfaces;

public interface IProductService
{
    Task<PagedProductsResultDto> GetAllPagedAsync(GetProductsQueryDto queryDto);
    Task<ProductResponseDto> GetByIdAsync(int id);
    Task<ProductResponseDto> CreateAsync(CreateProductRequestDto requestDto);
    Task<ProductResponseDto> PatchAsync(int id, UpdateProductPartialRequestDto requestDto);
    Task DeleteAsync(int id);
}
