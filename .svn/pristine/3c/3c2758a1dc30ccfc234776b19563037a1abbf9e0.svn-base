using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.Areas.Admin.Manager;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Controllers
{
    public class ChartController : Controller
    {
        public JsonResult GetChart(string start, string end)
        {
            try
            {
                start = DateScrutnizer.ReverseToServerDate(start.Replace('-', '/'));
                end = DateScrutnizer.ReverseToServerDate(end.Replace('-', '/'));
                string msg;

                //var check = ServiceChurch.GetAndComputeChurchServiceAttendanceRemittance(3, 1, start, end);


                // 2017-09-15 to 2017-10-05
                //var start = "2017-09-15";
                //var end = "2017-10-05";
                ////var clientId = 1;
                ////var churchId = 3;
                //start = DateScrutnizer.ReverseToServerDate(start.Replace('-', '/'));
                //end = DateScrutnizer.ReverseToServerDate(end.Replace('-', '/'));


                //var remittance = ServiceChurch.GetAndComputeChurchServiceAttendanceRemittance(3, 1, start, end);






                //var tranx = CustomManager.GetDonationTransactions(start, end);
                //if (tranx != null)
                //{
                //    var donationTransactions = tranx;
                //    List<ChartObj> donationData;
                //    if (start == end)
                //    {

                //        donationData = (from transaction in donationTransactions.OrderBy(m => m.DonationTransactionId)
                //                        let tranxDate = transaction.TransactionTimeStamp.Substring(0, 2)
                //                        group transaction by tranxDate
                //                            into g
                //                            select new ChartObj
                //                            {
                //                                TransactionDate = ConvertTime(g.Key),
                //                                TotalAmount = g.Sum(x => x.TransactionAmount),
                //                            }
                //                    ).ToList();
                //    }
                //    else
                //    {
                //        donationData = (from transaction in donationTransactions
                //                        let tranxDate = transaction.TransactionTimeStamp.Substring(0, 10)
                //                        group transaction by tranxDate into g
                //                        select new ChartObj
                //                        {

                //                            TransactionDate = DateScrutnizer.ReverseToGeneralDate(g.Key),
                //                            TotalAmount = g.Sum(x => x.TransactionAmount),
                //                        }).ToList();
                //    }


                //    if (!donationData.Any())
                //    {
                //        donationData = new List<ChartObj>();
                //    }
                //    return Json(new { xRec = donationData.Select(m => m.TransactionDate).OrderBy(m => m), yRec = donationData.Select(m => m.TotalAmount).OrderBy(m => m) }, JsonRequestBehavior.AllowGet);
                //}

                return null;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        private string ConvertTime(string hours)
        {
            try
            {
                var time = DateTime.ParseExact(hours, "HH", CultureInfo.CurrentCulture).ToString("hh tt");
                return time;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return hours;
            }
        }
	}
}