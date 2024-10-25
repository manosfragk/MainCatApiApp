using CatApiApp.Models;

namespace CatApiApp.Interfaces {
    /// <summary>
    /// Repository interface for managing tag entities in the database.
    /// </summary>
    public interface ITagRepository {

        /// <summary>
        /// Retrieves a tag by its name.
        /// </summary>
        /// <param name="tagName">The name of the tag to find.</param>
        /// <returns>The found <see cref="TagEntity"/> or <c>null</c> if not found.</returns>
        Task<TagEntity> GetTagByName(string tagName);

        /// <summary>
        /// Adds a new tag to the database asynchronously.
        /// </summary>
        /// <param name="tag">The <see cref="TagEntity"/> to add.</param>
        Task AddTagAsync(TagEntity tag);

        /// <summary>
        /// Retrieves all tags from the database asynchronously.
        /// </summary>
        /// <returns>A list of all <see cref="TagEntity"/> objects.</returns>
        Task<List<TagEntity>> GetAllTagsAsync();
    }
}
