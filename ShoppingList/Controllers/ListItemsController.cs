using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingList.Models;

namespace ShoppingList.Controllers
{
    public class ListItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ListItems
        public ActionResult Index()
        {
            return View();
        }

        private IEnumerable<ListItem> GetMyListItems()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault
                (x => x.Id == currentUserId);
            return db.ListItems.ToList().Where(x => x.User == currentUser);
        }

        public ActionResult BuildListItemTable()
        {
            return PartialView("_ListItemTable", GetMyListItems());
        }

        // GET: ListItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListItem listItem = db.ListItems.Find(id);
            if (listItem == null)
            {
                return HttpNotFound();
            }
            return View(listItem);
        }

        // GET: ListItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,IsDone")] ListItem listItem)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault
                    (x => x.Id == currentUserId);
                listItem.User = currentUser;
                db.ListItems.Add(listItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(listItem);
        }

        // MY NEW METHOD 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description")] ListItem listItem)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault
                    (x => x.Id == currentUserId);
                listItem.User = currentUser;
                listItem.IsDone = false;
                db.ListItems.Add(listItem);
                db.SaveChanges();
            }

            return PartialView("_ListItemTable", GetMyListItems());
        }


        // GET: ListItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListItem listItem = db.ListItems.Find(id);
            if (listItem == null)
            {
                return HttpNotFound();
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault
                (x => x.Id == currentUserId);

            if (listItem.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View(listItem);
        }

        // POST: ListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,IsDone")] ListItem listItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(listItem);
        }

        // POST: ListItems/Edit/5 CHECKBOXES
        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListItem listItem = db.ListItems.Find(id);
            if (listItem == null)
            {
                return HttpNotFound();
            }
            else
            {
                listItem.IsDone = value;
                db.Entry(listItem).State = EntityState.Modified;
                db.SaveChanges();
            }
            return PartialView("_ListItemTable", GetMyListItems());
        }


        // GET: ListItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListItem listItem = db.ListItems.Find(id);
            if (listItem == null)
            {
                return HttpNotFound();
            }
            return View(listItem);
        }

        // POST: ListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListItem listItem = db.ListItems.Find(id);
            db.ListItems.Remove(listItem);
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
