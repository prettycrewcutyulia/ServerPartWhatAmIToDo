using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerPartWhatAmIToDo.Models.DataBase
{
    [Table("filters", Schema = "public")]
    public class FilterEntity
    {
        [Key]
        [Column("filter_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilterId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("color")]
        public string? Color { get; set; }
    }
}