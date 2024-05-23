using Hotels_API.Controllers;
using Hotels_API.Models.Domain;
using Hotels_API.Models.DTO;
using Hotels_API.Models.Enums;
using Hotels_API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hotels_API.Tests
{
	public class BookingControllerTests
	{
		private readonly Mock<IBookingRepository> _mockBookingRepo;
		private readonly Mock<IHotelRepository> _mockHotelRepo;
		private readonly BookingController _controller;

		public BookingControllerTests()
		{
			_mockBookingRepo = new Mock<IBookingRepository>();
			_mockHotelRepo = new Mock<IHotelRepository>();
			_controller = new BookingController(_mockBookingRepo.Object, _mockHotelRepo.Object);
		}

		[Fact]
		public async Task CreateBooking_ReturnsNotFound_WhenHotelDoesNotExist()
		{
			var request = new CreateBookingRequestDTO { HotelId = Guid.NewGuid() };
			_mockHotelRepo.Setup(repo => repo.GetHotelByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Hotel)null);

			var result = await _controller.CreateBooking(request);

			var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal("Hotel not found", notFoundResult.Value);
		}
		[Fact]
		public async Task CreateBooking_ReturnsOk_WithBookingDetails()
		{
			var hotel = new Hotel { Id = Guid.NewGuid(), Name = "Panemune" };
			var request = new CreateBookingRequestDTO
			{
				HotelId = hotel.Id,
				CheckIn = DateTime.Now,
				CheckOut = DateTime.Now.AddDays(50),
				PersonCount = 2,
				HasBreakfast = true,
				RoomType = "Standard"
			};
			_mockHotelRepo.Setup(repo => repo.GetHotelByIdAsync(hotel.Id)).ReturnsAsync(hotel);
			_mockBookingRepo.Setup(repo => repo.CreateBookingAsync(It.IsAny<Booking>())).Returns(Task.FromResult<Booking>(null));

			var result = await _controller.CreateBooking(request);

			var okResult = Assert.IsType<OkObjectResult>(result);
			var booking = Assert.IsType<Booking>(okResult.Value);
			Assert.Equal(request.CheckIn, booking.CheckIn);
			Assert.Equal(request.CheckOut, booking.CheckOut);
			Assert.Equal(request.PersonCount, booking.PersonCount);
			Assert.Equal(request.HasBreakfast, booking.HasBreakfast);
			Assert.Equal(request.RoomType, booking.RoomType);
			Assert.Equal(hotel, booking.Hotel);
		}
		[Fact]
		public async Task GetAllBookings_ReturnsOk_WithBookings()
		{
			var bookings = new List<Booking>
			{
				new Booking
				{
					CheckIn = DateTime.Now,
					CheckOut = DateTime.Now.AddDays(1),
					PersonCount = 2,
					HasBreakfast = true,
					RoomType = "Standard",
					Hotel = new Hotel
					{
						Name = "Upiukas",
						City = "Miestukas",
						Country = "Tarzanija",
						StreetAndNumber = "1590 Sumustiniu gatve"
					},
					Price = 120
				}
			};
			_mockBookingRepo.Setup(repo => repo.GetAllBookingsAsync()).ReturnsAsync(bookings);

			var result = await _controller.GetAllBookings();

			var okResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<List<BookingListingDTO>>(okResult.Value);
			Assert.Single(response);
		}
		[Fact]
		public void CalculatePrice_CorrectlyCalculatesPrice_ForStandardRoom_WithBreakfast()
		{
			var booking = new Booking
			{
				CheckIn = DateTime.Now,
				CheckOut = DateTime.Now.AddDays(2),
				PersonCount = 2,
				HasBreakfast = true,
				RoomType = RoomType.Standard.ToString()
			};
			var price = CalculatePrice(booking);
			
			Assert.Equal(280, price); // (100*2 + 20 + 15*2*2)
		}
		[Fact]
		public void CalculatePrice_CorrectlyCalculatesPrice_ForDeluxeWithoutBreakfast()
		{
			var booking = new Booking
			{
				CheckIn = DateTime.Now,
				CheckOut = DateTime.Now.AddDays(3),
				PersonCount = 3,
				HasBreakfast = false,
				RoomType = RoomType.Deluxe.ToString()
			};
			var price = CalculatePrice(booking);

			Assert.Equal(470, price); // (150*3 + 20)
		}
		[Fact]
		public void CalculatePrice_CorrectlyCalculatesPrice_ForSuiteWithBreakfast()
		{
			var booking = new Booking
			{
				CheckIn = DateTime.Now,
				CheckOut = DateTime.Now.AddDays(4),
				PersonCount = 4,
				HasBreakfast = true,
				RoomType = RoomType.Suite.ToString()
			};
			var price = CalculatePrice(booking);

			Assert.Equal(1060, price); // (200*4 + 20+4*4*15)
		}
		[Fact]
		public void CalculatePrice_ThrowsArgumentException_ForInvalidDates()
		{
			var booking = new Booking
			{
				CheckIn = DateTime.Now,
				CheckOut = DateTime.Now.AddDays(-1),
				PersonCount = 2,
				HasBreakfast = true,
				RoomType = RoomType.Standard.ToString()
			};
			var ex = Assert.Throws<ArgumentException>(() => CalculatePrice(booking));
			Assert.Equal("Dates are not correctt", ex.Message);
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
				throw new ArgumentException("Dates are not correctt");
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