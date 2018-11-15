using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Json;
using Castle.Core.Logging;
using FSLib.Network.Http;
using LesoftWuye2.Application.WuyeApis.Dto;
using LesoftWuye2.Core.ApiLogs;
using LesoftWuye2.Core.Configuration;
using Newtonsoft.Json;

namespace LesoftWuye2.Application.WuyeApis.Utils
{
    public class WuyeApi : ITransientDependency
    {
        private readonly ISettingManager _settingManager;
        private readonly IRepository<ApiLog, long> _apilogRepository;
        private readonly ILogger _logger;


        public WuyeApi(ISettingManager settingManager,
            IRepository<ApiLog, long> apilogRepository,
            ILogger logger)
        {
            _settingManager = settingManager;
            _apilogRepository = apilogRepository;
            _logger = logger;
        }

      
        public virtual T Get<T>(string apiName, IDictionary<string, object> data = null) where T : InvokeResultDto, new()
        {
            var client = new HttpClient();
            string apiAddress = _settingManager.GetSettingValue(AppSettings.Wuye.ApiAddress);
            string accountCode = _settingManager.GetSettingValue(AppSettings.Wuye.AccountCode);

            Dictionary<string, object> postData = new Dictionary<string, object> { { "accountCode", accountCode } };
            if (data != null)
            {
                foreach (var o in data)
                {
                    postData.AddIfNotContains(new KeyValuePair<string, object>(o.Key, o.Value));
                }
            }

            string url = apiAddress + apiName + ".ashx";
            try
            {

                //string response = client.PostStringResult(url, postData);

                var ctx = client.Create<string>(HttpMethod.Post, url,data:postData);
                ctx.Send();
                string response = ctx.Result;
                
                try
                {
                    T result = JsonConvert.DeserializeObject<T>(response);
                    SaveLog(apiName, postData, response,true);
                    return result;
                }
                catch (Exception ex)
                {
                    SaveLog(apiName, postData, response,false);
                    _logger.Error("WuyeApi "+ apiName, ex);
                    T result = new T {msg = $"调用接口:{apiName},解析数据发生错误."};
                    return result;
                }
            }
            catch (Exception ex)
            {
                SaveLog(apiName, postData, "",false);
                _logger.Error("WuyeApi " + apiName, ex);
                T result = new T { msg = $"调用接口:{apiName}发生错误:" + ex.Message };
                return result;
            }
        }

        //[UnitOfWork(isTransactional: false)]
        void SaveLog(string apiName, IDictionary<string, object> requestData, string response,bool isSuccess=true)
        {
            string request = "";
            foreach (var o in requestData)
            {
                if (!string.IsNullOrEmpty(request))
                    request += "&";
                request += $"{o.Key}={o.Value}";
            }
            ApiLog apiLog=new ApiLog();
            apiLog.Name = apiName;
            apiLog.Request = request;
            apiLog.Response = response;
            apiLog.Success = isSuccess;
            _apilogRepository.Insert(apiLog);
            _logger.Debug("WuyeApi Post:"+Environment.NewLine+JsonConvert.SerializeObject(apiLog, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
