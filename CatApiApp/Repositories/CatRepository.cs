using CatApiApp.Data;
using CatApiApp.Interfaces;
using CatApiApp.Models;

namespace CatApiApp.Repositories {
    /// <summary>
    /// Repository for managing cat entities in the database.
    /// </summary>
    /// <param name="context">The data context for interacting with the database.</param>
    public class CatRepository(DataContext context) : ICatRepository {

        private readonly DataContext _context = context;

        /// <summary>
        /// Adds a new cat entity to the database.
        /// </summary>
        /// <param name="cat">The <see cref="CatEntity"/> to add.</param>
        public void AddCat(CatEntity cat) {
            _context.Cats.Add(cat);
        }

        /// <summary>
        /// Checks if a cat with the specified identifier exists in the database.
        /// </summary>
        /// <param name="catId">The identifier of the cat to check.</param>
        /// <returns><c>true</c> if the cat exists; otherwise, <c>false</c>.</returns>
        public bool CatExists(string catId) {
            return _context.Cats.Any(c => c.CatId == catId);
        }

        /// <summary>
        /// Saves all changes asynchronously to the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SaveAsync() {
            await _context.SaveChangesAsync();
        }
    }
}
