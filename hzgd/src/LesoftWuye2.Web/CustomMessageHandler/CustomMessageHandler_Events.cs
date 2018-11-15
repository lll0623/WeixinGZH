/*----------------------------------------------------------------
    Copyright (C) 2017 Senparc
    
    文件名：CustomMessageHandler_Events.cs
    文件功能描述：自定义MessageHandler
    
    
    创建标识：Senparc - 20150312
----------------------------------------------------------------*/

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using LesoftWuye2.Application.WeixinSubscribes;
using Obs.Dto;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.MP.Entities;

namespace LesoftWuye2.Web.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        public IWeixinSubscribeAppService WeixinSubscribeAppService { get; set; }
        public string Host { get; set; }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
            if (WeixinSubscribeAppService != null)
            {
               var data= WeixinSubscribeAppService.GetWeixinSubscribes(new GetPageListRequstDto() {PageSize = 10,CurrentPage = 1}).Result.Items;
                foreach (var listItem in data)
                {
                    responseMessage.Articles.Add(new Article()
                    {
                        Description = listItem.Summary,
                        PicUrl =Host+listItem.Thumbnail,
                        Title = listItem.Title,
                        Url = listItem.Url
                    });
                }
            }

            return responseMessage;
        }

       

        

    

      
        

   

       
    }
}