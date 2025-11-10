using System.Net;

namespace Kruggers_Backend.Configuration;

public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;


    public static ApiResponse<T> SuccessResponse(T data, string message = "",  HttpStatusCode  statusCode = HttpStatusCode.OK)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }


    public static ApiResponse<T> FailureResponse(string message, List<string>? errors = null,HttpStatusCode  statusCode = HttpStatusCode.OK)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors,
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }
}