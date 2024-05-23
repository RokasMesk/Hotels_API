using Hotels_API.Models.Domain;
using Hotels_API.Models.DTO;
using Hotels_API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Hotels_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingController : ControllerBase
	{
		private readonly IBookingRepository _bookingRepo;
		private readonly IHotelRepository _hotelRepository;
		public BookingController(IBookingRepository bookingRepo, IHotelRepository hotelRepository) 
		{
			_bookingRepo = bookingRepo;
			_hotelRepository = hotelRepository;
		}
		[HttpPost]
		public async Task<IActionResult> CreateBooking(CreateBookingRequestDTO request)
		{
			var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId);
			if (hotel is null) 
			{
				return NotFound("Hotel not found");
			}
			var booking = new Booking
			{
				CheckIn = request.CheckIn,
				CheckOut = request.CheckOut,
				PersonCount = request.PersonCount,
				HasBreakfast = request.HasBreakfast,
				RoomType = request.RoomType,
				Hotel = hotel
			};
			booking.Price = CalculatePrice(booking);
			await _bookingRepo.CreateBookingAsync(booking);
			return Ok(booking);
		}
		[HttpGet]
		public async Task<IActionResult> GetAllBookings()
		{
			var bookings = await _bookingRepo.GetAllBookingsAsync();
			if (bookings is null) {
				return NotFound();
			}
			List<BookingListingDTO> response = new List<BookingListingDTO>();
			foreach (var booking in bookings)
			{
				response.Add(new BookingListingDTO
				{
					CheckIn = booking.CheckIn.ToString("yyyy-MM-dd"),
					CheckOut = booking.CheckOut.ToString("yyyy-MM-dd"),
					PersonCount = booking.PersonCount,
					HasBreakfast = booking.HasBreakfast,
					RoomType = booking.RoomType,
					HotelAdress = String.Format("{0}, {1} {2}", booking.Hotel.Country, booking.Hotel.City, booking.Hotel.StreetAndNumber),
					HotelName = booking.Hotel.Name,
					Price = booking.Price

				});
			}
			return Ok(response);
		}
	
		private double CalculatePrice(Booking booking)
		{
			Dictionary<string, int> roomRates = new Dictionary<string, int>
			{
				{ "Standard", 100 },
				{ "Deluxe", 150},
				{ "Suite", 200}
			};
			
			if (!roomRates.TryGetValue(booking.RoomType, out var roomRate))
			{
				throw new ArgumentException("NO roomtype found");
			}
			int numberOfNights = (booking.CheckOut-booking.CheckIn).Days;
			if (numberOfNights <= 0) 
			{
				throw new ArgumentException("Dates are not correct");
			}
			double roomCost = roomRate*numberOfNights;
			int cleaningFee = 20;
			int breakfastCost = 0;
			if (booking.HasBreakfast) 
			{
				breakfastCost = 15*booking.PersonCount*numberOfNights;
			}
			double totalCost = roomCost + cleaningFee + breakfastCost;
			return totalCost;
		}
	}
	
}
