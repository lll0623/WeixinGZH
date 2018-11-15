using System;

namespace LesoftWuye2.CodeGenerator
{
    public class EntityInfoDto
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public string TableName { get; set; }

        public bool IsExists { get; set; }
    }
}
