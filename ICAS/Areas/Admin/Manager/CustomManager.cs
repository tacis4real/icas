using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.Enum;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Areas.Admin.Manager
{
    public class CustomManager
    {

        #region Load Structure Church Parishes
        public static List<RegisteredStructureChurchParishReportObj> GetChurchStructureParishHeadQuarters(long churchId, int stateId, int churchStructureTypeId)
        {
            try
            {
                var items = ServiceChurch.GetChurchStructureParishHeadQuartersByChurchStateId(churchId, stateId, churchStructureTypeId).ToList();
                if (!items.Any())
                {
                    return new List<RegisteredStructureChurchParishReportObj>();
                }

                return items;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredStructureChurchParishReportObj>();
            }

        }

        #endregion




        public class UniqueTick
        {
            public long Tick { get; set; }
        }


        public static List<UniqueTick> GetUniqueTicks()
        {

            var ticks = new List<UniqueTick>();
            for (int i = 0; i < 10; i++)
            {
                ticks.Add(new UniqueTick
                {
                    Tick = GetUniqueId()
                });
            }

            return ticks;
        } 


        public static long GetUniqueId()
        {

            return DateTime.Now.Ticks;

            //string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

            //long ticks = DateTime.Now.Ticks;
            //long ticks = (DateTime.Now.Millisecond/10)%1000000000;
            //long ticks = DateTime.Now.Ticks;
            //var bytes = BitConverter.GetBytes(ticks);
            //var id = Convert.ToBase64String(bytes)
            //                        .Replace('+', '_')
            //                        .Replace('/', '-')
            //                        .TrimEnd('=');

            //return id;
        }



        #region Dashboard Objects

        public static DashboardObj GetDashboardObjs()
        {
            try
            {
                var dashboard = new DashboardObj();
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return dashboard;
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    
                    // Parent church
                    var noOfChurch = 0;
                    var churchs = GetParentChurches();
                    var clients = GetClients();

                    if (churchs.Any() || churchs != null)
                    {
                        noOfChurch +=
                            churchs.Select(church => clients.Find(x => x.ChurchId == church.ChurchId))
                                .Count(check => check.ChurchId >= 1 && check.ClientId >= 1);

                        //noOfChurch +=
                        //    churchs.Select(church => clients.Find(x => x.ChurchId == church.ChurchId))
                        //        .Count(check => check.ChurchId >= 1 && check.ClientId >= 1);
                    }
                    dashboard.ParentChurch = noOfChurch;


                    // Clients
                    var sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat("SELECT COUNT(*) FROM \"ICASDB\".\"Client\"  ");
                    var sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteScalar();
                    dashboard.Client = Convert.ToInt64(dr);


                    // Members
                    sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat("SELECT COUNT(*) FROM \"ICASDB\".\"ChurchMember\"  ");
                    sql = sqlBuilder.ToString();
                    command = new SqlCommand(sql, sqlConnection);
                    dr = command.ExecuteScalar();
                    dashboard.ChurchMember = Convert.ToInt64(dr);

                }

                return dashboard;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new DashboardObj(); 
            }
        }


        public static ClientDashboardObj GetClientDashboardObjs(long clientId)
        {
            try
            {
                var dashboard = new ClientDashboardObj();
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return dashboard;
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();

                    // Men & Women
                    var noOfMens = 0;
                    var noOfWomens = 0;
                    var totalMembers = 0;
                    var totalServices = 0;
                    double malePercentage = 0;
                    double femalePercentage = 0;
                    var churchMembers = GetClientMembers(clientId);
                    var churchServices = GetClientChurchServices(clientId);

                    if (churchMembers != null && churchMembers.Count > 0)
                    {
                        noOfMens += churchMembers.Count(m => m.Sex == (int)Sex.Male);
                        noOfWomens += churchMembers.Count(m => m.Sex == (int)Sex.Female);
                        totalMembers += churchMembers.Count;

                        // Calculate Percentage
                        malePercentage = Math.Round((Convert.ToDouble(noOfMens) / Convert.ToDouble(totalMembers)) * 100, 1);
                        femalePercentage = Math.Round((Convert.ToDouble(noOfWomens) / Convert.ToDouble(totalMembers)) * 100, 1);
                    }

                    if (churchServices != null)
                    {
                        totalServices += churchServices.Count;
                    }
                    
                    dashboard.MaleMember = noOfMens;
                    dashboard.FemaleMember = noOfWomens;
                    dashboard.MalePercent = malePercentage;
                    dashboard.FemalePercent = femalePercentage;
                    // Total Members
                    dashboard.ChurchMember = totalMembers;

                    // Total Services
                    dashboard.ChurchService = totalServices;
                }

                return dashboard;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientDashboardObj();
            }
        }
        #endregion

        #region Administrative

        public static List<NameAndValueObject> GetDaysOfWeekList()
        {
            var items = Enum.GetValues(typeof(WeekDays)).Cast<WeekDays>().Select(x => new NameAndValueObject
            {
                Name = x.ToString(),
                Id = ((int)x)
            }).ToList();

            return items;
        }
        

        public static List<RoleInChurch> GetRolesInChurch()
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<RoleInChurch>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"RoleInChurch\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<RoleInChurch>();
                    while (dr.Read())
                    {
                        var item = new RoleInChurch();
                        {
                            item.RoleInChurchId = dr.GetInt32(dr.GetOrdinal("RoleInChurchId"));
                            //item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RoleInChurch>();
            }
        }
        public static List<RoleInChurch> GetRolesInChurchs(long clientId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<RoleInChurch>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"RoleInChurch\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<RoleInChurch>();
                    while (dr.Read())
                    {
                        var item = new RoleInChurch();
                        {
                            item.RoleInChurchId = dr.GetInt32(dr.GetOrdinal("RoleInChurchId"));
                            //item.Name = dr.GetString(dr.GetOrdinal("Name"));

                            item.AddedByUserId = dr.GetInt32(dr.GetOrdinal("AddedByUserId"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    if (clientId > 0)
                    {
                        var clientRoles = items.FindAll(x => x.AddedByUserId == clientId).ToList();
                        var defaultRoles = items.FindAll(x => x.AddedByUserId == 1).ToList();

                        defaultRoles.AddRange(clientRoles);
                        return defaultRoles;
                    }
                    return items.FindAll(x => x.AddedByUserId == 1);
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RoleInChurch>();
            }
        }


        public static List<ChurchServiceDetailObj> GetClientChurchServiceList(long clientChurchId)
        {
            try
            {
                var items = new List<ChurchServiceDetailObj>();
                var clientChurchService = ServiceChurch.GetClientChurchService(clientChurchId);
                if (clientChurchService == null || clientChurchService.Count < 1)
                {
                    return new List<ChurchServiceDetailObj>();
                }

                clientChurchService.ForEachx(x => items.Add(new ChurchServiceDetailObj
                {
                    ChurchServiceTypeRefId = x.ChurchServiceTypeRefId,
                    Name = x.Name
                }));

                return items;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchServiceDetailObj>();
            }
        }


        public static List<ChurchServiceType> GetChurchServices(long churchId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<ChurchServiceType>();
                }

                var items = new List<ChurchServiceType>();
                var churchService = ServiceChurch.GetChurchService(churchId);
                if (churchService == null || churchService.ChurchServiceId < 1)
                {
                    return new List<ChurchServiceType>();
                }

                churchService.ServiceTypeDetail.ForEachx(x => items.Add(new ChurchServiceType
                {
                    ChurchServiceTypeId = x.ChurchServiceTypeId,
                    Name = x.Name
                }));

                return items;
                
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchServiceType>();
            }
        }

        public static List<ChurchService> GetChurchServices1(long churchId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<ChurchService>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"ChurchService\" A " +
                                            " LEFT JOIN \"ICASDB\".\"ChurchServiceType\" B " +
                                            " ON A.\"ChurchServiceTypeId\" = B.\"ChurchServiceTypeId\" " +
                                            " WHERE \"ChurchId\" = {0}", churchId);



                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"ChurchService\" " +
                                            "WHERE \"ChurchId\" = {0}", churchId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<ChurchService>();
                    while (dr.Read())
                    {
                        var item = new ChurchService();
                        {
                            //item.ChurchServiceTypeId = dr.GetInt32(dr.GetOrdinal("ChurchServiceTypeId"));
                            //item.Name = dr.GetString(dr.GetOrdinal("Name"));

                            //item.ClientId = dr.GetInt64(dr.GetOrdinal("ClientId"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    //if (clientId > 0)
                    //{
                    //    var clientChurchServices = items.FindAll(x => x.ClientId == clientId).ToList();
                    //    var defaultChurchServices = items.FindAll(x => x.ClientId == 1).ToList();

                    //    defaultChurchServices.AddRange(clientChurchServices);
                    //    return defaultChurchServices;
                    //}
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchService>();
            }
        }


        public static List<Profession> GetProfessionx(long clientId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<Profession>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"Profession\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<Profession>();
                    while (dr.Read())
                    {
                        var item = new Profession();
                        {
                            item.ProfessionId = dr.GetInt32(dr.GetOrdinal("ProfessionId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items.FindAll(x => x.AddedByUserId == 1 && x.AddedByUserId == clientId);
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<Profession>();
            }
        }


        public static List<Profession> GetProfessions()
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<Profession>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"Profession\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<Profession>();
                    while (dr.Read())
                    {
                        var item = new Profession();
                        {
                            item.ProfessionId = dr.GetInt32(dr.GetOrdinal("ProfessionId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<Profession>();
            }
        }
        #endregion

        public static bool ResetTable(string table)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return false;
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();
                    //sqlBuilder.AppendFormat("SELECT * " +
                    //                        " FROM \"ICASDB\".\"" + table + "\" ");

                    //string.Format("DBCC CHECKIDENT ({0}, RESEED, 1)", table);

                    sqlBuilder.AppendFormat("DBCC CHECKIDENT(\"ICASDB\".\"" + table + ", RESEED, 1");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    if (dr.RecordsAffected > 0)
                    {
                        return true;
                    }
                    sqlConnection.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }






        
        public static List<StateOfLocation> GetStateOfLocations()
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<StateOfLocation>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"StateOfLocation\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<StateOfLocation>();
                    while (dr.Read())
                    {
                        var item = new StateOfLocation();
                        {
                            item.StateOfLocationId = dr.GetInt32(dr.GetOrdinal("StateOfLocationId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<StateOfLocation>();
            }

        }

        public static List<Bank> GetBanks()
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<Bank>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"Bank\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<Bank>();
                    while (dr.Read())
                    {
                        var item = new Bank();
                        {
                            item.BankId = dr.GetInt32(dr.GetOrdinal("BankId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<Bank>();
            } 
        }

        public static List<ChurchStructureType> GetChurchStructureTypes()
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<ChurchStructureType>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"ChurchStructureType\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<ChurchStructureType>();
                    while (dr.Read())
                    {
                        var item = new ChurchStructureType();
                        {
                            item.ChurchStructureTypeId = dr.GetInt32(dr.GetOrdinal("ChurchStructureTypeId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchStructureType>();
            }
        }


        public static List<ChurchStructureType> GetChurchStructureByChurchId(long churchId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<ChurchStructureType>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"ChurchStructure\" A " +
                                            " LEFT JOIN \"ICASDB\".\"ChurchStructureType\" B " +
                                            " ON A.\"ChurchStructureTypeId\" = B.\"ChurchStructureTypeId\" " +
                                            " WHERE \"ChurchId\" = {0}", churchId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<ChurchStructureType>();
                    while (dr.Read())
                    {
                        var status = dr.GetInt32(dr.GetOrdinal("Status"));
                        if (status == 0 || status == 2) continue;
                        var item = new ChurchStructureType();
                        {
                            item.ChurchStructureTypeId = dr.GetInt32(dr.GetOrdinal("ChurchStructureTypeId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchStructureType>();
            }
        }

        public static List<ChurchStructureType> GetChurchStructureTypeByChurchId(long churchId)
        {
            try
            {

                var items = ServiceChurch.GetChurchStructureTypeByChurchId(churchId);
                if (!items.Any())
                {
                    return new List<ChurchStructureType>();
                }

                return items;

                //var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                //if (string.IsNullOrEmpty(connection))
                //{
                //    return new List<ChurchStructureType>();
                //}

                //using (var sqlConnection = new SqlConnection(connection))
                //{
                //    sqlConnection.Open();
                //    var sqlBuilder = new StringBuilder();
                //    sqlBuilder.AppendFormat("SELECT * " +
                //                            " FROM \"ICASDB\".\"ChurchStructure\" A " +
                //                            " LEFT JOIN \"ICASDB\".\"ChurchStructureType\" B " +
                //                            " ON A.\"ChurchStructureTypeId\" = B.\"ChurchStructureTypeId\" " +
                //                            " WHERE \"ChurchId\" = {0}", churchId);
                //    string sql = sqlBuilder.ToString();
                //    var command = new SqlCommand(sql, sqlConnection);
                //    var dr = command.ExecuteReader();
                //    var items = new List<ChurchStructureType>();
                //    while (dr.Read())
                //    {
                //        var status = dr.GetInt32(dr.GetOrdinal("Status"));
                //        if (status == 0 || status == 2) continue;
                //        var item = new ChurchStructureType();
                //        {
                //            item.ChurchStructureTypeId = dr.GetInt32(dr.GetOrdinal("ChurchStructureTypeId"));
                //            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                //        };
                //        items.Add(item);
                //    }
                //    sqlConnection.Close();
                //    return items;
                //}
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchStructureType>();
            }
        }



        public static List<NameAndValueObject> GetTitleList()
        {
            var items = Enum.GetValues(typeof(Title)).Cast<Title>().Select(x => new NameAndValueObject
            {
                Name = x.ToString(),
                Id = ((int)x)
            }).ToList();

            return items;
        }

        public static List<Church> GetParentChurches()
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<Church>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"Church\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<Church>();
                    while (dr.Read())
                    {
                        var item = new Church();
                        {
                            item.ChurchId = dr.GetInt64(dr.GetOrdinal("ChurchId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    //return items;
                    return items.FindAll(x => x.ChurchId > 2);
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<Church>();
            }
        }
        
        public static List<Client> GetClients()
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<Client>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"Client\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<Client>();
                    while (dr.Read())
                    {
                        var item = new Client();
                        {
                            item.ClientId = dr.GetInt64(dr.GetOrdinal("ClientId"));
                            item.ChurchId = dr.GetInt64(dr.GetOrdinal("ChurchId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<Client>();
            }
        }

        public static List<ChurchMember> GetClientMembers(long clientId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<ChurchMember>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * " +
                                           " FROM \"ICASDB\".\"ChurchMember\" " +
                                           " WHERE \"ClientChurchId\" = {0}", clientId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<ChurchMember>();
                    while (dr.Read())
                    {
                        //var item = new Member();
                        //{
                        //    item.Children = dr.GetInt64(dr.GetOrdinal("ClientId"));
                        //    item.ChurchId = dr.GetInt64(dr.GetOrdinal("ChurchId"));
                        //    item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        //};
                        var item = new ChurchMember();
                        {
                            item.ChurchMemberId = dr.GetInt64(dr.GetOrdinal("ChurchMemberId"));
                            item.FullName = dr.GetString(dr.GetOrdinal("FullName"));
                            item.ClientChurchId = dr.GetInt64(dr.GetOrdinal("ClientChurchId"));
                            item.Sex = dr.GetInt32(dr.GetOrdinal("Sex"));
                        }
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchMember>();
            }
        }

        public static List<ChurchService> GetClientDefaulChurchServices(long churchId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<ChurchService>();
                }

                if (churchId < 1) { return new List<ChurchService>(); }
                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();
                    
                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"ChurchService\" " +
                                            " WHERE \"ChurchId\" = {0}", churchId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<ChurchService>();
                    while (dr.Read())
                    {
                        var item = new ChurchService();
                        {
                            //item.ChurchServiceTypeId = dr.GetInt32(dr.GetOrdinal("ChurchServiceTypeId"));
                            //item.DayOfWeekId = dr.GetInt32(dr.GetOrdinal("DayOfWeekId"));
                        }
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchService>();
            }
        }


        public static List<ChurchServiceType> GetClientChurchServices(long clientId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<ChurchServiceType>();
                }

                if (clientId < 1) { return new List<ChurchServiceType>(); }
                var churchId = GetClientChurchId(clientId);
                if (churchId < 1) { return new List<ChurchServiceType>(); }

                // Get this Client Default Church Services
                var defaultChurchService = GetClientDefaulChurchServices(churchId);

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();


                    //sqlBuilder.AppendFormat("SELECT * " +
                    //                       " FROM \"ICASDB\".\"ChurchService\" " +
                    //                       " WHERE \"ClientId\" = {0}", clientId);

                    //sqlBuilder.AppendFormat("SELECT * " +
                    //                        " FROM \"ICASDB\".\"ClientChurchService\" A " +
                    //                        " LEFT JOIN \"ICASDB\".\"Client\" B " +
                    //                        " ON A.\"ClientId\" = B.\"ClientId\" " +
                    //                        " LEFT JOIN \"ICASDB\".\"ChurchService\" C " +
                    //                        " ON B.\"ChurchId\" = C.\"ChurchId\" " +
                    //                        " WHERE \"ClientId\" = {0}", clientId);

                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"ClientChurchService\" " +
                                            " WHERE \"ClientId\" = {0}", clientId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<ClientChurchService>();

                    var retList = new List<ChurchServiceType>();
                    while (dr.Read())
                    {
                        var item = new ClientChurchService();
                        {
                            //item.ChurchServiceTypeId = dr.GetInt32(dr.GetOrdinal("ChurchServiceTypeId"));
                            //item.DayOfWeekId = dr.GetInt32(dr.GetOrdinal("DayOfWeekId"));
                        }
                        items.Add(item);
                    }
                    sqlConnection.Close();

                    if (defaultChurchService.Any())
                    {
                        defaultChurchService.ForEachx(x => items.Add(new ClientChurchService
                        {
                            //ChurchServiceTypeId = x.ChurchServiceTypeId,
                            //DayOfWeekId = x.DayOfWeekId,
                        }));
                    }




                    //myItemList.ForEachx(m =>
                    //{
                    //    string msg;
                    //    var client = GetClient(m.ClientId, out msg);
                    //    var church = GetClientChurch(m.ClientId);


                    //    retList.Add(new RegisteredClientAccountListReportObj
                    //    {
                    //        ClientAccountId = m.ClientAccountId,
                    //        ChurchId = church.ChurchId,
                    //        ClientId = m.ClientId,
                    //        Church = church.ShortName,
                    //        Client = client.Name,
                    //        BankId = m.BankId,
                    //        Bank = _bankRepository.GetById(m.BankId).Name,
                    //        AccountName = m.AccountName,
                    //        AccountNumber = m.AccountNumber,
                    //        AccountTypeId = m.AccountTypeId,
                    //        AccountType = Enum.GetName(typeof(AccountType), m.AccountTypeId),
                    //        Status = m.Status,
                    //        StatusType = Enum.GetName(typeof(ClientAccountStatus), m.Status),
                    //    });
                    //});

                    if (items.Any())
                    {
                        items.ForEachx(x =>
                        {
                            //var churchServiceType = GetChurchServiceType(x.ChurchServiceTypeId);
                            //retList.Add(churchServiceType);
                        });
                    }

                    return retList;

                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchServiceType>();
            }
        }


        public static ChurchServiceType GetChurchServiceType(int churchServiceTypeId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new ChurchServiceType();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"ChurchServiceType\" " +
                                            " WHERE \"ChurchServiceTypeId\" = {0}", churchServiceTypeId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var item = new ChurchServiceType();
                    while (dr.Read())
                    {
                        item = new ChurchServiceType();
                        {
                            item.ChurchServiceTypeId = dr.GetInt32(dr.GetOrdinal("ChurchServiceTypeId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                    }
                    sqlConnection.Close();
                    return item;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ChurchServiceType();;
            }
        }


        public static long GetClientChurchId(long clientId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return 0;
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"Client\" " +
                                            " WHERE \"ClientId\" = {0}", clientId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var item = new Church();
                    while (dr.Read())
                    {
                        item = new Church();
                        {
                            item.ChurchId = dr.GetInt64(dr.GetOrdinal("ChurchId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                    }
                    sqlConnection.Close();
                    return item.ChurchId;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }


        public static List<Client> GetClientsByChurchId(long churchId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<Client>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"Client\" " +
                                            " WHERE \"ChurchId\" = {0}", churchId);

                    //sqlBuilder.AppendFormat("SELECT * FROM \"ICASDB\".\"Client\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<Client>();
                    while (dr.Read())
                    {
                        var item = new Client();
                        {
                            item.ClientId = dr.GetInt64(dr.GetOrdinal("ClientId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<Client>();
            }
        }


        public static List<Client> GetClientsByStateChurchId(long churchId, int stateId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<Client>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"Client\" " +
                                            " WHERE \"ChurchId\" = {0}" +
                                            " AND \"StateOfLocationId\" = {1}", churchId, stateId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<Client>();
                    while (dr.Read())
                    {
                        var item = new Client();
                        {
                            item.ClientId = dr.GetInt64(dr.GetOrdinal("ClientId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<Client>();
            }
        }



        
        #region Structures

        #region Used One
        
        public static List<StructureChurchRegObj> GetStructureListByType(string structureType)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<StructureChurchRegObj>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"" + structureType + "\" ");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<StructureChurchRegObj>();
                    while (dr.Read())
                    {
                        var item = new StructureChurchRegObj();
                        {

                            if (typeof(Parish).Name == structureType)
                            {
                                item.TypeStructureChurchId = dr.GetInt64(dr.GetOrdinal("ParishId"));
                            }

                            if (typeof(Region).Name == structureType)
                            {
                                item.TypeStructureChurchId = dr.GetInt64(dr.GetOrdinal("RegionId"));
                            }

                            if (typeof(Province).Name == structureType)
                            {
                                item.TypeStructureChurchId = dr.GetInt64(dr.GetOrdinal("ProvinceId"));
                            }

                            if (typeof(Zone).Name == structureType)
                            {
                                item.TypeStructureChurchId = dr.GetInt64(dr.GetOrdinal("ZoneId"));
                            }

                            if (typeof(Area).Name == structureType)
                            {
                                item.TypeStructureChurchId = dr.GetInt64(dr.GetOrdinal("AreaId"));
                            }

                            if (typeof(Diocese).Name == structureType)
                            {
                                item.TypeStructureChurchId = dr.GetInt64(dr.GetOrdinal("DioceseId"));
                            }

                            if (typeof(District).Name == structureType)
                            {
                                item.TypeStructureChurchId = dr.GetInt64(dr.GetOrdinal("DistrictId"));
                            }

                            if (typeof(State).Name == structureType)
                            {
                                item.TypeStructureChurchId = dr.GetInt64(dr.GetOrdinal("StateId"));
                            }

                            if (typeof(Group).Name == structureType)
                            {
                                item.TypeStructureChurchId = dr.GetInt64(dr.GetOrdinal("GroupId"));
                            }

                            item.ChurchId = dr.GetInt64(dr.GetOrdinal("ChurchId"));
                            item.StateOfLocationId = dr.GetInt32(dr.GetOrdinal("StateOfLocationId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<StructureChurchRegObj>();
            }
        }
        

        public static List<StructureChurch> GetStructureChurchListObjByChurchStateId(long churchId, int stateId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<StructureChurch>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"StructureChurch\" A " +
                                            " LEFT JOIN \"ICASDB\".\"ChurchStructureType\" B " +
                                            " ON A.\"ChurchStructureTypeId\" = B.\"ChurchStructureTypeId\" " +
                                            " WHERE \"ChurchId\" = {0} AND \"StateOfLocationId\" = {1}", churchId, stateId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<StructureChurch>();
                    while (dr.Read())
                    {
                        var item = new StructureChurch();
                        {
                            item.StructureChurchId = dr.GetInt64(dr.GetOrdinal("StructureChurchId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<StructureChurch>();
            }
        }

        //GetStructureChurchById

        public static List<ChurchStructureType> GetChurchStructureTypeByStructureChurchId(long id)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<ChurchStructureType>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"StructureChurch\" A " +
                                            " LEFT JOIN \"ICASDB\".\"ChurchStructureType\" B " +
                                            " ON A.\"ChurchStructureTypeId\" = B.\"ChurchStructureTypeId\" " +
                                            " WHERE \"StructureChurchId\" = {0}", id);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<ChurchStructureType>();
                    while (dr.Read())
                    {
                        var item = new ChurchStructureType();
                        {
                            item.ChurchStructureTypeId = dr.GetInt32(dr.GetOrdinal("ChurchStructureTypeId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchStructureType>();
            }
        }
        
        #endregion


        public static string ReCleanMobile(string mobileNumber)
        {
            //2347023567478
            try
            {
                if (string.IsNullOrEmpty(mobileNumber))
                    return string.Empty;
                if (mobileNumber.StartsWith("234"))
                {
                    //return mobileNumber;
                    mobileNumber = mobileNumber.TrimStart(new char[] { '2', '3', '4' });
                    return string.Format("0{0}", mobileNumber);
                }
                return mobileNumber;
            }
            catch (Exception)
            {
                return mobileNumber;
            }
        }


        public static List<Parish> GetParishByChurchStateId(int churchId, int stateId)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ICASDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return new List<Parish>();
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat("SELECT * " +
                                            " FROM \"ICASDB\".\"Parish\" " +
                                            "WHERE \"ChurchId\" = {0} AND \"StateOfLocationId\" = {1} ", churchId, stateId);
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    var items = new List<Parish>();
                    while (dr.Read())
                    {
                        var item = new Parish();
                        {
                            item.ParishId = dr.GetInt32(dr.GetOrdinal("ParishId"));
                            item.Name = dr.GetString(dr.GetOrdinal("Name"));
                        };
                        items.Add(item);
                    }
                    sqlConnection.Close();
                    return items;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<Parish>();
            }
        }

        

        #region HEADQUARTERS

        

        #endregion

        #endregion



        

    }
}