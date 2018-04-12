using System;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.ClientPortal
{
    public class ClientPortalInit
    {
        public static bool InitPortal(out string msg)
        {
            try
            {
                //AutomaticMigrator.Update(out msg);
                var client = PortalClientUser.GetClientChurchProfile("simpleadmin");
                if (client == null)
                {
                    msg = "";
                    return false;
                }
                msg = "";
                return client.IsApproved;
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