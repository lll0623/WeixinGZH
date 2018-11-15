using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.Wuyebase.Members.Dto;using LesoftWuye2.Core.Wuyebase.Members;
namespace LesoftWuye2.Application.Wuyebase.Members 
 {  public class MemberAppService :LesoftWuye2.Core.Wuyebase.MembersAppServiceBase,IMemberAppService 
{
private readonly IRepository<Member, long> _memberRepository;public MemberAppService(IRepository<Member, long> memberRepository){_memberRepository = memberRepository;}
public async Task CreateMember(CreateMemberInput input){var member = input.MapTo<Member>();await _memberRepository.InsertAsync(member);}
public async Task DeleteMember(long id){await _memberRepository.DeleteAsync(id);}
public async Task UpdateMember(UpdateMemberInput input){var member = await _memberRepository.GetAsync(input.Id);input.MapTo(member);await _memberRepository.UpdateAsync(member);}
public async Task<PageListResultDto<MemberListDto>> GetMembers(GetPageListRequstDto dto){var query = _memberRepository.GetAll();var where = FilterExpression.FindByGroup<Member>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<MemberListDto>>();return new PageListResultDto<MemberListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<MemberItemDto> GetMember(long id){var member = await _memberRepository.GetAsync(id);return member.MapTo<MemberItemDto>();}
}
}
