using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace LesoftWuye2.Core.Utils
{
    public class DateFormat : IsoDateTimeConverter
    {
        public DateFormat()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }

    public class DateTimeFormat : IsoDateTimeConverter
    {
        public DateTimeFormat()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
