using System;
using System.Linq;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchServiceAttendanceAttendeeRepository
    {

        private readonly IIcasRepository<ChurchServiceAttendanceAttendee> _repository;
        private readonly IcasUoWork _uoWork;

        public ChurchServiceAttendanceAttendeeRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchServiceAttendanceAttendee>(_uoWork);
        }



        internal ChurchServiceAttendanceAttendee GetChurchServiceAttendanceAttendeeByChurchServiceAttendanceId(long churchServiceAttendanceId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ChurchServiceAttendanceAttendee\"  WHERE \"ChurchServiceAttendanceId\" = {0};", churchServiceAttendanceId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ChurchServiceAttendanceAttendee>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Service Attendance Attendee record found!";
                    return new ChurchServiceAttendanceAttendee();
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Service Attendance Record!";
                    return new ChurchServiceAttendanceAttendee();
                }
                msg = "";
                return check[0];
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ChurchServiceAttendanceAttendee();
            }
        }

    }
}
