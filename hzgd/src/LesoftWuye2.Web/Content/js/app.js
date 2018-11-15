var app = app || {};
(function ($) {

    app.changeUrlArg = function (url, arg, argVal) {
        var pattern = arg + '=([^&]*)';
        var replaceText = arg + '=' + argVal;
        if (url.match(pattern)) {
            var tmp = '/(' + arg + '=)([^&]*)/gi';
            tmp = url.replace(eval(tmp), replaceText);
            return tmp;
        } else {
            if (url.match('[\?]')) {
                return url + '&' + replaceText;
            } else {
                return url + '?' + replaceText;
            }
        }
    };

    app.downloadTempFile = function (file) {
        location.href = abp.appPath + 'File/DownloadTempFile?fileType=' + file.fileType + '&fileToken=' + file.fileToken + '&fileName=' + file.fileName;
    };

    //serializeFormToObject plugin for jQuery
    $.fn.serializeFormToObject = function () {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        return obj;
    };

    //Configure blockUI
    if ($.blockUI) {
        $.blockUI.defaults.baseZ = 2000;
    }

    /*修改密码*/
    var initChangePassword = function () {
        var $modal = $('#ChangePasswordModal');
        var $form = $modal.find('form');

        $form.attr('data-bv-message', '请输入正确的值');

        $form.bootstrapValidator({
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                NewPassword: {
                    validators: {
                        identical: {
                            field: 'NewPassword2',
                            message: '2次输入的新密码必须一致'
                        }
                    }
                },
                NewPassword2: {
                    validators: {
                        identical: {
                            field: 'NewPassword',
                            message: '2次输入的新密码必须一致'
                        }
                    }
                }
            }
        });

        $form.find('button[type="submit"]').click(function (e) {
            e.preventDefault();

            $form.data('bootstrapValidator').validate();

            if (!$form.data('bootstrapValidator').isValid()) {
                return;
            }

            var passwordInfo = $form.serializeFormToObject();
            abp.ui.setBusy($modal);
            abp.services.app.user.changeMyPassword(passwordInfo).done(function () {
                $modal.modal('hide');
                abp.notify.success('修改密码成功');
            }).always(function () {
                $form.find('button[type="submit"]').removeAttr("disabled");
                abp.ui.clearBusy($modal);
            });
        });

        $modal.on('shown.bs.modal', function () {
            $modal.find('input:not([type=hidden]):first').focus();
        });

        $modal.on('hidden.bs.modal', function () {
            $form[0].reset();
            $form.data('bootstrapValidator').resetForm();
        });

    };

    /*页面按钮权限通一处理*/
    app.pageButtonPermissionProcess = function () {
        var $buttons = $('*[data-role-name]');
        console.log("Permission buttons:" + $buttons.length);
        $($buttons).each(function (i, e) {

            if (!abp.auth.isGranted($(e).data('role-name'))) {
                $(e).attr('disabled', "true");
            }
        });
    };

    app.formAutoBootstrapValidator = function (form) {

        //遍历表单,设置验证信息
        var $form = $(form);

        $form.attr('data-bv-message', '请输入正确的值');
        $form.attr('data-bv-feedbackicons-valid', 'glyphicon glyphicon-ok');
        $form.attr('data-bv-feedbackicons-invalid', 'glyphicon glyphicon-remove');
        $form.attr('data-bv-feedbackicons-validating', 'glyphicon glyphicon-refresh');


        $form.find('input[name]').each(function () {
            //排除hidden,disabled
            var type = $(this).attr('type');
            if (type === 'hidden' || typeof ($(this).attr("disabled")) !== "undefined") {
                return;
            }

            var name = this.name;
            var label = $form.find('label[label-for="' + name + '"]');
            var labelName = "";
            if (label != null)
                labelName = $(label).text().replace('*', '');

            if (typeof ($(this).attr("required")) !== "undefined") {
                $(this).attr('data-bv-notempty', 'true');
                $(this).attr('data-bv-notempty-message', labelName + '不能为空');
            }

            if (type === "email") {
                $(this).attr('data-bv-emailaddress-message', '请输入正确的Email地址');
            };
            if (type === "url") {
                $(this).attr('data-bv-uri-message', '请输入正确的链接地址，必须以http:// 或者 https:// 开头');
            };
        });

        $form.bootstrapValidator();
    };


    app.kingeditorConfig = {
        uploadJson: '/File/KingeditorUploadImage',
        urlType: 'absolute',
        items: [
            'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
            'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
            'insertunorderedlist', '|', 'image', '|', 'emoticons', 'link', 'source', 'fullscreen'
        ]
    };

    var initControls = function () {
        $('.select2').select2();
        $('.date-picker').datepicker();
    };





    initControls();
    initChangePassword();

})(jQuery);


//上传图片
var ThumbnailUploader = function (fileid, removeid, img, thumbnailid, placehold, width, height) {

    this.ajaxupload = function () {

        //判断是否有选择上传文件  
        var imgPath = $("#" + fileid).val();
        if (imgPath === "") {
            abp.message.warn("请选择上传图片！");
            return;
        }

        //判断上传文件的后缀名  
        var strExtension = imgPath.substr(imgPath.lastIndexOf('.') + 1).toLowerCase();
        if (strExtension !== 'jpg' && strExtension !== 'gif' && strExtension !== 'png' && strExtension !== 'bmp') {
            abp.message.warn("请选择上传图片！");
            return;
        }

        $('#' + fileid).ajaxfileupload({
            action: "/File/UploadImage",
            valid_extensions: ['jpg', 'gif', 'png', 'bmp'],
            params: {
                extra: 'info'
            },
            onComplete: function (response) {
                //console.log('custom handler for file:');
                if (response.success) {
                    var imageUrl = "/Upload/Images/" + response.result.fileName;
                    $('#' + removeid).show();
                    $('#' + fileid).hide();
                    $('#' + thumbnailid).val(imageUrl);
                    $('#' + img).attr('src', imageUrl + "?width=" + width + "&height=" + height + "&mode=crop");

                    //abp.notify.success("图片上传成功!");
                } else {
                    abp.message.warn(response.error.message, "图片上传失败!");
                }
            },
            onStart: function () {

            },
            onCancel: function () {
                //console.log('no file selected');
            }
        });
    };

    this.remove = function () {
        $('#' + removeid).hide();
        $('#' + fileid).show();
        $('#' + thumbnailid).val('');
        $('#' + img).attr('src', placehold);
    };
};

//多图上传插件
var MultiImageUploader = function (dom, width, height) {

    this.init = function() {
        
    };

    this.setImages = function (images) { };

    this.getImages = function () { };
}

//搜索面板
var SearchPanel = function () {

    var prex = "<div class='portlet search-panel' style='border: 0px; margin-bottom: 0px; box-shadow: none; padding-left: 10px; padding-top: 10px;padding-bottom: 5px'><div class='portlet-body' style='margin-top: 0px'>";
    var end = "</div></div>";
    this.maxlines = 3;
    this.searchItems = null;
    this.autoSearch = true;
    this.lastsearch = null;//上次搜索的条件

    var createFielInputTemp = function (searchItem, row) {
        switch (searchItem.type) {
            case "text":
                var placeholder = "请输入 `" + searchItem.title + "` 进行模糊查询";
                if (searchItem.operator === "like")
                    placeholder = "请输入 `" + searchItem.title + "` 进行模糊查询";
                if (searchItem.operator === "equal")
                    placeholder = "请输入 `" + searchItem.title + "`进行精确查询";
                return "<div class='col-md-6 search-value'  data-role='search-input'>" +
                       "<input type='text' class='form-control' data-role='search-value' data-row='" + row + "' data-field='" + searchItem.field + "' data-operator='" + searchItem.field + "' placeholder='" + placeholder + "'></div>";
            case "list":
                var line = "";
                var prex1 = "<div class='col-md-6 search-value' data-role='search-input'>" +
                    "<select class='bs-select form-control' data-role='search-value' data-row='" + row + "' data-field='" + searchItem.field + "' data-operator='" + searchItem.field + "'>";
                var end1 = "</select></div>";
                line += prex1;
                for (var i = 0; i < searchItem.listItems.length; i++) {
                    line += "<option value='" + searchItem.listItems[i].id + "'>" + searchItem.listItems[i].text + "</option>";
                }
                line += end1;
                return line;
            case "date":
                return "<div class='col-md-3 search-value' data-role='search-input'>" +
                    "<input type='text' class='form-control date-picker' data-value='search-min-value' data-date-format='yyyy-mm-dd' placeholder='日期起始日期(YYYY-MM-DD)' data-role='search-value' data-row='" + row + "' data-field='" + searchItem.field + "' data-operator='" + searchItem.field + "'></div>" +
                    "<div class='col-md-3 search-value'  data-role='search-input'>" +
                    "<input type='text' class='form-control date-picker' data-value='search-max-value'  data-date-format='yyyy-mm-dd'  placeholder='日期截止日期(YYYY-MM-DD)' data-role='search-value' data-row='" + row + "' data-field='" + searchItem.field + "' data-operator='" + searchItem.field + "'></div>";
            default:
                break;
        }
        return "";
    };

    var expand = function (obj, dom) {

        var $btn = $(obj);
        var $rows = $(dom).find('div[class="row search-row"]:gt(0)');

        if ($btn.data('status')) {
            $btn.data('status', false);
            $btn.find('i').removeClass().addClass('fa fa-angle-down');
            $rows.hide();
        } else {
            $btn.data('status', true);
            $btn.find('i').removeClass().addClass('fa fa-angle-up');
            $rows.show();
        }
    };

    var genSearchPara = function (searchItem, logic, value, maxvalue, minvalue) {
        //拼接格式 field:ignore,type,logic,operator,value, maxvalue,minvalue
        var para = "";
        para += searchItem.field + ":";

        para += searchItem.ignore ? "1" : "0";
        para += logic === 'OR' ? ",1" : ",0";

        switch (searchItem.type) {
            case "text":
                para += ",0";
                break;
            case "list":
                para += ",1";
                break;
            case "date":
                para += ",2";
                break;
            default:
                para += ",";
                break;
        }

        switch (searchItem.operator) {
            case "like":
                para += ",0";
                break;
            case "equal":
                para += ",1";
                break;
            case "range":
                para += ",2";
                break;
            default:
                para += ",";
                break;
        }
        if (value !== '') para += "," + value;
        if (maxvalue !== '') para += "," + maxvalue;
        if (minvalue !== '') para += "," + minvalue;

        console.log("para:" + para);
        return para;
    }

    var search = function (obj, dom, panel) {
        var $this = panel;
        var $dom = $(dom);

        //遍历所有输入控件,有值得就拼接
        //一行行遍历
        var searchPara = "";
        for (var i = 0; i < $this.maxlines; i++) {
            var field = $dom.find('select[id=search-field-' + i + ']').val();
            var $row = $(dom).find('div[id="search-row-' + i + '"]');
            var searchItem = null;
            $.each($this.searchItems, function (n, value) {
                if (value.field === field) {
                    searchItem = value;
                    return;
                }
            });
            if (searchItem == null) continue;

            var logic = $row.find('select[id="search-logic-' + i + '"]').val();

            switch (searchItem.type) {
                case "text":
                    var val = $row.find('input[data-role="search-value"]').val();
                    if (val === '') continue;
                    if (searchPara.length > 0) searchPara += "|";
                    searchPara += genSearchPara(searchItem, logic, val, '', '');
                    break;
                case "list":
                    var val = $row.find('select[data-role="search-value"]').val();
                    if (val === '') continue;
                    if (searchPara.length > 0) searchPara += "|";
                    searchPara += genSearchPara(searchItem, logic, val, '', '');
                    break;

                case "date":
                    var minValue = $row.find('input[data-value="search-min-value"]').val();
                    var maxValue = $row.find('input[data-value="search-max-value"]').val();
                    if (minValue === '' && maxValue === '') continue;
                    if (searchPara.length > 0) searchPara += "|";
                    searchPara += genSearchPara(searchItem, logic, '', minValue, maxValue);
                    break;
                default:
                    break;
            }
        }

        console.log("searchPara:" + searchPara);
        if ($this.autoSearch) {
            //if (searchPara === "") {
            //    window.location.reload();
            //    return;
            //}
            var reloadurl = app.changeUrlArg(window.location.href, "q", searchPara);
            reloadurl = app.changeUrlArg(reloadurl, "p", "1");

            window.location.href = reloadurl;
        } else {
            console.log("not implemented.");
        }
    }

    var changeField = function (obj, dom, panel) {
        var $select = $(obj);
        var index = $select.data('index');
        var field = $select.val();
        var $this = panel;
        var $thisrow = $(dom).find('div[id="search-row-' + index + '"]');
        var $searchInputs = $thisrow.find('div[data-role="search-input"]');


        console.log("changeField... row:" + index + ",field:" + field);

        //更换对应的字段控件
        var searchItem = null;
        $.each($this.searchItems, function (n, value) {
            if (value.field === field) {
                searchItem = value;
                return;
            }
        });
        if (searchItem == null) return;

        console.log("changeField... row:" + index + ",field:" + field + " title:" + searchItem.title);

        var fieldInputTemp = createFielInputTemp(searchItem, index);

        //删除当前行,字段选择控件后面的所有元素
        $searchInputs.remove();
        $select.parent().after(fieldInputTemp);
        $('.date-picker').datepicker();
    }

    var processLastSearch = function (dom, panel) {
        var $this = panel;
        var $dom = $(dom);
        if ($this.lastsearch === '') return;

        console.log("processLastSearch");
        var lines = $this.lastsearch.split('|');
        var validCount = 0;//有效的条件行数
        for (var i = 0; i < lines.length; i++) {
            var line = lines[i];
            var items = line.split(':');
            if (items.length !== 2) continue;

            var field = items[0];
            var searchItem = null;
            $.each($this.searchItems, function (n, value) {
                if (value.field === field) {
                    searchItem = value;
                    return;
                }
            });
            if (searchItem == null) continue;;

            var kvs = items[1].split(',');
            if (searchItem.type === 'date' && kvs.length !== 6) continue;
            if (searchItem.type !== 'date' && kvs.length !== 5) continue;

            //只取逻辑符,值,最小值,最大值,其他内容按配置来
            var logic = kvs[1] === "1" ? "OR" : "AND";
            var value = '', minvalue = '', maxvalue = '';
            if (searchItem.type === 'date') {
                minvalue = kvs[4];
                maxvalue = kvs[5];
            } else {
                value = kvs[4];
            }

            if (i > 0)
                $dom.find('select[id="search-logic-' + i + '"]').val(logic);

            var fieldInputTemp = createFielInputTemp(searchItem, i);

            //删除当前行,字段选择控件后面的所有元素
            var $thisrow = $(dom).find('div[id="search-row-' + i + '"]');
            var $select = $thisrow.find('select[data-role="search-field"]');
            $select.val(field);

            var $searchInputs = $thisrow.find('div[data-role="search-input"]');
            $searchInputs.remove();
            $select.parent().after(fieldInputTemp);
            $('.date-picker').datepicker();

            //查找新控件,赋值
            switch (searchItem.type) {
                case "text":
                    $thisrow.find('input[data-role="search-value"]').val(value);
                    break;
                case "list":
                    $thisrow.find('select[data-role="search-value"]').val(value);
                    break;
                case "date":
                    $thisrow.find('input[data-value="search-min-value"]').val(minvalue);
                    $thisrow.find('input[data-value="search-max-value"]').val(maxvalue);
                    break;
            }
            validCount++;
        }

        if (validCount > 1) {
            expand($dom.find('button[data-role="more"]'), $dom);
        }
    };

    this.init = function (dom, searchitems) {
        var $dom = $(dom);
        this.searchItems = searchitems;
        this.autoSearch = $dom.data('autosearch') || true;
        this.lastsearch = $dom.data('lastsearch') || '';

        //根据搜索条件构建搜索框ui
        var html = "";
        html += prex;
        this.maxlines = $dom.data('lines');
        if (!this.maxlines)
            this.maxlines = 3;

        console.log("max lines:" + this.maxlines);

        for (var i = 0; i < this.maxlines; i++) {
            var line = "";
            line += "<div id='search-row-" + i + "' class='row search-row' style='" + (i === 0 ? "" : "display:none") + "'>";

            //字段名
            if (i === 0) {
                line += "<div class='col-md-3 search-field' style='padding-right: 0px;padding-left: 15px!important;'><select class='bs-select form-control' data-role='search-field' data-index='" + i + "' id='search-field-" + i + "'>";
                for (var j = 0; j < this.searchItems.length; j++) {
                    line += "<option value='" + this.searchItems[j].field + "'>" + this.searchItems[j].title + "</option>";
                }
                line += "</select></div>";
            } else {
                line += "<div class='col-md-1 search-logic'><select class='bs-select form-control' id='search-logic-" + i + "'><option value='AND'>并且</option><option value='OR'>或者</option></select></div>";
                line += "<div class='col-md-2 search-field' style='padding-right: 0px;padding-left: 15px!important;'><select class='bs-select form-control' data-role='search-field' data-index='" + i + "' id='search-field-" + i + "'>";
                for (var j = 0; j < this.searchItems.length; j++) {
                    line += "<option value='" + this.searchItems[j].field + "'>" + this.searchItems[j].title + "</option>";
                }
                line += "</select></div>";
            }

            //创建字段对应控件
            line += createFielInputTemp(this.searchItems[0], i);
            if (i === 0) {
                line += "<div class='col-md-3'><div class='clearfix'><button type='button' class='btn btn-default' data-role='search' ><i class='fa fa-search'></i>查询</button>&nbsp;&nbsp;<button type='button' data-role='more' class='btn btn-default' >更多&nbsp;<i class='fa fa-angle-down'></i></button></div></div>";
            }
            line += "</div>";
            html += line;
        }

        html += end;
        $dom.html(html);

        var thisPanel = this;

        //绑定事件
        $dom.find('button[data-role="more"]').click(function (e) {
            expand(this, $dom);
        });

        $dom.find('button[data-role="search"]').click(function (e) {
            search(this, $dom, thisPanel);
        });

        $dom.find('select[data-role="search-field"]').change(function (e) {
            changeField(this, $dom, thisPanel);
        });
        $('.date-picker').datepicker();
        processLastSearch($dom, thisPanel);
        return this;
    };

    this.getSearchJson = function () {

        var lines = new Array();

        return lines;
    }
};


//通用新增数据页面(弹出)
var CommonCreateModel = function (dom, service) {
    this.init = function () {
        var $modal = $(dom);
        var $form = $modal.find('form');

        $form.attr('onsubmit', 'return false;');//阻止表单自动提交

        app.formAutoBootstrapValidator($form);
        var thisObj = this;
        $form.find('button[type="submit"]').click(function (e) {
            e.preventDefault();

            $form.data('bootstrapValidator').validate();

            if (!$form.data('bootstrapValidator').isValid()) {
                return;
            }

            var data = $form.serializeFormToObject();

            //接入手动验证
            $form.find('button[type="submit"]').removeAttr("disabled");
            var preSubmitArg = { cancel: false, data: data };
            $(thisObj).trigger("preSubmit", preSubmitArg);
            if (preSubmitArg.cancel) {
                //取消提交
                console.log("preSubmit cancel");
                return;
            }

            abp.ui.setBusy($modal);
            service(data).done(function () {
                $modal.modal('hide');
                location.reload(true);
            }).always(function () {
                $form.find('button[type="submit"]').removeAttr("disabled");
                abp.ui.clearBusy($modal);
            });
        });

        $modal.on('shown.bs.modal', function () {
            $modal.find('input:not([type=hidden]):first').focus();
            $(thisObj).trigger("modalShown");
        });

        $modal.on('hidden.bs.modal', function () {
            $form[0].reset();
            $form.data('bootstrapValidator').resetForm();
            $(thisObj).trigger("modalHidden");
        });

    };
};


//通用删除数据(单条)
var CommonDeleteData = function (service) {

    this.delete = function (dataid, msg) {
        var delmsg = msg || '数据';
        abp.message.confirm(
                "确认删除" + delmsg + "吗?",
                function (isConfirmed) {
                    if (isConfirmed) {
                        console.log("delete data id:" + dataid);
                        service(dataid).done(function () {
                            location.reload(true);
                        }).always(function () {

                        });
                    }
                }
            );
    }
}

//通用修改数据页面(弹出)
var CommonUpdateModel = function (dom, getService, updateService) {

    var $modal;
    var $form;

    this.init = function () {

        $modal = $(dom);
        $form = $modal.find('form');
        $form.attr('onsubmit', 'return false;');//阻止表单自动提交

        app.formAutoBootstrapValidator($form);
        var thisObj = this;
        $form.find('button[type="submit"]').click(function (e) {
            e.preventDefault();

            $form.data('bootstrapValidator').validate();

            if (!$form.data('bootstrapValidator').isValid()) {
                return false;
            }

            var user = $form.serializeFormToObject();

            //接入手动验证
            var preSubmitArg = { cancel: false, data: user };
            $(thisObj).trigger("preSubmit", preSubmitArg);
            if (preSubmitArg.cancel) {
                //取消提交
                //$form.find('button[type="submit"]').unbind("click");
                console.log("preSubmit cancel");
                return false;
            }
            console.log("begin submit.");
            abp.ui.setBusy($modal);
            updateService(user).done(function () {
                $modal.modal('hide');
                location.reload(true);
            }).always(function () {
                $form.find('button[type="submit"]').removeAttr("disabled");
                abp.ui.clearBusy($modal);
            });
        });

        $modal.on('shown.bs.modal', function () {
            $modal.find('input:not([type=hidden]):first').focus();
        });

        $modal.on('hidden.bs.modal', function () {
            $form[0].reset();
            $form.data('bootstrapValidator').resetForm();
            $(thisObj).trigger("modalClose", this);
        });
    };

    this.getdata = function (dataid) {
        //手动弹出页面
        $modal.modal('show');
        abp.ui.setBusy($modal);
        var thisObj = this;
        getService(dataid).done(function (result) {
            //修改页面赋值
            $.each(result, function (name, ival) {
                console.log("name:" + name);
                var $oinput = $modal.find("input[name='" + name + "']");
                if ($oinput.attr("type") === "radio" || $oinput.attr("type") === "checkbox") {
                    $oinput.each(function () {
                        if (Object.prototype.toString.apply(ival) === '[object Array]') {//是复选框，并且是数组  
                            for (var i = 0; i < ival.length; i++) {
                                if ($(this).val() === ival[i])
                                    $(this).attr("checked", "checked");
                            }
                        } else {
                            if ($oinput.attr("type") === "checkbox") {
                                if (ival) {
                                    $(this).attr("checked", "checked");
                                } else {
                                    $(this).removeAttr("checked");
                                }
                            }
                            //radio暂时不支持
                        }
                    });
                } else if ($oinput.attr("type") === "textarea") {//多行文本框  
                    $modal.find("textarea[name='" + name + "']").html(ival);
                } else {
                    $modal.find("input[name='" + name + "']").val(ival);
                }
            });
            //接入手动赋值
            $(thisObj).trigger("setData", result);
        }).always(function () {
            abp.ui.clearBusy($modal);
        });
    };
}



/**
 * 输入框工具
 * @constructor
 */
var PromptHelper = function () {
    var obj = {};

    obj._title = "输入";
    obj._text = "请输入内容";
    obj._inputPlaceholder = "请输入内容";
    obj._emptyAlert = "输入的内容不能为空";
    obj._successCallback = null;

    obj.__proto__ = PromptHelper.prototype;
    return obj;
};

PromptHelper.prototype = {
    init: function (title, text) {

        this._title = title || this._title;
        this._text = text || this._text;

        return this;
    },
    setMoreInfo: function (inputPlaceholder, emptyAlert) {

        this._inputPlaceholder = inputPlaceholder || this._inputPlaceholder;
        this._emptyAlert = emptyAlert || this._emptyAlert;

        return this;
    },
    onSuccess: function (callback) {
        this._successCallback = callback;
        return this;
    },
    run: function () {

        var me = this;

        swal({
            title: me._title,
            text: me._text,
            type: "input",
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            showCancelButton: true,
            closeOnConfirm: false,
            // animation: "slide-from-top",
            inputPlaceholder: me._inputPlaceholder
        },
            function (inputValue) {
                if (inputValue === false) return false;

                if (inputValue === "") {
                    swal.showInputError(me._emptyAlert);
                    return false;
                }

                if (me._successCallback != undefined && (typeof me._successCallback === "function")) {
                    me._successCallback(inputValue);
                }
                swal.close();
            });

    }
};