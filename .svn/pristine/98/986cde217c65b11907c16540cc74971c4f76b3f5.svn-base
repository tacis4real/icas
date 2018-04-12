namespace WebAdminStacks.DataManager.Migration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebAdminStackEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataManager\Migration";
        }

        protected override void Seed(WebAdminStackEntities context)
        {
            var seeding = new DataManager.Configuration();
            seeding.ProcessSeed(context);
        }
    }
}
