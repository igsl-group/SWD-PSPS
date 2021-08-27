using System;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Core.Models
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">TEntity</typeparam>
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Non zero-based page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int total = source.Count();
            this.TotalCount = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.CurrentPageIndex = pageIndex;
            if (this.TotalCount > 0)
                this.AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
            else
                this.AddRange(new List<T>());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Non zero-based page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            this.TotalCount = source.Count();
            this.TotalPages = TotalCount / pageSize;

            if (this.TotalCount % pageSize > 0)
                this.TotalPages++;

            this.PageSize = pageSize;
            this.CurrentPageIndex = pageIndex;
            if (this.TotalCount > 0)
                this.AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
            else
                this.AddRange(new List<T>());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Non zero-based page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.CurrentPageIndex = pageIndex;
            if (this.TotalCount > 0)
                this.AddRange(source);
            else
                this.AddRange(new List<T>());
        }

        public bool HasNextPage
        {
            get { return (CurrentPageIndex < TotalPages); }
        }

        public bool HasPreviousPage
        {
            get { return (CurrentPageIndex > 1); }
        }

        public int CurrentPageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public int TotalPages { get; private set; }
    }
}