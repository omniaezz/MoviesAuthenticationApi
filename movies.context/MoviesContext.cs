using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using movies.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies.context
{
    public class MoviesContext :IdentityDbContext<User>
    {
        public MoviesContext(DbContextOptions options):base(options) { }
     
    }
}
