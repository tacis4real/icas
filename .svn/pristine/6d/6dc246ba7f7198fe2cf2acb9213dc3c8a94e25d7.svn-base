using System;
using System.Collections.Generic;
using System.Linq;
using ICASStacks.DataContract;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchStructureTypeRepository
    {

        private readonly IIcasRepository<ChurchStructureType> _repository;
        private readonly IcasUoWork _uoWork;

        public ChurchStructureTypeRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchStructureType>(_uoWork);
        }


        internal List<ChurchStructureType> GetChurchStructureTypes()
        {
            var myItemList = _repository.GetAll();
            if(myItemList == null || !myItemList.Any()) return null;
            return myItemList.ToList();
        }

        internal int GetChurchStructureTypeIdByName(string typeName)
        {
            try
            {
                if (string.IsNullOrEmpty(typeName)) return 0;
                var myItemList = _repository.GetAll();
                if (myItemList == null || !myItemList.Any()) return 0;

                var check = myItemList.Where(x => x.Name == typeName).ToList();
                return !check.Any() ? 0 : check[0].ChurchStructureTypeId;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        internal string GetChurchStructureTypeNameById(int typeId)
        {
            try
            {
                if (typeId < 1) return string.Empty;
                var myItem = _repository.GetById(typeId);
                if (myItem == null || myItem.ChurchStructureTypeId < 1) return string.Empty;

                return myItem.Name;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return string.Empty;
            }
        }
    }
}
