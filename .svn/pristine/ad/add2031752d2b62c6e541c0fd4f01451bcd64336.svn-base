using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace WebAdminStacks.Repository.Helper
{
    public static class BaseExceptionHelper
    {
        public static string GetFriendlyException(this Exception triggeredException, string itemName = "")
        {
            try
            {

                var basException = triggeredException.GetBaseException();
                if (basException.Equals(null))
                {
                    return "Unknown Exception Occurred";
                }
                var exceptionMessage = basException.Message;
                if (string.IsNullOrEmpty(exceptionMessage))
                {
                    return "Unknown Exception Occurred";
                }
                if (exceptionMessage.Contains("duplicate key value") || exceptionMessage.Contains("violates unique constraint"))
                {
                    return string.Format("Duplicate Error! This {0} already exist", string.IsNullOrEmpty(itemName) ? "Item" : itemName);
                }
                if (exceptionMessage.Contains("violates check constraint"))
                {
                    return string.Format("Invalid Parameter Error! At least one of the supplied parameter for {0} violates", string.IsNullOrEmpty(itemName) ? "Item" : itemName);
                }
                if (exceptionMessage.Contains("violates check constraint"))
                {
                    return string.Format("Invalid Parameter Error! At least one of the supplied parameter for {0} violates", string.IsNullOrEmpty(itemName) ? "Item" : itemName);
                }
                //NOT NULL VIOLATION
                return exceptionMessage;
            }
            catch (Exception ex)
            {
                return string.Format("Exception {0} Occurred", ex.GetBaseException().Message);
            }
        }
        public static string GetFriendlyException(this DbEntityValidationException triggeredException, string itemName = "")
        {
            try
            {
                var errorMessages = triggeredException.EntityValidationErrors
                         .SelectMany(x => x.ValidationErrors)
                         .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                return "Validation Error Occurred! Deatil: " + fullErrorMessage;
            }
            catch (Exception ex)
            {
                return string.Format("Exception {0} Occurred", ex.GetBaseException().Message);
            }
        }
    }
}
