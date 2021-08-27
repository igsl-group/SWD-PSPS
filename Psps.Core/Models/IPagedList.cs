using System.Collections.Generic;

namespace Psps.Core.Models
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        bool HasNextPage { get; }

        bool HasPreviousPage { get; }

        int CurrentPageIndex { get; }

        int PageSize { get; }

        int TotalCount { get; }

        int TotalPages { get; }
    }
}