using Hotels_API.Models.DTO;
using Hotels_API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotels_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HotelController : ControllerBase
	{
		private readonly IHotelRepository _hotelRepository;
		public HotelController(IHotelRepository hotelRepo) { 
			_hotelRepository = hotelRepo;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllHotelsAsync() 
		{
			var hotels = await _hotelRepository.GetAllHotelsAsync();
			var response = new List<HotelDTO>();
			foreach (var hotel in hotels)
			{
				response.Add(new HotelDTO
				{
					Id = hotel.Id,
					Name = hotel.Name,
					City = hotel.City,
					Country = hotel.Country,
					StreetAndNumber = hotel.StreetAndNumber,
					PhotoUrl = hotel.PhotoUrl
				});
			}
			return Ok(response);
		}
		[HttpGet("search")]
		public async Task<IActionResult> GetHotelsByLocation([FromQuery] string? city, [FromQuery] string? country)
		{
			var hotels = await _hotelRepository.GetHotelsByLocationAsync(city, country);
			if (hotels == null) {
				return NotFound();
			}
			var response = new List<HotelDTO>();
			foreach (var hotel in hotels)
			{
				response.Add(new HotelDTO
				{
					Id = hotel.Id,
					Name = hotel.Name,
					City = hotel.City,
					Country = hotel.Country,
					StreetAndNumber = hotel.StreetAndNumber,
					PhotoUrl = hotel.PhotoUrl
				});
			}

			return Ok(response);
		}
	}
}
