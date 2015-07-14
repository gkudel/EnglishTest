namespace EnglishTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pass = c.Boolean(nullable: false),
                        WordId = c.Int(nullable: false),
                        TestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Test", t => t.TestId, cascadeDelete: true)
                .ForeignKey("dbo.Word", t => t.WordId, cascadeDelete: true)
                .Index(t => t.WordId)
                .Index(t => t.TestId);
            
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        Finish = c.DateTime(),
                        Finished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Word",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Polish = c.String(nullable: false, maxLength: 100),
                        English = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Polish, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Question", "WordId", "dbo.Word");
            DropForeignKey("dbo.Question", "TestId", "dbo.Test");
            DropIndex("dbo.Word", new[] { "Polish" });
            DropIndex("dbo.Question", new[] { "TestId" });
            DropIndex("dbo.Question", new[] { "WordId" });
            DropTable("dbo.Word");
            DropTable("dbo.Test");
            DropTable("dbo.Question");
        }
    }
}
