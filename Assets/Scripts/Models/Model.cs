namespace VirtualEngineer.Models
{
    public class Model
    {
        public int id;
        public string name;
        public string title;
        public string description;
        public bool is_draggable;
        public bool is_rotatable;
        public bool is_assemblable;
        public bool is_informational;
        public int? parent_id;
    }
}