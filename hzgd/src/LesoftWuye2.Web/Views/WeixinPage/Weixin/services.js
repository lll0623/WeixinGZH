
(function () {

    var serviceNamespace = app.utils.createNamespace(app, 'services.app.weixinService');

    serviceNamespace.getNotices = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetNotices',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getNewss = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetNewss',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getSubstations = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetSubstations',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getActivitys = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetActivitys',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.login = function (username, password, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/Login?username=' + username + "&password=" + password,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getMyRooms = function (ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetMyRooms',
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.addService = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/AddService',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getNoPayFeeByMember = function (pRoomCode, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetNoPayFeeByMember?PRoomCode=' + pRoomCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.setCurrentRoomInfo = function (pRoomCode, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SetCurrentRoomInfo?PRoomCode=' + pRoomCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getPRoomInfoByPhone = function (phone, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetPRoomInfoByPhone?Phone=' + phone,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getPRoomInfoByUserPass = function (username, password, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetPRoomInfoByUserPass?username=' + username + "&password=" + password,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };
    

    serviceNamespace.getPRoomInfoByQRCode = function (content, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetPRoomInfoByQRCode?content=' + content,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.memberAddPRoom = function (pRoomCode,role,ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/MemberAddPRoom?PRoomCode=' + pRoomCode+"&role="+role,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.memberRemovePRoom = function (pRoomCode, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/MemberRemovePRoom?PRoomCode=' + pRoomCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.sendSMS = function (phone, type, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SendSMS?Phone=' + phone + "&Type=" + type,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.gegistered = function (phone, password, nickname, name, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/Registered?phone=' + phone + "&password=" + password + "&nickname=" + nickname + "&name=" + name,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.createUserByWX = function (phone, nickname, name, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/CreateUserByWX?phone=' + phone + "&nickname=" + nickname + "&name=" + name,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.retrievePassWord = function (phone, NewPwd, VerCode, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/RetrievePassWord?phone=' + phone + "&NewPwd=" + NewPwd + "&VerCode=" + VerCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.logout = function (ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/Logout',
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };



    serviceNamespace.getGrouponItems = function (cid, dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetGrouponItems?cid=' + cid,
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getProvinces = function (ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetProvinces',
            type: 'POST',
            data: {}
        }, ajaxParams));
    };

    serviceNamespace.getCities = function (provinceId, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetCities?provinceId=' + provinceId,
            type: 'POST',
            data: {}
        }, ajaxParams));
    };

    serviceNamespace.getDistricts = function (cityId, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetDistricts?cityId=' + cityId,
            type: 'POST',
            data: {}
        }, ajaxParams));
    };

    serviceNamespace.addAddress = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/AddAddress',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.editAddress = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/editAddress',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getAddresss = function (ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetAddresss',
            type: 'POST',
            data: {}
        }, ajaxParams));
    };

    serviceNamespace.deleteAddress = function (id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/DeleteAddress?id=' + id,
            type: 'POST',
            data: {}
        }, ajaxParams));
    };

    serviceNamespace.setDefaultAddress = function (id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SetDefaultAddress?id=' + id,
            type: 'POST',
            data: {}
        }, ajaxParams));
    };

    serviceNamespace.syncMemberInfo = function (id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SyncMemberInfo?id=' + id,
            type: 'POST',
            data: {}
        }, ajaxParams));
    };

    serviceNamespace.getAddressForSubmit = function (selectAddressId, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetAddressForSubmit?selectAddressId=' + selectAddressId,
            type: 'POST',
            data: {}
        }, ajaxParams));
    };

    serviceNamespace.submitOrder = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SubmitOrder',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.payOrder = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/PayOrder',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getGrouponOrders = function (type, dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetGrouponOrders?type=' + type,
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getOrders = function (type, dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetOrders?type=' + type,
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getRefundOrders = function (type, dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetRefundOrders?type=' + type,
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.checkMemberIsOwner = function (memberId, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/CheckMemberIsOwner?id=' + memberId,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.confirmReceived = function (id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/ConfirmReceived?id=' + id,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.cancelOrder = function (id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/CancelOrder?id=' + id,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.orderComment = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/OrderComment',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getProductComments = function (productId, dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetProductComments?productId=' + productId,
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.likeProduct = function (productId, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/LikeProduct?productId=' + productId,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.unLikeProduct = function (productId, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/UnLikeProduct?productId=' + productId,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };


    serviceNamespace.getMyLikeGrouponItems = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetMyLikeGrouponItems',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getEventCount = function (Type, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetEventCount?Type=' + Type,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getEventItem = function (Type, EType, EDType, PageIndex, PageSize, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetEventItem?Type=' + Type + "&EType=" + EType + "&EDType=" + EDType + "&PageIndex=" + PageIndex + "&PageSize=" + PageSize,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getEventInfo = function (EType, EDType, BillCode, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetEventInfo?EType=' + EType + "&EDType=" + EDType + "&BillCode=" + BillCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.joinActivity = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/JoinActivity',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getMyActivitys = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetMyActivitys',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };


    serviceNamespace.createEstateinfo = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/CreateEstateinfo',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.createEstateinfoComment = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/CreateEstateinfoComment',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.getEstateinfoMyInfo = function (memberId, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetEstateinfoMyInfo?memberId=' + memberId,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getMyEstateinfos = function (type, dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetMyEstateinfos?type=' + type,
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.setEstateinfoUnSale = function (id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SetEstateinfoUnSale?id=' + id,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.setEstateinfoSale = function (id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SetEstateinfoSale?id=' + id,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.createRentsaleinfo = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/CreateRentsaleinfo',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };


    serviceNamespace.getRentsaleinfoMyInfo = function (memberId, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetRentsaleinfoMyInfo?memberId=' + memberId,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getMyRentsaleinfos = function (memberId, type, dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetMyRentsaleinfos?memberId=' + memberId + "&type=" + type,
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.setRentsaleinfoUnSale = function (memberId, id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SetRentsaleinfoUnSale?memberId=' + memberId + "&id=" + id,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.setRentsaleinfoSale = function (memberId, id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SetRentsaleinfoSale?memberId=' + memberId + "&id=" + id,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };



    serviceNamespace.createRentsaleinfo = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/CreateRentsaleinfo',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };


    serviceNamespace.getRentsaleinfoMyInfo = function (ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetRentsaleinfoMyInfo',
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getMyRentsaleinfos = function (type, dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetMyRentsaleinfos?type=' + type,
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

    serviceNamespace.setRentsaleinfoUnSale = function (id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SetRentsaleinfoUnSale?id=' + id,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.setRentsaleinfoSale = function (id, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SetRentsaleinfoSale?id=' + id,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.createPayBill = function (FeedIds,PRoomCode, Amount, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/CreatePayBill?FeedIds=' + FeedIds + "&PRoomCode=" + PRoomCode + "&Amount=" + Amount,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.searchHousingManagement = function (pRoomCode, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/SearchHousingManagement?PRoomCode=' + pRoomCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getPRoomPrePayItem = function (pRoomCode, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetPRoomPrePayItem?PRoomCode=' + pRoomCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.createPayOrderPrePay = function (pRoomCode,PrePayItems,AmountSummary, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/CreatePayOrderPrePay?PRoomCode=' + pRoomCode + "&PrePayItems=" + PrePayItems + "&AmountSummary=" + AmountSummary,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getPProjectList = function (ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetPProjectList',
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getPBuildingList = function (pProjectCode,ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetPBuildingList?pProjectCode=' + pProjectCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getPUnitList = function (pBuildingCode, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetPUnitList?pProjectCode=&pBuildingCode=' + pBuildingCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getPFloorListByUnit = function (pProjectCode,pBuildingCode,PUnitCode,ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetPFloorListByUnit?pProjectCode=' + pProjectCode + '&pBuildingCode=' + pBuildingCode + '&PUnitCode=' + PUnitCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getPRoomListByFloor = function (pProjectCode, pBuildingCode, PUnitCode, pFloorName, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetPRoomListByFloor?pProjectCode=' + pProjectCode + '&pBuildingCode=' + pBuildingCode + '&PUnitCode=' + PUnitCode + "&pFloorName=" + pFloorName,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.addLinkBillMemberMSG = function (billcode, chartinfo, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/AddLinkBillMemberMSG?billcode=' + billcode + '&chartinfo=' + chartinfo,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.updateLinkBillEvaluation = function (billcode, evalLevel, suggest,ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/UpdateLinkBillEvaluation?billcode=' + billcode + '&evalLevel=' + evalLevel + '&suggest=' + suggest ,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.getFeeService = function (projectCode, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/GetFeeService?projectCode=' + projectCode,
            type: 'POST',
            data: JSON.stringify({})
        }, ajaxParams));
    };

    serviceNamespace.refundApply = function (dto, ajaxParams) {
        return app.ajax($.extend({
            url: app.appPath + 'api/services/app/WeixinService/RefundApply',
            type: 'POST',
            data: JSON.stringify(dto)
        }, ajaxParams));
    };

})();