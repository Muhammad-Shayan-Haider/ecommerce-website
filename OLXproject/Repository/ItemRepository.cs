using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ItemRepository : IItemRepository
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public ItemRepository(ApplicationDbContext ctx)
        {
            this.ctx = ctx;
        }

        public ItemRepository()
        {
        }

        // Finds the product by its id
        //    It takes item id as parameter 
        //    and returns the item if found.

        public Item getItemById(int? id)
        {
            Item item = ctx.Items.Find(id);
            return item;
        }

        public void addItem(Item item)
        {
            ctx.Items.Add(item);
            ctx.SaveChanges();
        }
        public void removeItem(Item item)
        {
            ctx.Items.Remove(item);
            ctx.SaveChanges();
        }

        public List<Item> getSearchedItemsById(String SearchValue)
        {
            int Id = Convert.ToInt32(SearchValue);
            List<Item> itemList = new List<Item>();
            itemList = ctx.Items.Where(x => x.itemID == Id || SearchValue == null).ToList();
            return itemList;
        }

        public List<Item> getSearchedItemsByName(String SearchValue)
        {
            String Name = SearchValue;
            List<Item> itemList = new List<Item>();
            itemList = ctx
                .Items
                .Where(x => x.name.Contains(Name) || SearchValue == null).ToList();
            return itemList;
        }
        public List<Item> getSearchedItemsByCategory(String SearchValue)
        {
            String Name = SearchValue;
            List<Item> itemList = new List<Item>();
            itemList = ctx.Items.Where(x => x.Category.name.Contains(Name) || SearchValue == null).ToList();
            return itemList;
        }

        public void updateItem(Item item)
        {
            ctx.Entry(item).State = EntityState.Modified;
            ctx.SaveChanges();

        }

        public bool checkIfItemExists(List<Item> itemList, int? id)
        {
            Item item = new Item();
            item = itemList.Find(x => x.itemID == id);
            if (item != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ApplicationDbContext getDbContext()
        {
            return ctx;
        }

        public void saveChanges()
        {
            ctx.SaveChanges();
        }
    }
}
