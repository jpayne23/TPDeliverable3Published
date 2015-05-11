using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class Scotthi
{
    //requestID, x.periodID, x.semester, x.week, x.dayID

    public int reqID;
    public int? periodID;
    public int? semester;
    public string week;
    public int? dayID;
    public int? moduleID;
    public int? sessionTypeID;

}

namespace TimetableSys_T17.Controllers
{
    public class TimetableController : Controller
    {
        private string formatWeeks(string Weeks)
        {
            //[1,1,1,0,0,0,1,0,1,0,0,1,0,0,0]
            var temp = Weeks.Substring(1, Weeks.Length - 2);
            //1,1,1,0,0,0,1,0,1,0,0,1,0,0,0
            List<int> weeksList = temp.Split(',').Select(int.Parse).ToList();
            //
            string result = "";
            int consecutive = 0;
            int startI = 0;
            int endI = 0;
            string selected = "";
            int n = 0;

            foreach (var i in weeksList)
            {
                n++;
                if (i == 1 && n == weeksList.Count())
                {
                    if (consecutive > 1)
                    {
                        startI = n - consecutive;
                        endI = n;
                        selected = startI.ToString() + "-" + endI.ToString() + ", ";
                        result += selected;
                    }
                    else
                    {
                        startI = n;
                        selected = startI.ToString() + ", ";
                        result += selected;
                    }
                }
                else if (i == 1)
                {
                    consecutive++;
                }
                else if (i == 0 && consecutive > 1)
                {
                    startI = n - consecutive;
                    endI = n - 1;
                    selected = startI.ToString() + "-" + endI.ToString() + ", ";
                    result += selected;
                    consecutive = 0;
                }
                else if (i == 0 && consecutive == 1)
                {
                    startI = n - consecutive;
                    selected = startI.ToString() + ", ";
                    result += selected;
                    consecutive = 0;
                }

            }

            return result.Substring(0, result.Length - 2);
        }
        // GET: Timetable
        public ActionResult Index(string modOrLec, string nameOrCode, int? week, int? getSemester)
        {
            //dskufhsdjkfhaidsuf
            var db = new TimetableDbEntities();
            var getLec = db.LecturerInfoes.Where(f => f.deptID == 5).Select(f => f.name).ToList();
            var getCourse = db.DegreeInfoes.Where(d => d.deptID == 5).Select(o => o.degreeName).ToList();

          
            @ViewBag.getLec = getLec;
            @ViewBag.getCourse = getCourse;

            if (modOrLec != null && nameOrCode != null && week != null && getSemester != null)
            {

                var getLecID = db.DegreeInfoes.Where(a => a.degreeName == nameOrCode).Select(b => b.Modules.Select(c => c.moduleID)).FirstOrDefault();

                if (modOrLec == "Lecturer")
                {
                    getLecID = db.LecturerInfoes.Where(a => a.name == nameOrCode).Select(b => b.Modules.Select(c => c.moduleID)).FirstOrDefault();
                }
               
                
                if (getLecID != null)
                {
                    IEnumerable<int> temp = getLecID; // do try catch incase index out of range

                    var getWeekID = db.Requests.Where(x => temp.Contains((int)x.moduleID)).Select(x => x.week).ToList();
                    var getReqID = new List<Scotthi>();
                    foreach (var a in getWeekID)
                    {

                        var aNew = a.Replace("[", "");

                        var aNew2 = aNew.Replace("]", "");

                        string[] weekIDs = aNew2.Split(new char[] { ',' });

                        List<int> weekIDs2 = new List<int>();//all week IDs will be put in here


                        if (weekIDs[(int)week - 1] == "1")
                        {



                            var tempxxx = db.Requests.Where(x => temp.Contains((int)x.moduleID) && x.semester == getSemester && x.week == a).Select(x => new Scotthi { reqID = x.requestID, periodID = x.periodID, semester = x.semester, week = x.week, dayID = x.dayID, moduleID = x.moduleID, sessionTypeID = x.sessionTypeID }).FirstOrDefault();

                            if (tempxxx != null)
                            {

                                getReqID.Add(tempxxx);

                            }
                        }
                    }

                    foreach (var p in getReqID)
                    {

                        var moduleCode = db.Requests.Join(db.Modules, a => a.moduleID, d => d.moduleID, (a, d) => new { a.moduleID, d.modCode }).Where(a => a.moduleID == p.moduleID).Select(d => d.modCode);

                        var moduleName = db.Requests.Join(db.Modules, a => a.moduleID, d => d.moduleID, (a, d) => new { a.moduleID, d.modTitle }).Where(a => a.moduleID == p.moduleID).Select(d => d.modTitle);

                        var type = db.Requests.Join(db.SessionTypeInfoes, a => a.sessionTypeID, d => d.sessionTypeID, (a, d) => new { a.sessionTypeID, d.sessionType }).Where(a => a.sessionTypeID == p.sessionTypeID).Select(d => d.sessionType);

                        var weeks = db.Requests.Select(a => a.week).FirstOrDefault();

                        var roomID = db.Requests.Where(a => a.requestID == p.reqID).Select(a => a.RoomRequests.Select(b => b.roomID)).FirstOrDefault();

                        var getRoomCode = db.Rooms.Where(a => a.roomID == roomID.FirstOrDefault()).Select(b => b.roomCode).First();

                        var getBuildingID = db.Rooms.Where(a => a.roomCode == getRoomCode).Select(b => b.buildingID).FirstOrDefault();

                        var getBuildingName = db.Buildings.Where(a => a.buildingID == getBuildingID).Select(b => b.buildingName).FirstOrDefault();

                        var sessionLength = db.Requests.Where(a => a.requestID == p.reqID).Select(b => b.sessionLength).FirstOrDefault();

                       

                        @ViewBag.error = "good";
                        @ViewBag.req = p;
                        @ViewBag.moduleCode = moduleCode.FirstOrDefault();
                        @ViewBag.moduleName = moduleName.FirstOrDefault();
                        @ViewBag.type = type.FirstOrDefault();
                        @ViewBag.weeks = formatWeeks(weeks);
                        @ViewBag.roomCode = getRoomCode;
                        @ViewBag.buildingName = getBuildingName;
                        @ViewBag.sessionLength = sessionLength;
                        

                    }
                }
            }
            else
            {
                @ViewBag.error = "";
            }
            return View();
        }


    }
}