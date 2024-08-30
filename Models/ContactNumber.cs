namespace CustomerAPI.Models
{
    public class ContactNumber
    {
        public int Id { get; set; }
        public string Type { get; set; } // "Mobile", "Phone"
        public string Number { get; set; }
    }
}
