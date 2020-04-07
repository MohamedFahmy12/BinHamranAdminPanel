using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BinHamranAdminPanel.Models
{
  public class UserLog
  {
    [Key]
    public int UserLogID { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string PageName { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string OperationName { get; set; }

    public bool MobileView { get; set; }

    public DateTime OperationDate { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; }
  }
}
