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

                    //string fileExtension = GetFileExtention(dto.File);


                    //if (fileExtension.IsNullOrEmpty()) {
                    //    return null;
                    //}

                    //File name: ISIN+lang+docType
                    string fileName = $"{dto.ISIN}_{dto.Language}_{dto.DocType}";

                    string folderName = $"{_baseFolder}\\{dto.ClientId}\\{dto.DocType}";

                    if (!Directory.Exists(folderName)) {
                        Directory.CreateDirectory(folderName);
                    }

                    dto.DocExt = dto.DocExt.ToLower();

                    bool saved = await SaveFileFromBase64String(folderName, $"{fileName}.{dto.DocExt}", dto.File);

                    if (!saved) {
                        throw new Exception("The attempt to save the file in local storage was unsuccessful!");
                    }

                    var existingRecord = await context.Documents.AsNoTracking()
                                                                .FirstOrDefaultAsync(
                                                                    x => x.DocName == fileName &&
                                                                    x.ClientId == dto.ClientId &&
                                                                    x.DocExt == dto.DocExt
                                                                );

                    if (existingRecord.IsNullOrEmpty()) {
                        var dbInstace = _mapper.Map<Document>(dto);
                        dbInstace.DocName = fileName;


                        var dbInstance = await context.Documents.AddAsync(dbInstace);
                        await context.SaveChangesAsync();
                        return _mapper.Map<UploadFileDto>(dbInstace);
                    }

                    var dto1 = _mapper.Map<UploadFileDto>(existingRecord);
                    return dto1;
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


                //string[] parts = base64String.Split(',');

                //// Convert base64 string to bytes
                //byte[] fileBytes = Convert.FromBase64String(parts[1]);

                byte[] fileBytes = Convert.FromBase64String(base64String);

                // Write the bytes to the file
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
