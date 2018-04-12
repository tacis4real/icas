using System;
using System.Collections.Generic;
using System.Linq;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class CollectionTypeRepository
    {

        private readonly IIcasRepository<CollectionType> _repository;
        private readonly IcasUoWork _uoWork;

        public CollectionTypeRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<CollectionType>(_uoWork);
        }


        internal List<CollectionType> GetCollectionTypes()
        {
            var myItemList = _repository.GetAll();
            if (myItemList == null || !myItemList.Any()) return null;
            return myItemList.ToList();
        }

        internal int GetCollectionTypeIdByName(string typeName)
        {
            try
            {
                if (string.IsNullOrEmpty(typeName)) return 0;
                var myItemList = _repository.GetAll();
                if (myItemList == null || !myItemList.Any()) return 0;

                var check = myItemList.Where(x => x.Name == typeName).ToList();
                return !check.Any() ? 0 : check[0].CollectionTypeId;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }


        internal string GetChurchCollectionTypeNameById(int typeId)
        {
            try
            {
                if (typeId < 1) return string.Empty;
                var myItem = _repository.GetById(typeId);
                if (myItem == null || myItem.CollectionTypeId < 1) return string.Empty;

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
