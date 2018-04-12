﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.BioEnroll;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository;
using WebAdminStacks.APIObjs;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;
using RespStatus = ICASStacks.APIObjs.RespStatus;

namespace ICASStacks.StackService
{
    public class ServiceChurch
    {

      
        #region Background Mailing Jobs

        public static void Mailing()
        {
            new ClientChurchRepository().Mailing();
        }

        #endregion





        #region Church Structures Parishes

        public static List<RegisteredStructureChurchParishReportObj> GetChurchStructureParishHeadQuartersByChurchStateId(long churchId, int stateId, int churchStructureTypeId)
        {
            return new ChurchStructureParishHeadQuarterRepository().GetChurchStructureParishHeadQuartersByChurchStateId(churchId, stateId, churchStructureTypeId);
        }

        public static StructureChurchHeadQuarterParish GetParishByStructureChurchHeadQuarterParishId(long churchId, int stateId, string structureChurchHeadQuarterParishId)
        {
            return new ChurchStructureParishHeadQuarterRepository().GetParishByStructureChurchHeadQuarterParishId(churchId, stateId, structureChurchHeadQuarterParishId);
        }

        public static ChurchStructureParishHeadQuarter IsChurchStructureParishHeadQuarterExist(long churchId, int stateId, int churchStructureTypeId)
        {
            return new ChurchStructureParishHeadQuarterRepository().IsChurchStructureParishHeadQuarterExist(churchId, stateId, churchStructureTypeId);
        }


        public static bool RemoveChurchStructureChurchHeadQuarterParish(long churchId, int stateId, string structureChurchHeadQuarterParishId, out string msg)
        {
            return new ChurchStructureParishHeadQuarterRepository().RemoveChurchStructureChurchHeadQuarterParish(churchId, stateId, structureChurchHeadQuarterParishId, out msg);
        }


        #endregion




        #region Church

        public static ChurchRegResponse AddChurch(ChurchRegObj churchRegObj)
        {
            return new ChurchRepository().AddChurch(churchRegObj);
        }

        public static RespStatus UpdateChurch(ChurchRegObj churchRegObj)
        {
            return new ChurchRepository().UpdateChurch(churchRegObj);
        }

        public static Church GetChurch(long id)
        {
            return new ChurchRepository().GetChurch(id);
        }



        public static List<Church> GetChurches()
        {
            return new ChurchRepository().GetChurches();
        }

        public static List<RegisteredChurchReportObj> GetAllRegisteredChurchObjs()
        {
            return new ChurchRepository().GetAllRegisteredChurchObjs();
        }
        #endregion
        

        #region Church Theme Setting

        public static ChurchThemeSettingRegResponse AddChurchThemeSetting(ChurchThemeSettingRegObj churchThemeSettingRegObj)
        {
            return new ChurchThemeSettingRepository().AddChurchThemeSetting(churchThemeSettingRegObj);
        }

        public static RespStatus UpdateChurchThemeSetting(ChurchThemeSettingRegObj churchThemeSettingRegObj)
        {
            return new ChurchThemeSettingRepository().UpdateChurchThemeSetting(churchThemeSettingRegObj);
        }

        public static List<RegisteredChurchThemeReportObj> GetAllRegisteredChurchThemeSettingObjs()
        {
            return new ChurchThemeSettingRepository().GetAllRegisteredChurchThemeSettingObjs();
        }


        //public static ChurchThemeSetting GetChurchThemeSettingByChurchId(long churchId, out string msg)
        //{
        //    return new ChurchThemeSettingRepository().GetChurchThemeSettingByChurchId(churchId, out msg);
        //}
        #endregion

        #region Clients & Client Church

        #region Client Church

        #region Calling & Use of Handler from another Context
        //var processedParishHqtr =
        //    new ChurchStructureParishHeadQuarterRepository()._repository.Update(
        //        existingParishHeadQuarter);

        //public static IIcasRepository<ChurchStructureParishHeadQuarter> ClientChurchHandler()
        //{
        //    return new ClientChurchRepository()._churchStructureParishHeadQuarterRepository;
        //}
        #endregion
        

        public static ClientChurchRegResponse AddClientChurch(ClientChurchRegistrationObj clientChurchRegObj)
        {
            return new ClientChurchRepository().AddClientChurch(clientChurchRegObj);
        }

        public static RespStatus UpdateClientChurch(ClientChurchRegistrationObj clientChurchRegObj)
        {
            return new ClientChurchRepository().UpdateClientChurch(clientChurchRegObj);
        }


        public static List<RegisteredClientChurchReportObj> GetAllRegisteredClientChurchObjs()
        {
            return new ClientChurchRepository().GetAllRegisteredClientChurchObjs();
        }


        public static ChurchThemeSetting GetClientChurchThemeDetail(long clientChurchId)
        {
            return new ClientChurchRepository().GetClientChurchThemeDetail(clientChurchId);
        }

        #endregion






        #region Client Bank

        public static Bank GetBank(int bankId)
        {
            return new BankRepository().GetBank(bankId);
        }


        #endregion



        public static ClientRegResponse AddClient(ClientRegistrationObj clientRegObj)
        {
            return new ClientRepository().AddClient(clientRegObj);
        }
        
        public static ClientAccountRegResponse AddClientAccount(ClientAccountRegistrationObj clientAccountRegObj)
        {
            return new ClientRepository().AddClientAccount(clientAccountRegObj);
        }

        public static ClientStructureChurchRegResponse AddClientStructureChurch(ClientStructureChurchRegistrationObj clientStructureChurchRegObj)
        {
            return new ClientRepository().AddClientStructureChurch(clientStructureChurchRegObj);
        }
        
        public static RespStatus UpdateClient(ClientRegistrationObj client)
        {
            return new ClientRepository().UpdateClient(client);
        }

        public static ClientStructureTaskResponseObj ResetClientStructureChurch(long clientId)
        {
            return new ClientRepository().ResetClientStructureChurch(clientId);
        }


        public static Client GetClient(long clientId)
        {
            return new ClientRepository().GetClient(clientId);
        }

        public static ChurchThemeSetting GetClientChurchThemeInfo(long clientId)
        {
            return new ClientRepository().GetClientChurchThemeInfo(clientId);
        }

        public static RespStatus UpdateClientAccount(ClientAccountRegistrationObj clientAccount)
        {
            return new ClientRepository().UpdateClientAccount(clientAccount);
        }

        public static List<RegisteredClientReportObj> GetClientObjs()
        {
            return new ClientRepository().GetClientObjs();
        }

        public static List<RegisteredClientAccountListReportObj> GetAllRegisteredClientAccountObjs()
        {
            return new ClientRepository().GetAllRegisteredClientAccountObjs();
        }

        public static List<RegisteredClientListReportObj> GetAllRegisteredClientObjs()
        {
            return new ClientRepository().GetAllRegisteredClientObjs();
        }

        public static List<RegisteredClientStructureChurchListReportObj> GetAllRegisteredClientStructureChurchListObjs()
        {
            return new ClientRepository().GetAllRegisteredClientStructureChurchListObjs();
        }



        #region Client Profile
        public static List<RegisteredClientProfileReportObj> GetAllRegisteredClientProfileObjs()
        {
            var items = PortalClientUser.GetAllRegisteredClientProfileObjs();
            if (!items.Any())
            {
                return new List<RegisteredClientProfileReportObj>();
            }

            foreach (var item in items)
            {
                var church = new ClientRepository().GetClientChurch(item.ClientId);
                item.ChurchId = (church.ChurchId < 1 ? 0 : church.ChurchId);
                item.ParentChurch = church.Name ?? "No Parent Found";
                item.ParentChurchShortName = church.ShortName ?? "No Parent Found";
                item.Client = new ClientRepository().GetClient(item.ClientId).Name ?? "No Client Found";
            }

            return items;
        }
        #endregion

        #endregion


        #region Church Administrative

        public static ChurchMemberRegResponse AddChurchMember(ChurchMemberRegistrationObj churchMemberRegObj)
        {
            return new ChurchMemberRepository().AddChurchMember(churchMemberRegObj);
        }

        #region Church Services

        public static ChurchServiceTypeRegResponse AddChurchServiceType(ChurchServiceTypeRegObj churchServiceTypeRegObj)
        {
            return new ChurchServiceTypeRepository().AddChurchServiceType(churchServiceTypeRegObj);
        }

        public static RespStatus UpdateChurchServiceType(ChurchServiceTypeRegObj churchServiceTypeRegObj)
        {
            return new ChurchServiceTypeRepository().UpdateChurchServiceType(churchServiceTypeRegObj);
        }

        public static List<ChurchServiceDetailObj> GetChurchServiceServiceTypeDetail(long churchId)
        {
            return new ChurchServiceRepository().GetChurchServiceServiceTypeDetail(churchId);
        }



        public static List<RegisteredChurchServiceTypeReportObj> GetAllRegisteredChurchServiceTypeObjs()
        {
            return new ChurchServiceTypeRepository().GetAllRegisteredChurchServiceTypeObjs();
        }

        public static ParentChurchServiceRegObj GetParentChurchServiceRegObj()
        {
            return new ChurchServiceRepository().GetParentChurchServiceRegObj();
        }

        public static ParentChurchServiceRegObj GetParentChurchServiceRegObj(long churchId)
        {
            return new ChurchServiceRepository().GetParentChurchServiceRegObj(churchId);
        }

        public static ParentChurchServiceRegObj GetParentChurchServiceDetailObj(long churchId)
        {
            return new ChurchServiceRepository().GetParentChurchServiceDetailObj(churchId);
        }

        public static ParentChurchServiceRegResponse AddParentChurchService(ParentChurchServiceRegObj churchServiceRegObj)
        {
            return new ChurchServiceRepository().AddParentChurchService(churchServiceRegObj);
        }


        #region Church

        #endregion

        #region Client Church



        public static ClientChurchServiceRegResponse AddClientChurchService(ClientChurchServiceRegObj clientChurchServiceRegObj)
        {
            return new ClientChurchServiceRepository().AddClientChurchService(clientChurchServiceRegObj);
        }


        public static RespStatus UpdateClientChurchServiceType(ClientChurchService agentObj)
        {
            return new ClientChurchServiceRepository().UpdateClientChurchServiceType(agentObj);
        }

        public static RespStatus UpdateClientChurchService(ClientChurchServiceRegObj clientChurchServiceRegObj)
        {
            return new ClientChurchServiceRepository().UpdateClientChurchService(clientChurchServiceRegObj);
        }

        public static ClientChurchService GetClientChurchServiceTypeReportObj(long clientChurchId)
        {
            return new ClientChurchServiceRepository().GetClientChurchServiceTypeReportObj(clientChurchId);
        }

        public static List<ChurchServiceDetailObj> GetClientChurchService(long clientChurchId)
        {
            return new ClientChurchServiceRepository().GetClientChurchService(clientChurchId);
        }

        public static ChurchServiceDetailObj GetClientChurchServiceDetail(long clientChurchId, string churchServiceTypeRefId)
        {
            return new ClientChurchServiceRepository().GetClientChurchServiceDetail(clientChurchId, churchServiceTypeRefId);
        }


        public static RespStatus RemoveClientChurchService(long churchServiceId)
        {
            return new ClientChurchServiceRepository().RemoveClientChurchService(churchServiceId);
        }

        public static List<RegisteredClientChurchServiceReportObj> GetAllRegisteredClientChurchServiceObjs(long clientId)
        {
            return new ClientChurchServiceRepository().GetAllRegisteredClientChurchServiceObjs(clientId);
        }
        
        #endregion

        #region Service Type
        public static List<ChurchServiceType> GetChurchServiceTypes()
        {
            return new ChurchServiceTypeRepository().GetChurchServiceTypes();
        }

        public static string GetChurchServiceTypeNameById(int churchServiceTypeId)
        {
            return new ChurchServiceTypeRepository().GetChurchServiceTypeNameById(churchServiceTypeId);
        }
        #endregion

        #endregion
        

        #region Church Service Attendance
        public static ChurchServiceAttendanceRegResponse AddChurchServiceAttendance(ChurchServiceAttendanceRegObj churchServiceAttendanceRegObj)
        {
            return new ChurchServiceAttendanceRepository().AddChurchServiceAttendance(churchServiceAttendanceRegObj);
        }

        public static RespStatus UpdateChurchServiceAttendance(ChurchServiceAttendanceRegObj churchServiceAttendanceRegObj)
        {
            return new ChurchServiceAttendanceRepository().UpdateChurchServiceAttendance(churchServiceAttendanceRegObj);
        }

        public static List<ChurchServiceAttendanceCollectionObj> GetClientChurchCollectionObj(long clientId)
        {
            return new ClientChurchCollectionTypeRepository().GetClientChurchCollectionObj(clientId);
        }

        

        #region Unsed

        //public static ChurchServiceAttendanceRemittance ComputeRemittance(long churchId, long clientId, int year, int month)
        //{
        //    return new ChurchServiceAttendanceRemittanceRepository().ComputeRemittance(churchId, clientId, year, month);
        //}

        //public static ChurchServiceAttendanceRemittance ComputeRemittanceByDateRange(long churchId, long clientId, string start, string end)
        //{
        //    return new ChurchServiceAttendanceRemittanceRepository().ComputeRemittanceByDateRange(churchId, clientId, start, end);
        //}

        //public static ChurchServiceAttendanceRemittance GetAndComputeChurchServiceAttendanceRemittance(long churchId, long clientId, string start, string end)
        //{
        //    return new ChurchServiceAttendanceRemittanceRepository().GetAndComputeChurchServiceAttendanceRemittance(churchId, clientId, start, end);
        //}

        //public static List<ChurchServiceAttendanceRemittanceReportObj> GetChurchServiceAttendanceRemittances()
        //{
        //    return new ChurchServiceAttendanceRemittanceRepository().GetChurchServiceAttendanceRemittances();
        //}
        #endregion


        #region Church Service Attendance Remittance

        public static RespStatus SetClientChurchCollectionTypeForRemittance(ClientChurchCollectionTypeSettingObjs clientChurchCollectionTypeSettingObjs)
        {
            return new ClientChurchCollectionTypeRepository().SetClientChurchCollectionTypeForRemittance(clientChurchCollectionTypeSettingObjs);
        }

       

        

        

        public static ChurchServiceAttendanceRemittanceReportObj GetClientChurchServiceAttendanceRemittance(long churchId, long clientId, string start, string end)
        {
            return new ChurchServiceAttendanceRemittanceRepository().GetClientChurchServiceAttendanceRemittance(churchId, clientId, start, end);
        }
        #endregion
        

        #endregion
        

        

        public static BulkChurchMemberRegResponseObj AddBulkChurchMemberInfo(List<ChurchMemberRegistrationObj> bulkItems, HttpPostedFileBase uploadedFile)
        {
            return new ChurchMemberRepository().AddBulkChurchMemberInfo(bulkItems, uploadedFile);
        }

        public static RespStatus UpdateChurchMember(ChurchMemberRegistrationObj churchMemberRegObj)
        {
            return new ChurchMemberRepository().UpdateChurchMember(churchMemberRegObj);
        }

        public static List<RegisteredChurchMemberReportObj> GetAllRegisteredChurchMemberObjs(long clientId)
        {
            return new ChurchMemberRepository().GetAllRegisteredChurchMemberObjs(clientId);
        }


        public static List<RegisteredChurchServiceReportObj> GetAllRegisteredChurchServiceObjs()
        {
            return new ChurchServiceRepository().GetAllRegisteredChurchServiceObjs();
        }

        public static List<RegisteredChurchServiceReportObj> GetAllRegisteredChurchServiceObjs(long churchId)
        {
            return new ChurchServiceRepository().GetAllRegisteredChurchServiceObjs(churchId);
        }

        public static ChurchService GetChurchService(long churchId)
        {
            return new ChurchServiceRepository().GetChurchService(churchId);
        }

        public static List<RegisteredChurchServiceAttendanceReportObj> GetAllRegisteredChurchServiceAttendanceObjs(long clientChurchId)
        {
            return new ChurchServiceAttendanceRepository().GetAllRegisteredChurchServiceAttendanceObjs(clientChurchId);
        }

        #region Collection Types
        

        public static RespStatus UpdateClientChurchCollectionType(ClientChurchCollectionType agentObj)
        {
            return new ClientChurchCollectionTypeRepository().UpdateClientChurchCollectionType(agentObj);
        }

        public static ClientChurchCollectionTypeSettingObjs GetClientChurchCollectionTypeForSettings(long clientId)
        {
            return new ClientChurchCollectionTypeRepository().GetClientChurchCollectionTypeForSettings(clientId);
        }

        public static List<ClientChurchServiceAttendanceCollectionObj> GetClientChurchCollectionTypesByClientChurchId(long clientChurchId)
        {
            return new ClientChurchCollectionTypeRepository().GetClientChurchCollectionTypesByClientChurchId(clientChurchId);
        }

        public static List<ChurchServiceAttendanceCollectionObj> GetChurchCollectionTypesByChurchId(long churchId)
        {
            return new ChurchCollectionTypeRepository().GetChurchCollectionTypesByChurchId(churchId);
        }

        public static ClientChurchCollectionType GetClientChurchCollectionTypeReportObj(long clientChurchId)
        {
            return new ClientChurchCollectionTypeRepository().GetClientChurchCollectionTypeReportObj(clientChurchId);
        }


        public static List<ClientChurchServiceAttendanceCollectionObj> GetClientChurchCollectionObjs(long clientChurchId)
        {
            return new ClientChurchCollectionTypeRepository().GetClientChurchCollectionObjs(clientChurchId);
        }

        public static ClientChurchCollectionTypeReportObj GetClientChurchCollectionTypeReportObjs(long clientChurchId)
        {
            return new ClientChurchCollectionTypeRepository().GetClientChurchCollectionTypeReportObjs(clientChurchId);
        }


        #region Unsed

        public static ClientChurchTreasuryRegistrationObj GetClientChurchTreasuryCollectionTypeRegObj(long clientId)
        {
            return new ClientChurchTreasuryRepository().GetClientChurchTreasuryCollectionTypeRegObj(clientId);
        }

        public static ParentChurchCollectionTypeRegResponse AddParentChurchCollectionType(ParentChurchCollectionTypeRegObj parentChurchCollectionTypeRegObj)
        {
            return new ChurchCollectionTypeRepository().AddParentChurchCollectionType(parentChurchCollectionTypeRegObj);
        }

        public static List<RegisteredChurchCollectionTypeListReportObj> GetAllRegisteredChurchCollectionTypeObjs()
        {
            return new ChurchCollectionTypeRepository().GetAllRegisteredChurchCollectionTypeObjs();
        }

        public static ParentChurchCollectionTypeResponseObj EnableChurchCollectionType(long churchId, int collectionTypeId)
        {
            return new ChurchCollectionTypeRepository().EnableChurchCollectionType(churchId, collectionTypeId);
        }

        public static ParentChurchCollectionTypeResponseObj RemoveChurchCollectionType(long churchId, int collectionTypeId)
        {
            return new ChurchCollectionTypeRepository().RemoveChurchCollectionType(churchId, collectionTypeId);
        }


        public static List<CollectionType> GetCollectionTypes()
        {
            return new CollectionTypeRepository().GetCollectionTypes();
        }
        public static int GetCollectionTypeIdByName(string typeName)
        {
            return new CollectionTypeRepository().GetCollectionTypeIdByName(typeName);
        }

        public static string GetChurchCollectionTypeNameById(int typeId)
        {
            return new CollectionTypeRepository().GetChurchCollectionTypeNameById(typeId);
        }

       
        public static ChurchCollectionType GetChurchCollectionType(long churchId)
        {
            return new ChurchCollectionTypeRepository().GetChurchCollectionType(churchId);
        }

        public static List<CollectionTypeObj> GetChurchCollectionTypeDetail(long churchId)
        {
            return new ChurchCollectionTypeRepository().GetChurchCollectionTypeDetail(churchId);
        }

        //GetChurchCollectionTypeForSetting
        public static ChurchCollectionTypeSettingObjs GetChurchCollectionTypeForSetting(long churchId)
        {
            return new ChurchCollectionTypeRepository().GetChurchCollectionTypeForSetting(churchId);
        }

        public static ClientChurchCollectionTypeSettingObjs GetClientChurchCollectionTypeForSetting(long clientId, long churchId)
        {
            return new ClientChurchCollectionTypeRepository().GetClientChurchCollectionTypeForSetting(clientId, churchId);
        }

        #endregion


        #endregion

        #endregion




        #region Church Structure Types
        public static List<ChurchStructureType> GetChurchStructureTypes()
        {
            return new ChurchStructureTypeRepository().GetChurchStructureTypes();
        }

        public static int GetChurchStructureTypeIdByName(string typeName)
        {
            return new ChurchStructureTypeRepository().GetChurchStructureTypeIdByName(typeName);
        }

        public static string GetChurchStructureTypeNameById(int typeId)
        {
            return new ChurchStructureTypeRepository().GetChurchStructureTypeNameById(typeId);
        }
        #endregion


        #region Structure Church
        public static List<ChurchStructureType> GetChurchStructureTypeByStructureChurchId(long typeStructureChurchId)
        {
            return new StructureChurchRepository().GetChurchStructureTypeByTypeStructureChurchId(typeStructureChurchId);
        }
        #endregion

        


        #region Church Structures

        public static List<ChurchStructuresLookUpObj> GetChurchStructureHierachyLookUp(long churchId)
        {
            return new ChurchStructureRepository().GetChurchStructureHierachyLookUp(churchId);
        }

        public static List<ChurchStructureType> GetDisplayActiveChurchStructureTypes(long churchId, int structureTypeId)
        {
            return new ChurchStructureRepository().GetDisplayActiveChurchStructureTypes(churchId, structureTypeId);
        }

        public static ChurchStructureRegResponse AddChurchStructure(ChurchStructureRegObj churchStructureRegObj)
        {
            return new ChurchStructureRepository().AddChurchStructure(churchStructureRegObj);
        }

        public static RespStatus UpdateChurchStructureWithHierachys(ChurchStructureHierachyResetObj churchStructureHierachyResetObj)
        {
            return new ChurchStructureRepository().UpdateChurchStructureWithHierachys(churchStructureHierachyResetObj);
        }

        public static ChurchStructure GetChurchStructure(long churchId, int structureTypeId)
        {
            return new ChurchStructureRepository().GetChurchStructure(churchId, structureTypeId);
        }

        public static List<RegisteredChurchStructureListReportObj> GetAllRegisteredChurchStructureObjs()
        {
            return new ChurchStructureRepository().GetAllRegisteredChurchStructureObjs();
        }

        public static List<ChurchStructureType> GetChurchStructureTypeByChurchId(long churchId)
        {
            return new ChurchStructureRepository().GetChurchStructureTypeByChurchId(churchId);
        }

        public static ChurchStructureResponseObj EnableChurchStructure(long churchId, int structureTypeId)
        {
            return new ChurchStructureRepository().EnableChurchStructure(churchId, structureTypeId);
        }

        public static ChurchStructureResponseObj DisableChurchStructure(long churchId, int structureTypeId)
        {
            return new ChurchStructureRepository().DisableChurchStructure(churchId, structureTypeId);
        }

        public static ChurchStructureResponseObj RemoveChurchStructure(long churchId, int structureTypeId)
        {
            return new ChurchStructureRepository().RemoveChurchStructure(churchId, structureTypeId);
        }

        public static bool IsChurchStructureActive(long churchId, int structureTypeId)
        {
            return new ChurchStructureRepository().IsChurchStructureActive(churchId, structureTypeId);
        }
        #endregion


        #region Structure Churches
        public static StructureChurchRegResponse AddStructureChurch(StructureChurchRegObj structureChurchRegObj)
        {
            return new StructureChurchRepository().AddStructureChurch(structureChurchRegObj);
        }

        //internal RespStatus UpdateStructureChurch(StructureChurchRegObj structureChurchRegObj)
        public static RespStatus UpdateStructureChurch(StructureChurchRegObj structureChurchRegObj)
        {
            return new StructureChurchRepository().UpdateStructureChurch(structureChurchRegObj);
        }


        public static List<RegisteredStructureChurchReportObj> GetAllRegisteredStructureChurchObjs()
        {
            return new StructureChurchRepository().GetAllRegisteredStructureChurchObjs();
        }

        public static bool IsDuplicate<T>(long churchId, string name, out string msg) where T : class
        {
            return new StructureChurchRepository().IsDuplicate(churchId, name, out msg);
        }
        #endregion


        #region Others

        public static ClientStructureChurchDetail GetClientChurchStructureDetailByClientId(long clientId)
        {
            return new ClientStructureChurchDetailRepository().GetClientChurchStructureDetailByClientId(clientId);
        }

        public static List<ClientStructureChurchDetail> GetClientChurchStructureDetailsByClientId(long clientId)
        {
            return new ClientStructureChurchDetailRepository().GetClientChurchStructureDetailsByClientId(clientId);
        }


        public static ChurchStructureHqtr GetChurchStructureHqtrByClientId(long clientId)
        {
            return new ChurchStructureHqtrRepository().GetChurchStructureHqtrByClientId(clientId);
        }
        

        public static bool IsChurchStructureHqtrDuplicatex(long churchId, long structureHqrtId, long structureTypeId, out string msg)
        {
            return new ChurchStructureHqtrRepository().IsDuplicatex(churchId, structureHqrtId, structureTypeId, out msg);
        }




        public static bool IsChurchStructureHqtrDuplicate(ChurchStructureHqtr hqtrObj, out string msg, int status = 0)
        {
            return new ChurchStructureHqtrRepository().IsDuplicate(hqtrObj, out msg, status);
        }

        public static bool IsChurchStructureHqtrDuplicatexx(ChurchStructureHqtr hqtrObj, out string msg)
        {
            return new ChurchStructureHqtrRepository().IsDuplicate(hqtrObj, out msg);
        }


        public static bool IsClientChurchStructureDetailDuplicate(long churchId, long clientId, long churchStructureId, int structureTypeId, out string msg)
        {
            return new ClientStructureChurchDetailRepository().IsDuplicate(churchId, clientId, churchStructureId, structureTypeId, out msg);
        }
        #endregion




        #region Church Structures Hirachy
        //public static ChurchStructureHierachyRegResponse AddChurchStructureHierachy(ChurchStructureHierachyRegObj churchStructureHierachyRegObj)
        //{
        //    return new ChurchStructureHierachyRepository().AddChurchStructureHierachy(churchStructureHierachyRegObj);
        //}

        //public static List<RegisteredChurchStructureHierachyReportObj> GetAllRegisteredChurchStructureHierachyObjs()
        //{
        //    return new ChurchStructureHierachyRepository().GetAllRegisteredChurchStructureHierachyObjs();
        //}

        //public static ChurchStructureHierachyRegResponse AddChurchStructureHierachys(ChurchStructureHierachyRegObj churchStructureHierachyRegObj)
        //{
        //    return new ChurchStructureHierachyRepository().AddChurchStructureHierachys(churchStructureHierachyRegObj);
        //}

        //public static RespStatus UpdateChurchStructureHierachys(ChurchStructureHierachyRegObj churchStructureHierachyRegObj)
        //{
        //    return new ChurchStructureHierachyRepository().UpdateChurchStructureHierachys(churchStructureHierachyRegObj);
        //}

        //public static List<ChurchStructureType> GetDisplayChurchStructureTypes(long churchId, int structureId)
        //{
        //    return new ChurchStructureRepository().GetDisplayActiveChurchStructureTypes(churchId, structureId);
        //}

        ////public static List<ChurchStructureType> GetDisplayChurchStructureTypes(long churchId, int structureId)
        ////{
        ////    return new ChurchStructureHierachyRepository().GetDisplayActiveChurchStructureTypes(churchId, structureId);
        ////}

        //public static List<ChurchStructureHierachyLookUpObj> GetChurchStructureHierachyLookUp(long churchId)
        //{
        //    return new ChurchStructureHierachyRepository().GetChurchStructuresHierachyLookUp(churchId);
        //}
        #endregion

        
    }
}
