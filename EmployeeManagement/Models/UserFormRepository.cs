using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class UserFormRepository : IUserFormRepository
    {
        private readonly AppDbContext appDbContext;

        public UserFormRepository(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }
        public async Task<User> Add(User user)
        {
            await appDbContext.UserForm.AddAsync(user);
            await appDbContext.SaveChangesAsync();
            return user;
        }
    }
}
