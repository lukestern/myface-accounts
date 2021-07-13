﻿using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _users;

        public PostsController(IPostsRepo posts, IUsersRepo users)
        {
            _posts = posts;
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<PostListResponse> Search([FromQuery] PostSearchRequest searchRequest)
        {
            if (!_users.HasAccess(HttpContext.Request.Headers["Authorization"]))
            {
                return new NotFoundResult();
            }

            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);
        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById([FromRoute] int id)
        {
            if (!_users.HasAccess(HttpContext.Request.Headers["Authorization"]))
            {
                return new NotFoundResult();
            }

            var post = _posts.GetById(id);
            return new PostResponse(post);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePostRequest newPost)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            if (!_users.HasAccess(authHeader))
            {
                return new NotFoundResult();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _users.GetByUsername(_users.GetUsernameFromAuthHeader(authHeader));

            var post = _posts.Create(newPost, user.Id);

            var url = Url.Action("GetById", new {id = post.Id});
            var postResponse = new PostResponse(post);
            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update([FromRoute] int id, [FromBody] UpdatePostRequest update)
        {
            if (!_users.HasAccess(HttpContext.Request.Headers["Authorization"]))
            {
                return new NotFoundResult();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = _posts.Update(id, update);
            return new PostResponse(post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (!_users.HasAccess(HttpContext.Request.Headers["Authorization"]))
            {
                return new NotFoundResult();
            }

            _posts.Delete(id);
            return Ok();
        }
    }
}