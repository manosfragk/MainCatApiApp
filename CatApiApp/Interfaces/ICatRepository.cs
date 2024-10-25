using CatApiApp.Models;

namespace CatApiApp.Interfaces {
    /// <summary>
    /// Repository interface for managing cat entities in the database.
    /// </summary>
    public interface ICatRepository {

        /// <summary>
        /// Checks if a cat with the specified identifier exists in the database.
        /// </summary>
        /// <param name="catId">The identifier of the cat to check.</param>
        /// <returns><c>true</c> if the cat exists; otherwise, <c>false</c>.</returns>
        bool CatExists(string catId);

        /// <summary>
        /// Adds a new cat entity to the database.
        /// </summary>
        /// <param name="cat">The <see cref="CatEntity"/> to add.</param>
        void AddCat(CatEntity cat);

        /// <summary>
        /// Saves all changes asynchronously to the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SaveAsync();
    }
}
