using System;
using System.Linq;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchServiceAttendanceClientChurchCollectionRepository
    {

        private readonly IIcasRepository<ClientChurchCollection> _repository;
        private readonly IcasUoWork _uoWork;

        public ChurchServiceAttendanceClientChurchCollectionRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ClientChurchCollection>(_uoWork);
        }



        internal ClientChurchCollection GetChurchServiceAttendanceClientCollectionByChurchServiceAttendanceId(long churchServiceAttendanceId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ClientChurchCollection\"  WHERE \"ChurchServiceAttendanceId\" = {0};", churchServiceAttendanceId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ClientChurchCollection>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Service Attendance Collection record found!";
                    return new ClientChurchCollection();
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Service Attendance Collection Record!";
                    return new ClientChurchCollection();
                }
                msg = "";
                return check[0];
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientChurchCollection();
            }
        }
    }
}
