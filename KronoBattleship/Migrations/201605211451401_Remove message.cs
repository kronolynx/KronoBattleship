namespace KronoBattleship.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removemessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "Battle_BattleId", "dbo.Battles");
            DropForeignKey("dbo.Messages", "Player_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "Battle_BattleId" });
            DropIndex("dbo.Messages", new[] { "Player_Id" });
            DropPrimaryKey("dbo.Battles");
            AddColumn("dbo.Battles", "PlayerUserName", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Battles", "EnemyUserName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Battles", "ActivePlayer", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.Battles", new[] { "PlayerUserName", "EnemyUserName" });
            DropColumn("dbo.Battles", "BattleId");
            DropColumn("dbo.Battles", "PlayerId");
            DropColumn("dbo.Battles", "EnemyId");
            DropTable("dbo.Messages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        DateSent = c.DateTime(nullable: false),
                        Battle_BattleId = c.Int(),
                        Player_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MessageId);
            
            AddColumn("dbo.Battles", "EnemyId", c => c.Int(nullable: false));
            AddColumn("dbo.Battles", "PlayerId", c => c.Int(nullable: false));
            AddColumn("dbo.Battles", "BattleId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Battles");
            AlterColumn("dbo.Battles", "ActivePlayer", c => c.Int(nullable: false));
            DropColumn("dbo.Battles", "EnemyUserName");
            DropColumn("dbo.Battles", "PlayerUserName");
            AddPrimaryKey("dbo.Battles", "BattleId");
            CreateIndex("dbo.Messages", "Player_Id");
            CreateIndex("dbo.Messages", "Battle_BattleId");
            AddForeignKey("dbo.Messages", "Player_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "Battle_BattleId", "dbo.Battles", "BattleId");
        }
    }
}
