using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Frontpages.Dto
{
   public class MyCountInfo
    {
        public int RentsaleinfoUnShow { get; set; }
        public int RentsaleinfoIsSale { get; set; }
        public int RentsaleinfoUnSale { get; set; }

        public int EstateinfoUnShow { get; set; }
        public int EstateinfoIsSale { get; set; }
        public int EstateinfoUnSale { get; set; }

       public int ActivityJoinCount { get; set; }
    }
}
