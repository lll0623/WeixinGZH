using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Json;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.Orders;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using LesoftWuye2.Core.Configuration;
using Obs;
using Senparc.Weixin.MP.TenPayLibV3;
using LesoftWuye2.Core.Mall.RefundOrders;

namespace LesoftWuye2.Application.Jobs
{
    public class OrderRefundWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {

        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<OrderPay, long> _orderPayRepository;
        private readonly IRepository<RefundOrder, long> _refundOrderRepository;
        private readonly AppFolders _appFolders;

        public OrderRefundWorker(AbpTimer timer,
            IRepository<Order, long> orderRepository,
            IRepository<OrderPay, long> orderPayRepository,
           IRepository<RefundOrder, long> refundOrderRepository,
            AppFolders appFolders
            ) : base(timer)
        {
            _orderRepository = orderRepository;
            _orderPayRepository = orderPayRepository;
            _refundOrderRepository = refundOrderRepository;
            _appFolders = appFolders;
            Timer.Period = 1000 * 60 * 10; //每10分钟执行一次
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            //将已经过期的团购订单设置为失败,并退款
            //_orderRepository.GetAll().Where(t=>t.Type==OrderType.Groupon && t.GrouponStatus==GrouponStatus.Grouponing)

            var groupOrdrs = _refundOrderRepository.GetAll()
                 .Where(t => t.Status==RefundStatus.Accept)
                 .ToList();

            Logger.Info("OrderRefundWorker 需要退款数量:" + groupOrdrs.Count);

            foreach (var refundOrder in groupOrdrs)
            {
                //设置对应订单的状态
                var orderInfo = _orderRepository.FirstOrDefault(t => t.Id == refundOrder.OrderId);
                if (orderInfo != null  &&
                    orderInfo.Status==OrderStatus.Cancel && orderInfo.RefundStatus==RefundStatus.Accept)
                {
                    var payinfo = _orderPayRepository.FirstOrDefault(t => t.OrderId == orderInfo.Id);
                    if (payinfo == null) continue;

                    switch (payinfo.Channel)
                    {
                        case "微信支付":
                            string nonceStr = TenPayV3Util.GetNoncestr();
                            string outTradeNo = orderInfo.OrderNo;
                            string outRefundNo = "OutRefunNo-" + DateTime.Now.Ticks;
                            int totalFee = (int)(payinfo.Money * 100);
                            int refundFee = totalFee;
                            var paykey = SettingManager.GetSettingValue(AppSettings.Weixin.PayKey);
                            var AppId = SettingManager.GetSettingValue(AppSettings.Weixin.AppId);
                            var MchId = SettingManager.GetSettingValue(AppSettings.Weixin.MchId);

                            string opUserId = MchId;
                            var dataInfo = new TenPayV3RefundRequestData(AppId, MchId, paykey,
                                null, nonceStr, null, outTradeNo, outRefundNo, totalFee, refundFee, opUserId, null);
                            var cert = Path.Combine(_appFolders.Root, "apiclient_cert.p12");//  @"D:\cert\apiclient_cert_SenparcRobot.p12";//根据自己的证书位置修改
                            var password = MchId;//默认为商户号，建议修改
                            var result = TenPayV3.Refund(dataInfo, cert, password);
                            Logger.Warn("refundFee:"+ refundFee);
                            Logger.Warn(result.ToJsonString());
                            if (result.IsResultCodeSuccess() && result.IsReturnCodeSuccess())
                            {
                                payinfo.RefundsTradeNo = result.out_refund_no;
                                payinfo.RefundsPayTime = DateTime.Now;
                                orderInfo.RefundStatus = RefundStatus.Done;
                                _orderRepository.Update(orderInfo);

                                refundOrder.Status = RefundStatus.Done;
                                _refundOrderRepository.Update(refundOrder);
                               
                            }
                            

                            break;
                        case "支付宝":

                            string APPID = "2016081501749843";
                            string PID = "2088421556051016";
                            string APP_PRIVATE_KEY = @"MIIEowIBAAKCAQEArtd47la2jHyV5wbTkn/GtVPvTzTr0w6PxWYRY/dnscHVHyPd56Yc/NMJoavcMvJaH4lUzmTBcj3Nz8cv4gsVqYUqyojtukh9JBqW4njdC3IuggBH1Lmo/6cgaZjOzSEHcwyWNU0TcgfFj+y8Kd47YVdh9+xW9/M3nQn9MAaQgfHZbmLMzV5mg/IsB9JzCG963JSuMGq1CiEtYggllC+bhLtJTi+Mmlf0EcTI7CWd80ibMVgtaCyuEl9mdPo9qf0uUHQvZjPo24H24yhinspNlWYkRz9KeqU+agaIz5VKL7Nqwf5YZMauCzsDvM6wurfAHBClRh/UfAvxHN7+Oeqz8QIDAQABAoIBAF/Vd1Gccf7bIwc4tKsuImqtkRRnO4O6DY/zfEDBETNbvUeOT0lzwZvKyRK2ssGyGTgD/FoM3AOUYMUsttA9pyf9+BB/sV5T8VPixyVnfjGR6nATW0v8X+eRYbC/s0q4ee7TzVl139y26dETv6drSjz2upo8DwdlZuxK116Fmpu+XiMc3u0QCbrnbFamhMXQoaYrYcU+pYYLc7Zzoc9ZN4szULZDDF6wkrktZbAuKyiERYF8vLNS2Gm6A6hMpVPpviSA48hhk4k4Gjn2oihEsvuqDf5Y8yI+5xASBLyqylvpZTNjp6B4V49MjyrY5DEecdpTf1kzFR6clAxaOmoCgCkCgYEA4t/5Ikbvyn3WdZK8Oyqlw9MxQKrZHz//s1OE+QV5Aqf5R3U30HaOGdImvocQb+Shgz7nV9fKN1hVdKNK6ASgQ0clhQGb6FrdxdyvPGrDmjDBvKULNM7rFbzeUSC5wHEHbZ8xkMY+sdai0R/sEk8FvUa+hzU3yyv5SmxJ2ESLjUMCgYEAxUl6Qaa5BO8qsrj33Px3Ag8umzLP7Dco+cPjR+b2D1Eg2RYvvr/+68gXB27nde5GEo+i8mIi9C2vfAsfbHJqLFBWX95gePMnRFoeWylNPqSz1+cGZJTsBWvptj7nHle5MYn8fe4JBLBIOGAUwPwA5SN5pMRkhU1+xT3mcUjRLLsCgYBw7ifm5gSKeOT9lVLY6LumpEOJ+wEkywiOzO4NvqmjptUwuqpTvA+zzqW2hSiradTzraYeVa20quWur3Gj2Fml445LjKd8m251BQq9Oi+vWsG1EzpmyPC/20mWfIG5xwl5iZp0hBnFEB/vlMI/wtIKi2Jfjx/8pCDs6MZBPq1wXQKBgG0lHmbtttRdAJFJtY7jeW+BOLaR4Of9CEVNsxLXWu/UYUjYdmegTobg9qSdHZ5nyQqBvpM76byO/dOxT5wunECR3YdCPrsLQoEVHlAuxFZQxlI+tJG2tfC15+F0YWau/3zBqxd8Ni8K25mcxj6R7GjYPHcEU9xPqD+05CVuNJL7AoGBAM1Xi7gWKaV7bYQdmW2HFfmAaW1z5DUPPZoVQ/wsZwSbBPf3Hn7qE6DM+dRMK6RBL1GIuRdxfCPOT4ktei9dvghM42gU2me8U5B6T5s1Sf99m/iw6svyoU5OAcIPSZ38R4l00VA1w+jYSMdQukhBlgYZQgRxeXKOFiM9IpkVvH9U";
                            string ALIPAY_PUBLIC_KEY = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlhGDVAL2+yN9HYkxgLBTI4MswpHi9QLrN/8zkJDFWQOGJEuUic2wPEJ/Mi45wuNMpeE57gzGMEk9AODTb+dzN5uA9gd0GVBcDI+5pBxJASWCouDUQR4Lvuo52vPTJGc2oageP5Of2L7eUazIYKU6jxoJWFbjzRN36p0l3SCvyvkOFaiTyIUFqDe5pprEPRWdxJmbwDnPsF4b3W1NdivoxP9y/ztCGN7ImX/mPLRrwveZe4xqvDOJ0sZ9LgfzJL7POE8lkj7m5LWM/5cNDKB3rtElp6eDWI7blBCvBsqMuzaCjzuYswYttu/j5gVLUd1oQ8v0wdw8lmPskcYEtobF6wIDAQAB";
                            string CHARSET = "UTF-8";


                            APPID = SettingManager.GetSettingValue(AppSettings.Alipay.APPID);
                            PID = SettingManager.GetSettingValue(AppSettings.Alipay.PID);
                            APP_PRIVATE_KEY = SettingManager.GetSettingValue(AppSettings.Alipay.APP_PRIVATE_KEY);
                            ALIPAY_PUBLIC_KEY = SettingManager.GetSettingValue(AppSettings.Alipay.ALIPAY_PUBLIC_KEY);

                            var client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, CHARSET, false);
                            AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
                            request.BizContent = "{" +
                            "    \"out_trade_no\":\"" + orderInfo.OrderNo + "\"," +
                            "    \"trade_no\":\"" + payinfo.TradeNo + "\"," +
                            "    \"refund_amount\":" + payinfo.Money.ToString("F2") + "," +
                            "    \"refund_reason\":\"退款\"" +
                            "  }";
                            AlipayTradeRefundResponse response = client.Execute(request);
                            Logger.Warn(response.Body);
                            if (!response.IsError)
                            {
                                payinfo.RefundsTradeNo = response.TradeNo;
                                payinfo.RefundsPayTime = DateTime.Now;
                                orderInfo.RefundStatus = RefundStatus.Done;
                                _orderRepository.Update(orderInfo);

                                refundOrder.Status = RefundStatus.Done;
                                _refundOrderRepository.Update(refundOrder);

                            }
                            break;
                    }
                }




            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }

}
