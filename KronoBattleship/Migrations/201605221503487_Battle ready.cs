namespace KronoBattleship.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Battleready : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Battles", "ActivePlayer", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Battles", "ActivePlayer", c => c.Boolean(nullable: false));
        }
    }
}
