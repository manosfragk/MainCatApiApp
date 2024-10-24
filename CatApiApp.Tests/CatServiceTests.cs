using CatApiApp.Data;
using CatApiApp.Interfaces;
using CatApiApp.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CatApiApp.Tests
{
    public class CatServiceTests
    {
        private readonly Mock<ICatApiClient> _mockCatApiClient;
        private readonly Mock<ICatRepository> _mockCatRepository;
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly DataContext _context;
        private readonly CatService _catService;

        public CatServiceTests()
        {
            // Mock the ICatApiClient to simulate external API
            _mockCatApiClient = new Mock<ICatApiClient>();
            _mockCatRepository = new Mock<ICatRepository>();
            _mockTagRepository = new Mock<ITagRepository>();

            // Set up in-memory database
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataContext(options);

            // Initialize the service with the mocked client and in-memory database
            _catService = new CatService(_mockCatApiClient.Object, _mockCatRepository.Object, _mockTagRepository.Object);
        }

        [Fact]
        public async Task FetchAndStoreCatsAsync_ShouldSaveNewCats()
        {
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
            var cats = _context.Cats.Include(c => c.Tags).ToList();
            Assert.Equal(2, cats.Count); // Check that 2 cats were saved

            // Ensure the first cat's tags are unique
            var firstCatTags = cats[0].Tags.Select(t => t.Name).Distinct().ToList();
            Assert.Equal(2, firstCatTags.Count); // "Playful" and "Friendly"

            // Ensure the second cat's tags are unique
            var secondCatTags = cats[1].Tags.Select(t => t.Name).Distinct().ToList();
            Assert.Equal(2, secondCatTags.Count); // "Calm" and "Independent"
        }
    }
}