using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class RequestsParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        public string OrderBy { get; set; }
    }

    public class EmployeeParameters : RequestsParameters
    {
        public EmployeeParameters()
        {
            OrderBy = "name";
        }
        public uint MinAge { get; set; }
        public uint MaxAge { get; set; } = int.MaxValue;


        public bool ValidAgeRange => MinAge < MaxAge;

        public string SearchTerm { get; set; }
    }
}
