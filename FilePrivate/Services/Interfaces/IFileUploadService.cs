using FilePrivate.Models;

namespace FilePrivate.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<UploadFileDto> UploadFileAsync(UploadFileDto dto);
    }
}
