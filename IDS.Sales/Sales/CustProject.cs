using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class CustProject
    {
        [Display(Name = "Project Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project Code")]
        [MaxLength(20)]
        public string CustProjCode { get; set; }

        [Display(Name = "Project Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project Name")]
        [MaxLength(200)]
        public string CustProjName { get; set; }

        [Display(Name = "Branch Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch Name is required")]
        public IDS.GeneralTable.Branch BranchProject { get; set; }

        [Display(Name = "Customer Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer Code")]
        [MaxLength(20)]
        public string CustCode { get; set; }

        [Display(Name = "Payment Cycle")]
        [MaxLength(20)]
        public string PaymentCycle { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Start Period")]
        public DateTime StartPeriod { get; set; }

        [Display(Name = "Discount Amount")]
        //[Range(0, 255)]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Amount Billing")]
        //[Range(0, 255)]
        public decimal AmountBilling { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }

        [Display(Name = "Next Period")]
        public string NextPeriod { get; set; }

        [Display(Name = "Charts of Account Customer Project")]
        //[MaxLength(10), StringLength(10)]
        public IDS.GLTable.ChartOfAccount CustProjAcc { get; set; }

        [Display(Name = "Unearned ACC")]
        public string UnearnedACC { get; set; }

        [Display(Name = "Currency Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        public IDS.GeneralTable.Currency CustProjCCy { get; set; }

        [Display(Name = "Status Active")]
        public bool StatusActive { get; set; }

        [Display(Name = "Status PPn")]
        public bool StatusPPn { get; set; }

        [Display(Name = "Entry User")]
        [MaxLength(20), StringLength(20)]
        public string EntryUser { get; set; }

        [Display(Name = "Entry Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        [MaxLength(20), StringLength(20)]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public CustProject()
        {

        }

        public CustProject(string custProjCode, string branchCode)
        {
            CustProjCode = custProjCode;
            BranchProject = IDS.GeneralTable.Branch.GetBranch(branchCode);
        }

        public static List<CustProject> GetCustProject()
        {
            List<IDS.Sales.CustProject> list = new List<CustProject>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelCustProject";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            CustProject custProj = new CustProject();
                            custProj.CustProjCode = dr["ProjectCode"] as string;
                            custProj.CustProjName = dr["ProjectName"] as string;

                            custProj.BranchProject = new GeneralTable.Branch();
                            custProj.BranchProject.BranchCode = dr["branchcode"] as string;
                            custProj.BranchProject.BranchName = dr["branchname"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            custProj.CustCode = dr["CustCode"] as string;
                            custProj.PaymentCycle = dr["PaymentCycle"] as string;

                            if (!string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["StartPeriod"])))
                                custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);

                            custProj.DiscountAmount = Tool.GeneralHelper.NullToDecimal(dr["DiscountAmount"],0);
                            custProj.AmountBilling = Tool.GeneralHelper.NullToDecimal(dr["AmountBilling"],0);
                            custProj.Remark = dr["Remark"] as string;
                            custProj.NextPeriod = dr["NextPeriod"] as string;

                            custProj.CustProjAcc = new GLTable.ChartOfAccount();
                            custProj.CustProjAcc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            custProj.UnearnedACC = dr["UnearnedACC"] as string;

                            custProj.CustProjCCy = new GeneralTable.Currency();
                            custProj.CustProjCCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            custProj.EntryUser = dr["EntryUser"] as string;
                            custProj.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            custProj.OperatorID = dr["OperatorID"] as string;
                            custProj.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(custProj);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static CustProject GetCustProject(string custProjCode)
        {
            CustProject custProj = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelCustProject";
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, custProjCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        custProj = new CustProject();
                        custProj.CustProjCode = dr["ProjectCode"] as string;
                        custProj.CustProjName = dr["ProjectName"] as string;

                        custProj.BranchProject = new GeneralTable.Branch();
                        custProj.BranchProject.BranchCode = dr["branchcode"] as string;
                        custProj.BranchProject.BranchName = dr["branchname"] as string;
                        //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                        //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                        //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                        //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                        custProj.CustCode = dr["CustCode"] as string;
                        custProj.PaymentCycle = dr["PaymentCycle"] as string;
                        custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                        custProj.DiscountAmount = Tool.GeneralHelper.NullToDecimal(dr["DiscountAmount"],0);
                        custProj.AmountBilling = Tool.GeneralHelper.NullToDecimal(dr["AmountBilling"],0);
                        custProj.Remark = dr["Remark"] as string;
                        custProj.NextPeriod = dr["NextPeriod"] as string;
                        custProj.StatusActive = Tool.GeneralHelper.NullToBool(dr["StatusActive"]);
                        custProj.StatusPPn = Tool.GeneralHelper.NullToBool(dr["StatusPPn"]);

                        custProj.CustProjAcc = new GLTable.ChartOfAccount();
                        custProj.CustProjAcc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                        custProj.UnearnedACC = dr["UnearnedACC"] as string;

                        custProj.CustProjCCy = new GeneralTable.Currency();
                        custProj.CustProjCCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                        custProj.EntryUser = dr["EntryUser"] as string;
                        custProj.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        custProj.OperatorID = dr["OperatorID"] as string;
                        custProj.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return custProj;
        }

        public int InsUpDelCustProject(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "slsUpdateCustProject";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@CustProjCODE", System.Data.SqlDbType.VarChar, CustProjCode);
                    cmd.AddParameter("@CustProjName", System.Data.SqlDbType.VarChar, CustProjName);
                    cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchProject.BranchCode);
                    cmd.AddParameter("@CustCODE", System.Data.SqlDbType.VarChar, CustCode);
                    cmd.AddParameter("@PaymentCycle", System.Data.SqlDbType.VarChar, PaymentCycle);
                    cmd.AddParameter("@StartPeriod", System.Data.SqlDbType.DateTime, StartPeriod);
                    cmd.AddParameter("@DiscAmt", System.Data.SqlDbType.Money, DiscountAmount);
                    cmd.AddParameter("@AmtBill", System.Data.SqlDbType.VarChar, AmountBilling);
                    cmd.AddParameter("@Remark", System.Data.SqlDbType.VarChar, Remark);
                    cmd.AddParameter("@NextPeriod", System.Data.SqlDbType.VarChar, Convert.ToDateTime(NextPeriod).Year.ToString() + Convert.ToDateTime(NextPeriod).Month.ToString().PadLeft(2, '0'));
                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, CustProjAcc.Account);
                    cmd.AddParameter("@UnearnedAcc", System.Data.SqlDbType.VarChar, UnearnedACC);
                    cmd.AddParameter("@Ccy", System.Data.SqlDbType.VarChar, CustProjCCy.CurrencyCode);
                    cmd.AddParameter("@UnearnedAcc", System.Data.SqlDbType.VarChar, UnearnedACC);
                    cmd.AddParameter("@StatusActive", System.Data.SqlDbType.Bit, StatusActive);
                    cmd.AddParameter("@StatusPPn", System.Data.SqlDbType.Bit, StatusPPn);
                    cmd.AddParameter("@operatorID", System.Data.SqlDbType.VarChar, OperatorID);
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
                            throw new Exception("Customer Project code is already exists. Please choose other Customer Project code.");
                        default:
                            throw;
                    }
                }

                finally
                {
                    cmd.Close();
                }
            }

            return result;
        }

        public int InsUpDelCustProject(Tool.PageActivity ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "slsUpdateCustProject";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "slsUpdateCustProject";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, (int)ExecCode);
                        cmd.AddParameter("@CustProjCODE", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Customer Project Code is already exists. Please choose other Customer Project Code.");
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

        public static int GetNextPeriod(string nextPeriod)
        {
            int result = 0;
            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                cmd.CommandText = "SalesSelCustProject";
                cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                cmd.AddParameter("@NextPeriod", System.Data.SqlDbType.VarChar, nextPeriod);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Open();
                result = Tool.GeneralHelper.NullToInt(cmd.ExecuteScalar(),0);
                cmd.Close();

                return result;
            }
        }
    }
}
