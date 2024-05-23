using Hotels_API.Data;
using Hotels_API.Models.Domain;
using Hotels_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hotels_API.Repositories.Implementation
{
	public class BookingRepository : IBookingRepository
	{
		private readonly ApplicationDbContext _db;
		public BookingRepository(ApplicationDbContext db)
		{
			_db=db;
		}

		public async Task<Booking> CreateBookingAsync(Booking booking)
		{
			await _db.Bookings.AddAsync(booking);
			await _db.SaveChangesAsync();
			return booking;
		}

		public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
		{
			return await _db.Bookings.Include(x=> x.Hotel).ToListAsync();
		}
		
	}
}
