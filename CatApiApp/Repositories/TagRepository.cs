using CatApiApp.Data;
using CatApiApp.Interfaces;
using CatApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CatApiApp.Repositories {
    /// <summary>
    /// Repository for managing tag entities in the database.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TagRepository"/> class.
    /// </remarks>
    /// <param name="context">The data context for accessing the database.</param>
    public class TagRepository(DataContext context) : ITagRepository {

        private readonly DataContext _context = context;

        /// <summary>
        /// Retrieves a tag by its name from the database.
        /// </summary>
        /// <param name="tagName">The name of the tag to find.</param>
        /// <returns>
        /// A <see cref="TagEntity"/> object representing the found tag, or <c>null</c> if not found.
        /// </returns>
        public async Task<TagEntity> GetTagByName(string tagName) {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
        }

        /// <summary>
        /// Adds a new tag entity to the database asynchronously.
        /// </summary>
        /// <param name="tag">The <see cref="TagEntity"/> to add.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation of adding a tag and saving changes.
        /// </returns>
        public async Task AddTagAsync(TagEntity tag) {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all tag entities from the database asynchronously.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation, with a list of all <see cref="TagEntity"/> objects.
        /// </returns>
        public async Task<List<TagEntity>> GetAllTagsAsync() {
            return await _context.Tags.ToListAsync();
        }
    }

}
