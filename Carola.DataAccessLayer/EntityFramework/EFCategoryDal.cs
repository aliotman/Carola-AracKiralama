using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.EntityFramework
{
    public class EFCategoryDal : GenericRepository<Category>,ICategoryDal
    {
        public EFCategoryDal(CarolaContext context) : base(context)
        {
        }

        public async Task<List<Category>> GetLast5CategoriesAsync()
        {

            return await _context.Categories
                .OrderByDescending(x => x.CategoryId)
                .Take(5)
                .ToListAsync();
        }
    }
}
