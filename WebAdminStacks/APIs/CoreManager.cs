using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace WebAdminStacks.APIs
{

    public class StcPrincipal : IPrincipal
    {

        private readonly string[] _roles;

        public bool IsInRole(string role)
        {
            try
            {
                if (string.IsNullOrEmpty(role))
                {
                    throw new Exception("Role Name is empty or invalid");     
                }
                if (role == "*") { return true; }
                if (_roles != null && _roles.Length > 0)
                {
                    return _roles.Contains(role);
                }

                return true;    //return new RoleRepository().IsUserInRole(Identity.Name, role.Trim());
            }
            catch (Exception ex)
            {
                 BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                throw new Exception("Unknown Error Occured");
            }
        }

        public IIdentity Identity { get; private set; }

        public StcPrincipal(IIdentity identity)
        {
            if (identity == null)
            {
                 throw new ArgumentNullException("identity");
            }
            if (string.IsNullOrEmpty(identity.Name))
            {
                throw new InvalidCredentialException("Authorized name cannot be null");
            }
            if (!identity.IsAuthenticated)
            {
                throw new InvalidCredentialException("Current User is not authenticated");
            }

            Identity = identity;
        }

        public StcPrincipal(IIdentity identity, string[] roles)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            if (string.IsNullOrEmpty(identity.Name))
            {
                throw new InvalidCredentialException("Authorized name cannot be null");
            }
            if (!identity.IsAuthenticated)
            {
                throw new InvalidCredentialException("Current User is not authenticated");
            }

            Identity = identity;
            _roles = roles;
        }
    }

}
