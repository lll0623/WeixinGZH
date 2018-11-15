using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using LesoftWuye2.Application.ApiLogs.Dto;
using LesoftWuye2.Core.ApiLogs;
using Newtonsoft.Json;
using Obs.Dto;
using Obs.Filter;

namespace LesoftWuye2.Application.ApiLogs
{
    public class ApiLogAppService : LesoftWuye2AppServiceBase, IApiLogAppService
    {
        private readonly IRepository<ApiLog, long> _apilogRepository;

        public ApiLogAppService(IRepository<ApiLog, long> apilogRepository)
        {
            _apilogRepository = apilogRepository;
        }

        public async Task CreateApiLog(CreateApiLogInput input)
        {
            var apilog = input.MapTo<ApiLog>(); await _apilogRepository.InsertAsync(apilog);
        }

        public async Task DeleteApiLog(long id)
        {
            await _apilogRepository.DeleteAsync(id);
        }

        public async Task UpdateApiLog(UpdateApiLogInput input)
        {
            var apilog = await _apilogRepository.GetAsync(input.Id); input.MapTo(apilog); await _apilogRepository.UpdateAsync(apilog);
        }

        public async Task<PageListResultDto<ApiLogListDto>> GetApiLogs(GetPageListRequstDto dto)
        {
            var query = _apilogRepository.GetAll();
            var where = FilterExpression.FindByGroup<ApiLog>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<ApiLogListDto>>(); return new PageListResultDto<ApiLogListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<ApiLogItemDto> GetApiLog(long id)
        {
            var apilog = await _apilogRepository.GetAsync(id);
            var dto= apilog.MapTo<ApiLogItemDto>();
            if (!string.IsNullOrEmpty(dto.Response))
                dto.Response = ConvertJsonString(dto.Response);
            dto.Response = System.Web.HttpUtility.HtmlEncode(dto.Response);
            return dto;
        }

        public async Task ClearAll()
        {
            await _apilogRepository.DeleteAsync(t => t.Id > 0);
        }

        private string ConvertJsonString(string str)
        {
            try
            {
                //格式化json字符串
                JsonSerializer serializer = new JsonSerializer();
                TextReader tr = new StringReader(str);
                JsonTextReader jtr = new JsonTextReader(tr);
                object obj = serializer.Deserialize(jtr);
                if (obj != null)
                {
                    StringWriter textWriter = new StringWriter();
                    JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4,
                        IndentChar = ' '
                    };
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }
                else
                {
                    return str;
                }
            }
            catch (Exception ex)
            {
                return str;
            }
        
        }
    }
}
