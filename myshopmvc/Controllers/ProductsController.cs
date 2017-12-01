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
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            List<ProductsModel> list = new List< ProductsModel > ();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT p.ProductID, p.Name, c.Category, p.Image, p.Description, p.Price, p.Status, p.DateAdded, p.DateModified 
                                FROM Products p INNER JOIN Categories c ON p.CatID = c.CatID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new ProductsModel
                            {
                                ID = Convert.ToInt32(data["ProductID"].ToString()),
                                Name = data["Name"].ToString(),
                                Category = data["Category"].ToString(),
                                Image = data["Image"].ToString(),
                                Description = data["Description"].ToString(),
                                Price = double.Parse(data["Price"].ToString()),
                                Status = data["Status"].ToString(),
                                DateAdded = DateTime.Parse(data["DateAdded"].ToString())

                            });
                        }
                    }
                }
            }
                return View(list);
        }

        public ActionResult Add()
        {
            ProductsModel product = new ProductsModel();
            product.Categories = GetCategories();
            return View(product);
        }

        [HttpPost]
        public ActionResult Add(ProductsModel product, HttpPostedFileBase image)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO Products VALUES (@Name, @CatID, @Code, @Description, @Image, @Price, @Isfeatured, @Available, @Criticallevel, 
                                                               @Maximum, @Status, @DateAdded, @DateModified)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    string imageFile = DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName;
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@CatID", product.CatID);
                    cmd.Parameters.AddWithValue("@Code", product.Code);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    cmd.Parameters.AddWithValue("@Image", imageFile);
                    image.SaveAs(Server.MapPath("~/Images/Products/" + imageFile));
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Isfeatured", product.IsFeatured ? "Yes" : "No");
                    cmd.Parameters.AddWithValue("@Available", 0);
                    cmd.Parameters.AddWithValue("@Criticallevel", product.Critical);
                    cmd.Parameters.AddWithValue("@Maximum", product.Max);
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DBNull.Value);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            } 
        }

        public List<SelectListItem> GetCategories()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT CatID, Category FROM Categories ORDER BY Category";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                            items.Add(new SelectListItem
                            {
                                Text = data["category"].ToString(),
                                Value = data["CatID"].ToString()
                            });                  
                    }
                }
            }

            return items;
        }
    }
}