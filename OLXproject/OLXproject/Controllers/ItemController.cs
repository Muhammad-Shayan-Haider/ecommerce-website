using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using Repository;
using OLXproject.CustomFilters;
using System.IO;

namespace OLXproject.Controllers
{
    public class ItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Item
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var items = db.Items.Include(i => i.Category);
            return View(await items.ToListAsync());
        }

        // GET: Item/Details/5
        [CustomAuthorize(Roles = "Admin")]

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            Session["DetailedImage"] = item.imgPath.ToString();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [CustomAuthorize(Roles = "Admin")]
        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.cId = new SelectList(db.Categories, "categoryID", "name");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Create(Item item)
        {
            if (ModelState.IsValid == true)
            {

                //itm.cId = Convert.ToInt32(x);

                string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
                string extension = Path.GetExtension(item.ImageFile.FileName);

                HttpPostedFileBase postedFile = item.ImageFile;

                int length = postedFile.ContentLength; // length has image size in bytes

                if (extension.ToLower() == ".jpeg" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png")
                {
                    if (length <= 1000000) //1mb
                    {

                        fileName = fileName + extension;
                        item.imgPath = "~/images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                        item.ImageFile.SaveAs(fileName);

                        db.Items.Add(item);
                        await db.SaveChangesAsync();

                        ModelState.Clear();
                        return RedirectToAction("Index", "Item");
                    }
                }
            }
            return View();

        }

        // GET: Item/Edit/5
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.cId = new SelectList(db.Categories, "categoryID", "name", item.cId);

            Session["Image"] = item.imgPath;
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                if (item.ImageFile != null)
                {

                    string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
                    string extension = Path.GetExtension(item.ImageFile.FileName);

                    HttpPostedFileBase postedFile = item.ImageFile;

                    int length = postedFile.ContentLength; // length has image size in bytes

                    if (extension.ToLower() == ".jpeg" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png")
                    {
                        if (length <= 1000000) //1mb
                        {

                            fileName = fileName + extension;
                            item.imgPath = "~/images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            item.ImageFile.SaveAs(fileName);

                            db.Entry(item).State = EntityState.Modified;
                            await db.SaveChangesAsync();

                            String ImagePath = Request.MapPath(Session["Image"].ToString());
                            if (System.IO.File.Exists(ImagePath))
                            {
                                System.IO.File.Delete(ImagePath);
                            }

                            ModelState.Clear();
                            return RedirectToAction("Index", "Item");
                        }
                    }                      
                } else
                {
                    item.imgPath = Session["Image"].ToString();
                    db.Entry(item).State = EntityState.Modified;
                    int a = await db.SaveChangesAsync();
                }
            }
           
            return View();
        }

        // GET: Item/Delete/5
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            Session["DeleteImage"] = item.imgPath.ToString();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Item item = await db.Items.FindAsync(id);
            db.Items.Remove(item);
            await db.SaveChangesAsync();

            String ImagePath = Request.MapPath(item.imgPath.ToString());
            
            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }

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

        public ActionResult AddtoCart(int id)
        {
            Session["ItemId"] = id;
            return RedirectToAction("AddtoCart", "Order");
        }
    }
}
