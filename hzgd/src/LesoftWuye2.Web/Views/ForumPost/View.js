(function () {

    $(function () {

        //初始化查询条件
        var searchItems = [
                           { 'field': 'title', 'title': '标题', 'type': 'text', 'operator': 'like' },
		                   { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

        new SearchPanel().init($('#searchpanel'), searchItems);

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.forumPostService.deleteForumComment);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });


        //$('#images').viewer();

        var viewer = new Viewer(document.getElementById('posts'), {
            url: 'data-original'
        });

        $('#btnSetPostReply').click(function (e) {
            e.preventDefault();
            var postId = $(this).data('id');
            (new PromptHelper())
                .init('输入', '请输入回复内容')
                .onSuccess(function (text) {
                    abp.services.app.forumPostService.forumPostReply({ postId: postId, content: text }).done(function () {
                        window.location.reload();
                    });
                })
                .run();
        });

        $('a[data-role="reply-row"]').click(function (e) {
            e.preventDefault();
            var postCommentId = $(this).data('id');
            
            (new PromptHelper())
                .init('输入', '请输入回复内容')
                .onSuccess(function (text) {
                    abp.services.app.forumPostService.forumCommentReply({ postCommentId: postCommentId, content: text }).done(function () {
                        window.location.reload();
                    });
                })
                .run();
        });

    });

})();


