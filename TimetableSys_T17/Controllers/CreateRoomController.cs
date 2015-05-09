using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimetableSys_T17;
using System.Text.RegularExpressions;

namespace TimetableSys_T17.Controllers
{
    public class CreateRoomController : Controller
    {
        private TimetableDbEntities db = new TimetableDbEntities();

        private string roomCodeToUpper(string roomCode)
        {
            string buildingCode = roomCode.Substring(0, roomCode.IndexOf("."));
            buildingCode = buildingCode.ToUpper();

            string result = buildingCode + roomCode.Substring(1);

            return result;
        }


        private int isLab(bool lab) //Converts checkbox value for lab from boolean to int for database
        {
            int labValue = 0;

            if (lab)
            {
                labValue = 1;
            }
            else
            {
                labValue = 0;
            }

            return labValue;
        }

        private bool checkRoomCode(string roomCode) 
        {
            Regex reg = new Regex(@"^[A-Za-z]{1,3}(\.)[0-9](\.)[0-9]{1,3}[a-z]?$");

            return reg.IsMatch(roomCode);
        }//Regular expression check for room code

        private bool validate(string roomCode) //Checks that room with same name hasn't been created before
        {

            bool returnVal = false;

            using (var db = new TimetableDbEntities())
            {
                //Get all roomID where the roomName is the same as the one the user inputted
                var room = from roomDB in db.Rooms where roomDB.roomCode == roomCode select roomDB.roomCode;

                if (room.Count() == 0)
                {
                    //Allows room to be created if there are no existing rooms with same name
                    returnVal = true;
                }
            }

            return returnVal;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //Change when deptID is carried through
            var buildings = db.DeptInfoes.Where(a => a.deptID == 5).Select(b => b.Buildings.Select(c => c.buildingID)).ToList();

            List<Room> validRooms = new List<Room>();

            foreach (var i in buildings[0])
            {

                var query = db.Rooms.Where(a => a.buildingID == i && a.@private == 1).ToList();

                foreach (var j in query)
                {
                    validRooms.Add(j);
                }
            }

            ViewBag.createdRooms = validRooms;

            return View();
        }


        [HttpGet]
        public ActionResult Create()
        {
            //Change when deptID is carried through
            var buildings = db.DeptInfoes.Where(a => a.deptID == 5).Select(b => b.Buildings.Select(c => c.buildingID)).ToList();

            List<Building> validBuildings = new List<Building>();

            foreach(var i in buildings[0]){
                var query = db.Buildings.Where(a => a.buildingID == i).First();
                validBuildings.Add(query);
            }

            var options = validBuildings.AsEnumerable().Select(s => new
            {
                buildingID = s.buildingID,
                Info = string.Format("{0} - {1}", s.buildingCode, s.buildingName)
            });

            var facilityNames = db.Facilities.ToList();
            ViewBag.facilities = facilityNames;

            ViewBag.buildingID = new SelectList(options, "buildingID", "Info");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.CreateRoomModel room, bool Labe, IEnumerable<int> fac)
        {
            //Error when no building chosen
            if (room.roomCode == null)
            {
                return View();
            }

            Room room1 = new Room();

            room.roomCode = roomCodeToUpper(room.roomCode);

            var bID = room.buildingID;
            var bCode = room.roomCode;
            bCode = bCode.Substring(0, bCode.IndexOf("."));

            var result = db.Buildings.Where(s => s.buildingCode.Contains(bCode)).Select(s => s.buildingID);


            if (ModelState.IsValid && validate(room.roomCode) && result.First() == bID && checkRoomCode(room.roomCode))
            {

                //Data for new room instance
                room1.roomCode = room.roomCode;
                room1.capacity = room.capacity;
                room1.buildingID = room.buildingID;
                room1.lab = isLab(Labe);
                room1.@private = 1;

                if (fac != null)
                {
                    foreach (var i in fac)
                    {
                        //Gets facility object from db for correct id and adds it to room
                        room1.Facilities.Add(db.Facilities.Where(a => a.facilityID == i).First());
                    }
                }
                // Add the new object to the Rooms table.
                db.Rooms.Add(room1);
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            var bID = room.buildingID;

            var facilities = db.Rooms.Where(a => a.roomID == id).Select(a => a.Facilities.Select(c => c.facilityName).ToList()).ToList();

            var name = db.Buildings.Where(s => s.buildingID == bID).Select(s => s.buildingName);

            ViewBag.Fac = facilities;
            ViewBag.count = facilities.First().Count();
            ViewBag.building = name.First();
            ViewBag.Lab = room.lab;

            return View(room);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            //Change when deptID is carried through
            var buildings = db.DeptInfoes.Where(a => a.deptID == 5).Select(b => b.Buildings.Select(c => c.buildingID)).ToList();

            List<Building> validBuildings = new List<Building>();

            foreach (var i in buildings[0])
            {
                var query = db.Buildings.Where(a => a.buildingID == i).First();
                validBuildings.Add(query);
            }

            var options = validBuildings.AsEnumerable().Select(s => new
            {
                buildingID = s.buildingID,
                Info = string.Format("{0} - {1}", s.buildingCode, s.buildingName)
            });

            var selected = db.Rooms.Where(a => a.roomID == id).Select(a => a.Facilities.Select(c => c.facilityID)).ToList();
            ViewBag.selectedFac = selected[0];

            var facilityNames = db.Facilities.ToList();
            ViewBag.facilities = facilityNames;

            ViewBag.buildingID = new SelectList(options, "buildingID", "Info", db.Buildings.Where(a => a.buildingID == room.buildingID).Select(a => a.buildingID).First());
            ViewBag.Lab = room.lab;
            ViewBag.@Private = room.@private;

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Editing everything works, not complete with error checking
        public ActionResult Edit([Bind(Include = "roomID,roomCode,buildingID,capacity")] Room room, bool Labe, IEnumerable<int> fac)
        {

            room.roomCode = roomCodeToUpper(room.roomCode);

            var oldRoomCode = db.Rooms.Where(x => x.roomID == room.roomID).Select(x => x.roomCode).First();
            var newRoomCode = room.roomCode;
            var bCode = newRoomCode.Substring(0, newRoomCode.IndexOf("."));
            var result = db.Buildings.Where(s => s.buildingCode.Contains(bCode)).Select(s => s.buildingID);

            //Get all roomID where the roomName is the same as the one the user inputted
            var roomings = from roomDB in db.Rooms where roomDB.roomCode == newRoomCode select roomDB.roomCode;

            if (result.First() == room.buildingID && (roomings.Count() == 0 || oldRoomCode == newRoomCode) && checkRoomCode(room.roomCode))
            {

                if (Labe)
                {
                    room.lab = 1;
                }
                else
                {
                    room.lab = 0;
                }

                room.@private = 1;

                if (ModelState.IsValid)
                {
                    //Updates the room with the correct set of facilities, either remove or add is allowed
                    Room postAttached = db.Rooms.Where(x => x.roomID == room.roomID).First();
                    room.Facilities = postAttached.Facilities;
                    room.Facilities.Clear();
                    if (fac != null)
                    {
                        foreach (int f in fac)
                        {
                            room.Facilities.Add(db.Facilities.Where(x => x.facilityID == f).First());
                        }
                    }
                    //Updates old version of room with edited values, then saves to database
                    db.Entry(postAttached).CurrentValues.SetValues(room);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            var selected = db.Rooms.Where(a => a.roomID == room.roomID).Select(a => a.Facilities.Select(c => c.facilityID)).ToList();
            ViewBag.selectedFac = selected[0];

            var facilityNames = db.Facilities.ToList();

            ViewBag.facilities = facilityNames;

            var options = db.Buildings.AsEnumerable().Select(s => new
            {
                buildingID = s.buildingID,
                Info = string.Format("{0} - {1}", s.buildingCode, s.buildingName)
            });

            ViewBag.buildingID = new SelectList(options, "buildingID", "Info");

            return View(room);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            var bID = room.buildingID;

            var facilities = db.Rooms.Where(a => a.roomID == id).Select(a => a.Facilities.Select(c => c.facilityName).ToList()).ToList();

            var name = db.Buildings.Where(s => s.buildingID == bID).Select(s => s.buildingName);
            ViewBag.count = facilities.First().Count();
            ViewBag.Fac = facilities;
            ViewBag.building = name.First();
            ViewBag.Lab = room.lab;

            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);

            //Gets all facilities ID for room
            var fac = db.Rooms.Where(a => a.roomID == id).Select(a => a.Facilities.Select(c => c.facilityID).ToList()).ToList();

            //Loops through every facility a room has and deletes it
            foreach (var i in fac[0])
            {
                var fa = db.Facilities.Where(a => a.facilityID == i).First();
                room.Facilities.Remove(fa);
            }

            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}

//TO DO: - Error check, regexp for roomName