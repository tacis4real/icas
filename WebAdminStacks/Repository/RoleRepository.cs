using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAdminStacks.DataContract;
using WebAdminStacks.Infrastructure;
using WebAdminStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace WebAdminStacks.Repository
{
    internal class RoleRepository
    {

        private readonly IWebAdminRepository<Role> _repository;
        private readonly WebAdminUoWork _uoWork;

        public RoleRepository()
        {
            _uoWork = new WebAdminUoWork();
            _repository = new WebAdminRepository<Role>(_uoWork);
        }


        public int AddRole(Role role, out string msg)
        {
            try
            {
                if (IsDuplicate(role.Name, out msg))
                {
                    return -1;
                }

                var processedRole = _repository.Add(role);
                _uoWork.SaveChanges();
                msg = "";
                if (processedRole.RoleId > 0)
                {
                    if (CacheManager.GetCache("ccPortalRoleList") != null)
                    {
                        CacheManager.RemoveCache("ccPortalRoleList");
                    }
                    GetRoles();
                }
                return processedRole.RoleId;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public bool UpdateRole(Role role, out string msg)
        {
            try
            {
                if (!IsUpdateable(role.Name, role.RoleId, out msg))
                {
                    return false;
                }
                var processedRole = _repository.Update(role);
                _uoWork.SaveChanges();
                msg = "";
                if (processedRole.RoleId > 0)
                {
                    if (CacheManager.GetCache("ccPortalRoleList") != null)
                    {
                        CacheManager.RemoveCache("ccPortalRoleList");
                    }
                    GetRoles();
                }
                return processedRole.RoleId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public bool DeleteRole(int roleId, out string msg)
        {
            try
            {
                //remove users from role
                if (!new UserRoleRepository().DeleteUserRolesByRole(roleId, out msg)) { return false; }
                //delete role
                var processedRole = _repository.Remove(roleId);
                _uoWork.SaveChanges();
                msg = "";
                if (processedRole.RoleId > 0)
                {
                    if (CacheManager.GetCache("ccPortalRoleList") != null)
                    {
                        CacheManager.RemoveCache("ccPortalRoleList");
                    }
                    GetRoles();
                }
                return processedRole.RoleId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public bool DeleteRole(string roleName, out string msg)
        {
            try
            {
                var processedRole = _repository.Remove(_repository.GetAll(m => string.Compare(m.Name.Trim(), roleName.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0).ToList()[0]);
                _uoWork.SaveChanges();
                msg = "";
                return processedRole.RoleId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public Role GetRole(int roleId, out string msg)
        {
            try
            {
                msg = "";
                var myItem = _repository.GetById(roleId);
                if (myItem == null || myItem.RoleId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public IQueryable<Role> GetRoles()
        {
            try
            {
                var roleList = CacheManager.GetCache("ccPortalRoleList") as IQueryable<Role>;
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

        private bool IsDuplicate(string roleName, out string msg)
        {
            try
            {
                var sql1 =
                 String.Format(
                     "Select * FROM \"ChurchWebAdmin\".\"Role\"  WHERE lower(\"Name\") = lower('{0}')", roleName.Replace("'", "''"));

                var check = _repository.RepositoryContext().Database.SqlQuery<Role>(sql1).ToList();
                msg = "";
                if (check.IsNullOrEmpty()) return false;
                msg = "Duplicate Error! Item already exist";
                return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }

        private bool IsUpdateable(string roleName, int id, out string msg)
        {
            try
            {
                var sql1 =
                 String.Format(
                     "Select * FROM \"ChurchWebAdmin\".\"Role\"  WHERE lower(\"Name\") = lower('{0}')", roleName.Replace("'", "''"));

                var check = _repository.RepositoryContext().Database.SqlQuery<Role>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "";
                    return true;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Update Request: Update will affect unexpected number of records";
                    return true;
                }
                if (check[0].RoleId != id)
                {
                    msg = "Invalid Update Request: Update will result in duplicate entry";
                    return false;
                }
                msg = "";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }

        public string[] GetAllRoles(out string msg)
        {
            try
            {
                msg = "";
                var roles = GetRoles().Select(m => m.Name).ToArray();
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

        public string[] GetAllRoles()
        {
            try
            {

                var roles = GetRoles().Select(m => m.Name).ToArray();
                return roles;
            }
            catch (DbEntityValidationException ex)
            {

                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public string[] GetRolesForUser(string username, out string msg)
        {
            try
            {
                msg = "";
                var roleIds = new UserRoleRepository().GetUserRolesByUserId(new UserRepository().GetUser(username).UserId).Select(l => l.RoleId).ToList();
                if (roleIds.Count < 1) { return null; }
                var roles = GetRoles().Where(m => roleIds.Contains(m.RoleId)).ToList().Select(m => m.Name).ToArray();
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

        public int[] GetRoleIdsForUser(string username, out string msg)
        {
            try
            {
                msg = "";
                var roleIds = new UserRoleRepository().GetUserRolesByUserId(new UserRepository().GetUser(username).UserId).Select(l => l.RoleId).ToList();
                if (roleIds.Count < 1) { return null; }
                var roles = GetRoles().Where(m => roleIds.Contains(m.RoleId)).ToList().Select(m => m.RoleId).ToArray();
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

        public List<RoleNameValue> GetCheckableRoleList(string userRoles)
        {
            try
            {
                var roles = GetRoles();
                if (roles == null)
                {
                    return new List<RoleNameValue>();
                }
                var userChecked = string.IsNullOrEmpty(userRoles) ? new[] { "NoRole" } : userRoles.Split(";".ToCharArray());
                var myList = (from item in roles
                              where !string.IsNullOrEmpty(item.Name)
                              select new RoleNameValue
                              {
                                  Value = item.Name,
                                  DisplayText = Regex.Replace(item.Name, "(\\B[A-Z])", " $1"),
                                  IsChecked = userChecked.Contains(item.Name)
                              }).ToList();


                return myList;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RoleNameValue>();
            }

        }

        internal string[] GetUsersInRole(string rolename, string usernameToMatch, out string msg)
        {
            try
            {
                msg = "";
                var userIds = new UserRoleRepository().GetUserRolesByRoleId(GetRoles().Where(p => String.Compare(p.Name.Trim(), rolename.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0).ToList()[0].RoleId).Select(m => m.UserId).ToList();
                if (!userIds.Any()) return null;
                return new UserRepository().GetUsers().Where(m => userIds.Contains(m.UserId) && m.Username.Contains(usernameToMatch.Trim())).Select(q => q.Username).ToArray();
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public bool IsUserInRole(string username, string roleName, out string msg)
        {
            try
            {
                msg = "";
                var userId =
                        new UserRepository().GetUsers().Where(
                            m =>
                                String.Compare(m.Username.Trim(), username.Trim(),
                                    StringComparison.CurrentCultureIgnoreCase) == 0).Select(m => m.UserId).ToList()[0];

                var roles = GetRoles().Where(
                    m =>
                        String.Compare(m.Name.Trim(), roleName.Trim(),
                           StringComparison.CurrentCultureIgnoreCase) == 0).ToList();

                var roleId = 0;
                if (roles.Any())
                {
                    roleId = roles[0].RoleId;
                }

                if (roleId < 1) { return false; }


                return new UserRoleRepository().GetUserRoles().Count(m => m.UserId == userId && m.RoleId == roleId) > 0;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public bool IsUserInRole(string username, string roleName)
        {
            try
            {

                var userId =
                        new UserRepository().GetUsers().Where(
                            m =>
                                String.Compare(m.Username.Trim(), username.Trim(),
                                    StringComparison.CurrentCultureIgnoreCase) == 0).Select(m => m.UserId).ToList()[0];

                var roles = GetRoles().Where(
                     m =>
                         String.Compare(m.Name.Trim(), roleName.Trim(),
                            StringComparison.CurrentCultureIgnoreCase) == 0).ToList();

                var roleId = 0;
                if (roles.Any())
                {
                    roleId = roles[0].RoleId;
                }

                if (roleId < 1) { return false; }

                return new UserRoleRepository().GetUserRoles().Count(m => m.UserId == userId && m.RoleId == roleId) > 0;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        internal string[] GetUsersInRole(string rolename, out string msg)
        {
            try
            {
                msg = "";
                var userIds = new UserRoleRepository().GetUserRolesByRoleId(GetRoles().Where(p => String.Compare(p.Name.Trim(), rolename.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0).ToList()[0].RoleId).Select(m => m.UserId).ToList();
                if (!userIds.Any()) return null;
                return new UserRepository().GetUsers().Where(m => userIds.Contains(m.UserId)).Select(q => q.Username).ToArray();
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return null;
            }
        }

        internal int AddUserToRoles(string username, string[] roleNames, out string msg)
        {
            try
            {

                var roles = String.Join("), lower(", roleNames);
                var userId = (new UserRepository().GetUser(username) ?? new DataContract.User()).UserId;

                if (roles.StartsWith("), lower("))
                {
                    roles = roles.Substring(2);
                }
                roles = roles + ")";


                var sql2 =
                    String.Format(
                        "Select \"RoleId\" FROM   \"ChurchWebAdmin\".\"Role\"  WHERE \"Name\" in ({0})", roles);



                var roleIds = _repository.RepositoryContext().Database.SqlQuery<int>(sql2).ToList();

                if (roleIds.Any())
                {
                    msg = "Process Failed! Either User list is empty or Role list is empty";
                    return -1;
                }

                var counter = roleIds.Select(roleitem => new UserRole { UserId = userId, RoleId = roleitem }).Select(userRole => new UserRoleRepository().AddUserRole(userRole)).Count(retVal => retVal > 0);
                msg = "";
                return counter;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return 0;
            }
        }

        internal int AddUsersToRoles(string[] usernames, string[] roleNames, out string msg)
        {
            try
            {
                var names = String.Join("), lower(", usernames);
                var roles = String.Join("), lower(", roleNames);
                if (names.StartsWith("), lower("))
                {
                    names = names.Substring(2);
                }
                names = names + ")";

                if (roles.StartsWith("), lower("))
                {
                    roles = roles.Substring(2);
                }
                roles = roles + ")";

                var sql1 =
                    String.Format(
                        "Select \"UserId\" FROM   \"ChurchWebAdmin\".\"User\"  WHERE \"UserName\" in ({0})", names);
                var sql2 =
                    String.Format(
                        "Select \"RoleId\" FROM   \"ChurchWebAdmin\".\"Role\"  WHERE \"Name\" in ({0})", roles);


                var userIds = _repository.RepositoryContext().Database.SqlQuery<int>(sql1).ToList();
                var roleIds = _repository.RepositoryContext().Database.SqlQuery<int>(sql2).ToList();

                if (!userIds.Any() || roleIds.Any())
                {
                    msg = "Process Failed! Either User list is empty or Role list is empty";
                    return -1;
                }

                var counter = 0;
                foreach (var item in userIds)
                {
                    foreach (var roleitem in roleIds)
                    {
                        var userRole = new UserRole { UserId = item, RoleId = roleitem };
                        var retVal = new UserRoleRepository().AddUserRole(userRole);
                        if (retVal > 0) counter++;
                    }
                }

                msg = "";
                return counter;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return 0;
            }
        }

        internal int RemoveUsersFromRoles(string[] usernames, string[] roleNames, out string msg)
        {
            try
            {
                var names = String.Join("), lower(", usernames);
                var roles = String.Join("), lower(", roleNames);
                if (names.StartsWith("), lower("))
                {
                    names = names.Substring(2);
                }
                names = names + ")";

                if (roles.StartsWith("), lower("))
                {
                    roles = roles.Substring(2);
                }
                roles = roles + ")";

                var sql1 =
                    String.Format(
                        "Select \"UserId\" FROM   \"ChurchWebAdmin\".\"User\"  WHERE \"UserName\" in ({0});", names);
                var sql2 =
                    String.Format(
                        "Select \"RoleId\" FROM   \"ChurchWebAdmin\".\"Role\"  WHERE \"Name\" in ({0});", roles);


                var userIds = _repository.RepositoryContext().Database.SqlQuery<int>(sql1).ToList();
                var roleIds = _repository.RepositoryContext().Database.SqlQuery<int>(sql2).ToList();

                if (!userIds.Any() || roleIds.Any())
                {
                    msg = "Process Failed! Either User list is empty or Role list is empty";
                    return -1;
                }

                var counter = 0;
                foreach (var item in userIds)
                {
                    foreach (var roleitem in roleIds)
                    {
                        sql1 =
                    String.Format(
                        "Delete From \"ChurchWebAdmin\".\"UserRole\"  WHERE \"UserId\" = {0} And \"RoleId\" = {1};", item, roleitem);
                        var retId = _repository.RepositoryContext().Database.ExecuteSqlCommand(sql1);
                        if (retId > 0) counter++;
                    }
                }

                msg = "";
                return counter;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return 0;
            }
        }

        internal bool RoleExists(string roleName, out string msg)
        {
            try
            {
                msg = "";
                return
                        GetRoles().Count(
                            m =>
                                string.Compare(m.Name.Trim(), roleName.Trim(), StringComparison.CurrentCultureIgnoreCase) ==
                                0) > 0;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return false;
            }
        }



        #region Client

        //public RoleClient GetClientRoleByName(string roleName, out string msg)
        //{
        //    try
        //    {
        //        //var clientRole = _repository.GetAll(x => string.Compare(x.Name.Trim(), roleName.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0).ToList()[0];
        //        //msg = "";
        //        //return clientRole;
        //        msg = "";
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.Message;
        //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        return null;
        //    }
        //}
        #endregion
    }
}
