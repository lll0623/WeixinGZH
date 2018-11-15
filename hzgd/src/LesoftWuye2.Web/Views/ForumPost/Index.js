(function () {

    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'title', 'title': '标题', 'type': 'text', 'operator': 'like' },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);
      
        //通用删除数据
		var commdel = new CommonDeleteData(abp.services.app.forumPostService.deleteForumPost);
		$('a[data-role="delete-row"]').click(function (e) {
		    e.preventDefault();
		    commdel.delete($(this).data('id'), $(this).data('delete-msg'));
		});

	});

})();
