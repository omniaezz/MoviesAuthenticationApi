using movies.dtos;
using movies.dtos.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies.application.Services
{
    public interface IUserService
    {
        Task<ResultView<UserDto>> RegistersUserAsync(UserDto userDto);
        Task<ResultView<UserTokenDto>> loginUserAsync(LoginUserDto userDto);
        Task<ResultDataList<UserDto>> GetAllUsersAsync();
    }
}
