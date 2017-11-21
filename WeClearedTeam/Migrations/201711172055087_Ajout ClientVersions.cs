namespace WeClearedTeam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutClientVersions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        Description = c.String(),
                        VersionPath = c.String(),
                        Version = c.String(),
                        WebPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WeClearedClientVersions",
                c => new
                    {
                        Version = c.String(nullable: false, maxLength: 128),
                        XmlContent = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Version);
            
            CreateTable(
                "dbo.Raiders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        ClassId = c.Int(nullable: false),
                        SpecializationId = c.Int(nullable: false),
                        Joined = c.DateTime(),
                        Left = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Raiders");
            DropTable("dbo.WeClearedClientVersions");
            DropTable("dbo.Addons");
        }
    }
}
