using Microsoft.EntityFrameworkCore;
using MyWebPage.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zadanie_rekrutacyjne.Database.Data;
using zadanie_rekrutacyjne.Database.Models;

namespace zadanie_rekrutacyjne.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseContext _context;

        public CategoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        // standard get method with include relationship (get with parent or with children or current category with paren and children)
        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Category>> GetAllWithParentsAsync()
        {
            return await _context.Categories.Include(c => c.Parent).ToListAsync();
        }

        public async Task<List<Category>> GetAllWithChildrenAsync()
        {
            return await _context.Categories.Include(c => c.CategoryChildren).ToListAsync();
        }

        public async Task<List<Category>> GetMainAsync()
        {
            return await _context.Categories.Where(c => c.Parent == null).ToListAsync();
        }

        public async Task<List<Category>> GetMainWithChildrenAsync()
        {
            return await _context.Categories.Where(c => c.Parent == null).Include(c => c.CategoryChildren).ToListAsync();
        }

        public async Task<Category> FindAsync(int? id)
        {
            return await _context.Categories.Include(p => p.Parent)
                .FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<Category> FindWithChildrenAsync(int? id)
        {
            return await _context.Categories.Include(p => p.Parent).Include(c => c.CategoryChildren)
                .FirstOrDefaultAsync(c => c.ID == id);
        }
        public async Task<List<Category>> GetChildrenAsync(int parentID)
        {
            return await _context.Categories.Where(c => c.Parent.ID == parentID).ToListAsync();
        }

        // recursive method to get all categories from current node
        public async Task<List<Category>> GetAllNodeChildrenAsync(int parentID)
        {
            List<Category> children = await _context.Categories.Where(c => c.Parent.ID == parentID).ToListAsync();
            List<Category> allNodeChildren = new List<Category>();
            allNodeChildren.AddRange(children);

            foreach(Category category in children)
            {
                allNodeChildren.AddRange(await GetAllNodeChildrenAsync(category.ID));
            }

            return allNodeChildren;
        }

        public void Add(Category category)
        {
            _context.Add(category);
        }

        public void Update(Category category)
        {
            _context.Update(category);
        }

        // recursive method to delete all categories from current node
        public async Task DeleteWithChildrenAsync(Category category)
        {
            try
            {
                if (category.CategoryChildren != null && category.CategoryChildren.Count > 0)
                {
                    foreach (Category categoryChildren in category.CategoryChildren)
                    {
                        Category categoryChildWithChildren = await FindWithChildrenAsync(categoryChildren.ID);
                        await DeleteWithChildrenAsync(categoryChildWithChildren);
                    }
                    DeleteAsync(category);
                }
                else
                {
                   DeleteAsync(category);
                }  
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        public void DeleteAsync(Category category)
        {
            _context.Remove(category);
        }

        public bool CategoryExist(int id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            
        }
    }
}
