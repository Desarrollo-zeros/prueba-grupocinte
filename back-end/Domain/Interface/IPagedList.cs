using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IPagedList<T> : IEnumerable<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
