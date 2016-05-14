using KronoBattleship.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace KronoBattleship.Datalayer
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("Player");

            //Property(u => u.Id)
            //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(u => u.Email)
                .IsRequired()
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(
                        new IndexAttribute("index_users_on_email")
                        ));
        }
    }
}