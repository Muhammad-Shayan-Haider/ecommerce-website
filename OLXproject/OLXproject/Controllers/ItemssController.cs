using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using OLXproject.CustomFilters;
using Repository;

namespace OLXproject.Controllers
{
    public class ItemssController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IItemRepository _item;

        public ItemssController()
        {
            _item = new ItemRepository();
        }
        public ItemssController(IItemRepository itemrepo)
        {
            this._item = itemrepo;
        }

        // GET: Itemss
        public ActionResult CustomerView()
        {
            var items = _item.getDbContext().Items.Include(i => i.Category);
            return View(items.ToList());
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //var items = db.Items.Include(i => i.Category);
            var items = _item.getDbContext().Items.Include(i => i.Category);
            return View(items.ToList());
        }

        public ActionResult GetSearchingData1(string SearchBy, string SearchValue)
        {
            //_item = new ItemRepository();
            List<Item> itemList = new List<Item>();
            if (SearchBy == "ID")
            {
                try
                {
                    itemList = _item.getSearchedItemsById(SearchValue);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} Is Not A ID ", SearchValue);
                }
                if (User.IsInRole("Admin"))
                {
                    return PartialView("SearchData", itemList);
                }
                else
                {
                    return PartialView("customerCompareProducts", itemList);
                }
            }
            else if (SearchBy == "Name")
            {
                try
                {
                    itemList = _item.getSearchedItemsByName(SearchValue);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} Is Not A ID ", SearchValue);
                }
                if (User.IsInRole("Admin"))
                {
                    return PartialView("SearchData", itemList);
                }
                else
                {
                    return PartialView("customerCompareProducts", itemList);
                }

            }
            else if (SearchBy == "Category")
            {
                try
                {
                    itemList = _item.getSearchedItemsByCategory(SearchValue);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} Is Not A ID ", SearchValue);
                }
                if (User.IsInRole("Admin"))
                {
                    return PartialView("SearchData", itemList);
                }
                else
                {
                    return PartialView("customerCompareProducts", itemList);
                }
            }
            else
            {
                itemList = _item.getSearchedItemsByName(SearchValue);
                if (User.IsInRole("Admin"))
                {
                    return PartialView("SearchData", itemList);
                }
                else
                {
                    return PartialView("customerCompareProducts", itemList);
                }
            }
        }

        // GET: Itemss/Details/5
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = _item.getItemById(id);

            Session["DetailedImage"] = item.imgPath.ToString();

            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Itemss/Create
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.cId = new SelectList(_item.getDbContext().Categories, "categoryID", "name");
            //ViewBag.cId = new SelectList(db.Categories, "categoryID", "name");
            return View();
        }

        // POST: Itemss/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid == true)
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

                        _item.addItem(item);

                        ModelState.Clear();
                        return RedirectToAction("Index", "Itemss");
                    }
                }
            }
            return View();
        }

        // GET: Itemss/Edit/5
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _item.getItemById(id);
            Session["Image"] = item.imgPath;
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.cId = new SelectList(_item.getDbContext().Categories, "categoryID", "name");
            //ViewBag.cId = new SelectList(db.Categories, "categoryID", "name", item.cId);
            return View(item);
        }

        // POST: Itemss/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
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

                            _item.updateItem(item);

                            String ImagePath = Request.MapPath(Session["Image"].ToString());
                            if (System.IO.File.Exists(ImagePath))
                            {
                                System.IO.File.Delete(ImagePath);
                            }

                            ModelState.Clear();
                            return RedirectToAction("Index", "Itemss");
                        }
                    }
                }
                else
                {
                    item.imgPath = Session["Image"].ToString();
                    _item.updateItem(item);
                    ModelState.Clear();
                    return RedirectToAction("Index", "Itemss");
                }
            }
            //ViewBag.cId = new SelectList(db.Categories, "categoryID", "name", item.cId);
            ViewBag.cId = new SelectList(_item.getDbContext().Categories, "categoryID", "name");
            return View(item);
        }

        // GET: Itemss/Delete/5
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _item.getItemById(id);
            Session["DeleteImage"] = item.imgPath.ToString();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Itemss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = _item.getItemById(id);
            if (item != null)
            {
                _item.removeItem(item);
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ViewCompareProducts()
        {
            var items = Session["CompareP"];
            return View(items);
        }

        public ActionResult ComparingProducts(int? id)
        {
            if (Session["compareP"] == null)
            {
                List<Item> compareP = new List<Item>();
                var product = _item.getItemById(id);
                if (_item.checkIfItemExists(compareP, id) == false)
                {
                    compareP.Add(product);
                }
                Session["compareP"] = compareP;
            }
            else
            {
                List<Item> compareP = (List<Item>)Session["compareP"];
                var product = _item.getItemById(id);
                if (_item.checkIfItemExists(compareP, id) == false)
                {
                    compareP.Add(product);
                }
                Session["compareP"] = compareP;
            }
            /*  CompareProducts cp = new CompareProducts();
              cp.items = new List<Item>();
              if (id != null)
              {
                  var item = db.Items.Find(id);
                  cp.items.Add(item);
              }
              Session["CompareObjects"] = cp.items;
              */
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("HomeIndex", "BrowseItem");
            }
        }

        public ActionResult RemoveCompareProduct(int? id)
        {
            var items = Session["CompareP"] as List<Item>;

            if (items != null)
            {
                items.RemoveAll(n => n.itemID == id);
                Session["CompareP"] = items;
            }

            return RedirectToAction("ViewCompareProducts");
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
