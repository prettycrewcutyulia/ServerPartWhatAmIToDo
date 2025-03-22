using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerPartWhatAmIToDo.Models.DataBase
{
    [Table("reminders", Schema = "public")]
    public class ReminderEntity
    {
        [Key]
        [Column("reminder_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReminderId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("days_count")]
        public int DaysCount { get; set; }
    }
}