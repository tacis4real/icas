using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.DataContract;
using WebAdminStacks.Infrastructure;
using WebAdminStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace WebAdminStacks.Repository
{
    internal class RoleClientRepository
    {

        private readonly IWebAdminRepository<RoleClient> _repository;
        private readonly WebAdminUoWork _uoWork;

        public RoleClientRepository()
        {
            _uoWork = new WebAdminUoWork();
            _repository = new WebAdminRepository<RoleClient>(_uoWork);
        }


        #region Client Church

        public string[] GetRolesForClientChurchProfile(string username, out string msg)
        {
            try
            {
                msg = "";
                var roleIds = new ClientChurchRoleRepository().GetClientChurchRolesByClientChurchProfileId(new ClientAdminRepository().GetClientChurchProfile(username).ClientChurchProfileId).Select(l => l.RoleClientId).ToList();
                if (roleIds.Count < 1) { return null; }
                var roles = GetRolesClient().Where(m => roleIds.Contains(m.RoleClientId)).ToList().Select(m => m.Name).ToArray();
                return roles;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        
        public int[] GetRoleIdsForClientChurchProfile(string username, out string msg)
        {
            try
            {
                msg = "";
                var roleIds = new ClientChurchRoleRepository().GetClientChurchRolesByClientChurchProfileId(new ClientAdminRepository().GetClientChurchProfile(username).ClientChurchProfileId).Select(l => l.RoleClientId).ToList();
                if (roleIds.Count < 1) { return null; }
                var roles = GetRolesClient().Where(m => roleIds.Contains(m.RoleClientId)).ToList().Select(m => m.RoleClientId).ToArray();
                return roles;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        #endregion



        public string[] GetRolesForClient(string username, out string msg)
        {
            try
            {
                msg = "";
                var roleIds = new ClientRoleRepository().GetClientRolesByClientProfileId(new ClientAdminRepository().GetClientProfile(username).ClientProfileId).Select(l => l.RoleClientId).ToList();
                if (roleIds.Count < 1) { return null; }
                var roles = GetRolesClient().Where(m => roleIds.Contains(m.RoleClientId)).ToList().Select(m => m.Name).ToArray();
                return roles;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        
        public int[] GetRoleIdsForClient(string username, out string msg)
        {
            try
            {
                msg = "";
                var roleIds = new ClientRoleRepository().GetClientRolesByClientProfileId(new ClientAdminRepository().GetClientProfile(username).ClientProfileId).Select(l => l.RoleClientId).ToList();
                if (roleIds.Count < 1) { return null; }
                var roles = GetRolesClient().Where(m => roleIds.Contains(m.RoleClientId)).ToList().Select(m => m.RoleClientId).ToArray();
                return roles;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        
        public IQueryable<RoleClient> GetRolesClient()
        {
            try
            {
                var roleList = CacheManager.GetCache("ccPortalRoleList") as IQueryable<RoleClient>;
                if (roleList.IsNullOrEmpty())
                {
                    roleList = _repository.GetAll();
                    if (roleList.IsNullOrEmpty()) { return null; }
                    CacheManager.SetCache("ccPortalRoleList", roleList, DateTime.Now.AddYears(1));
                }
                if (roleList == null || !roleList.Any()) { return null; }
                return roleList;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public RoleClient GetRoleClientByName(string roleName, out string msg)
        {
            try
            {
                var clientRole = _repository.GetAll(x => string.Compare(x.Name.Trim(), roleName.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0).ToList()[0];
                msg = "";
                return clientRole;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
    }
}
