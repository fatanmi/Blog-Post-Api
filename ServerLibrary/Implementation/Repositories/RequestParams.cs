using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Implementation.Repositories
{
    public class RequestParams
    {
        public int MaxPageSize { get; set; } = 50;
        public int PageNumber { get; set; } = 1;
        private int _PageSize { get; set; }
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = (value > 0 && value <= MaxPageSize) ? value : MaxPageSize; }
        }
    }
}
