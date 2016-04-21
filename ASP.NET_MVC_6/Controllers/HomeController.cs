using ASP.NET_MVC_6.Models;
using ASP.NET_MVC_6.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_MVC_6.Controllers
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

        public ActionResult UploadFile()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult UploadFile(HttpPostedFileBase file)
        //{
        //    var fileName = file.FileName;
        //    var filePath = Server.MapPath(string.Format("~/{0}", "File"));
        //    file.SaveAs(Path.Combine(filePath, fileName));
        //    return View();
        //}

        [HttpPost]
        public ActionResult UploadFile(BlogModel bModel)
        {
            var testSample = new TestSample() { UserName = "xpy0928", Id = Guid.NewGuid().ToString("N") };
            if (ModelState.IsValid)
            {
                var fileName = bModel.BlogPhoto.FileName;
                var success = UploadManager.SaveFile(bModel.BlogPhoto.InputStream, fileName, testSample.UserName, testSample.Id);
                if (!success) { 
                
                }
                //var filePath = Server.MapPath(string.Format("~/{0}", "File"));
                //bModel.BlogPhoto.SaveAs(Path.Combine(filePath, fileName));
                ModelState.Clear();
            }
            return View();
        }
    }
}