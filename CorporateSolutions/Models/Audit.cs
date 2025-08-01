namespace CorporateSolutions.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Username { get; set; }
        public DateTime Timestamp { get; set; }
        public string Changes { get; set; }
    }
}
