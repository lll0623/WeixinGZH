(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [{ 'field': 'code', 'title': '小区编号', 'type': 'text', 'operator': 'like' },
                           { 'field': 'name', 'title': '小区名称', 'type': 'text', 'operator': 'like' },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

        //通用添加数据
        new CommonCreateModel($('#ProjectCreateModal'), abp.services.app.project.createProject).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.project.deleteProject);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#ProjectUpdateModal'), abp.services.app.project.getProject, abp.services.app.project.updateProject);
        commonUpdate.init();
        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });

        $('#syncProjects').click(function(e) {
            abp.services.app.project.syncWithWuyeApi().done(function () {
                abp.message.success('同步小区数据成功');
                //window.location.reload();
            }).always(function () {
                
            });
        });

    });

})();
