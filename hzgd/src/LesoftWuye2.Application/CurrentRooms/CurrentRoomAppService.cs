using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.CurrentRooms.Dto;using LesoftWuye2.Core.CurrentRooms;
namespace LesoftWuye2.Application.CurrentRooms 
 {  public class CurrentRoomAppService :LesoftWuye2AppServiceBase,ICurrentRoomAppService 
{
private readonly IRepository<CurrentRoom, long> _currentroomRepository;public CurrentRoomAppService(IRepository<CurrentRoom, long> currentroomRepository){_currentroomRepository = currentroomRepository;}
public async Task CreateCurrentRoom(CreateCurrentRoomInput input){var currentroom = input.MapTo<CurrentRoom>();await _currentroomRepository.InsertAsync(currentroom);}
public async Task DeleteCurrentRoom(long id){await _currentroomRepository.DeleteAsync(id);}
public async Task UpdateCurrentRoom(UpdateCurrentRoomInput input){var currentroom = await _currentroomRepository.GetAsync(input.Id);input.MapTo(currentroom);await _currentroomRepository.UpdateAsync(currentroom);}
public async Task<PageListResultDto<CurrentRoomListDto>> GetCurrentRooms(GetPageListRequstDto dto){var query = _currentroomRepository.GetAll();var where = FilterExpression.FindByGroup<CurrentRoom>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<CurrentRoomListDto>>();return new PageListResultDto<CurrentRoomListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<CurrentRoomItemDto> GetCurrentRoom(long id){var currentroom = await _currentroomRepository.GetAsync(id);return currentroom.MapTo<CurrentRoomItemDto>();}
}
}
