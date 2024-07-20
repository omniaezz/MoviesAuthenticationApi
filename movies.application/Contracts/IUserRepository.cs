using movies.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies.application.Contracts
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<User> DeleteAsync(User user);
        Task<User> GetByEmailAsync(string UserEmail);
        Task<IQueryable<User>> GetAllAsync();
        Task<int> SaveChangesAsync(); //return number of effected rows
    }
}
