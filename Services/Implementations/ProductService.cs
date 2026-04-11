using clase7_controller.DTOs;
using clase7_controller.Exceptions;
using clase7_controller.Models;
using clase7_controller.Repositories.Interfaces;
using clase7_controller.Services.Interfaces;
using Mapster;

namespace clase7_controller.Services.Implementations;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<PagedProductsResultDto> GetAllPagedAsync(GetProductsQueryDto queryDto)
    {
        IReadOnlyList<Product> products = await _productRepository.GetPagedAsync(
            queryDto.Page,
            queryDto.PageSize
        );
        int totalItems = await _productRepository.CountAsync();
        int totalPages =
            totalItems == 0 ? 0 : (int)Math.Ceiling((double)totalItems / queryDto.PageSize);

        PagedProductsResultDto result = new()
        {
            Items = products.Adapt<IReadOnlyList<ProductResponseDto>>(),
            Page = queryDto.Page,
            PageSize = queryDto.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasNext = queryDto.Page < totalPages,
            HasPrevious = queryDto.Page > 1,
        };

        return result;
    }

    public async Task<ProductResponseDto> GetByIdAsync(int id)
    {
        Product product =
            await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Producto no encontrado.");
        return product.Adapt<ProductResponseDto>();
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductRequestDto requestDto)
    {
        Product product = requestDto.Adapt<Product>();
        Product createdProduct = await _productRepository.CreateAsync(product);
        return createdProduct.Adapt<ProductResponseDto>();
    }

    public async Task<ProductResponseDto> PatchAsync(
        int id,
        UpdateProductPartialRequestDto requestDto
    )
    {
        Product product =
            await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Producto no encontrado.");

        if (requestDto.Name is not null)
        {
            product.Name = requestDto.Name;
        }

        if (requestDto.Brand is not null)
        {
            product.Brand = requestDto.Brand;
        }

        if (requestDto.Price.HasValue)
        {
            product.Price = requestDto.Price.Value;
        }

        Product updatedProduct = await _productRepository.UpdateAsync(product);
        return updatedProduct.Adapt<ProductResponseDto>();
    }

    public async Task DeleteAsync(int id)
    {
        bool wasDeleted = await _productRepository.DeleteAsync(id);
        if (!wasDeleted)
        {
            throw new NotFoundException("Producto no encontrado.");
        }
    }
}
