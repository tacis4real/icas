using System;
using System.Linq;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class RoleInChurchRepository
    {
        private readonly IIcasRepository<RoleInChurch> _repository;
        private readonly IcasUoWork _uoWork;

        public RoleInChurchRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<RoleInChurch>(_uoWork);
        }


        public int AddRoleInChurch(RoleInChurch role, out string msg)
        {
            try
            {
                //if (IsDuplicate(role.Name, out msg))
                //{
                //    return -1;
                //}

                var processedRole = _repository.Add(role);
                _uoWork.SaveChanges();
                msg = "";
                if (processedRole.RoleInChurchId < 0)
                {
                    return -1;
                }
                return processedRole.RoleInChurchId;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        internal RoleInChurch GetRoleInChurch(string roleName)
        {
            try
            {
                //var myItem = _repository.GetAll(m => string.Compare(m.Name, roleName, StringComparison.CurrentCultureIgnoreCase) == 0).ToList();
                //if (!myItem.Any() || myItem.Count() != 1) { return null; }
                //return myItem[0].RoleInChurchId < 1 ? null : myItem[0];

                return null;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal bool IsRoleInChurchExist(string roleName, out int id)
        {
            try
            {

                var check = GetRoleInChurch(roleName);
                if (check == null)
                {
                    id = -1;
                    return false;
                }
                id = check.RoleInChurchId;
                return true;


                //var check = GetRoleInChurch(roleName);
                //return check != null && check.RoleInChurchId >= 1;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                id = -1;
                return false;
            }
        }

        private bool IsDuplicate(string roleName, out string msg)
        {
            try
            {
                var sql1 =
                 String.Format(
                     "Select * FROM \"ChurchAPPDB\".\"RoleInChurch\" WHERE lower(\"Name\") = lower('{0}')", roleName);

                var check = _repository.RepositoryContext().Database.SqlQuery<RoleInChurch>(sql1).ToList();
                msg = "";
                if (check.IsNullOrEmpty()) return false;
                msg = "Duplicate Error! Role already exist";
                return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }
    }
}
