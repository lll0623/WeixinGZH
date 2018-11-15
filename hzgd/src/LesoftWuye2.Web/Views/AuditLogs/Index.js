(function () {
    $(function () {

        app.pageButtonPermissionProcess();

        //初始化查询条件
        var searchItems = [{ 'field': 'serviceName', 'title': '服务名称', 'type': 'text', 'operator': 'like' },
                           { 'field': 'methodName', 'title': '方法名称', 'type': 'text', 'operator': 'like' },
                           { 'field': 'userName', 'title': '用户名', 'type': 'text', 'operator': 'like', ignore: true },
                           { 'field': 'clientIpAddress', 'title': '客户端Ip', 'type': 'text', 'operator': 'like' },
                           { 'field': 'BrowserInfo', 'title': '浏览器', 'type': 'text', 'operator': 'like' },
                           { 'field': 'executionTime', 'title': '执行时间', 'type': 'date', 'operator': 'range' }];

        var searchPanel = new SearchPanel().init($('#searchpanel'), searchItems);

        $('#exportToExcel').click(function (e) {
            abp.services.app.auditLog.getAuditLogsToExcel({ Filter: searchPanel.getSearchJson() }).done(function (result) {
                app.downloadTempFile(result);
            }).always(function () {

            });
        });

    });
})();