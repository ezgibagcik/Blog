using DataAccessLayer.Concreate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            Context c = new Context();
            var userMail = User.Identity.Name;
            var writerId = c.Writers.Where(x => x.WriterMail == userMail).Select(y => y.WriterId).FirstOrDefault();
            ViewBag.v1 = c.Blogs.Count().ToString();
            ViewBag.v2 = c.Blogs.Where(x => x.WriterId == writerId).Count();
            ViewBag.v3 = c.Categorys.Count().ToString();

            return View();
        }

    }
}
