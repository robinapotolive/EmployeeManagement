using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public interface IUserFormRepository
    {
        Task<User> Add(User user);
    }
}
