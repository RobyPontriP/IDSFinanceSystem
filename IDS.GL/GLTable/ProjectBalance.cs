using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class ProjectBalance
    {
        [Display(Name = "Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        [MaxLength(15), StringLength(15)]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public string ProjectName { get; set; }

        [Display(Name = "Account")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Account No is required")]
        public IDS.GLTable.ChartOfAccount COAProjectBalance { get; set; }

        [Display(Name = "Currency Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        public IDS.GeneralTable.Currency CurrencyProjectBalance { get; set; }

        [Display(Name = "Branch")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch is required")]
        public IDS.GeneralTable.Branch BranchProjectBalance { get; set; }

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

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Beginning Date")]
        public DateTime BegDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Created By")]
        public string Description { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public ProjectBalance()
        {

        }

        public static List<ProjectBalance> GetProjectBalance(string branchCode)
        {
            List<IDS.GLTable.ProjectBalance> list = new List<ProjectBalance>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelProjectBalance";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.AddParameter("@BRANCHCODE", System.Data.SqlDbType.VarChar, branchCode);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ProjectBalance projBal = new ProjectBalance();
                            projBal.Code = dr["Code"] as string;

                            projBal.BranchProjectBalance = new GeneralTable.Branch();
                            projBal.BranchProjectBalance.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            projBal.COAProjectBalance = new GLTable.ChartOfAccount();
                            projBal.COAProjectBalance.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            projBal.CurrencyProjectBalance = new GeneralTable.Currency();
                            projBal.CurrencyProjectBalance.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            projBal.BegBal = string.IsNullOrEmpty(dr["BEGBAL"].ToString()) ? 0 : Convert.ToDecimal(dr["BEGBAL"]);
                            projBal.Debit = string.IsNullOrEmpty(dr["DEBIT"].ToString()) ? 0 : Convert.ToDecimal(dr["DEBIT"]);
                            projBal.Credit = string.IsNullOrEmpty(dr["CREDIT"].ToString()) ? 0 : Convert.ToDecimal(dr["CREDIT"]);
                            projBal.Budget = string.IsNullOrEmpty(dr["BUDGET"].ToString()) ? 0 : Convert.ToDecimal(dr["BUDGET"]);
                            projBal.EndBal = string.IsNullOrEmpty(dr["ENDBAL"].ToString()) ? 0 : Convert.ToDecimal(dr["ENDBAL"]);
                            projBal.BegDate = Convert.ToDateTime(dr["BegDate"]);
                            projBal.EndDate = Convert.ToDateTime(dr["EndDate"]);
                            projBal.OperatorID = dr["OperatorID"] as string;
                            projBal.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(projBal);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static ProjectBalance GetProjectBalance(string branchCode, string projCode, string coa, string currency)
        {
            ProjectBalance projBal = new ProjectBalance();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelProjectBalance";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, projCode);
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, coa);
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, currency);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        projBal.Code = dr["Code"] as string;

                        projBal.Code = dr["Code"] as string;

                        projBal.BranchProjectBalance = new GeneralTable.Branch();
                        projBal.BranchProjectBalance.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                        projBal.COAProjectBalance = new GLTable.ChartOfAccount();
                        projBal.COAProjectBalance.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                        projBal.CurrencyProjectBalance = new GeneralTable.Currency();
                        projBal.CurrencyProjectBalance.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                        projBal.BegBal = string.IsNullOrEmpty(dr["BEGBAL"].ToString()) ? 0 : Convert.ToDecimal(dr["BEGBAL"]);
                        projBal.Debit = string.IsNullOrEmpty(dr["DEBIT"].ToString()) ? 0 : Convert.ToDecimal(dr["DEBIT"]);
                        projBal.Credit = string.IsNullOrEmpty(dr["CREDIT"].ToString()) ? 0 : Convert.ToDecimal(dr["CREDIT"]);
                        projBal.Budget = string.IsNullOrEmpty(dr["BUDGET"].ToString()) ? 0 : Convert.ToDecimal(dr["BUDGET"]);
                        projBal.EndBal = string.IsNullOrEmpty(dr["ENDBAL"].ToString()) ? 0 : Convert.ToDecimal(dr["ENDBAL"]);
                        projBal.BegDate = Convert.ToDateTime(dr["BegDate"]);
                        projBal.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        projBal.OperatorID = dr["OperatorID"] as string;
                        projBal.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return projBal;
        }

        public int InsUpDelProjectBalance(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLupdateProjectBalance";
                    cmd.AddParameter("@Utype", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, Code);
                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, COAProjectBalance.Account);
                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CurrencyProjectBalance.CurrencyCode);
                    cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchProjectBalance.BranchCode);
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
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Project Balance is already exists. Please choose other Project Balance.");
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

        public int InsUpDelProjectBalance(int ExecCode, string branch, string proj, string[] coa, string[] currency)
        {
            int result = 0;

            if (coa == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLupdateProjectBalance";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < coa.Length; i++)
                    {
                        cmd.CommandText = "GLupdateProjectBalance";
                        cmd.AddParameter("@UType", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, proj);
                        cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                        cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, coa[i]);
                        cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, currency[i]);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommitTransaction();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Project Balance is already exists. Please choose other Projuect Balance.");
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
