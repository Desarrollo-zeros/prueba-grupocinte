using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Login
    {

        [Required]
        [MaxLength(10)]
        [MinLength(8)]
        public string IdentityNumber { set; get; }

        [Required]
        public int Type { set; get; }

        [Required]
        [MaxLength(20)]
        [MinLength(4)]
        public string Password { set; get; }

        [JsonIgnore]
        public UserModel User { set; get; }
    }
}
