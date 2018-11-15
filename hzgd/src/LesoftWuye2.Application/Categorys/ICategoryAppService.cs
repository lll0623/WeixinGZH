using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.Categories.Dto;
namespace LesoftWuye2.Application.Categorys
{
    public interface ICategoryAppService : IApplicationService
    {
        Task CreateCategory(CreateCategoryInput input);
        Task DeleteCategory(long id);
        Task UpdateCategory(UpdateCategoryInput input);
        Task<PageListResultDto<CategoryListDto>> GetCategorys(GetPageListRequstDto dto);
        Task<CategoryItemDto> GetCategory(long id);

        Task<List<ComboboxItemDto>> GetCategoryForCombo();
    }
}
