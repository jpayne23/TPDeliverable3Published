using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimetableSys_T17.Models
{

    public static class userLogged
    {

        static string _usrName;
        static int _deptID;

        public static string UserName
        {
            get { return _usrName; }
            set { _usrName = value; }
        }

        public static int usrId
        {

            get { return _deptID; }
            set { _deptID = value; }

        }
        
    }

    public class ViewModel
    {

 
        public int requestID { get; set; }
        public int? userID { get; set; }
        public String email { get; set; }
        public int? moduleID { get; set; }
        public String moduleCode { get; set; }
        public int? sessionTypeID { get; set; }
        public String sessionType { get; set; }
        public int? dayID { get; set; }
        public String day { get; set; }
        public int? periodID { get; set; }
        public int? sessionLength { get; set; }
        public int? semester { get; set; }
        public int? round { get; set; }
        public int? year { get; set; }
        public int? priority { get; set; }
        public int? adhoc { get; set; }
        public String specialRequirement { get; set; }
        public int? statusID { get; set; }
        public String status { get; set; }
        public String weekID { get; set; }
        public String week { get; set; }
        public List<string> room { get; set; }
    }
}