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
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            List<OrderDetailsModel> list = new List<OrderDetailsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT od.RefNo, Od.OrderNo, od.ProductID, p.Name, p.image, p.Price, od.Quantity, od.Amount FROM OrderDetails od
                                   INNER JOIN Products p ON od.ProductID = p.ProductID WHERE od.OrderNo=@OrderNo AND od.UserID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", 1);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new OrderDetailsModel
                            {
                                RefNo = int.Parse(data["RefNo"].ToString()),
                                OrderNo = int.Parse(data["OrderNo"].ToString()),
                                ProductID = int.Parse(data["ProductID"].ToString()),
                                ProductName = data["Name"].ToString(),
                                ProductImage = data["Image"].ToString(),
                                ProductPrice = decimal.Parse(data["Price"].ToString()),
                                Quantity = int.Parse(data["Quantity"].ToString()),
                                Amount = decimal.Parse(data["Amount"].ToString())

                            });
                        }
                          
                    }
                }

            }
            ViewBag.GrossAmount = GetTotalAmount() * .88;
            ViewBag.VAT = GetTotalAmount() * .12;
            ViewBag.TotalAmount = GetTotalAmount();
                return View(list);
        }

        double GetTotalAmount()
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT SUM(Amount) FROM OrderDetails WHERE OrderNo=@OrderNo AND UserID=@UserID HAVING COUNT(RefNo) > 0";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", 1);
                    decimal total = cmd.ExecuteScalar() == null ? 0 : (decimal)cmd.ExecuteScalar();
                    return Convert.ToDouble(total);
                }

            }
        }

        public ActionResult Add(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            AddToCart(Convert.ToInt32(id), 1);
            return RedirectToAction("Index", "Store");
        }

       public static double GetPrice(int ProductID)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Price FROM Products WHERE ProductID=@ProductID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", ProductID);
                    decimal price = (decimal)cmd.ExecuteScalar();
                    return Convert.ToDouble(price);
                }
            }
        }

        /// <summary>
        /// Check if the chosen product is existing inside cart
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool IsExisting(int productID)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT RefNo FROM OrderDetails WHERE OrderNo=@OrderNo AND UserID=@UserID AND ProductID=@ProductID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", 1);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    return cmd.ExecuteScalar() == null? false : true; 
                  
                }
            }
        }

        public static void AddToCart(int productID, int quantity)
        {
            bool productExisting = IsExisting(productID);
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = "";
                if(productExisting)
                {
                    query = @"UPDATE OrderDetails SET Quantity = Quantity + @Quantity, Amount = Amount + @Amount 
                    WHERE OrderNo=@OrderNo AND UserID=@UserID AND ProductID=@ProductID";
                }
                else
                {
                    query = @"INSERT INTO OrderDetails VALUES (@OrderNo, @UserID, @ProductID, @Price, @Quantity, @Amount, @Status)";
                }
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", 1);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@Price", GetPrice(productID));
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Amount", GetPrice(productID) * quantity);
                    cmd.Parameters.AddWithValue("@Status", "In Cart");
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}