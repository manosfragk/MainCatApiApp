using CatApiApp.Data;
using CatApiApp.Interfaces;
using CatApiApp.Models;
using CatApiApp.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatApiApp.Tests {
    public class CatServiceTests {
        private readonly Mock<ICatApiClient> _mockCatApiClient;
        private readonly Mock<ICatRepository> _mockCatRepository;
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly DataContext _context;
        private readonly CatService _catService;

        public CatServiceTests() {
            // Mock the ICatApiClient to simulate external API
            _mockCatApiClient = new Mock<ICatApiClient>();
            _mockCatRepository = new Mock<ICatRepository>();
            _mockTagRepository = new Mock<ITagRepository>();

            // Set up in-memory database
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataContext(options);

            // Initialize the service with the mocked client and repositories
            _catService = new CatService(_mockCatApiClient.Object, _mockCatRepository.Object, _mockTagRepository.Object);

            // Configure the mock repositories to interact with _context
            _mockCatRepository.Setup(repo => repo.AddCat(It.IsAny<CatEntity>()))
                .Callback<CatEntity>(cat => _context.Cats.Add(cat));
            _mockCatRepository.Setup(repo => repo.CatExists(It.IsAny<string>()))
                .Returns<string>(id => _context.Cats.Any(c => c.CatId == id));
            _mockCatRepository.Setup(repo => repo.SaveAsync())
                .Returns(() => _context.SaveChangesAsync());

            _mockTagRepository.Setup(repo => repo.GetTagByName(It.IsAny<string>()))
                .ReturnsAsync((string tagName) => _context.Tags.FirstOrDefault(t => t.Name == tagName));
            _mockTagRepository.Setup(repo => repo.AddTagAsync(It.IsAny<TagEntity>()))
                .Callback<TagEntity>(tag => _context.Tags.Add(tag))
                .Returns(() => _context.SaveChangesAsync());
        }

        [Fact]
        public async Task FetchAndStoreCatsAsync_ShouldSaveNewCats() {
            // Arrange
            var mockCats = new List<CatApiResponse>
            {
                new CatApiResponse
                {
                    Id = "1",
                    Url = "http://testcat.com/cat1.jpg",
                    Width = 400,
                    Height = 500,
                    Breeds = new List<CatBreed>
                    {
                        new CatBreed { Temperament = "Playful, Friendly" }
                    }
                },
                new CatApiResponse
                {
                    Id = "2",
                    Url = "http://testcat.com/cat2.jpg",
                    Width = 300,
                    Height = 400,
                    Breeds = new List<CatBreed>
                    {
                        new CatBreed { Temperament = "Calm, Independent" }
                    }
                }
            };

            // Mock the external API call to return the mockCats
            _mockCatApiClient.Setup(client => client.FetchCatImagesAsync()).ReturnsAsync(mockCats);

            // Act
            await _catService.FetchAndStoreCatsAsync();

            // Assert
            _context.ChangeTracker.Clear();  // Ensure no tracking issues on reload
            var cats = await _context.Cats.Include(c => c.Tags).ToListAsync();

            Assert.Equal(2, cats.Count); // Verify 2 cats were saved

            // Verify each cat's tags
            var firstCatTags = cats[0].Tags.Select(t => t.Name).ToList();
            Assert.Contains("Playful", firstCatTags);
            Assert.Contains("Friendly", firstCatTags);

            var secondCatTags = cats[1].Tags.Select(t => t.Name).ToList();
            Assert.Contains("Calm", secondCatTags);
            Assert.Contains("Independent", secondCatTags);
        }
    }
}
