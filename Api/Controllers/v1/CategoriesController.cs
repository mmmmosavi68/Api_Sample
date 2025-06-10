using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    [ApiVersion("1")]
    public class CategoriesController : CrudController<CategoryDto, Category>
    {
        public CategoriesController(IRepository<Category> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual Task<ActionResult<List<CategoryDto>>> test(CancellationToken cancellationToken)
        {
            return base.Get(cancellationToken);
        }
    }
}
