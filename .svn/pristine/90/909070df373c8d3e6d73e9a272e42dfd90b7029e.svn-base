using System;
using System.Linq;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ProfessionRepository
    {
        private readonly IIcasRepository<Profession> _repository;
        private readonly IcasUoWork _uoWork;

        public ProfessionRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<Profession>(_uoWork);
        }

        public int AddProfession(Profession profession, out string msg)
        {
            try
            {
                if (IsDuplicate(profession.Name, out msg))
                {
                    return -1;
                }

                var processedProfesession = _repository.Add(profession);
                _uoWork.SaveChanges();
                msg = "";
                if (processedProfesession.ProfessionId < 0)
                {
                    return -1;
                }
                return processedProfesession.ProfessionId;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        internal Profession GetProfession(string professionName)
        {
            try
            {
                var myItem = _repository.GetAll(m => string.Compare(m.Name, professionName, StringComparison.CurrentCultureIgnoreCase) == 0).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].ProfessionId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal bool IsProfessionExist(string professionName, out int id)
        {
            try
            {
                var check = GetProfession(professionName);
                if (check == null)
                {
                    id = -1;
                    return false;
                }
                id = check.ProfessionId;
                return true;
                //return check != null && check.ProfessionId >= 1;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                id = -1;
                return false;
            }
        }


        private bool IsDuplicate(string professionName, out string msg)
        {
            try
            {
                var sql1 =
                 String.Format(
                     "Select * FROM \"ChurchAPPDB\".\"Profession\" WHERE lower(\"Name\") = lower('{0}')", professionName);

                var check = _repository.RepositoryContext().Database.SqlQuery<Profession>(sql1).ToList();
                msg = "";
                if (check.IsNullOrEmpty()) return false;
                msg = "Duplicate Error! Profession already exist";
                return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }
    }
}
