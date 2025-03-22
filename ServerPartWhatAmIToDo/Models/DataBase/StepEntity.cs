using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerPartWhatAmIToDo.Models.DataBase
{
    [Table("steps", Schema = "public")]
    public class StepEntity
    {
        [Key]
        [Column("step_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StepId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("is_completed")]
        public bool? IsCompleted { get; set; } = false;

        [Column("deadline")]
        public DateTime? Deadline { get; set; }
    }
}