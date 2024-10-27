using CatApiApp.Data;
using CatApiApp.Dtos;
using CatApiApp.Interfaces;
using CatApiApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace CatApiApp.Repositories {
    /// <summary>
    /// Repository for managing tag entities in the database with caching support.
    /// </summary>
    /// <remarks>
    /// Provides methods to retrieve and manipulate <see cref="TagEntity"/> objects, 
    /// using Redis caching to optimize retrieval times and reduce database load.
    /// </remarks>
    /// <param name="context">The data context for accessing the database.</param>
    /// <param name="cache">The distributed cache instance for caching data.</param>
    public class TagRepository(DataContext context, IDistributedCache cache) : ITagRepository {
        private readonly DataContext _context = context;
        private readonly IDistributedCache _cache = cache;

        /// <summary>
        /// Retrieves a tag by its name from the cache or database.
        /// </summary>
        /// <remarks>
        /// First attempts to retrieve the tag from cache using the specified <paramref name="tagName"/>.
        /// If not found in cache, retrieves from the database, caches the result for future access, 
        /// and returns the tag.
        /// </remarks>
        /// <param name="tagName">The name of the tag to search for.</param>
        /// <returns>
        /// A <see cref="TagEntity"/> object representing the found tag, or <c>null</c> if the tag is not found.
        /// </returns>
        public async Task<TagEntity> GetTagByName(string tagName) {
            var cacheKey = $"Tag_{tagName}";

            // Try to get the tag DTO from cache
            var cachedTagDto = await _cache.GetStringAsync(cacheKey);
            if (cachedTagDto != null) {
                var tagDto = JsonConvert.DeserializeObject<TagDto>(cachedTagDto);
                if (tagDto != null) {
                    // Check if the tag is already tracked by context to avoid re-attaching
                    var trackedTag = _context.ChangeTracker.Entries<TagEntity>()
                                             .FirstOrDefault(e => e.Entity.Id == tagDto.Id)?.Entity;
                    if (trackedTag != null) {
                        return trackedTag;
                    }

                    // Retrieve the tag with tracking enabled since it was not previously tracked
                    return await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagDto.Id);
                }
            }

            // If not in cache, retrieve from database and cache as DTO
            var tag = await _context.Tags.AsNoTracking().FirstOrDefaultAsync(t => t.Name == tagName);
            if (tag != null) {
                // Cache the tag as a DTO to avoid tracking conflicts
                var tagDto = new TagDto { Id = tag.Id, Name = tag.Name };
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(tagDto),
                    new DistributedCacheEntryOptions {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
            }

            return tag;
        }

        /// <summary>
        /// Adds a new tag entity to the database asynchronously and invalidates the relevant cache.
        /// </summary>
        /// <remarks>
        /// After adding the tag to the database, removes the cache entry for the tag if it exists,
        /// to ensure that subsequent retrievals reflect the updated database state.
        /// </remarks>
        /// <param name="tag">The <see cref="TagEntity"/> to be added to the database.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation of adding the tag and saving changes.
        /// </returns>
        public async Task AddTagAsync(TagEntity tag) {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            // Cache the newly added tag as a DTO
            var cacheKey = $"Tag_{tag.Name}";
            var tagDto = new TagDto { Id = tag.Id, Name = tag.Name };
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(tagDto),
                new DistributedCacheEntryOptions {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
        }

        /// <summary>
        /// Retrieves all tags from the cache or, if not cached, from the database.
        /// </summary>
        /// <remarks>
        /// This method first attempts to retrieve the list of tags from the Redis cache. 
        /// If the cache entry is missing, it fetches the tags from the database, 
        /// caches the result for future access, and then returns the list.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation, with a list of 
        /// all <see cref="TagEntity"/> objects retrieved from either the cache or the database.
        /// </returns>
        public async Task<List<TagEntity>> GetAllTagsAsync() {
            const string cacheKey = "AllTags";

            // Check if the tags are in cache as byte array
            var cachedTags = await _cache.GetAsync(cacheKey);
            if (cachedTags != null) {
                var cachedTagsJson = Encoding.UTF8.GetString(cachedTags);
                return JsonConvert.DeserializeObject<List<TagEntity>>(cachedTagsJson);
            }

            // If not in cache, retrieve from database
            var tags = await _context.Tags.AsNoTracking().ToListAsync();

            // Cache the result as a byte array
            var tagsJson = JsonConvert.SerializeObject(tags);
            var tagsBytes = Encoding.UTF8.GetBytes(tagsJson);
            await _cache.SetAsync(cacheKey, tagsBytes, new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return tags;
        }
    }
}
