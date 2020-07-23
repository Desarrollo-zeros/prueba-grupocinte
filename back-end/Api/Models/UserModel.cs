using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class UserModel
    {

        public int Id { set; get; }

     
        public string Name { set; get; }

       
        public string LastName { set; get; }

       
        public int TypeId { set; get; }

       
        public string IdentityNumber { set; get; }


        
        public string Email { set; get; }

        public string Token { set; get; }

        public TypeIdentity TypeIdentity { set; get; }

       

    }
}
