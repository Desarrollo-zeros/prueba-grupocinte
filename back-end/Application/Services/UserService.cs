using Application.Base;
using Domain.Entities;
using Domain.Interface;
using Domain.Interface.Domain.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Services
{
    public class UserService : Service<User>
    {
        public UserService(IUnitOfWork unitOfWork, IRepository<User> repository) : base(unitOfWork, repository)
        {
        }

        public override User Create(User user)
        {
            if (user == null) throw new Exception("Entity empty or null");
            if (_repository.FindBy(x => x.Email == user.Email).Count() > 0)
            {
                return null;
            }

            user.Password = new PasswordHasher().HashPassword(user.Password);
            
            return base.Create(user);
        }

        public bool ResponsePassword(User user, User entity)
        {
            var verify = new PasswordHasher().VerifyHashedPassword(entity.Password, user.Password);

            if (verify == PasswordVerificationResult.Success)
            {
                return true;
            }

            return false;
        }

        public User GetUser(User user, User entity = null)
        {
            entity = (_repository.FindBy(x => x.Email == user.Email)).FirstOrDefault();
            if(entity == null) { return null; }

            return ResponsePassword(user, entity) ? entity : null;
        }

    }
}
