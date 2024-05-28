using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class MonthlyBalanceBudget
    {
        [Display(Name = "Account")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Account No is required")]
        [MaxLength(10), StringLength(10)]
        public IDS.GLTable.ChartOfAccount COAMonthlyBalanceBudget { get; set; }

        [Display(Name = "Currency Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        //[MaxLength(3)]
        public IDS.GeneralTable.Currency CurrencyMonthlyBalanceBudget { get; set; }

        [Display(Name = "Period")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Period is required")]
        [MaxLength(8), StringLength(8)]
        public string Period { get; set; }

        [Display(Name = "Branch")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch is required")]
        public IDS.GeneralTable.Branch BranchMonthlyBalanceBudget { get; set; }

        [Display(Name = "Beginning Balance")]
        //[Range(0, 255)]
        public decimal BegBal { get; set; }

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

        public MonthlyBalanceBudget()
        {

        }

        public MonthlyBalanceBudget(string period, IDS.GLTable.ChartOfAccount coaMonthlyBalanceBudget, IDS.GeneralTable.Branch branch)
        {
            COAMonthlyBalanceBudget = coaMonthlyBalanceBudget;
            Period = period;
            BranchMonthlyBalanceBudget = branch;
        }

        public static MonthlyBalanceBudget GetMonthlyBalanceBudget(string period, string branchCode, string coa, string currency)
        {
            MonthlyBalanceBudget monthlyBalanceBudget = new MonthlyBalanceBudget();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelMonthlyBalanceBudget";
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
                        monthlyBalanceBudget.COAMonthlyBalanceBudget = new IDS.GLTable.ChartOfAccount();
                        monthlyBalanceBudget.COAMonthlyBalanceBudget.Account = dr["acc"] as string;
                        monthlyBalanceBudget.COAMonthlyBalanceBudget.AccountName = dr["acc_name"] as string;

                        monthlyBalanceBudget.CurrencyMonthlyBalanceBudget = new IDS.GeneralTable.Currency();
                        monthlyBalanceBudget.CurrencyMonthlyBalanceBudget.CurrencyCode = dr["ccy"] as string;

                        monthlyBalanceBudget.Period = dr["mn"] as string;
                        monthlyBalanceBudget.BegBal = Convert.ToDecimal(dr["begbal"]);
                        monthlyBalanceBudget.Budget = Convert.ToDecimal(dr["budget"]);

                        monthlyBalanceBudget.BranchMonthlyBalanceBudget = new IDS.GeneralTable.Branch();
                        monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchCode = dr["branchcode"] as string;
                        monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchName = dr["branchname"] as string;
                        monthlyBalanceBudget.BranchMonthlyBalanceBudget.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                        monthlyBalanceBudget.BranchMonthlyBalanceBudget.NPWP = dr["NPWP"] as string;

                        monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchManagerName = dr["BranchManager"] as string;
                        monthlyBalanceBudget.BranchMonthlyBalanceBudget.FinAccOfficer = dr["FinAccOfficer"] as string;
                        monthlyBalanceBudget.OperatorID = dr["OperatorID"] as string;
                        monthlyBalanceBudget.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return monthlyBalanceBudget;
        }

        public static List<MonthlyBalanceBudget> GetMonthlyBalanceBudget()
        {
            List<IDS.GLTable.MonthlyBalanceBudget> list = new List<MonthlyBalanceBudget>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelMonthlyBalanceBudget";
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
                            MonthlyBalanceBudget monthlyBalanceBudget = new MonthlyBalanceBudget();
                            monthlyBalanceBudget.COAMonthlyBalanceBudget = new IDS.GLTable.ChartOfAccount();
                            monthlyBalanceBudget.COAMonthlyBalanceBudget.Account = dr["acc"] as string;

                            monthlyBalanceBudget.COAMonthlyBalanceBudget.CCy = new IDS.GeneralTable.Currency();
                            monthlyBalanceBudget.COAMonthlyBalanceBudget.CCy.CurrencyCode = dr["ccy"] as string;

                            monthlyBalanceBudget.Period = dr["mn"] as string;
                            monthlyBalanceBudget.BegBal = Convert.ToDecimal(dr["begbal"]);
                            monthlyBalanceBudget.Budget = Convert.ToDecimal(dr["budget"]);

                            monthlyBalanceBudget.BranchMonthlyBalanceBudget = new IDS.GeneralTable.Branch();
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchCode = dr["branchcode"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchName = dr["branchname"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.NPWP = dr["NPWP"] as string;

                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchManagerName = dr["BranchManager"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.FinAccOfficer = dr["FinAccOfficer"] as string;

                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.Address1 = dr["Addr1"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.Address2 = dr["Addr2"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.Address3 = dr["Addr3"] as string;
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.Country = Country.GetCountry(Tool.GeneralHelper.NullToString(dr["CountryCode"]));
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.City = City.GetCity(Tool.GeneralHelper.NullToString(dr["CityCode"]), Tool.GeneralHelper.NullToString(dr["CountryCode"]));

                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.PostalCode = dr["PostalCode"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.Phone1 = dr["Phone1"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.Phone2 = dr["Phone2"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.Phone3 = dr["Phone3"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.Fax = dr["Fax"] as string;

                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.FullAddress =
                            //        IDS.Tool.GeneralHelper.NullToString(dr["Addr1"]) +
                            //        IDS.Tool.GeneralHelper.NullToString(dr["Addr2"]) +
                            //        IDS.Tool.GeneralHelper.NullToString(dr["Addr3"]) +
                            //        IDS.Tool.GeneralHelper.NullToString(monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchCity?.CityName);

                            // Print Option
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.PrintBranchName = Convert.ToBoolean(dr["Chknama"]);
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.PrintBranchAddress = Convert.ToBoolean(dr["Chkaddress"]);
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.PrintBranchCity = Convert.ToBoolean(dr["Chkcity"]);
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.PrintBranchCountry = Convert.ToBoolean(dr["Chkcountry"]);
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.PrintDate = Convert.ToBoolean(dr["ChkPrintDate"]);
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.PrintTime = Convert.ToBoolean(dr["ChkPrintTime"]);
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.PrintPage = Convert.ToBoolean(dr["ChkPage"]);
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.Language = Convert.ToBoolean(dr["OptIndex"]);

                            // GL
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.RepNo = IDS.GL.ChartOfAccount.GetCOA(IDS.Tool.GeneralHelper.NullToString(dr["RepNo"]));
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.HopNo = IDS.GL.ChartOfAccount.GetCOA(IDS.Tool.GeneralHelper.NullToString(dr["HopNo"]));

                            // Leasing
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.ContractLimit = Convert.ToDecimal(dr["ContractLimit"]);
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.NUPApprovalLimit = Convert.ToDecimal(dr["NUPApprovalLimit"]);
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.NUPExpiredDays = Convert.ToInt32(dr["NUPExpiredDay"]);
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.OverRate = Convert.ToDecimal(dr["OverRate"]);
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.OverRateRetail = Convert.ToDecimal(dr["OverRateRetail"]);
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.PenaltyRate = Convert.ToDecimal(dr["PenaltyRate"]);
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.PenaltyRateRetail = Convert.ToDecimal(dr["PenaltyRateRetail"]);
                            ////monthlyBalance.BranchMonthlyBalance.LastCount = Convert.ToInt32(dr["LastCount"]);
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.LastReceiptNo = Convert.ToInt32(dr["LastReceiptNo"]);

                            //// SLIK
                            //monthlyBalanceBudget.BranchMonthlyBalanceBudget.SLIKCode = dr["SLIKCode"] as string;

                            monthlyBalanceBudget.EntryUser = dr["EntryUser"] as string;
                            monthlyBalanceBudget.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            monthlyBalanceBudget.OperatorID = dr["OperatorID"] as string;
                            monthlyBalanceBudget.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(monthlyBalanceBudget);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<MonthlyBalanceBudget> GetMonthlyBalanceBudget(string period, string branchCode)
        {
            List<IDS.GLTable.MonthlyBalanceBudget> list = new List<MonthlyBalanceBudget>();

            if (string.IsNullOrEmpty(period))
            {
                period = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');
            }

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelMonthlyBalanceBudget";
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
                            MonthlyBalanceBudget monthlyBalanceBudget = new MonthlyBalanceBudget();
                            monthlyBalanceBudget.COAMonthlyBalanceBudget = new IDS.GLTable.ChartOfAccount();
                            monthlyBalanceBudget.COAMonthlyBalanceBudget.Account = dr["acc"] as string;
                            monthlyBalanceBudget.COAMonthlyBalanceBudget.AccountName = dr["acc_name"] as string;

                            monthlyBalanceBudget.CurrencyMonthlyBalanceBudget = new IDS.GeneralTable.Currency();
                            monthlyBalanceBudget.CurrencyMonthlyBalanceBudget.CurrencyCode = dr["ccy"] as string;

                            monthlyBalanceBudget.Period = dr["mn"] as string;
                            monthlyBalanceBudget.BegBal = Convert.ToDecimal(dr["begbal"]);
                            monthlyBalanceBudget.Budget = Convert.ToDecimal(dr["budget"]);

                            monthlyBalanceBudget.BranchMonthlyBalanceBudget = new IDS.GeneralTable.Branch();
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchCode = dr["branchcode"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchName = dr["branchname"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.NPWP = dr["NPWP"] as string;

                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.BranchManagerName = dr["BranchManager"] as string;
                            monthlyBalanceBudget.BranchMonthlyBalanceBudget.FinAccOfficer = dr["FinAccOfficer"] as string;
                            monthlyBalanceBudget.OperatorID = dr["OperatorID"] as string;
                            monthlyBalanceBudget.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);


                            list.Add(monthlyBalanceBudget);
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

        public int InsUpDelMonthlyBalanceBudget(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLTUpdateACFGLMD";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, COAMonthlyBalanceBudget.Account);
                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CurrencyMonthlyBalanceBudget.CurrencyCode);
                    cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchMonthlyBalanceBudget.BranchCode);
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
                            throw new Exception("Monthly Balance Budget is already exists. Please choose other Monthly Balance Budget.");
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
                            throw new Exception("Monthly Balance Budget is already exists. Please choose other Monthly Balance Budget.");
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
