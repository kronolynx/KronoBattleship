namespace KronoBattleship.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Battles", "RowVersion", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Battles", "RowVersion");
        }
    }
}
