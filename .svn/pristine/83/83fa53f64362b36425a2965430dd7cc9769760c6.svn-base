using System;
using System.Collections.Generic;
using System.Linq;
using ICASStacks.DataContract;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ClientStructureChurchDetailRepository
    {
        private readonly IIcasRepository<ClientStructureChurchDetail> _repository;
        private readonly IcasUoWork _uoWork;

        public ClientStructureChurchDetailRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ClientStructureChurchDetail>(_uoWork);
        }


        internal ClientStructureChurchDetail GetClientChurchStructureDetailByClientId(long clientId)
        {
            try
            {
                if(clientId < 1) return null;
                var myItem = _repository.GetAll(x => x.ClientId == clientId).ToList();
                if(!myItem.Any()) return null;
                return myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<ClientStructureChurchDetail> GetClientChurchStructureDetailsByClientId(long clientId)
        {
            try
            {
                if (clientId < 1) return null;
                var myItem = _repository.GetAll(x => x.ClientId == clientId).ToList();
                if (!myItem.Any()) return null;
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal bool IsDuplicate(long churchId, long clientId, long churchStructureId, int structureTypeId, out string msg)
        {
            try
            {
                List<ClientStructureChurchDetail> check;
                if (churchId > 0 && clientId > 0 && churchStructureId > 0 && structureTypeId > 0)
                {
                    var sql =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ClientChurchStructureDetail\" WHERE \"ChurchId\" = {0}" +
                            " AND \"ClientId\" = {1}" +
                            " AND \"ChurchStructureId\" = {2}" +
                            " AND \"ChurchStructureId\" = {3}",
                            churchId, clientId, churchStructureId, structureTypeId);
                    check = _repository.RepositoryContext().Database.SqlQuery<ClientStructureChurchDetail>(sql).ToList();
                }
                else
                {
                    check = null;
                }

                if (check != null)
                {
                    if (check.Count > 0)
                    {
                        var structureName = ServiceChurch.GetChurchStructureTypeNameById(structureTypeId);
                        if (!string.IsNullOrEmpty(structureName))
                        {
                            msg = "Duplicate Error! Another Church under this parent church with the same " +structureName+ " already exist";
                            return true;
                        }
                        msg = "Duplicate Error! Another Church under this parent church with the same structure already exist";
                        return true;
                    }

                    msg = "";
                    return false;
                }


                msg = "Unable to check duplicate! Please try again later";
                return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
    }
}
