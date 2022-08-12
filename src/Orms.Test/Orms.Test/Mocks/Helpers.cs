using Orms.Domain.Entities;
using Orms.Domain.Enuns;

namespace Orms.Tests.Mocks
{
    /// <summary>
    /// Helpers for tests
    /// </summary>
    static class Helpers
    {
        private static List<Post>? _posts;

        /// <summary>
        /// Return 500 posts
        /// </summary>
        /// <returns></returns>
        public static List<Post> GetListPost(int filter = default)
        {

            if (_posts == null)
            {
                _posts = new List<Post>();

                for (int i = 1; i <= 500; i++)
                {
                    _posts.Add(new Post
                    {
                        PostId = i,
                        Comment = $"Teste-Post-Comment {i}",
                        Content = "Teste-Post-Content 1",
                        DateInsert = DateTime.Now.AddDays(-1),
                        Type = GetType(i % 3),
                        UserId = i % 10
                    });
                }

            }
            if (filter != default)
                return _posts.Take(filter).ToList();

            return _posts;
        }


        /// <summary>
        /// Return the type of post
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static PostType GetType(int index) => index switch
        {
            0 => PostType.Quote,
            1 => PostType.Repost,
            2 => PostType.Original,
            _ => PostType.Original
        };

        /// <summary>
        /// Return 10 users
        /// </summary>
        /// <returns></returns>
        public static List<User> GetUsers()
        {
            var users = new List<User>();

            for (int i = 1; i <= 10; i++)
            {
                users.Add(
                new User
                {
                    Name = $"Edson {i}",
                    Surname = $"Catugy {i}",
                    DateInsert = DateTime.Now.AddDays(-1),
                    UserID = i,
                    User_Post = Helpers.GetListPost().Where(p => p.UserId == i).ToList(),
                });
            }

            return users;
        }

    }
}
