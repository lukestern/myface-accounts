﻿using MyFace.Models.Database;
using System;

namespace MyFace.Models.Response
{
    public class PostResponse
    {
        private readonly Post _post;
        private readonly int _userId;

        public PostResponse(Post post)
        {
            _post = post;
            _userId = post.UserId;
        }
        
        public PostResponse(Post post, int userId)
        {
            _post = post;
            _userId = userId;
        }

        public int Id => _post.Id;
        public string Message => _post.Message;
        public string ImageUrl => _post.ImageUrl;
        public DateTime PostedAt => _post.PostedAt;
        public int UserId => _userId;
    }
}