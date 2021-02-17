using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zadanie_rekrutacyjne.Database.Models;

namespace MyWebPage.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<List<Category>> GetAllWithParentsAsync();
        Task<List<Category>> GetAllWithChildrenAsync();

        Task<List<Category>> GetMainAsync();
        Task<List<Category>> GetMainWithChildrenAsync();

        Task<List<Category>> GetChildrenAsync(int parentID);
        Task<List<Category>> GetAllNodeChildrenAsync(int parentID);

        Task<Category> FindAsync(int? id);
        Task<Category> FindWithChildrenAsync(int? id);

        void Add(Category category);

        void Update(Category category);
        //Task UpdateParentChildren(int childrenID, Category parent);

        void DeleteAsync(Category category);
        Task DeleteWithChildrenAsync(Category category);

        bool CategoryExist(int id);

        Task SaveChangesAsync();

    }
}
