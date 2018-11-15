var tUploader = new ThumbnailUploader("uploadImage", "removeImage", "thumbnailImg", "thumbnail", "/Content/img/placehold/640x300.png", 160, 75);
var tUploader2 = new ThumbnailUploader("uploadImage2", "removeImage2", "thumbnailImg2", "thumbnail2", "/Content/img/placehold/640x300.png", 160, 75);



(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'name', 'title': '名称', 'type': 'text', 'operator': 'like' },
                           { 'field': 'isSale', 'title': '销售状态', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '下架' }, { 'id': '1', 'text': '上架' }] },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

		var editorCreate;
		KindEditor.ready(function (K) { editorCreate = K.create('textarea[id="create_content"]', app.kingeditorConfig); });
		var editorUpdate;
		KindEditor.ready(function (K) { editorUpdate = K.create('textarea[id="update_content"]', app.kingeditorConfig); });

        //通用添加数据
		var commonCreate= new CommonCreateModel($('#ProductCreateModal'), abp.services.app.product.createProduct);
		commonCreate.init();

        $(commonCreate).bind('modalHidden', function (obj) {
            //初始化数据
            editorCreate.html('');
            tUploader.remove();
            $('#tags_create').val('');
            $('#tags_create').select2();
        });

        $(commonCreate).bind('preSubmit', function (obj, arg) {

            var tags = $('#tags_create').val();

            arg.data.tags = [];
            if (tags != null)
                $.merge(arg.data.tags, tags);

           

            arg.data.content = editorCreate.html();
            if (arg.data.content === '') {
                arg.cancel = true;
                abp.message.warn('商品详情不能为空!');
                return;
            }

        });


        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.product.deleteProduct);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#ProductUpdateModal'), abp.services.app.product.getProduct, abp.services.app.product.updateProduct);
        commonUpdate.init();


        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件
            editorUpdate.html(data.content);
            $('#thumbnail2').val(data.thumbnail);
            if (data.thumbnail !== '') {
                $('#thumbnailImg2').attr('src', data.thumbnail + "?width=160&height=75&mode=crop");
                $('#uploadImage2').hide();
                $('#removeImage2').show();
            } else {
                tUploader2.remove();
            }

            $('#tags_update').val(data.tags);
            $('#tags_update').select2();

            $('form[name="productUpdateForm"').find('textarea[name="content"]').html(data.content);
            $('form[name="productUpdateForm"').find('select[name="supplierId"]').val(data.supplierId);
            $('form[name="productUpdateForm"').find('select[name="categoryId"]').val(data.categoryId);

        });

        $(commonUpdate).bind('preSubmit', function (obj, arg) {

            var tags = $('#tags_update').val();

            arg.data.tags = [];
            if (tags != null)
                $.merge(arg.data.tags, tags);



            arg.data.content = editorUpdate.html();
            if (arg.data.content === '') {
                arg.cancel = true;
                abp.message.warn('商品详情不能为空!');
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
