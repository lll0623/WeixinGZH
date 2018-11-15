using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Dependency;
using LesoftWuye2.Application.Weixin.Session;

namespace LesoftWuye2.Web.Models
{
    public class WeixinPageAuthAttribute: ActionFilterAttribute
    {
        private readonly IIocManager _iocManager;

        public WeixinPageAuthAttribute()
        {
            
        }

        public WeixinPageAuthAttribute(IIocManager iocManager)
        {
            _iocManager = iocManager;
        }

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    base.OnActionExecuting(filterContext);

        //    var session = _iocManager.Resolve<WeixinSession>();
        //    if (session.MemberId == 0)
        //        filterContext.Result = new RedirectResult("/weixin/register");

        //}

    }
}