namespace Project_Management_Tool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "AssignedByUser", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "AssignedByUser");
        }
    }
}
