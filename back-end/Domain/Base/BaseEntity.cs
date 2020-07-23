using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Base
{
    public abstract class BaseEntity
    {
        [Column("created_at", TypeName = "bigint", Order = 7)]
        [Required]
        //
        // Resumen:
        //    Fecha de creación del registro
        //
        // Devuelve:
        //    Fecha de creación del registro
        public long CreatedAt { set; get; }

        [Column("updated_at", TypeName = "bigint", Order = 8)]
        [DefaultValue((long)0)]
        //
        // Resumen:
        //    Última fecha de modificación del registro
        //
        // Devuelve:
        //     Última fecha de modificación del registro
        public long UpdatedAt { set; get; }
    }
    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        [Key]
        [Column("id", Order = 1, TypeName = "INTEGER")]
        //
        // Resumen:
        //    Identificador único del registro
        //
        // Devuelve:
        //     Identificador único del registro
        public virtual T Id { get; set; }
    }
}