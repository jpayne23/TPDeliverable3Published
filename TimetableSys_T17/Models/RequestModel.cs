using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimetableSys_T17.Models
{
    public class RequestModel
    {

        public List<string> parkName { get; set; }
        public List<string> buildingName { get; set; }
        public List<string> roomCode { get; set; }
        public List<string> facilities { get; set; }
        public List<string> moduleCode { get; set; }
        public List<string> moduleTitle { get; set; }
        public List<string> roomType { get; set; }

    }
    
    
}