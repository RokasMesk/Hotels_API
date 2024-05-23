using Hotels_API.Models.Enums;

namespace Hotels_API.Models.Domain
{
	public class Booking
	{
		public Guid Id { get; set; }
		public string RoomType { get; set; }
		public DateTime CheckIn { get; set; }
		public DateTime CheckOut { get; set; }
		public bool HasBreakfast { get; set; }
		public int PersonCount { get; set; }
		public Hotel Hotel { get; set; }
		public double Price { get; set; }
		
		
	}
}
