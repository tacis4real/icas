using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using AwesomeMvc;
using ICAS.Areas.Admin.Manager;
using ICAS.Areas.Admin.Models.PortalModel;
using ICAS.Models;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.Enum;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Areas.Admin.Controllers.Awesome
{
    public class DataController : Controller
    {


        #region Load Structure Church Parishes
        public ActionResult LoadChurchStructureParishHeadQuarters(long churchId, int stateId, int churchStructureTypeId)
        {
            try
            {
                //var add = new KeyContent("", "Select a Parish HeadQuarter");
                var items = CustomManager.GetChurchStructureParishHeadQuarters(churchId, stateId, churchStructureTypeId);
                if (items == null || !items.Any())
                {
                    return Json(new List<StructureChurchHeadQuarterParish>(), JsonRequestBehavior.AllowGet);
                }

                return Json(items, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public ActionResult GetChurchStructureTypeByChurchId(long churchId)
        {
            try
            {
                var add = new KeyContent("", "Select Structure");
                var items = CustomManager.GetChurchStructureTypeByChurchId(churchId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ChurchStructureTypeId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion




        #region Administrative
        public ActionResult LoadProfessionals()
        {
            try
            {
                var professionalList = Enum.GetValues(typeof(Professional)).Cast<Professional>().Select(x => new NameAndValueObject
                {
                    Name = x.ToString().Replace("_", " "),
                    Id = ((int)x)
                }).ToList();
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new SelectList(
                              professionalList.OrderBy(m => m.Id),
                              "Id",
                              "Name"), JsonRequestBehavior.AllowGet
                              );
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult LoadProfessionx(long clientId)
        {
            try
            {
                var add = new KeyContent("", "Select Member Profession");
                var items = CustomManager.GetProfessionx(clientId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ProfessionId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult LoadProfessions()
        {
            try
            {
                var add = new KeyContent("", "Select Member Profession");
                var items = CustomManager.GetProfessions();
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ProfessionId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetRolesInChurch()
        {
            try
            {
                var add = new KeyContent("", "Select Member Role In Church");
                var items = CustomManager.GetRolesInChurch();
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                //var jsonitem = items.Select(o => new KeyContent(o.RoleInChurchId, o.Name)).ToList();
                var jsonitem = items.Select(o => new KeyContent(o.RoleInChurchId, "")).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetRolesInChurchs(long clientId)
        {
            try
            {
                var add = new KeyContent("", "Select Member Role In Church");
                var items = CustomManager.GetRolesInChurchs(clientId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                //var jsonitem = items.Select(o => new KeyContent(o.RoleInChurchId, o.Name)).ToList();
                var jsonitem = items.Select(o => new KeyContent(o.RoleInChurchId, "")).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetClientChurchServiceList(long clientChurchId)
        {
            try
            {
                var add = new KeyContent("", "Select Church Service");
                var items = CustomManager.GetClientChurchServiceList(clientChurchId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ChurchServiceTypeRefId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult GetChurchServices(long churchId)
        {
            try
            {
                var add = new KeyContent("", "Select Church Service");
                var items = CustomManager.GetChurchServices(churchId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                //var jsonitem = items.Select(o => new KeyContent(o.ChurchServiceId, o.Name)).ToList();
                var jsonitem = items.Select(o => new KeyContent(o.ChurchServiceTypeId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult GetClientChurchServices(long clientId)
        {
            try
            {
                var add = new KeyContent("", "Select Church Service");
                var items = CustomManager.GetClientChurchServices(clientId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                //var jsonitem = items.Select(o => new KeyContent(o.ChurchServiceId, o.Name)).ToList();
                var jsonitem = items.Select(o => new KeyContent(o.ChurchServiceTypeId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        public ActionResult GetStateOfLocations()
        {
            try
            {
                var add = new KeyContent("", "Select a State");
                var items = CustomManager.GetStateOfLocations();
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.StateOfLocationId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetBanks()
        {
            try
            {
                var add = new KeyContent("", "Select a Bank");
                var items = CustomManager.GetBanks();
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.BankId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetParentChurches()
        {
            try
            {
                var add = new KeyContent(0, "Select a Church");
                var items = CustomManager.GetParentChurches();
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ChurchId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult GetClients()
        {
            try
            {
                var add = new KeyContent(0, "Select a Client");
                var items = CustomManager.GetClients();
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ClientId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult GetClientsByChurchId(long churchId)
        {
            try
            {
                var add = new KeyContent(0, "Select a Client");
                var items = CustomManager.GetClientsByChurchId(churchId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ClientId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult GetClientsByStateChurchId(long churchId, int stateId)
        {
            try
            {
                var add = new KeyContent(0, "Select a Client");
                var items = CustomManager.GetClientsByStateChurchId(churchId, stateId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ClientId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }




        public ActionResult LoadAccountType()
        {
            try
            {
                var accountTypeList = Enum.GetValues(typeof(AccountType)).Cast<AccountType>().Select(x => new NameAndValueObject
                {
                    Name = x.ToString(),
                    Id = ((int)x)
                }).ToList();
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new SelectList(
                              accountTypeList.OrderBy(m => m.Id),
                              "Id",
                              "Name"), JsonRequestBehavior.AllowGet
                              );
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetTitles()
        {
            try
            {
                var accountTypeList = Enum.GetValues(typeof(Title)).Cast<Title>().Select(x => new NameAndValueObject
                {
                    Name = x.ToString(),
                    Id = ((int)x)
                }).ToList();
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new SelectList(
                              accountTypeList.OrderBy(m => m.Id),
                              "Id",
                              "Name"), JsonRequestBehavior.AllowGet
                              );
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public ActionResult GetChurchStructures()
        {
            try
            {
                var churchStructureList = Enum.GetValues(typeof(ChurchPatternType)).Cast<ChurchPatternType>().Select(x => new NameAndValueObject
                {
                    Name = x.ToString(),
                    Id = ((int)x)
                }).ToList();
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new SelectList(
                              churchStructureList.OrderBy(m => m.Id),
                              "Id",
                              "Name"), JsonRequestBehavior.AllowGet
                              );
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public JsonResult GetSex()
        {
            try
            {
                var add = new KeyContent("", "Select Sex");
                var items = SexManager.GetList();
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.Id, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetWeekDays()
        {
            try
            {
                var add = new KeyContent("", "Select Day");
                var items = WeekDaysManager.GetList();
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.Id, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult GetChurchStructureTypes()
        {
            try
            {
                var add = new KeyContent("", "Select Structure Type");
                var items = CustomManager.GetChurchStructureTypes();
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ChurchStructureTypeId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult GetChurchStructureByChurchId(long churchId)
        {
            try
            {
                var add = new KeyContent("", "Select Structure");
                var items = CustomManager.GetChurchStructureByChurchId(churchId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ChurchStructureTypeId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetChurchStructureForHierachyByChurchId(long churchId)
        {
            try
            {
                var add = new KeyContent();
                var items = CustomManager.GetChurchStructureByChurchId(churchId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.ChurchStructureTypeId, o.Name)).ToList();
                jsonitem.Insert(1, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }
        }

        // New Ways to load Church Structure by ChurchId for Church Structure Hierachy

        public ActionResult GetChurchStructuresLookUpForHierachyByChurchId(long churchId)
        {
            try
            {
                var add = new KeyContentStatus();
                var items = ServiceChurch.GetChurchStructureHierachyLookUp(churchId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContentStatus> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContentStatus{ Id = o.ChurchStructureTypeId, Name = o.ChurchStructureTypeName, Status = o.Status }).ToList();
                jsonitem.Insert(1, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }
        }

        


        #region Structures

        #region Used One
        
        public ActionResult GetStructureListObjByChurchStateId(string userAction, string structureType, int churchId, int stateId)
        {

            string actionSession = "";
            switch (userAction)
            {
                case "New":
                    actionSession = "New";
                    break;
                case "Modify":
                    actionSession = "Edit";
                    break;
            }
            try
            {
                var add = new KeyContent("", "Select Church " + structureType);

                #region Region
                if (typeof(Region).Name == structureType)
                {
                    var regionLists = new List<Region>();
                    if (Session[actionSession + "_" + structureType + "_List"] != null)
                    {
                        var regionItems = Session[actionSession +"_"+structureType+"_List"] as List<Region>;
                        if (regionItems == null) return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                        regionLists =
                            regionItems.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId).ToList();
                    }
                    if (!regionLists.Any())
                    {
                        return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                    }
                    var jsonregionitem = regionLists.Select(o => new KeyContent(o.RegionId, o.Name)).ToList();
                    jsonregionitem.Insert(0, add);
                    return Json(jsonregionitem, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region Province
                if (typeof(Province).Name == structureType)
                {
                    var provinceLists = new List<Province>();
                    if (Session[actionSession + "_" + structureType + "_List"] != null)
                    {
                        var provinceItems = Session[actionSession + "_" + structureType + "_List"] as List<Province>;
                        if (provinceItems == null) return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                        provinceLists =
                            provinceItems.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId).ToList();
                    }
                    if (!provinceLists.Any())
                    {
                        return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                    }
                    var jsonprovinceitem = provinceLists.Select(o => new KeyContent(o.ProvinceId, o.Name)).ToList();
                    jsonprovinceitem.Insert(0, add);
                    return Json(jsonprovinceitem, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region Area
                if (typeof(Area).Name == structureType)
                {
                    var areaLists = new List<Area>();
                    if (Session[actionSession + "_" + structureType + "_List"] != null)
                    {
                        var areaItems = Session[actionSession + "_" + structureType + "_List"] as List<Area>;
                        if (areaItems == null) return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                        areaLists =
                            areaItems.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId).ToList();
                    }
                    if (!areaLists.Any())
                    {
                        return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                    }
                    var jsonareaitem = areaLists.Select(o => new KeyContent(o.AreaId, o.Name)).ToList();
                    jsonareaitem.Insert(0, add);
                    return Json(jsonareaitem, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region Zone
                if (typeof(Zone).Name == structureType)
                {
                    var zoneLists = new List<Zone>();
                    if (Session[actionSession + "_" + structureType + "_List"] != null)
                    {
                        var zoneItems = Session[actionSession + "_" + structureType + "_List"] as List<Zone>;
                        if (zoneItems == null) return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                        zoneLists =
                            zoneItems.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId).ToList();
                    }
                    if (!zoneLists.Any())
                    {
                        return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                    }
                    var jsonzoneitem = zoneLists.Select(o => new KeyContent(o.ZoneId, o.Name)).ToList();
                    jsonzoneitem.Insert(0, add);
                    return Json(jsonzoneitem, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region Diocese
                if (typeof(Diocese).Name == structureType)
                {
                    var dioceseLists = new List<Diocese>();
                    if (Session[actionSession + "_" + structureType + "_List"] != null)
                    {
                        var dioceseItems = Session[actionSession + "_" + structureType + "_List"] as List<Diocese>;
                        if (dioceseItems == null) return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                        dioceseLists =
                            dioceseItems.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId).ToList();
                    }
                    if (!dioceseLists.Any())
                    {
                        return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                    }
                    var jsondioceseitem = dioceseLists.Select(o => new KeyContent(o.DioceseId, o.Name)).ToList();
                    jsondioceseitem.Insert(0, add);
                    return Json(jsondioceseitem, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region District
                if (typeof(District).Name == structureType)
                {
                    var districtLists = new List<District>();
                    if (Session[actionSession + "_" + structureType + "_List"] != null)
                    {
                        var districtItems = Session[actionSession + "_" + structureType + "_List"] as List<District>;
                        if (districtItems == null) return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                        districtLists =
                            districtItems.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId).ToList();
                    }
                    if (!districtLists.Any())
                    {
                        return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                    }
                    var jsondistrictitem = districtLists.Select(o => new KeyContent(o.DistrictId, o.Name)).ToList();
                    jsondistrictitem.Insert(0, add);
                    return Json(jsondistrictitem, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region State
                if (typeof(State).Name == structureType)
                {
                    var stateLists = new List<State>();
                    if (Session[actionSession + "_" + structureType + "_List"] != null)
                    {
                        var stateItems = Session[actionSession + "_" + structureType + "_List"] as List<State>;
                        if (stateItems == null) return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                        stateLists =
                            stateItems.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId).ToList();
                    }
                    if (!stateLists.Any())
                    {
                        return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                    }
                    var jsonstateitem = stateLists.Select(o => new KeyContent(o.StateId, o.Name)).ToList();
                    jsonstateitem.Insert(0, add);
                    return Json(jsonstateitem, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region Group
                if (typeof(Group).Name == structureType)
                {
                    var groupLists = new List<Group>();
                    if (Session[actionSession + "_" + structureType + "_List"] != null)
                    {
                        var groupItems = Session[actionSession + "_" + structureType + "_List"] as List<Group>;
                        if (groupItems == null) return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                        groupLists =
                            groupItems.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId).ToList();
                    }
                    if (!groupLists.Any())
                    {
                        return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                    }
                    var jsongroupitem = groupLists.Select(o => new KeyContent(o.GroupId, o.Name)).ToList();
                    jsongroupitem.Insert(0, add);
                    return Json(jsongroupitem, JsonRequestBehavior.AllowGet);
                }
                #endregion
                
                return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetStructureChurchListObjByChurchStateId(long churchId, int stateId)
        {
            try
            {
                var add = new KeyContent("", "Select Structure");
                var items = CustomManager.GetStructureChurchListObjByChurchStateId(churchId, stateId);
                if (items == null || !items.Any())
                {
                    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items.Select(o => new KeyContent(o.StructureChurchId, o.Name)).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetChurchStructureTypeByStructureChurchId(long structureChurchId)
        {
            try
            {
                //var add = new KeyContent();
                //var items = ServiceChurch.GetChurchStructureTypeByStructureChurchId(structureChurchId);
                //if (items == null || !items.Any())
                //{
                //    return Json(new List<KeyContent> { add }, JsonRequestBehavior.AllowGet);
                //}
                //var jsonitem = items.Select(o => new KeyContent(o.ChurchStructureTypeId, o.Name)).ToList();
                //jsonitem.Insert(0, add);
                //return Json(jsonitem, JsonRequestBehavior.AllowGet);

                var add = new KeyContent();
                var items = ServiceChurch.GetChurchStructureTypeByStructureChurchId(structureChurchId);
                if (items == null || !items.Any())
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                var jsonitem = items[0].Name;
                //jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<KeyContent>(), JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetStructureTypeByStructureChurchId(long structureChurchId)
        {
            try
            {
                if (structureChurchId < 1)
                {
                    return Json(new { success = false, error = "Invalid selection", string.Empty }, JsonRequestBehavior.AllowGet);
                }

                var item = ServiceChurch.GetChurchStructureTypeByStructureChurchId(structureChurchId);
                if (item == null || !item.Any())
                {
                    return Json( new { success = false, error = "No record found", string.Empty}, JsonRequestBehavior.AllowGet);
                }

                var jsonitem = item[0].Name;
                return Json(new { success = true, error = "", typeName = jsonitem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { success = false, error = ex.Message, string.Empty },
                                JsonRequestBehavior.AllowGet);
            }
        }
        

        #endregion

        
        

        #endregion

        
        
	}
}