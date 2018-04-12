using System;
using System.Collections.Generic;
using System.Linq;
using ICASStacks.APIObjs;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ClientChurchTreasuryRepository
    {
        private readonly IIcasRepository<ClientChurchTreasury> _repository;
        private readonly IIcasRepository<ClientChurchCollectionType> _clientChurchCollectionTypeRepository;
        private readonly IIcasRepository<CollectionType> _collectionTypeRepository; 
        private readonly IcasUoWork _uoWork;


        public ClientChurchTreasuryRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ClientChurchTreasury>(_uoWork);
            _clientChurchCollectionTypeRepository = new IcasRepository<ClientChurchCollectionType>(_uoWork);
            _collectionTypeRepository = new IcasRepository<CollectionType>(_uoWork);
        }


        internal ClientChurchTreasuryRegistrationObj GetClientChurchTreasuryCollectionTypeRegObj(long clientChurchId)
        {
            try
            {

                if (clientChurchId < 1) { return null; }

                var myCollectionTypeItemLists =
                    _clientChurchCollectionTypeRepository.GetAll(x => x.ClientChurchId == clientChurchId).ToList();
                if (!myCollectionTypeItemLists.Any()) { return null; }
                var retObj = new ClientChurchTreasuryRegistrationObj();
                var thisClientChurchCollectionTypeObjs = new List<ClientChurchCollectionTypeObj>();


                myCollectionTypeItemLists.ForEachx(m =>
                {
                    //var thisCollectionType = _collectionTypeRepository.GetById(m.CollectionTypeId);
                    //thisClientChurchCollectionTypeObjs.Add(new ClientChurchCollectionTypeObj
                    //{
                    //    CollectionTypeId = m.CollectionTypeId,
                    //    Name = thisCollectionType.Name
                    //});
                });

                retObj.ChurchCollectionTypeObjs = thisClientChurchCollectionTypeObjs;
                return retObj;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientChurchTreasuryRegistrationObj();
            }
        }


        internal ClientChurchTreasuryRegistrationObj GetClientChurchTreasuryRegObj(long clientChurchId)
        {
            try
            {

                if (clientChurchId < 1) { return null; }

                var myCollectionTypeItemLists =
                    _clientChurchCollectionTypeRepository.GetAll(x => x.ClientChurchId == clientChurchId).ToList();
                if (!myCollectionTypeItemLists.Any()) { return null; }
                var retObj = new ClientChurchTreasuryRegistrationObj();
                var thisClientChurchCollectionTypeObjs = new List<ClientChurchCollectionTypeObj>();


                myCollectionTypeItemLists.ForEachx(m =>
                {
                    //var thisCollectionType = _collectionTypeRepository.GetById(m.CollectionTypeId);
                    //thisClientChurchCollectionTypeObjs.Add(new ClientChurchCollectionTypeObj
                    //{
                    //    CollectionTypeId = m.CollectionTypeId,
                    //    Name = thisCollectionType.Name
                    //});
                });

                retObj.ChurchCollectionTypeObjs = thisClientChurchCollectionTypeObjs;
                return retObj;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientChurchTreasuryRegistrationObj();
            }
        }

    }
}
