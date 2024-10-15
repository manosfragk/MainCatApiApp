namespace CatApiApp.Dtos
{
    namespace CatApiApp.Dtos
    {
        /// <summary>
        /// Data Transfer Object (DTO) representing a cat entity for API responses.
        /// </summary>
        public class CatDto
        {
            /// <summary>
            /// Gets or sets the unique identifier for the cat.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the unique identifier from the external Cat API.
            /// </summary>
            public string CatId { get; set; }

            /// <summary>
            /// Gets or sets the width of the cat image.
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            /// Gets or sets the height of the cat image.
            /// </summary>
            public int Height { get; set; }

            /// <summary>
            /// Gets or sets the URL of the cat image.
            /// </summary>
            public string Image { get; set; }

            /// <summary>
            /// Gets or sets the date and time the cat entity was created.
            /// </summary>
            public DateTime Created { get; set; }

            /// <summary>
            /// Gets or sets the list of tag names associated with the cat's temperament or breed.
            /// </summary>
            public List<string> Tags { get; set; }
        }
    }

}
