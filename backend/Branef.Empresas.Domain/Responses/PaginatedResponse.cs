using Branef.Empresas.Domain.Interfaces.Base;

namespace Branef.Empresas.Domain.Responses
{
    public class PaginatedResponse<TData> : IPaginatedResponse<TData> where TData : class
    {
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<TData>? Data { get; set; }
    }
}
