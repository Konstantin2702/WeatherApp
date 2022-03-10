using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class SavedFiles
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string FileName { get; set; }
    }
}
