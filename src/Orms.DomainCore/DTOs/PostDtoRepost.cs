using Orms.Domain.Enuns;

namespace Orms.Domain.DTOs
{
    public record PostDtoRepost(PostType Type, string? Content, string? Comment, int UserId, int PostId) : PostDto(Type, Content, UserId);
}
