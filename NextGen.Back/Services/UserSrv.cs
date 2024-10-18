using NextGen.Model;
using NextGen.Dal.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGen.Dal.Interfaces;

namespace NextGen.Back.Services
{
    public class UserSrv : IUserSrv
    {
        private readonly NextGenDbContext _context;

        public UserSrv(NextGenDbContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            user.Validate();
            // Code pour ajouter un Actualite
            _context.Users.Add(user);

            _context.SaveChanges();
        }

        public User GetUser(int id)
        {
            // Code pour récupérer un Actualite
            var user = _context.Users.Find(id);

            return user;
        }

        public List<User> GetAllUsers()
        {
            // Code pour récupérer tous les Actualites
            var users = _context.Users.ToList();

            return users;
        }
    }
}
