var tUploader = new ThumbnailUploader("uploadImage", "removeImage", "thumbnailImg", "thumbnail", "/Content/img/placehold/160x100.png", 160, 100);
var tUploader2 = new ThumbnailUploader("uploadImage2", "removeImage2", "thumbnailImg2", "thumbnail2", "/Content/img/placehold/160x100.png", 160, 100);

(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'name', 'title': '名称', 'type': 'text', 'operator': 'like' },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

        //通用添加数据
        new CommonCreateModel($('#SubstationCreateModal'), abp.services.app.substation.createSubstation).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.substation.deleteSubstation);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#SubstationUpdateModal'), abp.services.app.substation.getSubstation, abp.services.app.substation.updateSubstation);
        commonUpdate.init();
        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件
            
            $('#thumbnail2').val(data.thumbnail);
            if (data.thumbnail !== '') {
                $('#thumbnailImg2').attr('src', data.thumbnail + "?width=160&height=100&mode=crop");
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
