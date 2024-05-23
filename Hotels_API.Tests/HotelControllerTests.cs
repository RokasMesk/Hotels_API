using Hotels_API.Controllers;
using Hotels_API.Models.Domain;
using Hotels_API.Models.DTO;
using Hotels_API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels_API.Tests
{
	public class HotelControllerTests
	{
		private readonly Mock<IHotelRepository> _mockHotelRepo;
		private readonly HotelController _controller;

		public HotelControllerTests()
		{
			_mockHotelRepo = new Mock<IHotelRepository>();
			_controller = new HotelController(_mockHotelRepo.Object);
		}

		[Fact]
		public async Task GetAllHotelsAsync_ReturnsOk_WithHotels()
		{
			var hotels = new List<Hotel>
			{
				new Hotel { Id = Guid.NewGuid(), Name = "Vezucijaus", City = "Manchesteris", Country = "Kountry", StreetAndNumber = "1590 testas", PhotoUrl = "url" }
			};
			_mockHotelRepo.Setup(repo => repo.GetAllHotelsAsync()).ReturnsAsync(hotels);

			var result = await _controller.GetAllHotelsAsync();

			var okResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<List<HotelDTO>>(okResult.Value);
			Assert.Single(response);
		}

		[Fact]
		public async Task GetHotelsByLocation_ReturnsNotFound_WhenNoHotels()
		{
			_mockHotelRepo.Setup(repo => repo.GetHotelsByLocationAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((List<Hotel>)null);

			var result = await _controller.GetHotelsByLocation("Test City", "Test Country");

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task GetHotelsByLocation_ReturnsOk_WithHotels()
		{
			var hotels = new List<Hotel>
			{
				new Hotel { Id = Guid.NewGuid(), Name = "Test Hotel", City = "Test City", Country = "Test Country", StreetAndNumber = "15905190", PhotoUrl = "159015901590" }
			};
			_mockHotelRepo.Setup(repo => repo.GetHotelsByLocationAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hotels);

			var result = await _controller.GetHotelsByLocation("Test City", "Test Country");

			var okResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<List<HotelDTO>>(okResult.Value);
			Assert.Single(response);
		}
	}
}

