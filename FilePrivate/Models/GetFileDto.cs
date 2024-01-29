using System.ComponentModel.DataAnnotations;

namespace FilePrivate.Models
{
    public class GetFileDto
    {
        [Required]
        public string Clientid { get; set; }
        [Required]
        public string ISIN { get; set; }
        [Required]
        public string Lang { get; set; }
        [Required]
        public string Type { get; set; }
        public bool? save { get; set; }
    }
}
