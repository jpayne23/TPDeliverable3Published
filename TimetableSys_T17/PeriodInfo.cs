//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace TimetableSys_T17
{
    public partial class PeriodInfo
    {
        public PeriodInfo()
        {
            this.Requests = new HashSet<Request>();
        }
    
        public int periodID { get; set; }
        public string time { get; set; }
    
        public virtual ICollection<Request> Requests { get; set; }
    }
    
}