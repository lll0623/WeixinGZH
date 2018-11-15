(function () {
    $(function () {

        //初始化查询条件
        var searchItems = [{ 'field': 'name', 'title': '角色名称', 'type': 'text', 'operator': 'like' },
                            { 'field': 'isDefault', 'title': '默认', 'type': 'list', 'operator': 'equal', 'listItems': [{ 'id': '', 'text': '所有状态' }, { 'id': '0', 'text': '否' }, { 'id': '1', 'text': '是' }] },
                            { 'field': 'creationTime', 'title': '创建时间', 'type': 'date', 'operator': 'range' }];

        new SearchPanel().init($('#searchpanel'), searchItems);

        //通用添加数据
        new CommonCreateModel($('#RoleCreateModal'), abp.services.app.role.createRole).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.role.deleteRole);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#RoleUpdateModal'), abp.services.app.role.getRole, abp.services.app.role.updateRole);
        commonUpdate.init();
        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });

        //授权
        $('a[data-role="granted-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            var $modal = $('#RolePermissionModal');
            var $form = $modal.find('form');
            var $tree = $($modal).find('.permission-tree');
            
            console.log("granted-row click");

            var inTreeChangeEvent = false;
            $tree.jstree('destroy');
            $tree.on("changed.jstree", function (e, data) {
                if (!data.node) {
                    return;
                }

                var wasInTreeChangeEvent = inTreeChangeEvent;
                if (!wasInTreeChangeEvent) {
                    inTreeChangeEvent = true;
                }

                var childrenNodes;

                if (data.node.state.selected) {
                    selectNodeAndAllParents($tree.jstree('get_parent', data.node));

                    childrenNodes = $.makeArray($tree.jstree('get_children_dom', data.node));
                    $tree.jstree('select_node', childrenNodes);

                } else {
                    childrenNodes = $.makeArray($tree.jstree('get_children_dom', data.node));
                    $tree.jstree('deselect_node', childrenNodes);
                }
                
            });

            function selectNodeAndAllParents(node) {
                $tree.jstree('select_node', node, true);
                var parent = $tree.jstree('get_parent', node);
                if (parent) {
                    selectNodeAndAllParents(parent);
                }
            };
            
            $modal.modal('show');
            abp.ui.setBusy($modal);
            abp.services.app.role.getGrantedPermissions(dataid).done(function (result) {

                var treeData = $.map(result.permissions, function (item) {
                    return {
                        id: item.name,
                        parent: item.parentName ? item.parentName : '#',
                        text: item.displayName,
                        state: {
                            opened: false,
                            selected: result.grantedPermissionNames.indexOf(item.name) !== -1 //$.contains(result.grantedPermissionNames, item.name)
                        }
                    };
                });

                $tree.jstree({
                    'core': {
                        data: treeData
                    },
                    "types": {
                        "default": {
                            "icon": "fa fa-folder tree-item-icon-color icon-lg"
                        },
                        "file": {
                            "icon": "fa fa-file tree-item-icon-color icon-lg"
                        }
                    },
                    'checkbox': {
                        keep_selected_style: false,
                        three_state: false,
                        cascade: ''
                    },
                    plugins: ['checkbox', 'types']
                });



            }).always(function () {
                abp.ui.clearBusy($modal);
            });

            function getSelectedPermissionNames() {
                var permissionNames = [];

                var selectedPermissions = $tree.jstree('get_selected', true);
                for (var i = 0; i < selectedPermissions.length; i++) {
                    permissionNames.push(selectedPermissions[i].original.id);
                }

                return permissionNames;
            };

            $form.find('button[type="submit"]').click(function (e) {
                e.preventDefault();

                var selectedPermissionNames = getSelectedPermissionNames();
                abp.ui.setBusy($modal);
                abp.services.app.role.saveGrantedPermissions(dataid, selectedPermissionNames).done(function () {
                    $modal.modal('hide');
                    abp.notify.success('修改角色权限成功');
                }).always(function () {
                    $form.find('button[type="submit"]').removeAttr("disabled");
                    abp.ui.clearBusy($modal);
                });
            });

            $modal.on('hidden.bs.modal', function () {
                $form.find('button[type="submit"]').unbind("click");
            });
        });


    });

   


})();
