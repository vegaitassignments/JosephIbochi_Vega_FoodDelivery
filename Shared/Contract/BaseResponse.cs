namespace FuelAlert.Shared.Contract;

public class BaseResponse<T>
{

    public bool Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string>? ValidationErrors { get; set; }
    public T? Data { get; set; }

    public BaseResponse()
    {
        Status = true;
    }

    public BaseResponse(string message)
    {
        Status = true;
        Message = message;
    }
    public BaseResponse(string message, bool status)
    {
        Status = status;
        Message = message;
    }
    public BaseResponse(string message, bool status, T data)
    {
        Status = status;
        Message = message;
        Data = data;
    }
}

public class BaseResponse : BaseResponse<object>
{
    public BaseResponse(string message, bool status) : base(message, status)
    {
    }
    public BaseResponse() : base()
    {

    }
}