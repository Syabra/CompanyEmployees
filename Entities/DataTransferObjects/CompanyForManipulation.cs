using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CompanyForManipulation
    {
        [Required(ErrorMessage = "Company name is required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company adress is a requires field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is a requires field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Country is 60 characters.")]
        public string Country { get; set; }
    }
}
