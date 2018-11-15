(function () {

    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'orderNo', 'title': '订单号', 'type': 'text', 'operator': 'equal' },
                           { 'field': 'type', 'title': '类型', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有类型' }, { 'id': '1', 'text': '仅退款' }, { 'id': '2', 'text': '退货' }] },
                           { 'field': 'status', 'title': '退款状态', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '1', 'text': '退款申请中' }, { 'id': '2', 'text': '退款申请通过，退款中' }, { 'id': '3', 'text': '退款申请未通过' }, { 'id': '4', 'text': '退款完成' }] },
                           { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);
      
	});

})();
