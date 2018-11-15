using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Json;
using LesoftWuye2.Application.WuyeApis.Dto;
using LesoftWuye2.Application.WuyeApis.Utils;
using LesoftWuye2.Core.Configuration;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.Wuyebase.Members;
using Newtonsoft.Json;
using Obs;

namespace LesoftWuye2.Application.WuyeApis
{
    public class WuyeApiAppSrvice : LesoftWuye2AppServiceBase, IWuyeApiAppSrvice
    {
        private readonly WuyeApi _wuyeApi;
        private readonly IRepository<Member, long> _memberRepository;
        private readonly IAppFolders _appFolders;

        public WuyeApiAppSrvice(WuyeApi wuyeApi, IRepository<Member, long> memberRepository, IAppFolders appFolders)
        {
            _wuyeApi = wuyeApi;
            _memberRepository = memberRepository;
            _appFolders = appFolders;
        }

        public GetPProjectListResult GetPProjectList()
        {
            return _wuyeApi.Get<GetPProjectListResult>("GetPProjectList");
        }

        public GetPBuildingListResult GetPBuildingList(string pProjectCode)
        {
            return _wuyeApi.Get<GetPBuildingListResult>("GetPBuildingList", new Dictionary<string, object>()
            {
                { "pProjectCode",pProjectCode}
            });
        }

        public GetPUnitListResult GetPUnitList(string pProjectCode, string pBuildingCode)
        {
            return _wuyeApi.Get<GetPUnitListResult>("GetPUnitList", new Dictionary<string, object>()
            {
                { "pProjectCode",pProjectCode},
                { "pBuildingCode",pBuildingCode}
            });
        }

        public GetPFloorListByUnitResult GetPFloorListByUnit(string pProjectCode, string pBuildingCode, string PUnitCode)
        {
            return _wuyeApi.Get<GetPFloorListByUnitResult>("GetPFloorListByUnit", new Dictionary<string, object>()
            {
                { "pProjectCode",pProjectCode},
                { "pBuildingCode",pBuildingCode},
                { "PUnitCode",PUnitCode}
            });
        }

        public GetPRoomListByFloorResult GetPRoomListByFloor(string pProjectCode, string pBuildingCode, string PUnitCode,
            string pFloorName)
        {
            return _wuyeApi.Get<GetPRoomListByFloorResult>("GetPRoomListByFloor", new Dictionary<string, object>()
            {
                { "pProjectCode",pProjectCode},
                { "pBuildingCode",pBuildingCode},
                { "PUnitCode",PUnitCode},
                //{ "pFloorName",HttpUtility.UrlEncode(pFloorName,Encoding.UTF8)}
                { "pFloorName",pFloorName}
            });
        }

        public GetMemberInfoByAPPUserIDResult GetMemberInfoByAPPUserID(string appUserID)
        {
            return _wuyeApi.Get<GetMemberInfoByAPPUserIDResult>("GetMemberInfoByAPPUserID", new Dictionary<string, object>()
            {
                { "appUserID",appUserID}
            });
        }

        public AddLinkBillResult AddLinkBill(AddLinkBillDto dto)
        {
            return _wuyeApi.Get<AddLinkBillResult>("AddLinkBill", new Dictionary<string, object>()
            {
                { "billInfo",JsonConvert.SerializeObject(dto)}
            });
        }

        public InvokeResultDto AddMemberByAPPUser(AddMemberByAPPUserDto dto)
        {
            return _wuyeApi.Get<InvokeResultDto>("AddMemberByAPPUser", new Dictionary<string, object>()
            {
                { "appUserID",dto.appUserID},
                { "appUserName",dto.appUserName},
                { "appUserPhone",dto.appUserPhone},
                { "pRoomCode",dto.pRoomCode}
            });
        }

        public InvokeResultDto MemberAddPRoom(string MemberID, string pRoomCode, string role)
        {
            return _wuyeApi.Get<InvokeResultDto>("MemberAddPRoom", new Dictionary<string, object>()
            {
                { "MemberID",MemberID},
                { "pRoomCode",pRoomCode},
                { "Role",role}
            });
        }

        public InvokeResultDto MemberRemovePRoom(string MemberID, string pRoomCode)
        {
            return _wuyeApi.Get<InvokeResultDto>("MemberRemovePRoom", new Dictionary<string, object>()
            {
                { "MemberID",MemberID},
                { "pRoomCode",pRoomCode}
            });
        }

        public SearchHousingManagementResult SearchHousingManagement(string PRoomCode)
        {
            return _wuyeApi.Get<SearchHousingManagementResult>("SearchHousingManagement", new Dictionary<string, object>()
            {
                { "upk","System"},
                { "pRoomCode",PRoomCode}
            });
        }

        public GetMessageItemResult GetMessageItem(string Type, string MemberId)
        {
            return _wuyeApi.Get<GetMessageItemResult>("GetMessageItem", new Dictionary<string, object>()
            {
                { "Type",Type},
                { "MemberId",MemberId}
            });
        }

        public GetMessageInfoResult GetMessageInfo(string Id)
        {
            return _wuyeApi.Get<GetMessageInfoResult>("GetMessageInfo", new Dictionary<string, object>()
            {
                { "Id",Id}
            });
        }

        public GetNoPayFeeByMemberResult GetNoPayFeeByMember(string Type, string MemberId, string PRoomCode)
        {
            var data = _wuyeApi.Get<GetNoPayFeeByMemberResult>("GetNoPayFeeByMember", new Dictionary<string, object>()
            {
                { "Type",Type},
                { "MemberId",MemberId},
                { "PRoomCode",PRoomCode}
            });
            if (data.Info != null)
            {
                foreach (var item in data.Info)
                {
                    List<string> feeids = new List<string>();
                    if (item.Items == null) continue;
                    foreach (var infoData in item.Items)
                    {
                        feeids.Add(infoData.FeeId);
                    }
                    item.Id = string.Join("|", feeids.ToArray());
                }
            }

            return data;
        }

        public GetEventCountResult GetEventCount(string Type, string MemberId)
        {
            return _wuyeApi.Get<GetEventCountResult>("GetEventCount", new Dictionary<string, object>()
            {
                { "Type",Type},
                { "MemberId",MemberId}
            });
        }

        public GetEventItemResult GetEventItem(string Type, string MemberId, string EType, string EDType, int PageIndex, int PageSize)
        {
            return _wuyeApi.Get<GetEventItemResult>("GetEventItem", new Dictionary<string, object>()
            {
                { "Type",Type},
                { "MemberId",MemberId},
                { "EType",EType},
                { "EDType",EDType},
                { "PageIndex",PageIndex},
                { "PageSize",PageSize}
            });
        }

        public GetEventInfoResult GetEventInfo(string EType, string EDType, string BillCode)
        {
            return _wuyeApi.Get<GetEventInfoResult>("GetEventInfo", new Dictionary<string, object>()
            {

                { "EType",EType},
                { "EDType",EDType},
                { "BillCode",BillCode}
            });
        }

        public CreateReceiptResult CreateReceipt(string BillCode, string Amount, string OtherCode)
        {
            return _wuyeApi.Get<CreateReceiptResult>("CreateReceipt", new Dictionary<string, object>()
            {
                { "BillCode",BillCode},
                { "Amount",Amount},
                { "OtherCode",OtherCode}
            });
        }

        public CreatePayBillResult CreatePayBill(string FeedIds, string MemberId, string PRoomCode, string Amount)
        {
            return _wuyeApi.Get<CreatePayBillResult>("CreatePayBill", new Dictionary<string, object>()
            {
                { "FeedIds",FeedIds},
                { "MemberId",MemberId},
                { "PRoomCode",PRoomCode},
                { "Amount",Amount}
            });
        }

        public GetPRoomInfoByPhoneResult GetPRoomInfoByPhone(string Phone, string memberId)
        {
            return _wuyeApi.Get<GetPRoomInfoByPhoneResult>("GetPRoomInfoByPhone", new Dictionary<string, object>()
            {
                { "Phone",Phone},
                { "memberId",memberId}
            });
        }

        public GetUserInfoResult GetUserInfo(string Id)
        {
            var data = _wuyeApi.Get<GetUserInfoResult>("UserService/GetUserInfo", new Dictionary<string, object>()
            {
                { "Id",Id}
            });
            return data;
        }

        public LoginResult Login(string Phone, string PassWord)
        {
            return _wuyeApi.Get<LoginResult>("UserService/Login", new Dictionary<string, object>()
            {
                { "Phone",Phone},
                { "PassWord",PassWord}
            });
        }

        public InvokeResultDto ChangeDefaultPRoom(string MemberId, string pRoomCode)
        {
            return _wuyeApi.Get<InvokeResultDto>("ChangeDefaultPRoom", new Dictionary<string, object>()
            {
                { "MemberId",MemberId},
                { "pRoomCode",pRoomCode}
            });
        }

        public SendSMSResult SendSMS(string Phone, int Type)
        {
            string accountCode = SettingManager.GetSettingValue(AppSettings.Wuye.AccountCode);
            return _wuyeApi.Get<SendSMSResult>("API/SendSMS", new Dictionary<string, object>()
            {
                { "Account",accountCode},
                { "Phone",Phone},
                { "Type",Type}
            });
        }

        public RegisteredResult Registered(string Phone, string Password, string Name, string NickName)
        {
            return _wuyeApi.Get<RegisteredResult>("UserService/Registered", new Dictionary<string, object>()
            {
                { "Phone",Phone},
                { "Password",Password},
                { "Name",Name},
                { "NickName",NickName}
            });
        }

        public InvokeResultDto RetrievePassWord(string Phone, string NewPwd, string VerCode)
        {
            var result = _wuyeApi.Get<InvokeResultDto>("UserService/RetrievePassWord", new Dictionary<string, object>()
            {
                { "Phone",Phone},
                { "NewPwd",NewPwd},
                { "VerCode",VerCode}
            });

            return result;
        }

        public GetPRoomPrePayItemResult GetPRoomPrePayItem(string pRoomCode)
        {
            return _wuyeApi.Get<GetPRoomPrePayItemResult>("fmservice/GetPRoomPrePayItem", new Dictionary<string, object>()
            {
                { "pRoomCode",pRoomCode}
            });
        }

        public CreatePayOrderPrePayResult CreatePayOrderPrePay(string pRoomCode, string PrePayItems, string AmountSummary)
        {
            return _wuyeApi.Get<CreatePayOrderPrePayResult>("fmservice/CreatePayOrderPrePay", new Dictionary<string, object>()
            {
                { "pRoomCode",pRoomCode},
                { "PrePayItems",PrePayItems},
                { "AmountSummary",AmountSummary}
            });
        }

        public PayByOrderResult PayByOrder(string OrderBillCode, string Amount, string PayBillCode, string PayWay)
        {

            return _wuyeApi.Get<PayByOrderResult>("fmservice/PayByOrder", new Dictionary<string, object>()
            {
                { "OrderBillCode",OrderBillCode},
                { "Amount",Amount},
                { "PayBillCode",PayBillCode},
                { "PayWay",PayWay}
            });
        }

        public CreateUserByWXResult CreateUserByWX(string phone, string nickname, string name)
        {
            return _wuyeApi.Get<CreateUserByWXResult>("UserService/CreateUserByWX", new Dictionary<string, object>()
            {
                { "phone",phone},
                { "nickname",nickname},
                { "name",name}
            });
        }

        public InvokeResultDto AddLinkBillMemberMSG(string billcode, string memberid, string chartinfo)
        {
            return _wuyeApi.Get<CreateUserByWXResult>("csservice/AddLinkBillMemberMSG", new Dictionary<string, object>()
            {
                { "billcode",billcode},
                { "memberid",memberid},
                { "chartinfo",chartinfo}
            });
        }

        public InvokeResultDto UpdateLinkBillEvaluation(string billcode, string evalLevel, string suggest)
        {
            return _wuyeApi.Get<CreateUserByWXResult>("csservice/UpdateLinkBillEvaluation", new Dictionary<string, object>()
            {
                { "billcode",billcode},
                { "EvalLevel",evalLevel},
                { "Suggest",suggest}
            });
        }

        public GetPRoomInfoByPhoneResult GetPRoomInfoByQRCode(string content, string memberId)
        {
            return _wuyeApi.Get<GetPRoomInfoByPhoneResult>("GetPRoomInfoByQRCode", new Dictionary<string, object>()
            {
                { "content",content},
                { "memberId",memberId}
            });
        }

        public GetPRoomInfoByPhoneResult GetPRoomInfoByUserPass(string username, string password, string memberId)
        {
            return _wuyeApi.Get<GetPRoomInfoByPhoneResult>("GetPRoomInfoByUserPass", new Dictionary<string, object>()
            {
                { "username",username},
                { "password",password},
                { "memberId",memberId}
            });
        }

        public GetBillResult GetBill(string memberId)
        {
            return _wuyeApi.Get<GetBillResult>("GetBill", new Dictionary<string, object>()
            {
                { "memberId",memberId}
            });
        }

        public GetBillResult GetBill2(string memberId)
        {
            return _wuyeApi.Get<GetBillResult>("GetBill2", new Dictionary<string, object>()
            {
                { "memberId",memberId}
            });
        }

        public GetBillResult GetCheck(string memberId)
        {
            return _wuyeApi.Get<GetBillResult>("GetCheck", new Dictionary<string, object>()
            {
                { "memberId",memberId}
            });
        }
    }
}
