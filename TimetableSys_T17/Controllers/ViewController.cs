using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSys_T17.Models;

namespace TimetableSys_T17.Controllers
{
    public class ViewController : Controller
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


        //
        // GET: /View/
        public ActionResult Index(string sortOrder, int? roundID, int? cancelledID, string moduleCode, int? semester, int? day, int? status, int? year)
        {
            //get db and run query

            if (userLogged.UserName == null)
            {
                userLogged.UserName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase((String)TempData["deptLogin"]);
                userLogged.usrId = (int)TempData["usrId"];
            }



            

            var db = new TimetableDbEntities();
            var getRequests = from t in db.Requests
                              select t;

            var getRounds = from t in db.RoundInfoes select t.round;
            @ViewBag.rounds = getRounds;

            var moduleCodes = db.Modules.Where(f => f.deptID == userLogged.usrId).Select(a => a.modCode).ToList();
            var lecturer = db.LecturerInfoes.Where(f => f.deptID == userLogged.usrId).Select(a => a.name).ToList();

            List<string> codeOrName = new List<string>();
            codeOrName.Add(moduleCode);
            //codeOrName.Add(lecturer);
            @ViewBag.moduleCodes = moduleCodes;

            if (roundID != null)
            {
                getRequests = getRequests.Where(t => t.round == roundID);
            }
            if (moduleCode != null && moduleCode != "")
            {
                var getModID = db.Modules.Where(t => t.modCode == moduleCode).Select(o => o.moduleID).FirstOrDefault();

                getRequests = getRequests.Where(t => t.moduleID == getModID);
            }
            if (semester != null)
            {
                getRequests = getRequests.Where(t => t.semester == semester);
            }
            if (day != null)
            {
                getRequests = getRequests.Where(t => t.dayID == day);
            }
            if (status != null)
            {
                getRequests = getRequests.Where(t => t.statusID == status);
            }




            if (cancelledID != null)
            {
                //var deleteRequest = (from del in db.Requests where del.requestID == cancelledID select del).First();
                // you want to change. 
                var updateStatus =
                    (from del in db.Requests
                     where del.requestID == cancelledID
                     select del).Single();

                updateStatus.statusID = 5;
                db.SaveChanges();
            }



            if (year == 2014)
            {
                getRequests = getRequests.Where(t => t.year == 2014);
            }
            if (year == 2015 || year == null)
            {
                getRequests = getRequests.Where(t => t.year == 2015);
            }



            //sort dependent from view  
            ViewBag.IDSortPram = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.ModuleSortPram = sortOrder == "Module" ? "module_desc" : "Module";
            ViewBag.DateSortPram = sortOrder == "Date" ? "date_desc" : "Date";
            var reqArray = getRequests.ToArray();
            //switch and run sort
            switch (sortOrder)
            {
                case "id_desc":
                    getRequests = getRequests.OrderByDescending(s => s.requestID);
                    reqArray = getRequests.ToArray();
                    break;
                case "Date":
                    getRequests = getRequests.OrderByDescending(s => s.year);
                    reqArray = getRequests.ToArray();
                    break;
                case "date_desc":
                    getRequests = getRequests.OrderBy(s => s.year);
                    reqArray = getRequests.ToArray();
                    break;
                case "Module":
                    getRequests = getRequests.OrderBy(f => f.Module.modCode);
                    reqArray = getRequests.ToArray();
                    break;
                case "module_desc":
                    getRequests = getRequests.OrderByDescending(s => s.moduleID);
                    reqArray = getRequests.ToArray();
                    break;
                default:
                    getRequests = getRequests.OrderBy(s => s.requestID);
                    reqArray = getRequests.ToArray();
                    break;
            }







            List<Models.ViewModel> requestList = new List<Models.ViewModel>();

            foreach (var x in reqArray)
            {
                Models.ViewModel tmp = new Models.ViewModel();


                tmp.requestID = x.requestID;
                tmp.moduleID = x.moduleID;
                tmp.periodID = x.periodID;
                tmp.priority = x.priority;
                tmp.round = x.round;
                tmp.semester = x.semester;
                tmp.sessionLength = x.sessionLength;
                tmp.sessionTypeID = x.sessionTypeID;
                tmp.specialRequirement = x.specialRequirement;
                tmp.statusID = x.statusID;
                tmp.year = x.year;
                tmp.dayID = x.dayID;
                tmp.adhoc = x.adhoc;
                tmp.userID = x.userID;
                tmp.weekID = formatWeeks(x.week);

                var roomCodes = db.Requests.Join(db.Modules, a => a.moduleID, d => d.moduleID, (a, d) => new { a.moduleID, d.modCode }).Where(a => a.moduleID == x.moduleID).Select(d => d.modCode);

                tmp.moduleCode = roomCodes.FirstOrDefault();
                //var roomCodeName = db.Requests.Where(a => a.requestID == x.requestID).Select(a => a.RoomRequests.Select(c => c.ToList()).ToList();

                var roomCodeName = db.Requests.Where(a => a.requestID == x.requestID).Select(a => a.RoomRequests.Select(c => c.roomID)).FirstOrDefault();

                var roomIDList = roomCodeName;
                List<string> roomCodes2 = new List<string>();
                foreach (var i in roomIDList)
                {
                    var getRoomCode = db.Rooms.Where(a => a.roomID == i).Select(b => b.roomCode).First();
                    roomCodes2.Add(getRoomCode);
                }
                tmp.room = roomCodes2;
                var dayName = db.Requests.Join(db.DayInfoes, a => a.dayID, d => d.dayID, (a, d) => new { a.dayID, d.day }).Where(a => a.dayID == x.dayID).Select(d => d.day);
                tmp.day = dayName.FirstOrDefault();

                var statusName = db.Requests.Join(db.StatusInfoes, a => a.statusID, d => d.statusID, (a, d) => new { a.statusID, d.status }).Where(a => a.statusID == x.statusID).Select(d => d.status);
                tmp.status = statusName.FirstOrDefault();




                var sessionTypeName = db.Requests.Join(db.SessionTypeInfoes, a => a.sessionTypeID, d => d.sessionTypeID, (a, d) => new { a.sessionTypeID, d.sessionType }).Where(a => a.sessionTypeID == x.sessionTypeID).Select(d => d.sessionType);
                tmp.sessionType = sessionTypeName.FirstOrDefault();

                var email = db.Requests.Join(db.Users, a => a.userID, d => d.userID, (a, d) => new { a.userID, d.email }).Where(a => a.userID == x.userID).Select(d => d.email);
                tmp.email = email.FirstOrDefault();
                requestList.Add(tmp);



            }

            var example = requestList.ToList();
            return View(requestList);
        }
    }
}