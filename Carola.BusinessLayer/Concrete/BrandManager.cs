using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Abstract;
using Carola.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Concrete
{
    public class BrandManager : IBrandService
    {
        private readonly IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _brandDal.DeleteAsync(id);
        }

        public async Task<List<Brand>> TGetAllAsync()
        {
            return await _brandDal.GetAllAsync();
        }

        public async Task<List<Brand>> TGetLast5BrandsAsync()
        {
            return await _brandDal.GetLast5BrandsAsync();
        }

        public async Task<Brand> TGetByIdAsync(int id)
        {
            return await _brandDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(Brand entity)
        {
            await _brandDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Brand entity)
        {
            await _brandDal.UpdateAsync(entity);
        }
    }
}
