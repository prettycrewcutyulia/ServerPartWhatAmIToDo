using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerPartWhatAmIToDo.Models.DataBase
{
    [Table("goals", Schema = "public")]
    public class GoalEntity
    {
        [Key]
        [Column("goal_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalId { get; set; }

        [Required]
        [Column("id_user")]
        public int UserId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        // Arrays in PostgreSQL can be mapped to List<int>
        [Column("id_filters")]
        public int[]? IdFilters { get; set; }

        [Column("id_steps")]
        public int[]? IdSteps { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("deadline")]
        public DateTime? Deadline { get; set; }
    }
}