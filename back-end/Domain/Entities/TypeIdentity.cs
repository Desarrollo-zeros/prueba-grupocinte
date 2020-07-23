using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    [Table("type_identity")]
    public class TypeIdentity : Entity<int>
    {

        [Column("type", TypeName = "varchar(20)", Order = 2)]
        public string Type { set; get; }

        [Column("name", TypeName = "varchar(20)", Order = 2)]
        public string Name { set; get; }

        [JsonIgnore]
        public ICollection<User> Users { set; get; }
  
    }
}
