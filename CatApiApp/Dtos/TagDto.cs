using CatApiApp.Models;

namespace CatApiApp.Dtos {
    /// <summary>
    /// Data Transfer Object for <see cref="TagEntity"/>.
    /// </summary>
    /// <remarks>
    /// Represents a simplified view of the <see cref="TagEntity"/> for caching purposes,
    /// containing only the tag ID and name to avoid EF Core tracking conflicts.
    /// </remarks>
    public class TagDto {
        /// <summary>
        /// Gets or sets the unique identifier of the tag.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public string Name { get; set; }
    }
}
