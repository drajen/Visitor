using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Visitor.Entities
{
    [TableName("VISITORS")]
    public class VisitorEntity
    {
        [Column("ID")] public int Id { get; set; }
        [Column("NAME")] public string Name { get; set; }
        [Column("COMPANY")] public string Company { get; set; }
        [Column("DATETIME_IN")] public DateTime Datetime_In { get; set; }
        [Column("DATETIME_OUT")] public DateTime? Datetime_Out { get; set; }
        [Column("IP_ADDRESS")] public string Ip_Address { get; set; }
    }
}
