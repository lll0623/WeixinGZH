var tUploader = new ThumbnailUploader("uploadImage", "removeImage", "thumbnailImg", "thumbnail", "/Content/img/placehold/160x100.png", 160, 100);
var tUploader2 = new ThumbnailUploader("uploadImage2", "removeImage2", "thumbnailImg2", "thumbnail2", "/Content/img/placehold/160x100.png", 160, 100);


(function () {

    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'title', 'title': '标题', 'type': 'text', 'operator': 'like' },
                           { 'field': 'source', 'title': '来源', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有来源' }, { 'id': '0', 'text': '业主' }, { 'id': '1', 'text': '物业' }] },
                           { 'field': 'rentsaleinfoTypeId', 'title': '类型', 'type': 'list', 'operator': 'equal', 'listItems': eval("(" + $('#searchpanel').data('types') + ")") },
                           { 'field': 'isSale', 'title': '上架状态', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '下架' }, { 'id': '1', 'text': '上架' }] },
                           { 'field': 'isShow', 'title': '审核状态', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '隐藏' }, { 'id': '1', 'text': '显示' }] },
                           { 'field': 'contact', 'title': '联系人', 'type': 'text', 'operator': 'like' },
                           { 'field': 'mobile', 'title': '联系电话', 'type': 'text', 'operator': 'like' },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);
	
		var editorCreate;
		KindEditor.ready(function (K) { editorCreate = K.create('textarea[id="create_content"]', app.kingeditorConfig); });
		var editorUpdate;
		KindEditor.ready(function (K) { editorUpdate = K.create('textarea[id="update_content"]', app.kingeditorConfig); });

        //通用添加数据
		var commonCreate = new CommonCreateModel($('#RentsaleinfoCreateModal'), abp.services.app.rentsaleinfo.createRentsaleinfo);
		commonCreate.init();

		$(commonCreate).bind('modalHidden', function (obj) {
		    //初始化数据
		    editorCreate.html('');
		    tUploader.remove();
		    //$('#projects_create').val('');
		    //$('#projects_create').select2();
		});

		$(commonCreate).bind('preSubmit', function (obj, arg) {

		    //var projects = $('form[name="newsCreateForm"').find('select[name="projects"]').val();

		    //arg.data.projects = [];
		    //if (projects != null)
		    //    $.merge(arg.data.projects, projects);

		    //arg.data.allProjects = arg.data.allProjects === 'true';
		    //if (!arg.data.allProjects && arg.data.projects.length === 0) {
		    //    arg.cancel = true;
		    //    abp.message.warn('不区分小区，所属小区至少选择一样!');
		    //    return;
		    //}

		    arg.data.content = editorCreate.html();
		    if (arg.data.content === '') {
		        arg.cancel = true;
		        abp.message.warn('内容不能为空!');
		        return;
		    }

		});



        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.rentsaleinfo.deleteRentsaleinfo);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //显示
        $('a[data-role="show-row"]').click(function (e) {
            e.preventDefault();
            abp.services.app.rentsaleinfo.showRentsaleinfo($(this).data('id')).done(function (result) {
                window.location.reload();
            });
        });

        //隐藏
        $('a[data-role="hide-row"]').click(function (e) {
            e.preventDefault();
            abp.services.app.rentsaleinfo.hideRentsaleinfo($(this).data('id')).done(function (result) {
                window.location.reload();
            });
        });


        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#RentsaleinfoUpdateModal'), abp.services.app.rentsaleinfo.getRentsaleinfo, abp.services.app.rentsaleinfo.updateRentsaleinfo);
        commonUpdate.init();
        var viewer;
        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件
            editorUpdate.html(data.content);
            
            $('#contact').text(data.contact);
            $('#mobile').text(data.mobile);
            $('#creationTime').text(data.creationTime);

            //var images = "";
            //$(data.images).each(function (idx, obj) {
            //    images += "<img data-original='" + obj + "' src='" + obj + "?width=128&height=128&mode=crop' />";
            //});

            //$('#images').html(images);

            viewer = new Viewer(document.getElementById('RentsaleinfoUpdateModal'), {
                url: 'data-original'
            });

            $('#thumbnail2').val(data.thumbnail);
            if (data.thumbnail !== '') {
                $('#thumbnailImg2').attr('src', data.thumbnail + "?width=160&height=100&mode=crop");
                $('#uploadImage2').hide();
                $('#removeImage2').show();
            } else {
                tUploader2.remove();
            }

            $('#type_update').val(data.rentsaleinfoTypeId);
            $('#type_update').select2();


        });

        $(commonUpdate).bind('preSubmit', function (obj, arg) {
            arg.data.content = editorUpdate.html();
            if (arg.data.content === '') {
                arg.cancel = true;
                abp.message.warn('内容不能为空!');
                return;
            }
           

        });


        $(commonUpdate).bind('modalClose', function (obj, data) {
            if (viewer)
                viewer.destroy();
        });

        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });





	});

})();
