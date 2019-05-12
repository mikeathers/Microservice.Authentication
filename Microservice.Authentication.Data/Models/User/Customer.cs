namespace Microservice.Authentication.Data.Models.User
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public ApplicationUser Identity { get; set; }
        public string Location { get; set; }
        public string Postcode { get; set; }

    }
}