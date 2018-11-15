using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace LesoftWuye2.Core.Details
{
    public interface IDetailManager : IDomainService
    {
        void Save(DetailType type,long dataId,string content);

        string Get(DetailType type, long dataId);

        void Delete(DetailType type, long dataId);
    }
}
