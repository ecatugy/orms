using Orms.Domain.Enuns;

namespace Orms.Domain.DTOs
{
    public record PostDto(PostType Type, string? Content, int UserId);
}
