using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BO
{
    public class BaseModel
    {
        public DateTime CreatedDate { get; set; } = DateTime.Today;

        public DateTime ModifiedDate { get; set; } = DateTime.Today;

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public EntityState State { get; set; } = EntityState.None;
    }
}