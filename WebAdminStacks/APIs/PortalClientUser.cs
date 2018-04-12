using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.APIObjs;
using WebAdminStacks.DataContract;
using WebAdminStacks.Infrastructure;
using WebAdminStacks.Infrastructure.Contract;
using WebAdminStacks.Repository;

namespace WebAdminStacks.APIs
{
    public class PortalClientUser
    {

        #region Client Church

        public static ClientChurchProfile GetClientChurchProfileByClientChurchId(long clientChurchId)
        {
            return new ClientAdminRepository().GetClientChurchProfileByClientChurchId(clientChurchId);
        }

        public static List<ClientChurchRole> GetClientChurchRolesByClientChurchProfileId(long clientChurchProfileId)
        {
            return new ClientAdminRepository().GetClientChurchRolesByClientChurchProfileId(clientChurchProfileId);
        }

        public static ClientChurchProfile GetClientChurchProfile(string username)
        {
            return new ClientAdminRepository().GetClientChurchProfile(username);
        }

        public static ClientChurchLoginResponseObj LoginClientChurchUser(string username, string password, int loginSource, string deviceSerial)
        {
            return new ClientAdminRepository().LoginClientChurchUser(username, password, loginSource, deviceSerial);
        }


        public static ClientChurchResponseObj ChangeClientChurchPassword(string username, string oldPassword, string newPassword)
        {
            return new ClientAdminRepository().ChangeClientChurchPassword(username, oldPassword, newPassword);
        }




        #region Client Church

        public static RegisteredClientChurchProfileReportObj GetClientChurchAdminUser(long clientChurchUserId)
        {
            return new ClientAdminRepository().GetClientChurchAdminUserProfileObj(clientChurchUserId);
        }


        public static bool UpdateClientChurchUser(ClientChurchProfile agentUser, out string msg)
        {
            return new ClientAdminRepository().UpdateClientChurchAdminUser(agentUser, out msg);
        }


        public static ClientChurchProfile GetRawClientChurchUser(long clientChurchProfileId)
        {
            return new ClientAdminRepository().GetClientChurchUserProfile(clientChurchProfileId);
        }

        #endregion



        #endregion

        #region Handlers

        #region Client Church Handler

        public static IWebAdminOpenRepository<ClientChurchProfile> ClientChurchProfileHandler()
        {
            return new ClientAdminRepository().ClientChurchProfileHandler();
        }

        public static WebAdminOpenUoWork WebAdminOpenUoWork()
        {
            return new ClientAdminRepository().WebAdminOpenUoWork();
        }

        public static List<ClientChurchRole> GetClientChurchRoles(int[] roleIds)
        {
            return new ClientAdminRepository().GetClientChurchRoles(roleIds);
        }

        #endregion


        #endregion







        public static ClientChurchProfileRegResponse AddClientChurchProfile(ClientProfileRegistrationObj clientChurchProfile)
        {
            return new ClientAdminRepository().AddClientChurchProfile(clientChurchProfile);
        }

        public static RespStatus UpdateClientChurchProfile(ClientProfileRegistrationObj clientChurchProfile)
        {
            return new ClientAdminRepository().UpdateClientChurchProfile(clientChurchProfile);
        }

        public static ClientProfileRegResponse AddClientProile(ClientProfileRegObj clientProfile)
        {
            return new ClientAdminRepository().AddClientProfile(clientProfile);
        }


        public static RespStatus UpdateClientProfile(ClientProfileRegObj clientProfile)
        {
            return new ClientAdminRepository().UpdateClientProfile(clientProfile);
        }


        public static ClientLoginResponseObj LoginClientUser(string username, string password, int loginSource, string deviceSerial)
        {
            return new ClientAdminRepository().LoginClientUser(username, password, loginSource, deviceSerial);
        }


        public static ClientResponseObj LockClientAdminUser(string targetUsername)
        {
            return new ClientAdminRepository().LockClientAdminUser(targetUsername);
        }

        public static ClientResponseObj UnLockClientAdminUser(string targetUsername)
        {
            return new ClientAdminRepository().UnLockClientAdminUser(targetUsername);
        }

        public static bool UpdateClientUser(ClientProfile agentUser, out string msg)
        {
            return new ClientAdminRepository().UpdateClientAdminUser(agentUser, out msg);
        }

        public static ClientProfile GetClientProfileByClientId(long clientId)
        {
            return new ClientAdminRepository().GetClientProfileByClientId(clientId);
        }



        public static ClientProfile GetRawClientUser(long clientProfileId)
        {
            return new ClientAdminRepository().GetClientUserProfile(clientProfileId);
        }

        public static ClientProfile GetClientProfile(string username)
        {
            return new ClientAdminRepository().GetClientProfile(username);
        }

        public static List<RegisteredClientProfileReportObj> GetAllRegisteredClientProfileObjs()
        {
            return new ClientAdminRepository().GetAllRegisteredClientProfileObjs();
        }


        public static bool IsDuplicate(string userName, string mobileNo, out string msg, int status)
        {
            return new ClientAdminRepository().IsDuplicate(userName, mobileNo, out msg, status);
        }

        public static ClientProfile GetClientProfile(long clientId, out string msg)
        {
            return new ClientAdminRepository().GetClientProfile(clientId, out msg);
        }

        public static ClientResponseObj ChangeClientPassword(string username, string oldPassword, string newPassword)
        {
            return new ClientAdminRepository().ChangeClientPassword(username, oldPassword, newPassword);
        }




        #region Client Church Admin Mgnt

        public static List<RegisteredClientProfileReportObj> GetAllRegisteredClientProfileObjsByClientId(long clientId)
        {
            return new ClientAdminRepository().GetAllRegisteredClientProfileObjsByClientId(clientId);
        }

        public static AdminTaskResponseObj ResetChurchAdminUserPassword(string targetUsername)
        {
            return new ClientAdminRepository().ResetChurchAdminUserPassword(targetUsername);
        }

        public static RegisteredClientProfileReportObj GetClientAdminUser(long clientUserId)
        {
            return new ClientAdminRepository().GetClientAdminUserProfileObj(clientUserId);
        }

        #endregion

        //public static RoleClient GetClientRoleByName(string roleName, out string msg)
        //{
        //    return new RoleClientRepository().GetRoleClientByName(roleName, out msg);
        //}
    }
}
