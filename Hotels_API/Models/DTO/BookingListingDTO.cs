using Hotels_API.Models.Domain;

namespace Hotels_API.Models.DTO
{
	public class BookingListingDTO
	{
		public string RoomType { get; set; }
		public string CheckIn { get; set; }
		public string CheckOut { get; set; }
		public bool HasBreakfast { get; set; }
		public int PersonCount { get; set; }
		public string HotelName { get; set; }
		public string HotelAdress { get; set; }
		public double Price { get; set; }
	}
}
