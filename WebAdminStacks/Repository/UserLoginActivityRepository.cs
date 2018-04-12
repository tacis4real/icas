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
    internal class UserLoginActivityRepository
    {

        private readonly IWebAdminRepository<UserLoginActivity> _repository;
        private readonly WebAdminUoWork _uoWork;

        public UserLoginActivityRepository()
        {
            _uoWork = new WebAdminUoWork();
            _repository = new WebAdminRepository<UserLoginActivity>(_uoWork);
        }


        public long AddUserLoginActivity(UserLoginActivity loginActivity)
        {
            try
            {
                var processedUserLoginActivity = _repository.Add(loginActivity);
                _uoWork.SaveChanges();
                return processedUserLoginActivity.UserLoginActivityId;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public bool UpdateUserLoginActivity(UserLoginActivity loginActivity)
        {
            try
            {
                var processedUserLoginActivity = _repository.Update(loginActivity);
                _uoWork.SaveChanges();
                return processedUserLoginActivity.UserLoginActivityId > 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public bool DeleteUserLoginActivity(long loginActivityId)
        {
            try
            {
                var processedUserLoginActivity = _repository.Remove(loginActivityId);
                _uoWork.SaveChanges();
                return processedUserLoginActivity.UserLoginActivityId > 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public UserLoginActivity GetUserLoginActivity(long loginActivityId)
        {
            try
            {
                var myItem = _repository.GetById(loginActivityId);
                if (myItem == null || myItem.UserLoginActivityId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public List<UserLoginActivity> GetLoginActivities()
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

        public List<UserLoginActivity> GetLoginActivitiesByUserId(Int64 userId)
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
    }
}
