var tUploader = new ThumbnailUploader("uploadImage", "removeImage", "thumbnailImg", "thumbnail", "/Content/img/placehold/160x100.png", 160, 100);
var tUploader2 = new ThumbnailUploader("uploadImage2", "removeImage2", "thumbnailImg2", "thumbnail2", "/Content/img/placehold/160x100.png", 160, 100);


(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'title', 'title': '名称', 'type': 'text', 'operator': 'like' },
                           { 'field': 'propertyTypeId', 'title': '类型', 'type': 'list', 'operator': 'equal', 'listItems': eval("(" + $('#searchpanel').data('types') + ")") },
                           { 'field': 'propertyCityId', 'title': '城市', 'type': 'list', 'operator': 'equal', 'listItems': eval("(" + $('#searchpanel').data('citys') + ")") },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

        new SearchPanel().init($('#searchpanel'), searchItems);

        var editorCreate;
        KindEditor.ready(function (K) { editorCreate = K.create('textarea[id="create_content"]', app.kingeditorConfig); });
        var editorUpdate;
        KindEditor.ready(function (K) { editorUpdate = K.create('textarea[id="update_content"]', app.kingeditorConfig); });



        //通用添加数据
        var commonCreate= new CommonCreateModel($('#PropertyCreateModal'), abp.services.app.property.createProperty);

        commonCreate.init();
        $(commonCreate).bind('modalHidden', function (obj) {
            //初始化数据
            editorCreate.html('');
            tUploader.remove();
        });

        $(commonCreate).bind('preSubmit', function (obj, arg) {
            
            arg.data.content = editorCreate.html();
            if (arg.data.content === '') {
                arg.cancel = true;
                abp.message.warn('内容不能为空!');
                return;
            }

        });


        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.property.deleteProperty);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#PropertyUpdateModal'), abp.services.app.property.getProperty, abp.services.app.property.updateProperty);
        commonUpdate.init();

        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件
            editorUpdate.html(data.content);
            $('#thumbnail2').val(data.thumbnail);
            if (data.thumbnail !== '') {
                $('#thumbnailImg2').attr('src', data.thumbnail + "?width=160&height=100&mode=crop");
                $('#uploadImage2').hide();
                $('#removeImage2').show();
            } else {
                tUploader2.remove();
            }

            
            $('form[name="propertyUpdateForm"').find('select[name="propertyTypeId"]').val(data.propertyTypeId);
            $('form[name="propertyUpdateForm"').find('select[name="propertyCityId"]').val(data.propertyCityId);

        });

        $(commonUpdate).bind('preSubmit', function (obj, arg) {

            arg.data.content = editorUpdate.html();
            if (arg.data.content === '') {
                arg.cancel = true;
                abp.message.warn('内容不能为空!');
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
