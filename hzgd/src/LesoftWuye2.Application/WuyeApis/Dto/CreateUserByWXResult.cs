﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class CreateUserByWXResult : InvokeResultDto
    {
        public long Id { get; set; }
        public int Type { get; set; }
    }

   
}
