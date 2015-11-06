namespace SimpleMathClient.Controllers
{
    using SimpleMathClient.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// HomeController class - extends Controller class 
    /// </summary>
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        /// <summary>
        /// Index method
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new HomeViewModel();

            // AddTwoNumbers creates side-effects (InputA, InputB, and Result are set) - need a better way to do this
            model.AddTwoNumbers(3, 3);

            return View(model);
        }

    }
}
