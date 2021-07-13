using MyFace.Models.Database;
using System;

namespace MyFace.Data
{
    public class RandomNumberGenerator
    {
        private static readonly Random _random = new Random();

        public static int GetUserId() => _random.Next(1, SampleUsers.NumberOfUsers + 1);

        public static int GetPostId() => _random.Next(1, SamplePosts.NumberOfPosts + 1);

        public static InteractionType GetInteractionType() => _random.Next(0, 2) == 0 ? InteractionType.LIKE : InteractionType.DISLIKE;
    }
}