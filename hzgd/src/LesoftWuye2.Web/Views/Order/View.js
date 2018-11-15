(function () {

    $(function () {

        
		$('#btnShip').click(function (e) {
		    e.preventDefault();
		    var orderId = $('#orderId').val();
		    abp.message.confirm(
               "确认订单已发货?",
               function (isConfirmed) {
                   if (isConfirmed) {
                       
                       abp.services.app.orderService.ship({ orderId: orderId }).done(function () {
                           location.reload(true);
                       }).always(function () {

                       });
                   }
               }
           );

		});

    });

})();
