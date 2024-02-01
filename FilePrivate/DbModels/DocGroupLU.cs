using System.ComponentModel.DataAnnotations;

namespace FilePrivate.DbModels
{
    public class DocGroupLU
    {
        [Key]
        public int DocTypeID { get; set; }

        [Required]
        [MaxLength(20)]
        public string DocGroup { get; set; }

    }
}
