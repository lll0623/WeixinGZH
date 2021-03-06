﻿(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'name', 'title': '名称', 'type': 'text', 'operator': 'like' },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

        //通用添加数据
        new CommonCreateModel($('#SupplierCreateModal'), abp.services.app.supplier.createSupplier).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.supplier.deleteSupplier);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#SupplierUpdateModal'), abp.services.app.supplier.getSupplier, abp.services.app.supplier.updateSupplier);
        commonUpdate.init();
        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });

	});

})();
