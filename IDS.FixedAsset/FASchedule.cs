using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.FixedAsset
{
    public class FASchedule
    {
        public string AssetNo { get; set; }
        public string Period { get; set; }
        public string BranchCode { get; set; }
        public decimal BegVal { get; set; }
        public decimal Increment { get; set; }
        public decimal Decrement { get; set; }
        public decimal Depreciation { get; set; }
        public decimal AccumDepre { get; set; }
        public decimal EndVal { get; set; }
        public bool IsJournal { get; set; }
        public string Voucher { get; set; }
        public string OperatorID { get; set; }
        public DateTime LastUpdate { get; set; }

        public FASchedule()
        {

        }

        public static List<FASchedule> GetFASchedule(string assetNo, string branchCode, string operatorID, int status)
        {
            List<FASchedule> items = new List<FASchedule>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "FAProcDepSchedule";
                db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, assetNo);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, operatorID);
                db.AddParameter("@Status", System.Data.SqlDbType.Int, 1);

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            FASchedule item = new FASchedule();
                            item.AssetNo = assetNo;
                            item.Period = Tool.GeneralHelper.NullToString(dr["Period"]);
                            item.BranchCode = branchCode;
                            item.BegVal = Tool.GeneralHelper.NullToDecimal(dr["Beginning Value"], 0);
                            item.Increment = Tool.GeneralHelper.NullToDecimal(dr["Increment"], 0);
                            item.Decrement = Tool.GeneralHelper.NullToDecimal(dr["Decrement"], 0);
                            item.Depreciation = Tool.GeneralHelper.NullToDecimal(dr["Depreciation"], 0);
                            item.AccumDepre = Tool.GeneralHelper.NullToDecimal(dr["Accumulation"], 0);
                            item.EndVal = Tool.GeneralHelper.NullToDecimal(dr["Ending Value"], 0);
                            item.IsJournal = Tool.GeneralHelper.NullToBool(dr["Journal"], false);
                            item.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);
                            item.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            item.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now.Date);

                            items.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }
            }

            return items;
        }

        public static bool IsJournalExists(string AssetNo, string branchCode)
        {
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "IF EXISTS(SELECT AssetNo FROM FASchedule WHERE AssetNo = @assetNo AND BranchCode = @branchCode AND ISNULL(Journal, 0) = 1) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
                    db.AddParameter("@assetNo", System.Data.SqlDbType.VarChar, AssetNo);
                    db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
                    db.CommandType = System.Data.CommandType.Text;
                    db.Open();

                    bool result = Convert.ToBoolean(db.ExecuteScalar());

                    return result;
                }
                catch (Exception ex)
                {
                    return true;
                }
                finally
                {
                    db.Close();
                }
            }
        }

        public static List<System.Web.Mvc.SelectListItem> GetYearFromFASchedule()
        {
            List<System.Web.Mvc.SelectListItem> branches = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "select distinct left(period,4) as Year from FASchedule order by left(period,4)";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem branch = new System.Web.Mvc.SelectListItem(); //<string, string>(dr["BranchCode"].ToString(), dr["BranchName"].ToString());
                            branch.Text = dr["Year"].ToString();
                            branch.Value = dr["Year"].ToString();
                            branches.Add(branch);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            branches = branches.OrderBy(x => x.Text).ToList();

            return branches;
        }

        public static bool IssNoEmpty(string cboXDept, string cboBranch, string cboMontYear, int init_)
        {
            string fullDate = "";
            if (string.IsNullOrEmpty(cboMontYear))
                fullDate = System.DateTime.Now.ToString("yyyyMM");
            else
            {
                //DateTime datePeriod = System.Convert.ToDateTime(cboMontYear);
                fullDate = cboMontYear;
            }

            bool b = false;
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "FARepFAMaster";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Locate", System.Data.SqlDbType.VarChar, "NULL");
                db.AddParameter("@Dept", System.Data.SqlDbType.VarChar, cboXDept);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, init_);
                db.AddParameter("@Expense", System.Data.SqlDbType.TinyInt, 0);
                db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, cboBranch);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, fullDate);
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        b = true;
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return b;
        }


        public static List<System.Web.Mvc.SelectListItem> LoadLocation()
        {
            List<System.Web.Mvc.SelectListItem> loc_ = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "select distinct locationcode as code,locationname as descr from tbllocation where locationcode is not null";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem l = new System.Web.Mvc.SelectListItem(); //<string, string>(dr["BranchCode"].ToString(), dr["BranchName"].ToString());
                            l.Text = dr["descr"].ToString();
                            l.Value = dr["code"].ToString();
                            loc_.Add(l);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            loc_ = loc_.OrderBy(x => x.Text).ToList();

            return loc_;
        }

        public static List<FASchedule> GetFASchedule_Stat(string assetNo, string branchCode, string operatorID, int status)
        {
            List<FASchedule> items = new List<FASchedule>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "FAProcDepSchedule";
                db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, assetNo);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, operatorID);
                db.AddParameter("@Status", System.Data.SqlDbType.Int, status);

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            FASchedule item = new FASchedule();
                            item.AssetNo = assetNo;
                            item.Period = Tool.GeneralHelper.NullToString(dr["Period"]);
                            item.BranchCode = branchCode;
                            item.BegVal = Tool.GeneralHelper.NullToDecimal(dr["Beginning Value"], 0);
                            item.Increment = Tool.GeneralHelper.NullToDecimal(dr["Increment"], 0);
                            item.Decrement = Tool.GeneralHelper.NullToDecimal(dr["Decrement"], 0);
                            item.Depreciation = Tool.GeneralHelper.NullToDecimal(dr["Depreciation"], 0);
                            item.AccumDepre = Tool.GeneralHelper.NullToDecimal(dr["Accumulation"], 0);
                            item.EndVal = Tool.GeneralHelper.NullToDecimal(dr["Ending Value"], 0);
                            item.IsJournal = Tool.GeneralHelper.NullToBool(dr["Journal"], false);
                            item.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);
                            item.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            item.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now.Date);

                            items.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }
            }

            return items;
        }

        public static bool IsAlreadyJournal(string AssetNo, string BranchCode)
        {
            bool l = false;
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SELECT * FROM FASchedule FAS INNER JOIN FAAsset FAA ON FAA.AssetNo = FAS.AssetNo AND FAA.BranchCode = FAS.BranchCode WHERE FAS.AssetNo = @AssetNo AND FAS.BranchCode = @BranchCode AND (Journal = 1 OR Status <> 0)";
                db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, AssetNo);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchCode);
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        l = true;
                    }
                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }
                db.Close();
            }
            return l;
        }

        public static bool IsAlreadyJournalTax(string AssetNo, string BranchCode)
        {
            bool l = false;
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SELECT * FROM FAScheduleTax WHERE AssetTaxNo = @AssetTaxNo AND AssetTaxJournal = 1";
                db.AddParameter("@AssetTaxNo", System.Data.SqlDbType.VarChar, AssetNo);
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        l = true;
                    }
                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }
                db.Close();
            }
            return l;
        }

        public static decimal GetBookValue(string period, string assetNo)
        {
            decimal BookValue = 0;
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SELECT EndVal FROM FASchedule WHERE AssetNo = @AssetNo AND BranchCode = @BranchCode AND Period = @Period";
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, assetNo);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, "DTN");
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BookValue = IDS.Tool.GeneralHelper.NullToDecimal(dr["EndVal"],0);
                        }
                    }
                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }
                db.Close();
            }
            return BookValue;
        }


        public static List<FASchedule> GetFASchedule_TAX(string assetNo, string branchCode, string operatorID, int status)
        {
            List<FASchedule> items = new List<FASchedule>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "FAProcDepScheduleTax";
                db.AddParameter("@AssetTaxNo", System.Data.SqlDbType.VarChar, assetNo);
                db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, operatorID);
                db.AddParameter("@AssetTaxBranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@LastUpdate", System.Data.SqlDbType.DateTime, System.DateTime.Now);
                db.AddParameter("@Status", System.Data.SqlDbType.Int, status);
                //  exec FAProcDepScheduleTax @AssetTaxNo='BL1/001/DTN/2000',@OperatorID='admin',@AssetTaxBranchCode='DTN',@LastUpdate='',@Status=0
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            FASchedule item = new FASchedule();
                            item.AssetNo = assetNo;
                            item.Period = Tool.GeneralHelper.NullToString(dr["AssetTaxPeriod"]);
                            item.BranchCode = branchCode;
                            item.BegVal = Tool.GeneralHelper.NullToDecimal(dr["Beginning Value"], 0);
                            item.Increment = Tool.GeneralHelper.NullToDecimal(dr["AssetTaxIncrement"], 0);
                            item.Decrement = Tool.GeneralHelper.NullToDecimal(dr["AssetTaxDecrement"], 0);
                            item.Depreciation = Tool.GeneralHelper.NullToDecimal(dr["AssetTaxDepreciation"], 0);
                            item.AccumDepre = Tool.GeneralHelper.NullToDecimal(dr["Accumulation"], 0);
                            item.EndVal = Tool.GeneralHelper.NullToDecimal(dr["Ending Value"], 0);
                            item.IsJournal = Tool.GeneralHelper.NullToBool(dr["AssetTaxJournal"], false);
                            item.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            item.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now.Date);
                            items.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }

            }
            return items;
        }

    }
}