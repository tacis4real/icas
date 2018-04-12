using System;
using System.Collections.Generic;
using System.Linq;
using WebAdminStacks.DataContract;
using WebAdminStacks.Repository;

namespace WebAdminStacks.APIs
{
    public class PortalRole
    {


        #region Portal User
        public static int AddRole(Role role, out string msg)
        {
            return new RoleRepository().AddRole(role, out msg);
        }

        public static bool UpdateRole(Role role, out string msg)
        {
            return new RoleRepository().UpdateRole(role, out msg);
        }

        public static bool DeleteRole(Int32 roleId, out string msg)
        {
            return new RoleRepository().DeleteRole(roleId, out msg);
        }

        public static Role GetRole(int roleId, out string msg)
        {
            return new RoleRepository().GetRole(roleId, out msg);
        }

        public static List<Role> GetRoles()
        {
            return new RoleRepository().GetRoles().ToList();
        }

        public static string[] GetAllRoles(out string msg)
        {
            return new RoleRepository().GetAllRoles(out msg);
        }

        public static string[] GetAllRoles()
        {
            return new RoleRepository().GetAllRoles();
        }

        public static string[] GetRolesForUser(string username, out string msg)
        {
            return new RoleRepository().GetRolesForUser(username, out msg);
        }

        public static List<RoleNameValue> GetCheckableRoleList(string userRoles)
        {
            return new RoleRepository().GetCheckableRoleList(userRoles);
        }

        public static string[] GetUsersInRole(string rolename, string usernameToMatch, out string msg)
        {
            return new RoleRepository().GetUsersInRole(rolename, usernameToMatch, out msg);
        }

        public static bool IsUserInRole(string username, string roleName, out string msg)
        {
            return new RoleRepository().IsUserInRole(username, roleName, out msg);
        }

        public static string[] GetUsersInRole(string rolename, out string msg)
        {
            return new RoleRepository().GetUsersInRole(rolename, out msg);
        }

        public static int AddUserToRoles(string username, string[] roleNames, out string msg)
        {
            return new RoleRepository().AddUserToRoles(username, roleNames, out msg);
        }

        public static int AddUsersToRoles(string[] usernames, string[] roleNames, out string msg)
        {
            return new RoleRepository().AddUsersToRoles(usernames, roleNames, out msg);
        }

        public static int RemoveUsersFromRoles(string[] usernames, string[] roleNames, out string msg)
        {
            return new RoleRepository().RemoveUsersFromRoles(usernames, roleNames, out msg);
        }

        public static bool RoleExists(string roleName, out string msg)
        {
            return new RoleRepository().RoleExists(roleName, out msg);
        }
        #endregion
    }
}
