namespace Branef.Empresas.Domain.Interfaces.Base
{
    public interface IPaginatedResponse<TData> where TData : class
    {
        int Count { get; set; }
        int PageNumber {  get; set; }
        int PageSize { get; set; }
        IEnumerable<TData>? Data { get; set; }
    }
}
