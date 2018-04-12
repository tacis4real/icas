using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using ICAS.App_Start;
using ICAS.Areas.Admin.Models.PortalModel;
using ICAS.Models.ClientPortalModel;
using ICASStacks.StackService;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS
{
    public class MvcApplication : HttpApplication
    {

        public static Hashtable ThisPortalDefaultSettings;
        private static BackgroundWorker _bgWorkerDisburse;
        
        //public static WithdrawRepository _withdrawRepo;

        protected void Application_Start()
        {
            CronJobs();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(new DataAnnotationsModelValidatorProvider());
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
        }
        

        void Application_AuthenticateRequest()
        {

            var myCookie = FormsAuthentication.FormsCookieName;
            var myAuthCookie = Context.Request.Cookies[myCookie];

            if (null == myAuthCookie)
            {
                return;
            }

            FormsAuthenticationTicket myAuthTicket;
            try
            {
                myAuthTicket = FormsAuthentication.Decrypt(myAuthCookie.Value);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return;
            }

            if (null == myAuthTicket)
            {
                return;
            }


            var userDataSplit = myAuthTicket.UserData.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var size = userDataSplit.Length;

            //if (!userDataSplit.Any() || userDataSplit.Length != 3)
            if (!userDataSplit.Any())
            {
                if ((size != 3 || size != 4))
                {
                    return;
                }
            }


            //if (!userDataSplit.Any() || userDataSplit.Length != 3)
            //{
            //    return;
            //}

            if (!DataCheck.IsNumeric(userDataSplit[0].Trim()))
            {
                return;
            }

            switch (size)
            {
                case 3:
                    var roles = userDataSplit[2].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var id = new FormsIdentity(myAuthTicket);
                    IPrincipal principal = new StcPrincipal(id, roles);
                    Context.User = principal;
                    break;

                case 4:
                    var clientRoles = userDataSplit[3].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var clientId = new FormsIdentity(myAuthTicket);
                    IPrincipal clientPrincipal = new StcPrincipal(clientId, clientRoles);
                    Context.User = clientPrincipal;
                    break;
            }

            //var roles = userDataSplit[2].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            //var id = new FormsIdentity(myAuthTicket);
            //IPrincipal principal = new StcPrincipal(id, roles);
            //Context.User = principal;



        }

        void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null)
            {
                //This extends cache timeout due to recent access of the cache data
                var sKey = Session["UserINFO"] as string;
                var mKey = Session["ClientINFO"] as string;
                var dKey = Session["UserDATAINFO"] as string;
                var nKey = Session["ClientDATAINFO"] as string;

                if (!string.IsNullOrEmpty(sKey))
                {
                    var sUser = HttpContext.Current.Cache[sKey] as string;
                }
                if (!string.IsNullOrEmpty(dKey))
                {
                    var xcode = "CHURCHAPP_USER_DATA_" + dKey;
                    var sUserData = HttpContext.Current.Cache[xcode] as UserData;
                }

                // For Client
                if (!string.IsNullOrEmpty(mKey))
                {
                    var sClient = HttpContext.Current.Cache[mKey] as string;
                }
                if (!string.IsNullOrEmpty(nKey))
                {
                    var xcode = "CHURCHAPP_CLIENT_DATA_" + nKey;
                    var sClientData = HttpContext.Current.Cache[xcode] as ClientData;
                }
            }
            else
            {
                foreach (var nItem in HttpContext.Current.Cache.OfType<string>())
                {
                    HttpContext.Current.Cache.Remove(nItem);
                }
                foreach (var nItem in HttpContext.Current.Cache.OfType<UserData>())
                {
                    if (nItem != null)
                    {
                        var xcode = "CHURCHAPP_USER_DATA_" + nItem.Username;
                        HttpContext.Current.Cache.Remove(xcode);
                    }
                }

                foreach (var nItem in HttpContext.Current.Cache.OfType<ClientData>())
                {
                    if (nItem != null)
                    {
                        var xcode = "CHURCHAPP_CLIENT_DATA_" + nItem.Username;
                        HttpContext.Current.Cache.Remove(xcode);
                    }
                }
            }
        }
        
        public static bool IsUserAlreadyLoggedIn(string code, out string msg)
        {
            var storedUser = Convert.ToString(HttpContext.Current.Cache[code]);
            if (string.IsNullOrEmpty(storedUser))
            {
                var timeout = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                HttpContext.Current.Cache.Insert(code, code, null, DateTime.MaxValue, timeout, CacheItemPriority.High, null);
                msg = "";
                return false;
            }

            msg = "Multiple Login Not Allowed!";
            return true;

        }

        void Session_Start(object sender, EventArgs e)
        {
            ThisPortalDefaultSettings = new Hashtable();
        }

        void Session_End(object sender, EventArgs e)
        {
            ThisPortalDefaultSettings = new Hashtable();

        }


        #region Users

        public static void ResetLogin(string code)
        {
            try
            {
                HttpContext.Current.Cache[code] = null;

            }
            catch (Exception)
            {

            }
        }

        public static string GetApplicationPath(HttpRequest request)
        {
            var path = String.Empty;
            try
            {
                if (request.ApplicationPath != "/")
                {
                    path = request.ApplicationPath;
                }
            }
            catch
            {
                return "";
            }
            return path;
        }

        public static string GetRandomString()
        {
            var ret = Environment.TickCount.ToString(CultureInfo.InvariantCulture) + (new Guid()).ToString().Replace("-", "").Substring(0, 8);
            return ret;
        }

        public static bool SetUserData(UserData userData)
        {
            try
            {
                if (userData == null)
                {
                    return false;
                }

                var usercode = "CHURCHAPP_USER_DATA_" + userData.Username;


                if (HttpContext.Current.Cache[usercode] != null)
                {
                    HttpContext.Current.Cache.Remove(usercode);
                }

                var timeout = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                HttpContext.Current.Cache.Insert(usercode, userData, null, DateTime.MaxValue, timeout, CacheItemPriority.High, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static UserData GetUserData(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return null;
                }
                var usercode = "CHURCHAPP_USER_DATA_" + username;
                var userData = HttpContext.Current.Cache[usercode] as UserData;
                if (userData == null || userData.UserId < 1)
                {
                    return null;
                }
                return userData;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public static void ResetUserData(string username)
        {
            try
            {
                HttpContext.Current.Cache["CHURCHAPP_USER_DATA_" + username] = null;
            }
            catch (Exception)
            {

            }
        }

       
        
        
        
        public static bool SetClientData(ClientData clientData)
        {
            try
            {
                if (clientData == null)
                {
                    return false;
                }

                var clientcode = "CHURCHAPP_CLIENT_DATA_" + clientData.Username;
                if (HttpContext.Current.Cache[clientcode] != null)
                {
                    HttpContext.Current.Cache.Remove(clientcode);
                }

                var timeout = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                HttpContext.Current.Cache.Insert(clientcode, clientData, null, DateTime.MaxValue, timeout, CacheItemPriority.High, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SetClientChurchData(ClientChurchData clientChurchData)
        {
            try
            {
                if (clientChurchData == null)
                {
                    return false;
                }

                var clientchurchcode = "CHURCHAPP_CLIENTCHURCH_DATA_" + clientChurchData.Username;
                if (HttpContext.Current.Cache[clientchurchcode] != null)
                {
                    HttpContext.Current.Cache.Remove(clientchurchcode);
                }

                var timeout = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                HttpContext.Current.Cache.Insert(clientchurchcode, clientChurchData, null, DateTime.MaxValue, timeout, CacheItemPriority.High, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static ClientChurchData GetClientChurchData(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return null;
                }

                var clientchurchcode = "CHURCHAPP_CLIENTCHURCH_DATA_" + username;
                var clientChurchData = HttpContext.Current.Cache[clientchurchcode] as ClientChurchData;
                if (clientChurchData == null || clientChurchData.ClientId < 1)
                {
                    return null;
                }
                return clientChurchData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ClientData GetClientData(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return null;
                }

                var clientcode = "CHURCHAPP_CLIENT_DATA_" + username;
                var clientData = HttpContext.Current.Cache[clientcode] as ClientData;
                if (clientData == null || clientData.ClientProfileId < 1)
                {
                    return null;
                }
                return clientData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void ResetClientData(string username)
        {
            try
            {
                HttpContext.Current.Cache["CHURCHAPP_CLIENT_DATA_" + username] = null;
            }
            catch (Exception)
            {

            }
        }

        #endregion




        #region Background Stuffs
        private void CronJobs()
        {

            //_withdrawRepo = new WithdrawRepository();


            _bgWorkerDisburse = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            _bgWorkerDisburse.DoWork += DisburseWithdrawOnDoWork;
            _bgWorkerDisburse.ProgressChanged += DisburseWithdrawProgressChanged;
            _bgWorkerDisburse.RunWorkerCompleted += DisburseWithdrawOnRunWorkerCompleted;
           
            if (_bgWorkerDisburse.IsBusy != true)
            {
                _bgWorkerDisburse.RunWorkerAsync();
            }

        }

        private static void DisburseWithdrawProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {

            var text = (progressChangedEventArgs.ProgressPercentage.ToString(CultureInfo.InvariantCulture) + "%");
        }


        private static void DisburseWithdrawOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            try
            {
                //_withdrawRepo.LogProcessedDisburse();

                if (runWorkerCompletedEventArgs.Cancelled)
                {
                    var text = "Canceled!";
                }

                if (runWorkerCompletedEventArgs.Error != null)
                {
                    var text = ("Error: " + runWorkerCompletedEventArgs.Error.Message);
                }

                else
                {
                    var text = "Done!";
                }
            }
            catch (Exception ex)
            {

            }

        }
        private static void DisburseWithdrawOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {

            var worker = sender as BackgroundWorker;
            if (worker != null)
            {
                
                if (worker.CancellationPending)
                {
                    doWorkEventArgs.Cancel = true;
                    CancelWorkerOperation(worker);
                    return;
                }

                // Perform a time consuming operation and report progress.
                Thread.Sleep(3000);
                
                
                // Call Repository to implement actual operation
                ServiceChurch.Mailing();

                // I will use this guy, after everything went successfull (i.e. 100% Done)
                worker.ReportProgress((100));

            }

        }
        private static void CancelWorkerOperation(BackgroundWorker worker)
        {
            if (worker.WorkerSupportsCancellation)
            {
                worker.CancelAsync();
            }
        }

        #endregion

        

    }
}
