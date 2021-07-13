using MyFace.Models.Database;
using System.Collections.Generic;
using System.Linq;

namespace MyFace.Data
{
    public static class SamplePosts
    {
        public static int NumberOfPosts => 200;

        public static IEnumerable<Post> GetPosts() => Enumerable.Range(0, NumberOfPosts).Select(CreateRandomPost);

        private static Post CreateRandomPost(int index)
        {
            return new Post
            {
                Message = MessageGenerator.GetMessage(index),
                UserId = RandomNumberGenerator.GetUserId(),
                ImageUrl = ImageGenerator.GetPostImage(index),
                PostedAt = DateGenerator.GetPostDate()
            };
        }
    }
}