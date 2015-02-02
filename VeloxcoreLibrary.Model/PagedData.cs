using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloxcoreLibrary.Model
{
    public class PagedData
    {
        public int CurrentPage { get; set; }
        public int RowCount { get; set; }
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        private int _pageSize = 20;
    }

    public class PagedResult<T> : PagedData
    {
        public IList<T> Results { get; set; }
    }
}
