//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using Abp.Dependency;
//using Abp.Web.Mvc.Authorization;
//using LesoftWuye2.Application.Frontpages;
//using LesoftWuye2.Application.Frontpages.Session;
//using LesoftWuye2.Application.WuyeApis;
//using LesoftWuye2.Core.Estateinfos;
//using Obs;
//using Obs.Extensions;
//using Obs.Users;

//using PermissionNames = Obs.Authorization.PermissionNames;
//using Abp.Application.Services.Dto;
//using LesoftWuye2.Application.Malls;
//using LesoftWuye2.Core.Configuration;

//namespace LesoftWuye2.Web.Controllers
//{
//    //真旺云所有页面集中在这里处理
//    public class MallController : LesoftWuye2ControllerBase
//    {
//        private readonly IIocManager _iocManager;
//        private readonly IFrontpageAppSrvice _frontpageAppSrvice;
//        private readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;

//        private readonly IMallService _mallAppSrvice;


//        public MallController(IIocManager iocManager,
//            IMallService mallAppSrvice)
//        {
//            _mallAppSrvice = mallAppSrvice;
//            _iocManager = iocManager;
//        }

//        protected override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            base.OnActionExecuting(filterContext);
//            //根据cooike信息,设置session数据
//            var session = _iocManager.Resolve<FrontSession>();
//            if (this.Request.Cookies?["app.mid"] != null)
//            {
//                session.AppMemberId = this.Request.Cookies["app.mid"].Value;
//                session.Prepare();
//            }
//        }

//        #region 拼团

//        public async Task<ActionResult> GrouponList(int cid = 0)
//        {
//            ViewData["categories"] = await _mallAppSrvice.GetCategories();
//            ViewData["cid"] = cid;
//            return View("Groupon/List");

//        }

//        public async Task<ActionResult> GrouponOrderList(int type = 0)
//        {
//            ViewData["type"] = type;
//            return View("Groupon/OrderList");

//        }

//        public ActionResult GrouponComments(long id)
//        {
//            ViewData["id"] = id;
//            return View("Groupon/Comments");
//        }
        
//        public async Task<ActionResult> GrouponView(int id)
//        {
//            var data = await _mallAppSrvice.GetGrouponDetail(id);
//            string GrouponDescription= await SettingManager.GetSettingValueAsync(AppSettings.Mall.GrouponDescription);
//            GrouponDescription = GrouponDescription.Replace("\n", "<br/>");
//            ViewData["GrouponDescription"] = GrouponDescription;
            
//            return View("Groupon/View", data);

//        }

//        public async Task<ActionResult> GrouponOrder(int id)
//        {
//            var data = await _mallAppSrvice.GetGrouponOrder(id);

//            return View("Groupon/Order", data);

//        }

//        public async Task<ActionResult> GrouponSubmit(int id, int type, int num, long joinGrouponId = 0)
//        {
//            var data = await _mallAppSrvice.GetGrouponDetail(id);
//            if (await _mallAppSrvice.CheckMemberIsOwner(this.Request["mid"] ?? "0") && data != null)
//                data.SalePrice = data.MemberPrice;

//            ViewData["type"] = type;
//            ViewData["num"] = num;
//            ViewData["joinGrouponId"] = joinGrouponId;
//            return View("Groupon/Submit", data);

//        }


//        public async Task<ActionResult> OrderView(int id)
//        {
//            var data = await _mallAppSrvice.GetOrderDetail(id);


//            return View("Order/View", data);

//        }

//        public ActionResult OrderComment(int id)
//        {
//            ViewData["id"] = id;
//            return View("Order/Comment");
//        }


//        #endregion


//        public ActionResult AddressList()
//        {
//            return View("Address/List");
//        }

//        public ActionResult AddressSelect()
//        {
//            return View("Address/Select");
//        }

//        public ActionResult AddressAdd()
//        {
//            return View("Address/Add");
//        }

//        public async Task<ActionResult> AddressEdit(long id)
//        {
//            var data = await _mallAppSrvice.GetAddress(id);

//            ViewData["provinces"] = await _mallAppSrvice.GetProvinces();
//            ViewData["cities"] = await _mallAppSrvice.GetCities(data.ProvinceId);
//            ViewData["district"] = await _mallAppSrvice.GetDistricts(data.CityId);

//            return View("Address/Edit", data);
//        }

//        public async Task<ActionResult> Index()
//        {
//            var data = await _mallAppSrvice.GetHomeData();

//            //广告配置
//            var AdImage = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdImage);
//            var AdUrl = await SettingManager.GetSettingValueAsync(AppSettings.Mall.AdUrl);

//            ViewData["AdImage"] = AdImage;
//            ViewData["AdUrl"] = AdUrl;


//            return View(data);

//        }

//        public ActionResult OrderList(int type = 0)
//        {
//            ViewData["type"] = type;
//            return View("Order/List");

//        }

//        public async Task<ActionResult> Waiting()
//        {

//            return View();
//        }
//    }
//}












