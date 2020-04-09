using System;
using System.Collections.Generic;

namespace IService
{
    public interface IUserService
    {
        List<Models.User> GetList();

        Models.User GetID(int id);  
    }
}
