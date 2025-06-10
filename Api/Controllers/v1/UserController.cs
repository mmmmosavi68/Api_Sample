using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using Common.Exceptions;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Services;
using WebFramework.Api;
using WebFramework.Filters;
using Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Cors;

namespace Api.Controllers.v1
{
    [ApiVersion("1")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository
            , ILogger<UserController> logger
            , IJwtService jwtService
            , UserManager<User> userManager
            , RoleManager<Role> roleManager
            , SignInManager<User> signInManager
            , IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _jwtService = jwtService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            //_logger.LogError("Call Get List User Action");

            //use Common IdentityExtentions
            var username = HttpContext.User.Identity.GetUserName();
            username = HttpContext.User.Identity.Name;
            var userid = HttpContext.User.Identity.GetUserId();
            var useridint = HttpContext.User.Identity.GetUserId<int>();
            var MobilePhone = HttpContext.User.Identity.FindFirstValue(ClaimTypes.MobilePhone);
            var Role = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);
            var Token = HttpContext.User.Identity.FindFirstValue("Token");

            //var users = await _userRepository.TableNoTracking.ToListAsync(cancellationToken);
            //return Ok(users);

            var list = _userRepository.TableNoTracking.ProjectTo<UserDto>(_mapper.ConfigurationProvider);

            //var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(list);

        }

        [HttpGet("{id:int}")]
        public virtual async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user2 = await _userManager.FindByIdAsync(id.ToString());

            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();

            await _userManager.UpdateSecurityStampAsync(user);
            //await _userRepository.UpdateSecuirtyStampAsync(user, cancellationToken);
            return user;
        }


        /// <summary>
        /// This method genarate Jwt Token
        /// </summary>
        /// <param name="tokenRequest">The information of token request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public virtual async Task<ActionResult> Token([FromForm] TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            if (!tokenRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
                throw new Exception("OAuth flow is not password.");

            //var user = await userRepository.GetByUserAndPass(username, password, cancellationToken);
            var user = await _userManager.FindByNameAsync(tokenRequest.username);
            if (user == null)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, tokenRequest.password);
            if (!isPasswordValid)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");


            //if (user == null)
            //    throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var jwt = await _jwtService.GenerateAsync(user);
            return new JsonResult(jwt);

        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            _logger.LogError("متد Create فراخوانی شد");
            //HttpContext.RiseError(new Exception("متد Create فراخوانی شد"));
            //var exists = await userRepository.TableNoTracking.AnyAsync(p => p.UserName == userDto.UserName);
            //if (exists)
            //    return BadRequest("نام کاربری تکراری است");

            var user = new User
            {
                Age = userDto.Age,
                FullName = userDto.FullName,
                Gender = userDto.Gender,
                UserName = userDto.UserName,
                Email = userDto.Email

            };
            //await _userRepository.AddAsync(user, userDto.Password, cancellationToken);  //Sellf Code
            var result = await _userManager.CreateAsync(user, userDto.Password);  // Identity Code

            var result2 = await _roleManager.CreateAsync(new Role
            {
                Name = "Admin2",
                Description = "admin role"
            });


            var result3 = await _userManager.AddToRoleAsync(user, "Admin2");

            return user;
        }

        [HttpPut]
        public virtual async Task<ApiResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await _userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;

            await _userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            await _userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }

    }
}
