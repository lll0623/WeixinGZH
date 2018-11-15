var tUploader = new ThumbnailUploader("uploadImage", "removeImage", "thumbnailImg", "thumbnail", "/Content/img/placehold/640x300.png", 320, 150);
var tUploader2 = new ThumbnailUploader("uploadImage2", "removeImage2", "thumbnailImg2", "thumbnail2", "/Content/img/placehold/640x300.png", 320, 150);


(function () {
    $(function () {

        
        //通用添加数据
        new CommonCreateModel($('#MallSlideImageCreateModal'), abp.services.app.mallSlideImage.createMallSlideImage).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.mallSlideImage.deleteMallSlideImage);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#MallSlideImageUpdateModal'), abp.services.app.mallSlideImage.getMallSlideImage, abp.services.app.mallSlideImage.updateMallSlideImage);
        commonUpdate.init();
        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件

            $('#thumbnail2').val(data.thumbnail);
            if (data.thumbnail !== '') {
                $('#thumbnailImg2').attr('src', data.thumbnail + "?width=320&height=150&mode=crop");
                $('#uploadImage2').hide();
                $('#removeImage2').show();
            } else {
                tUploader2.remove();
            }
        });

        $('a[data-role="update-row"]').click(function (e) {
            e.preventDefault();
            var dataid = $(this).data('id');
            commonUpdate.getdata(dataid);
        });

	});

})();
