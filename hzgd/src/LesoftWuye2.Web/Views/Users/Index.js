(function () {
    $(function () {

        app.pageButtonPermissionProcess();

        //初始化查询条件
        var searchItems = [{ 'field': 'userName', 'title': '用户名', 'type': 'text', 'operator': 'like' },
                            { 'field': 'isActive', 'title': '激活状态', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '否' }, { 'id': '1', 'text': '是' }] },
                            { 'field': 'name', 'title': '姓名', 'type': 'text', 'operator': 'equal' },
                            { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

        new SearchPanel().init($('#searchpanel'), searchItems);

        //通用添加数据
        new CommonCreateModel($('#UserCreateModal'), abp.services.app.user.createUser).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.user.deleteUser);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#UserUpdateModal'), abp.services.app.user.getUser, abp.services.app.user.updateUser);
        commonUpdate.init();
        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });

        //设置角色
        $('a[data-role="setrole-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            var $modal = $('#UserSetRoleModal');
            var $form = $modal.find('form');
            var $roles = $($modal).find('div[data-role="roles"]');

            $modal.modal('show');
            abp.ui.setBusy($modal);
            abp.services.app.user.getRoles(dataid).done(function (result) {
                var html = "";
                $(result.allRoles).each(function (index, element) {
                    var isSelected = result.userRoles.indexOf(element.roleId) !== -1;

                    html += "<div class='checkbox'><label><input type='checkbox' "+(isSelected?"checked='checked'":"")+" name='role' value='"+element.roleId+"'><span class='text'>" + element.roleName + "</span></label></div>";

                });
                $roles.html(html);

            }).always(function () {
                abp.ui.clearBusy($modal);
            });

            function getSelectedRoles() {
                
                var selectRoleIds = new Array();
                

                $($roles.find('input[name="role"]:checked')).each(function(i,e) {
                    selectRoleIds.push($(e).val());
                });
                console.log("selectRoleIds:" + selectRoleIds);

                return selectRoleIds;
            };

            $form.find('button[type="submit"]').click(function (e) {
                e.preventDefault();

                var selectedRoles = getSelectedRoles();
                abp.ui.setBusy($modal);
                abp.services.app.user.saveRoles(dataid, selectedRoles).done(function () {
                    $modal.modal('hide');
                    window.location.reload();
                }).always(function () {
                    $form.find('button[type="submit"]').removeAttr("disabled");
                    abp.ui.clearBusy($modal);
                });
            });

        });
    });
})();