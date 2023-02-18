using System.Text.Json.Serialization;
using Amazon.Runtime;

namespace Digitus.Models;

public class Response<T>
{
    public T? Data { get; set; }
    
    [JsonIgnore]
    public int StatusCode { get; set; }

    [JsonIgnore]
    public bool IsSuccess { get; set; }

    public string? Message { get; set; }
    
    public TimeSpan? Time { get; set; }

    public List<string>? Errors { get; set; }

    
    //static factory method
    public static Response<T> Success(T data, int statusCode)
    {
        return new Response<T>() { Data = data, StatusCode = statusCode, IsSuccess = true };
    }
    
    public static Response<T> Success(TimeSpan? timeSpan, int statusCode)
    {
        return new Response<T> { Data = default(T),StatusCode = statusCode,IsSuccess = true};
    }
    
    public static Response<T> Success(string message,int statusCode)
    {
        return new Response<T> { Data = default(T),Message = message,StatusCode = statusCode,IsSuccess = true};
    }
    
    // public static Response<T> Success(TimeSpan time,int statusCode)
    // {
    //     return new Response<T> { Data = default(T),Time = time,StatusCode = statusCode,IsSuccess = true};
    // }
    

    public static Response<T> Fail(List<string> errors,int statusCode)
    {
        return new Response<T> { Errors = errors, StatusCode = statusCode, IsSuccess = false };
    }
    
    public static Response<T> Fail(string error,int statusCode)
    {
        return new Response<T> { Errors = new List<string>(){error}, StatusCode = statusCode, IsSuccess = false };
    }

    
}