using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using LesoftWuye2.Application.Categories;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Categories.Dto;
using LesoftWuye2.Core.Categories;
namespace LesoftWuye2.Application.Categorys
{
    public class CategoryAppService : LesoftWuye2AppServiceBase, ICategoryAppService
    {
        private readonly IRepository<Category, long> _categoryRepository;

        public CategoryAppService(IRepository<Category, long> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task CreateCategory(CreateCategoryInput input)
        {
            var category = input.MapTo<Category>(); await _categoryRepository.InsertAsync(category);
        }


        public async Task DeleteCategory(long id)
        {
            await _categoryRepository.DeleteAsync(id);
        }


        public async Task UpdateCategory(UpdateCategoryInput input)
        {
            var category = await _categoryRepository.GetAsync(input.Id); input.MapTo(category); await _categoryRepository.UpdateAsync(category);
        }


        public async Task<PageListResultDto<CategoryListDto>> GetCategorys(GetPageListRequstDto dto)
        {
            var query = _categoryRepository.GetAll();
            var where = FilterExpression.FindByGroup<Category>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderBy(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<CategoryListDto>>();
            return (new PageListResultDto<CategoryListDto>(count, pageList, dto.CurrentPage, dto.PageSize));
        }


        public async Task<CategoryItemDto> GetCategory(long id)
        {
            var category = await _categoryRepository.GetAsync(id); return (category.MapTo<CategoryItemDto>());
        }

        public async Task<List<ComboboxItemDto>> GetCategoryForCombo()
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
