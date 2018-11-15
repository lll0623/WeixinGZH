
(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [

		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

        new SearchPanel().init($('#searchpanel'), searchItems);


        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.product.deleteProductComment);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

    });

})();
