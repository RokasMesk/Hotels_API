using Hotels_API.Models.Domain;

namespace Hotels_API.Repositories.Interface
{
	public interface IBookingRepository
	{
		Task<IEnumerable<Booking>>GetAllBookingsAsync();
		Task<Booking> CreateBookingAsync(Booking booking);
		
	}
}
