using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using AutoMapper;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;

namespace Api.Controllers.v2
{
    [ApiVersion("2")]
    public class PostsController : v1.PostsController
    {
        public PostsController(IRepository<Post> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public override Task<ApiResult<PostSelectDto>> Create(PostDto dto, CancellationToken cancellationToken)
        {
            return base.Create(dto, cancellationToken);
        }

        [NonAction]
        public override Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }

        public override async Task<ActionResult<List<PostSelectDto>>> Get(CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<PostSelectDto>
            {
                new PostSelectDto
                {
                    FullTitle="FullTitle",
                    AuthorFullName="AuthorFullName",
                    CategoryName="CategoryName",
                    Description="Description",
                    Title="Title",
                }
            });
        }

        public override Task<ApiResult<PostSelectDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            return base.Get(id, cancellationToken);
        }

        [HttpGet("TestPost")]
        public ActionResult TestPost()
        {
            return Content("this is test");
        }

        public override Task<ApiResult<PostSelectDto>> Update(Guid id, PostDto dto, CancellationToken cancellationToken)
        {
            return base.Update(id, dto, cancellationToken);
        }
    }
}
