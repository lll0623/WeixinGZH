using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using LesoftWuye2.Application.Categories;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Categories.Dto;
using LesoftWuye2.Application.Plates.Dto;
using LesoftWuye2.Core.Categories;
using LesoftWuye2.Core.ForumPosts;
using LesoftWuye2.Core.Plates;

namespace LesoftWuye2.Application.Plates
{
    public class PlateAppService : LesoftWuye2AppServiceBase, IPlateAppService
    {
        private readonly IRepository<Plate, long> _categoryRepository;
        private readonly IRepository<ForumPost, long> _forumPostRepository;

        public PlateAppService(IRepository<Plate, long> categoryRepository, IRepository<ForumPost, long> forumPostRepository)
        {
            _categoryRepository = categoryRepository;
            _forumPostRepository = forumPostRepository;
        }
        public async Task CreatePlate(CreatePlateInput input)
        {
            var category = input.MapTo<Plate>(); await _categoryRepository.InsertAsync(category);
        }


        public async Task DeletePlate(long id)
        {
            if(await _forumPostRepository.CountAsync(t=>t.PlateId==id)>0)
                throw new UserFriendlyException("改版块已有帖子，不能删除!");
            await _categoryRepository.DeleteAsync(id);
        }


        public async Task UpdatePlate(UpdatePlateInput input)
        {
            var category = await _categoryRepository.GetAsync(input.Id); input.MapTo(category); await _categoryRepository.UpdateAsync(category);
        }


        public async Task<PageListResultDto<PlateListDto>> GetPlates(GetPageListRequstDto dto)
        {
            var query = _categoryRepository.GetAll();
            var where = FilterExpression.FindByGroup<Plate>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderBy(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<PlateListDto>>();
            return (new PageListResultDto<PlateListDto>(count, pageList, dto.CurrentPage, dto.PageSize));
        }


        public async Task<PlateItemDto> GetPlate(long id)
        {
            var category = await _categoryRepository.GetAsync(id); return (category.MapTo<PlateItemDto>());
        }

        public async Task<List<ComboboxItemDto>> GetPlateForCombo()
        {
            var items = await _categoryRepository.GetAll().OrderBy(t => t.Sort).ToListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }
    }
}
