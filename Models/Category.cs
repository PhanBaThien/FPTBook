using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTBook_v3.Models
{
    public class Category
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Required]
            public int cate_Id { get; set; }
            [Required]
            public string cate_Name { get; set; }
            [Required]
            public string cate_Description { get; set; }
        [Required]
        public string? cate_Status { get; set; }

            public virtual ICollection<Book>? Books { get; set; }
     }
}
