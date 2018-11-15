using System.Collections.Generic;
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

namespace LesoftWuye2.Web.Controllers
{
    //真旺云所有页面集中在这里处理
    public class MaxwonController : LesoftWuye2ControllerBase
    {
        private readonly IIocManager _iocManager;
        private readonly IFrontpageAppSrvice _frontpageAppSrvice;
        private readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;
        private readonly IFrontSession _frontSession;



        public MaxwonController(IIocManager iocManager, IFrontpageAppSrvice frontpageAppSrvice, IWuyeApiAppSrvice wuyeApiAppSrvice, IFrontSession frontSession)
        {
            _iocManager = iocManager;
            _frontpageAppSrvice = frontpageAppSrvice;
            _wuyeApiAppSrvice = wuyeApiAppSrvice;
            _frontSession = frontSession;
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

        #region 首页
        public async Task<ActionResult> Index()
        {
            return Redirect("/Maxwon/Home");
        }

        public async Task<ActionResult> Home()
        {
            var data = await _frontpageAppSrvice.GetHomeData();
            return View(data);
        }


        #endregion

        #region 公告
        public async Task<ActionResult> Notice_List()
        {
            var data = await _frontpageAppSrvice.GetNotices(BuildPageListRequstDto());
            return View("Notice/List", data);
        }

        public async Task<ActionResult> Notice_View(long id)
        {
            var data = await _frontpageAppSrvice.GetNotice(id);
            return View("Notice/View", data);
        }
        #endregion

        #region 活动
        public async Task<ActionResult> Activity_List()
        {
            var data = await _frontpageAppSrvice.GetActivitys(BuildPageListRequstDto());
            return View("Activity/List", data);
        }

        public async Task<ActionResult> Activity_View(long id)
        {
            var data = await _frontpageAppSrvice.GetActivity(id);
            ViewData["is_join"] = await _frontpageAppSrvice.CheckIsJoinActivity(id, _frontSession.MemberId.ToString());
            return View("Activity/View", data);
        }

        public async Task<ActionResult> Activity_My()
        {
            return View("Activity/My");
        }

        #endregion

        #region 新闻
        public async Task<ActionResult> News_List()
        {
            var data = await _frontpageAppSrvice.GetNewss(BuildPageListRequstDto());
            return View("News/List", data);
        }

        public async Task<ActionResult> News_View(long id)
        {
            var data = await _frontpageAppSrvice.GetNews(id);
            return View("News/View", data);
        }

        #endregion

        #region 服务电话
        public async Task<ActionResult> ServiceTel_List()
        {
            var data = await _frontpageAppSrvice.GetServiceTel();
            return View("ServiceTel/List", data);
        }
        #endregion

        #region 生活服务
        public async Task<ActionResult> LifeInfo_Nav()
        {
            var data = await _frontpageAppSrvice.GetLifeInfoNav();
            return View("LifeInfo/Nav", data);
        }

        public async Task<ActionResult> LifeInfo_List(long id)
        {
            var data = await _frontpageAppSrvice.GetLifeInfoList(BuildPageListRequstDto(), id);
            return View("LifeInfo/List", data);
        }

        public async Task<ActionResult> LifeInfo_View(long id)
        {
            var data = await _frontpageAppSrvice.GetLifeInfo(id);
            return View("LifeInfo/View", data);
        }
        #endregion

        #region 联系单添加
        public ActionResult Repair_Add()
        {
            return View("Repair/Add");
        }

        public ActionResult Complaint_Add()
        {
            return View("Complaint/Add");
        }

        public ActionResult Like_Add()
        {
            return View("Like/Add");
        }

        public ActionResult Feedback_Add()
        {
            return View("Feedback/Add");
        }

        #endregion

        #region 房间管理

        public ActionResult Room_Add_Old()
        {
            return View("Room/Add_Old");
        }


        public ActionResult Room_Add()
        {
            return View("Room/Add");
        }

        public async Task<ActionResult> Room_List()
        {
            var slideImages = await _frontpageAppSrvice.GetSlideImages();
            return View("Room/List", slideImages);
        }

        #endregion

        #region 关于我们
        public async Task<ActionResult> Substation_List()
        {
            var data = await _frontpageAppSrvice.GetSubstations(BuildPageListRequstDto());
            return View("Substation/List", data);
        }

        public async Task<ActionResult> Substation_View(long id)
        {
            var data = await _frontpageAppSrvice.GetSubstations(id);
            return View("Substation/View", data);
        }

        #endregion

        #region 跳蚤市场
        public async Task<ActionResult> Estateinfo_Add()
        {
            var data = await _frontpageAppSrvice.GetEstateinfoTypesForCombo();
            return View("Estateinfo/Add", data);
        }

        public async Task<ActionResult> Estateinfo_List()
        {
            int mode = (this.Request["mode"] ?? "").ParseTo<int>(0);

            List<ComboboxItemDto> nextItems = new List<ComboboxItemDto>();
            string thisMode = "全部";
            var types = await _frontpageAppSrvice.GetEstateinfoTypesForCombo();
            types.Add(new ComboboxItemDto("0", "全部"));
            foreach (var dto in types)
            {
                if (dto.Value != mode.ToString())
                {
                    nextItems.Add(dto);
                }
                else
                {
                    thisMode = dto.DisplayText;
                }
            }


            ViewData["thismode"] = thisMode;
            ViewData["nextItems"] = nextItems;
            var data = await _frontpageAppSrvice.GetEstateinfos(mode, BuildPageListRequstDto());
            return View("Estateinfo/List", data);
            //return null;
        }

        public async Task<ActionResult> Estateinfo_View(long id)
        {
            var data = await _frontpageAppSrvice.GetEstateinfo(id);
            return View("Estateinfo/View", data);
        }

        public async Task<ActionResult> Estateinfo_MyView(long id)
        {
            var data = await _frontpageAppSrvice.GetEstateinfo(id);
            return View("Estateinfo/MyView", data);
        }

        public ActionResult Estateinfo_My(int type)
        {
            string title = "未知";
            switch (type)
            {
                case 0:
                    title = "未审核的跳蚤商品信息";
                    break;
                case 1:
                    title = "上架的跳蚤商品信息";
                    break;
                case 2:
                    title = "下架的跳蚤商品信息";
                    break;
            }

            ViewData["title"] = title;
            ViewData["type"] = type;
            return View("Estateinfo/My");
        }

        #endregion

        #region 消息模块
        public ActionResult Message_List()
        {
            return View("Message/List");
        }

        public ActionResult Message_View(string id)
        {
            ViewData["id"] = id;
            return View("Message/View");
        }
        #endregion

        #region 欠费查询
        public ActionResult Fee_NoPay()
        {
            return View("Fee/NoPay");
        }

        public ActionResult Fee_Pay()
        {
            ViewData["payurl"] = Request["payurl"];
            return View("Fee/Pay");
        }

        #endregion

        #region 我的事务
        public ActionResult My_Count()
        {
            return View("My/Count");
        }

        public ActionResult My_List(int EType, int EDType)
        {
            string title = "";

            switch (EType)
            {
                case 1:
                    title = "报事报修";
                    break;
                case 2:
                    title = "投诉";
                    break;
                case 3:
                    title = "其它";
                    break;
                default:
                    title = "未知";
                    break;
            }
            if (EType == 1 || EType == 2)
            {
                switch (EDType)
                {
                    case 1:
                        title += "-未派单";
                        break;
                    case 2:
                        title += "-进行中";
                        break;
                    case 3:
                        title += "-已完成";
                        break;
                }
            }
            if (EType == 3)
            {
                switch (EDType)
                {
                    case 1:
                        title += "-建议";
                        break;
                    case 2:
                        title += "-表扬";
                        break;
                    case 3:
                        title += "-咨询";
                        break;
                }
            }
            ViewData["title"] = title;
            ViewData["EType"] = EType;
            ViewData["EDType"] = EDType;
            return View("My/List");
        }

        public ActionResult My_View(int EType, int EDType, string BillCode)
        {

            ViewData["EType"] = EType;
            ViewData["EDType"] = EDType;
            ViewData["BillCode"] = BillCode;
            return View("My/View");
        }

        #endregion

        #region 租售信息
        public async Task<ActionResult> Rentsaleinfo_Add()
        {
            var data = await _frontpageAppSrvice.GetRentsaleinfoTypesForCombo();
            return View("Rentsaleinfo/Add", data);
        }

        public async Task<ActionResult> Rentsaleinfo_List()
        {
            int mode = (this.Request["mode"] ?? "").ParseTo<int>(0);

            List<ComboboxItemDto> nextItems = new List<ComboboxItemDto>();
            string thisMode = "全部";
            var types = await _frontpageAppSrvice.GetRentsaleinfoTypesForCombo();
            types.Add(new ComboboxItemDto("0", "全部"));
            foreach (var dto in types)
            {
                if (dto.Value != mode.ToString())
                {
                    nextItems.Add(dto);
                }
                else
                {
                    thisMode = dto.DisplayText;
                }
            }


            ViewData["thismode"] = thisMode;
            ViewData["nextItems"] = nextItems;
            var data = await _frontpageAppSrvice.GetRentsaleinfos(mode, BuildPageListRequstDto());
            return View("Rentsaleinfo/List", data);
            //return null;
        }

        public async Task<ActionResult> Rentsaleinfo_View(long id)
        {
            var data = await _frontpageAppSrvice.GetRentsaleinfo(id);
            return View("Rentsaleinfo/View", data);
        }

        public async Task<ActionResult> Rentsaleinfo_MyView(long id)
        {
            var data = await _frontpageAppSrvice.GetRentsaleinfo(id);
            return View("Rentsaleinfo/MyView", data);
        }

        public ActionResult Rentsaleinfo_My(int type)
        {
            string title = "未知";
            switch (type)
            {
                case 0:
                    title = "未审核的租售信息";
                    break;
                case 1:
                    title = "上架的租售信息";
                    break;
                case 2:
                    title = "下架的租售信息";
                    break;
            }

            ViewData["title"] = title;
            ViewData["type"] = type;
            return View("Rentsaleinfo/My");
        }

        #endregion
    }
}












