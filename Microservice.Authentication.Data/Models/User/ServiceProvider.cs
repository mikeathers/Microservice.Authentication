namespace Microservice.Authentication.Data.Models.User
{
    public class ServiceProvider
    {
        public int ServiceProviderId { get; set; }
        public ApplicationUser Identity { get; set; }
        public string Name { get; set; }
        public string BuildingNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Postcode { get; set; }
        

    }
}