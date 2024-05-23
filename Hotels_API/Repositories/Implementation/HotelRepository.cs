using Hotels_API.Data;
using Hotels_API.Models.Domain;
using Hotels_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hotels_API.Repositories.Implementation
{
	public class HotelRepository : IHotelRepository
	{
		private readonly ApplicationDbContext _db;
		public HotelRepository(ApplicationDbContext db)
		{
			_db=db;
		}
		public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
		{
			return await _db.Hotels.ToListAsync();
		}

		public async Task<Hotel?> GetHotelByIdAsync(Guid id)
		{
			return await _db.Hotels.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<Hotel>?> GetHotelsByLocationAsync(string city, string country)
		{
			var query = _db.Hotels.AsQueryable();

			if (!string.IsNullOrWhiteSpace(city))
			{
				city = city.ToLower();
				query = query.Where(h => h.City.ToLower().Contains(city));
			}

			if (!string.IsNullOrWhiteSpace(country))
			{
				country = country.ToLower();
				query = query.Where(h => h.Country.ToLower().Contains(country));
			}

			return await query.ToListAsync();
		}
	}
}
