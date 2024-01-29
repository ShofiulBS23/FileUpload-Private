using System.ComponentModel.DataAnnotations;

namespace FilePrivate.Models
{
    public class UploadFileDto : GetFileDto
    {
        [Required]
        public string File { get; set; }
    }
}
