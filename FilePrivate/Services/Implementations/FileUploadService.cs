using AutoMapper;
using FilePrivate.Data;
using FilePrivate.DbModels;
using FilePrivate.Extensions;
using FilePrivate.Models;
using FilePrivate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilePrivate.Services.Implementations
{
    public class FileUploadService : IFileUploadService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly string _baseFolder;
        public FileUploadService(
            ApplicationDbContext context,
            IMapper mapper,
            IConfiguration config
        )
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _baseFolder = _config.GetValue<string>("FileStorage:FileLocation") ?? String.Empty;
        }
        public async Task<UploadFileDto> UploadFileAsync(UploadFileDto dto)
        {
            try {
                using (var context = _context) {
                    var docType = await context.DocTypes.AsNoTracking().FirstOrDefaultAsync(x => x.DocType == dto.DocType);

                    if (docType.IsNullOrEmpty()) {
                        throw new Exception($"The document type[{dto.DocType}] is not supported!");
                    }

                    string folderName = $"{_baseFolder}\\{dto.ClientId}\\{dto.DocType}";

                    if (!Directory.Exists(folderName)) {
                        Directory.CreateDirectory(folderName);
                    }

                    dto.DocExt = dto.DocExt.ToLower();

                    string fileName = $"{dto.ISIN}_{dto.Language}_{dto.DocType}.{dto.DocExt}";

                    bool saved = await SaveFileFromBase64String(folderName, fileName, dto.File);

                    if (!saved) {
                        throw new Exception("The attempt to save the file in local storage was unsuccessful!");
                    }

                    var existingRecord = await context.Documents.FirstOrDefaultAsync(
                                                                    x => x.DocName == dto.DocName &&
                                                                    x.ClientId == dto.ClientId &&
                                                                    x.DocExt == dto.DocExt &&
                                                                    x.ISIN == dto.ISIN &&
                                                                    x.DocType == dto.DocType &&
                                                                    x.Language == dto.Language
                                                                );

                    if (existingRecord.IsNullOrEmpty()) {
                        var dbInstace = _mapper.Map<Document>(dto);
                        dbInstace.DocDate = DateTime.Now;

                        var dbInstance = await context.Documents.AddAsync(dbInstace);
                        await context.SaveChangesAsync();
                        return _mapper.Map<UploadFileDto>(dbInstace);
                    } else {
                        existingRecord.DocDate = DateTime.Now;
                        context.Documents.Update(existingRecord);
                        await context.SaveChangesAsync();
                    }

                    return _mapper.Map<UploadFileDto>(existingRecord);
                }
               
            }catch(Exception ex) {
                throw ex;
            }
            
        }

        private async Task<bool> SaveFileFromBase64String(string targetFolder,string fileName,string base64String)
        {
            try {
                // Create the full file path
                string filePath = Path.Combine(targetFolder, fileName);

                byte[] fileBytes = Convert.FromBase64String(base64String);

                // FileMode.Create => create if not exist, otherwise replace
                await using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
                    await fs.WriteAsync(fileBytes, 0, fileBytes.Length);
                }

                return true;
            } catch (Exception ex) {
                throw ex;
            }
        }

        private string ConvertPdfToBase64(string path)
        {
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            return file;
        }

        private string GetFileExtention(string base64String)
        {
            string[] parts = base64String.Split(',');

            if (parts.Length < 2) {
                return String.Empty;
            }

            string mimeTypePart = parts[0];

            string[] mimeTypeParts = mimeTypePart.Split(';');

            if (mimeTypeParts.Length < 2) {
                return String.Empty;
            }

            string mimeType = mimeTypeParts[0];

            string[] mimeParts = mimeType.Split('/');

            if (mimeParts.Length < 2) {
                return String.Empty;
            }
            string fileExtension = mimeParts[1];

            return fileExtension;
        }
    }
}
