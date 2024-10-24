using CatApiApp.Data;
using CatApiApp.Interfaces;
using CatApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CatApiApp.Repositories {
    public class TagRepository : ITagRepository {
        private readonly DataContext _context;

        public TagRepository(DataContext context) {
            _context = context;
        }

        /// <summary>
        /// Retrieves a tag by its name from the database.
        /// </summary>
        /// <param name="tagName">The name of the tag to find.</param>
        /// <returns>The found <see cref="TagEntity"/> or null if not found.</returns>
        public async Task<TagEntity> GetTagByName(string tagName) {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
        }

        /// <summary>
        /// Adds a new tag to the database.
        /// </summary>
        /// <param name="tag">The <see cref="TagEntity"/> to add.</param>
        public async Task AddTagAsync(TagEntity tag) {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all tags from the database.
        /// </summary>
        /// <returns>A list of all <see cref="TagEntity"/>.</returns>
        public async Task<List<TagEntity>> GetAllTagsAsync() {
            return await _context.Tags.ToListAsync();
        }
    }
}
