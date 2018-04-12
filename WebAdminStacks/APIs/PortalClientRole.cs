using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.DataContract;
using WebAdminStacks.Repository;

namespace WebAdminStacks.APIs
{
    public class PortalClientRole
    {


        #region Client Church

        public static string[] GetRolesForClientChurchProfile(string username, out string msg)
        {
            return new RoleClientRepository().GetRolesForClientChurchProfile(username, out msg);
        }

        public static int[] GetRoleIdsForClientChurchProfile(string username, out string msg)
        {
            return new RoleClientRepository().GetRoleIdsForClientChurchProfile(username, out msg);
        }

        #endregion





        public static string[] GetRolesForClient(string username, out string msg)
        {
            return new RoleClientRepository().GetRolesForClient(username, out msg);
        }
        

        public static List<RoleClient> GetRolesClient()
        {
            return new RoleClientRepository().GetRolesClient().ToList();
        }

    }
}
