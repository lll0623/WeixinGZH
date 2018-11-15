var tUploader = new ThumbnailUploader("uploadImage", "removeImage", "thumbnailImg", "thumbnail", "/Content/img/placehold/640x300.png", 320, 150);
var tUploader2 = new ThumbnailUploader("uploadImage2", "removeImage2", "thumbnailImg2", "thumbnail2", "/Content/img/placehold/640x300.png", 320, 150);


(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'name', 'title': '名称', 'type': 'text', 'operator': 'like' },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

        //通用添加数据
		var commonCreate = new CommonCreateModel($('#WeixinSubscribeCreateModal'), abp.services.app.weixinSubscribe.createWeixinSubscribe);
		commonCreate.init();
		$(commonCreate).bind('preSubmit', function (obj, arg) {
		    if (arg.data.thumbnail === '') {
		        arg.cancel = true;
		        abp.message.warn('缩略图不能为空!');
		        return;
		    }

		});

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.weixinSubscribe.deleteWeixinSubscribe);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#WeixinSubscribeUpdateModal'), abp.services.app.weixinSubscribe.getWeixinSubscribe, abp.services.app.weixinSubscribe.updateWeixinSubscribe);
        commonUpdate.init();
        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件
            $('form[name="WeixinSubscribeUpdateForm"').find('textarea[name="summary"]').text(data.summary);
            $('#thumbnail2').val(data.thumbnail);
            if (data.thumbnail !== '') {
                $('#thumbnailImg2').attr('src', data.thumbnail + "?width=160&height=100&mode=crop");
                $('#uploadImage2').hide();
                $('#removeImage2').show();
            } else {
                tUploader2.remove();
            }            

        });


        $(commonUpdate).bind('preSubmit', function (obj, arg) {

            if (arg.data.thumbnail === '') {
                arg.cancel = true;
                abp.message.warn('缩略图不能为空!');
                return;
            }

        });


        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });

	});

})();
