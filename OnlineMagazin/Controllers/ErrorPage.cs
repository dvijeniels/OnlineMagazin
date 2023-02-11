using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMagazin.Controllers
{
    public class ErrorPage : Controller
    {
        // GET: ErrorPage
        [AllowAnonymous]
        public ActionResult ErrorName(int code)
        {
            return View();
        }
    }
}
