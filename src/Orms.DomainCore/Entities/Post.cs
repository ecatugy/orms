using Orms.Domain.Enuns;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orms.Domain.Entities
{
    /// <summary>
    /// Entity of posts
    /// </summary>
    public class Post
    { 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public PostType Type { get; set; }

        public DateTime DateInsert { get; set; } = default;

        [MaxLength(777)]
        public string? Content { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public string? Comment { get; set; }
    }
}
