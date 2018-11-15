using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace LesoftWuye2.Application.PayNoifys
{
    public interface IPayNotifyAppService : IApplicationService
    {
        void Notify(int type,string source, string orderNo, decimal money,string request,string tradeNo);
    }
}
