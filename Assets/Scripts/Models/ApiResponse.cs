using System.Net;

public class ApiResponse<T>
{
    public bool isSuccess;
    public HttpStatusCode statusCode;
    public T data;
}