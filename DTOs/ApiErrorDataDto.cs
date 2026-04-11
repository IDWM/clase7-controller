namespace clase7_controller.DTOs;

public class ApiErrorDataDto
{
    public string ErrorCode { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    public IDictionary<string, string[]>? Errors { get; set; }
}
