using CatApiApp.Data;
using CatApiApp.Interfaces;
using CatApiApp.Models;

namespace CatApiApp.Repositories {
    public class CatRepository(DataContext context) : ICatRepository {

        private readonly DataContext _context = context;

        public void AddCat(CatEntity cat) {
            _context.Cats.Add(cat);
        }

        public bool CatExists(string catId) {
            return _context.Cats.Any(c => c.CatId == catId);
        }

        public async Task SaveAsync() {
            await _context.SaveChangesAsync();
        }
    }
}
