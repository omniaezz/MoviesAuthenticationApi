using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using movies.application.Services;
using movies.dtos;

namespace movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            
            if (ModelState.IsValid) 
            {
               var result = await _userService.RegistersUserAsync(userDto);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }

            return BadRequest(ModelState);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if (ModelState.IsValid) 
            {
               var result = await _userService.loginUserAsync(userDto);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }

            return BadRequest(ModelState);
        }


        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers() 
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.GetAllUsersAsync();
                if (result.Entites is not null)
                {
                    return Ok(result.Entites);
                }
                return BadRequest(result);
            }

            return BadRequest(ModelState);

        }

    }
}
