using Carola.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Abstract
{
    public interface IBrandService:IGenericService<Brand>
    {
        Task<List<Brand>> TGetLast5BrandsAsync();
    }
}
