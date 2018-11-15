using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LesoftWuye2.Application.Malls;
using LesoftWuye2.Application.Malls.Dto;
using LesoftWuye2.Application.Weixin;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.ApiLogs;
using LesoftWuye2.Core.PayNotifys;
using Newtonsoft.Json;

namespace LesoftWuye2.Application.PayNoifys
{
    public class PayNotifyAppService : LesoftWuye2AppServiceBase, IPayNotifyAppService
    {

        private readonly IRepository<PayNotify, long> _payNotifyRepository;
        private readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;
        private readonly IWeixinService _weixinService;

        public PayNotifyAppService(IRepository<PayNotify, long> payNotifyRepository, IWuyeApiAppSrvice wuyeApiAppSrvice, IWeixinService weixinService)
        {
            _payNotifyRepository = payNotifyRepository;
            _wuyeApiAppSrvice = wuyeApiAppSrvice;
            _weixinService = weixinService;
        }

        public void Notify(int type, string source, string orderNo, decimal money, string request, string tradeNo)
        {
            PayNotify notify = new PayNotify();
            notify.Source = source;
            notify.OrderNo = orderNo;
            notify.Money = money;
            notify.Request = request;
            notify.TradeNo = tradeNo;

            switch (type)
            {
                case 0:
                    //物业缴费
                    {
                        var apiResult = _wuyeApiAppSrvice.CreateReceipt(orderNo, money.ToString("F2"), tradeNo);
                        notify.Response = JsonConvert.SerializeObject(apiResult);
                        notify.Success = apiResult.Success;
                    }
                    break;
                case 1:
                    //商城
                    _weixinService.PayOrder(new OrderPayInfo()
                    {
                        Channel = source,
                        Money = money,
                        OrderNo = orderNo,
                        TradeNo = tradeNo
                    });
                    notify.Response = "NONE";
                    notify.Success = true;
                    break;
                case 2:
                    //物业预交
                    {
                        var apiResult = _wuyeApiAppSrvice.PayByOrder(orderNo, money.ToString("F2"), tradeNo, source);
                        notify.Response = JsonConvert.SerializeObject(apiResult);
                        notify.Success = apiResult.Success;
                    }

                    break;
            }



            _payNotifyRepository.Insert(notify);
        }
    }
}
