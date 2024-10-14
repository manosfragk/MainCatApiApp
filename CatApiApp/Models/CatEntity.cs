using System.ComponentModel.DataAnnotations;

namespace CatApiApp.Models
{
    /// <summary>
    /// Represents the CatEntity stored in the database.
    /// </summary>
    public class CatEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cat entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the cat image from the external Cat API.
        /// </summary>
        [Required(ErrorMessage = "CatId is required")]
        public string CatId { get; set; }

        /// <summary>
        /// Gets or sets the width of the cat image.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Width must be greater than 0")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the cat image.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Height must be greater than 0")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the URL of the cat image.
        /// </summary>
        [Required(ErrorMessage = "Image is required")]
        [Url(ErrorMessage = "The Image field must be a valid URL.")] 
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the cat entity was created.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the list of tags associated with the cat.
        /// </summary>
        public List<TagEntity> Tags { get; set; } = [];
    }
}
