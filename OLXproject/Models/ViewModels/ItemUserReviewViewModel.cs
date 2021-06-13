using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;
namespace Models.ViewModels
{
    public class ItemUserReviewViewModel
    {
        public List<UserComment> UserComments { get; set; }
        public List<float> ratings { get; set; }
        public Item item { get; set; }
        public List<Trending> trendings { get; set; }

    }
}
