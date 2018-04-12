using System;
using System.Data.Entity.Migrations;
using ICASStacks.DataManager.Migration;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.DataManager
{
    internal class MigrationManager
    {
        public static bool Migrate(out string msg)
        {
            try
            {
                var configuration = new Migration.Configuration();
                var migrator = new DbMigrator(configuration);
                migrator.Update();
                msg = "";
                return true;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return false;
            }
        }
    }
}
