using MyShopMVC.App_Code;
using MyShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopMVC.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            List<UsersModel> list = new List<UsersModel>();

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT u.UserID, t.UserType, u.Email, u.LastName, u.FirstName, u.Street, u.Municipality,
                                u.City, u.Status, u.DateAdded, u.DateModified FROM  Users u INNER JOIN Types t ON u.TypeID = t.TypeID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader da = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(da);
                        foreach (DataRow dr in dt.Rows)

                        {
                            var user = new UsersModel();
                            user.ID = Convert.ToInt32(dr["UserID"].ToString());
                            user.UserType = dr["UserType"].ToString();
                            user.Email = dr["Email"].ToString();
                            user.LastName = dr["LastName"].ToString();
                            user.FirstName = dr["FirstName"].ToString();
                            user.Street = dr["Street"].ToString();
                            user.Municipality = dr["Municipality"].ToString();
                            user.City = dr["City"].ToString();
                            user.DateAdded = DateTime.Parse(dr["DateAdded"].ToString());
                            list.Add(user);
                        }
                    }
                    return View(list);
                }

            }
        }

        public List<SelectListItem> GetUserTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT TypeID, UserType FROM Types
                        ORDER BY UserType";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {

                        while (data.Read())
                        {
                            items.Add(new SelectListItem { Text = data["UserType"].ToString(), Value = data["TypeID"].ToString() });
                        }
                    }
                }
            }

            return items;
        }

        public ActionResult Add()
        {
            UsersModel user = new UsersModel();
            user.UserTypes = GetUserTypes();
            return View(user);
        }

        [HttpPost]
        public ActionResult Add(UsersModel user)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO users VALUES (@TypeID, @Email, @Password, @FirstName, @LastName, @Street, @Municipality, 
                                @City, @Phone, @Mobile, @Status, @DateAdded, @DateModified)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TypeID", user.TypeID);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", Helper.CreateSHAHash(user.Password));
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Street", user.Street);
                    cmd.Parameters.AddWithValue("@Municipality", user.Municipality);
                    cmd.Parameters.AddWithValue("@City", user.City);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone);
                    cmd.Parameters.AddWithValue("@Mobile", user.Mobile);
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DBNull.Value);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                        
                }

            }
                return View();
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null) //record is not selcted
                return RedirectToAction("Index");

            UsersModel user = new UsersModel();
            user.UserTypes = GetUserTypes(); //display list of user types

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT userID, TypeID, Email, LastName, FirstName, Street, Municipality,
                       City, Phone, Mobile FROM Users WHERE UserID=@UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", id);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                user.TypeID = Convert.ToInt32(data["TypeID"].ToString());
                                user.Email = data["Email"].ToString();
                                user.LastName = data["Lastname"].ToString();
                                user.FirstName = data["FirstName"].ToString();
                                user.Street = data["Street"].ToString();
                                user.Municipality = data["Municipality"].ToString();
                                user.City = data["City"].ToString();
                                user.Phone = data["Phone"].ToString();
                                user.Mobile = data["Mobile"].ToString();
                            }

                            return View(user);
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
        public ActionResult Details(UsersModel user)
        {
            return RedirectToAction("Index");
        }


    }
}