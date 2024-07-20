using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using movies.application.Contracts;
using movies.dtos;
using movies.dtos.Result;
using movies.models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace movies.application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepository,UserManager<User> userManager,IMapper mapper,IConfiguration config)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
        }
        public async Task<ResultView<UserDto>> RegistersUserAsync(UserDto userDto)
        {
            var ExistingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (ExistingUser is not null) 
            {
                return new ResultView<UserDto>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "User Is Already Exist"
                };
            }

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                UserName= userDto.FirstName +"."+userDto.LastName,
                Age = userDto.Age,
            };
            var result = await _userManager.CreateAsync(user,userDto.Password);

            if (result.Succeeded)
            {
                return new ResultView<UserDto>
                {
                    Entity = _mapper.Map<UserDto>(user),
                    IsSuccess = true,
                    Message = "User Register Successfully"
                };
            }

            return new ResultView<UserDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Faild To Register"
            };
        }


        public async Task<ResultDataList<UserDto>> GetAllUsersAsync()
        {
            var Users = (await _userRepository.GetAllAsync()).ToList();
            if (Users == null)
            {
                return new ResultDataList<UserDto>
                {
                    Entites = null,
                    Count = 0
                };
            }

            return new ResultDataList<UserDto>
            {
                Entites = _mapper.Map<List<UserDto>>(Users),
                Count = Users.Count()
            };
        }


        public async Task<ResultView<UserTokenDto>> loginUserAsync(LoginUserDto userDto)
        {
            var ExistingUser = await _userManager.FindByEmailAsync(userDto.Email);

            if (ExistingUser is null)
            {
                return new ResultView<UserTokenDto>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "Email isn't Exist"
                };
            }
            else
            {
                var ExistingPassword = await _userManager.CheckPasswordAsync(ExistingUser, userDto.Password);
                if(ExistingPassword)
                {
                    var Claims = new List<Claim>
                     {
                        new Claim("Email", ExistingUser.Email),
                        new Claim("UserId", ExistingUser.Id.ToString()), // Adding user ID as a claim
                        new Claim("FirstName", ExistingUser.FirstName), // Example: Adding first name
                        new Claim("LastName", ExistingUser.LastName), // Example: Adding last name
                        new Claim("Age", ExistingUser.Age.ToString()) // Example: Adding last name
                     };

                    SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));

                    JwtSecurityToken token = new JwtSecurityToken(
                       issuer: _config["JWT:Issuer"],
                       audience: _config["JWT:Audiance"],
                       claims:Claims,
                       expires: DateTime.Now.AddDays(3),
                       signingCredentials : new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                     );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    var UserToken = new UserTokenDto
                    {
                        Id = ExistingUser.Id,
                        Email = ExistingUser.Email,
                        token = tokenString,
                    };

                    return new ResultView<UserTokenDto>
                    {
                        Entity = UserToken,
                        IsSuccess = true,
                        Message = "Login successful"
                    };
                }
                else
                {
                    return new ResultView<UserTokenDto>
                    {
                        Entity = null,
                        IsSuccess = false,
                        Message = "Password Isn't Correct"//password
                    };
                }
            }


        }
    }
}
