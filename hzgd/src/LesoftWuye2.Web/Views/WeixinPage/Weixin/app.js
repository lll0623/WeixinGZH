var app = app || {};
(function ($) {

    app.appPath = app.appPath || '/';


    app.utils = app.utils || {};

    /* Creates a name namespace.
    *  Example:
    *  var taskService = app.utils.createNamespace(app, 'services.task');
    *  taskService will be equal to app.services.task
    *  first argument (root) must be defined first
    ************************************************************/
    app.utils.createNamespace = function (root, ns) {
        var parts = ns.split('.');
        for (var i = 0; i < parts.length; i++) {
            if (typeof root[parts[i]] == 'undefined') {
                root[parts[i]] = {};
            }

            root = root[parts[i]];
        }

        return root;
    };


    app.reloadPage = function () {
        window.location.href = app.updateUrl(window.location.href);
        //alert(window.location.href);
    };

    app.gotoPage = function (url) {
        window.location.href = app.updateUrl(url);
        //alert(window.location.href);
    }; 

    app.updateUrl = function (url, key) {
        var key = (key || 't') + '='; //默认是“t”  
        var reg = new RegExp(key + '\\d+');//正则：t=1472286066028  
        var timestamp = +new Date();
        if (url.indexOf(key) > -1)//有时间戳，直接更新  
        {
            return url.replace(reg, key + timestamp);
        }
        else //没有时间戳，加上时间戳  
        {
            if (url.indexOf('\?') > -1) {
                var urlArr = url.split('\?');
                if (urlArr[1]) {
                    return urlArr[0] + '?' + key + timestamp + '&' + urlArr[1];
                }
                else {
                    return urlArr[0] + '?' + key + timestamp;
                }
            }
            else {
                if (url.indexOf('#') > -1) {
                    return url.split('#')[0] + '?' + key + timestamp + location.hash;
                }
                else {
                    return url + '?' + key + timestamp;
                }
            }
        }
    }


    /**
     * Sets a cookie value for given key.
     * @param {string} key
     * @param {string} value 
     * @param {Date} expireDate Optional expire date (default: 30 days).
     */
    app.utils.setCookieValue = function (key, value, expireDate) {
        if (!expireDate) {
            expireDate = new Date();
            expireDate.setDate(expireDate.getDate() + 30);
        }

        document.cookie = encodeURIComponent(key) + '=' + encodeURIComponent(value) + "; expires=" + expireDate.toUTCString() + ";path=/";;
    };

    /**
     * Gets a cookie with given key.
     * @param {string} key
     * @returns {string} Cookie value
     */
    app.utils.getCookieValue = function (key) {
        var equalities = document.cookie.split('; ');
        for (var i = 0; i < equalities.length; i++) {
            if (!equalities[i]) {
                continue;
            }

            var splitted = equalities[i].split('=');
            if (splitted.length != 2) {
                continue;
            }

            if (decodeURIComponent(splitted[0]) === key) {
                return decodeURIComponent(splitted[1] || '');
            }
        }

        return null;
    };

    //    使用canvas对大图片进行压缩
    app.utils.compress=function(img) {
        //    用于压缩图片的canvas
        var canvas = document.createElement("canvas");
        var ctx = canvas.getContext('2d');
        //    瓦片canvas
        var tCanvas = document.createElement("canvas");
        var tctx = tCanvas.getContext("2d");

        var initSize = img.src.length;
        var width = img.width;
        var height = img.height;
        //如果图片大于四百万像素，计算压缩比并将大小压至400万以下
        var ratio;
        if ((ratio = width * height / 4000000) > 1) {
            ratio = Math.sqrt(ratio);
            width /= ratio;
            height /= ratio;
        } else {
            ratio = 1;
        }
        canvas.width = width;
        canvas.height = height;
        //        铺底色
        ctx.fillStyle = "#fff";
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        //如果图片像素大于100万则使用瓦片绘制
        var count;
        if ((count = width * height / 1000000) > 1) {
            count = ~~(Math.sqrt(count) + 1); //计算要分成多少块瓦片
            //            计算每块瓦片的宽和高
            var nw = ~~(width / count);
            var nh = ~~(height / count);
            tCanvas.width = nw;
            tCanvas.height = nh;
            for (var i = 0; i < count; i++) {
                for (var j = 0; j < count; j++) {
                    tctx.drawImage(img, i * nw * ratio, j * nh * ratio, nw * ratio, nh * ratio, 0, 0, nw, nh);
                    ctx.drawImage(tCanvas, i * nw, j * nh, nw, nh);
                }
            }
        } else {
            ctx.drawImage(img, 0, 0, width, height);
        }
        //进行最小压缩
        var ndata = canvas.toDataURL('image/jpeg', 0.1);
        console.log('压缩前：' + initSize);
        console.log('压缩后：' + ndata.length);
        console.log('压缩率：' + ~~(100 * (initSize - ndata.length) / initSize) + "%");
        tCanvas.width = tCanvas.height = canvas.width = canvas.height = 0;
        return ndata;
    }

    /* UI *******************************************************/

    app.ui = app.ui || {};


    app.ui.setButtonBusy = function (elm, optionsOrPromise) {
        $(elm).text('提交中...');
        $(elm).attr("disabled", "disabled");
    };

    app.ui.clearButtonBusy = function (elm) {
        $(elm).text('提交');
        $(elm).removeAttr("disabled");
    };



    app.libs = app.libs || {};
    app.libs.sweetAlert = {
        config: {
            'default': {

            },
            info: {
                type: 'info'
            },
            success: {
                type: 'success'
            },
            warn: {
                type: 'warning'
            },
            error: {
                type: 'error'
            },
            confirm: {
                type: 'warning',
                title: 'Are you sure?',
                showCancelButton: true,
                cancelButtonText: '取消',
                confirmButtonColor: "#DD6B55",
                confirmButtonText: '确定'
            }
        }
    };


    app.message = app.message || {};

    var showMessage = function (type, message, title, callback) {
        if (!title) {
            title = message;
            message = undefined;
        }

        var opts = $.extend(
            {},
            app.libs.sweetAlert.config.default,
            app.libs.sweetAlert.config[type],
            {
                title: title,
                text: message
            }
        );

        return $.Deferred(function ($dfd) {
            //sweetAlert(opts, function () {
            //    $dfd.resolve();
            //});
            sweetAlert(opts, function (e) {
                callback && callback(e);
                $dfd.resolve(e);
            });
        });
    };

    app.message.info = function (message, title, callback) {
        return showMessage('info', message, title, callback);
    };

    app.message.success = function (message, title, callback) {
        return showMessage('success', message, title, callback);
    };

    app.message.warn = function (message, title, callback) {
        return showMessage('warn', message, title, callback);
    };

    
    app.message.error = function (message, title, callback) {
        return showMessage('error', message, title, callback);
    };

    app.message.confirm = function (message, titleOrCallback, callback) {
        var userOpts = {
            text: message
        };

        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            userOpts.title = titleOrCallback;
        };

        var opts = $.extend(
            {},
            app.libs.sweetAlert.config.default,
            app.libs.sweetAlert.config.confirm,
            userOpts
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts, function (isConfirmed) {
                callback && callback(isConfirmed);
                $dfd.resolve(isConfirmed);
            });
        });
    };

    app.message.confirm1 = function (message, titleOrCallback,confirmButtonText,cancelButtonText,callback) {
        var userOpts = {
            text: message
        };

        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            userOpts.title = titleOrCallback;
        };

        var opts = $.extend(
            {},
            app.libs.sweetAlert.config.default,
            {
                type: 'warning',
                confirmButtonColor: "#DD6B55",
                showCancelButton: true,
                cancelButtonText: cancelButtonText,
                confirmButtonText: confirmButtonText
            },
            userOpts
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts, function (isConfirmed) {
                callback && callback(isConfirmed);
                $dfd.resolve(isConfirmed);
            });
        });
    };

    app.ajax = function (userOptions) {
        userOptions = userOptions || {};

        var options = $.extend({}, app.ajax.defaultOpts, userOptions);
        options.success = undefined;
        options.error = undefined;
        options.beforeSend=function(request) {
            var token = app.utils.getCookieValue('_token_');
            request.setRequestHeader("_token_", token == null ? '' : token);
        };

        return $.Deferred(function ($dfd) {
            $.ajax(options)
                .done(function (data, textStatus, jqXHR) {
                    if (data.__abp) {
                        app.ajax.handleResponse(data, userOptions, $dfd, jqXHR);
                    } else {
                        $dfd.resolve(data);
                        userOptions.success && userOptions.success(data);
                    }
                }).fail(function (jqXHR) {
                    if (jqXHR.responseJSON && jqXHR.responseJSON.__abp) {
                        app.ajax.handleResponse(jqXHR.responseJSON, userOptions, $dfd, jqXHR);
                    } else {
                        app.ajax.handleNonAbpErrorResponse(jqXHR, userOptions, $dfd);
                    }
                });
        });
    };

    $.extend(app.ajax, {
        defaultOpts: {
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json'
        },

        defaultError: {
            message: '发生了错误!',
            details: '服务器未发送错误详细信息.'
        },

        defaultError401: {
            message: '您没有通过验证!',
            details: '您应该验证（登录）为了执行此操作.'
        },

        defaultError403: {
            message: '您没有权限!',
            details: '您不能执行此操作.'
        },

        defaultError404: {
            message: '找不到资源!',
            details: '在服务器上找不到请求的资源.'
        },

        showError: function (error) {
            if (error.details) {
                return app.message.error(error.details, error.message);
            } else {
                return app.message.error(error.message || app.ajax.defaultError.message);
            }
        },

        handleTargetUrl: function (targetUrl) {
            if (!targetUrl) {
                location.href = app.appPath;
            } else {
                location.href = targetUrl;
            }
        },

        handleNonAbpErrorResponse: function (jqXHR, userOptions, $dfd) {
            if (userOptions.appHandleError !== false) {
                switch (jqXHR.status) {
                    case 401:
                        app.ajax.handleUnAuthorizedRequest(
                            app.ajax.showError(app.ajax.defaultError401),
                            app.appPath
                        );
                        break;
                    case 403:
                        app.ajax.showError(app.ajax.defaultError403);
                        break;
                    case 404:
                        app.ajax.showError(app.ajax.defaultError404);
                        break;
                    default:
                        app.ajax.showError(app.ajax.defaultError);
                        break;
                }
            }

            $dfd.reject.apply(this, arguments);
            userOptions.error && userOptions.error.apply(this, arguments);
        },



        handleResponse: function (data, userOptions, $dfd, jqXHR) {
            if (data) {
                if (data.success === true) {
                    $dfd && $dfd.resolve(data.result, data, jqXHR);
                    userOptions.success && userOptions.success(data.result, data, jqXHR);

                    if (data.targetUrl) {
                        app.ajax.handleTargetUrl(data.targetUrl);
                    }
                } else if (data.success === false) {
                    var messagePromise = null;

                    if (data.error) {
                        if (userOptions.appHandleError !== false) {
                            messagePromise = app.ajax.showError(data.error);
                        }
                    } else {
                        data.error = app.ajax.defaultError;
                    }

                    $dfd && $dfd.reject(data.error, jqXHR);
                    userOptions.error && userOptions.error(data.error, jqXHR);

                    if (jqXHR.status === 401 && userOptions.appHandleError !== false) {
                        app.ajax.handleUnAuthorizedRequest(messagePromise, data.targetUrl);
                    }
                } else { //not wrapped result
                    $dfd && $dfd.resolve(data, null, jqXHR);
                    userOptions.success && userOptions.success(data, null, jqXHR);
                }
            } else { //no data sent to back
                $dfd && $dfd.resolve(jqXHR);
                userOptions.success && userOptions.success(jqXHR);
            }
        }

    });



})(jQuery);
