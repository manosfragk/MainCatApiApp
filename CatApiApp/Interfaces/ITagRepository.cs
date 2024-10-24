using CatApiApp.Models;

namespace CatApiApp.Interfaces {
    public interface ITagRepository {
        /// <summary>
        /// Retrieves a tag by its name.
        /// </summary>
        /// <param name="tagName">The name of the tag to find.</param>
        /// <returns>The found <see cref="TagEntity"/> or null if not found.</returns>
        Task<TagEntity> GetTagByName(string tagName);

        /// <summary>
        /// Adds a new tag to the database.
        /// </summary>
        /// <param name="tag">The <see cref="TagEntity"/> to add.</param>
        Task AddTagAsync(TagEntity tag);

        /// <summary>
        /// Retrieves all tags.
        /// </summary>
        /// <returns>A list of all <see cref="TagEntity"/>.</returns>
        Task<List<TagEntity>> GetAllTagsAsync();
    }
}
