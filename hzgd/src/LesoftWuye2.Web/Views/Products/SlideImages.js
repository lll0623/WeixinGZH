var tUploader = new ThumbnailUploader("uploadImage", "removeImage", "thumbnailImg", "thumbnail", "/Content/img/placehold/640x400.png", 320, 200);
var tUploader2 = new ThumbnailUploader("uploadImage2", "removeImage2", "thumbnailImg2", "thumbnail2", "/Content/img/placehold/640x400.png", 320, 200);


(function () {
    $(function () {

        
        //通用添加数据
        new CommonCreateModel($('#SlideImageCreateModal'), abp.services.app.productSlideImage.createSlideImage).init();

        //通用删除数据
        var commdel = new CommonDeleteData(abp.services.app.productSlideImage.deleteSlideImage);
        $('a[data-role="delete-row"]').click(function (e) {
            e.preventDefault();
            commdel.delete($(this).data('id'), $(this).data('delete-msg'));
        });

        //通用修改(先获取单条数据再修改)
        var commonUpdate = new CommonUpdateModel($('#SlideImageUpdateModal'), abp.services.app.productSlideImage.getSlideImage, abp.services.app.productSlideImage.updateSlideImage);
        commonUpdate.init();
        $(commonUpdate).bind('setData', function (obj, data) {
            //数据赋值到控件

            $('#thumbnail2').val(data.thumbnail);
            if (data.thumbnail !== '') {
                $('#thumbnailImg2').attr('src', data.thumbnail + "?width=320&height=200&mode=crop");
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
