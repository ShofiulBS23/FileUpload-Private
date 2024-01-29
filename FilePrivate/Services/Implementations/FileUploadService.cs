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
                    var docType = await context.DocTypes.AsNoTracking().FirstOrDefaultAsync(x => x.DocType == dto.Type);

                    if (docType.IsNullOrEmpty()) {
                        return null;
                    }

                    string fileExtension = GetFileExtention(dto.File);

                    if (fileExtension.IsNullOrEmpty()) {
                        return null;
                    }

                    //File name: ISIN+lang+docType
                    string fileName = $"{dto.ISIN}_{dto.Lang}_{dto.Type}";

                    string folderName = $"{_baseFolder}\\{dto.Clientid}_{dto.Type}";

                    if (!Directory.Exists(folderName)) {
                        Directory.CreateDirectory(folderName);
                    }

                    bool saved = await SaveFileFromBase64String(folderName, $"{fileName}.{fileExtension}", dto.File);

                    if (!saved) {
                        return null;
                    }

                    var existingRecord = await context.Documents.AsNoTracking()
                                                                .FirstOrDefaultAsync(
                                                                    x => x.DocName == fileName &&
                                                                    x.ClientId.ToString() == dto.Clientid &&
                                                                    x.DocExt == fileExtension
                                                                );

                    if (existingRecord.IsNullOrEmpty()) {
                        var dbInstace = _mapper.Map<Document>(dto);
                        dbInstace.DocDate = DateTime.Now;
                        dbInstace.DocExt = fileExtension;
                        dbInstace.DocName = fileName;


                        var dbInstance = await context.Documents.AddAsync(dbInstace);
                        await context.SaveChangesAsync();
                        return _mapper.Map<UploadFileDto>(dbInstace);
                    }

                    return _mapper.Map<UploadFileDto>(existingRecord);
                }
               
            }catch(Exception ex) {
                return null;
            }
            
        }

        private async Task<bool> SaveFileFromBase64String(string targetFolder,string fileName,string base64String)
        {
            try {
                // Create the full file path
                string filePath = Path.Combine(targetFolder, fileName);


                string[] parts = base64String.Split(',');

                // Convert base64 string to bytes
                byte[] fileBytes = Convert.FromBase64String(parts[1]);

                // Write the bytes to the file
                // FileMode.Create => create if not exist, otherwise replace
                await using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
                    await fs.WriteAsync(fileBytes, 0, fileBytes.Length);
                }

                return true;
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
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
