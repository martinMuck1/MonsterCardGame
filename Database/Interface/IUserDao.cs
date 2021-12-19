﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public interface IUserDao
    {
        public int CreateUser(UserModel user);
        public int LoginUser(UserModel user);
    }
}
