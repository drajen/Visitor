using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Visitor.Models
{
    public class UpdateStatus
    {
        public int ID;
        public bool Success = false;

        public UpdateStatus(int id, bool success)
        {
            ID = id;
            Success = success;
        }
    }
}
