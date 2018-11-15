using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace LesoftWuye2.Core.Details
{
    public class DetailManager : DomainService, IDetailManager
    {
        private readonly IRepository<Detail, long> _detailRepository;

        public DetailManager(IRepository<Detail, long> detailRepository)
        {
            _detailRepository = detailRepository;
        }

        public void Save(DetailType type, long dataId, string content)
        {
            var data = _detailRepository.GetAll().FirstOrDefault(t => t.Type == type && t.DataId == dataId);
            if (data == null)
                _detailRepository.Insert(new Detail()
                {
                    Type = type,
                    DataId = dataId,
                    Content = content
                });
            else
            {
                data.Content = content;
                _detailRepository.Update(data);
            }
        }

        public string Get(DetailType type, long dataId)
        {
            var data = _detailRepository.GetAll().FirstOrDefault(t => t.Type == type && t.DataId == dataId);
            if (data == null)
                return "";
            else
            {
                return data.Content;
            }
        }

        public void Delete(DetailType type, long dataId)
        {
            var data = _detailRepository.GetAll().FirstOrDefault(t => t.Type == type && t.DataId == dataId);
            if (data != null)
            {
                _detailRepository.Delete(data);
            }
        }
    }
}
