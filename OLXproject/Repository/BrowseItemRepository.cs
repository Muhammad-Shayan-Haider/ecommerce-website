using Models;
using Models.Models;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Repository
{
    public class BrowseItemRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationDbContext getContext()
        {
            return db;
        }

        public List<string> getCategories()
        {
            return db.Database.SqlQuery<string>("select Categories.name from Categories group by Categories.name").ToList();
        }

        public List<Item> getItemsByCategory(string search, string Category)
        {
            return db.Items.Where(x => (x.name.StartsWith(search) || x.name.Contains(search) || search == null) && x.Category.name == Category).OrderBy(x => x.cId).ToList<Item>();
        }

        public List<Item> getItems(string search)
        {
            return db.
                Items.
                Where(x => (x.name.StartsWith(search) || x.name.Contains(search) || search == null)).OrderBy(x => x.cId).ToList<Item>();
        }

        public List<Item> getAllItems()
        {
            return db.
                Items.
                OrderBy(x => x.cId).ToList<Item>();
        }

        public List<string> getCategoryStrings()
        {
            return db.
                Database.
                SqlQuery<string>("select Categories.name from Categories join Items on Items.cId = Categories.categoryID group by Categories.name").ToList<string>();
        }

        public Item findItem(int id)
        {
            return db.Items.Find(id);
        }

        public List<Trending> getTrendingItems(string cat)
        {
            return db.Database.SqlQuery<Trending>("select top 1 Items.name, Items.itemID, Items.imgPath, Items.price , avg(Ratings.ratingValue) as AvgRating from Ratings join Items on Items.itemID = Ratings.ItemId join Categories on Categories.categoryID = Items.cId where Categories.name = '" + cat + "' group by Items.itemID, Categories.name, Items.name, Items.imgPath, Items.price having avg(Ratings.ratingValue) >= 4 order by AvgRating desc").ToList<Trending>();
        }

        public List<Review> getReviews(int id)
        {
            var obj = from review in db.Review
                      join items in db.Items on review.itemId equals items.itemID
                      where review.itemId == id
                      select review;
            return obj.ToList<Review>();
        }


        public List<Rating> getRatings(int id)
        {
            var obj = from rating in db.Rating
                      join items in db.Items on rating.ItemId equals items.itemID
                      where rating.ItemId == id
                      select rating;
            return obj.ToList<Rating>();
        }

        public void postComment(Review review)
        {
            db.Review.Add(review);
            db.SaveChanges();
        }

        public void postRating(Rating rating)
        {
            db.Rating.Add(rating);
            db.SaveChanges();
        }

        public List<Rating> getItemRating(int itemId)
        {
            var rating = from obj in db.Rating
                         where obj.ItemId == itemId
                         select obj;
            return rating.ToList<Rating>();
        }

        public List<Item> getItemsByCategoryName(string id)
        {
            var items = from obj in db.Items
                        where obj.Category.name == id
                        select obj;
            return items.ToList<Item>();
        }

        public List<Item> getWishListItems(string uId)
        {
            var wishlist = from obj in db.WishList
                           where obj.userId == uId
                           select obj.Item;
            return wishlist.ToList<Item>();
        }

        public void postWishListItem(WishList wishList)
        {
            db.WishList.Add(wishList);
            db.SaveChanges();
        }

        public void sort()
        {
            int[] A = new int[10];
            for (int i = 0; i < 10; i++)
            {
                A[i] = i;
            }
            Math.Cosh(8.32);
        }
    }
}
