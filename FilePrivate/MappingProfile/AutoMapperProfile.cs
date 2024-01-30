using AutoMapper;
using FilePrivate.DbModels;
using FilePrivate.Models;

namespace FilePrivate.MappingProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UploadFileDto, Document>();
            CreateMap<Document, UploadFileDto>();
        }
    }
}
