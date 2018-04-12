using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchServiceAttendanceRemittanceRepository
    {
        private readonly IIcasRepository<ChurchServiceAttendanceRemittance> _repository;
        private readonly IIcasRepository<ChurchServiceType> _churchServiceTypeRepository;
        private readonly IIcasRepository<ChurchServiceAttendance> _churchServiceAttendanceRepository;
        private readonly IcasUoWork _uoWork;


        public ChurchServiceAttendanceRemittanceRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchServiceAttendanceRemittance>(_uoWork);
            _churchServiceTypeRepository = new IcasRepository<ChurchServiceType>(_uoWork);
            _churchServiceAttendanceRepository = new IcasRepository<ChurchServiceAttendance>(_uoWork);
        }


        // This is the Main Remittance Calling
        internal ChurchServiceAttendanceRemittanceReportObj GetClientChurchServiceAttendanceRemittance(long churchId, long clientId, string start, string end)
        {

            try
            {
                
                var myItem = ComputeClientRemittanceByDateRange(churchId, clientId, start, end);
                if (myItem == null) { return new ChurchServiceAttendanceRemittanceReportObj(); }



                #region Extract & Collates Monetary Collection Types Total - National Hqtr
                
                //var checks = 
                //        thisMonthChurchServiceAttendances.Select(
                //            thisMonthChurchServiceAttendance =>
                //                thisMonthChurchServiceAttendance.ServiceAttendanceDetail
                //                    .ChurchServiceAttendanceCollections)
                //            .Select(
                //                collectionTypes =>
                //                    collectionTypes.Find(x => x.CollectionTypeId == thisCollectionTypeId)).ToList();


                //var monetaryTotals =
                //    thisMonthCollectionRemittanceDetails.Select(
                //        x => x.CollectionRemittanceChurchStructureType.Select(s => s.ChurchStructureTypeId == 1))
                //        .ToList();


                //var monetaryTotals =
                //    myItem.CollectionRemittanceDetail.Select(
                //        x => x.CollectionRemittanceChurchStructureType.FindAll(s => s.ChurchStructureTypeId == 1).ToList())
                //        .ToList();

                #endregion



                #region Extract Remittance Parameters
                
                var monthlyAmountCaptures = myItem.CollectionRemittanceDetail.Select(x => x.TotalMonthlyCaptured).Sum();
                var monthlyAmountRemitts = myItem.CollectionRemittanceDetail.Select(x => x.TotalPercentRemitted).Sum();
                var monthlyLeftBalances = myItem.CollectionRemittanceDetail.Select(x => x.TotalBalanceLeft).Sum();
                var monthlyTotalAttendee =
                    myItem.RemittanceChurchServiceDetail.Select(
                        x => x.RemittanceChurchServiceAttendeeDetail.TotalAttendee).Sum();
                
                var from = start;
                var to = end;
                if (!from.IsNullOrEmpty() && !to.IsNullOrEmpty())
                {

                    var dt = DateTime.ParseExact(from, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    from = dt.ToString("dd MMMM");

                    dt = DateTime.ParseExact(to, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    to = dt.ToString("dd MMMM, yyyy");
                }
                
                #region Calculate Total Collection Remittance Under Each Structure

                #endregion
                

                #endregion
                var retItem = new ChurchServiceAttendanceRemittanceReportObj
                {
                    ChurchServiceAttendanceRemittanceId = myItem.ChurchServiceAttendanceRemittanceId,
                    DateRange = from + " - " + to,
                    TotalMonthlyAttendee = monthlyTotalAttendee,
                    TotalMonthlyAmountCaptured = monthlyAmountCaptures,
                    TotalMonthlyAmountRemitted = monthlyAmountRemitts,
                    TotalMonthlyBalanceLeft = monthlyLeftBalances,

                    RemittanceChurchServiceMonthlyTotalAttendee = myItem.RemittanceChurchServiceMonthlyTotalAttendee,
                    RemittanceChurchServiceAverageAttendanceDetail = myItem.RemittanceChurchServiceAverageAttendanceDetail,
                    CollectionTypeMonetaryTotals = myItem.CollectionTypeMonetaryTotalObjs,
                    ChurchStructureTypeCollectionTotals = myItem.CollectionRemittanceChurchStructureTypeTotal,

                    RemittanceDetailReport = new RemittanceDetailReportObj
                    {
                        ChurchServiceAttendanceRemittanceCollections = myItem.ChurchServiceAttendanceRemittanceCollection,
                        CollectionRemittanceDetails = myItem.CollectionRemittanceDetail,
                        RemittanceChurchServiceDetails = myItem.RemittanceChurchServiceDetail
                    }
                };

                return retItem;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ChurchServiceAttendanceRemittanceReportObj();
            }

        }


        // This is Main Remittance Date Range Computation
        internal ComputedChurchServiceAttendanceRemittanceObj ComputeClientRemittanceByDateRange(long churchId, long clientId, string start, string end)
        {
            try
            {

                string msg;
                #region Church Service Attendances - DateRange(From - To) Step 1

                var thisMonthChurchServiceAttendances =
                    new ChurchServiceAttendanceRepository().GetMonthlyChurchServiceAttendanceByDateRange(clientId, start, end,
                        out msg);
                if (!thisMonthChurchServiceAttendances.Any()) { return new ComputedChurchServiceAttendanceRemittanceObj(); }

                #endregion

                #region Client Church Collection Types - By(ClientId) Step 2

                //var clientChurchCollectionType =
                //    new ChurchCollectionTypeRepository().GetChurchCollectionType(churchId);
                //var thisClientChurchCollectionTypes = clientChurchCollectionType.CollectionTypes;

                var clientChurchCollectionType =
                    new ClientChurchCollectionTypeRepository().GetClientChurchCollectionTypeByClientChurchId(clientId);
                var thisClientChurchCollectionTypes = clientChurchCollectionType.CollectionTypes;

                #endregion

                #region Related Objects For Composing Remittance Object Step 3

                var churchStructureTypes = new ChurchStructureTypeRepository().GetChurchStructureTypes();
                var churchCollectionTypes = new CollectionTypeRepository().GetCollectionTypes();

                //var thisClientChurchStruture = new ChurchStructureRepository().GetChurchStructureType(churchId);
                //var thisClientChurchStrutureTypes = new List<ChurchStructureType>();
                //var thisClientChurchStrutureTypes = thisClientChurchStruture.ChurchStructureTypeDetail;
                
                var clientChurchServiceTypes = new ChurchServiceRepository().GetChurchService(churchId);

                #endregion



                #region Instantiating Remittance Complex Properties Step 4


                #region Client Church

                var thisMonthClientChurchServiceAttendanceRemittanceCollections =
                    new List<ClientChurchServiceAttendanceRemittanceCollectionObj>();

                var thisMonthCollectionRemittanceDetails =
                   new List<ClientCollectionRemittanceDetailObj>();

                #endregion


                var collectionTypeMonetaryTotals = new List<CollectionTypeMonetaryTotalObj>();

                var thisMonthChurchServiceAttendanceRemittanceCollections =
                    new List<ChurchServiceAttendanceRemittanceCollectionObj>();
                //var thisMonthCollectionRemittanceDetails =
                //    new List<CollectionRemittanceDetailObj>();
                var churchServiceAttendanceDetails = new List<RemittanceChurchServiceDetailObj>();

                var churchServiceAverageAttendances = new List<ChurchServiceAverageAttendanceObj>();

                var remittanceChurchServiceMonthlyTotalAttendee = new RemittanceChurchServiceMonthlyTotalAttendeeObj();

                #endregion

                #region Composing Remittance ChurchService Attendance Details - Step 5
                

                #region Compute Church Service Average Attendee

                // Step 1 : Get this Client List of Church Services
                // Step 2 : Loop through the list of Client Church Services
                // Step 3 : Inside the loop, get all the current Service Types in the list of all (thisMonthChurchServiceAttendances)
                // Step 4 : Sum the total of Men, Women and Children from the list of the current Service Type for that Month
                // Step 5 : Count the number of the current service by its .Count()
                // Step 6 : Calc. Average Attendance per Men, Women, and Children by dividing (TotalMen/number of current service)
                // Step 7: Total the Average of Men, Women, and Children - TotalAverageAttendee 
                    //(i.e. Add up the average Attendee we get above for men, women & children together)

                foreach (var clientChurchSeviceType in clientChurchServiceTypes.ServiceTypeDetail)
                {
                    //var thisServiceTypeId = clientChurchSeviceType.ChurchServiceTypeId;
                    //var thisServiceTypeAllServices =
                    //    thisMonthChurchServiceAttendances.FindAll(x => x.ChurchServiceTypeId == thisServiceTypeId);
                    var thisServiceTypeRefId = clientChurchSeviceType.ChurchServiceTypeRefId;
                    var thisServiceTypeAllServices =
                        thisMonthChurchServiceAttendances.FindAll(x => x.ChurchServiceTypeRefId == thisServiceTypeRefId);

                    var totalMen = 0;
                    var totalWomen = 0;
                    var totalChildren = 0;



                    if (thisServiceTypeAllServices.Any() && thisServiceTypeAllServices.Count > 0)
                    {
                        foreach (var thisServiceTypeAllService in thisServiceTypeAllServices)
                        {
                            totalMen += thisServiceTypeAllService.ServiceAttendanceDetail.NumberOfMen;
                            totalWomen += thisServiceTypeAllService.ServiceAttendanceDetail.NumberOfWomen;
                            totalChildren += thisServiceTypeAllService.ServiceAttendanceDetail.NumberOfChildren;
                        }

                        var numberOfThisService = thisServiceTypeAllServices.Count;
                        if (numberOfThisService > 0)
                        {
                            var averageAttendeeMen = (totalMen / numberOfThisService);
                            var averageAttendeeWomen = (totalWomen / numberOfThisService);
                            var averageAttendeeChildren = (totalChildren / numberOfThisService);

                            #region RemittanceChurchServiceMonthlyTotalAttendee (Men, Women & Children

                            remittanceChurchServiceMonthlyTotalAttendee.TotalMen += totalMen;
                            remittanceChurchServiceMonthlyTotalAttendee.TotalWomen += totalWomen;
                            remittanceChurchServiceMonthlyTotalAttendee.TotalChildren += totalChildren;

                            #endregion


                            var thisChurchServiceAverageAttendance = new ChurchServiceAverageAttendanceObj
                            {
                                ChurchServiceTypeId = clientChurchSeviceType.ChurchServiceTypeId,
                                ServiceName = _churchServiceTypeRepository.GetById(clientChurchSeviceType.ChurchServiceTypeId).Name,
                                AverageAttendeeMen = averageAttendeeMen,
                                AverageAttendeeWomen = averageAttendeeWomen,
                                AverageAttendeeChildren = averageAttendeeChildren,
                                TotalAverageAttendee = (averageAttendeeMen + averageAttendeeWomen + averageAttendeeChildren)
                            };

                            churchServiceAverageAttendances.Add(thisChurchServiceAverageAttendance);
                        }
                    }
                }

                #endregion


                thisMonthChurchServiceAttendances.ForEachx(m =>
                {
                    //var serviceName = _churchServiceTypeRepository.GetById(m.ChurchServiceTypeId).Name;
                    var serviceName = _churchServiceTypeRepository.GetById(m.ChurchServiceTypeRefId).Name;

                    #region Latest Getting Service Name

                    // Get it from Client Church Service with m.ClientChurchId & m.ChurchServiceTypeRefId
                    var clientChurchServiceName =
                        ServiceChurch.GetClientChurchServiceDetail(m.ClientChurchId, m.ChurchServiceTypeRefId).Name;

                    #endregion

                    churchServiceAttendanceDetails.Add(new RemittanceChurchServiceDetailObj
                    {
                        ChurchServiceAttendanceId = m.ChurchServiceAttendanceId,
                        //ChurchServiceTypeId = m.ChurchServiceTypeId,
                        ChurchServiceTypeRefId = m.ChurchServiceTypeRefId,
                        ChurchServiceTypeName = clientChurchServiceName,
                        DateServiceHeld = m.DateServiceHeld,
                        Preacher = m.Preacher,
                        RemittanceChurchServiceAttendeeDetail = new RemittanceChurchServiceAttendeeDetailObj
                        {
                            NumberOfMen = m.ServiceAttendanceDetail.NumberOfMen,
                            NumberOfWomen = m.ServiceAttendanceDetail.NumberOfWomen,
                            NumberOfChildren = m.ServiceAttendanceDetail.NumberOfChildren,
                            FirstTimer = m.ServiceAttendanceDetail.FirstTimer,
                            NewConvert = m.ServiceAttendanceDetail.NewConvert,
                            TotalAttendee = m.TotalAttendee,
                        }
                    });
                });
                #endregion

                var structureAmountCollectionRemitteds = new List<CollectionRemittanceChurchStructureTypeTotalObj>();
                

                #region Composing This Client Church Strucuture Types For Computing Remittance

                
                var thisChurchStructureTypesForRemittances = new List<ChurchStructureType>();
                foreach (var churchStructureType in churchStructureTypes)
                {
                    foreach (var thisClientChurchCollectionType in thisClientChurchCollectionTypes)
                    {
                        var check =
                           thisClientChurchCollectionType.ChurchStructureTypeObjs.Find(
                               x => x.ChurchStructureTypeId == churchStructureType.ChurchStructureTypeId);

                        if (check != null)
                        {
                            if (check.Percent > 0)
                            {
                                thisChurchStructureTypesForRemittances.Add(churchStructureType);
                                break;
                            }

                        }
                    }
                }

                #endregion




                #region Computing Remittance - Loop Through Client Church Colletion Types

                double thisCollectionTotalAmount = 0;
                foreach (var thisClientChurchCollectionType in thisClientChurchCollectionTypes)
                {
                    //var thisCollectionTypeId = thisClientChurchCollectionType.CollectionTypeId;
                    var thisCollectionRefId = thisClientChurchCollectionType.CollectionRefId;

                    #region Latest Get All the Attendance Details Belong to the Current CollectionType

                    var checks =
                        thisMonthChurchServiceAttendances.Select(
                            thisMonthChurchServiceAttendance =>
                                thisMonthChurchServiceAttendance.ServiceAttendanceDetail
                                    .ClientChurchServiceAttendanceCollections)
                            .Select(
                                collectionTypes =>
                                    collectionTypes.Find(x => x.CollectionRefId == thisCollectionRefId)).ToList();

                    foreach (var check in checks)
                    {
                        if (check != null && check.Amount > 0)
                        {
                            thisCollectionTotalAmount += check.Amount;
                        }
                    }

                    #endregion

                    #region Latest Compose the Object going to Remittance Table

                    //thisClientChurchCollectionTypes

                    var collectionTypeName =
                        (thisClientChurchCollectionTypes.Find(x => x.CollectionRefId == thisCollectionRefId).Name ==
                         string.Empty
                            ? "Collection Not Exist"
                            : thisClientChurchCollectionTypes.Find(x => x.CollectionRefId == thisCollectionRefId).Name);
                    
                    // Here Compose the Object going to Remittance Table  ChurchServiceAttendanceRemittanceCollectionObj
                    var thisCollectionTypeRemittanceObj = new ClientChurchServiceAttendanceRemittanceCollectionObj
                    {
                        CollectionRefId = thisCollectionRefId,
                        CollectionTypeName = collectionTypeName,
                        TotalRemittance = thisCollectionTotalAmount
                    };
                    thisMonthClientChurchServiceAttendanceRemittanceCollections.Add(thisCollectionTypeRemittanceObj);

                    #endregion

                    #region Old Get All the Attendance Details Belong to the Current CollectionType

                    //var checks = 
                    //    thisMonthChurchServiceAttendances.Select(
                    //        thisMonthChurchServiceAttendance =>
                    //            thisMonthChurchServiceAttendance.ServiceAttendanceDetail
                    //                .ChurchServiceAttendanceCollections)
                    //        .Select(
                    //            collectionTypes =>
                    //                collectionTypes.Find(x => x.CollectionTypeId == thisCollectionTypeId)).ToList();

                    //foreach (var check in checks)
                    //{
                    //    if (check != null && check.Amount > 0)
                    //    {
                    //        thisCollectionTotalAmount += check.Amount;
                    //    }
                    //}

                    //var collectionTypeName = churchCollectionTypes.Find(x => x.CollectionTypeId == thisCollectionTypeId).Name;


                    //var collectionTypeName =
                    //    (churchCollectionTypes.Find(x => x.CollectionTypeId == thisCollectionTypeId).Name ==
                    //     string.Empty
                    //        ? "Collection Not Exist"
                    //        : churchCollectionTypes.Find(x => x.CollectionTypeId == thisCollectionTypeId).Name);



                    //// Here Compose the Object going to Remittance Table
                    //var thisCollectionTypeRemittanceObj = new ChurchServiceAttendanceRemittanceCollectionObj
                    //{
                    //    CollectionTypeId = thisCollectionTypeId,
                    //    CollectionTypeName = collectionTypeName,
                    //    TotalRemittance = thisCollectionTotalAmount
                    //};
                    //thisMonthChurchServiceAttendanceRemittanceCollections.Add(thisCollectionTypeRemittanceObj);

                    #endregion

                    // Loop Through this Client ChurchCollectionTypes with ChurchStructureType 
                    // And Calculate the Percentage of each ChurchStructureType (i.e. National, Region etc. 
                    // On this current CollectionType (i.e. Offerring)
                    var structurePercentages = new List<CollectionRemittanceChurchStructureTypeObj>();
                    double thisCollectionTotalPercentRemit = 0;
                    foreach (var churchStructureCollectionPercent in thisClientChurchCollectionType.ChurchStructureTypeObjs)
                    {
                        //var structureName =
                        //    churchStructureTypes.Find(
                        //        x => x.ChurchStructureTypeId == churchStructureCollectionPercent.ChurchStructureTypeId)
                        //        .Name;

                        #region Latestsss

                        var checkStructureForRemittanace =
                            thisChurchStructureTypesForRemittances.Find(
                                x => x.ChurchStructureTypeId == churchStructureCollectionPercent.ChurchStructureTypeId);

                        if (checkStructureForRemittanace != null)
                        {
                            double percentRemit = 0;
                            if (churchStructureCollectionPercent.Percent > 0)
                            {
                                percentRemit = (thisCollectionTotalAmount * churchStructureCollectionPercent.Percent);
                            }

                            thisCollectionTotalPercentRemit += percentRemit;

                            #region Calculate Total Collection Remittance Under Each Structure

                            var checkCurrentStructure =
                                structureAmountCollectionRemitteds.Find(
                                    x =>
                                        x.ChurchStructureTypeId ==
                                        churchStructureCollectionPercent.ChurchStructureTypeId);

                            if (checkCurrentStructure != null)
                            {
                                if (checkCurrentStructure.ChurchStructureTypeId > 0)
                                {
                                    var collectionRemittanceChurchStructureTypeTotalObj =
                                    structureAmountCollectionRemitteds.FirstOrDefault(x => x.ChurchStructureTypeId ==
                                                                                           churchStructureCollectionPercent
                                                                                               .ChurchStructureTypeId);
                                    if (collectionRemittanceChurchStructureTypeTotalObj != null)
                                        collectionRemittanceChurchStructureTypeTotalObj
                                            .TotalCollectionAmountRemitted += percentRemit;
                                }

                            }
                            else
                            {
                                //CollectionRemittanceChurchStructureTypeTotal
                                var structureAmountRemitted = new CollectionRemittanceChurchStructureTypeTotalObj
                                {
                                    ChurchStructureTypeId = churchStructureCollectionPercent.ChurchStructureTypeId,
                                    TotalCollectionAmountRemitted = percentRemit
                                };
                                structureAmountCollectionRemitteds.Add(structureAmountRemitted);
                            }

                            #endregion

                            structurePercentages.Add(new CollectionRemittanceChurchStructureTypeObj
                            {
                                ChurchStructureTypeId = churchStructureCollectionPercent.ChurchStructureTypeId,
                                ChurchStructureTypeName = churchStructureCollectionPercent.Name,
                                //ChurchStructureTypeName = structureName,
                                Percent = churchStructureCollectionPercent.Percent,
                                AmountRemitted = percentRemit
                            });



                            #region Composing this Collection Type Remittance Church Structure (National Hqtr) For Monetary Totals
                            
                            if (churchStructureCollectionPercent.ChurchStructureTypeId == 1)
                            {
                                collectionTypeMonetaryTotals.Add(new CollectionTypeMonetaryTotalObj
                                {
                                    CollectionRefId = thisCollectionRefId,
                                    CollectionTypeName = collectionTypeName,
                                    TotalRemittance = thisCollectionTotalAmount,
                                    MonetaryTotalChurchStructureTypes = new CollectionRemittanceChurchStructureTypeObj
                                    {
                                        ChurchStructureTypeId = churchStructureCollectionPercent.ChurchStructureTypeId,
                                        ChurchStructureTypeName = churchStructureCollectionPercent.Name,
                                        Percent = churchStructureCollectionPercent.Percent,
                                        AmountRemitted = percentRemit,
                                        AmountRemittedCurrency = string.Format("{0:N}", percentRemit)
                                    }
                                });
                            }

                            #endregion


                        }
                        #endregion


                        #region Oldiessxx
                        //if (churchStructureCollectionPercent.Percent > 0)
                        //{
                        //    var percentRemit = (thisCollectionTotalAmount * churchStructureCollectionPercent.Percent);
                        //    thisCollectionTotalPercentRemit += percentRemit;

                        //    #region Calculate Total Collection Remittance Under Each Structure

                        //    var checkCurrentStructure =
                        //        structureAmountCollectionRemitteds.Find(
                        //            x =>
                        //                x.ChurchStructureTypeId ==
                        //                churchStructureCollectionPercent.ChurchStructureTypeId);

                        //    if (checkCurrentStructure != null)
                        //    {
                        //        if (checkCurrentStructure.ChurchStructureTypeId > 0)
                        //        {
                        //            var collectionRemittanceChurchStructureTypeTotalObj =
                        //            structureAmountCollectionRemitteds.FirstOrDefault(x => x.ChurchStructureTypeId ==
                        //                                                                   churchStructureCollectionPercent
                        //                                                                       .ChurchStructureTypeId);
                        //            if (collectionRemittanceChurchStructureTypeTotalObj != null)
                        //                collectionRemittanceChurchStructureTypeTotalObj
                        //                    .TotalCollectionAmountRemitted += percentRemit;
                        //        }


                        //        //structureAmountCollectionRemitteds.Where(
                        //        //    x =>
                        //        //        x.ChurchStructureTypeId ==
                        //        //        churchStructureCollectionPercent.ChurchStructureTypeId)
                        //        //    .FirstOrDefault()
                        //        //    .TotalCollectionAmountRemitted += percentRemit;
                        //    }
                        //    else
                        //    {
                        //        var structureAmountRemitted = new CollectionRemittanceChurchStructureTypeTotalObj
                        //        {
                        //            ChurchStructureTypeId = churchStructureCollectionPercent.ChurchStructureTypeId,
                        //            TotalCollectionAmountRemitted = percentRemit
                        //        };
                        //        structureAmountCollectionRemitteds.Add(structureAmountRemitted);
                        //    }

                        //    #endregion


                        //    structurePercentages.Add(new CollectionRemittanceChurchStructureTypeObj
                        //    {
                        //        ChurchStructureTypeId = churchStructureCollectionPercent.ChurchStructureTypeId,
                        //        ChurchStructureTypeName = structureName,
                        //        Percent = churchStructureCollectionPercent.Percent,
                        //        AmountRemitted = percentRemit
                        //    });
                        //}
                        #endregion

                    }

                    #region Oldiess

                    var thisCollectionTypeRemittanceDetailObj = new ClientCollectionRemittanceDetailObj
                    {
                        CollectionRefId = thisCollectionRefId,
                        CollectionTypeName = collectionTypeName,
                        TotalMonthlyCaptured = thisCollectionTotalAmount,
                        TotalPercentRemitted = thisCollectionTotalPercentRemit,
                        TotalBalanceLeft = (thisCollectionTotalAmount - thisCollectionTotalPercentRemit),
                        CollectionRemittanceChurchStructureType = structurePercentages
                    };
                    thisMonthCollectionRemittanceDetails.Add(thisCollectionTypeRemittanceDetailObj);
                    thisCollectionTotalAmount = 0;

                    #endregion

                    //var thisCollectionTypeRemittanceDetailObj = new CollectionRemittanceDetailObj
                    //{
                    //    CollectionTypeId = thisCollectionTypeId,
                    //    CollectionTypeName = collectionTypeName,
                    //    TotalMonthlyCaptured = thisCollectionTotalAmount,
                    //    TotalPercentRemitted = thisCollectionTotalPercentRemit,
                    //    TotalBalanceLeft = (thisCollectionTotalAmount - thisCollectionTotalPercentRemit),
                    //    CollectionRemittanceChurchStructureType = structurePercentages
                    //};
                    //thisMonthCollectionRemittanceDetails.Add(thisCollectionTypeRemittanceDetailObj);
                    //thisCollectionTotalAmount = 0;
                }

                #endregion

                #region Submitting Remittance

                var churchServiceAttendanceRemittanceObj = new ComputedChurchServiceAttendanceRemittanceObj
                {
                    ClientId = clientId,
                    From = start,
                    To = end,
                    RemittanceChurchServiceDetail = churchServiceAttendanceDetails,
                    RemittanceChurchServiceMonthlyTotalAttendee = remittanceChurchServiceMonthlyTotalAttendee,
                    RemittanceChurchServiceAverageAttendanceDetail = churchServiceAverageAttendances,
                    CollectionTypeMonetaryTotalObjs = collectionTypeMonetaryTotals,
                    ChurchServiceAttendanceRemittanceCollection = thisMonthChurchServiceAttendanceRemittanceCollections,
                    CollectionRemittanceDetail = thisMonthCollectionRemittanceDetails,
                    CollectionRemittanceChurchStructureTypeTotal = structureAmountCollectionRemitteds,
                    TimeStampRemitted = DateScrutnizer.CurrentTimeStamp(),
                    RemittedByUserId = 1
                };
                
                //var churchServiceAttendanceRemittanceObj = new ChurchServiceAttendanceRemittance
                //{
                //    ClientId = clientId,
                //    From = start,
                //    To = end,
                //    RemittanceChurchServiceDetail = churchServiceAttendanceDetails,
                //    RemittanceChurchServiceAverageAttendanceDetail = churchServiceAverageAttendances,
                //    ChurchServiceAttendanceRemittanceCollection = thisMonthChurchServiceAttendanceRemittanceCollections,
                //    CollectionRemittanceDetail = thisMonthCollectionRemittanceDetails,
                //    CollectionRemittanceChurchStructureTypeTotal = structureAmountCollectionRemitteds,
                //    TimeStampRemitted = DateScrutnizer.CurrentTimeStamp(),
                //    RemittedByUserId = 1
                //};

                //var processedChurchServiceAttendanceRemittance = _repository.Add(churchServiceAttendanceRemittanceObj);
                //_uoWork.SaveChanges();
                //if (processedChurchServiceAttendanceRemittance.ChurchServiceAttendanceRemittanceId < 1)
                //{
                //    return new ChurchServiceAttendanceRemittance();
                //}
                #endregion


                return churchServiceAttendanceRemittanceObj;


            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }























        #region Unsed 

        //internal ChurchServiceAttendanceRemittance GetAndComputeChurchServiceAttendanceRemittance(long churchId,
        //    long clientId, string start, string end)
        //{

        //    try
        //    {
        //        if (churchId < 1 || clientId < 1 || start.IsNullOrEmpty() || end.IsNullOrEmpty())
        //        {
        //            return new ChurchServiceAttendanceRemittance();
        //        }

        //        string msg;

        //        // Check If the range of this ChurchServiceAttendance has been remitted
        //        var check = IsAttendanceRemittanceExist(clientId, start, end,
        //                out msg);
        //        if (check != null && check.ChurchServiceAttendanceRemittanceId > 0)
        //        {
        //            return check;
        //        }

        //        var freshChurchServiceAttendance = ComputeRemittanceByDateRange(churchId, clientId, start, end);
        //        if (freshChurchServiceAttendance == null ||
        //            freshChurchServiceAttendance.ChurchServiceAttendanceRemittanceId < 1)
        //        {
        //            return new ChurchServiceAttendanceRemittance();
        //        }

        //        return freshChurchServiceAttendance;
        //    }
        //    catch (Exception ex)
        //    {
        //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        return new ChurchServiceAttendanceRemittance(); ;
        //    }

        //}


        //internal ChurchServiceAttendanceRemittance ComputeRemittanceByDateRange(long churchId, long clientId, string start, string end)
        //{
        //    try
        //    {

        //        string msg;
        //        #region Church Service Attendances - DateRange(From - To) Step 1

        //        var thisMonthChurchServiceAttendances =
        //            new ChurchServiceAttendanceRepository().GetMonthlyChurchServiceAttendanceByDateRange(clientId, start, end,
        //                out msg);
        //        if (!thisMonthChurchServiceAttendances.Any()) { return new ChurchServiceAttendanceRemittance(); }

        //        #endregion

        //        #region Client Church Collection Types - By(ClientId) Step 2

        //        var clientChurchCollectionType =
        //            new ChurchCollectionTypeRepository().GetChurchCollectionType(churchId);
        //        var thisClientChurchCollectionTypes = clientChurchCollectionType.CollectionTypes;

        //        #endregion

        //        #region Related Objects For Composing Remittance Object Step 3

        //        var churchStructureTypes = new ChurchStructureTypeRepository().GetChurchStructureTypes();
        //        var churchCollectionTypes = new CollectionTypeRepository().GetCollectionTypes();

        //        #endregion

        //        #region Instantiating Remittance Complex Properties Step 4

        //        var thisMonthChurchServiceAttendanceRemittanceCollections =
        //            new List<ChurchServiceAttendanceRemittanceCollectionObj>();
        //        var thisMonthCollectionRemittanceDetails =
        //            new List<ClientCollectionRemittanceDetailObj>();
        //        var churchServiceAttendanceDetails = new List<RemittanceChurchServiceDetailObj>();

        //        #endregion

        //        #region Composing Remittance ChurchService Attendance Details - Step 5
        //        thisMonthChurchServiceAttendances.ForEachx(m =>
        //        {

        //            //var serviceName = _churchServiceTypeRepository.GetById(m.ChurchServiceTypeId).Name;
        //            var serviceName = _churchServiceTypeRepository.GetById(m.ChurchServiceTypeRefId).Name;
        //            churchServiceAttendanceDetails.Add(new RemittanceChurchServiceDetailObj
        //            {
        //                ChurchServiceAttendanceId = m.ChurchServiceAttendanceId,
        //                //ChurchServiceTypeId = m.ChurchServiceTypeId,
        //                ChurchServiceTypeRefId = m.ChurchServiceTypeRefId,
        //                ChurchServiceTypeName = serviceName,
        //                DateServiceHeld = m.DateServiceHeld,
        //                RemittanceChurchServiceAttendeeDetail = new RemittanceChurchServiceAttendeeDetailObj
        //                {
        //                    NumberOfMen = m.ServiceAttendanceDetail.NumberOfMen,
        //                    NumberOfWomen = m.ServiceAttendanceDetail.NumberOfWomen,
        //                    NumberOfChildren = m.ServiceAttendanceDetail.NumberOfChildren,
        //                    FirstTimer = m.ServiceAttendanceDetail.FirstTimer,
        //                    NewConvert = m.ServiceAttendanceDetail.NewConvert,
        //                    TotalAttendee = m.TotalAttendee
        //                }
        //            });
        //        });
        //        #endregion


        //        #region Computing Remittance - Loop Through Client Church Colletion Types

        //        double thisCollectionTotalAmount = 0;
        //        foreach (var thisClientChurchCollectionType in thisClientChurchCollectionTypes)
        //        {
        //            var thisCollectionRefId = thisClientChurchCollectionType.CollectionRefId;

        //            thisCollectionTotalAmount +=
        //                thisMonthChurchServiceAttendances.Select(
        //                    thisMonthChurchServiceAttendance =>
        //                        thisMonthChurchServiceAttendance.ServiceAttendanceDetail
        //                            .ClientChurchServiceAttendanceCollections)
        //                    .Select(
        //                        collectionTypes =>
        //                            collectionTypes.Find(x => x.CollectionRefId == thisCollectionRefId).Amount)
        //                    .Sum();


        //            //var collectionTypeName = churchCollectionTypes.Find(x => x.CollectionTypeId == thisCollectionTypeId).Name;

        //            // Here Compose the Object going to Remittance Table
        //            var thisCollectionTypeRemittanceObj = new ChurchServiceAttendanceRemittanceCollectionObj
        //            {
        //                //CollectionTypeId = thisCollectionTypeId,
        //                //CollectionTypeName = collectionTypeName,
        //                TotalRemittance = thisCollectionTotalAmount
        //            };
        //            thisMonthChurchServiceAttendanceRemittanceCollections.Add(thisCollectionTypeRemittanceObj);

        //            // Loop Through this Client ChurchCollectionTypes with ChurchStructureType 
        //            // And Calculate the Percentage of each ChurchStructureType (i.e. National, Region etc. 
        //            // On this current CollectionType (i.e. Offerring)
        //            var structurePercentages = new List<CollectionRemittanceChurchStructureTypeObj>();
        //            double thisCollectionTotalPercentRemit = 0;
        //            //foreach (var churchStructureCollectionPercent in thisClientChurchCollectionType.ChurchStructureTypeObjs)
        //            //{

        //            //    var structureName =
        //            //        churchStructureTypes.Find(
        //            //            x => x.ChurchStructureTypeId == churchStructureCollectionPercent.ChurchStructureTypeId)
        //            //            .Name;

        //            //    if (churchStructureCollectionPercent.Percent > 0)
        //            //    {
        //            //        var percentRemit = (thisCollectionTotalAmount * churchStructureCollectionPercent.Percent);
        //            //        thisCollectionTotalPercentRemit += percentRemit;
        //            //        structurePercentages.Add(new CollectionRemittanceChurchStructureTypeObj
        //            //        {
        //            //            ChurchStructureTypeId = churchStructureCollectionPercent.ChurchStructureTypeId,
        //            //            ChurchStructureTypeName = structureName,
        //            //            Percent = churchStructureCollectionPercent.Percent,
        //            //            AmountRemitted = percentRemit
        //            //        });
        //            //    }

        //            //}

        //            var thisCollectionTypeRemittanceDetailObj = new ClientCollectionRemittanceDetailObj
        //            {
        //                //CollectionRefId = th,
        //                //CollectionTypeName = collectionTypeName,
        //                TotalMonthlyCaptured = thisCollectionTotalAmount,
        //                TotalPercentRemitted = thisCollectionTotalPercentRemit,
        //                TotalBalanceLeft = (thisCollectionTotalAmount - thisCollectionTotalPercentRemit),
        //                CollectionRemittanceChurchStructureType = structurePercentages

        //            };
        //            thisMonthCollectionRemittanceDetails.Add(thisCollectionTypeRemittanceDetailObj);
        //            thisCollectionTotalAmount = 0;
        //        }

        //        #endregion

        //        #region Submitting Remittance
        //        var churchServiceAttendanceRemittanceObj = new ChurchServiceAttendanceRemittance
        //        {
        //            ClientId = clientId,
        //            From = start,
        //            To = end,
        //            RemittanceChurchServiceDetail = churchServiceAttendanceDetails,
        //            ChurchServiceAttendanceRemittanceCollection = thisMonthChurchServiceAttendanceRemittanceCollections,
        //            CollectionRemittanceDetail = thisMonthCollectionRemittanceDetails,
        //            TimeStampRemitted = DateScrutnizer.CurrentTimeStamp(),
        //            RemittedByUserId = 1
        //        };

        //        var processedChurchServiceAttendanceRemittance = _repository.Add(churchServiceAttendanceRemittanceObj);
        //        _uoWork.SaveChanges();
        //        if (processedChurchServiceAttendanceRemittance.ChurchServiceAttendanceRemittanceId < 1)
        //        {
        //            return new ChurchServiceAttendanceRemittance();
        //        }
        //        #endregion


        //        return processedChurchServiceAttendanceRemittance;


        //    }
        //    catch (Exception ex)
        //    {
        //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        return null;
        //    }
        //}

        //internal ChurchServiceAttendanceRemittance IsAttendanceRemittanceExist(long clientId, string start, string end, out string msg)
        //{
        //    try
        //    {
        //        if (clientId > 0 && !start.IsNullOrEmpty() && !end.IsNullOrEmpty())
        //        {
        //            //var sql1 =
        //            //string.Format(
        //            //"Select * FROM \"ICASDB\".\"ChurchServiceAttendanceRemittance\" WHERE " +
        //            //" \"ClientId\" = {0} AND \"Year\" = {1} " +
        //            //"AND \"Month\" = {2} ", clientId, year, month);

        //            //var check = _repository.RepositoryContext().Database.SqlQuery<ChurchServiceAttendanceRemittance>(sql1).ToList();


        //            var myItems = _repository.GetAll().ToList();
        //            if (!myItems.Any() || myItems.Count == 0)
        //            {
        //                msg = "No church service attendance remittance record found for the selected date range";
        //                return null;
        //            }

        //            var retItem =
        //                myItems.Find(
        //                    x =>
        //                        DateTime.Parse(x.From) == DateTime.Parse(start) &&
        //                        DateTime.Parse(x.To) == DateTime.Parse(end));
        //            if (retItem == null || retItem.ChurchServiceAttendanceRemittanceId < 1)
        //            {
        //                msg = "No church service attendance remittance record found for the selected date range";
        //                return null;
        //            }

        //            msg = "";
        //            return retItem;


        //            //var check = _repository.GetAll(x => x.ClientId == clientId && x.From == start.Trim() && x.To == end.Trim()).ToList();
        //            //if (!check.Any() || check.Count == 0)
        //            //{
        //            //    msg = "No attendance remittance found";
        //            //    return null;
        //            //}

        //            //msg = "";
        //            //return check[0];
        //        }

        //        msg = "Unable to check Church Service Attendance Remittance Existency! Please try again later";
        //        return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "Error: " + ex.Message;
        //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        return null;
        //    }
        //}


        //internal List<ChurchServiceAttendanceRemittanceReportObj> GetChurchServiceAttendanceRemittances()
        //{

        //    try
        //    {
        //        var myItemList = _repository.GetAll().ToList();
        //        if (!myItemList.Any()) { return new List<ChurchServiceAttendanceRemittanceReportObj>(); }
        //        //return myItemList;

        //        var retItems = new List<ChurchServiceAttendanceRemittanceReportObj>();

        //        myItemList.ForEachx(m =>
        //        {
        //            //var serviceName = _churchServiceTypeRepository.GetById(m.ChurchServiceTypeId).Name;
        //            // List<string> properties = objectList.Select(o => o.StringProperty).ToList();
        //            var amountCaptures = m.CollectionRemittanceDetail.Select(x => x.TotalMonthlyCaptured).Sum();
        //            var amountRemitts = m.CollectionRemittanceDetail.Select(x => x.TotalPercentRemitted).Sum();
        //            var leftBalances = m.CollectionRemittanceDetail.Select(x => x.TotalBalanceLeft).Sum();
        //            var totalMonthlyAttendee =
        //                m.RemittanceChurchServiceDetail.Select(
        //                    x => x.RemittanceChurchServiceAttendeeDetail.TotalAttendee).Sum();

        //            var dateFrom = m.From;
        //            var dateTo = m.To;
        //            if (!dateFrom.IsNullOrEmpty() && !dateTo.IsNullOrEmpty())
        //            {
        //                dateFrom = DateScrutnizer.ReverseToGeneralDate(dateFrom);
        //                dateTo = DateScrutnizer.ReverseToGeneralDate(dateTo);

        //                var dt = DateTime.ParseExact(dateFrom, "yyyy/MM/dd", CultureInfo.InvariantCulture);
        //                dateFrom = dt.ToString("dd MMMM");

        //                dt = DateTime.ParseExact(dateTo, "yyyy/MM/dd", CultureInfo.InvariantCulture);
        //                dateTo = dt.ToString("dd MMMM, yyyy");
        //            }

        //            retItems.Add(new ChurchServiceAttendanceRemittanceReportObj
        //            {
        //                ChurchServiceAttendanceRemittanceId = m.ChurchServiceAttendanceRemittanceId,
        //                DateRange = dateFrom + " - " + dateTo,
        //                TotalMonthlyAttendee = totalMonthlyAttendee,
        //                TotalMonthlyAmountCaptured = amountCaptures,
        //                TotalMonthlyAmountRemitted = amountRemitts,
        //                TotalMonthlyBalanceLeft = leftBalances,
        //                RemittanceDetailReport = new RemittanceDetailReportObj
        //                {
        //                    ChurchServiceAttendanceRemittanceCollections = m.ChurchServiceAttendanceRemittanceCollection,
        //                    CollectionRemittanceDetails = m.CollectionRemittanceDetail,
        //                    RemittanceChurchServiceDetails = m.RemittanceChurchServiceDetail
        //                }
        //            });
        //        });

        //        return retItems;
        //    }
        //    catch (Exception ex)
        //    {
        //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        return new List<ChurchServiceAttendanceRemittanceReportObj>();
        //    }

        //}


        //internal ChurchServiceAttendanceRemittance ComputeRemittance(long churchId, long clientId, int year, int month)
        //{
        //    try
        //    {

        //        string msg;
        //        var thisMonthChurchServiceAttendanceRemittanceCollections =
        //            new List<ChurchServiceAttendanceRemittanceCollectionObj>();
        //        var thisMonthCollectionRemittanceDetails =
        //            new List<CollectionRemittanceDetailObj>();

        //        // Get All Church Service Attendance for this Client in this Year and Month
        //        var thisMonthChurchServiceAttendances =
        //            new ChurchServiceAttendanceRepository().GetMonthlyChurchServiceAttendance(clientId, year, month,
        //                out msg);

        //        if (!thisMonthChurchServiceAttendances.Any()) { return new ChurchServiceAttendanceRemittance(); }

        //        // Get this Client Church Collection Types
        //        var clientChurchCollectionType =
        //            new ChurchCollectionTypeRepository().GetChurchCollectionType(churchId);
        //        var thisClientChurchCollectionTypes = clientChurchCollectionType.CollectionTypes;


        //        double thisCollectionTotalAmount = 0;
        //        foreach (var thisClientChurchCollectionType in thisClientChurchCollectionTypes)
        //        {
        //            var thisCollectionTypeId = thisClientChurchCollectionType.CollectionTypeId;

        //            //var thisCollectionMonthly =
        //            //    thisMonthChurchServiceAttendances.Select(
        //            //        x =>
        //            //            x.ServiceAttendanceDetail.ChurchServiceAttendanceCollections.Where(
        //            //                i => i.CollectionTypeId == thisCollectionTypeId)).ToList();


        //            // Loop through All Church Service Attendance for this month and Total the current 
        //            // CollectionType in all the Attendance
        //            //foreach (var thisMonthChurchServiceAttendance in thisMonthChurchServiceAttendances)
        //            //{
        //            //    var collectionTypes =
        //            //        thisMonthChurchServiceAttendance.ServiceAttendanceDetail.ChurchServiceAttendanceCollections;
        //            //    var amount = collectionTypes.Find(x => x.CollectionTypeId == thisCollectionTypeId).Amount;
        //            //    thisCollectionTotalAmount += amount;
        //            //}


        //            // Loop through All Church Service Attendance for this month and Total the current 
        //            // CollectionType in all the Attendance
        //            //thisCollectionTotalAmount +=
        //            //    thisMonthChurchServiceAttendances.Select(
        //            //        thisMonthChurchServiceAttendance =>
        //            //            thisMonthChurchServiceAttendance.ServiceAttendanceDetail
        //            //                .ClientChurchServiceAttendanceCollections)
        //            //        .Select(
        //            //            collectionTypes =>
        //            //                collectionTypes.Find(x => x.CollectionRefId == thisCollectionTypeId).Amount)
        //            //        .Sum();


        //            // Here Compose the Object going to Remittance Table
        //            var thisCollectionTypeRemittanceObj = new ChurchServiceAttendanceRemittanceCollectionObj
        //            {
        //                CollectionTypeId = thisCollectionTypeId,
        //                CollectionTypeName = "",
        //                TotalRemittance = thisCollectionTotalAmount
        //            };
        //            thisMonthChurchServiceAttendanceRemittanceCollections.Add(thisCollectionTypeRemittanceObj);


        //            // Loop Through this Client ChurchCollectionTypes with ChurchStructureType 
        //            // And Calculate the Percentage of each ChurchStructureType (i.e. National, Region etc. 
        //            // On this current CollectionType (i.e. Offerring)
        //            var structurePercentages = new List<CollectionRemittanceChurchStructureTypeObj>();
        //            double thisCollectionTotalPercentRemit = 0;
        //            //foreach (var churchStructureCollectionPercent in thisClientChurchCollectionType.ChurchStructureTypeObjs)
        //            //{
        //            //    if (churchStructureCollectionPercent.Percent > 0)
        //            //    {
        //            //        var percentRemit = (thisCollectionTotalAmount * churchStructureCollectionPercent.Percent);
        //            //        thisCollectionTotalPercentRemit += percentRemit;
        //            //        structurePercentages.Add(new CollectionRemittanceChurchStructureTypeObj
        //            //        {
        //            //            ChurchStructureTypeId = churchStructureCollectionPercent.ChurchStructureTypeId,
        //            //            AmountRemitted = percentRemit
        //            //        });
        //            //    }
        //            //}

        //            // Here Compose the second Object going to Remittance Table
        //            var thisCollectionTypeRemittanceDetailObj = new CollectionRemittanceDetailObj
        //            {
        //                CollectionTypeId = thisCollectionTypeId,
        //                CollectionTypeName = "",
        //                TotalMonthlyCaptured = thisCollectionTotalAmount,
        //                TotalPercentRemitted = thisCollectionTotalPercentRemit,
        //                TotalBalanceLeft = (thisCollectionTotalAmount - thisCollectionTotalPercentRemit),
        //                CollectionRemittanceChurchStructureType = structurePercentages

        //            };

        //            thisMonthCollectionRemittanceDetails.Add(thisCollectionTypeRemittanceDetailObj);
        //            thisCollectionTotalAmount = 0;
        //        }



        //        //var existenceRemittanceCollections = new List<ChurchServiceAttendanceRemittanceCollectionObj>();
        //        //var takenRemittanceCollections = new List<ChurchServiceAttendanceRemittanceCollectionObj>();

        //        // Used to accept the just taken attendance Collection types
        //        //var takenCollections =
        //        //        churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail.ChurchServiceAttendanceCollections;


        //        // Check Remittance Table to confirm Existence
        //        //var check =
        //        //    new ChurchServiceAttendanceRemittanceRepository().IsAttendanceRemittanceExist(
        //        //        churchServiceAttendanceRegObj.ClientId, year, month, out msg);

        //        //if (check != null && check.ChurchServiceAttendanceRemittanceId > 0)
        //        //{
        //        //    // Can be used to calculate Remittance for Monthly at once
        //        //    //double total = myList.Where(item => item.Name == "Eggs").Sum(item => item.Amount);



        //        //    //existenceRemittanceCollections = check.ChurchServiceAttendanceRemittanceCollection;
        //        //    //var i = 0;
        //        //    //foreach (var addUpCollectionAmount in check.ChurchServiceAttendanceRemittanceCollection
        //        //    //    .Select(existCollectionRemittance => 
        //        //    //        takenCollections.Find(x => 
        //        //    //            x.CollectionTypeId == existCollectionRemittance.CollectionTypeId).Amount))
        //        //    //{
        //        //    //    check.ChurchServiceAttendanceRemittanceCollection[i].TotalRemittance +=
        //        //    //        addUpCollectionAmount;

        //        //    //    i++;
        //        //    //}

        //        //    //foreach (var existCollectionRemittance in check.ChurchServiceAttendanceRemittanceCollection)
        //        //    //{
        //        //    //    var addUpCollectionAmount =
        //        //    //        takenCollections.Find(x => x.CollectionTypeId == existCollectionRemittance.CollectionTypeId)
        //        //    //            .Amount;

        //        //    //    check.ChurchServiceAttendanceRemittanceCollection[i].TotalRemittance +=
        //        //    //        addUpCollectionAmount;

        //        //    //    i++;
        //        //    //}

        //        //    existenceRemittanceCollections = check.ChurchServiceAttendanceRemittanceCollection;
        //        //    foreach (var existCollectionRemittance in existenceRemittanceCollections)
        //        //    {
        //        //        var addUpCollectionAmount =
        //        //            takenCollections.Find(x => x.CollectionTypeId == existCollectionRemittance.CollectionTypeId)
        //        //                .Amount;
        //        //        existCollectionRemittance.TotalRemittance += addUpCollectionAmount;
        //        //    }
        //        //    // Then Update after changes to all Collection Types Total Remittance
        //        //    check.ChurchServiceAttendanceRemittanceCollection = existenceRemittanceCollections;
        //        //}
        //        //else
        //        //{

        //        //    takenCollections.ForEachx(x => takenRemittanceCollections.Add(new ChurchServiceAttendanceRemittanceCollectionObj
        //        //    {
        //        //        CollectionTypeId = x.CollectionTypeId,
        //        //        CollectionTypeName = x.CollectionTypeName,
        //        //        TotalRemittance = x.Amount
        //        //    }));
        //        //    //foreach (var takenCollection in takenCollections)
        //        //    //{
        //        //    //    takenRemittanceCollections = new List<ChurchServiceAttendanceRemittanceCollectionObj>
        //        //    //    {
        //        //    //        new ChurchServiceAttendanceRemittanceCollectionObj
        //        //    //        {
        //        //    //            CollectionTypeId = takenCollection.CollectionTypeId,
        //        //    //            CollectionTypeName = takenCollection.CollectionTypeName,
        //        //    //            TotalRemittance = takenCollection.Amount
        //        //    //        }
        //        //    //    };
        //        //    //}
        //        //}



        //        //if (takenRemittanceCollections.Any() && takenRemittanceCollections.Count > 0)
        //        //{
        //        //    thisChurchServiceAttendance.ChurchServiceAttendanceRemittance = new ChurchServiceAttendanceRemittance
        //        //    {
        //        //        ClientId = churchServiceAttendanceRegObj.ClientId,
        //        //        Year = year,
        //        //        Month = month,
        //        //        _ServiceAttendanceRemittanceCollection = JsonConvert.SerializeObject(takenRemittanceCollections),
        //        //    };
        //        //}



        //        //if (check != null && check.ChurchServiceAttendanceRemittanceId > 0)
        //        //{
        //        //    var processedUpdateRemittance = _churchServiceAttendanceRemittanceRepository.Update(check);
        //        //    _uoWork.SaveChanges();
        //        //    if (processedUpdateRemittance.ChurchServiceAttendanceRemittanceId < 1)
        //        //    {
        //        //        db.Rollback();
        //        //        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
        //        //        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
        //        //        response.Status.IsSuccessful = false;
        //        //        return response;
        //        //    }
        //        //}

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        return null;
        //    }
        //}

        #endregion

        

        

        

        
    }
}
