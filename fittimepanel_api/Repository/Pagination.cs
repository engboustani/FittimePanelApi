using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Repository
{
    public class Paginated<T>
    {
        public Paginated(
                IEnumerable<T> items, int count, int pageNumber, int itemsPerPage)
        {
            PageInfo = new PageInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = itemsPerPage,
                TotalPages = (int)Math.Ceiling(count / (double)itemsPerPage),
                TotalItems = count,
            };

            Items = items;
        }
        public PageInfo PageInfo { get; set; }
        private IEnumerable<T> Items { get; set; }
        public IList<T> ItemsList { get; set; }
        public static Paginated<T> ToPaginatedItem(
            IQueryable<T> items, int pageNumber, int itemsPerPage)
        {
            var count = items.Count();
            var chunk = items.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);
            return new Paginated<T>(chunk, count, pageNumber, itemsPerPage);
        }
        public async Task ToListAsync()
        {
            ItemsList = await Items.AsQueryable().ToListAsync();
        }
    }

    public class PageInfo
    {
        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
    }

}
