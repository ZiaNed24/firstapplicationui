using Microsoft.AspNetCore.Mvc;

namespace FirstApplicationUI.Controllers
{
    public class AdminController : BaseController
    {
        public IActionResult Index()
        {

            var redirect = CheckAccess(1);
            if (redirect != null) return redirect;

            return View();
        }
    }
}
