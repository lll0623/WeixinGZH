﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Events.Bus;

namespace LesoftWuye2.Application.Weixin.Events.EventDatas
{
   public class GrouponCreateEventData : EventData
    {
       public long GrouponOrderId { get; set; }
    }
}
