using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BinHamranAdminPanel.Models
{
    public class DataBaseName
    {
        [Key]
        public int DatabaseNameId { get; set; }
        [Column(TypeName ="nvarchar(100)")]
        public string DbArabicName { get; set; }
        public string DbEnglishName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(128)")]
        public string DbName { get; set; }
        
        public ICollection<Branch> Branches { get; set; }

    }
}
