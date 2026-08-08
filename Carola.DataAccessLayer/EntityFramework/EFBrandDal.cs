using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.EntityFramework
{
    public class EFBrandDal : GenericRepository<Brand>, IBrandDal
    {
        public EFBrandDal(CarolaContext context) : base(context)
        {
        }

        public async Task<List<Brand>> GetLast5BrandsAsync()
        {

            return await _context.Brands
                .OrderByDescending(x => x.BrandId)
                .Take(5)
                .ToListAsync();
        }
    }
}
