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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string ContactNumber { get; set; }
        public string Reason { get; set; }
  

        public DateTime Datetime_In { get; set; }
        public DateTime? Datetime_Out { get; set; }
        public string Ip_Address { get; set; }
    }
}
