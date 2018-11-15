using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.WebApi;
using Obs;
using LesoftWuye2.Application;
using LesoftWuye2.Application.Frontpages.Session;
using LesoftWuye2.Application.Weixin.Session;
using Swashbuckle.Application;

namespace LesoftWuye2.WebApi.Api
{
    [DependsOn(typeof(AbpWebApiModule), typeof(ObsApplicationModule), typeof(LesoftWuye2ApplicationModule))]
    public class LesoftWuye2WebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(ObsApplicationModule).Assembly, "app")
                .Build();

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
               .ForAll<IApplicationService>(typeof(LesoftWuye2ApplicationModule).Assembly, "app")
               .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new WeixinAuthAttribute(IocManager));
            //ConfigureSwaggerUi();
        }

        /// <summary>
        /// 微信api授权控制
        /// </summary>
        public class WeixinAuthAttribute : ActionFilterAttribute
        {
            private readonly IIocManager _iocManager;
            public WeixinAuthAttribute(IIocManager iocManager)
            {
                _iocManager = iocManager;
            }

            public override void OnActionExecuting(HttpActionContext actionContext)
            {
                if (actionContext.Request.RequestUri.LocalPath.ToLower().StartsWith("/api/services/app/weixinservice/"))
                {
                    //从自定义header中读取openid,来验证是否登录用户
                    //openid,设置session数据
                    var session = _iocManager.Resolve<WeixinSession>();
                    session.Reset();

                    var token = getToken(actionContext);
                  
                    if (!string.IsNullOrEmpty(token))
                    {
                        session.OpenId = token;
                        session.Prepare();
                    }
                }
            }

            string getToken(HttpActionContext actionContext)
            {
                string token = actionContext.Request.GetOwinContext().Request.Cookies["_token_"] ?? "";

                if (string.IsNullOrEmpty(token))
                {
                    var tokens = actionContext.Request.Headers.GetValues("_token_");
                    var enumerable = tokens as string[] ?? tokens.ToArray();
                    if (tokens != null && enumerable.Any())
                    {
                        token = enumerable[0];
                    }
                }

                return token;


            }
        }
    }





}

