using AutoMapper;
using FilePrivate.DbModels;
using FilePrivate.Models;

namespace FilePrivate.MappingProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UploadFileDto, Document>()
                .ForMember(m => m.ClientId, opt => opt.MapFrom(x => x.Clientid))
                .ForMember(m => m.ISIN, opt => opt.MapFrom(x => x.ISIN))
                .ForMember(m => m.Language, opt => opt.MapFrom(x => x.Lang))
                .ForMember(m => m.DocType, opt => opt.MapFrom(x => x.Type));

            CreateMap<Document, UploadFileDto>()
                .ForMember(m => m.ISIN, opt => opt.MapFrom(x => x.ISIN))
                .ForMember(m => m.Clientid, opt => opt.MapFrom(x => x.ClientId))
                .ForMember(m => m.Lang, opt => opt.MapFrom(x => x.Language))
                .ForMember(m => m.Type, opt => opt.MapFrom(x => x.DocType));
            
        }
    }
}
