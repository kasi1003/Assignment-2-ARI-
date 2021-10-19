using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Utilities
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int pageIndex, int pageSize)
        {
            int count = items.Count();
            List<T> pageItems = null;
            if (items.Count > 7)
                pageItems = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            else
                pageItems = items;
            PageIndex = pageIndex;
            this.AddRange(pageItems);
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}
