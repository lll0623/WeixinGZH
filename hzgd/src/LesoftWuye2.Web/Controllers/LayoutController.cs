using System;
using System.Linq;
using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using Obs.Sessions;
using LesoftWuye2.Web.Models.Layout;
using LesoftWuye2.Web.Views;


namespace LesoftWuye2.Web.Controllers
{
    public class LayoutController : LesoftWuye2ControllerBase
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly ISessionAppService _sessionAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ILanguageManager _languageManager;

        public LayoutController(
            IUserNavigationManager userNavigationManager, 
            ISessionAppService sessionAppService, 
            IMultiTenancyConfig multiTenancyConfig,
            ILanguageManager languageManager)
        {
            _userNavigationManager = userNavigationManager;
            _sessionAppService = sessionAppService;
            _multiTenancyConfig = multiTenancyConfig;
            _languageManager = languageManager;
        }

        //[ChildActionOnly]
        //public PartialViewResult TopMenu(string activeMenu = "")
        //{
        //    var model = new TopMenuViewModel
        //                {
        //                    MainMenu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier())),
        //                    ActiveMenuItemName = activeMenu
        //                };

        //    return PartialView("_TopMenu", model);
        //}

        [ChildActionOnly]
        public PartialViewResult LeftMenu(string activeMenu = "")
        {
            var model = new TopMenuViewModel
            {
                MainMenu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier())),
                ActiveMenuItemName = activeMenu
            };

            //计算出上级
            foreach (var item in model.MainMenu.Items)
            {
                var pitem = item.Items.FirstOrDefault(t => t.Name == model.ActiveMenuItemName);
                if (pitem != null)
                {
                    model.ActiveMenuItemParentName = item.Name;
                    break;
                }
            }

            return PartialView("_LeftMenu", model);
        }

        [ChildActionOnly]
        public PartialViewResult Breadcrumb(string activeMenu = "")
        {
            var menus =
                AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier()));

            var model = new BreadcrumbViewModel{};
            model.Items.Add(new BreadcrumbViewModelItem() {Name = "首页",Link = "/",Icon = "fa fa-home" });

            //计算出上级
            foreach (var item in menus.Items)
            {
                if (item.Name == activeMenu)
                {
                    model.Items.Add(new BreadcrumbViewModelItem() { Name = item.DisplayName, Link = WrapUrl(item.Url),IsActive = true});
                    break;
                }

                var pitem = item.Items.FirstOrDefault(t => t.Name == activeMenu);
                if (pitem != null)
                {
                    model.Items.Add(new BreadcrumbViewModelItem() { Name = item.DisplayName, Link = "#" });
                    model.Items.Add(new BreadcrumbViewModelItem() { Name = pitem.DisplayName, Link = WrapUrl(pitem.Url), IsActive = true });
                    break;
                }
            }

            return PartialView("_Breadcrumb", model);
        }

        string WrapUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return this.Request.ApplicationPath;
            }

            if (UrlChecker.IsRooted(url))
            {
                return url;
            }

            return this.Request.ApplicationPath + url;
        }


        [ChildActionOnly]
        public PartialViewResult TopBar()
        {
            UserMenuModel model;

            if (AbpSession.UserId.HasValue)
            {
                model = new UserMenuModel
                {
                    LoginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations()),
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                };
            }
            else
            {
                model = new UserMenuModel
                {
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled
                };
            }

            return PartialView("_TopBar", model);
        }
    }
}












