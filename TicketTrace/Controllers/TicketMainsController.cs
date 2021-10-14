using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketTrace.Models;

namespace TicketTrace.Controllers
{
    public class TicketMainsController : Controller
    {
        private TicketEntities db = new TicketEntities();

        // GET: TicketMains
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                TempData["message"] = "<script>alert('請先登入');</script>";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View(db.TicketMain.ToList());
            }
        }

        // GET: TicketMains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketMain ticketMain = db.TicketMain.Find(id);
            if (ticketMain == null)
            {
                return HttpNotFound();
            }
            return View(ticketMain);
        }

        // GET: TicketMains/Create
        public ActionResult Create()
        {
            var form_categories = db.Form.ToList();

            if (Session["RoleName"].ToString() == "QA")
            {
                form_categories.Remove(form_categories.Find(p => p.FormName == "Feature Request"));
            }
            if (Session["RoleName"].ToString() == "PM")
            {
                form_categories.Remove(form_categories.Find(p => p.FormName == "Bug" && p.FormName == "Test Case"));
            }
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var category in form_categories)
            {
                items.Add(new SelectListItem()
                {
                    Text = category.FormName,
                    Value = category.FID.ToString()
                });
            }

            ViewBag.CategoryItems = items;

            return View();
        }

        // POST: TicketMains/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TID,FID,UID,Summary,Description,Severity,Priority,CreateTime")] TicketMain ticketMain)
        {
            if (ModelState.IsValid)
            {
                db.TicketMain.Add(ticketMain);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticketMain);
        }

        // GET: TicketMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketMain ticketMain = db.TicketMain.Find(id);
            if (ticketMain == null)
            {
                return HttpNotFound();
            }
            return View(ticketMain);
        }

        // POST: TicketMains/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TID,FID,UID,Summary,Description,Severity,Priority,CreateTime")] TicketMain ticketMain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketMain).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticketMain);
        }

        // GET: TicketMains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketMain ticketMain = db.TicketMain.Find(id);
            if (ticketMain == null)
            {
                return HttpNotFound();
            }
            return View(ticketMain);
        }

        // POST: TicketMains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketMain ticketMain = db.TicketMain.Find(id);
            db.TicketMain.Remove(ticketMain);
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
