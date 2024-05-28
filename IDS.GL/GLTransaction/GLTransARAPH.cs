using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDS.Tool;
using System.ComponentModel.DataAnnotations;

namespace IDS.GLTransaction
{
    /// <summary>
    /// Voucher Header
    /// </summary>
    public class GLTransARAPH
    {
        public GLTable.SourceCode SCode { get; set; }
        public string Voucher { get; set; }
        public GeneralTable.Branch VBranch { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "{0: " + IDS.Tool.GlobalVariable.DEFAULT_DATE_FORMAT  + "}")]
        [Display(Name = "Entry Date")]
        public DateTime Entry_Date { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "{0: " + IDS.Tool.GlobalVariable.DEFAULT_DATE_FORMAT + "}")]
        [Display(Name = "Trans. Date")]
        public DateTime TransDate { get; set; }
        public int PayTerm { get; set; }
        public int Reversed { get; set; }
        public string ReversedVoucher { get; set; }
        public int ARAP_Trans { get; set; }
        public int StatusPayment { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "{0: " + IDS.Tool.GlobalVariable.DEFAULT_DATE_FORMAT + "}")]
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "{0: " + IDS.Tool.GlobalVariable.DEFAULT_DATE_FORMAT + "}")]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public List<GLVoucherD> Detail { get; set; }
        
        public GLTransARAPH()
        {
        }

        /// <summary>
        /// Retrieve Branch Voucher
        /// </summary>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        public static List<GLTransARAPH> GetVoucherWithDetailByAccountAndCurrency(string period, string branchCode, string account, string ccy)
        {
            List<GLTransARAPH> voucher = new List<GLTransARAPH>();
            
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelARAPHeader";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@VoucherNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@SCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@Account", System.Data.SqlDbType.VarChar, account);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.VarChar, 4);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        GLTransARAPH lastVoucher = null;

                        while (dr.Read())
                        {
                            #region Fill Header
                            if (lastVoucher == null ||
                                (Tool.GeneralHelper.NullToString(dr["VOUCHER"]) != lastVoucher.Voucher || 
                                Tool.GeneralHelper.NullToString(dr["SCODE"]) != lastVoucher.SCode.Code || 
                                Tool.GeneralHelper.NullToString(dr["BranchCode"]) != lastVoucher.VBranch.BranchCode))
                            {
                                GLTransARAPH vh = new GLTransARAPH();
                                vh.SCode = new GLTable.SourceCode();
                                vh.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCODE"]);

                                vh.VBranch = new GeneralTable.Branch();
                                vh.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                                vh.Voucher = Tool.GeneralHelper.NullToString(dr["VOUCHER"]);
                                vh.Entry_Date = Convert.ToDateTime(dr["ENT_DATE"]);
                                vh.TransDate = Convert.ToDateTime(dr["TRANS_DATE"]);
                                vh.PayTerm = Convert.ToInt32(dr["PAY_TERM"]);
                                vh.Reversed = Tool.GeneralHelper.NullToInt(dr["REVERSED"], Convert.ToInt32(string.IsNullOrEmpty(Tool.GeneralHelper.NullToString(dr["REV_VOUCHER"]))));
                                vh.ReversedVoucher = Tool.GeneralHelper.NullToString(dr["REV_VOUCHER"]);
                                vh.ARAP_Trans = Tool.GeneralHelper.NullToInt(dr["ARAP_TRANS"], 0);

                                vh.Status = Tool.GeneralHelper.NullToBool(dr["STATUS"], false);
                                vh.StatusPayment = Tool.GeneralHelper.NullToInt(dr["STS_PAYMENT"], 0);
                                vh.Description = Tool.GeneralHelper.NullToString(dr["DESC_ACFTRANH"]);

                                vh.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"]);
                                vh.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                                vh.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                                vh.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                                voucher.Add(vh);

                                lastVoucher = vh;
                            }
                            #endregion


                            #region Fill Detail
                            GLVoucherD vd = new GLVoucherD();
                            vd.SCode = new GLTable.SourceCode();
                            vd.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCODE"]);
                            vd.Voucher = Tool.GeneralHelper.NullToString(dr["VOUCHER"]);
                            vd.Counter = Tool.GeneralHelper.NullToInt(dr["COUNTER"], 0);

                            vd.VBranch = new GeneralTable.Branch();
                            vd.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            vd.COA = new GLTable.ChartOfAccount();
                            vd.COA.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            vd.CCy = new GeneralTable.Currency();
                            vd.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            vd.CashAccount = Tool.GeneralHelper.NullToString(dr["CASHACC"]);

                            vd.Dept = new GeneralTable.Department();
                            vd.Dept.DepartmentCode = Tool.GeneralHelper.NullToString(dr["DEPT"]);

                            vd.Proj = new GLTable.Project();
                            vd.Proj.ProjectCode = Tool.GeneralHelper.NullToString(dr["PROJ"]);

                            vd.DocumentNo = Tool.GeneralHelper.NullToString(dr["DOC_NO"]);
                            vd.Descrip = Tool.GeneralHelper.NullToString(dr["DESCRIP"]);
                            vd.CustSupp = Tool.GeneralHelper.NullToString(dr["C_S"]);
                            vd.Amount = Convert.ToDouble(dr["AMOUNT"]);
                            vd.UPD = Tool.GeneralHelper.NullToString(dr["UPD"]);
                            vd.InstrumentNo = Tool.GeneralHelper.NullToString(dr["INSTRUMENT_NO"]);
                            vd.MatchStatus = Tool.GeneralHelper.NullToBool(dr["MATCHSTATUS"], false);

                            if (lastVoucher.Detail == null)
                                lastVoucher.Detail = new List<GLVoucherD>();

                            lastVoucher.Detail.Add(vd);
                            #endregion
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return voucher;
        }
                
        public static GLTransARAPH GetVoucherWithDetail(string voucher, string sCode, string branchCode)
        {
            GLTransARAPH vh = new GLTransARAPH();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelARAPHeader";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@VoucherNo", System.Data.SqlDbType.VarChar, voucher);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@SCode", System.Data.SqlDbType.VarChar, sCode);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Account", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.VarChar, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        GLTransARAPH lastVoucher = null;

                        while (dr.Read())
                        {
                            #region Fill Header
                            if (lastVoucher == null ||
                                (Tool.GeneralHelper.NullToString(dr["VOUCHER"]) != lastVoucher.Voucher ||
                                Tool.GeneralHelper.NullToString(dr["SCODE"]) != lastVoucher.SCode.Code ||
                                Tool.GeneralHelper.NullToString(dr["BranchCode"]) != lastVoucher.VBranch.BranchCode))
                            {
                                vh.SCode = new GLTable.SourceCode();
                                vh.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCODE"]);

                                vh.VBranch = new GeneralTable.Branch();
                                vh.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                                vh.Voucher = Tool.GeneralHelper.NullToString(dr["VOUCHER"]);
                                vh.Entry_Date = Convert.ToDateTime(dr["ENT_DATE"]);
                                vh.TransDate = Convert.ToDateTime(dr["TRANS_DATE"]);
                                vh.PayTerm = Convert.ToInt32(dr["PAY_TERM"]);
                                vh.Reversed = Tool.GeneralHelper.NullToInt(dr["REVERSED"], Convert.ToInt32(string.IsNullOrEmpty(Tool.GeneralHelper.NullToString(dr["REV_VOUCHER"]))));
                                vh.ReversedVoucher = Tool.GeneralHelper.NullToString(dr["REV_VOUCHER"]);
                                vh.ARAP_Trans = Tool.GeneralHelper.NullToInt(dr["ARAP_TRANS"], 0);

                                vh.Status = Tool.GeneralHelper.NullToBool(dr["STATUS"], false);
                                vh.StatusPayment = Tool.GeneralHelper.NullToInt(dr["STS_PAYMENT"], 0);
                                vh.Description = Tool.GeneralHelper.NullToString(dr["DESC_ACFTRANH"]);

                                vh.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"]);
                                vh.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                                vh.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                                vh.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                                lastVoucher = vh;
                            }
                            #endregion

                            if (vh.Detail == null)
                                vh.Detail = new List<GLVoucherD>();

                            #region Fill Detail
                            GLVoucherD vd = new GLVoucherD();
                            vd.SCode = new GLTable.SourceCode();
                            vd.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCODE"]);
                            vd.Voucher = Tool.GeneralHelper.NullToString(dr["VOUCHER"]);
                            vd.Counter = Tool.GeneralHelper.NullToInt(dr["COUNTER"], 0);

                            vd.VBranch = new GeneralTable.Branch();
                            vd.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            vd.COA = new GLTable.ChartOfAccount();
                            vd.COA.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);
                            vd.COA.AccountName = Tool.GeneralHelper.NullToString(dr["ACCNAME"]);

                            vd.CCy = new GeneralTable.Currency();
                            vd.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            vd.CashAccount = Tool.GeneralHelper.NullToString(dr["CASHACC"]);

                            vd.Dept = new GeneralTable.Department();
                            vd.Dept.DepartmentCode = Tool.GeneralHelper.NullToString(dr["DEPT"]);

                            vd.Proj = new GLTable.Project();
                            vd.Proj.ProjectCode = Tool.GeneralHelper.NullToString(dr["PROJ"]);

                            vd.DocumentNo = Tool.GeneralHelper.NullToString(dr["DOC_NO"]);
                            vd.Descrip = Tool.GeneralHelper.NullToString(dr["DESCRIP"]);
                            vd.CustSupp = Tool.GeneralHelper.NullToString(dr["C_S"]);
                            vd.Amount = Convert.ToDouble(dr["AMOUNT"]);
                            vd.UPD = Tool.GeneralHelper.NullToString(dr["UPD"]);
                            vd.InstrumentNo = Tool.GeneralHelper.NullToString(dr["INSTRUMENT_NO"]);
                            vd.MatchStatus = Tool.GeneralHelper.NullToBool(dr["MATCHSTATUS"], false);

                            lastVoucher.Detail.Add(vd);
                            #endregion
                        }

                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return vh;
        }

        public static List<GLTransARAPH> GetVoucherWithDetail(string branchCode, string sCode, DateTime? dateFrom, DateTime? dateTo, int dateType,string upd)
        {
            List<GLTransARAPH> items = new List<GLTransARAPH>();
            int counter = 0;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "GLSelARAPHeader";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@VoucherNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                    db.AddParameter("@SCode", System.Data.SqlDbType.VarChar, string.IsNullOrWhiteSpace(sCode) ? DBNull.Value : (object)sCode);
                    db.AddParameter("@Period", System.Data.SqlDbType.VarChar, DBNull.Value);

                    db.AddParameter("@Account", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                    if (string.IsNullOrEmpty(upd))
                    {
                        db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, DBNull.Value);
                    }
                    else
                    {
                        db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, upd + "000000000");
                    }

                    switch (dateType)
                    {
                        case 0: // tidak ada tanggal from-to
                            db.AddParameter("@Type", System.Data.SqlDbType.VarChar, 7);
                            db.AddParameter("@dateFrom", System.Data.SqlDbType.DateTime, DBNull.Value);
                            db.AddParameter("@dateTo", System.Data.SqlDbType.DateTime, DBNull.Value);
                            break;
                        case 1: // Date From - to, tanggal transaction date
                            db.AddParameter("@Type", System.Data.SqlDbType.VarChar, 8);
                            db.AddParameter("@dateFrom", System.Data.SqlDbType.DateTime, dateFrom);
                            db.AddParameter("@dateTo", System.Data.SqlDbType.DateTime, dateTo);
                            break;
                        case 2: // date From - To, tanggal entry date
                            db.AddParameter("@Type", System.Data.SqlDbType.VarChar, 9);
                            db.AddParameter("@dateFrom", System.Data.SqlDbType.DateTime, dateFrom);
                            db.AddParameter("@dateTo", System.Data.SqlDbType.DateTime, dateTo);
                            break;
                        default:
                            db.AddParameter("@Type", System.Data.SqlDbType.VarChar, 7);
                            break;
                    }

                    db.Open();

                    db.ExecuteReader();
                    int i = 1;
                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            GLTransARAPH lastVoucher = null;

                            

                            while (dr.Read())
                            {
                                i++;

                                #region Fill Header
                                if (lastVoucher == null ||
                                    (Tool.GeneralHelper.NullToString(dr["VOUCHER"]) != lastVoucher.Voucher ||
                                    Tool.GeneralHelper.NullToString(dr["SCODE"]) != lastVoucher.SCode.Code ||
                                    Tool.GeneralHelper.NullToString(dr["BranchCode"]) != lastVoucher.VBranch.BranchCode))
                                {
                                    counter = counter + 1;
                                    GLTransARAPH vh = new GLTransARAPH();
                                    vh.SCode = new GLTable.SourceCode();
                                    vh.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCODE"]);

                                    vh.VBranch = new GeneralTable.Branch();
                                    vh.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                                    vh.Voucher = Tool.GeneralHelper.NullToString(dr["VOUCHER"]);
                                    vh.Entry_Date = Convert.ToDateTime(dr["ENT_DATE"]);
                                    vh.TransDate = Convert.ToDateTime(dr["TRANS_DATE"]);
                                    vh.PayTerm = Convert.ToInt32(dr["PAY_TERM"]);
                                    vh.Reversed = Tool.GeneralHelper.NullToInt(dr["REVERSED"], Convert.ToInt32(string.IsNullOrEmpty(Tool.GeneralHelper.NullToString(dr["REV_VOUCHER"]))));
                                    vh.ReversedVoucher = Tool.GeneralHelper.NullToString(dr["REV_VOUCHER"]);
                                    vh.ARAP_Trans = Tool.GeneralHelper.NullToInt(dr["ARAP_TRANS"], 0);

                                    vh.Status = Tool.GeneralHelper.NullToBool(dr["STATUS"], false);
                                    vh.StatusPayment = Tool.GeneralHelper.NullToInt(dr["STS_PAYMENT"], 0);
                                    vh.Description = Tool.GeneralHelper.NullToString(dr["DESC_ACFTRANH"]);

                                    vh.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"]);
                                    vh.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                                    vh.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                                    vh.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                                    items.Add(vh);
                                    lastVoucher = vh;
                                }
                                #endregion

                                #region Fill Detail
                                if (dr["DVOUCHER"] == DBNull.Value)
                                {
                                    lastVoucher.Detail = new List<GLVoucherD>();
                                    continue;
                                }

                                GLVoucherD vd = new GLVoucherD();
                                vd.SCode = new GLTable.SourceCode();
                                vd.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCODE"]);
                                vd.Voucher = Tool.GeneralHelper.NullToString(dr["VOUCHER"]);
                                vd.Counter = Tool.GeneralHelper.NullToInt(dr["COUNTER"], 0);

                                vd.VBranch = new GeneralTable.Branch();
                                vd.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                                vd.COA = new GLTable.ChartOfAccount();
                                vd.COA.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                                vd.CCy = new GeneralTable.Currency();
                                vd.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                                vd.CashAccount = Tool.GeneralHelper.NullToString(dr["CASHACC"]);

                                vd.Dept = new GeneralTable.Department();
                                vd.Dept.DepartmentCode = Tool.GeneralHelper.NullToString(dr["DEPT"]);

                                vd.Proj = new GLTable.Project();
                                vd.Proj.ProjectCode = Tool.GeneralHelper.NullToString(dr["PROJ"]);

                                vd.DocumentNo = Tool.GeneralHelper.NullToString(dr["DOC_NO"]);
                                vd.Descrip = Tool.GeneralHelper.NullToString(dr["DESCRIP"]);
                                vd.CustSupp = Tool.GeneralHelper.NullToString(dr["C_S"]);
                                vd.Amount = Tool.GeneralHelper.NullToDouble(dr["AMOUNT"], 0);
                                vd.UPD = Tool.GeneralHelper.NullToString(dr["UPD"]);
                                vd.InstrumentNo = Tool.GeneralHelper.NullToString(dr["INSTRUMENT_NO"]);
                                vd.MatchStatus = Tool.GeneralHelper.NullToBool(dr["MATCHSTATUS"], false);

                                if (lastVoucher.Detail == null)
                                    lastVoucher.Detail = new List<GLVoucherD>();

                                lastVoucher.Detail.Add(vd);
                                #endregion
                            }
                        }

                        if (!dr.IsClosed)
                            dr.Close();
                    }

                    db.Close();
                }
                catch (Exception ex)
                {

                }                
            }

            return items;
        }

        public int InsUpDel(int ExeCode, ref string newVoucherNo)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    if ((int)IDS.Tool.PageActivity.Insert == ExeCode)
                    {
                        if (IDS.GeneralTable.SerialNo.IsAutomatic("Voucher"))
                        {
                            Voucher = this.GenerateNewVoucher(VBranch?.BranchCode, SCode?.Code, TransDate);
                            newVoucherNo = Voucher;
                        }
                    }
                    else
                    {
                        newVoucherNo = Voucher;
                    }

                    db.CommandText = "GL_UpdAcfTranH";
                    db.DbCommand.CommandTimeout = 0;
                    db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SCode?.Code));
                    db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Voucher));
                    db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(VBranch.BranchCode));
                    db.AddParameter("@TRANS_DATE", System.Data.SqlDbType.DateTime, TransDate);
                    db.AddParameter("@PAY_TERM", System.Data.SqlDbType.TinyInt, 0);
                    db.AddParameter("@ARAP_TRANS", System.Data.SqlDbType.TinyInt, 0);
                    db.AddParameter("@OPERATORID", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(OperatorID));
                    db.AddParameter("@MATCHSTATUS", System.Data.SqlDbType.Bit, 0);
                    db.AddParameter("@STS_PAYMENT", System.Data.SqlDbType.TinyInt, 0);
                    db.AddParameter("@DESC_ACFTRANH", System.Data.SqlDbType.VarChar, Description);
                    db.AddParameter("@UTYPE", System.Data.SqlDbType.TinyInt, ExeCode);

                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();

                    db.BeginTransaction();
                    result = db.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                    }

                    if ((int)IDS.Tool.PageActivity.Edit == ExeCode)
                    {
                        db.CommandText = "GL_UpdAcfTranD";
                        db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SCode.Code));
                        db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Voucher));
                        db.AddParameter("@COUNTER", System.Data.SqlDbType.SmallInt, DBNull.Value);
                        db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(VBranch.BranchCode));
                        db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@CASHACC", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@PROJ", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DESCRIP", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@C_S", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@AMOUNT", System.Data.SqlDbType.Money, DBNull.Value);
                        db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@INSTRUMENT_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@MATCHSTATUS", System.Data.SqlDbType.Bit, DBNull.Value);
                        db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);

                        result = db.ExecuteNonQuery();

                        if (result <= 0)
                        {
                            throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                        }
                    }

                    db.CommandText = "GL_UpdAcfTranD";
                    db.DbCommand.CommandTimeout = 0;
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();

                    for (int i = 0; i < Detail.Count; i++)
                    {
                        db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SCode.Code));
                        db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Voucher));
                        db.AddParameter("@COUNTER", System.Data.SqlDbType.SmallInt, i + 1);
                        db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(VBranch.BranchCode));
                        db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].COA?.Account));
                        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].CCy?.CurrencyCode));
                        db.AddParameter("@CASHACC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].CashAccount));
                        db.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Dept?.DepartmentCode));
                        db.AddParameter("@PROJ", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].DocumentNo));
                        db.AddParameter("@DESCRIP", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Descrip));
                        db.AddParameter("@C_S", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@AMOUNT", System.Data.SqlDbType.Money, Detail[i].Amount);
                        db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, "0000000000");
                        db.AddParameter("@INSTRUMENT_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@MATCHSTATUS", System.Data.SqlDbType.Bit, 0);
                        db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);

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
                            throw new Exception("Voucher already exists, please try to save again.");
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
                    cmd.CommandText = "GL_UpdAcfTranH";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();
                    cmd.BeginTransaction();

                    string[] item;
                    string sCode = "";
                    string voucher = "";
                    string branchCode = "";

                    char separator = ';';

                    for (int i = 0; i < data.Length; i++)
                    {
                        item = data[i].Split(separator);

                        sCode = item[0];
                        voucher = item[1];
                        branchCode = item[2];                       

                        cmd.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(sCode));
                        cmd.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(voucher));
                        cmd.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(branchCode));
                        cmd.AddParameter("@TRANS_DATE", System.Data.SqlDbType.DateTime, DBNull.Value);
                        cmd.AddParameter("@PAY_TERM", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        cmd.AddParameter("@ARAP_TRANS", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        cmd.AddParameter("@OPERATORID", System.Data.SqlDbType.VarChar, DBNull.Value);
                        cmd.AddParameter("@MATCHSTATUS", System.Data.SqlDbType.Bit, DBNull.Value);
                        cmd.AddParameter("@STS_PAYMENT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        cmd.AddParameter("@DESC_ACFTRANH", System.Data.SqlDbType.VarChar, DBNull.Value);
                        cmd.AddParameter("@UTYPE", System.Data.SqlDbType.TinyInt, 3);
                        

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
                            throw new Exception("Area Code is already exists. Please choose other Area Code.");
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

        public string GenerateNewVoucher(string branchCode, string sCode, DateTime transDate)
        {
            string result = "";

            if (string.IsNullOrEmpty(sCode) || string.IsNullOrEmpty(branchCode) || (transDate == null || transDate.Date == DateTime.MinValue.Date))
            {
                return result;
            }

            int counter = 1;

            try
            {
                using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                {
                    //SQLText = " select (case when right(max(voucher),1) IN " +
                    //                        " ('A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P' " +
                    //                        " ,'Q','R','S','T','U','V','W','X','Y','Z') then " +
                    //                        " left(right(max(voucher),4) ,3) " +
                    //                   " else " +
                    //                        " right(max(voucher),3) end) as res " +
                    //          " from ACFTRANH " +
                    //          " where scode='" + Scode + "' AND BranchCode='" + BranchCode + "'" +
                    //          " and CONVERT(VARCHAR(6),TRANS_DATE,112)='" + Period + "' " +
                    //          " AND LEFT(Voucher,4) = RIGHT('" + Period + "',4)";

                    try
                    {
                        db.CommandText = "SELECT " +
                                " (CASE WHEN RIGHT(MAX(VOUCHER),1) IN ('A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z') THEN " +
                                " LEFT(RIGHT(MAX(VOUCHER), 4), 3) " +
                                " ELSE RIGHT(MAX(VOUCHER), 3) END) AS res " +
                            " FROM ACFTRANH " +
                            " WHERE SCODE = @SCODE AND BranchCode = @BranchCode " +
                            " AND CONVERT(VARCHAR(6),TRANS_DATE,112) = @Period " +
                            " AND LEFT(Voucher, 4) = @voucherPeriod";
                        db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SCode?.Code));
                        db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(VBranch.BranchCode));
                        db.AddParameter("@Period", System.Data.SqlDbType.VarChar, transDate.ToPeriod());
                        db.AddParameter("@voucherPeriod", System.Data.SqlDbType.VarChar, transDate.ToPeriod().Right(4));
                        db.CommandType = System.Data.CommandType.Text;
                        db.Open();

                        db.ExecuteReader();

                        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    if (!(dr[0] is DBNull))
                                    {
                                        counter = Convert.ToInt32(dr[0]);
                                        counter++;
                                    }
                                }
                            }

                            dr.Close();
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        db.Close();
                        GC.Collect();
                    }      
                }
            }
            catch 
            {
                throw;
            }

            result = TransDate.ToString("yyMM") + counter.ToString("000");

            return result;
        }

        public bool CheckPostedVoucherExists(string[] data)
        {
            if (data == null || data.Length == 0)
                return false;

            bool exists = false;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "IF(EXISTS (SELECT VOUCHER FROM ACFTRAND WHERE VOUCHER = @voucher AND SCODE = @scode AND BranchCode = @branchCode AND UPD = '1000000000')) " +
                        " BEGIN SELECT 1 END " +
                        " ELSE BEGIN SELECT 0 END";
                    db.CommandType = System.Data.CommandType.Text;
                    db.Open();

                    string[] dataSplit;
                    string sCode = "";
                    string branchCode = "";
                    string voucher = "";
                    char separator = ';';

                    for (int i = 0; i < data.Length; i++)
                    {
                        dataSplit = data[i].Split(separator);
                        sCode = dataSplit[0];
                        voucher = dataSplit[1];
                        branchCode = dataSplit[2];

                        db.AddParameter("@voucher", System.Data.SqlDbType.VarChar, sCode);
                        db.AddParameter("@scode", System.Data.SqlDbType.VarChar, branchCode);
                        db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, voucher);

                        exists = Convert.ToBoolean(db.ExecuteScalar());

                        if (exists)
                            break;
                    }
                }
                catch
                {
                    throw;
                }         
                finally
                {
                    db.Close();
                }
            }

            return exists;
        }
    }
}