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
            Property(m => m.UserName)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("index_username") { IsUnique = true }));

            //Property(u => u.Id)
            //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(m => m.Email)
                .IsRequired()
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(
                        new IndexAttribute("index_users_on_email")
                        ));
        }
    }
}