using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDS.Tool;
using System.ComponentModel.DataAnnotations;

namespace IDS.GLTransaction
{
    public class CashBankH
    {
        public string CashBankNumber { get; set; }
        public IDS.Tool.GLCashBank Type { get; set; }
        public GeneralTable.Branch Branch { get; set; }
        public GeneralTable.Supplier Supplier { get; set; }
        [Display(Name = "Account")]
        public IDS.GLTable.ChartOfAccount Account { get; set; }

        [Display(Name = "Ccy")]
        public IDS.GeneralTable.Currency Ccy { get; set; }
        public string RefNo { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "{0: " + IDS.Tool.GlobalVariable.DEFAULT_DATE_FORMAT + "}")]
        [Display(Name = "Cash/Bank Date")]
        public DateTime CBDate { get; set; }

        public decimal CBAmount { get; set; }

        public int PayTerm { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal OutstandingAmount { get; set; }

        public int StatusCB { get; set; }

        [Display(Name = "PPh No")]
        public string PPhNo { get; set; }

        [Display(Name = "PPh Amount")]
        public decimal PPhAmount { get; set; }

        [Display(Name = "PPh Date Receive")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime PPhDateReceive { get; set; }

        [Display(Name = "Payment Status")]
        public bool PaymentStatus { get; set; }

        [Display(Name = "Pay Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime PayDate { get; set; }

        [Display(Name = "Voucher")]
        [MaxLength(12), StringLength(12)]
        public string Voucher { get; set; }

        [Display(Name = "Exchange Rate")]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Equiv Amount")]
        public decimal EquivAmount { get; set; }

        [Display(Name = "Status AR/AP")]
        public bool StatusARAP { get; set; }

        [Display(Name = "Bukti Potong PPh")]
        [MaxLength(30), StringLength(30)]
        public string BuktiPotongPPh { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "{0: " + IDS.Tool.GlobalVariable.DEFAULT_DATE_FORMAT + "}")]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public string Approval1 { get; set; }
        public string Approval2 { get; set; }
        public string Approval3 { get; set; }
        public string ProcessedBy { get; set; }

        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "{0: " + IDS.Tool.GlobalVariable.DEFAULT_DATE_FORMAT + "}")]
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "{0: " + IDS.Tool.GlobalVariable.DEFAULT_DATE_FORMAT + "}")]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public List<CashBankD> CBDetail { get; set; }

        public CashBankH()
        {

        }

        public static List<CashBankH> GetCashBankH(string period)
        {
            List<CashBankH> list = new List<CashBankH>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelCashBankH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            CashBankH cbH = new CashBankH();
                            cbH.CashBankNumber = IDS.Tool.GeneralHelper.NullToString(dr["CashBankNumber"],"");
                            cbH.RefNo = IDS.Tool.GeneralHelper.NullToString(dr["RefNo"], "");
                            cbH.CBDate = Convert.ToDateTime(dr["CashBankDate"]);
                            cbH.Branch = new GeneralTable.Branch();
                            cbH.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            cbH.Supplier = new GeneralTable.Supplier();
                            cbH.Supplier.SupCode = IDS.Tool.GeneralHelper.NullToString(dr["SupCode"], "");
                            cbH.Supplier.SupName = IDS.Tool.GeneralHelper.NullToString(dr["Name"], "");

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            //cbH.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            cbH.CBAmount = Convert.ToDecimal(dr["CashBankAmount"]);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            //cbH.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            //cbH.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            cbH.Account = new GLTable.ChartOfAccount();
                            cbH.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            cbH.Ccy = new GeneralTable.Currency();
                            cbH.Ccy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            cbH.Type = (IDS.Tool.GLCashBank)Convert.ToInt16(dr["Type"]);
                            cbH.Description = Tool.GeneralHelper.NullToString(dr["Description"], "");
                            cbH.Approval1 = Tool.GeneralHelper.NullToString(dr["Approval1"], "");
                            cbH.Approval2 = Tool.GeneralHelper.NullToString(dr["Approval2"], "");
                            cbH.Approval3 = Tool.GeneralHelper.NullToString(dr["Approval3"], "");
                            cbH.ProcessedBy = Tool.GeneralHelper.NullToString(dr["ProcessedBy"], "");
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            cbH.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"], "");
                            cbH.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            cbH.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"], "");
                            cbH.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(cbH);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<CashBankH> GetCashBankHExcel(string period,string cbno)
        {
            List<CashBankH> list = new List<CashBankH>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelCashBankH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@cbno", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(cbno));
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 6);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            CashBankH cbH = new CashBankH();
                            cbH.CashBankNumber = IDS.Tool.GeneralHelper.NullToString(dr["CashBankNumber"], "");
                            cbH.RefNo = IDS.Tool.GeneralHelper.NullToString(dr["RefNo"], "");
                            cbH.CBDate = Convert.ToDateTime(dr["CashBankDate"]);
                            cbH.Branch = new GeneralTable.Branch();
                            cbH.Branch.BranchCode = dr["branchcode"] as string;
                            cbH.Branch.BranchName = IDS.Tool.GeneralHelper.NullToString(dr["BranchName"],"");
                            cbH.Branch.Address1 = IDS.Tool.GeneralHelper.NullToString(dr["BranchAddress"], "");
                            cbH.Branch.Phone1 = IDS.Tool.GeneralHelper.NullToString(dr["PhoneBranch"], "");
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            cbH.Supplier = new GeneralTable.Supplier();
                            cbH.Supplier.SupCode = IDS.Tool.GeneralHelper.NullToString(dr["SupCode"], "");
                            cbH.Supplier.SupName = IDS.Tool.GeneralHelper.NullToString(dr["Name"], "");
                            cbH.Supplier.BenBankAcc = IDS.Tool.GeneralHelper.NullToString(dr["BankAcc"], "");
                            cbH.Supplier.BenBank = IDS.Tool.GeneralHelper.NullToString(dr["Bank"], "");

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            //cbH.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            cbH.CBAmount = Convert.ToDecimal(dr["CashBankAmount"]);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            //cbH.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            //cbH.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            cbH.Account = new GLTable.ChartOfAccount();
                            cbH.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            cbH.Ccy = new GeneralTable.Currency();
                            cbH.Ccy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            cbH.Type = (IDS.Tool.GLCashBank)Convert.ToInt16(dr["Type"]);
                            cbH.Description = Tool.GeneralHelper.NullToString(dr["Description"], "");
                            cbH.Approval1 = Tool.GeneralHelper.NullToString(dr["Approval1"], "");
                            cbH.Approval2 = Tool.GeneralHelper.NullToString(dr["Approval2"], "");
                            cbH.Approval3 = Tool.GeneralHelper.NullToString(dr["Approval3"], "");
                            cbH.ProcessedBy = Tool.GeneralHelper.NullToString(dr["ProcessedBy"], "");
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            cbH.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"], "");
                            cbH.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            cbH.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"], "");
                            cbH.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(cbH);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<CashBankH> GetApprovalList(string period)
        {
            List<CashBankH> list = new List<CashBankH>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelCashBankH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            CashBankH cbH = new CashBankH();
                            cbH.CashBankNumber = IDS.Tool.GeneralHelper.NullToString(dr["CashBankNumber"], "");
                            cbH.RefNo = IDS.Tool.GeneralHelper.NullToString(dr["RefNo"], "");
                            cbH.CBDate = Convert.ToDateTime(dr["CashBankDate"]);
                            cbH.Branch = new GeneralTable.Branch();
                            cbH.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            cbH.Supplier = new GeneralTable.Supplier();
                            cbH.Supplier.SupCode = IDS.Tool.GeneralHelper.NullToString(dr["SupCode"], "");
                            cbH.Supplier.SupName = IDS.Tool.GeneralHelper.NullToString(dr["Name"], "");

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            //cbH.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            cbH.CBAmount = Convert.ToDecimal(dr["CashBankAmount"]);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            //cbH.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            //cbH.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            cbH.Account = new GLTable.ChartOfAccount();
                            cbH.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            cbH.Ccy = new GeneralTable.Currency();
                            cbH.Ccy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            cbH.Type = (IDS.Tool.GLCashBank)Convert.ToInt16(dr["Type"]);
                            cbH.Description = Tool.GeneralHelper.NullToString(dr["Description"],"");
                            cbH.Approval1 = Tool.GeneralHelper.NullToString(dr["Approval1"], "");
                            cbH.Approval2 = Tool.GeneralHelper.NullToString(dr["Approval2"], "");
                            cbH.Approval3 = Tool.GeneralHelper.NullToString(dr["Approval3"], "");
                            cbH.ProcessedBy = Tool.GeneralHelper.NullToString(dr["ProcessedBy"], "");
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            cbH.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"], "");
                            cbH.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            cbH.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"],"");
                            cbH.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(cbH);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static CashBankH GetCashBankHWithDetail(string cbNo)
        {
            CashBankH cbH = new CashBankH();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelCashBankH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@cbNo", System.Data.SqlDbType.VarChar, cbNo);
                db.AddParameter("@Type", System.Data.SqlDbType.Int, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            #region Fill Header
                            cbH.CashBankNumber = IDS.Tool.GeneralHelper.NullToString(dr["CashBankNumber"], "");
                            cbH.Type = (Tool.GLCashBank)Convert.ToInt16(dr["cbType"]);
                            cbH.RefNo = IDS.Tool.GeneralHelper.NullToString(dr["RefNo"], "");
                            cbH.CBDate = Convert.ToDateTime(dr["CashBankDate"]);
                            cbH.Branch = new GeneralTable.Branch();
                            cbH.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            cbH.Supplier = new GeneralTable.Supplier();
                            cbH.Supplier.SupCode = IDS.Tool.GeneralHelper.NullToString(dr["SupCode"], "");
                            cbH.Supplier.SupName = IDS.Tool.GeneralHelper.NullToString(dr["Name"], "");

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            //cbH.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            cbH.CBAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashBankAmount"],0);
                            cbH.EquivAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["EquivAmt"], 0);
                            cbH.OutstandingAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["OutstandingAmount"], 0);
                            cbH.ExchangeRate = IDS.Tool.GeneralHelper.NullToDecimal(dr["ExchangeRate"], 0);
                            cbH.PayTerm = IDS.Tool.GeneralHelper.NullToInt(dr["TermOfPayment"], 0);
                            cbH.StatusCB = IDS.Tool.GeneralHelper.NullToInt(dr["StatusCB"],0);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            //cbH.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            //cbH.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            cbH.Account = new GLTable.ChartOfAccount();
                            cbH.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            cbH.Ccy = new GeneralTable.Currency();
                            cbH.Ccy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            //cbH.Type = (IDS.Tool.GLCashBank)Convert.ToInt16(dr["Type"]);
                            cbH.Description = Tool.GeneralHelper.NullToString(dr["Description"], "");
                            cbH.Approval1 = Tool.GeneralHelper.NullToString(dr["Approval1"], "");
                            cbH.Approval2 = Tool.GeneralHelper.NullToString(dr["Approval2"], "");
                            cbH.Approval3 = Tool.GeneralHelper.NullToString(dr["Approval3"], "");
                            cbH.ProcessedBy = Tool.GeneralHelper.NullToString(dr["ProcessedBy"], "");
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            cbH.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"], "");
                            cbH.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            cbH.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"], "");
                            cbH.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            #endregion

                            if (cbH.CBDetail == null)
                                cbH.CBDetail = new List<CashBankD>();

                            #region Fill Detail
                            CashBankD cbDtl = new CashBankD();
                            cbDtl.Counter = Tool.GeneralHelper.NullToInt(dr["Counter"], 0);
                            cbDtl.SubCounter = Tool.GeneralHelper.NullToInt(dr["SubCounter"], 0);
                            cbDtl.Remark = Tool.GeneralHelper.NullToString(dr["Remark"], "");
                            cbDtl.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            cbDtl.Type = Tool.GeneralHelper.NullToInt(dr["Type"], 0);

                            cbH.CBDetail.Add(cbDtl);
                            #endregion
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return cbH;
        }

        public int InsUpDelCB(int ExecCode, ref string cbNo)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    //if (ExecCode == (int)IDS.Tool.PageActivity.Insert)
                    //{
                    if (ExecCode == (int)IDS.Tool.PageActivity.Insert)
                    {
                        CashBankNumber = GetNextCBNumber(CBDate);    
                    }

                        cbNo = CashBankNumber;
                        cmd.CommandText = "GLCashBankH";
                        cmd.AddParameter("@cbNo", System.Data.SqlDbType.VarChar, CashBankNumber);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, (int)Type);
                        cmd.AddParameter("@cbDate", System.Data.SqlDbType.DateTime, CBDate);
                        cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                        cmd.AddParameter("@sup", System.Data.SqlDbType.VarChar, Supplier.SupCode);
                        cmd.AddParameter("@cbAmount", System.Data.SqlDbType.Money, CBAmount);
                        cmd.AddParameter("@PaidAmount", System.Data.SqlDbType.Money, PaidAmount);
                        cmd.AddParameter("@OutstandingAmount", System.Data.SqlDbType.Money, OutstandingAmount);
                        cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, EquivAmount);
                        //cmd.AddParameter("@Status", System.Data.SqlDbType.Bit, StatusCB);
                        cmd.AddParameter("@payTerm", System.Data.SqlDbType.Int, PayTerm);
                        cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Ccy.CurrencyCode);
                        cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                        cmd.AddParameter("@exchRate", System.Data.SqlDbType.Money, ExchangeRate);
                        cmd.AddParameter("@description", System.Data.SqlDbType.VarChar, Description);
                        cmd.AddParameter("@operator", System.Data.SqlDbType.VarChar, OperatorID);
                        cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, (int)ExecCode);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Open();

                        cmd.BeginTransaction();
                        result = cmd.ExecuteNonQuery();

                        //if (result > 0)
                        //{
                        //    cmd.CommandText = "GLSelCashBankH";
                        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 4);
                        //    cmd.Open();

                        //    cmd.ExecuteReader();

                            //using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
                            //{
                            //    if (dr.HasRows)
                            //    {
                            //        while (dr.Read())
                            //        {
                            //            CashBankNumber = Tool.GeneralHelper.NullToInt(dr["CashBankNumber"], 0);
                            //        }
                            //    }

                            //    if (!dr.IsClosed)
                            //        dr.Close();
                            //}
                        //}

                        IDS.GLTable.ACFARAP arap = new GLTable.ACFARAP();
                        arap.RP = "P";
                        arap.Acc = Account;
                        arap.CCy = Ccy;
                        arap.Supplier = new GeneralTable.Supplier();
                        arap.Supplier.SupCode = IDS.Tool.GeneralHelper.NullToString(Supplier.SupCode);
                        arap.DocNo = CashBankNumber;
                        arap.Branch = Branch;
                        arap.DocDate = CBDate;
                        arap.ReceivedDate = CBDate;
                        arap.PaymentTerm = PayTerm;
                        arap.Amount = CBAmount;
                        arap.ExchangeRate = ExchangeRate;
                        arap.Remark = Description;
                        arap.EquivAmt = EquivAmount;
                        arap.OperatorID = OperatorID;

                        arap.InsUpDelACFARAPSupWithOld(1, cmd);

                        if (ExecCode == 2)
                        {
                            cmd.CommandText = "GLCashBankD";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.AddParameter("@cbNo", System.Data.SqlDbType.VarChar, CashBankNumber);
                            cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, 3);
                            cmd.Open();
                            result = cmd.ExecuteNonQuery();
                        }

                        int seqNo = 1;
                        foreach (var cbDtl in CBDetail)
                        {
                            cmd.CommandText = "GLCashBankD";
                            cmd.AddParameter("@cbNo", System.Data.SqlDbType.VarChar, CashBankNumber);
                            cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                            cmd.AddParameter("@sup", System.Data.SqlDbType.VarChar, Supplier.SupCode);
                            cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Ccy.CurrencyCode);
                            cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                            cmd.AddParameter("@counter", System.Data.SqlDbType.Int, cbDtl.Counter);
                            cmd.AddParameter("@subcounter", System.Data.SqlDbType.Int, cbDtl.SubCounter);
                            cmd.AddParameter("@seqNoARAP", System.Data.SqlDbType.SmallInt, seqNo);
                            cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, cbDtl.Type);
                            cmd.AddParameter("@remark", System.Data.SqlDbType.VarChar, cbDtl.Remark);
                            cmd.AddParameter("@amount", System.Data.SqlDbType.Money, cbDtl.Amount);
                            cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, 1);
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Open();

                            result = cmd.ExecuteNonQuery();
                            seqNo++;
                        }

                        cmd.CommitTransaction();
                    //}
                    //else if (ExecCode == (int)IDS.Tool.PageActivity.Edit)
                    //{
                    //    cmd.CommitTransaction();
                    //}

                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Cash/Bank Number is already exists. Please choose other Cash/Bank.");
                        case 547:
                            throw new Exception("Data can not be delete while data used for reference.");
                        default:
                            throw;
                    }
                }
                catch (Exception e)
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

        public int InsUpDelCB(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    string cbProcessed = "";
                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GLSelCashBankH";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@cbNo", System.Data.SqlDbType.VarChar, data[i]);
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 4);
                        cmd.Open();

                        cmd.ExecuteReader();

                        using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    if (Tool.GeneralHelper.NullToDecimal(dr["PaidAmount"], 0) != 0 || Tool.GeneralHelper.NullToInt(dr["StatusCB"], 0) != 0)
                                    {
                                        cbProcessed += data[i] + ", ";
                                        //throw new Exception("Some Cash/Bank can not be deleted. Because Its already paid.");
                                    }
                                    else
                                    {

                                    }
                                }

                            }

                            if (!dr.IsClosed)
                                dr.Close();
                        }
                    }

                    if (!string.IsNullOrEmpty(cbProcessed))
                    {
                        throw new Exception("Cash/Bank Number : "+cbProcessed+" can not be deleted. Because Its already processed.");
                    }

                    cmd.CommandText = "GLCashBankH";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GLCashBankH";
                        cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@cbNo", System.Data.SqlDbType.VarChar, data[i]);
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

        public static string SetApproval(string userID,string cbNo)
        {
            string result = "";

            if (userID == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLCashBankH";
                    cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, 4);
                    cmd.AddParameter("@cbNo", System.Data.SqlDbType.VarChar, cbNo);
                    cmd.AddParameter("@operator", System.Data.SqlDbType.VarChar, userID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();
                    cmd.BeginTransaction();
                    int rlst = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();

                    if (rlst > 0)
                    {
                        result = "Approval Success";
                    }
                    else
                    {
                        result = "Approval Failed";
                    }
                    
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 547:
                            throw new Exception(sex.Message);
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

        public static string GetNextCBNumber(DateTime cbDate)
        {
            object CurrentNumber;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                cmd.CommandText = "GLSelCashBankH";
                cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                cmd.AddParameter("@period", System.Data.SqlDbType.VarChar, cbDate.Year.ToString().Substring(2, 2) + cbDate.Month.ToString("00"));
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Open();
                CurrentNumber = Tool.GeneralHelper.NullToInt(cmd.ExecuteScalar(), 0);
                cmd.Close();

                string NextNumber = "CB/"+ cbDate.Year.ToString().Substring(2, 2) + cbDate.Month.ToString("00") +"/"+ (Tool.GeneralHelper.NullToInt(CurrentNumber, 0) + 1).ToString("00000");
                //string NextNumber = (Tool.GeneralHelper.NullToInt(CurrentNumber, 0) + 1).ToString("00000") + "/" + Tool.DateTimeExtension.GetMonthRomawi(nextPeriod.Substring(4, 2)) + "/" + nextPeriod.Substring(0, 4);

                return NextNumber;
            }
        }

        //public void DownloadExcel()
        //{
        //    List<IDS.GLTransaction.CashBankH> cbH = IDS.GLTransaction.CashBankH.GetCashBankH("202206");
        //    NorthwindEntities entities = new NorthwindEntities();
        //    dt.Columns.AddRange(new DataColumn[4] { new DataColumn("CustomerId"),
        //                                    new DataColumn("ContactName"),
        //                                    new DataColumn("City"),
        //                                    new DataColumn("Country") });

        //    var customers = from customer in entities.Customers.Take(10)
        //                    select customer;

        //    foreach (var customer in customers)
        //    {
        //        dt.Rows.Add(customer.CustomerID, customer.ContactName, customer.City, customer.Country);
        //    }

        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt);
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);
        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
        //        }
        //    }
        //}
    }
}
