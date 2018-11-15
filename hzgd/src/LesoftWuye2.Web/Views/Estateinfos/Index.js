(function () {

    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'title', 'title': '标题', 'type': 'text', 'operator': 'like' },
                           { 'field': 'estateinfoTypeId', 'title': '类型', 'type': 'list', 'operator': 'equal', 'listItems': eval("(" + $('#searchpanel').data('types') + ")") },
                           { 'field': 'isSale', 'title': '上架状态', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '下架' }, { 'id': '1', 'text': '上架' }] },
                           { 'field': 'isShow', 'title': '审核状态', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '隐藏' }, { 'id': '1', 'text': '显示' }] },
                           { 'field': 'contact', 'title': '联系人', 'type': 'text', 'operator': 'like' },
                           { 'field': 'mobile', 'title': '联系电话', 'type': 'text', 'operator': 'like' },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);
	

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.estateinfo.deleteEstateinfo);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //显示
        $('a[data-role="show-row"]').click(function (e) {
            e.preventDefault();
            abp.services.app.estateinfo.showEstateinfo($(this).data('id')).done(function (result) {
                window.location.reload();
            });
        });

        //隐藏
        $('a[data-role="hide-row"]').click(function (e) {
            e.preventDefault();
            abp.services.app.estateinfo.hideEstateinfo($(this).data('id')).done(function (result) {
                window.location.reload();
            });
        });


        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#EstateinfoUpdateModal'), abp.services.app.estateinfo.getEstateinfo, abp.services.app.estateinfo.updateEstateinfo);
        commonUpdate.init();
        var viewer;
        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件
            $('#update_content').html(data.content);
            $('#contact').text(data.contact);
            $('#mobile').text(data.mobile);
            $('#creationTime').text(data.creationTime);

            var images = "";
            $(data.images).each(function(idx, obj) {
                images += "<img data-original='" + obj + "' src='" + obj + "?width=128&height=128&mode=crop' />";
            });
            
            $('#images').html(images);

            viewer = new Viewer(document.getElementById('EstateinfoUpdateModal'), {
                url: 'data-original'
            });
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
