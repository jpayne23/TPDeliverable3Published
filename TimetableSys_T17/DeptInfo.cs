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
    public partial class DeptInfo
    {
        public DeptInfo()
        {
            this.DegreeInfoes = new HashSet<DegreeInfo>();
            this.LecturerInfoes = new HashSet<LecturerInfo>();
            this.Modules = new HashSet<Module>();
            this.Buildings = new HashSet<Building>();
        }
    
        public int deptID { get; set; }
        public string deptCode { get; set; }
        public string deptName { get; set; }
    
        public virtual ICollection<DegreeInfo> DegreeInfoes { get; set; }
        public virtual ICollection<LecturerInfo> LecturerInfoes { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Building> Buildings { get; set; }
    }
    
}
