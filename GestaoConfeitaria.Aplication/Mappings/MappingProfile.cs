using AutoMapper;
using GestaoConfeitaria.Application.DTOs;
using GestaoConfeitaria.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestaoConfeitaria.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Venda
            CreateMap<Venda, VendaDto>().ReverseMap();
            CreateMap<VendaCreateDto, Venda>();

            // Material
            CreateMap<Material, MaterialDto>()
                .ForMember(dest => dest.CustoTotal, opt => opt.MapFrom(src => src.CustoTotal))
                .ReverseMap();

            CreateMap<MaterialCreateDto, Material>();

            // Gasto
            CreateMap<Gasto, GastoDto>().ReverseMap();
            CreateMap<GastoCreateDto, Gasto>();

            // User (caso precise no futuro)
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
