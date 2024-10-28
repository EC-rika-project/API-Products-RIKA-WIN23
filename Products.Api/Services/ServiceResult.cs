namespace Products.Api.Services;

public class ServiceResult<T>
{
    public T Data { get; set; } = default!;
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = null!;
    
    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T>
        {
            Data = data,
            IsSuccess = true
        };
    }
    
    public static ServiceResult<T> Failure(string errorMessage)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
    
    
}