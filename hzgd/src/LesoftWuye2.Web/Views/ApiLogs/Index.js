
(function () {

    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'name', 'title': '接口名称', 'type': 'text', 'operator': 'like' },
                           { 'field': 'success', 'title': '结果', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有结果' }, { 'id': '0', 'text': '失败' }, { 'id': '1', 'text': '成功' }] },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);


        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#ApiLogUpdateModal'), abp.services.app.apiLog.getApiLog, null);
        commonUpdate.init();

        $(commonUpdate).bind('setData', function (obj, data) {
            $('#update_content').html(data.response);
        });

   


        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });

        $('#clearall').click(function (e) {
            abp.services.app.apiLog.clearAll().done(function (result) {
                window.location.reload();
            }).always(function () {

            });
        });

	});

})();
