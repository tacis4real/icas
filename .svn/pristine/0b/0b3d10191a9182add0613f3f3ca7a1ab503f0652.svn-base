using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using ICASStacks.APIObjs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.DataManager.Utils
{
    public class ExcelImport
    {
        public bool Import(string filePath, ref List<ChurchMemberRegistrationObj> mList, ref string msg)
        {
            if (filePath.Length < 3 || new FileInfo(filePath).Exists == false || (Path.GetExtension(filePath) != ".xls" && Path.GetExtension(filePath) != ".xlsx"))
            {
                msg = "Invalid Excel File Format";
                return false;
            }

            string connectionstring = string.Empty;
            switch (Path.GetExtension(filePath))
            {
                case ".xls":
                    connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES;'";
                    break;
                case ".xlsx":
                    connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;ImportMixedTypes=Text'";
                    break;
            }

            if (connectionstring == "")
            {
                msg = "Process Error! Please try again later";
                return false;
            }


            try
            {
                using (var myCon = new OleDbConnection(connectionstring))
                {
                    if (myCon.State == ConnectionState.Closed)
                    {
                        myCon.Open();
                    }

                    using (var dtExcelSchema = myCon.GetSchema("Tables"))
                    {
                        var sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        if (sheetName.Length < 1)
                        {
                            msg = "Invalid Excel Sheet Name";
                            return false;
                        }

                        var selectString = @"SELECT [FullName], [Sex], [DateJoined], [Profession], [RoleInChurch], [Address], [MobileNumber], [Email] FROM [" + sheetName + "] WHERE [FullName] <> ''";

                        using (var cmd = new OleDbCommand(selectString, myCon))
                        {
                            using (var adap = new OleDbDataAdapter(cmd))
                            {
                                var ds = new DataSet();
                                adap.Fill(ds);
                                if (ds.Tables.Count < 1)
                                {
                                    msg = "Invalid Data Template!";
                                    return false;
                                }
                                DataView dv;
                                using (dv = new DataView(ds.Tables[0]))
                                {
                                    if (dv.Count < 1)
                                    {
                                        msg = "Invalid Data Template! Make sure the whole column are filled with valid data";
                                        return false;
                                    }

                                    for (int i = 0; i < dv.Count; i++)
                                    {
                                        mList.Add(new ChurchMemberRegistrationObj
                                        {
                                            UploadStatus = i + 1,
                                            FullName = (dv[i]["FullName"] == DBNull.Value || string.IsNullOrEmpty(dv[i]["FullName"].ToString())) ? "" : dv[i]["FullName"].ToString(),
                                            Sex = (dv[i]["Sex"] == DBNull.Value || string.IsNullOrEmpty(dv[i]["Sex"].ToString()) || !DataCheck.IsNumeric(dv[i]["Sex"].ToString())) ? 0 : int.Parse(dv[i]["Sex"].ToString()),
                                            DateJoined = (dv[i]["DateJoined"] == DBNull.Value || string.IsNullOrEmpty(dv[i]["DateJoined"].ToString())) ? "" : dv[i]["DateJoined"].ToString(),
                                            Profession = (dv[i]["Profession"] == DBNull.Value || string.IsNullOrEmpty(dv[i]["Profession"].ToString())) ? "" : dv[i]["Profession"].ToString(),
                                            RoleInChurch = (dv[i]["RoleInChurch"] == DBNull.Value || string.IsNullOrEmpty(dv[i]["RoleInChurch"].ToString())) ? "" : dv[i]["RoleInChurch"].ToString(),
                                            Address = (dv[i]["Address"] == DBNull.Value || string.IsNullOrEmpty(dv[i]["Address"].ToString())) ? "" : dv[i]["Address"].ToString(),
                                            PhoneNumber = (dv[i]["MobileNumber"] == DBNull.Value || string.IsNullOrEmpty(dv[i]["MobileNumber"].ToString())) ? "" : dv[i]["MobileNumber"].ToString(),
                                            Email = (dv[i]["Email"] == DBNull.Value || string.IsNullOrEmpty(dv[i]["Email"].ToString())) ? "" : dv[i]["Email"].ToString(),

                                        });

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return false;
            }

            return true;
        }
    }
}
