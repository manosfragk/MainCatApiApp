using System.ComponentModel.DataAnnotations;

namespace CatApiApp.Models
{
    /// <summary>
    /// Represents the TagEntity related to a cat's temperament or breed.
    /// </summary>
    public class TagEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tag entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag, describing the cat's temperament.
        /// </summary>
        [Required(ErrorMessage = "Tag name is required")]
        [MaxLength(50, ErrorMessage = "Tag name cannot be longer than 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the tag entity was created.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the list of cats associated with the tag.
        /// </summary>
        public List<CatEntity> Cats { get; set; } = [];
    }
}
