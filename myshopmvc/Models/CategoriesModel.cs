using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class CategoriesModel
    {
        [Key]
        public int CatID { get; set; }

        public string Category { get; set; }

        public int TotalCount { get; set; }
    }
}