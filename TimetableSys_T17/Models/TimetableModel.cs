using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimetableSys_T17.Models
{
    public class TimetableModel
    {
        public List<int> req { get; set; }
        public List<int?> semester { get; set; }
        public List<int?> periodID { get; set; }

    }
}