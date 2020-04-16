using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BinHamranAdminPanel.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        public int DbBranchID { get; set; }
        
        [Column(TypeName ="nvarchar(100)")]
        public string BRN_AR_NAME { get; set; }
        public string BRN_EN_NAME { get; set; }
        [ForeignKey("DatabaseNameId")]
        public int DatabaseNameId { get; set; }

        public DataBaseName DataBase { get; set; }

    }
}