using IDS.GeneralTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GeneralTable
{
    public sealed class Syspar
    {
        private static Syspar _instance = null;

        public int Code { get; set; }
        public string Name { get; set; }
        public string BaseCCy { get; set; }
        public string Department { get; set; }
        public string Version { get; set; }
        public decimal VAT { get; set; }
        //public string SignBy1 { get; set; }
        //public string Occupation1 { get; set; }
        //public string SignBy2 { get; set; }
        //public string Occupation2 { get; set; }

        #region Contact
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string CountryCode { get; set; }        
        public string Phone { get; set; }
        public string Fax { get; set; }
        #endregion

        #region Leeasing
        //public string OJK_SLIK_ID { get; set; }        
        //public int FundSource { get; set; }
        //public string FundSourceNo { get; set; }
        //public bool RetailContractAMApprovalNeeds { get; set; }
        #endregion

        #region GL Setting
        public DateTime? StartFiscalYear { get; set; }
        #endregion

        #region Print Report Setting
        public bool PrintName { get; set; }
        public bool PrintAddress { get; set; }
        public bool PrintCity { get; set; }
        public bool PrintCountry { get; set; }
        public bool PrintDate { get; set; }
        public bool PrintTime { get; set; }
        public bool PrintPageNumber { get; set; }
        public string Language { get; set; }
        #endregion
        
        private Syspar()
        {
            LoadData();
        }

        public static Syspar GetInstance()
        {
            if (_instance == null)
                _instance = new Syspar();
            return _instance;
        }

        private void LoadData()
        {
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelSyspar";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        Code = Convert.ToInt32(dr["Code"]);
                        Name = IDS.Tool.GeneralHelper.NullToString(dr["Name"]);
                        BaseCCy = IDS.Tool.GeneralHelper.NullToString(dr["BaseCcy"]);
                        Version = IDS.Tool.GeneralHelper.NullToString(dr["version"]);

                        Address1 = IDS.Tool.GeneralHelper.NullToString(dr["Address-1"]);
                        Address2 = IDS.Tool.GeneralHelper.NullToString(dr["Address-2"]);
                        Address3 = IDS.Tool.GeneralHelper.NullToString(dr["Address-3"]);
                        CountryCode = IDS.Tool.GeneralHelper.NullToString(dr["CountryCode"]);
                        Phone = IDS.Tool.GeneralHelper.NullToString(dr["telp"]);
                        Fax = IDS.Tool.GeneralHelper.NullToString(dr["fax"]);
                        VAT = IDS.Tool.GeneralHelper.NullToDecimal(dr["VAT"], 0);
                        //OJK_SLIK_ID = IDS.Tool.GeneralHelper.NullToString(dr["OJK_SLIK_ID"]);

                        // GL
                        StartFiscalYear = dr["StartFiscalYear"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartFiscalYear"]);

                        // Print Setting
                        PrintName = IDS.Tool.GeneralHelper.NullToBool(dr["chknama"], false);
                        PrintAddress = IDS.Tool.GeneralHelper.NullToBool(dr["chkaddress"], false);
                        PrintCity = IDS.Tool.GeneralHelper.NullToBool(dr["chkcity"], false);
                        PrintCountry = IDS.Tool.GeneralHelper.NullToBool(dr["chkcountry"], false);
                        PrintDate = IDS.Tool.GeneralHelper.NullToBool(dr["chkPrintDate"], false);
                        PrintTime = IDS.Tool.GeneralHelper.NullToBool(dr["chkPrintTime"], false);
                        PrintPageNumber = IDS.Tool.GeneralHelper.NullToBool(dr["chkPage"], false);
                        Language = IDS.Tool.GeneralHelper.NullToString(dr["optIndex"]);
                        VAT = IDS.Tool.GeneralHelper.NullToDecimal(dr["VAT"],0);
                        //SignBy1 = IDS.Tool.GeneralHelper.NullToString(dr["SignBy1"]);
                        //Occupation1 = IDS.Tool.GeneralHelper.NullToString(dr["Occupation1"]);
                        //SignBy2 = IDS.Tool.GeneralHelper.NullToString(dr["SignBy2"]);
                        //Occupation2 = IDS.Tool.GeneralHelper.NullToString(dr["Occupation2"]);
                        // Leasing
                        //FundSource = IDS.Tool.GeneralHelper.NullToInt(dr["FundSource"], 0);
                        //FundSourceNo = IDS.Tool.GeneralHelper.NullToString(dr["FundSourceNo"]);
                        // TODO: Check apakah kepake atau tidak. Jika tidak hapus dan di DB hapus juga
                        //RetailContractAMApprovalNeeds = IDS.Tool.GeneralHelper.NullToBool(dr["RetailContractAMApprovalNeeds"]);
                    }
                }
            }
        }

        public void RefreshData()
        {
            this.LoadData();
        }

        //public static List<System.Web.Mvc.SelectListItem> GetSignByForDataSource()
        //{
        //    List<System.Web.Mvc.SelectListItem> signBys = new List<System.Web.Mvc.SelectListItem>();

        //    using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
        //    {
        //        db.CommandText = "GTSelSyspar";
        //        db.CommandType = System.Data.CommandType.StoredProcedure;
        //        db.Open();

        //        db.ExecuteReader();

        //        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
        //        {
        //            if (dr.HasRows)
        //            {
        //                signBys = new List<System.Web.Mvc.SelectListItem>();
        //                string value = "";
        //                while (dr.Read())
        //                {

        //                    //value = IDS.Tool.GeneralHelper.NullToString(dr["SignBy1"]) + " - " + IDS.Tool.GeneralHelper.NullToString(dr["Occupation1"] + ",") + IDS.Tool.GeneralHelper.NullToString(dr["SignBy2"]) + " - " + IDS.Tool.GeneralHelper.NullToString(dr["Occupation2"] + ",");
        //                    value = IDS.Tool.GeneralHelper.NullToString(dr["SignBy1"]) + "," + IDS.Tool.GeneralHelper.NullToString(dr["SignBy2"]) + ",";

        //                    string[] valuesCode = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        //                    for (int i = 0; i < valuesCode.Length; i++)
        //                    {
        //                        System.Web.Mvc.SelectListItem signBy = new System.Web.Mvc.SelectListItem();
        //                        signBy.Value = valuesCode[i];
        //                        signBy.Text = valuesCode[i];

        //                        signBys.Add(signBy);
        //                    }


        //                }
        //            }

        //            if (!dr.IsClosed)
        //                dr.Close();
        //        }

        //        db.Close();
        //    }

        //    return signBys;
        //}

        //public static List<System.Web.Mvc.SelectListItem> GetSignByForDataSource()
        //{
        //    List<System.Web.Mvc.SelectListItem> signBys = new List<System.Web.Mvc.SelectListItem>();

        //    using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
        //    {
        //        db.CommandText = "GTSelBranch";
        //        db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
        //        db.CommandType = System.Data.CommandType.StoredProcedure;
        //        db.Open();

        //        db.ExecuteReader();

        //        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
        //        {
        //            if (dr.HasRows)
        //            {
        //                signBys = new List<System.Web.Mvc.SelectListItem>();
        //                string value = "";
        //                while (dr.Read())
        //                {

        //                    value = IDS.Tool.GeneralHelper.NullToString(dr["InvSignBy"]) + " - " + IDS.Tool.GeneralHelper.NullToString(dr["InvOccupation"] + ",") + IDS.Tool.GeneralHelper.NullToString(dr["TaxSignBy"]) + " - " + IDS.Tool.GeneralHelper.NullToString(dr["TaxOccupation"] + ",");
        //                    //value = IDS.Tool.GeneralHelper.NullToString(dr["SignBy1"]) + "," + IDS.Tool.GeneralHelper.NullToString(dr["SignBy2"]) + ",";

        //                    string[] valuesCode = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        //                    for (int i = 0; i < valuesCode.Length; i++)
        //                    {
        //                        System.Web.Mvc.SelectListItem signBy = new System.Web.Mvc.SelectListItem();
        //                        signBy.Value = valuesCode[i];
        //                        signBy.Text = valuesCode[i];

        //                        signBys.Add(signBy);
        //                    }


        //                }
        //            }

        //            if (!dr.IsClosed)
        //                dr.Close();
        //        }

        //        db.Close();
        //    }

        //    return signBys;
        //}

        public static List<System.Web.Mvc.SelectListItem> GetOccupationForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> signBys = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelSyspar";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        signBys = new List<System.Web.Mvc.SelectListItem>();
                        string value = "";
                        while (dr.Read())
                        {

                            //value = IDS.Tool.GeneralHelper.NullToString(dr["SignBy1"]) + " - " + IDS.Tool.GeneralHelper.NullToString(dr["Occupation1"] + ",") + IDS.Tool.GeneralHelper.NullToString(dr["SignBy2"]) + " - " + IDS.Tool.GeneralHelper.NullToString(dr["Occupation2"] + ",");
                            value = IDS.Tool.GeneralHelper.NullToString(dr["Occupation1"]) + "," + IDS.Tool.GeneralHelper.NullToString(dr["Occupation2"]) + ",";

                            string[] valuesCode = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            for (int i = 0; i < valuesCode.Length; i++)
                            {
                                System.Web.Mvc.SelectListItem signBy = new System.Web.Mvc.SelectListItem();
                                signBy.Value = valuesCode[i];
                                signBy.Text = valuesCode[i];

                                signBys.Add(signBy);
                            }


                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return signBys;
        }

        public int UpdateSyspar()
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SYSPARUpdate";
                    cmd.AddParameter("@ver", System.Data.SqlDbType.VarChar, Version);
                    cmd.AddParameter("@Name", System.Data.SqlDbType.VarChar, Name);
                    cmd.AddParameter("@address1", System.Data.SqlDbType.VarChar, Address1);
                    cmd.AddParameter("@address2", System.Data.SqlDbType.VarChar, Address2);
                    cmd.AddParameter("@address3", System.Data.SqlDbType.VarChar, Address3);
                    //cmd.AddParameter("@info", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(InfoListDir));
                    cmd.AddParameter("@countrycode", System.Data.SqlDbType.VarChar, CountryCode);
                    cmd.AddParameter("@baseccy", System.Data.SqlDbType.VarChar, BaseCCy);
                    cmd.AddParameter("@dept", System.Data.SqlDbType.VarChar, Department);
                    cmd.AddParameter("@startfiscal", System.Data.SqlDbType.DateTime, StartFiscalYear);
                    cmd.AddParameter("@telp", System.Data.SqlDbType.VarChar, Phone);
                    cmd.AddParameter("@fax", System.Data.SqlDbType.VarChar, Fax);
                    //cmd.AddParameter("@mobile", System.Data.SqlDbType.VarChar, Mobile);
                    cmd.AddParameter("@chknama", System.Data.SqlDbType.VarChar, PrintName == true ? "True" : "false");
                    cmd.AddParameter("@chkaddress", System.Data.SqlDbType.VarChar, PrintAddress == true ? "True" : "false");
                    cmd.AddParameter("@chkcity", System.Data.SqlDbType.VarChar, PrintCity == true ? "True" : "false");
                    cmd.AddParameter("@chkcountry", System.Data.SqlDbType.VarChar, PrintCountry == true ? "True" : "false");
                    cmd.AddParameter("@chkprintdate", System.Data.SqlDbType.VarChar, PrintDate == true ? "True" : "false");
                    cmd.AddParameter("@chkprinttime", System.Data.SqlDbType.VarChar, PrintTime == true ? "True" : "false");
                    cmd.AddParameter("@chkpage", System.Data.SqlDbType.VarChar, PrintPageNumber == true ? "true" : "false");
                    cmd.AddParameter("@optindex", System.Data.SqlDbType.VarChar, Language);
                    cmd.AddParameter("@VAT", System.Data.SqlDbType.VarChar, VAT);
                    //cmd.AddParameter("@SignBy1", System.Data.SqlDbType.VarChar, SignBy1);
                    //cmd.AddParameter("@Occupation1", System.Data.SqlDbType.VarChar, Occupation1);
                    //cmd.AddParameter("@SignBy2", System.Data.SqlDbType.VarChar, SignBy2);
                    //cmd.AddParameter("@Occupation2", System.Data.SqlDbType.VarChar, Occupation2);
                    //cmd.AddParameter("@FundSource", System.Data.SqlDbType.VarChar, FundSource);
                    //cmd.AddParameter("@FundSourceId", System.Data.SqlDbType.VarChar, FundSourceNo);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();

                    Syspar.GetInstance().RefreshData();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Syspar id is already exists. Please choose other Tax id.");
                        default:
                            throw;
                    }
                }
                catch
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    throw;
                }
                finally
                {
                    cmd.Close();
                }
            }

            return result;
        }
    }
}