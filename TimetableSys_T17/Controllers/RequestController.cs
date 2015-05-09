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

            var req_line = from datesTables in _db.RoundInfoes select datesTables;
            var dates_list = req_line.ToList();

            DateTime current_date = DateTime.Today;

            Int16 rs_day = 0, rs_month = 0, re_day = 0, re_month = 0;

            for (Int16 i = 0; i < 3; i++)
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
                
            return round;
        }

        protected Int16? ReturnSemester()
        {
            Int16? semester = null; // Make this null, so that error is thrown if problem. (DB wont accept null).
            if (DateTime.Today.Month > 8)
            {

                semester = 1;

            }
            else
            {

                semester = 2;

            }

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


        // Private

        public void SubmitRoundI(string room, List<string> facilities, string module_code, string module_title, string session_type)
        {

            if (isValidInput("Rooms", room) && isValidInput("Modules_C", module_code) && isValidInput("Modules_T", module_title) && isValidInput("SessionTypeInfo", session_type)) // && FREE - Round 2, 3, then ELSE if this.+adhoc, else reject
            {

                List<Int16> weeksSelected = new List<Int16>(15) {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0};


                Request submitNewRequest = new Request
                {

                    userID = 3,
                    moduleID = _db.Modules.Where(x => x.modCode == module_code).Select(x => x.moduleID).First(),
                    sessionTypeID = _db.SessionTypeInfoes.Where(x => x.sessionType == session_type).Select(x => x.sessionTypeID).First(),
                    dayID = 2,
                    periodID = 1,
                    sessionLength = 2,
                    semester = ReturnSemester(),
                    round = ReturnRound(),
                    year = DateTime.Today.Year,
                    priority = 0,
                    adhoc = 0, // pass this through switch statement
                    specialRequirement = "Cake must be provided!", // Complete this
                    statusID = 2,
                   
                    
                    
                };

                Debug.WriteLine(submitNewRequest.semester + " SEMESTER");
                Debug.WriteLine(submitNewRequest.round + " ROUND");
                Debug.WriteLine(submitNewRequest.year + " YEAR");

                _db.Requests.Add(submitNewRequest);
                _db.SaveChanges(); // -- -- -- -- -- TRY STATEMENT, ON FAIL RETURN ERR -- -- -- -- -- --

               // _db.Weeks.Add(temp);
               // _db.SaveChanges();
            
            
            
            
            
            }

        }

        protected void SubmitRoundII_III()
        {

        }

        protected void SubmitAdHoc()
        {
            // If Adhoc finds a pending request, it'll decline pending and approve this
            // Because there should not be any pending at this stage - assume admin is on it.


        }

        protected void SubmitEdit()
        {

            // Edit, check before this is executed if edit != original

        }


        public void ReturnResult(Boolean edit)
        {
            /*
              Return result will execute the appropriate submit request, and return, if any, a suitable 
              view, i.e. error - adhoc - Room taken etc.*/

            if (!edit)
            {
                int round = ReturnRound();

                switch (round)
                {

                    case 1:
                        // Do Round 1 - i.e. priority is different
                        //SubmitRoundI(); break;
                    case 2:
                        // Do Round 2 & 3 here, as neither have a difference. function will take arg, r, 1 | 3
                        SubmitRoundII_III(); break;
                    case 3:
                        // Adhoc here, auto approval;
                        SubmitAdHoc(); break;
                    default:
                        // Adhoc? Throw a message? They shouldn't reach this far, unless dates are wrong in db.
                        Console.WriteLine("temp 4"); break; // Return Error Page/Message

                }
            }
            else
            {

                SubmitEdit();

            }
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
        public JsonResult RequestModelUpdaterOptional2(Int16 which_call, string park_names, string building_names, string room_names, string facility_names)
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

                    var firstFilter = _db.Facilities.Select(x => new  { x.facilityName,  Rooms = x.Rooms.Select(y => y.roomCode).ToList() } ).ToList();
                    var wotsits = "SELECT Room.roomCode FROM Room JOIN RoomFacility on Room.roomID = RoomFacility.roomID JOIN Facility on RoomFacility.facilityID = Facility.facilityID WHERE facilityName = 'Chalk Board' OR facilityName = 'Data Projector' GROUP BY Room.roomCode HAVING COUNT(*) = 2";
                   //var temp = _db.SqlQuery(wotsits);
 
      

                    Debug.WriteLine("==================================");
                    foreach (var i in firstFilter)
                    {

                        Debug.WriteLine(i);

                    }


                    Debug.WriteLine("==================================");

                    
                    


                    break;
            }
            

            return Json(local, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult RequestModelUpdaterCompulsory(string module_code, string module_title, string room_type)
        {

            RequestModel local_return = new RequestModel();

            if (module_code != "" && module_title == "")
            {

                IQueryable<string> module_titles = _db.Modules.Where(x => x.modCode == module_code).Select(x => x.modTitle);
                             
                local_return.moduleTitle = module_titles.ToList();

            }
            else if (module_code == "" && module_title != "")
            {

                IQueryable<string> module_codes = _db.Modules.Where(x => x.modTitle == module_title).Select(x => x.modCode);

                local_return.moduleCode = module_codes.ToList();
            }
            else
            {

                IQueryable<string> module_codes = _db.Modules.Select(x => x.modCode);
                IQueryable<string> module_titles = _db.Modules.Select(x => x.modTitle);
                IQueryable<string> room_types = _db.SessionTypeInfoes.Select(x => x.sessionType);

                local_return.moduleCode = module_codes.ToList();
                local_return.moduleTitle = module_titles.ToList();
                local_return.roomType = room_types.ToList();
            }

            return Json(local_return, JsonRequestBehavior.AllowGet);
        }
	}
}