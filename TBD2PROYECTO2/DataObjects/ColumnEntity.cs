using TBD2PROYECTO2;

namespace TBD2PROYECTO2.DataObjects
{

    public class ColumnEntity
    {
        public Types Types { get; set; }
        public short Length { get; set; }
        public string Name { get; set; }
        public bool IsPrimaryKey { get; set; }

        public ColumnEntity(int dataType, short length, string name, bool isPrimaryKey)
        {
            Types = (Types)dataType;
            Length = length;
            Name = name;
            IsPrimaryKey = isPrimaryKey;
        }
    }
}
