using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IItemRepository
    {
        Item getItemById(int? id);
        void addItem(Item item);
        void saveChanges();
        void removeItem(Item item);
        List<Item> getSearchedItemsById(String Value);
        List<Item> getSearchedItemsByName(String Value);
        List<Item> getSearchedItemsByCategory(String Value);
        void updateItem(Item item);

        bool checkIfItemExists(List<Item> itemList, int? id);
        ApplicationDbContext getDbContext();
    }
}
