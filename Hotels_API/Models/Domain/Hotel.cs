using System.ComponentModel.DataAnnotations;

namespace Hotels_API.Models.Domain
{
    public class Hotel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PhotoUrl { get; set; }
        public string StreetAndNumber { get; set; }
    }
}
