namespace MagicMirror.Business.Models
{
    public class AddressModel : IModel
    {
        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }
}