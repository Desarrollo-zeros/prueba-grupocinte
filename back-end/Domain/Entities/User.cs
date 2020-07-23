using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    [Table("user")]
    
    public class User : Entity<int>
    {
        [Required]
        [Column("name", TypeName = "varchar(20)", Order = 2)]
        [MaxLength(20)]
        [MinLength(2)]
        public string Name { set; get; }

        [Required]
        [Column("last_name", TypeName = "varchar(30)", Order = 3)]
        [MaxLength(30)]
        [MinLength(2)]
        public string LastName { set; get; }

        [Required]
        [Column("type_id", TypeName = "integer", Order = 4)]
        [ForeignKey("type_id")]
        public int TypeId { set; get; }

        [Required]
        [Column("identity_number", TypeName = "varchar(11)", Order = 5)]
        [MaxLength(10)]
        [MinLength(8)]
        public string IdentityNumber { set; get; }


        [Required]
        [Column("email", TypeName = "varchar(255)", Order = 6)]
        [MaxLength(20)]
        [MinLength(4)]
        public string Email { set; get; }

        
        [Column("password", TypeName = "varchar(255)", Order = 7)]
        [MaxLength(20)]
        [MinLength(4)]
      
        public string Password { set; get; }

        public virtual TypeIdentity TypeIdentity { set; get; }
    }



}
