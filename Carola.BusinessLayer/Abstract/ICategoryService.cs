using Carola.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Abstract
{
    public interface ICategoryService:IGenericService<Category>
    {
        Task<List<Category>> TGetLast5CategoriesAsync();
    }
}
