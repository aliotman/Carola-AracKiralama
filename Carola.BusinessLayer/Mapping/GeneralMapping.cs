using AutoMapper;
using Carola.DtoLayer.Dtos.CustomerDtos;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Customer, ResultCustomerDto>()
                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(src => (src.FirstName + " " + src.LastName).Trim()))
                .ForMember(dest => dest.Initials,
                           opt => opt.MapFrom(src =>
                               (string.IsNullOrEmpty(src.FirstName) ? "" : src.FirstName.Substring(0, 1)) +
                               (string.IsNullOrEmpty(src.LastName) ? "" : src.LastName.Substring(0, 1))));

            CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<Customer, GetCustomerByIdDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();
        }
    }
}
