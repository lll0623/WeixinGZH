using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Frontpages;
using LesoftWuye2.Application.Frontpages.Session;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.Estateinfos;
using Obs;
using Obs.Extensions;
using Obs.Users;

using PermissionNames = Obs.Authorization.PermissionNames;
using Abp.Application.Services.Dto;
using LesoftWuye2.Application.Forums;
using LesoftWuye2.Application.Malls;
using LesoftWuye2.Application.Plates;

namespace LesoftWuye2.Web.Controllers
{
    //真旺云所有页面集中在这里处理
    public class ForumController : LesoftWuye2ControllerBase
    {

        private readonly IIocManager _iocManager;
        private readonly IForumService _forumService;
        private readonly IPlateAppService _plateAppService;


        public ForumController(IIocManager iocManager,
            IForumService forumService,
            IPlateAppService plateAppService)
        {
            _forumService = forumService;
            _plateAppService = plateAppService;
            _iocManager = iocManager;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //根据cooike信息,设置session数据
            var session = _iocManager.Resolve<FrontSession>();
            if (this.Request.Cookies?["app.mid"] != null)
            {
                session.AppMemberId = this.Request.Cookies["app.mid"].Value;
                session.Prepare();
            }
        }


        public async Task<ActionResult> Index()
        {
            var data = await _forumService.GetHomeData();
            return View(data);
        }

        public async Task<ActionResult> View(long id)
        {
            var data = await _forumService.Detail(id);
            return View(data);
        }

        public ActionResult Comment(long id)
        {
            ViewData["id"] = id;
            return View();
        }

        public async Task<ActionResult> New()
        {
            var data = await _plateAppService.GetPlateForCombo();
            ViewData["plateId"] = this.Request["plateId"]??"0";
            return View(data);
        }

        public  ActionResult List(long plateId)
        {
            ViewData["plateId"] = plateId;
            return View();
        }

        public async Task<ActionResult> Plates()
        {
            var data = await _forumService.GetPlates();
            return View(data);
        }

        public ActionResult MyPost()
        {
            return View();
        }


        public ActionResult MyComment()
        {
            return View();
        }
    }
}












