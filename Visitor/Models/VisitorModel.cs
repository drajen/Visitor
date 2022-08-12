using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Visitor.Models
{
    public class VisitorModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is Required.")]
        [StringLength(30, ErrorMessage = "First name is too long.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is Required.")]
        [StringLength(30, ErrorMessage = "Last name is too long.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Company is Required.")]
        [StringLength(50, ErrorMessage = "Company is too long.")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Contact number is Required.")]
        [StringLength(20, ErrorMessage = "Contact number is too long.")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Reason is Required.")]
        [StringLength(200, ErrorMessage = "Reason is too long.")]
        public string Reason { get; set; }


        public DateTime Datetime_In { get; set; }
        public DateTime? Datetime_Out { get; set; }
        public string Ip_Address { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        public bool IsSignedIn
        {
            get
            {
                return !Datetime_Out.HasValue;
            }
        }
    }
}
