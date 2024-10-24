using CatApiApp.Models;

namespace CatApiApp.Interfaces {
    public interface ICatRepository {
        bool CatExists(string catId);
        void AddCat(CatEntity cat);
        Task SaveAsync();
    }
}
