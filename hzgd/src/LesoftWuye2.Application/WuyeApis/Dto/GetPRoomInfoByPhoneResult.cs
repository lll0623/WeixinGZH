using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.WuyeApis.Dto
{
    public class GetPRoomInfoByPhoneResult : InvokeResultDto
    {

        public infos info { get; set; }

        public List<RoomItem> PRooms { get; set; }

        public class infos
        {
            public string ID { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class RoomItem
        {
            public string PRoomCode { get; set; }
            public string PProjectName { get; set; }
            public string PBuildingName { get; set; }
            public string PFloorName { get; set; }
            public string PRoomName { get; set; }

            public string PProjectCode { get; set; }

            public string PRoomFullName
            {
                get { return PProjectName + PBuildingName + PFloorName + PRoomName; }
            }
        }
    }
}
