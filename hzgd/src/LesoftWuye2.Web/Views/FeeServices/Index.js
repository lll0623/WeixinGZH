
(function () {

    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'title', 'title': '标题', 'type': 'text', 'operator': 'like' },
                           { 'field': 'allProjects', 'title': '不区分小区', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '否' }, { 'id': '1', 'text': '是' }] },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

		var editorCreate;
		KindEditor.ready(function (K) {editorCreate = K.create('textarea[id="create_content"]', app.kingeditorConfig);});
		var editorUpdate;
		KindEditor.ready(function (K) {editorUpdate = K.create('textarea[id="update_content"]',app.kingeditorConfig);});

        //通用添加数据
		var commonCreate = new CommonCreateModel($('#FeeServiceCreateModal'), abp.services.app.feeService.createFeeService);
		commonCreate.init();

		$(commonCreate).bind('modalHidden', function (obj) {
		    //初始化数据
		    editorCreate.html('');
		    $('#projects_create').val('');
		    $('#projects_create').select2();
		});

		$(commonCreate).bind('preSubmit', function (obj, arg) {

		    var projects = $('form[name="feeServiceCreateForm"').find('select[name="projects"]').val();

		    arg.data.projects = [];
		    if (projects!=null)
		        $.merge(arg.data.projects,projects);

		    arg.data.allProjects = arg.data.allProjects === 'true';
		    if (!arg.data.allProjects && arg.data.projects.length === 0) {
		        arg.cancel = true;
		        abp.message.warn('不区分小区，所属小区至少选择一样!');
		        return;
		    }

		    arg.data.content = editorCreate.html();
		    if (arg.data.content === '') {
		        arg.cancel = true;
		        abp.message.warn('内容不能为空!');
		        return;
		    }
		    
		});

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.feeService.deleteFeeService);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#FeeServiceUpdateModal'), abp.services.app.feeService.getFeeService, abp.services.app.feeService.updateFeeService);
        commonUpdate.init();

        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件
            editorUpdate.html(data.content);
            $('#projects_update').val(data.projects);
            $('#projects_update').select2();

        });

        $(commonUpdate).bind('preSubmit', function (obj, arg) {

            var projects = $('form[name="feeServiceUpdateForm"').find('select[name="projects"]').val();

            arg.data.projects = [];
            if (projects != null)
                $.merge(arg.data.projects, projects);

            arg.data.allProjects = arg.data.allProjects === 'true';
            if (!arg.data.allProjects && arg.data.projects.length === 0) {
                arg.cancel = true;
                abp.message.warn('不区分小区，所属小区至少选择一样!');
                return;
            }

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
