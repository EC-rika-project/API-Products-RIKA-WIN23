namespace Products.Api.Models;

public class PaginationResult<T>
{
    public T Data { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public int Count { get; set; }
}