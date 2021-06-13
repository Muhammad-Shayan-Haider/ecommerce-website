using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class Trending
    {
        public string name { get; set; }
        public int itemId { get; set; }
        public string imgPath { get; set; }
        public double price { get; set; }
        public float average { get; set; }

    }
}
