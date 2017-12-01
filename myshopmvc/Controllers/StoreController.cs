using MyShopMVC.App_Code;
using MyShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopMVC.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            StoreViewModel store = new StoreViewModel();
            store.AllProducts = GetProducts();
            store.AllCategories = GetCategories();
            return View(store);
        }

        public List<ProductsModel> GetProducts()
        {
            List<ProductsModel> list = new List<ProductsModel>();

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT p.ProductID, p.Image, p.Name, c.Category, p.Price
                                FROM products p INNER JOIN categories c ON p.CatID = c.CatID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new ProductsModel
                            {
                                ID = Convert.ToInt32(data["ProductID"].ToString()),
                                Image = data["Image"].ToString(),
                                Name = data["Name"].ToString(),
                                Category = data["Category"].ToString(),
                                Price = double.Parse(data["Price"].ToString())


                            });
                        }
                    }
                }

            }
            return list;
        }

        public List<CategoriesModel> GetCategories()
        {
            List<CategoriesModel> list = new List<CategoriesModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT c.CatID, c.Category,(SELECT COUNT(p.ProductID) FROM Products p WHERE p.CatID = c.CatID) AS TotalCount
                                    FROM Categories c ORDER BY c.Category";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new CategoriesModel
                            {
                                CatID = int.Parse(data["CatID"].ToString()),
                                Category = data["Category"].ToString(),
                                TotalCount = int.Parse(data["TotalCount"].ToString())
                            });
                        }
                    }

                }
            }
            return list;
        }

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT p.Name, p.Image, p.Description, p.Price, c.Category FROM Products p 
                                   INNER JOIN Categories c ON p.CatID = c.CatID WHERE p.ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if(data.HasRows)
                        {
                            ProductsModel product = new ProductsModel();
                            while(data.Read())
                            {
                                product.Name = data["Name"].ToString();
                                product.Image = data["Image"].ToString();
                                product.Description = data ["Description"].ToString();
                                product.Price = double.Parse(data["Price"].ToString());
                                product.Category = data["Category"].ToString();

                            }
                            return View(product);
                        }

                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Details(ProductsModel product, int quantity)
        {
            CartController.AddToCart(product.ID, quantity);
            return View();
        }
  
    }
}