using System;
using System.Web.Security;

namespace ICAS.ClientPortal
{
    #region Authentication Service
    public interface IFormsAuthenticationService
    {
        string SignIn(string userName, bool createPersistentCookie, string clientData);
        void SignOut();
    }

    public interface IEppFormsAuthenticationService
    {
        FormsAuthenticationTicket SignIn(string userName, bool createPersistentCookie, string clientData);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public string SignIn(string userName, bool createPersistentCookie, string clientData)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Client Username cannot be null");
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(20), createPersistentCookie, clientData, FormsAuthentication.FormsCookiePath);
            var encTicket = FormsAuthentication.Encrypt(ticket);
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            return encTicket;

        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        //public string GetUserData(HttpContext requestContext)
        //{
        //    try
        //    {
        //        // Get Forms Identity From Current User
        //        var id = (FormsIdentity)requestContext.User.Identity;

        //        // Create a custom Principal Instance and assign to Current User (with caching)
        //        var cookie = requestContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

        //        var cookieUserData = ticket.UserData; // not empty
        //        var httpContextIdentiyUserData = id.Ticket.UserData; // empty!
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

    }

    public class EppFormsAuthenticationService : IEppFormsAuthenticationService
    {
        public FormsAuthenticationTicket SignIn(string userName, bool createPersistentCookie, string userData)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("User name cannot be null");
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(20), createPersistentCookie, userData, FormsAuthentication.FormsCookiePath);
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            return ticket;

        }

        public void SignOut()
        {

            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        //public string GetUserData(HttpContext requestContext)
        //{
        //    try
        //    {
        //        // Get Forms Identity From Current User
        //        var id = (FormsIdentity)requestContext.User.Identity;

        //        // Create a custom Principal Instance and assign to Current User (with caching)
        //        var cookie = requestContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

        //        var cookieUserData = ticket.UserData; // not empty
        //        var httpContextIdentiyUserData = id.Ticket.UserData; // empty!
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

    }

    #endregion

}