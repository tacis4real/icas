using System.Linq;
using System.Web.Mvc;

namespace ICAS.Areas.Helpers.Awesome
{
    public static class ControllerExtensions
    {
        public static object GetErrorsObj(this ModelStateDictionary modelState)
        {
            var errorList = modelState.Where(o => o.Value.Errors.Count > 0)
                                .ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

            return new { Errors = errorList };
        }
    }
}