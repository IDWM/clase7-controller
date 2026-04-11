using System.Globalization;
using clase7_controller.DTOs;
using clase7_controller.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace clase7_controller.Controllers;

/// <summary>
/// API para la gestión de productos.
/// </summary>
[ApiController]
[Route("api/v1/product")]
[Produces("application/json")]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    /// <summary>
    /// Obtiene todos los productos paginados.
    /// </summary>
    /// <remarks>Retorna los datos y metadata de paginación en la respuesta.</remarks>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedProductsResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ApiErrorDataDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ApiResponse<ApiErrorDataDto>),
        StatusCodes.Status500InternalServerError
    )]
    public async Task<ActionResult<ApiResponse<PagedProductsResultDto>>> GetAllAsync(
        [FromQuery] GetProductsQueryDto queryDto
    )
    {
        PagedProductsResultDto pagedResult = await _productService.GetAllPagedAsync(queryDto);
        ApiResponse<PagedProductsResultDto> response = new()
        {
            Message = "Productos obtenidos correctamente.",
            Data = pagedResult,
        };

        return Ok(response);
    }

    /// <summary>
    /// Obtiene un producto por su identificador.
    /// </summary>
    [HttpGet("{id}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ApiResponse<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ApiErrorDataDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<ApiErrorDataDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ApiResponse<ApiErrorDataDto>),
        StatusCodes.Status500InternalServerError
    )]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        if (!TryParseProductId(id, out int productId, out IActionResult? badId))
        {
            return badId!;
        }

        ProductResponseDto product = await _productService.GetByIdAsync(productId);
        ApiResponse<ProductResponseDto> response = new()
        {
            Message = "Producto obtenido correctamente.",
            Data = product,
        };

        return Ok(response);
    }

    /// <summary>
    /// Crea un producto nuevo.
    /// </summary>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiResponse<ProductResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<ApiErrorDataDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ApiResponse<ApiErrorDataDto>),
        StatusCodes.Status500InternalServerError
    )]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> CreateAsync(
        [FromBody] CreateProductRequestDto requestDto
    )
    {
        ProductResponseDto createdProduct = await _productService.CreateAsync(requestDto);
        ApiResponse<ProductResponseDto> response = new()
        {
            Message = "Producto creado correctamente.",
            Data = createdProduct,
        };

        return CreatedAtRoute("GetProductById", new { id = createdProduct.Id }, response);
    }

    /// <summary>
    /// Edita parcialmente un producto.
    /// </summary>
    [HttpPatch("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiResponse<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ApiErrorDataDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<ApiErrorDataDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ApiResponse<ApiErrorDataDto>),
        StatusCodes.Status500InternalServerError
    )]
    public async Task<IActionResult> PatchAsync(
        [FromRoute] string id,
        [FromBody] UpdateProductPartialRequestDto requestDto
    )
    {
        if (!TryParseProductId(id, out int productId, out IActionResult? badId))
        {
            return badId!;
        }

        ProductResponseDto updatedProduct = await _productService.PatchAsync(productId, requestDto);
        ApiResponse<ProductResponseDto> response = new()
        {
            Message = "Producto actualizado correctamente.",
            Data = updatedProduct,
        };

        return Ok(response);
    }

    /// <summary>
    /// Elimina un producto por su identificador.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<ApiErrorDataDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<ApiErrorDataDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ApiResponse<ApiErrorDataDto>),
        StatusCodes.Status500InternalServerError
    )]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        if (!TryParseProductId(id, out int productId, out IActionResult? badId))
        {
            return badId!;
        }

        await _productService.DeleteAsync(productId);
        return NoContent();
    }

    private bool TryParseProductId(string idSegment, out int id, out IActionResult? errorResult)
    {
        if (!int.TryParse(idSegment, NumberStyles.Integer, CultureInfo.InvariantCulture, out id))
        {
            ApiResponse<ApiErrorDataDto> body = new()
            {
                Message = "Se encontraron errores de validación.",
                Data = new ApiErrorDataDto
                {
                    ErrorCode = "VALIDATION_ERROR",
                    TraceId = HttpContext.TraceIdentifier,
                    Errors = new Dictionary<string, string[]>
                    {
                        ["id"] = ["El identificador debe ser un número entero."],
                    },
                },
            };
            errorResult = BadRequest(body);
            return false;
        }

        errorResult = null;
        return true;
    }
}
