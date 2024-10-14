namespace CatApiApp.Services
{
    /// <summary>
    /// Defines a contract for fetching and storing cat images from an external API.
    /// </summary>
    public interface ICatService
    {
        /// <summary>
        /// Fetches 25 cat images from an external API and stores them in the database.
        /// </summary>
        /// <remarks>
        /// This method interacts with the external Cat API to fetch cat images,
        /// including their associated metadata (such as width, height, image URL, and tags).
        /// The fetched data is then stored in the SQL database, and duplicate entries are avoided
        /// based on the <c>CatId</c> field.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation of fetching and storing cats.
        /// </returns>
        Task FetchAndStoreCatsAsync();
    }

}
