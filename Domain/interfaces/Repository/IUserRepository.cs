﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.interfaces.Repository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetByIdWithToken(int userid);
        Task<User> GetByEmailWithToken(string email);
    }
}
