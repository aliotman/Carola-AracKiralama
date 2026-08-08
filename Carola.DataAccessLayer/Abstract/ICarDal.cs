using Carola.DataAccessLayer.Repository;
using Carola.DtoLayer.Dtos.CarDtos;
using Carola.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.Abstract
{
    public interface ICarDal : IGenericDal<Car>
    {
        Task<List<Car>> GetAllCarsWithCategoryAsync();
        Task<List<CarListDto>> GetLast6CarsAsync();
        Task<List<CarListDto>> GetAvailableCarsAsync(int? locationId, DateTime? startDate, DateTime? endDate);
        Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate);

    }
}
