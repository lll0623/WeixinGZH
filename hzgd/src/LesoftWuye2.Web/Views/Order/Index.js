(function () {

    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'orderNo', 'title': '订单号', 'type': 'text', 'operator': 'equal' },
                           { 'field': 'type', 'title': '类型', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有类型' }, { 'id': '0', 'text': '拼团' }, { 'id': '1', 'text': '直接购买' }] },
                           { 'field': 'contact', 'title': '收货人', 'type': 'text', 'operator': 'equal' },
                           { 'field': 'mobile', 'title': '联系手机号码', 'type': 'text', 'operator': 'equal' },
                           { 'field': 'status', 'title': '订单状态', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '待支付' }, { 'id': '1', 'text': '团购中' }, { 'id': '2', 'text': '待发货' }, { 'id': '3', 'text': '已发货' }, { 'id': '4', 'text': '待评价' }, { 'id': '5', 'text': '已评价' }, { 'id': '6', 'text': '已取消' }] },
                           { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

		new SearchPanel().init($('#searchpanel'), searchItems);
      
	});

})();
