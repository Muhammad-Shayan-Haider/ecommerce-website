using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserRepository
    {
        ApplicationUser GetById(string id);
        void deleteCustomer(string id);
    }
}
