using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library
{
    /// <summary>
    /// trạng thái model
    /// </summary>
    public enum EntityState
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 3,
        Duplicate = 4
    }
}