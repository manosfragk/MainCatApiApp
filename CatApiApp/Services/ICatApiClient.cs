using Refit;

namespace CatApiApp.Services
{
    /// <summary>
    /// Interface for interacting with the external Cat API.
    /// </summary>
    public interface ICatApiClient
    {
        /// <summary>
        /// Fetches 25 cat images with breeds from the external API.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of cat images.</returns>
        [Get("/v1/images/search?limit=25&has_breeds=true")]
        Task<List<CatApiResponse>> FetchCatImagesAsync();
    }

    /// <summary>
    /// Represents the response from the external Cat API for cat images.
    /// </summary>
    public class CatApiResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cat image.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the URL of the cat image.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the width of the cat image.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the cat image.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the list of breeds associated with the cat image.
        /// </summary>
        public List<CatBreed> Breeds { get; set; }
    }

    /// <summary>
    /// Represents a breed of the cat, including temperament.
    /// </summary>
    public class CatBreed
    {
        /// <summary>
        /// Gets or sets the temperament of the cat breed.
        /// </summary>
        public string Temperament { get; set; }
    }
}
