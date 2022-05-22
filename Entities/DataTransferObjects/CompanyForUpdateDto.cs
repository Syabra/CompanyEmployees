using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CompanyForUpdateDto : CompanyForManipulation
    {
        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
    }
}
