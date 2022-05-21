using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.Models
{
    public class PageResult<T>
    {
        public int ItemCount { get; set; }

        public int PageCount { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public IEnumerable<T> Data { get; set; }

        public int FirstPage { get; set; }

        public int LastPage { get; set; }

        public int NextPage { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage { get; set; }

        public PageResult(IQueryable<T> results, int pageNumber, int resultsPerPage)
        {
            ItemCount = results.Count();
            PageCount = (int)Math.Ceiling((double)ItemCount / resultsPerPage);

            Skip = (pageNumber - 1) * resultsPerPage;
            Take = resultsPerPage;

            Data = results.Skip(Skip).Take(Take).ToList();

            FirstPage = 1;
            LastPage = LastPage = PageCount == 0 ? 1 : PageCount;

            NextPage = Math.Min(pageNumber + 1, LastPage);
            CurrentPage = pageNumber;
            PreviousPage = Math.Max(pageNumber - 1, FirstPage);
        }
    }
}
