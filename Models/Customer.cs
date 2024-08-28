namespace CustomerAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }

        public List<ContactNumber> ContactNumbers { get; set; }

        public List<Address> Addresses { get; set; }
    }
}
