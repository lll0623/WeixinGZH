(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

        //通用添加数据
        new CommonCreateModel($('#GrouponCreateModal'), abp.services.app.groupon.createGroupon).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.groupon.deleteGroupon);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#GrouponUpdateModal'), abp.services.app.groupon.getGroupon, abp.services.app.groupon.updateGroupon);
        commonUpdate.init();

        $(commonUpdate).bind('setData', function (obj, data) {
            
            $('form[name="grouponUpdateForm"').find('select[name="productId"]').val(data.productId);
            $('form[name="grouponUpdateForm"').find('textarea[name="summary"]').text(data.summary);
             
        });

        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });


        $('#ExpireTime_New').datetimepicker({ format: 'yyyy-mm-dd hh:ii', autoclose: true });
        $('#ExpireTime_Edit').datetimepicker({ format: 'yyyy-mm-dd hh:ii', autoclose: true });

        $('#StartTime_New').datetimepicker({ format: 'yyyy-mm-dd hh:ii', autoclose: true });
        $('#StartTime_Edit').datetimepicker({ format: 'yyyy-mm-dd hh:ii', autoclose: true });
	});

})();
