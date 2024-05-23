using Hotels_API.Models.Domain;

namespace Hotels_API.Repositories.Interface
{
	public interface IHotelRepository
	{
		Task<IEnumerable<Hotel>> GetAllHotelsAsync();
		Task<Hotel?> GetHotelByIdAsync(Guid id);
		Task<IEnumerable<Hotel>?> GetHotelsByLocationAsync(string city, string country);
	
	}
}
