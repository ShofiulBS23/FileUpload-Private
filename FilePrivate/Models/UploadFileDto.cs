﻿using System.ComponentModel.DataAnnotations;

namespace FilePrivate.Models
{
    public class UploadFileDto
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public string ISIN { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string DocGroup { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        [Required]
        public string DocName { get; set; }

        [Required]
        public string DocExt { get; set; }
        [Required]
        public string File { get; set; }
    }
}
