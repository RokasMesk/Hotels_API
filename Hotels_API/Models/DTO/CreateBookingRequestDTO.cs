using Hotels_API.Models.Enums;

namespace Hotels_API.Models.DTO
{
	public class CreateBookingRequestDTO
	{
		public Guid HotelId { get; set; }
		public DateTime CheckIn { get; set; }
		public DateTime CheckOut { get; set; }
		public bool HasBreakfast { get; set; }
		public int PersonCount { get; set; }
		public string RoomType { get; set; }
	}
}
