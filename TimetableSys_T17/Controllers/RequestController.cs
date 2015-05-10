using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSys_T17.Models;

using System.Data.Entity;

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TimetableSys_T17.Controllers
{
    public class RequestController : Controller
    {
        
        // GET: /Request/
        // Done by Adam Dryden.
   
        /***********************
         *  NOTES: 
         *  
         *  Check if changes made to edit,
         *  pass 0/1 for edit? maybe catch it
         *  in the view using js? 
         *  
         * *********************/


        private TimetableDbEntities _db = new TimetableDbEntities();

        protected Int16 ReturnRound()
        {

            Int16 round = 1;
            bool satisfied = false;

            var req_line = _db.RoundInfoes;

            var dates_list = req_line.ToList();

            DateTime current_date = DateTime.Today;

            Int16 rs_day = 0, rs_month = 0, re_day = 0, re_month = 0;

            for (Int16 i = 0; i < dates_list.Count(); i++)
            {

                String[] round_start = dates_list[i].startDate.Split(new String[] { "/" }, StringSplitOptions.None);
                String[] round_end = dates_list[i].endDate.Split(new String[] { "/" }, StringSplitOptions.None);

                try
                {
                    // Convert strings to integers :-

                    rs_day = Convert.ToInt16(round_start[0]); // round start day. 
                    rs_month = Convert.ToInt16(round_start[1]); // round start month

                    re_day = Convert.ToInt16(round_end[0]); // round end day
                    re_month = Convert.ToInt16(round_end[1]); // round end month

                }
                catch (FormatException error)
                {

                    Debug.WriteLine("ReqCon L51 - ConStrToInt" + error); // Render error view.

                }
                finally
                {

                    if (current_date.Day >= rs_day && current_date.Day <= re_day && current_date.Month >= rs_month && current_date.Month <= re_month)
                    {

                        round = i++;
                        satisfied = true;

                    }
                }

                if (satisfied)
                {
                    break;
                }
            }

            round++;

            return round;
        }

        protected Int16? ReturnSemester()
        {
            Int16? semester = null; // Make this null, so that error is thrown if problem. (DB wont accept null).
            if (DateTime.Today.Month > 8 || DateTime.Today.Month < 2)
            {

                semester = 1;

            }
            else
            {

                semester = 2;

            }
            ViewBag.semester = "VIEWBAG!!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            return semester;

        }

        protected bool isValidInput(string table, string input)
        {
            bool return_val = false;
            Int16 query = 0;

            switch (table)
            {

                case "Parks":
                    query = (Int16)_db.Parks.Where(x => x.parkName == input).Select(x => x.parkID).FirstOrDefault(); 
                    break;

                case "Rooms":
                    query = (Int16)_db.Rooms.Where(x => x.roomCode == input).Select(x => x.roomID).FirstOrDefault();
                    break;

                case "Buildings":
                    query = (Int16)_db.Buildings.Where(x => x.buildingName == input).Select(x => x.buildingID).FirstOrDefault(); 
                    break;

                case "Modules_C":
                    query = (Int16)_db.Modules.Where(x => x.modCode == input).Select(x => x.moduleID).FirstOrDefault(); 
                    break;
                    
                case "Modules_T":
                    query = (Int16)_db.Modules.Where(x => x.modTitle == input).Select(x => x.moduleID).FirstOrDefault(); 
                    break;

                case "SessionTypeInfo":
                    query = (Int16)_db.SessionTypeInfoes.Where(x => x.sessionType == input).Select(x => x.sessionTypeID).FirstOrDefault();
                    break;

            }

            if (query != 0) {

                return_val = true;

            }
     

            return return_val;

        }

        public ActionResult Index()
        {

            return View();
        }

        private List<string> UniqFacilities(List<List<string>> input) {

            List<string> placeholder = new List<string>();

            foreach(var i in input)
            {
                foreach(var y in i)
                {

                    if (!(placeholder.Contains(y))) {

                        placeholder.Add(y);

                    }
                }
            }

            return placeholder;


        }

        protected List<string> returnStripped(string input)
        {

            List<string> return_string = Regex.Split(input, "\",\"").ToList();

            if (return_string[0] != "[]")
            {

                return_string[0] = return_string[0].Substring(2, (return_string[0].Length - 2));
                return_string[return_string.Count() - 1] = return_string[return_string.Count() - 1].Substring(0, (return_string[return_string.Count() - 1].Length - 2));

            }


            return return_string;

        }

        [HttpGet]
        public JsonResult RequestModelUpdaterOptional2(Int16 which_call, string park_names, string building_names, string room_names, string facility_names, string module_code, string module_title, string session_type, Int16? room_i, Int16? room_ii, Int16? room_iii)
        {
            
            RequestModel local = new RequestModel();

            switch (which_call) {

                case 1: IQueryable<string> park_name = _db.Parks.Select(x => x.parkName); local.parkName = park_name.ToList(); break;
                case 2: IQueryable<string> building_name_II = _db.Buildings.Select(x => x.buildingName); local.buildingName = building_name_II.ToList(); break;
                case 3: IQueryable<string> room_codes_II = _db.Rooms.Select(x => x.roomCode); local.roomCode = room_codes_II.ToList(); break;
                case 4: IQueryable<string> available_facilities_II = _db.Facilities.Select(x => x.facilityName); local.facilities = available_facilities_II.ToList(); break;
                case 5:

                    List<string> inputParks_V = returnStripped(park_names);
                    List<Int16> inputParkIDs = _db.Parks.Where(x => inputParks_V.Contains(x.parkName)).Select(x => (Int16)x.parkID).ToList();
                    IQueryable<string> building_name_V = _db.Buildings.Where(x => inputParkIDs.Contains((Int16)x.parkID)).Select(x => x.buildingName); local.buildingName = building_name_V.ToList();
                    IQueryable<string> room_codes_V = _db.Rooms.Join(_db.Buildings, x => x.buildingID, y => y.buildingID, (x, y) => new { x.roomCode, y.buildingName }).Where(x => local.buildingName.Contains(x.buildingName)).Select(x => x.roomCode); local.roomCode = room_codes_V.ToList();
                    IQueryable<List<string>> available_facilities_V = _db.Rooms.Where(x => local.roomCode.Contains(x.roomCode)).Select(x => x.Facilities.Select(y => y.facilityName).ToList()); local.facilities = UniqFacilities(available_facilities_V.ToList());
                    
                    break;
                case 6:
                    
                    List<string> inputBuildings_VI = returnStripped(building_names);
                    IQueryable<string> room_codes_VI = _db.Rooms.Join(_db.Buildings, x => x.buildingID, y => y.buildingID, (x, y) => new { x.roomCode, y.buildingName }).Where(x => inputBuildings_VI.Contains(x.buildingName)).Select(x => x.roomCode); local.roomCode = room_codes_VI.ToList();
                    IQueryable<List<string>> available_facilities_VI = _db.Rooms.Where(x => local.roomCode.Contains(x.roomCode)).Select(x => x.Facilities.Select(y => y.facilityName).ToList()); local.facilities = UniqFacilities(available_facilities_VI.ToList());

                    break;
                case 7:

                    List<string> inputRooms_VII = returnStripped(room_names);
                    IQueryable<List<string>> available_facilities_VII = _db.Rooms.Where(x => inputRooms_VII.Contains(x.roomCode)).Select(x => x.Facilities.Select(y => y.facilityName).ToList()); local.facilities = UniqFacilities(available_facilities_VII.ToList());
                    break;
                case 8:

                    List<string> inputFacilities_VIII = returnStripped(facility_names);
                    List<string> room_codes_VIII = _db.Rooms.Where(x => (inputFacilities_VIII.Intersect(x.Facilities.Select(y => y.facilityName).ToList())).Count() == inputFacilities_VIII.Count()).Select(x => x.roomCode).ToList(); local.roomCode = room_codes_VIII;

                   // This works, however, because Len = 0, jQuery executes call 7.
                   

                    break;
                case 9: IQueryable<string> return_modules = _db.Modules.Select(x => x.modCode); local.moduleCode = return_modules.ToList(); break;
                case 10:

                    List<string> input_moduleCode_X = returnStripped(module_code);
                    IQueryable<string> module_title_X = _db.Modules.Where(x => input_moduleCode_X.Contains(x.modCode)).Select(x => x.modTitle); local.moduleTitle = module_title_X.ToList();

                    break;
                case 11: IQueryable<string> return_titles = _db.Modules.Select(x => x.modTitle); local.moduleTitle = return_titles.ToList(); break;
                case 12:

                    List<string> input_moduleTitle_XI = returnStripped(module_title);
                    IQueryable<string> module_code_XI = _db.Modules.Where(x => input_moduleTitle_XI.Contains(x.modTitle)).Select(x => x.modCode); local.moduleCode = module_code_XI.ToList();

                    break;
                case 13: IQueryable<string> return_sessions = _db.SessionTypeInfoes.Select(x => x.sessionType); local.sessionType = return_sessions.ToList(); break;
                case 14:
                
                    List<string> room_codes_XIV = returnStripped(room_names);
                    List<Int16> room_ids_XIV = _db.Rooms.Where(x => room_codes_XIV.Contains(x.roomCode)).Select(x => (Int16)x.roomID).ToList();
                    IQueryable<List<tableViewTemplate>> return_room_base = _db.RoomRequests.Where(x => room_ids_XIV.Contains((Int16)x.roomID)).Where(x => (x.Requests.Select(y => y.statusID)).Contains(1) || (x.Requests.Select(y => y.statusID)).Contains(3)).Select(x => x.Requests.Select(y => new tableViewTemplate { dayID = y.dayID, periodID = y.periodID, sessionLength = y.sessionLength, semester = y.semester, week = y.week }).ToList());

                    local.tableRequest = return_room_base.ToList();

                    break;
            }
            

            return Json(local, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitThisThing(Int16 which_call, string park_names, string building_names, string room_names, string facility_names, string module_code, string module_title, string session_type, string weeks, string day, string dayInfo)
        {
            RequestModel local = new RequestModel();
            local.response = "Oops! Something has gone terribly wrong. Call 0800 70U80R0UGH 4 45515574NC3";

            string additional = "";

            List<string> rooms = returnStripped(room_names);

            if (rooms.Count() == 0)
            {

                additional += "¦" + building_names + park_names;

            }


            List<string> modCode = returnStripped(module_code);
            List<string> sessionInfo = returnStripped(session_type);
            string selectedDay = day.Substring(1, (day.Length - 1));
            Int16 tempModuleID = _db.Modules.Where(a => modCode.Contains(a.modCode)).Select(b => (Int16)b.moduleID).First();
            Int16 tempSessionTypeID = _db.SessionTypeInfoes.Where(a => sessionInfo.Contains(a.sessionType)).Select(b => (Int16)b.sessionTypeID).First();
            Int16? tempDayID = _db.DayInfoes.Where(a => selectedDay.Contains(a.day)).Select(b => (Int16)b.dayID).First();
            Int16 tempPeriodID = Convert.ToInt16(dayInfo.Substring(1, 1));
            Int16 tempSessionLength = Convert.ToInt16(dayInfo.Substring(3, 1));
            Int16 tempSemester = (Int16)ReturnSemester();
            Int16 tempRound = ReturnRound();
            Int16 tempYear = (Int16)DateTime.Today.Year;
            string tempWeeks = convertWeeks(toList(weeks));
            List<string> tempFacilities = returnStripped(facility_names);
            List<string> tempRooms = returnStripped(room_names);


            switch (tempRound)
            {

                case 1:

                    try { 
                          
                        pushToDb(tempRooms, tempFacilities, tempModuleID, tempSessionTypeID, tempWeeks, 4, tempDayID, tempPeriodID, tempSessionLength, tempSemester, tempRound, 0, 1, tempYear);  // sort out prio

                        local.response = "Round 1 Request submitted, please keep an eye on your view requests page for any changes.";
                    
                    }
                    catch (Exception x)
                    {
                        
                        local.response = "There appears to be something catastrophic... Have you been tampering with our work? " + x;
                    
                    }

                    break;

                case 2:

                    Int16? checkIfTakenR2 = null;
                    checkIfTakenR2 = _db.Requests.Where(x => x.week.Contains(tempWeeks) && x.statusID == 1 && (x.periodID >= tempPeriodID && x.periodID <= (tempPeriodID+tempSessionLength)) && (tempRooms.Intersect(x.RoomRequests.Select(y => _db.Rooms.Where(z => z.roomID == y.roomID).Select(z => z.roomCode).First())).Count() > 0)).Select(x => (Int16?)x.requestID).First();

                    if (checkIfTakenR2 != null) {

                        local.response = "It appears the rooms you've requested are not available, please try again or contact your administrator.";

                    }else{
                        
                        try { 
                          
                            pushToDb(tempRooms, tempFacilities, tempModuleID, tempSessionTypeID, tempWeeks, 4, tempDayID, tempPeriodID, tempSessionLength, tempSemester, tempRound, 0, 0, tempYear);

                            local.response = "Round 2 Request submitted, please keep an eye on your view requests page for any changes.";
                    
                        }
                        catch (Exception x)
                        {
                        
                            local.response = "There appears to be something catastrophic... Have you been tampering with our work? " + x;
                    
                        }

                    
                    }

                    break;
                    
                case 3:

                    Int16 checkIfTakenR3 = 0;

                    checkIfTakenR3 = _db.Requests.Where(x => x.week.Contains(tempWeeks) && x.statusID == 1 && (x.periodID >= tempPeriodID && x.periodID <= ((tempPeriodID + tempSessionLength) - 1)) && (tempRooms.Intersect(x.RoomRequests.Select(y => _db.Rooms.Where(z => z.roomID == y.roomID).Select(z => z.roomCode).FirstOrDefault())).Count() > 0) && x.dayID == tempDayID).Select(x => (Int16)x.requestID).FirstOrDefault();
                    Debug.WriteLine(checkIfTakenR3);
                    if (checkIfTakenR3 != 0)
                    {
                        
                        local.response = "It appears the rooms you've requested are not available, please try again or contact your administrator.";

                    }else{
                        
                        try { 
                          
                            pushToDb(tempRooms, tempFacilities, tempModuleID, tempSessionTypeID, tempWeeks, 4, tempDayID, tempPeriodID, tempSessionLength, tempSemester, tempRound, 0, 0, tempYear);

                            local.response = "Round 3 Request submitted, please keep an eye on your view requests page for any changes.";
                    
                        }
                        catch (Exception x)
                        {
                        
                            local.response = "There appears to be something catastrophic... Have you been tampering with our work? " + x;
                    
                        }

                    
                    }

                    break;

                case 4:

                    Int16 checkIfTakenAdhoc = 0;

                    checkIfTakenAdhoc =_db.Requests.Where(x => x.week.Contains(tempWeeks) && x.statusID == 1 && (x.periodID >= tempPeriodID && x.periodID <= ((tempPeriodID + tempSessionLength)-1)) && (tempRooms.Intersect(x.RoomRequests.Select(y => _db.Rooms.Where(z => z.roomID == y.roomID).Select(z => z.roomCode).FirstOrDefault())).Count() > 0) && x.dayID == tempDayID).Select(x => (Int16)x.requestID).FirstOrDefault();
                    Debug.WriteLine(checkIfTakenAdhoc);
                    if (checkIfTakenAdhoc != 0)
                    {

                        local.response = "It appears the rooms you've requested are not available, please try again or contact your administrator.";

                    }
                    else
                    {
                        try
                        {

                            pushToDb(tempRooms, tempFacilities, tempModuleID, tempSessionTypeID, tempWeeks, 1, tempDayID, tempPeriodID, tempSessionLength, tempSemester, tempRound, 1, 0, tempYear);

                            local.response = "Your request has been approved.";

                        }
                        catch(Exception x)
                        {

                            local.response = "Great Scott, there's a problem: " + x;

                        }
                    }

                    break;
            }

            return Json(local, JsonRequestBehavior.AllowGet);
        }

        protected void pushToDb(List<string> room_names, List<string>facility_names, Int16 module_id, Int16 session_type, string weeks, Int16 statusID, Int16? day, Int16 periodID, Int16 sessionLength, Int16 semester, Int16 round, Int16 adhoc, Int16 prio, Int16 year)
        {



            Request toSubmit = new Request()
            {
                userID = 2, // sort
                moduleID = module_id,
                sessionTypeID = session_type,
                dayID = day,
                periodID = periodID,
                sessionLength = sessionLength,
                semester = semester,
                round = round,
                year = year,
                priority = prio, // SORT
                adhoc = adhoc,
                specialRequirement = "There must be poop provided",
                statusID = statusID,
                week = weeks
            };

            foreach (var i in facility_names)
            {
                toSubmit.Facilities.Add(_db.Facilities.Where(a => a.facilityName == i).First());
            }

            foreach (var j in room_names)
            {
                RoomRequest temp = new RoomRequest()
                {
                    roomID = _db.Rooms.Where(a => a.roomCode == j).Select(b => b.roomID).First(),
                    groupSize = 50
                };

                toSubmit.RoomRequests.Add(temp);
            }

            _db.Requests.Add(toSubmit);
            _db.SaveChanges();
        }


        protected List<string> toList(string input)
        {

            List<string> these = Regex.Split(input.Substring(1, (input.Length - 2)), ",").ToList();
       
            return these;


        }



        protected string convertWeeks(List<string> weeks)
        {


            string bollocks = "[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]";


            foreach (var x in weeks)
            {
                bollocks = bollocks.Remove(((Convert.ToInt16(x) * 2)-1), 1).Insert(((Convert.ToInt16(x) * 2)-1), "1");

            }

            return bollocks;
        }
	}
}