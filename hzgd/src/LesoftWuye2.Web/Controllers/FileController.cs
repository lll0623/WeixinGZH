using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Json;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Utils;
using Obs;
using Obs.Dto;
using Obs.IO;

namespace LesoftWuye2.Web.Controllers
{
    public class FileController : LesoftWuye2ControllerBase
    {
        private readonly IAppFolders _appFolders;
        private readonly WeiXinApi _weiXinApi;

        public FileController(IAppFolders appFolders,
            WeiXinApi weiXinApi)
        {
            _appFolders = appFolders;
            _weiXinApi = weiXinApi;
        }

        [AbpMvcAuthorize]
        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            CheckModelState();

            var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, file.FileToken);
            if (!System.IO.File.Exists(filePath))
            {
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return File(fileBytes, file.FileType, file.FileName);
        }

        public JsonResult UploadImage()
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException("没有找到上传的图片");
                }

                var file = Request.Files[0];

                if (file.ContentLength > 5242880 * 10) //1MB.
                {
                    throw new UserFriendlyException("图片大小超出限制(不能超过10M)");
                }

                //Check file type & format
                var fileImage = Image.FromStream(file.InputStream);
                if (!fileImage.RawFormat.Equals(ImageFormat.Jpeg) &&
                    !fileImage.RawFormat.Equals(ImageFormat.Png) &&
                    !fileImage.RawFormat.Equals(ImageFormat.Gif) &&
                    !fileImage.RawFormat.Equals(ImageFormat.Bmp))
                {
                    throw new UserFriendlyException("不允许的图片格式(支持格式 Jpeg,Png,Gif,Bmp)");
                }
                

                //Save new picture
                var fileInfo = new FileInfo(file.FileName);
                var tempFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolders.UploadImageFolder, tempFileName);
                file.SaveAs(tempFilePath);

                //
                string host = this.Request.Url.Scheme+""+"://" +this.Request.Url.Authority.Replace(":80","");

                using (var bmpImage = new Bitmap(tempFilePath))
                {
                    return Json(new AjaxResponse(new { host= host, fileName = tempFileName, width = bmpImage.Width, height = bmpImage.Height }));
                }
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        public JsonResult UploadImageFromWeixinJssdk(string serverId)
        {
            try
            {
                

                //Save new picture
              
                var tempFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                var tempFilePath = Path.Combine(_appFolders.UploadImageFolder, tempFileName);
                _weiXinApi.DownImageFromWeixinServer(serverId,tempFilePath);
                string host = this.Request.Url.Scheme + "" + "://" + this.Request.Url.Authority.Replace(":80", "");

                using (var bmpImage = new Bitmap(tempFilePath))
                {
                    return Json(new AjaxResponse(new { host = host, fileName = tempFileName, width = bmpImage.Width, height = bmpImage.Height }));
                }
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        public ContentResult KingeditorUploadImage()
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    //throw new UserFriendlyException("没有找到上传的图片");
                    //return Json(new AjaxResponse(new { error = 1, message = "没有找到上传的图片" }));
                    var result = new KingEditorUploadImageResult { error = 1, message = "没有找到上传的图片" };
                    return Content(JsonSerializationHelper.SerializeWithType(result), "text/html");
                }

                var file = Request.Files[0];

                if (file.ContentLength > 5242880 * 10) //1MB.
                {
                    //throw new UserFriendlyException("图片大小超出限制(不能超过10M)");
                    //return Json(new AjaxResponse(new { error = 1, message = "图片大小超出限制(不能超过10M)" }));
                    var result = new KingEditorUploadImageResult { error = 1, message = "图片大小超出限制(不能超过10M)" };
                    return Content(JsonSerializationHelper.SerializeWithType(result), "text/html");
                }

                //Check file type & format
                var fileImage = Image.FromStream(file.InputStream);
                if (!fileImage.RawFormat.Equals(ImageFormat.Jpeg) &&
                    !fileImage.RawFormat.Equals(ImageFormat.Png) &&
                    !fileImage.RawFormat.Equals(ImageFormat.Gif) &&
                    !fileImage.RawFormat.Equals(ImageFormat.Bmp))
                {
                    //throw new UserFriendlyException("不允许的图片格式(支持格式 Jpeg,Png,Gif,Bmp)");
                    //return Json(new AjaxResponse(new { error = 1, message = "不允许的图片格式(支持格式 Jpeg,Png,Gif,Bmp)" }));
                    var result = new KingEditorUploadImageResult { error = 1, message = "不允许的图片格式(支持格式 Jpeg,Png,Gif,Bmp)" };
                    return Content(JsonSerializationHelper.SerializeWithType(result), "text/html");
                }

                //Delete old temp profile pictures
                //AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.UploadImageFolder, "userProfileImage_" + AbpSession.GetUserId());

                //Save new picture
                var fileInfo = new FileInfo(file.FileName);
                var tempFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolders.UploadImageFolder, tempFileName);
                file.SaveAs(tempFilePath);

                using (var bmpImage = new Bitmap(tempFilePath))
                {
                    var result = new KingEditorUploadImageResult {url = $"/Upload/Images/{tempFileName}?width=640"};
                    return Content(JsonSerializationHelper.SerializeWithType(result), "text/html");
                }
            }
            catch (UserFriendlyException ex)
            {
                //return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
                var result = new KingEditorUploadImageResult {error = 1,message = ex.Message };
                return Content(JsonSerializationHelper.SerializeWithType(result), "text/html");
            }
        }
    }

    public class KingEditorUploadImageResult
    {
        public int error { get; set; }
        public string url { get; set; }
        public string message { get; set; }
    }
}