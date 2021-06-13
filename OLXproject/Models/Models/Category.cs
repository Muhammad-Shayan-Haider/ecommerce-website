using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
     public class Category
    {
        [Key]
        public int categoryID { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string name { get; set; }
    }
}
