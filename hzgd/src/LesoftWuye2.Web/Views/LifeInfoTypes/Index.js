﻿var tUploader = new ThumbnailUploader("uploadImage", "removeImage", "thumbnailImg", "thumbnail", "/Content/img/placehold/128x128.png", 128, 128);
var tUploader2 = new ThumbnailUploader("uploadImage2", "removeImage2", "thumbnailImg2", "thumbnail2", "/Content/img/placehold/128x128.png", 128, 128);

(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [{ 'field': 'name', 'title': '名称', 'type': 'text', 'operator': 'like' },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

        //通用添加数据
        new CommonCreateModel($('#LifeInfoTypeCreateModal'), abp.services.app.lifeInfoType.createLifeInfoType).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.lifeInfoType.deleteLifeInfoType);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#LifeInfoTypeUpdateModal'), abp.services.app.lifeInfoType.getLifeInfoType, abp.services.app.lifeInfoType.updateLifeInfoType);
        commonUpdate.init();

        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件
            
            $('#thumbnail2').val(data.thumbnail);
            if (data.thumbnail !== '') {
                $('#thumbnailImg2').attr('src', data.thumbnail + "?width=128&height=128&mode=crop");
                $('#uploadImage2').hide();
                $('#removeImage2').show();
            } else {
                tUploader2.remove();
            }
        });


        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });

	});

})();
