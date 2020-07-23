using Api.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserModel = Api.Models.UserModel;


namespace Api.Interface
{
    public interface IUserService 
    {
        public UserModel Authenticate(Login user);

       

    }
}
