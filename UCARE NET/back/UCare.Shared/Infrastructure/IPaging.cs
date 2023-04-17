using GQ.Data.Abstractions.Paging;
using System.Collections;

namespace UCare.Shared.Infrastructure
{
    public interface IPaging
    {
        int? PageIndex { get; set; }

        int? PageSize { get; set; }

        long? PageCount { get; set; }

        long? RecordCount { get; set; }

        List<PagingFilter> Filter { get; set; }

        List<PagingOrder> Order { get; set; }

        string[] SearchTextProperties { get; set; }

        string SearchProperty { get; set; }

        IEnumerable? Data { get; set; }

        Task Apply<TEntityFirebase>(dynamic source, dynamic mapper);

        int GetMaxPageSize();

        void SetMaxPageSize(int value);
    }
}
