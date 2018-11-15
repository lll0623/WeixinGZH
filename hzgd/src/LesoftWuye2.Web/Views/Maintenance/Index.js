(function () {
    $(function () {

        app.pageButtonPermissionProcess();

        //初始化查询条件
        var searchItems = [{ 'field': 'userName', 'title': '用户名', 'type': 'text', 'operator': 'like' },
                            { 'field': 'isActive', 'title': '激活状态', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '否' }, { 'id': '1', 'text': '是' }] },
                            { 'field': 'name', 'title': '姓名', 'type': 'text', 'operator': 'equal' },
                            { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

        new SearchPanel().init($('#searchpanel'), searchItems);

        $('#clearAllCaches').click(function (e) {
            
            abp.services.app.caching.clearAllCaches().done(function () {
                abp.notify.success("所有缓存已清除");
            }).always(function () {
                
            });
        });

        $('button[data-role="clearCahce"]').click(function (e) {

            var name = $(this).data('name');

            abp.services.app.caching.clearCache(name).done(function () {
                abp.notify.success(name+" 缓存已清除");
            }).always(function () {

            });
        });
        
        $('#downAllLog').click(function (e) {

            abp.services.app.webLog.downloadWebLogs().done(function (result) {
                app.downloadTempFile(result);
            }).always(function () {

            });
        });


        var fixWebLogsPanelHeight=function() {
            var windowHeight = $(window).height();
            var panelHeight = $('.full-height').height();
            var difference = windowHeight - panelHeight;
            var fixedHeight = panelHeight + difference;
            $('.full-height').css('height', (fixedHeight - 350) + 'px');
        }


        var refreshLog = function () {

            abp.services.app.webLog.getLatestWebLogs().done(function (result) {
                
                var resultHtml = '';
                $.each(result.latesWebLogLines, function (idx,logLine) {
                    resultHtml += '<span class="log-line">' + logLine
                    .replace('DEBUG', '<span class="label label-default">DEBUG</span>')
                    .replace('INFO', '<span class="label label-info">INFO</span>')
                    .replace('WARN', '<span class="label label-warning">WARN</span>')
                    .replace('ERROR', '<span class="label label-danger">ERROR</span>')
                    .replace('FATAL', '<span class="label label-danger">FATAL</span>') + '</span>';
                });
                $('#logs').html(resultHtml);
                fixWebLogsPanelHeight();
            }).always(function () {

            });

        };

        $('#refreshLog').click(function (e) {
            refreshLog();
        });

        refreshLog();

    });
})();