using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Models;
using Repository;
using PagedList;
using PagedList.Mvc;
using OLXproject.CustomFilters;
using System.IO;
using System.Web.UI.WebControls;
using Models.ViewModels;
using Models.Models;

namespace OLXproject.Controllers
{
    public class BrowseItemController : Controller
    {
        // GET: BrowseItem
        private BrowseItemRepository itemRepository = new BrowseItemRepository();

        public ActionResult HomeIndex(string Category, string search, int? page)
        {
            ViewBag.Categories = itemRepository.getCategories();
            if (search != "" && search != null)
            {
                ViewBag.Message = "Showing results for: " + search;
            }
            if (Category != "" && Category != null && search != null)
            {
                List<Item> result = new List<Item>();
                result = itemRepository.getItemsByCategory(Category, search).ToPagedList(page ?? 1, 9).ToList();
                if (result.ToList().Count == 0)
                {
                    ViewBag.Message += " No results found!!";
                }

                return View(result.ToPagedList(page ?? 1, 9));
            }

            if ((Category == null || Category == "") && search != null)
            {
                var result = itemRepository.getItems(search).ToPagedList(page ?? 1, 9);
                if (result.ToList().Count == 0)
                {
                    ViewBag.Message += " No results found!!";
                }

                return View(itemRepository.getAllItems().ToPagedList(page ?? 1, 9));
            }
            return View(itemRepository.getAllItems().ToPagedList(page ?? 1, 9));
        }

        /*public ActionResult HomeIndex()
        {
            var items = db.Items.Include(i => i.Category);
            return View(items);
        }*/

        public JsonResult ReturnItems()
        {
            var items = itemRepository.getAllItems();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Categories()
        {
            var categorizedItems = itemRepository.getCategoryStrings();

            return View(categorizedItems.ToList());
        }

        [CustomAuthorize(Roles = "User")]
        public ActionResult Details(int? id)
        {
            ItemUserReviewViewModel itemUserReviewView = new ItemUserReviewViewModel();
            itemUserReviewView.item = new Item();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = itemRepository.findItem(id.GetValueOrDefault());
            string cat = item.Category.name;
            itemUserReviewView.item = item;

            var trends = itemRepository.getTrendingItems(cat);

            itemUserReviewView.trendings = new List<Trending>();
            itemUserReviewView.trendings = trends.ToList();

            var reviews = itemRepository.getReviews(id.GetValueOrDefault());

            var ratings = itemRepository.getRatings(id.GetValueOrDefault());

            itemUserReviewView.UserComments = new List<UserComment>();
            itemUserReviewView.ratings = new List<float>();

            foreach (var rating in ratings)
            {
                itemUserReviewView.ratings.Add(rating.ratingValue);
            }
            foreach (var review in reviews)
            {
                UserComment userComment = new UserComment();
                userComment.comment = review.text;
                userComment.user = review.ApplicationUser.UserName;
                itemUserReviewView.UserComments.Add(userComment);
            }
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(itemUserReviewView);

        }

        public JsonResult getComments(int itemId)
        {
            return Json(itemRepository.getReviews(itemId), JsonRequestBehavior.AllowGet);
        }

        public int addComment(string comment, int itemId)//Post a comment
        {
            try
            {
                Review review = new Review();
                review.uId = User.Identity.GetUserId();
                review.text = comment;
                review.reviewDate = DateTime.Now;
                review.itemId = itemId;
                itemRepository.postComment(review);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            //return RedirectToAction("Details/"+review.itemId);
        }

        public int addRating(int id, float value)
        {
            try
            {
                Rating rating = new Rating();
                rating.userId = User.Identity.GetUserId();
                rating.ratingValue = value;
                rating.ItemId = id;
                itemRepository.postRating(rating);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public JsonResult getAverageRating(int itemId)
        {
            var rating = itemRepository.getItemRating(itemId);
            List<float> ratings = new List<float>();
            foreach (var item in rating)
            {
                ratings.Add(item.ratingValue);
            }
            float avg = 0;
            if (ratings.Count() != 0)
            {
                avg = ratings.Sum() / ratings.Count();
            }
            return Json(avg, JsonRequestBehavior.AllowGet);

        }

        public ActionResult viewItemsByCategory(string id, int? page)
        {
            var categorizedItems = itemRepository.getItemsByCategoryName(id);
            List<Item> items = new List<Item>();
            items = categorizedItems.ToList<Item>();
            return View(items.ToPagedList(page ?? 1, 9));
        }
        public ActionResult wishList(int? page)
        {
            string uId = User.Identity.GetUserId();
            var wishListItems = itemRepository.getWishListItems(uId);
            List<Item> items = new List<Item>();
            items = wishListItems.ToList<Item>();
            return View(items.ToPagedList(page ?? 1, 9));
        }
        public int addtoWishList(int id)
        {
            try
            {
                WishList wishList = new WishList();
                wishList.ItemId = id;
                wishList.userId = User.Identity.GetUserId();
                itemRepository.postWishListItem(wishList);
                return 1;
            }
            catch (Exception)
            {

                return 0;
            }
        }

    }


}