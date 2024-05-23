namespace Hotels_API.Models.DTO
{
	public class HotelDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string PhotoUrl { get; set; }
		public string StreetAndNumber { get; set; }
	}
}
