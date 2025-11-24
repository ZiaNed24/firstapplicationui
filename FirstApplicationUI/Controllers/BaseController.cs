using Microsoft.AspNetCore.Mvc;

namespace FirstApplicationUI.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsAuthorized(int requiredRoleId)
        {
            var roleStr = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(roleStr))
                return false;

            int roleId = int.Parse(roleStr);
            return roleId == requiredRoleId;
        }

        protected IActionResult CheckAccess(int requiredRoleId)
        {
            if (!IsAuthorized(requiredRoleId))
                return RedirectToAction("Login", "Account");
            return null;
        }
    }
}
