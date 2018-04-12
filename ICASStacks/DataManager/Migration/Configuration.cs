using ICASStacks.DataContract.BioEnroll;

namespace ICASStacks.DataManager.Migration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IcasDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataManager\Migration";
        }

        protected override void Seed(IcasDataContext context)
        {
            var con = new DataManager.Configuration();
            con.ProcessedSeed(context);

        }
    }
}
