using System.Text;
using System.Web.Mvc;
using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using Castle.MicroKernel.Registration;
using Microsoft.AspNet.Identity;
using Obs;
using Obs.Dto;
using Obs.Extensions;
using Obs.Filter;


namespace LesoftWuye2.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary> 
    public abstract class LesoftWuye2ControllerBase : AbpController
    {
        protected LesoftWuye2ControllerBase()
        {
            LocalizationSourceName = ObsConsts.LocalizationSourceName;
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            AutoSetPagingBar(filterContext);
        }

        #region 统一设置分页工具条

        void AutoSetPagingBar(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is ViewResult)
            {
                var viewResult = filterContext.Result as ViewResult;
                if (viewResult.Model == null) return;
                var pageModel = viewResult.Model as IPageListResultDto;
                if (pageModel != null)
                {
                    ViewData["___paging"] = BuildPagingBar(pageModel, this.Request.Url.AbsoluteUri);
                }

            }
        }

        string BuildPagingBar(IPageListResultDto pageListResult, string url)
        {
            StringBuilder html = new StringBuilder();

            string nodata = "<div class='col-sm-12'><div class='dataTables_info active' style='text-align: center; margin-bottom: 8px' id='simpledatatable_info' role='alert' aria-live='polite' aria-relevant='all'>暂无数据</div></div>";
            string prx = "<div class='row DTTTFooter'>";
            string end = " </div>";
            if (pageListResult.ItemsCount == 0)
            {
                html.Append(prx);
                html.Append(nodata);
                html.Append(end);
            }
            else
            {
                bool firstEnable = pageListResult.PageIndex != 1;
                bool prevEnable = pageListResult.PageIndex > 1;
                bool nextEnable = pageListResult.PageIndex + 1 <= pageListResult.PageCount;
                bool lastEnable = pageListResult.PageIndex < pageListResult.PageCount;

                string firsturl = Obs.Utils.UrlHelper.ReplaceParaValue(url, "p", "1");
                string prevurl = Obs.Utils.UrlHelper.ReplaceParaValue(url, "p", (pageListResult.PageIndex - 1).ToString());
                string nexturl = Obs.Utils.UrlHelper.ReplaceParaValue(url, "p", (pageListResult.PageIndex + 1).ToString());
                string lasturl = Obs.Utils.UrlHelper.ReplaceParaValue(url, "p", pageListResult.PageCount.ToString());

                if (!firstEnable) firsturl = "javascript:void(0)";
                if (!prevEnable) prevurl = "javascript:void(0)";
                if (!nextEnable) nexturl = "javascript:void(0)";
                if (!lastEnable) lasturl = "javascript:void(0)";



                string pageInfo = $"<div class='col-sm-6'><div class='dataTables_info' id='simpledatatable_info' role='alert' aria-live='polite' aria-relevant='all'>第{pageListResult.PageIndex}页,共{pageListResult.PageCount}页 当前显示第{pageListResult.Begin}-{pageListResult.End}条记录,共{pageListResult.TotalCount}条记录</div></div>";
                string pageButtons =
                    $@"<div class='col-sm-6'>
                        <div class='dataTables_paginate paging_bootstrap' id='simpledatatable_paginate'>
                            <ul class='pagination'>
		                        <li class='prev {(firstEnable ? "" : "disabled")}' ng-click='gotoFirst()'><a href = '{firsturl}' title='First'><i class='fa fa-angle-double-left'></i>&nbsp;首页</a></li>
		                        <li class='prev {(prevEnable ? "" : "disabled")}' ng-click='prevPage()'><a href = '{prevurl}' title='Prev'><i class='fa fa-angle-left'></i>&nbsp;前一页</a></li>
		                        <li class='disabled active'>
			                        <a href = 'javascript:void(0);' >{pageListResult.PageIndex}/{pageListResult.PageCount}</a>
		                        </li>
		                        <li class='next {(nextEnable ? "" : "disabled")}' ng-click='nextPage();'><a href = '{nexturl}' title='Next'><i class='fa fa-angle-right'></i>&nbsp;后一页</a></li>
		                        <li class='next {(lastEnable ? "" : "disabled")}' ng-click='gotoLast();'><a href = '{lasturl}' title='Last'><i class='fa fa-angle-double-right'></i>&nbsp;末页</a></li>
	                        </ul>
                        </div>
                     </div>";

                html.Append(prx);
                html.Append(pageInfo);
                html.Append(pageButtons);
                html.Append(end);
            }

            return html.ToString();
        }

        #endregion

        protected GetPageListRequstDto BuildPageListRequstDto()
        {
            GetPageListRequstDto dto = new GetPageListRequstDto();
            dto.PageSize = 10;//固定
            dto.CurrentPage = this.Request["p"].ParseTo(1);

            //分析查询参数
            string q = this.Request["q"] ?? "";
            var paras = q.Split('|');
            foreach (string para in paras)
            {
                var item = para.Split(':');
                if (item.Length != 2) continue;
                string field = item[0];

                var kvs = item[1].Split(',');
                if (kvs.Length != 5 && kvs.Length != 6) continue;
                FilterCondition condition = new FilterCondition();
                condition.Field = field;
                condition.ignore = kvs[0] == "1";
                condition.logic = (kvs[1] == "1" ? "OR" : "AND");

                string type = "like";
                switch (kvs[2])
                {
                    case "0":
                        type = "text";
                        break;
                    case "1":
                        type = "list";
                        break;
                    case "2":
                        type = "date";
                        break;
                }
                condition.type = type;
                if (condition.type == "date" && kvs.Length != 6) continue;
                if (condition.type != "date" && kvs.Length != 5) continue;

                string @operator = "like";
                switch (kvs[3])
                {
                    case "0":
                        @operator = "like";
                        break;
                    case "1":
                        @operator = "equal";
                        break;
                    case "2":
                        @operator = "range";
                        break;
                }
                condition.@operator = @operator;

                if (condition.type != "date")
                {
                    condition.value = kvs[4];
                }
                else
                {
                    condition.minValue = kvs[4];
                    condition.maxValue = kvs[5];
                }
                dto.Filter.Add(condition);
            }

            ViewData["___searchparas"] = q;
            return dto;
        }

        protected string GetCookie(string key)
        {
            try
            {
                if (this.Request.Cookies?[key] != null)
                {
                    return this.Request.Cookies[key].Value;
                }
            }
            catch (System.Exception ex)
            {
                Logger.Warn("读取cookie出错", ex);
            }

            return "";
        }
    }
}












