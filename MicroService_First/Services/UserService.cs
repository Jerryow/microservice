using IService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class UserService : IService.IUserService
    {
        private static List<IService.Models.User> _users = new List<User>()
        {
        new User()
            {
                PKID=1,
                Name = "1",
                CreatedTime = DateTime.Now
            },

        new User()
        {
            PKID = 2,
                Name = "2",
                CreatedTime = DateTime.Now
            },

            new User()
        {
            PKID = 3,
                Name = "3",
                CreatedTime = DateTime.Now
            }
        };

        public User GetID(int id)
        {
            return _users.FirstOrDefault(x => x.PKID == id);
        }

        public List<User> GetList()
        {
            return _users;
        }
    }
}
