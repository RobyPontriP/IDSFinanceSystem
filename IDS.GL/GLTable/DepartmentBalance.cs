using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class DepartmentBalance
    {
        [Display(Name = "Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        [MaxLength(10), StringLength(10)]
        public string Code { get; set; }

        [Display(Name = "Account")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Account No is required")]
        public IDS.GLTable.ChartOfAccount COADepartmentBalance { get; set; }

        [Display(Name = "Currency Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        //[MaxLength(3)]
        public IDS.GeneralTable.Currency CurrencyDepartmentBalance { get; set; }

        [Display(Name = "Period")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Period is required")]
        //[MaxLength(6), StringLength(6)]
        public string Period { get; set; }

        [Display(Name = "Branch")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch is required")]
        public IDS.GeneralTable.Branch BranchDepartmentBalance { get; set; }

        //[Display(Name = "Branch")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Branch is required")]
        //public string BranchCode { get; set; }

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

        public DepartmentBalance()
        {

        }

        public static List<DepartmentBalance> GetDepartmentBalance()
        {
            List<IDS.GLTable.DepartmentBalance> list = new List<DepartmentBalance>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelDeptBalance";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DepartmentBalance deptBal = new DepartmentBalance();
                            deptBal.Code = dr["Code"] as string;

                            deptBal.BranchDepartmentBalance = new GeneralTable.Branch();
                            deptBal.BranchDepartmentBalance.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            deptBal.COADepartmentBalance= new GLTable.ChartOfAccount();
                            deptBal.COADepartmentBalance.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            deptBal.CurrencyDepartmentBalance = new GeneralTable.Currency();
                            deptBal.CurrencyDepartmentBalance.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            deptBal.Period = dr["MN"] as string;

                            deptBal.BegBal = string.IsNullOrEmpty(dr["BEGBAL"].ToString()) ? 0 : Convert.ToDecimal(dr["BEGBAL"]);
                            deptBal.Debit = string.IsNullOrEmpty(dr["DEBIT"].ToString()) ? 0 : Convert.ToDecimal(dr["DEBIT"]);
                            deptBal.Credit = string.IsNullOrEmpty(dr["CREDIT"].ToString()) ? 0 : Convert.ToDecimal(dr["CREDIT"]);
                            deptBal.Budget = string.IsNullOrEmpty(dr["BUDGET"].ToString()) ? 0 : Convert.ToDecimal(dr["BUDGET"]);
                            deptBal.EndBal = string.IsNullOrEmpty(dr["ENDBAL"].ToString()) ? 0 : Convert.ToDecimal(dr["ENDBAL"]);
                            deptBal.OperatorID = dr["OperatorID"] as string;
                            deptBal.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(deptBal);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static DepartmentBalance GetDepartmentBalance(string period, string branchCode,string deptCode, string coa, string currency)
        {
            DepartmentBalance deptBal = new DepartmentBalance();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelDeptBalance";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, deptCode);
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, coa);
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, currency);
                db.AddParameter("@MN", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        deptBal.Code = dr["Code"] as string;

                        deptBal.BranchDepartmentBalance = new GeneralTable.Branch();
                        deptBal.BranchDepartmentBalance.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                        deptBal.COADepartmentBalance = new GLTable.ChartOfAccount();
                        deptBal.COADepartmentBalance.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                        deptBal.CurrencyDepartmentBalance = new GeneralTable.Currency();
                        deptBal.CurrencyDepartmentBalance.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                        deptBal.Period = dr["MN"] as string;

                        deptBal.BegBal = string.IsNullOrEmpty(dr["BEGBAL"].ToString()) ? 0 : Convert.ToDecimal(dr["BEGBAL"]);
                        deptBal.Debit = string.IsNullOrEmpty(dr["DEBIT"].ToString()) ? 0 : Convert.ToDecimal(dr["DEBIT"]);
                        deptBal.Credit = string.IsNullOrEmpty(dr["CREDIT"].ToString()) ? 0 : Convert.ToDecimal(dr["CREDIT"]);
                        deptBal.Budget = string.IsNullOrEmpty(dr["BUDGET"].ToString()) ? 0 : Convert.ToDecimal(dr["BUDGET"]);
                        deptBal.EndBal = string.IsNullOrEmpty(dr["ENDBAL"].ToString()) ? 0 : Convert.ToDecimal(dr["ENDBAL"]);
                        deptBal.OperatorID = dr["OperatorID"] as string;
                        deptBal.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return deptBal;
        }

        public int InsUpDelDepartmentBalance(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLupdateDeptBalance";
                    cmd.AddParameter("@Utype", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, Code);
                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, COADepartmentBalance.Account);
                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CurrencyDepartmentBalance.CurrencyCode);
                    cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchDepartmentBalance.BranchCode);
                    cmd.AddParameter("@MN", System.Data.SqlDbType.VarChar, Period);
                    cmd.AddParameter("@begbal", System.Data.SqlDbType.Money, BegBal);
                    cmd.AddParameter("@Debit", System.Data.SqlDbType.Money, Debit);
                    cmd.AddParameter("@credit", System.Data.SqlDbType.Money, Credit);
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
                            throw new Exception("Department Balance is already exists. Please choose other Department Balance.");
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

        public int InsUpDelDepartmentBalance(int ExecCode, string[] data, string branch, string dept, string[] coa, string[] currency)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLupdateDeptBalance";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GLupdateDeptBalance";
                        cmd.AddParameter("@UType", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@MN", System.Data.SqlDbType.VarChar, data[i]);
                        cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, dept);
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
                            throw new Exception("Department Balance is already exists. Please choose other Department Balance.");
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

        public static List<System.Web.Mvc.SelectListItem> GetYearForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            int MinYear = 0;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelDeptBalance";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 5);
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
    }
}
