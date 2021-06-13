using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext ctx;

        public UserRepository(ApplicationDbContext ctx)
        {
            this.ctx = ctx;
        }
        public void deleteCustomer(string id)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> getAllCustomer()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetById(string id)
        {
            ApplicationUser user = ctx.Users.Find(id);

            return user;
        }
    }
}
