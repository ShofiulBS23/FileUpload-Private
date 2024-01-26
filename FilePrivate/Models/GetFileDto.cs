﻿using System.ComponentModel.DataAnnotations;

namespace FilePrivate.Models
{
    public class GetFileDto
    {
        [Required]
        public string clientid { get; set; }
        public string Isin { get; set; }
        public string lang { get; set; }
        [Required]
        public string type { get; set; }
        public bool? save { get; set; }
    }
}