using System;
using Microsoft.AspNetCore.Mvc;

namespace CodewareDb.Controllers
{
    public partial class HomeController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
