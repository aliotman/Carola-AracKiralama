using Carola.DtoLayer.Dtos.CarDtos;
using Carola.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Abstract
{
    public interface ICarService: IGenericService<Car>
    {
        Task<List<Car>> TGetAllCarsWithCategoryAsync();
        Task<List<CarListDto>> TGetLast6CarsAsync();
        Task<List<CarListDto>> TGetAvailableCarsAsync(int? locationId, DateTime? startDate, DateTime? endDate);
        Task<bool> TIsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate);
    }
}
