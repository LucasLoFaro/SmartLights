namespace ControlDashboardWeb.Models
{
    public class Operator
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public OperatorRole Role { get; set; }
        public DateTime LastLogin { get; set; }
    }
    public enum OperatorRole
    {
        Admin,
        Limited
    }
}
