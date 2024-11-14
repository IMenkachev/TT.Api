using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using TT.Mvc.Models;

namespace TT.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Data()
        {
            ViewBag.Message = "Your contact page.";

            string file = HostingEnvironment.MapPath("~/app_data/tt.mdf");

            string cnn = @"Data Source=.\SQLEXPRESS;
                          AttachDbFilename=" + file + @";
                          Integrated Security=True;
                          Connect Timeout=30;
                          User Instance=True";

            cnn = "Data Source=(localdb)\\v11.0;Initial Catalog=tt;Integrated Security=True";
            cnn = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\tt.mdf;Integrated Security=True;Connect Timeout=30";
            List<IdName> data = new List<IdName>();

            using (SqlConnection myConnection = new SqlConnection(cnn))
            {
                string oString = "SELECT * FROM Products";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);

                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        data.Add(new IdName()
                        {
                            Id = int.Parse(oReader["Id"].ToString()),
                            Name = oReader["Name"].ToString()
                        });
                    }

                    myConnection.Close();
                }
            }

            ViewBag.Data = data;

            return View(data);
        }
    }
}