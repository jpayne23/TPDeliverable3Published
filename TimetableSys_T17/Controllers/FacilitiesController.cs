using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimetableSys_T17;

namespace TimetableSys_T17.Controllers
{
    public class FacilitiesController : Controller
    {
        private TimetableDbEntities db = new TimetableDbEntities();

        private bool checkDuplicate(string facName) //Checks that room with same name hasn't been created before
        {

            bool returnVal = false;

            //Get all roomID where the roomName is the same as the one the user inputted
            var result = db.Facilities.Where(a => a.facilityName.Contains(facName)).Select(a => a.facilityName);

            if (result.Count() == 0)
            {
                //Allows facility to be created if there are no existing facilities with same name
                returnVal = true;
            }

            return returnVal;
        }


        // GET: Facilities
        public ActionResult Index()
        {
            //var numFac = db.Rooms.Select(a => a.Facilities.Select(b => b.facilityID)).ToList();
            //var facID = db.Facilities.Select(a => a.facilityID).ToList();

            //int[] result = new int[facID.Count()]; // { 0,0,0,0,0,0,0,0,0,0,0,0,0,0 };

            //for (var k = 0; k < facID.Count(); k++ )
            //{
            //    result[k-1] = 0;
            //}

            //for (var i = 0; i < numFac.Count(); i++ )
            //{
            //    for(var j = 0; j < numFac[i].Count(); j++)
            //    {
            //        result[j - 1]++;
            //    }
            //}

            //ViewBag.NumFac = result;

            return View(db.Facilities.ToList());
        }

        // GET: Facilities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // GET: Facilities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Facilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "facilityID,facilityName")] Facility facility)
        {
            ViewBag.error = "";

            if (!checkDuplicate(facility.facilityName)) 
            {
                ViewBag.error = "Facility already exists";
                return View(facility);
            }

            if (ModelState.IsValid && checkDuplicate(facility.facilityName))
            {
                db.Facilities.Add(facility);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facility);
        }

        // GET: Facilities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "facilityID,facilityName")] Facility facility)
        {

            var fac = db.Facilities.Where(a => a.facilityID == facility.facilityID).Select(a => a.facilityName);

            ViewBag.error = "";

            if (!checkDuplicate(facility.facilityName) && fac.First() != facility.facilityName)
            {
                ViewBag.error = "Facility already exists";
                return View(facility);
            }

            if (ModelState.IsValid && (checkDuplicate(facility.facilityName) || fac.First() == facility.facilityName))
            {


                Facility postAttached = db.Facilities.Where(x => x.facilityID == facility.facilityID).First();

                //Updates old version of room with edited values, then saves to database
                db.Entry(postAttached).CurrentValues.SetValues(facility);
                //db.Entry(facility).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facility);
        }

        // GET: Facilities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Facility facility = db.Facilities.Find(id);
            db.Facilities.Remove(facility);
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
