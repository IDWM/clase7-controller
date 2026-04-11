namespace clase7_controller.DTOs;

public class CreateProductRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
