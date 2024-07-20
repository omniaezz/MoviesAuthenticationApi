using Microsoft.EntityFrameworkCore;
using movies.application.Contracts;
using movies.context;
using movies.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies.infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly MoviesContext _moviesContext;
        private readonly DbSet<User> _entities;

        public UserRepository(MoviesContext moviesContext) {
            _moviesContext = moviesContext;
            _entities = _moviesContext.Set<User>();
        }
        public async Task<User> CreateAsync(User user)
        {
            return (await _entities.AddAsync(user)).Entity;
        }

        public Task<User> DeleteAsync(User user)
        {
            return Task.FromResult((_entities.Remove(user)).Entity);
        }

        public Task<IQueryable<User>> GetAllAsync()
        {
            return Task.FromResult(_entities.Select(u => u));
        }

        public async Task<User> GetByEmailAsync(string UserEmail)
        {
            return await _entities.FirstOrDefaultAsync(u => u.Email == UserEmail);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _moviesContext.SaveChangesAsync();
        }

        public Task<User> UpdateAsync(User user)
        {
            return Task.FromResult((_entities.Update(user)).Entity);
        }
    }
}
