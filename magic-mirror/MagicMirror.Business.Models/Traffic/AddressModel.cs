namespace MagicMirror.Business.Models.Traffic
{
    public class AddressModel:IModel
    {
        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }
}
