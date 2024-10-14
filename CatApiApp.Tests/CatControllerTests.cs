using CatApiApp.Controllers;
using CatApiApp.Data;
using CatApiApp.Models;
using CatApiApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatApiApp.Tests
{
    public class CatControllerTests
    {
        private readonly Mock<ICatService> _mockCatService;
        private readonly DataContext _context;
        private readonly CatController _controller;

        public CatControllerTests()
        {
            // Mock the CatService
            _mockCatService = new Mock<ICatService>();

            // Set up in-memory database for testing the DataContext
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataContext(options);

            // Initialize the controller with the mocked service and in-memory context
            _controller = new CatController(_mockCatService.Object, _context);
        }

        [Fact]
        public async Task FetchAndStoreCats_ShouldReturnOk()
        {
            // Arrange: Set up the mock service to not throw exceptions
            _mockCatService.Setup(s => s.FetchAndStoreCatsAsync()).Returns(Task.CompletedTask);

            // Act: Call the FetchAndStoreCats method
            var result = await _controller.FetchAndStoreCats();

            // Assert: Ensure it returns an OK result
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task FetchAndStoreCats_ShouldReturnInternalServerError_OnException()
        {
            // Arrange: Set up the mock service to throw an exception
            _mockCatService.Setup(s => s.FetchAndStoreCatsAsync()).ThrowsAsync(new System.Exception("Test exception"));

            // Act: Call the FetchAndStoreCats method
            var result = await _controller.FetchAndStoreCats();

            // Assert: Ensure it returns a 500 status code
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetCatById_ShouldReturnOk_WhenCatExists()
        {
            // Arrange: Add a test cat to the in-memory database
            var testCat = new CatEntity { Id = 1, CatId = "testcat", Width = 300, Height = 400, Image = "http://testcat.com" };
            _context.Cats.Add(testCat);
            await _context.SaveChangesAsync();

            // Act: Call the GetCatById method
            var result = await _controller.GetCatById(1);

            // Assert: Ensure it returns an OK result with the cat
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCat = Assert.IsType<CatEntity>(okResult.Value);
            Assert.Equal(1, returnedCat.Id);
        }

        [Fact]
        public async Task GetCatById_ShouldReturnNotFound_WhenCatDoesNotExist()
        {
            // Act: Call the GetCatById method with a non-existent ID
            var result = await _controller.GetCatById(99);

            // Assert: Ensure it returns a 404 Not Found result
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetCats_ShouldReturnOk_WithPaging()
        {
            // Arrange: Add multiple cats to the in-memory database
            for (int i = 1; i <= 10; i++)
            {
                _context.Cats.Add(new CatEntity { CatId = $"cat{i}", Width = 300, Height = 400, Image = $"http://testcat.com/cat{i}" });
            }
            await _context.SaveChangesAsync();

            // Act: Call the GetCats method with paging parameters
            var result = await _controller.GetCats(1, 5);  // Page 1, 5 cats per page

            // Assert: Ensure it returns an OK result with 5 cats
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCats = Assert.IsType<List<CatEntity>>(okResult.Value);
            Assert.Equal(5, returnedCats.Count); // Ensure 5 cats were returned
        }

        [Fact]
        public async Task GetCats_ShouldReturnNotFound_WhenNoCatsExist()
        {
            // Act: Call the GetCats method when no cats exist in the database
            var result = await _controller.GetCats();

            // Assert: Ensure it returns a 404 Not Found result (NotFoundObjectResult)
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task GetCats_ShouldFilterByTag()
        {
            // Arrange: Add cats with tags to the in-memory database
            var tag1 = new TagEntity { Name = "Playful" }; // No need to set the Id, let EF auto-generate it
            var tag2 = new TagEntity { Name = "Calm" };    // No need to set the Id, let EF auto-generate it

            var cat1 = new CatEntity { CatId = "cat1", Width = 300, Height = 400, Image = "http://testcat.com/cat1", Tags = new List<TagEntity> { tag1 } };
            var cat2 = new CatEntity { CatId = "cat2", Width = 300, Height = 400, Image = "http://testcat.com/cat2", Tags = new List<TagEntity> { tag2 } };

            _context.Cats.AddRange(cat1, cat2);
            await _context.SaveChangesAsync();

            // Act: Call the GetCats method to filter by the "Playful" tag
            var result = await _controller.GetCats(1, 10, "Playful");

            // Assert: Ensure it returns only cats with the "Playful" tag
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCats = Assert.IsType<List<CatEntity>>(okResult.Value);
            Assert.Single(returnedCats); // Only one cat should be returned
            Assert.Equal("cat1", returnedCats[0].CatId); // Ensure the correct cat is returned
        }

    }
}
