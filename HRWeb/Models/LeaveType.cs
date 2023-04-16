namespace HRWeb.Models
{
    public class LeaveType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Leave>? Leaves { get; set; }

        public LeaveType () { }

        public LeaveType(int id, string name)
        {
            Id = id;
            Name = name;
            
        }
    }
}
