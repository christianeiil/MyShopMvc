using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class OrderDetailsModel
    {
        [Key]

        public int RefNo { get; set; }

        public int OrderNo { get; set; }

        public int UserID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }
    }
}