(function () {

    $(function () {

        var viewer = new Viewer(document.getElementById('images'), {
            url: 'data-original'
        });
        
        $('#reject').click(function (e) {
		    e.preventDefault();
		    var orderId = $('#orderId').val();
		    abp.message.confirm(
               "确认拒绝退款申请?",
               function (isConfirmed) {
                   if (isConfirmed) {
                       abp.services.app.refundOrderService.reject({ orderId: orderId }).done(function () {
                           location.reload(true);
                       }).always(function () {

                       });
                   }
               }
           );

		});

        $('#accept').click(function (e) {
            e.preventDefault();
            var orderId = $('#orderId').val();
            abp.message.confirm(
               "确认同意退款申请?",
               function (isConfirmed) {
                   if (isConfirmed) {
                       abp.services.app.refundOrderService.accept({ orderId: orderId }).done(function () {
                           location.reload(true);
                       }).always(function () {

                       });
                   }
               }
           );

        });

    });

})();
