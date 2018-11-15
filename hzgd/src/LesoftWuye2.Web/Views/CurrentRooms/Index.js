(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

        //通用添加数据
        new CommonCreateModel($('#CurrentRoomCreateModal'), abp.services.app.currentroom.createCurrentRoom).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.currentroom.deleteCurrentRoom);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#CurrentRoomUpdateModal'), abp.services.app.currentroom.getCurrentRoom, abp.services.app.currentroom.updateCurrentRoom);
        commonUpdate.init();
        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });

	});

})();
