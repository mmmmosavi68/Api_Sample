using Api.Models;
using AutoMapper;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;

namespace Api.Controllers.v1
{
    public class PostsController : CrudController<PostDto, PostSelectDto, Post, Guid>
    {
        public PostsController(IRepository<Post> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
