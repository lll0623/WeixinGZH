(function () {

    $(function () {
        $('#LoginButton').click(function (e) {
            e.preventDefault();
            if ($('#EmailAddressInput').val() === '') {
                $('#EmailAddressInput').focus();
                abp.message.warn('请输入用户名!');
                return;
            }

            if ($('#PasswordInput').val() === '') {
                $('#PasswordInput').focus();
                abp.message.warn('请输入密码!');
                return;
            }

            abp.ui.setBusy(
                $('#LoginArea'),
                abp.ajax({
                    url: abp.appPath + 'Account/Login',
                    type: 'POST',
                    data: JSON.stringify({
                        tenancyName: $('#TenancyName').val(),
                        usernameOrEmailAddress: $('#EmailAddressInput').val(),
                        password: $('#PasswordInput').val(),
                        rememberMe: $('#RememberMeInput').is(':checked'),
                        returnUrlHash: $('#ReturnUrlHash').val()
                    })
                })
            );
        });

        $('#ReturnUrlHash').val(location.hash);
        $('#EmailAddressInput').focus();
    });

})();