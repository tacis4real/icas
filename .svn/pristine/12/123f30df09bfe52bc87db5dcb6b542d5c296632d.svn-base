using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICASStacks.DataContract;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class BankRepository
    {

        private readonly IIcasRepository<Bank> _repository;
        private readonly IcasUoWork _uoWork;


        public BankRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<Bank>(_uoWork);
        }


        internal Bank GetBank(int bankId)
        {
            try
            {
                var myItem = _repository.GetById(bankId);
                if (myItem == null || myItem.BankId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
    }
}
