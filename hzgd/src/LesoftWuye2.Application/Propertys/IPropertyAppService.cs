using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.Propertys.Dto;
namespace LesoftWuye2.Application.Propertys {  public interface IPropertyAppService : IApplicationService 
{
Task CreateProperty(CreatePropertyInput input);
Task DeleteProperty(long id);
Task UpdateProperty(UpdatePropertyInput input);
Task<PageListResultDto<PropertyListDto>> GetPropertys(GetPageListRequstDto dto);
Task<PropertyItemDto> GetProperty(long id);
}
}
