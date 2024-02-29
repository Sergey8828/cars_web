using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cars.Controllers
{
    public class ErrorOrNoController : Controller
    {
        // GET: ErrorOrNo
        public ActionResult Http400()
        {
            Response.StatusCode = 400;
            return View();
        }
        public ActionResult Http404()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}