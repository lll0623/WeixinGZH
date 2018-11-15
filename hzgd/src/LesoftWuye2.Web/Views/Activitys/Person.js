
(function () {
    $(function () {
        

        //初始化查询条件
        var searchItems = [
                    { 'field': 'contact', 'title': '姓名', 'type': 'text', 'operator': 'like' },
                    { 'field': 'mobile', 'title': '手机号码', 'type': 'text', 'operator': 'like' },
                    { 'field': 'creationTime', 'title': '报名时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);

		

	});

})();
