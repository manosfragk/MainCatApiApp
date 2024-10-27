using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using CatApiApp.Data;
using CatApiApp.Repositories;
using CatApiApp.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Threading;

namespace CatApiApp.Tests.Repositories {
    public class TagRepositoryTests {
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly DbContextOptions<DataContext> _dbContextOptions;

        public TagRepositoryTests() {
            _mockCache = new Mock<IDistributedCache>();

            // Configure in-memory database options for testing
            _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task GetAllTagsAsync_ShouldCacheResultAfterFirstDatabaseCall() {
            // Arrange
            using var context = new DataContext(_dbContextOptions);
            context.Tags.Add(new TagEntity { Id = 1, Name = "TestTag" });
            await context.SaveChangesAsync();

            var repository = new TagRepository(context, _mockCache.Object);

            // Set up the cache to initially return null
            _mockCache.Setup(c => c.GetAsync("AllTags", It.IsAny<CancellationToken>()))
                      .ReturnsAsync((byte[])null);

            // Act - first call should hit the database and cache the result
            var tags = await repository.GetAllTagsAsync();

            // Serialize the tags to JSON and convert to byte array for cache verification
            var serializedTags = JsonConvert.SerializeObject(tags);
            var cachedTags = Encoding.UTF8.GetBytes(serializedTags);

            // Verify that cache was set after first call
            _mockCache.Verify(c => c.SetAsync("AllTags", cachedTags,
                It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);

            // Set up the cache to return the cached result on the next call
            _mockCache.Setup(c => c.GetAsync("AllTags", It.IsAny<CancellationToken>()))
                      .ReturnsAsync(cachedTags);

            // Act - second call should use cache
            var cachedTagsResult = await repository.GetAllTagsAsync();

            // Assert
            Assert.Equal(tags.Count, cachedTagsResult.Count);
            _mockCache.Verify(c => c.GetAsync("AllTags", It.IsAny<CancellationToken>()), Times.Exactly(2)); // First for null, then cached
        }
    }
}
