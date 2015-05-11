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
    public class RoundsController : Controller
    {
        private TimetableDbEntities db = new TimetableDbEntities();

        // GET: Rounds
        public ActionResult Index()
        {
            return View(db.RoundInfoes.ToList());
        }

        // GET: Rounds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoundInfo roundInfo = db.RoundInfoes.Find(id);
            if (roundInfo == null)
            {
                return HttpNotFound();
            }
            return View(roundInfo);
        }

        // GET: Rounds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rounds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "round,startDate,endDate")] RoundInfo roundInfo)
        {
            ViewBag.error = "";

            var testStart = roundInfo.startDate;
            var testEnd = roundInfo.endDate;
            //Check start date is before end date
            var tStartDay = Convert.ToInt16(testStart.Substring(0, 2));
            var tStartMonth = Convert.ToInt16(testStart.Substring(3,2));
            var tStartYear = Convert.ToInt16(testStart.Substring(6));

            var tEndDay = Convert.ToInt16(testEnd.Substring(0, 2));
            var tEndMonth = Convert.ToInt16(testEnd.Substring(3, 2));
            var tEndYear = Convert.ToInt16(testEnd.Substring(6));

            if (ModelState.IsValid && tStartYear < tEndYear || (tStartYear == tEndYear && tStartMonth < tEndMonth) || (tStartYear == tEndYear && tStartMonth == tEndMonth && tStartDay < tEndDay))
            {

                db.RoundInfoes.Add(roundInfo);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            else {
                ViewBag.error = "End date before start date";
                return View(roundInfo);
            }
        }

        // GET: Rounds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoundInfo roundInfo = db.RoundInfoes.Find(id);
            if (roundInfo == null)
            {
                return HttpNotFound();
            }
            return View(roundInfo);
        }

        // POST: Rounds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "round,startDate,endDate")] RoundInfo roundInfo)
        {
            ViewBag.error = "";

            var testStart = roundInfo.startDate;
            var testEnd = roundInfo.endDate;
            //Check start date is before end date
            var tStartDay = Convert.ToInt16(testStart.Substring(0, 2));
            var tStartMonth = Convert.ToInt16(testStart.Substring(3,2));
            var tStartYear = Convert.ToInt16(testStart.Substring(6));

            var tEndDay = Convert.ToInt16(testEnd.Substring(0, 2));
            var tEndMonth = Convert.ToInt16(testEnd.Substring(3, 2));
            var tEndYear = Convert.ToInt16(testEnd.Substring(6));

            if (ModelState.IsValid && tStartYear < tEndYear || (tStartYear == tEndYear && tStartMonth < tEndMonth) || (tStartYear == tEndYear && tStartMonth == tEndMonth && tStartDay < tEndDay))
            {

                db.RoundInfoes.Add(roundInfo);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.error = "End date before start date";
            return View(roundInfo);
        }

        // GET: Rounds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoundInfo roundInfo = db.RoundInfoes.Find(id);
            if (roundInfo == null)
            {
                return HttpNotFound();
            }
            return View(roundInfo);
        }

        // POST: Rounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoundInfo roundInfo = db.RoundInfoes.Find(id);
            db.RoundInfoes.Remove(roundInfo);
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
