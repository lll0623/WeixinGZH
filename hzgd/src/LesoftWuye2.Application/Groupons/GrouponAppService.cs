using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Groupons.Dto;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.Products;

namespace LesoftWuye2.Application.Groupons
{
    public class GrouponAppService : LesoftWuye2AppServiceBase, IGrouponAppService
    {
        private readonly IRepository<Groupon, long> _grouponRepository;
        private readonly IRepository<Product, long> _productRepository;

        public GrouponAppService(IRepository<Groupon, long> grouponRepository,
            IRepository<Product, long> productRepository)
        {
            _grouponRepository = grouponRepository;
            _productRepository = productRepository;
        }

        public async Task CreateGroupon(CreateGrouponInput input)
        {
            if(input.RequireCount<2)
                throw new UserFriendlyException("拼团人数必须为2人(含2人)以上");

            if(input.StartTime>=input.ExpireTime)
                throw new UserFriendlyException("团购结束时间必须大于开始时间");

            var groupon = input.MapTo<Groupon>();
            await _grouponRepository.InsertAsync(groupon);
        }

        public async Task DeleteGroupon(long id)
        {
            await _grouponRepository.DeleteAsync(id);
        }

        public async Task UpdateGroupon(UpdateGrouponInput input)
        {
            if (input.RequireCount < 2)
                throw new UserFriendlyException("拼团人数必须为2人(含2人)以上");
            if (input.StartTime >= input.ExpireTime)
                throw new UserFriendlyException("团购结束时间必须大于开始时间");
            var groupon = await _grouponRepository.GetAsync(input.Id);
            input.MapTo(groupon);
            await _grouponRepository.UpdateAsync(groupon);
        }

        public async Task<PageListResultDto<GrouponListDto>> GetGroupons(GetPageListRequstDto dto)
        {
            var query = _grouponRepository.GetAll();
            var where = FilterExpression.FindByGroup<Groupon>(dto.Filter);
            var queryCount = query.Where(where)
                 .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                   (left, right) => new
                   {
                       Groupon = left,
                       Product = right
                   });

            //if (FilterExpression.HasValue(dto.Filter, "userName"))
            //{
            //    string value = FilterExpression.GetValue(input.Filter, "userName");
            //    queryCount = queryCount.Where(t => t.User.FirstOrDefault().Name.Contains(value));
            //}

            int count = await queryCount.CountAsync();

            var queryList = query.Where(where)
               .GroupJoin(_productRepository.GetAll(), left => left.ProductId, right => right.Id,
                   (left, right) => new
                   {
                       Groupon = left,
                       Product = right
                   });

           //var list = queryList.OrderByDescending(t => t.Groupon.CreationTime).PageBy(dto).ToListAsync();

            var tts = await queryList.Select(t => new GrouponAndProduct()
            {
                Groupon = t.Groupon,
                Product = t.Product.FirstOrDefault()
            }).OrderByDescending(t => t.Groupon.CreationTime).PageBy(dto).ToListAsync();

            var grouponListDtos = tts.Select(
                result =>
                {
                    var auditLogListDto = result.Groupon.MapTo<GrouponListDto>();
                    auditLogListDto.Name = result.Product?.Name;
                    auditLogListDto.Thumbnail = result.Product?.Thumbnail;
                    return auditLogListDto;
                }).ToList();

            var pageList = grouponListDtos.MapTo<List<GrouponListDto>>();
            return new PageListResultDto<GrouponListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<GrouponItemDto> GetGroupon(long id)
        {
            var groupon = await _grouponRepository.GetAsync(id);
            return groupon.MapTo<GrouponItemDto>();
        }
    }
}
