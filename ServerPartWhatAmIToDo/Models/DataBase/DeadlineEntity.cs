using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerPartWhatAmIToDo.Models.DataBase
{
    [Table("deadlines", Schema = "public")]
    public class DeadlineEntity
    {
        [Key]
        [Column("deadline_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeadlineId { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("goal_id")]
        public int? GoalId { get; set; }

        [Column("step_id")]
        public int? StepId { get; set; }

        [Required]
        [Column("deadline")]
        public DateTime Deadline { get; set; }
    }
}