using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace LesoftWuye2.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : LesoftWuye2ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}












