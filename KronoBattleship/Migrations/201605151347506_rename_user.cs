namespace KronoBattleship.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameUser : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "Player", newName: "UserName");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "UserName", newName: "Player");
        }
    }
}
