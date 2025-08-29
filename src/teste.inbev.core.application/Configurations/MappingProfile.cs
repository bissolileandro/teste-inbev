using System;
using AutoMapper;
using teste.inbev.core.domain.Models;
using teste.inbev.core.domain.Entities;

namespace teste.inbev.core.application.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {            
            CreateMap<Produto, ProdutoModel>().ReverseMap();
            CreateMap<Orcamento, OrcamentoResponseModel>().ReverseMap();
            CreateMap<Orcamento, OrcamentoRequestModel>().ReverseMap();
            CreateMap<OrcamentoItem, OrcamentoItemResponseModel>().ReverseMap();
            CreateMap<OrcamentoItem, OrcamentoItemRequestModel>().ReverseMap();
        }
    }
}
