namespace KronoBattleship.Migrations
{
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KronoBattleship.Datalayer.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(KronoBattleship.Datalayer.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("123456");
            new List<User>
            {
                new User { UserName = "Krono", Email = "krono@gmail.com", PasswordHash = password, SecurityStamp = Guid.NewGuid().ToString() },
                new User { UserName = "Ito", Email = "Ito@gmail.com", PasswordHash = password, SecurityStamp = Guid.NewGuid().ToString() },
                new User { UserName = "Lexy", Email = "Lexy@gmail.com", PasswordHash = password, SecurityStamp = Guid.NewGuid().ToString() },
                new User { UserName = "Yolka", Email = "Yolka@gmail.com", PasswordHash = password, SecurityStamp = Guid.NewGuid().ToString() },
                new User { UserName = "Titania", Email = "titania@gmail.com", PasswordHash = password, SecurityStamp = Guid.NewGuid().ToString() },
                new User { UserName = "Rox", Email = "rox@gmail.com", PasswordHash = password, SecurityStamp = Guid.NewGuid().ToString() },
            }.ForEach(u => context.Users.AddOrUpdate(u));

        }
    }
}
