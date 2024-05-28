using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IDS.GLTransaction
{
    public class GLStandingOrderH
    {
        [Display(Name = "SO Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "SO Code is required")]
        [MaxLength(5), StringLength(5)]
        public string Code { get; set; }

        [Display(Name = "Branch Code")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Branch code is required")]
        public IDS.GeneralTable.Branch VBranch { get; set; }

        [Display(Name = "Start date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Expiry Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Expiry date is required")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Exec date")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Start date is required")]
        [Range(minimum: 1, maximum: 31, ErrorMessage = "Exec date must be between 1 to 31")]
        public int ExecDate { get; set; }

        [Display(Name = "Source Code")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Source code is required")]
        public GLTable.SourceCode SCode { get; set; }

        [Display(Name = "Exec Code")]
        //[MaxLength(6), StringLength(6)]
        public string ExecCode { get; set; } // Next Exec Period

        [Display(Name = "Description")]
        [MaxLength(50), StringLength(50)]
        public string Description { get; set; }

        [Display(Name = "Active Status")]
        public bool ActiveStatus { get; set; }
        
        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public IList<GLStandingOrderD> Detail { get; set; }

        public GLStandingOrderH()
        {
        }

        public static List<GLStandingOrderH> GetStandingorderHeader(string branchCode)
        {
            List<GLStandingOrderH> SOH = new List<GLStandingOrderH>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelGLSOH";
                db.AddParameter("@SONo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@FromDate", System.Data.SqlDbType.DateTime, DBNull.Value);
                db.AddParameter("@ToDate", System.Data.SqlDbType.DateTime, DBNull.Value);
                db.AddParameter("@DateType", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Status", System.Data.SqlDbType.Bit, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GLStandingOrderH h = new GLStandingOrderH();
                            h.Code = Tool.GeneralHelper.NullToString(dr["SOHNo"]);

                            h.VBranch = new GeneralTable.Branch();
                            h.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            h.StartDate = Convert.ToDateTime(dr["SOHStartDate"]);
                            h.ExpiryDate = Convert.ToDateTime(dr["SOHExpiryDate"]);
                            h.ExecDate = Tool.GeneralHelper.NullToInt(dr["SOHExecD"], 0);
                            h.ExecCode = Tool.GeneralHelper.NullToString(dr["SOHLExec"]);
                            h.Description = Tool.GeneralHelper.NullToString(dr["SOHDesc"]);

                            h.SCode = new GLTable.SourceCode();
                            h.SCode.Code = Tool.GeneralHelper.NullToString(dr["SOHSC"]);

                            h.ActiveStatus = Tool.GeneralHelper.NullToBool(dr["SOHStsActive"], false);
                            h.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"]);
                            h.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            h.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            h.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            SOH.Add(h);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return SOH;
        }

        public static List<GLStandingOrderH> GetStandingorderHeaderForGrid(string branchCode, DateTime? startDate, DateTime? toDate, int dateType)
        {
            List<GLStandingOrderH> SOH = new List<GLStandingOrderH>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelGLSOH";
                db.AddParameter("@SONo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                
                db.AddParameter("@FromDate", System.Data.SqlDbType.DateTime, startDate);
                db.AddParameter("@ToDate", System.Data.SqlDbType.DateTime, toDate);
                db.AddParameter("@DateType", System.Data.SqlDbType.TinyInt, dateType);

                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 7);
                db.AddParameter("@Status", System.Data.SqlDbType.Bit, DBNull.Value);
                

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GLStandingOrderH h = new GLStandingOrderH();
                            h.Code = Tool.GeneralHelper.NullToString(dr["SOHNo"]);

                            h.VBranch = new GeneralTable.Branch();
                            h.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            h.StartDate = Convert.ToDateTime(dr["SOHStartDate"]);
                            h.ExpiryDate = Convert.ToDateTime(dr["SOHExpiryDate"]);
                            h.ExecDate = Tool.GeneralHelper.NullToInt(dr["SOHExecD"], 0);
                            h.ExecCode = Tool.GeneralHelper.NullToString(dr["SOHLExec"]);
                            h.Description = Tool.GeneralHelper.NullToString(dr["SOHDesc"]);

                            h.SCode = new GLTable.SourceCode();
                            h.SCode.Code = Tool.GeneralHelper.NullToString(dr["SOHSC"]);

                            h.ActiveStatus = Tool.GeneralHelper.NullToBool(dr["SOHStsActive"], false);
                            h.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"]);
                            h.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            h.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            h.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            SOH.Add(h);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return SOH;
        }

        public static List<GLStandingOrderH> GetStandingOrderWithDetail(string branchCode)
        {
            List<GLStandingOrderH> SOH = new List<GLStandingOrderH>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelGLSOH";

                db.AddParameter("@SONo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@FromDate", System.Data.SqlDbType.DateTime, DBNull.Value);
                db.AddParameter("@ToDate", System.Data.SqlDbType.DateTime, DBNull.Value);
                db.AddParameter("@DateType", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Status", System.Data.SqlDbType.Bit, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        GLStandingOrderH lastHeader = null;

                        while (dr.Read())
                        {
                            #region Fill Header
                            if (lastHeader == null || Tool.GeneralHelper.NullToString(dr["SOHNo"]) != lastHeader.Code)
                            {
                                GLStandingOrderH h = new GLStandingOrderH();
                                h.Code = Tool.GeneralHelper.NullToString(dr["SOHNo"]);
                                h.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);
                                h.StartDate = Convert.ToDateTime(dr["SOHStartDate"]);
                                h.ExpiryDate = Convert.ToDateTime(dr["SOHExpiryDate"]);
                                h.ExecDate = Tool.GeneralHelper.NullToInt(dr["SOHExecD"], 0);
                                h.ExecCode = Tool.GeneralHelper.NullToString(dr["SOHLExec"]);
                                h.Description = Tool.GeneralHelper.NullToString(dr["SOHDesc"]);

                                h.SCode = new GLTable.SourceCode();
                                h.SCode.Code = Tool.GeneralHelper.NullToString(dr["SOHSC"]);

                                h.ActiveStatus = Tool.GeneralHelper.NullToBool(dr["SOHStsActive"], false);
                                h.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"]);
                                h.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                                h.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                                h.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                                SOH.Add(h);
                                lastHeader = h;
                            }
                            #endregion

                            if (dr["SODCtr"] == DBNull.Value)
                                continue;

                            if (lastHeader.Detail == null)
                                lastHeader.Detail = new List<GLStandingOrderD>();

                            #region Fill Detail
                            GLStandingOrderD d = new GLStandingOrderD();
                            d.Counter = Convert.ToInt32(dr["SODCtr"]);
                            
                            d.CCy = new GeneralTable.Currency();
                            d.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["SODCcy"]);

                            d.COA = new GLTable.ChartOfAccount();
                            d.COA.Account = Tool.GeneralHelper.NullToString(dr["SODAcc"]);
                            d.COA.CCy = d.CCy;

                            d.CashAccount = new GLTable.CashBasisAccount();
                            d.CashAccount.Account = Tool.GeneralHelper.NullToString(dr["CASHACC"]);
                            d.CashAccount.CCy = d.CCy;

                            if (dr["SODDept"] != DBNull.Value)
                            {
                                d.Dept = new GeneralTable.Department();
                                d.Dept.DepartmentCode = Tool.GeneralHelper.NullToString(dr["SODDept"]);
                                d.Dept.BranchDepartment = lastHeader.VBranch;
                            }

                            if (dr["SODProj"] != DBNull.Value)
                            {
                                d.Proj = new GLTable.Project();
                                d.Proj.ProjectCode = Tool.GeneralHelper.NullToString(dr["SODProj"]);
                            }

                            d.DocumentNo = Tool.GeneralHelper.NullToString(dr["SODDocNo"]);

                            if (dr["SODCS"] != DBNull.Value)
                            {
                                d.CustSupp = new GeneralTable.CustomerSupplier();
                                d.CustSupp.Code = Tool.GeneralHelper.NullToString(dr["SODCS"]);
                            }

                            d.Amount = Tool.GeneralHelper.NullToDouble(dr["SODAmount"], 0);
                            d.Description = Tool.GeneralHelper.NullToString(dr["SODDesc"]);

                            lastHeader.Detail.Add(d);
                            #endregion
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return SOH;
        }

        public static GLStandingOrderH GetStandingOrderWithDetail(string branchCode, string standingOrderCode)
        {
            GLStandingOrderH h = null;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelGLSOH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@SONo", System.Data.SqlDbType.VarChar, standingOrderCode);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@FromDate", System.Data.SqlDbType.DateTime, DBNull.Value);
                db.AddParameter("@ToDate", System.Data.SqlDbType.DateTime, DBNull.Value);
                db.AddParameter("@DateType", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Status", System.Data.SqlDbType.Bit, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                                
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            #region Fill Header
                            if (h == null || Tool.GeneralHelper.NullToString(dr["SOHNo"]) != h.Code)
                            {
                                h = new GLStandingOrderH();
                                h.Code = Tool.GeneralHelper.NullToString(dr["SOHNo"]);

                                h.VBranch = new GeneralTable.Branch();
                                h.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                                h.StartDate = Convert.ToDateTime(dr["SOHStartDate"]);
                                h.ExpiryDate = Convert.ToDateTime(dr["SOHExpiryDate"]);
                                h.ExecDate = Tool.GeneralHelper.NullToInt(dr["SOHExecD"], 0);
                                h.ExecCode = Tool.GeneralHelper.NullToString(dr["SOHLExec"]);
                                h.Description = Tool.GeneralHelper.NullToString(dr["SOHDesc"]);

                                h.SCode = new GLTable.SourceCode();
                                h.SCode.Code = Tool.GeneralHelper.NullToString(dr["SOHSC"]);

                                h.ActiveStatus = Tool.GeneralHelper.NullToBool(dr["SOHStsActive"], false);
                                h.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"]);
                                h.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                                h.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                                h.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            }
                            #endregion

                            if (dr["SODCtr"] == DBNull.Value)
                                continue;

                            if (h.Detail == null)
                                h.Detail = new List<GLStandingOrderD>();

                            #region Fill Detail
                            GLStandingOrderD d = new GLStandingOrderD();
                            d.Counter = Convert.ToInt32(dr["SODCtr"]);

                            d.CCy = new GeneralTable.Currency();
                            d.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["SODCcy"]);

                            d.COA = new GLTable.ChartOfAccount();
                            d.COA.Account = Tool.GeneralHelper.NullToString(dr["SODAcc"]);
                            d.COA.AccountName = Tool.GeneralHelper.NullToString(dr["AccName"]);
                            d.COA.CCy = d.CCy;

                            d.CashAccount = new GLTable.CashBasisAccount();
                            d.CashAccount.Account = Tool.GeneralHelper.NullToString(dr["CASHACC"]);
                            d.CashAccount.CCy = d.CCy;

                            if (dr["SODDept"] != DBNull.Value)
                            {
                                d.Dept = new GeneralTable.Department();
                                d.Dept.DepartmentCode = Tool.GeneralHelper.NullToString(dr["SODDept"]);
                                d.Dept.BranchDepartment = h.VBranch;
                            }

                            if (dr["SODProj"] != DBNull.Value)
                            {
                                d.Proj = new GLTable.Project();
                                d.Proj.ProjectCode = Tool.GeneralHelper.NullToString(dr["SODProj"]);
                            }

                            d.DocumentNo = Tool.GeneralHelper.NullToString(dr["SODDocNo"]);

                            if (dr["SODCS"] != DBNull.Value)
                            {
                                d.CustSupp = new GeneralTable.CustomerSupplier();
                                d.CustSupp.Code = Tool.GeneralHelper.NullToString(dr["SODCS"]);
                            }

                            d.Amount = Tool.GeneralHelper.NullToDouble(dr["SODAmount"], 0);
                            d.Description = Tool.GeneralHelper.NullToString(dr["SODDesc"]);

                            h.Detail.Add(d);
                            #endregion
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return h;
        }

        public int InsUpDel(int ExeCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "GL_UpdACFSORDH";
                    db.AddParameter("@SOHNo", System.Data.SqlDbType.VarChar, Code);
                    db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, VBranch.BranchCode);
                    db.AddParameter("@SOHStartDate", System.Data.SqlDbType.DateTime, StartDate);
                    db.AddParameter("@SOHExpiryDate", System.Data.SqlDbType.DateTime, ExpiryDate);
                    db.AddParameter("@SOHExecD", System.Data.SqlDbType.TinyInt, ExecDate);
                    db.AddParameter("@SOHLExec", System.Data.SqlDbType.VarChar, ExeCode);
                    db.AddParameter("@SOHDesc", System.Data.SqlDbType.VarChar, Description);
                    db.AddParameter("@SOHSC", System.Data.SqlDbType.VarChar, (object)SCode?.Code ?? DBNull.Value);
                    db.AddParameter("@SOHStsActive", System.Data.SqlDbType.Bit, ActiveStatus);
                    db.AddParameter("@OperartorID", System.Data.SqlDbType.VarChar, OperatorID);
                    db.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExeCode);

                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();

                    db.BeginTransaction();
                    result = db.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                    }

                    //if ((int)IDS.Tool.PageActivity.Edit == ExeCode)
                    //{
                    //    db.CommandText = "GL_UpdACFSORDD";
                    //    db.AddParameter("@SODNo", System.Data.SqlDbType.VarChar, Code);
                    //    db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, VBranch.BranchCode);
                    //    db.AddParameter("@SODCtr", System.Data.SqlDbType.TinyInt, DBNull.Value);
                    //    db.AddParameter("@SODCcy", System.Data.SqlDbType.VarChar, DBNull.Value);
                    //    db.AddParameter("@SODAcc", System.Data.SqlDbType.VarChar, DBNull.Value);
                    //    db.AddParameter("@SODDept", System.Data.SqlDbType.VarChar, DBNull.Value);
                    //    db.AddParameter("@SODProj", System.Data.SqlDbType.VarChar, DBNull.Value);
                    //    db.AddParameter("@SODDocNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                    //    db.AddParameter("@SODCS", System.Data.SqlDbType.VarChar, DBNull.Value);
                    //    db.AddParameter("@SODAmount", System.Data.SqlDbType.Money, DBNull.Value);
                    //    db.AddParameter("@SODDesc", System.Data.SqlDbType.VarChar, DBNull.Value);
                    //    db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);

                    //    result = db.ExecuteNonQuery();

                    //    if (result <= 0)
                    //    {
                    //        throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                    //    }
                    //}

                    db.CommandText = "GL_UpdACFSORDD";
                    db.DbCommand.CommandTimeout = 0;
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();

                    for (int i = 0; i < Detail.Count; i++)
                    {
                        db.AddParameter("@SODNo", System.Data.SqlDbType.VarChar, Code);
                        db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, VBranch.BranchCode);
                        db.AddParameter("@SODCtr", System.Data.SqlDbType.TinyInt, (i + 1));
                        db.AddParameter("@SODCcy", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].CCy?.CurrencyCode ?? null));
                        db.AddParameter("@SODAcc", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].COA?.Account ?? null));
                        db.AddParameter("@SODCashAcc", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].CashAccount?.Account ?? null));
                        db.AddParameter("@SODDept", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Dept?.DepartmentCode ?? null));
                        db.AddParameter("@SODProj", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Proj?.ProjectCode ?? null));
                        db.AddParameter("@SODDocNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].DocumentNo));
                        db.AddParameter("@SODCS", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].CustSupp?.Code));
                        db.AddParameter("@SODAmount", System.Data.SqlDbType.Money, Tool.GeneralHelper.NullToDouble(Detail[i].Amount, 0));
                        db.AddParameter("@SODDesc", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Description));
                        db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExeCode);

                        result = db.ExecuteNonQuery();

                        if (result <= 0)
                        {
                            throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                        }
                    }

                    db.CommitTransaction();
                    db.Close();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Standing Order already exists, please try change new Standing Order code and save again.");
                        default:
                            throw;
                    }
                }
                catch
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    throw;
                }
                finally
                {
                    db.Close();
                }
            }

            return result;
        }

        public int InsUpDel(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GL_UpdACFSORDD";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();
                    cmd.BeginTransaction();

                    string[] item;
                    string sOHNo = "";
                    string branchCode = "";

                    char separator = ';';

                    for (int i = 0; i < data.Length; i++)
                    {
                        item = data[i].Split(separator);

                        sOHNo = item[0];
                        branchCode = item[1];

                        cmd.AddParameter("@SODNo", System.Data.SqlDbType.VarChar, sOHNo);
                        cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);


                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = "GL_UpdACFSORDH";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    for (int i = 0; i < data.Length; i++)
                    {
                        item = data[i].Split(separator);

                        sOHNo = item[0];
                        branchCode = item[1];

                        cmd.AddParameter("@SOHNo", System.Data.SqlDbType.VarChar, sOHNo);
                        cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);


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
                            throw new Exception(" Standing Order is already exists. Please choose other Standing Order Code.");
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
