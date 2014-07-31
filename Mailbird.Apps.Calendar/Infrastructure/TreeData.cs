using System;

namespace Mailbird.Apps.Calendar.Infrastructure
{
    public class TreeData
    {
        public string ParentID { get; set; }

        public string Name { get; set; }

        public TreeDataType DataType { get; set; }

        public Object Data { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is TreeData)) return false;
            var other = obj as TreeData;
            return ParentID == other.ParentID && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + ParentID.GetHashCode();
        }
    }
    
    public enum TreeDataType
    {
        Catalog, Provider, Calendar
    }
}
