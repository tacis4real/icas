using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.DataContract;
using WebAdminStacks.Infrastructure;
using WebAdminStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace WebAdminStacks.Repository
{
    internal class UserRoleRepository
    {

        private readonly IWebAdminRepository<UserRole> _repository;
        private readonly WebAdminUoWork _uoWork;

        public UserRoleRepository()
        {
            _uoWork = new WebAdminUoWork();
            _repository = new WebAdminRepository<UserRole>(_uoWork);
        }

        public int AddUserRole(UserRole userRole)
        {
            try
            {
                var processedUserRole = _repository.Add(userRole);
                _uoWork.SaveChanges();
                return processedUserRole.UserRoleId;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public bool UpdateUserRole(UserRole userRole)
        {
            try
            {
                var processedUserRole = _repository.Update(userRole);
                _uoWork.SaveChanges();
                return processedUserRole.UserRoleId > 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public bool DeleteUserRole(int userRoleId)
        {
            try
            {
                var processedUserRole = _repository.Remove(userRoleId);
                _uoWork.SaveChanges();
                return processedUserRole.UserRoleId > 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public UserRole GetUserRole(int userRoleId)
        {
            try
            {
                var myItem = _repository.GetById(userRoleId);
                if (myItem == null || myItem.UserRoleId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public List<UserRole> GetUserRoles()
        {
            try
            {
                var myItemList = _repository.GetAll();
                if (myItemList == null || !myItemList.Any()) return null;
                return myItemList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public bool DeleteUserRolesByRole(int roleId, out string msg)
        {

            try
            {
                var sql1 =
                 String.Format(
                     "Delete FROM \"ICASWebAdmin\".\"UserRole\"  WHERE \"RoleId\" = {0}", roleId);

                msg = "";
                return _repository.RepositoryContext().Database.ExecuteSqlCommand(sql1) > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public List<UserRole> GetUserRolesByUserId(Int64 userId)
        {
            try
            {
                var myItemList = _repository.GetAll().Where(m => m.UserId == userId);
                if (!myItemList.Any()) return null;
                return myItemList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public List<UserRole> GetUserRolesByRoleId(Int32 roleId)
        {
            try
            {
                var myItemList = _repository.GetAll().Where(m => m.RoleId == roleId);
                if (!myItemList.Any()) return null;
                return myItemList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
    }
}
