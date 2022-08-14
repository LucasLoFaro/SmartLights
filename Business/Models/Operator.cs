namespace Business.Models
{
    public class Operator
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public OperatorRole Role { get; set; }
        public DateTime LastLogin { get; set; }
        public List<Object> ActionsTaken { get; set; }
        
    }    

    public enum OperatorRole
    {
        Admin,
        User
    }
}