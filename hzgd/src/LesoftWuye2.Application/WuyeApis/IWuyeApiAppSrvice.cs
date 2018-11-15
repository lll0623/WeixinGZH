using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using LesoftWuye2.Application.WuyeApis.Dto;

namespace LesoftWuye2.Application.WuyeApis
{
    public interface IWuyeApiAppSrvice : IApplicationService
    {
        GetPProjectListResult GetPProjectList();

        GetPBuildingListResult GetPBuildingList(string pProjectCode);

        GetPUnitListResult GetPUnitList(string pProjectCode,string pBuildingCode);

        GetPFloorListByUnitResult GetPFloorListByUnit(string pProjectCode, string pBuildingCode,string PUnitCode);

        GetPRoomListByFloorResult GetPRoomListByFloor(string pProjectCode, string pBuildingCode, string PUnitCode,string pFloorName);

        GetMemberInfoByAPPUserIDResult GetMemberInfoByAPPUserID(string appUserID);

        AddLinkBillResult AddLinkBill(AddLinkBillDto dto);

        InvokeResultDto AddMemberByAPPUser(AddMemberByAPPUserDto dto);

        InvokeResultDto MemberAddPRoom(string MemberID, string pRoomCode,string role);

        InvokeResultDto MemberRemovePRoom(string MemberID, string pRoomCode);

        SearchHousingManagementResult SearchHousingManagement(string PRoomCode);

        GetMessageItemResult GetMessageItem(string Type,string MemberId);

        GetMessageInfoResult GetMessageInfo(string Id);

        GetNoPayFeeByMemberResult GetNoPayFeeByMember(string Type, string MemberId, string PRoomCode);

        GetEventCountResult GetEventCount(string Type, string MemberId);

        GetEventItemResult GetEventItem(string Type, string MemberId, string EType,string EDType,int PageIndex,int PageSize);

        GetEventInfoResult GetEventInfo(string EType, string EDType, string BillCode);

        CreateReceiptResult CreateReceipt(string BillCode, string Amount, string OtherCode);

        CreatePayBillResult CreatePayBill(string FeedIds, string MemberId, string PRoomCode, string Amount);

        GetPRoomInfoByPhoneResult GetPRoomInfoByPhone(string Phone,string memberId);

        GetUserInfoResult GetUserInfo(string Id);

        LoginResult Login(string Phone, string PassWord);
        
        InvokeResultDto ChangeDefaultPRoom(string MemberId, string pRoomCode);

        SendSMSResult SendSMS(string Phone, int Type);

        RegisteredResult Registered(string Phone, string Password,string Name,string NickName);

        InvokeResultDto RetrievePassWord(string Phone, string NewPwd, string VerCode);

        GetPRoomPrePayItemResult GetPRoomPrePayItem(string pRoomCode);

        CreatePayOrderPrePayResult CreatePayOrderPrePay(string pRoomCode,string PrePayItems,string AmountSummary);

        PayByOrderResult PayByOrder(string OrderBillCode, string Amount, string PayBillCode,string PayWay);

        CreateUserByWXResult CreateUserByWX(string phone, string nickname, string name);

        InvokeResultDto AddLinkBillMemberMSG(string billcode,string memberid,string chartinfo);

        InvokeResultDto UpdateLinkBillEvaluation(string billcode, string evalLevel, string suggest);

        GetPRoomInfoByPhoneResult GetPRoomInfoByQRCode(string content, string memberId);


        GetPRoomInfoByPhoneResult GetPRoomInfoByUserPass(string username,string password, string memberId);

        GetBillResult GetBill(string memberId);
        GetBillResult GetBill2(string memberId);

        GetBillResult GetCheck(string memberId);
    }
}
