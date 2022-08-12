using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orms.Domain.Entities
{
    /// <summary>
    /// Entity of users
    /// </summary>
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        public DateTime DateInsert { get; set; }

        [MaxLength(14)]
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public virtual ICollection<Post>? User_Post { get; set; }
    }
}
