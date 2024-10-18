using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGen.Model;

namespace NextGen.Dal.Interfaces
{
    public interface IUserSrv
    {
        void AddUser(User user);

        User GetUser(int id);

        List<User> GetAllUsers();
    }
}
