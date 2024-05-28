using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class MonthlyBalance
    {
        [Display(Name = "Account")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Account No is required")]
        [MaxLength(10), StringLength(10)]
        public IDS.GLTable.ChartOfAccount COAMonthlyBalance { get; set; }

        [Display(Name = "Currency Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        //[MaxLength(3)]
        public IDS.GeneralTable.Currency CurrencyMonthlyBalance { get; set; }

        [Display(Name = "Period")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Period is required")]
        [MaxLength(8), StringLength(8)]
        public string Period { get; set; }

        [Display(Name = "Branch")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch is required")]
        public IDS.GeneralTable.Branch BranchMonthlyBalance { get; set; }

        [Display(Name = "Beginning Balance")]
        //[Range(0, 255)]
        public decimal BegBal { get; set; }

        [Display(Name = "Debit")]
        //[Range(0, 255)]
        public decimal Debit { get; set; }

        [Display(Name = "Credit")]
        //[Range(0, 255)]
        public decimal Credit { get; set; }

        [Display(Name = "Budget")]
        //[Range(0, 255)]
        public decimal Budget { get; set; }

        [Display(Name = "Ending Balance")]
        //[Range(0, 255)]
        public decimal EndBal { get; set; }

        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string CurrencyName { get; set; }
        public string ParentDummy { get; set; }
        public string ParentID { get; set; }

        public bool TotDet { get; set; }
        public int Level { get; set; }
        public bool TotDetBefore { get; set; }
        public int LevelBefore { get; set; }

        public MonthlyBalance()
        {

        }

        public MonthlyBalance(string period, IDS.GLTable.ChartOfAccount coaMonthlyBalance, IDS.GeneralTable.Branch branch)
        {
            COAMonthlyBalance = coaMonthlyBalance;
            Period = period;
            BranchMonthlyBalance = branch;
        }

        public static MonthlyBalance GetMonthlyBalance(string period, string branchCode, string coa, string currency)
        {
            MonthlyBalance monthlyBalance = new MonthlyBalance();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelMonthlyBalance";
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, coa);
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, currency);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        monthlyBalance.COAMonthlyBalance = new IDS.GLTable.ChartOfAccount();
                        monthlyBalance.COAMonthlyBalance.Account = dr["acc"] as string;
                        monthlyBalance.COAMonthlyBalance.AccountName = dr["acc_name"] as string;

                        monthlyBalance.CurrencyMonthlyBalance = new IDS.GeneralTable.Currency();
                        monthlyBalance.CurrencyMonthlyBalance.CurrencyCode = dr["ccy"] as string;

                        monthlyBalance.Period = dr["mn"] as string;
                        monthlyBalance.BegBal = Convert.ToDecimal(dr["begbal"]);
                        monthlyBalance.Debit = Convert.ToDecimal(dr["debit"]);
                        monthlyBalance.Credit = Convert.ToDecimal(dr["credit"]);
                        monthlyBalance.Budget = Convert.ToDecimal(dr["budget"]);
                        monthlyBalance.EndBal = Convert.ToDecimal(dr["ending"]);

                        monthlyBalance.BranchMonthlyBalance = new IDS.GeneralTable.Branch();
                        monthlyBalance.BranchMonthlyBalance.BranchCode = dr["branchcode"] as string;
                        monthlyBalance.BranchMonthlyBalance.BranchName = dr["branchname"] as string;
                        monthlyBalance.BranchMonthlyBalance.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                        monthlyBalance.BranchMonthlyBalance.NPWP = dr["NPWP"] as string;

                        monthlyBalance.BranchMonthlyBalance.BranchManagerName = dr["BranchManager"] as string;
                        monthlyBalance.BranchMonthlyBalance.FinAccOfficer = dr["FinAccOfficer"] as string;

                        //monthlyBalance.BranchMonthlyBalance.Address1 = dr["Addr1"] as string;
                        //monthlyBalance.BranchMonthlyBalance.Address2 = dr["Addr2"] as string;
                        //monthlyBalance.BranchMonthlyBalance.Address3 = dr["Addr3"] as string;
                        ////monthlyBalance.BranchMonthlyBalance.Country = Country.GetCountry(Tool.GeneralHelper.NullToString(dr["CountryCode"]));
                        ////monthlyBalance.BranchMonthlyBalance.City = City.GetCity(Tool.GeneralHelper.NullToString(dr["CityCode"]), Tool.GeneralHelper.NullToString(dr["CountryCode"]));

                        //monthlyBalance.BranchMonthlyBalance.PostalCode = dr["PostalCode"] as string;
                        //monthlyBalance.BranchMonthlyBalance.Phone1 = dr["Phone1"] as string;
                        //monthlyBalance.BranchMonthlyBalance.Phone2 = dr["Phone2"] as string;
                        //monthlyBalance.BranchMonthlyBalance.Phone3 = dr["Phone3"] as string;
                        //monthlyBalance.BranchMonthlyBalance.Fax = dr["Fax"] as string;

                        //monthlyBalance.BranchMonthlyBalance.FullAddress =
                        //        IDS.Tool.GeneralHelper.NullToString(dr["Addr1"]) +
                        //        IDS.Tool.GeneralHelper.NullToString(dr["Addr2"]) +
                        //        IDS.Tool.GeneralHelper.NullToString(dr["Addr3"]) +
                        //        IDS.Tool.GeneralHelper.NullToString(monthlyBalance.BranchMonthlyBalance.BranchCity?.CityName);

                        //// Print Option
                        //monthlyBalance.BranchMonthlyBalance.PrintBranchName = Convert.ToBoolean(dr["Chknama"]);
                        //monthlyBalance.BranchMonthlyBalance.PrintBranchAddress = Convert.ToBoolean(dr["Chkaddress"]);
                        //monthlyBalance.BranchMonthlyBalance.PrintBranchCity = Convert.ToBoolean(dr["Chkcity"]);
                        //monthlyBalance.BranchMonthlyBalance.PrintBranchCountry = Convert.ToBoolean(dr["Chkcountry"]);
                        //monthlyBalance.BranchMonthlyBalance.PrintDate = Convert.ToBoolean(dr["ChkPrintDate"]);
                        //monthlyBalance.BranchMonthlyBalance.PrintTime = Convert.ToBoolean(dr["ChkPrintTime"]);
                        //monthlyBalance.BranchMonthlyBalance.PrintPage = Convert.ToBoolean(dr["ChkPage"]);
                        //monthlyBalance.BranchMonthlyBalance.Language = Convert.ToBoolean(dr["OptIndex"]);

                        //// GL
                        ////monthlyBalance.BranchMonthlyBalance.RepNo = IDS.GL.ChartOfAccount.GetCOA(IDS.Tool.GeneralHelper.NullToString(dr["RepNo"]));
                        ////monthlyBalance.BranchMonthlyBalance.HopNo = IDS.GL.ChartOfAccount.GetCOA(IDS.Tool.GeneralHelper.NullToString(dr["HopNo"]));

                        //// Leasing
                        //monthlyBalance.BranchMonthlyBalance.ContractLimit = Convert.ToDecimal(dr["ContractLimit"]);
                        //monthlyBalance.BranchMonthlyBalance.NUPApprovalLimit = Convert.ToDecimal(dr["NUPApprovalLimit"]);
                        //monthlyBalance.BranchMonthlyBalance.NUPExpiredDays = Convert.ToInt32(dr["NUPExpiredDay"]);
                        //monthlyBalance.BranchMonthlyBalance.OverRate = Convert.ToDecimal(dr["OverRate"]);
                        //monthlyBalance.BranchMonthlyBalance.OverRateRetail = Convert.ToDecimal(dr["OverRateRetail"]);
                        //monthlyBalance.BranchMonthlyBalance.PenaltyRate = Convert.ToDecimal(dr["PenaltyRate"]);
                        //monthlyBalance.BranchMonthlyBalance.PenaltyRateRetail = Convert.ToDecimal(dr["PenaltyRateRetail"]);
                        ////monthlyBalance.BranchMonthlyBalance.LastCount = Convert.ToInt32(dr["LastCount"]);
                        //monthlyBalance.BranchMonthlyBalance.LastReceiptNo = Convert.ToInt32(dr["LastReceiptNo"]);

                        //// SLIK
                        //monthlyBalance.BranchMonthlyBalance.SLIKCode = dr["SLIKCode"] as string;
                        
                        monthlyBalance.OperatorID = dr["OperatorID"] as string;
                        monthlyBalance.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return monthlyBalance;
        }

        public static List<MonthlyBalance> GetMonthlyBalance()
        {
            List<IDS.GLTable.MonthlyBalance> list = new List<MonthlyBalance>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelMonthlyBalance";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, DBNull.Value);

                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            MonthlyBalance monthlyBalance = new MonthlyBalance();
                            monthlyBalance.COAMonthlyBalance = new IDS.GLTable.ChartOfAccount();
                            monthlyBalance.COAMonthlyBalance.Account = dr["acc"] as string;
                            monthlyBalance.COAMonthlyBalance.AccountName = Tool.GeneralHelper.NullToString(dr["acc_name"], "");

                            monthlyBalance.COAMonthlyBalance.CCy = new IDS.GeneralTable.Currency();
                            monthlyBalance.COAMonthlyBalance.CCy.CurrencyCode = dr["ccy"] as string;

                            monthlyBalance.Period = dr["mn"] as string;
                            monthlyBalance.BegBal = Convert.ToDecimal(dr["begbal"]);
                            monthlyBalance.Debit = Convert.ToDecimal(dr["debit"]);
                            monthlyBalance.Credit = Convert.ToDecimal(dr["credit"]);
                            monthlyBalance.Budget = Convert.ToDecimal(dr["budget"]);
                            monthlyBalance.EndBal = Convert.ToDecimal(dr["Ending"]);

                            monthlyBalance.BranchMonthlyBalance = new IDS.GeneralTable.Branch();
                            monthlyBalance.BranchMonthlyBalance.BranchCode = dr["branchcode"] as string;
                            monthlyBalance.BranchMonthlyBalance.BranchName = dr["branchname"] as string;
                            monthlyBalance.BranchMonthlyBalance.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            monthlyBalance.BranchMonthlyBalance.NPWP = dr["NPWP"] as string;

                            monthlyBalance.BranchMonthlyBalance.BranchManagerName = dr["BranchManager"] as string;
                            monthlyBalance.BranchMonthlyBalance.FinAccOfficer = dr["FinAccOfficer"] as string;

                            monthlyBalance.BranchMonthlyBalance.Address1 = dr["Addr1"] as string;
                            monthlyBalance.BranchMonthlyBalance.Address2 = dr["Addr2"] as string;
                            monthlyBalance.BranchMonthlyBalance.Address3 = dr["Addr3"] as string;
                            //monthlyBalance.BranchMonthlyBalance.Country = Country.GetCountry(Tool.GeneralHelper.NullToString(dr["CountryCode"]));
                            //monthlyBalance.BranchMonthlyBalance.City = City.GetCity(Tool.GeneralHelper.NullToString(dr["CityCode"]), Tool.GeneralHelper.NullToString(dr["CountryCode"]));

                            monthlyBalance.BranchMonthlyBalance.PostalCode = dr["PostalCode"] as string;
                            monthlyBalance.BranchMonthlyBalance.Phone1 = dr["Phone1"] as string;
                            monthlyBalance.BranchMonthlyBalance.Phone2 = dr["Phone2"] as string;
                            monthlyBalance.BranchMonthlyBalance.Phone3 = dr["Phone3"] as string;
                            monthlyBalance.BranchMonthlyBalance.Fax = dr["Fax"] as string;

                            //monthlyBalance.BranchMonthlyBalance.FullAddress =
                            //        IDS.Tool.GeneralHelper.NullToString(dr["Addr1"]) +
                            //        IDS.Tool.GeneralHelper.NullToString(dr["Addr2"]) +
                            //        IDS.Tool.GeneralHelper.NullToString(dr["Addr3"]) +
                            //        IDS.Tool.GeneralHelper.NullToString(monthlyBalance.BranchMonthlyBalance.BranchCity?.CityName);

                            // Print Option
                            monthlyBalance.BranchMonthlyBalance.PrintBranchName = Convert.ToBoolean(dr["Chknama"]);
                            monthlyBalance.BranchMonthlyBalance.PrintBranchAddress = Convert.ToBoolean(dr["Chkaddress"]);
                            monthlyBalance.BranchMonthlyBalance.PrintBranchCity = Convert.ToBoolean(dr["Chkcity"]);
                            monthlyBalance.BranchMonthlyBalance.PrintBranchCountry = Convert.ToBoolean(dr["Chkcountry"]);
                            monthlyBalance.BranchMonthlyBalance.PrintDate = Convert.ToBoolean(dr["ChkPrintDate"]);
                            monthlyBalance.BranchMonthlyBalance.PrintTime = Convert.ToBoolean(dr["ChkPrintTime"]);
                            monthlyBalance.BranchMonthlyBalance.PrintPage = Convert.ToBoolean(dr["ChkPage"]);
                            //monthlyBalance.BranchMonthlyBalance.Language = Convert.ToBoolean(dr["OptIndex"]);

                            // GL
                            //monthlyBalance.BranchMonthlyBalance.RepNo = IDS.GL.ChartOfAccount.GetCOA(IDS.Tool.GeneralHelper.NullToString(dr["RepNo"]));
                            //monthlyBalance.BranchMonthlyBalance.HopNo = IDS.GL.ChartOfAccount.GetCOA(IDS.Tool.GeneralHelper.NullToString(dr["HopNo"]));

                            // Leasing
                            //monthlyBalance.BranchMonthlyBalance.ContractLimit = Convert.ToDecimal(dr["ContractLimit"]);
                            //monthlyBalance.BranchMonthlyBalance.NUPApprovalLimit = Convert.ToDecimal(dr["NUPApprovalLimit"]);
                            //monthlyBalance.BranchMonthlyBalance.NUPExpiredDays = Convert.ToInt32(dr["NUPExpiredDay"]);
                            //monthlyBalance.BranchMonthlyBalance.OverRate = Convert.ToDecimal(dr["OverRate"]);
                            //monthlyBalance.BranchMonthlyBalance.OverRateRetail = Convert.ToDecimal(dr["OverRateRetail"]);
                            //monthlyBalance.BranchMonthlyBalance.PenaltyRate = Convert.ToDecimal(dr["PenaltyRate"]);
                            //monthlyBalance.BranchMonthlyBalance.PenaltyRateRetail = Convert.ToDecimal(dr["PenaltyRateRetail"]);
                            ////monthlyBalance.BranchMonthlyBalance.LastCount = Convert.ToInt32(dr["LastCount"]);
                            //monthlyBalance.BranchMonthlyBalance.LastReceiptNo = Convert.ToInt32(dr["LastReceiptNo"]);

                            //// SLIK
                            //monthlyBalance.BranchMonthlyBalance.SLIKCode = dr["SLIKCode"] as string;

                            monthlyBalance.EntryUser = dr["EntryUser"] as string;
                            monthlyBalance.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            monthlyBalance.OperatorID = dr["OperatorID"] as string;
                            monthlyBalance.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(monthlyBalance);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<MonthlyBalance> GetMonthlyBalance(string period, string branchCode)
        {
            List<IDS.GLTable.MonthlyBalance> list = new List<MonthlyBalance>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelMonthlyBalance";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);

                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            MonthlyBalance monthlyBalance = new MonthlyBalance();
                            monthlyBalance.COAMonthlyBalance = new IDS.GLTable.ChartOfAccount();
                            monthlyBalance.COAMonthlyBalance.Account = Tool.GeneralHelper.NullToString(dr["acc"],"");
                            monthlyBalance.COAMonthlyBalance.AccountName = Tool.GeneralHelper.NullToString(dr["acc_name"], "");
                            monthlyBalance.COAMonthlyBalance.Level = Tool.GeneralHelper.NullToInt(dr["TL"], 0);
                            monthlyBalance.COAMonthlyBalance.AccountTotalDetail = Tool.GeneralHelper.NullToBool(dr["AT"], false);

                            monthlyBalance.COAMonthlyBalance.CCy = new IDS.GeneralTable.Currency();
                            monthlyBalance.COAMonthlyBalance.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["ccy"], "");

                            monthlyBalance.Period = Tool.GeneralHelper.NullToString(dr["mn"], "");
                            monthlyBalance.BegBal = Tool.GeneralHelper.NullToDecimal(dr["begbal"],0);
                            monthlyBalance.Debit = Tool.GeneralHelper.NullToDecimal(dr["debit"], 0);
                            monthlyBalance.Credit = Tool.GeneralHelper.NullToDecimal(dr["credit"], 0);
                            monthlyBalance.Budget = Tool.GeneralHelper.NullToDecimal(dr["budget"], 0);
                            monthlyBalance.EndBal = Tool.GeneralHelper.NullToDecimal(dr["Ending"], 0);

                            monthlyBalance.BranchMonthlyBalance = new IDS.GeneralTable.Branch();
                            monthlyBalance.BranchMonthlyBalance.BranchCode = dr["branchcode"] as string;
                            monthlyBalance.BranchMonthlyBalance.BranchName = dr["branchname"] as string;

                            monthlyBalance.OperatorID = dr["OperatorID"] as string;
                            monthlyBalance.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(monthlyBalance);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetYearForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            int MinYear = 0;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "select distinct mn from (select left(mn,4) as mn from ACFGLMD union select cast(year(getdate()) as varchar(4))) as tbl order by mn desc";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {


                            MinYear = Convert.ToInt16(dr["mn"]);

                            if (MinYear == 0)
                            {
                                MinYear = DateTime.Now.Year;
                            }

                            //year.Value = MinYear;
                            //year.Text = MinYear;

                            //list.Add(year);
                        }
                        int iYear;

                        int NowYear = DateTime.Now.Year;
                        int MaxFutureYear = DateTime.Now.Year + 10;
                        for (iYear = MinYear; iYear <= MaxFutureYear; iYear++)
                        {
                            int iYearL = iYear;
                            System.Web.Mvc.SelectListItem year = new System.Web.Mvc.SelectListItem();
                            year.Value = iYearL.ToString();
                            year.Text = iYearL.ToString();
                            list.Add(year);
                        }

                    }
                }
                db.Close();
            }
            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetMonthForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> Month = new List<System.Web.Mvc.SelectListItem>();
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "January", Value = "01" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "February", Value = "02" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "March", Value = "03" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "April", Value = "04" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "May", Value = "05" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "Juny", Value = "06" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "July", Value = "07" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "August", Value = "08" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "September", Value = "09" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "October", Value = "10" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "November", Value = "11" });
            Month.Add(new System.Web.Mvc.SelectListItem() { Text = "Desember", Value = "12" });

            return Month;
        }

        public int InsUpDelMonthlyBalance(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLTUpdateACFGLMD";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, COAMonthlyBalance.Account);
                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CurrencyMonthlyBalance.CurrencyCode);
                    cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchMonthlyBalance.BranchCode);
                    cmd.AddParameter("@MN", System.Data.SqlDbType.VarChar, Period);
                    cmd.AddParameter("@begbal", System.Data.SqlDbType.Money, BegBal);
                    cmd.AddParameter("@budget", System.Data.SqlDbType.Money, Budget);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();
                }
                catch (SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Monthly Balance is already exists. Please choose other Monthly Balance.");
                        default:
                            throw;
                    }
                }
                catch (Exception ex)
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

        public int InsUpDelMonthlyBalance(int ExecCode, string[] data, string branch, string[] coa, string[] currency)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLTUpdateACFGLMD";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GLTUpdateACFGLMD";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@MN", System.Data.SqlDbType.VarChar, data[i]);
                        cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                        cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, coa[i]);
                        cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, currency[i]);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommitTransaction();
                }
                catch (SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Monthly Balance is already exists. Please choose other Monthly Balance.");
                        case 547:
                            throw new Exception("One or more data can not be delete while data used for reference.");
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
