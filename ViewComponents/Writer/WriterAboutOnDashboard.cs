using BusinessLayer.Concreate;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.ViewComponents.Writer
{
    public class WriterAboutOnDashboard:ViewComponent
    {
        WriterManager writermanager = new WriterManager(new EfWriterRepository());
        public IViewComponentResult Invoke()
        {
            var values = writermanager.GetWritertById(1);
            return View(values);
        }

    }
}
