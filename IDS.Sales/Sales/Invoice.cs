using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class Invoice
    {
        [Display(Name = "Invoice Number")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Invoice Number")]
        [MaxLength(20)]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Ref No")]
        [MaxLength(20)]
        public string RefNo { get; set; }

        [Display(Name = "Invoice Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Invoice Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime InvoiceTransDate { get; set; }

        [Display(Name = "Branch Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch Name is required")]
        public IDS.GeneralTable.Branch Branch { get; set; }

        [Display(Name = "Project Code")]
        public IDS.Sales.CustProject CustProject { get; set; }

        [Display(Name = "Customer Code")]
        public IDS.GeneralTable.Customer Cust { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Invoice Amount")]
        public decimal InvoiceAmount { get; set; }

        [Display(Name = "Invoice Total")]
        public decimal InvoiceTotal { get; set; }

        [Display(Name = "Term of Payment")]
        public int TermOfPayment { get; set; }

        [Display(Name = "Contract No")]
        public string ContractNo { get; set; }

        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; }

        [Display(Name = "Outstanding Amount")]
        public decimal OutstandingAmount { get; set; }

        [Display(Name = "Status Invoice")]
        public int StatusInv { get; set; }

        [Display(Name = "Is PPh")]
        [MaxLength(10), StringLength(10)]
        public string IsPPh { get; set; }

        [Display(Name = "PPh Percentage")]
        public decimal PPhPercentage { get; set; }

        [Display(Name = "PPh No")]
        public string PPhNo { get; set; }

        [Display(Name = "PPh Amount")]
        public decimal PPhAmount { get; set; }

        [Display(Name = "PPh Date Receive")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime PPhDateReceive { get; set; }

        [Display(Name = "Is PPn")]
        public bool IsPPn { get; set; }

        [Display(Name = "PPn Amount")]
        public decimal PPnAmount { get; set; }

        [Display(Name = "PPn No")]
        public string PPnNo { get; set; }

        [Display(Name = "Tax Status")]
        public bool TaxStatus { get; set; }

        [Display(Name = "WH Tax (%)")]
        public double WHTaxPercent { get; set; }

        [Display(Name = "WH Tax")]
        public bool WHTax { get; set; }

        public string WHTaxCode { get; set; }

        [Display(Name = "Payment Status")]
        public bool PaymentStatus { get; set; }

        [Display(Name = "Pay Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime PayDate { get; set; }

        [Display(Name = "Voucher")]
        [MaxLength(12), StringLength(12)]
        public string Voucher { get; set; }

        [Display(Name = "Account")]
        public IDS.GLTable.ChartOfAccount Account { get; set; }

        [Display(Name = "CCy")]
        public IDS.GeneralTable.Currency CCy { get; set; }

        [Display(Name = "Exchange Rate")]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Equiv Amount")]
        public decimal EquivAmount { get; set; }

        [Display(Name = "Allocation Amount")]
        public decimal AllocAmount { get; set; }

        [Display(Name = "Status AR/AP")]
        public bool StatusARAP { get; set; }

        [Display(Name = "Invoice Role")]
        public int InvoiceRole { get; set; }

        [Display(Name = "Bukti Potong PPh")]
        [MaxLength(30), StringLength(30)]
        public string BuktiPotongPPh { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Grand Total")]
        public double GrandTotal { get; set; }

        public string RP { get; set; }

        [Display(Name = "Entry User")]
        [MaxLength(20), StringLength(20)]
        public string EntryUser { get; set; }

        public TaxNumber TaxNumber { get; set; }

        [Display(Name = "Entry Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        [MaxLength(20), StringLength(20)]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public List<InvoiceDetail> InvDetail { get; set; }

        public Invoice()
        {

        }

        public static List<Invoice> GetInvoice()
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesProjInvoice";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            //invoice.RefNo = dr["RefNo"] as string;

                            
                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            invoice.Cust.CUSTName = dr["Name"] as string;

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            invoice.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            invoice.DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]);
                            invoice.InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.IsPPh= Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            invoice.PPhPercentage = Convert.ToDecimal(dr["PPhPercentage"]);
                            invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            invoice.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            invoice.PPnAmount = Convert.ToDecimal(dr["PPnAmount"]);
                            invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            //invoice.Account= new GLTable.ChartOfAccount();
                            //invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            invoice.Description = dr["Description"] as string;
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<Invoice> GetInvoice(string invNo, DateTime payDate, string cust)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "loadInvoice";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@invoiceno", System.Data.SqlDbType.VarChar, invNo);
                db.AddParameter("@paymentNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@paymentdate", System.Data.SqlDbType.DateTime, payDate);
                db.AddParameter("@vendor", System.Data.SqlDbType.VarChar, cust);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.Account = new GLTable.ChartOfAccount();
                            invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["Account"]);
                            invoice.Account.AccountName = Tool.GeneralHelper.NullToString(dr["AccName"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            invoice.InvoiceAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Amount"],0);

                            invoice.OutstandingAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Outstanding"], 0);
                            invoice.AllocAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Allocation"], 0);
                            invoice.ExchangeRate = IDS.Tool.GeneralHelper.NullToDecimal(dr["Rate"], 0);
                            invoice.EquivAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["EquivAmount"], 0);
                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.BUMNTax = new GLTable.ChartOfAccount();
                            invoice.Cust.BUMNTax.Account = Tool.GeneralHelper.NullToString(dr["BUMN"]);
                            invoice.Cust.BUMNTax.AccountName = Tool.GeneralHelper.NullToString(dr["BUMNName"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<Invoice> GetTaxInvoice(string invNo, DateTime payDate, string cust)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "loadPPH23";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@invoiceno", System.Data.SqlDbType.VarChar, invNo);
                db.AddParameter("@paymentNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@paymentdate", System.Data.SqlDbType.DateTime, payDate);
                db.AddParameter("@vendor", System.Data.SqlDbType.VarChar, cust);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.Account = new GLTable.ChartOfAccount();
                            invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["Account"]);
                            //invoice.Account.AccountName = Tool.GeneralHelper.NullToString(dr["AccName"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            invoice.InvoiceAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            //invoice.PPnAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPnAmount"], 0);
                            //invoice.PPhAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPhAmount"], 0);
                            invoice.OutstandingAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Outstanding"], 0);
                            invoice.AllocAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Allocation"], 0);
                            invoice.ExchangeRate = IDS.Tool.GeneralHelper.NullToDecimal(dr["Rate"], 0);
                            invoice.EquivAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["EquivAmount"], 0);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<Invoice> GetInvoice(string period)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesProjInvoice";
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
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            //invoice.RefNo = dr["RefNo"] as string;
                            
                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            invoice.Cust.CUSTName = dr["Name"] as string;

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            invoice.InvoiceTotal = Tool.GeneralHelper.NullToDecimal(dr["InvoiceTotal"], 0);
                            invoice.DiscountAmount = Tool.GeneralHelper.NullToDecimal(dr["DiscountAmount"], 0);
                            invoice.InvoiceAmount = Tool.GeneralHelper.NullToDecimal(dr["InvoiceAmount"], 0);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            invoice.PaidAmount = Tool.GeneralHelper.NullToDecimal(dr["PaidAmount"], 0);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.IsPPh = Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            invoice.PPhPercentage = string.IsNullOrEmpty(dr["PPhPercentage"].ToString()) ? 0 : Convert.ToDecimal(dr["PPhPercentage"]);
                            invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            invoice.PPhAmount = string.IsNullOrEmpty(dr["PPhAmount"].ToString()) ? 0 : Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            invoice.PPnAmount = string.IsNullOrEmpty(dr["PPnAmount"].ToString()) ? 0 : Convert.ToDecimal(dr["PPnAmount"]);
                            invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            //invoice.Account= new GLTable.ChartOfAccount();
                            //invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            invoice.Description = dr["Description"] as string;
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<Invoice> GetSalesInvoice()
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesProjInvoice";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            //invoice.RefNo = dr["RefNo"] as string;
                            
                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            invoice.Cust.CUSTName = dr["Name"] as string;

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            invoice.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            invoice.DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]);
                            invoice.InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.IsPPh = Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            invoice.PPhPercentage = Convert.ToDecimal(dr["PPhPercentage"]);
                            invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            invoice.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            invoice.PPnAmount = Convert.ToDecimal(dr["PPnAmount"]);
                            invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            //invoice.Account= new GLTable.ChartOfAccount();
                            //invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            invoice.Description = dr["Description"] as string;
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<Invoice> GetSalesInvoice(string period)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
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
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            //invoice.RefNo = dr["RefNo"] as string;
                            
                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            invoice.Cust.CUSTName = dr["Name"] as string;
                            invoice.Cust.Address1 = dr["ADDR_1"] as string;
                            invoice.Cust.Phone = dr["PHONE"] as string;
                            invoice.Cust.Fax = dr["FAX"] as string;

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            invoice.InvoiceTotal = Tool.GeneralHelper.NullToDecimal(dr["InvoiceTotal"], 0);
                            invoice.DiscountAmount = Tool.GeneralHelper.NullToDecimal(dr["DiscountAmount"], 0);
                            invoice.InvoiceAmount = Tool.GeneralHelper.NullToDecimal(dr["InvoiceAmount"], 0);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            invoice.PaidAmount = Tool.GeneralHelper.NullToDecimal(dr["PaidAmount"], 0);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.IsPPh = Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            invoice.PPhPercentage = string.IsNullOrEmpty(dr["PPhPercentage"].ToString()) ? 0 : Convert.ToDecimal(dr["PPhPercentage"]);
                            invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            invoice.PPhAmount = string.IsNullOrEmpty(dr["PPhAmount"].ToString()) ? 0 : Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            invoice.PPnAmount = string.IsNullOrEmpty(dr["PPnAmount"].ToString()) ? 0 : Convert.ToDecimal(dr["PPnAmount"]);
                            invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            //invoice.Account= new GLTable.ChartOfAccount();
                            //invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            invoice.Description = dr["Description"] as string;
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<Invoice> GetSalesVoidInvoice(bool chkFilterDate,string cust,string inv, DateTime from, DateTime to)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "VoidInvList";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@pinit", System.Data.SqlDbType.TinyInt, chkFilterDate == true ? 1 : 0);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@invoice", System.Data.SqlDbType.VarChar, inv);
                db.AddParameter("@from", System.Data.SqlDbType.DateTime, from);
                db.AddParameter("@to", System.Data.SqlDbType.DateTime, to);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            invoice.RefNo = dr["RefNo"] as string;

                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            //invoice.Cust.CUSTName = dr["Name"] as string;

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            //invoice.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            invoice.DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]);
                            invoice.InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.IsPPh = Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            //invoice.PPhPercentage = Convert.ToDecimal(dr["PPhPercentage"]);
                            //invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            //invoice.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            //invoice.PPnAmount = Convert.ToDecimal(dr["PPnAmount"]);
                            //invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            //invoice.Account= new GLTable.ChartOfAccount();
                            //invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            invoice.Description = dr["Description"] as string;
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<Invoice> GetSalesVoidInvoiceExportFaktur(bool chkFilterDate, string cust, string inv, DateTime from, DateTime to)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"VoidInvExportFakturList";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@pinit", System.Data.SqlDbType.TinyInt, chkFilterDate == true ? 1 : 0);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@invoice", System.Data.SqlDbType.VarChar, inv);
                db.AddParameter("@from", System.Data.SqlDbType.DateTime, from);
                db.AddParameter("@to", System.Data.SqlDbType.DateTime, to);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            invoice.RefNo = dr["RefNo"] as string;

                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            //invoice.Cust.CUSTName = dr["Name"] as string;

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            //invoice.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            invoice.DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]);
                            invoice.InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.IsPPh = Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            //invoice.PPhPercentage = Convert.ToDecimal(dr["PPhPercentage"]);
                            //invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            //invoice.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            //invoice.PPnAmount = Convert.ToDecimal(dr["PPnAmount"]);
                            //invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            //invoice.Account= new GLTable.ChartOfAccount();
                            //invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            invoice.Description = dr["Description"] as string;
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static Invoice GetSalesInvoiceWithDetail(string invNo)
        {
            Invoice invoice = new Invoice();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@inv", System.Data.SqlDbType.VarChar, invNo);
                db.AddParameter("@Type", System.Data.SqlDbType.VarChar, 8);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            #region Fill Header
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            invoice.InvoiceTransDate = Convert.ToDateTime(dr["RECEIVEDDATE"]);

                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            invoice.Cust.CUSTName = dr["Name"] as string;
                            invoice.Cust.Address1 = Tool.GeneralHelper.NullToString(dr["ADDR_1"]);
                            invoice.Cust.Phone = Tool.GeneralHelper.NullToString(dr["Phone"]);
                            invoice.Cust.Fax = Tool.GeneralHelper.NullToString(dr["Fax"]);

                            invoice.InvoiceTotal = Tool.GeneralHelper.NullToDecimal(dr["InvoiceTotal"],0);
                            invoice.DiscountAmount = Tool.GeneralHelper.NullToDecimal(dr["DiscountAmount"],0);
                            invoice.InvoiceAmount = Tool.GeneralHelper.NullToDecimal(dr["InvoiceAmount"],0);
                            invoice.IsPPh = Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            if (!string.IsNullOrEmpty(invoice.IsPPh))
                            {
                                invoice.WHTax = true;
                            }
                            invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            invoice.TaxStatus= Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            invoice.PPhPercentage = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPhPercentage"],0);
                            invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            invoice.PPhAmount = Tool.GeneralHelper.NullToDecimal(dr["PPhAmount"],0);
                            invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["TermOfPayment"],0);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            invoice.Account = new GLTable.ChartOfAccount();
                            invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["Acc"]);

                            invoice.ContractNo = Tool.GeneralHelper.NullToString(dr["INSTRUMENT_NO"]);
                            invoice.RefNo = Tool.GeneralHelper.NullToString(dr["RefNo"]);
                            invoice.WHTaxCode = Tool.GeneralHelper.NullToString(dr["isPPh"])+"-"+ Tool.GeneralHelper.NullToDecimal(dr["PPhPercentage"],0).ToString("0.00");
                            invoice.Description = dr["Description"] as string;

                            invoice.TaxNumber = new TaxNumber();
                            invoice.TaxNumber.SerialNo = Tool.GeneralHelper.NullToString(dr["SerialNo"],"");
                            invoice.TaxNumber.TransType = Tool.GeneralHelper.NullToString(dr["TransType"],"");
                            invoice.TaxNumber.JenisFaktur = Tool.GeneralHelper.NullToString(dr["JenisFaktur"], "");

                            invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            #endregion

                            if (invoice.InvDetail == null)
                                invoice.InvDetail = new List<InvoiceDetail>();

                            #region Fill Detail
                            InvoiceDetail invoiceDtl = new InvoiceDetail();
                            invoiceDtl.Counter = Tool.GeneralHelper.NullToInt(dr["Counter"], 0);
                            invoiceDtl.SubCounter = Tool.GeneralHelper.NullToInt(dr["SubCounter"], 0);
                            invoiceDtl.SubAmount = Tool.GeneralHelper.NullToString(dr["SubAmount"], "");
                            invoiceDtl.Remark = Tool.GeneralHelper.NullToString(dr["Remark"], "");
                            invoiceDtl.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            invoiceDtl.TaxInvoice = Tool.GeneralHelper.NullToBool(dr["TaxInvoice"]);
                            

                            invoice.InvDetail.Add(invoiceDtl);
                            #endregion
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return invoice;
        }

        public static Invoice GetTotal(string invNo)
        {
            Invoice invoice = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesProjInvoice";
                db.AddParameter("@invNo", System.Data.SqlDbType.VarChar, invNo);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        invoice = new Invoice();
                        invoice.GrandTotal = Tool.GeneralHelper.NullToDouble(dr["Total"],0);
                        invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return invoice;
        }

        public static Invoice GetTotal(string invNo, bool IncludeText)
        {
            Invoice invoice = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesProjInvoice";
                db.AddParameter("@invNo", System.Data.SqlDbType.VarChar, invNo);
                db.AddParameter("@IncludeTax", System.Data.SqlDbType.Bit, IncludeText);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        invoice = new Invoice();
                        invoice.GrandTotal = Tool.GeneralHelper.NullToDouble(dr["Total"], 0);
                        invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return invoice;
        }

        public static System.Data.DataTable GetInvoiceHExportCSV(string invNo)
        {
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"select 'FK' FK,tblTaxNumberList.TransType fgTrans,tblTaxNumberList.JenisFaktur fgType,REPLACE(REPLACE(tblTaxNumberList.SerialNo,'-',''),'.','') 
                TaxNumber,MONTH(slsInvH.InvoiceDate) MasaPajak, YEAR(InvoiceDate) TahunPajak,Convert(varchar(10),InvoiceDate,103) TglFaktur,
                CASE ISNULL(c.nik,'') WHEN '' THEN '000000000000000' ELSE REPLACE(REPLACE(NPWP,'.',''),'-','') END Npwp,c.Name, 
                c.ADDR_1+' '+ADDR_2 +' '+ADDR_3 CustAddress,InvoiceAmount Dpp,
                PPnAmount TaxAmount,'0' Ppn,CASE WHEN tblTaxNumberList.TransType='08' 
                OR tblTaxNumberList.TransType='07' THEN '4' ELSE '' END IdTambahan,0 fgUangMuka,0 umDpp, 0 umPPn,0 umPPnbm,slsInvH.InvoiceNumber
                Referensi, '' AS KODE_DOKUMEN_PENDUKUNG from slsInvH Inner join ACFCUST c On slsInvH.CustCode=c.CUST 
                Inner Join tblTaxNumberList On slsInvH.InvoiceNumber=tblTaxNumberList.DocumentNo 
                Where tblTaxNumberList.Cancel=0 AND slsInvH.InvoiceNumber=@InvNo";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo);
                db.Open();

                return db.GetDataTable();
            }
        }

        public static System.Data.DataTable GetInvoiceDExportCSV(string invNo)
        {
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"SELECT 'OF' oft, '' kodeObj,slsInvD.Remark Description,Amount UnitPrice,
                1 Qty,slsInvD.Amount TotalPriceEq,0.0 Diskon,slsInvD.Amount Dpp,
                ROUND((slsInvH.PPnPercentage * slsInvD.Amount)/100,2, 1) PPn ,0 tarifppnbm,0.0 ppnBm from slsInvH 
                Inner Join slsInvD 
                On slsInvH.InvoiceNumber=slsInvD.InvoiceNumber Inner join ACFCUST On slsInvH.CustCode=ACFCUST.CUST 
                WHERE slsInvH.InvoiceNumber=@InvNo";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo);
                db.Open();

                return db.GetDataTable();
            }
        }

        public static System.Data.DataTable GetSysparExportCSV()
        {
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"select 'FAPR' LT,Syspar.Name,[Address-1]+' '+[Address-2] as Address,'Admin',[Address-3] as Name, 
                REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(19), CONVERT(DATETIME, getdate(), 112), 126), '-', ''), 'T', ''), ':', ''),'' 
                from SYSPAR";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                return db.GetDataTable();
            }
        }

        public static System.Data.DataTable GetVoidInvoiceHExportCSV(string invNo)
        {
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"select 'RK' RK,CASE ISNULL(ACFCUST.NPWP,'') WHEN '' THEN '000000000000000' ELSE REPLACE(REPLACE(NPWP,'.',''),'-','') END Npwp,
                                ACFCUST.Name,'01' fgTrans,'0' fgType,tblTaxNumberList.SerialNo TaxNumber,Convert(varchar(10),InvoiceDate,103) TglFaktur,
                                slsInvH.InvoiceNumber,Convert(varchar(10),InvoiceDate,103) TglFaktur,MONTH(slsInvH.InvoiceDate) MasaPajak, YEAR(InvoiceDate) TahunPajak, 
                                SLSInvH.InvoiceAmount Dpp,SLSInvH.PPnAmount TaxAmount,
                                '0' PpnBM from slsInvH
                                Inner join ACFCUST On slsInvH.CustCode=ACFCUST.CUST 
                                INNER JOIN tblTaxNumberList ON slsInvH.InvoiceNumber=tblTaxNumberList.DocumentNo 
                                Where slsInvH.InvoiceNumber = @InvNo  AND tblTaxNumberList.Cancel=1";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo);
                db.Open();

                return db.GetDataTable();
            }
        }

        public string CheckPPnNo(string ppnNo)
        {
            string result = "";

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesProjInvoice";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@PPnNo", System.Data.SqlDbType.VarChar, ppnNo.Replace(",", "."));
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        int i = 0;
                        while (dr.Read())
                        {
                            if (i==0)
                            {
                                result = dr["InvoiceNumber"] as string;
                            }
                            i++;
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return result;
        }

        public int UpdatePPnNo(string invNo, string ppnNo)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SelSalesProjInvoice";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 4);
                    cmd.AddParameter("@PPnNo", System.Data.SqlDbType.VarChar, ppnNo);
                    cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, invNo);
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
                            throw new Exception("Invoice is already exists. Please choose other Invoice.");
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

        public static List<IDS.Sales.Invoice> GetInvoicenNumber(string xperiod, string xcust, string xtype)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, xperiod);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, xcust);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, System.Convert.ToInt16(xtype));
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            //Console.WriteLine(invoice.InvoiceNumber);
                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static string GetNextInvoiceNumber(string nextPeriod,string year)
        {
            int WillBeProcess = 0;
            object CurrentNumber;

            WillBeProcess = IDS.Sales.CustProject.GetNextPeriod(nextPeriod);

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                cmd.CommandText = "SelSalesInvoiceList";
                cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                cmd.AddParameter("@period", System.Data.SqlDbType.VarChar, year);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Open();
                CurrentNumber = Tool.GeneralHelper.NullToInt(cmd.ExecuteScalar(), 0);
                cmd.Close();

                string NextNumber = (Tool.GeneralHelper.NullToInt(CurrentNumber, 0) + 1).ToString("00000") + "/" + Tool.DateTimeExtension.GetMonthRomawi(nextPeriod.Substring(4, 2)) + "/" + nextPeriod.Substring(0, 4);

                return NextNumber;
            }
        }

        public static bool CheckInvNo(string invNo)
        {
            bool result = false;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@inv", System.Data.SqlDbType.VarChar, invNo);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 6);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        result = true;
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return result;
        }

        public int InsUpDelInvoice(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    if (ExecCode == (int)IDS.Tool.PageActivity.Insert)
                    {
                        foreach (var invDtl in InvDetail)
                        {
                            if ((string.IsNullOrEmpty(invDtl.Remark) ? "" : invDtl.Remark.ToUpper()) != "PPN")
                            {
                                InvoiceAmount = InvoiceAmount + invDtl.Amount;
                            }
                        }

                        cmd.CommandText = "SELECT [dbo].GenerateNewInvoiceNumber(@invPeriod)";
                        cmd.AddParameter("@invPeriod", System.Data.SqlDbType.VarChar, Tool.DateTimeExtension.ToPeriod(InvoiceDate));
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Open();

                        object nextInvNum = cmd.ExecuteScalar(); // 00211  /I/2022
                        nextInvNum = Tool.GeneralHelper.NullToInt(InvoiceNumber, 0).ToString("00000") + nextInvNum.ToString().Substring(5);

                        cmd.CommandText = "SlsSalesH";
                        cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, nextInvNum.ToString());
                        cmd.AddParameter("@invDate", System.Data.SqlDbType.DateTime, InvoiceDate);
                        cmd.AddParameter("@invTransDate", System.Data.SqlDbType.DateTime, InvoiceTransDate);
                        //if (rbTax.Checked == true)
                        //    db.AddParameter("@invAmount", SqlDbType.Money, (totalAmount + totalAmount * 0.1));
                        //else
                        cmd.AddParameter("@invAmount", System.Data.SqlDbType.Money, InvoiceAmount);
                        cmd.AddParameter("@payTerm", System.Data.SqlDbType.Int, TermOfPayment);
                        cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                        cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                        cmd.AddParameter("@contractNo", System.Data.SqlDbType.VarChar, ContractNo);
                        cmd.AddParameter("@refNo", System.Data.SqlDbType.VarChar, RefNo);
                        cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, Cust.CUSTCode);
                        cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                        //cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, Tool.GeneralHelper.NullToDecimal((InvoiceAmount * ExchangeRate) + Math.Round(InvoiceAmount * 0.1m * ExchangeRate), 0));
                        cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, Tool.GeneralHelper.NullToDecimal((InvoiceAmount * ExchangeRate) + Math.Round(InvoiceAmount * (IDS.GeneralTable.Syspar.GetInstance().VAT / 100) * ExchangeRate), 0));
                        cmd.AddParameter("@oldInvAmount", System.Data.SqlDbType.Money, 0);

                        cmd.AddParameter("@description", System.Data.SqlDbType.Text, Description);
                        //db.AddParameter("@discountAmount", SqlDbType.Money, Convert.ToDouble(txtDiscount.Value));
                        cmd.AddParameter("@isPPn", System.Data.SqlDbType.Bit, IsPPn);
                        if (IsPPn)
                        {
                            //cmd.AddParameter("@taxAmount", System.Data.SqlDbType.Money, Math.Round((InvoiceAmount * 0.1m)));
                            cmd.AddParameter("@taxAmount", System.Data.SqlDbType.Money, Math.Round((InvoiceAmount * (IDS.GeneralTable.Syspar.GetInstance().VAT / 100))));
                            cmd.AddParameter("@ppnpercentage", System.Data.SqlDbType.Money, IDS.GeneralTable.Syspar.GetInstance().VAT);
                        }
                        else
                        {
                            cmd.AddParameter("@taxAmount", System.Data.SqlDbType.Money, 0);
                        }

                        if (WHTax)
                        {
                            cmd.AddParameter("@WHTaxCode", System.Data.SqlDbType.VarChar, WHTaxCode);
                            cmd.AddParameter("@whtaxPercentage", System.Data.SqlDbType.Money, WHTaxPercent);
                        }
                        else
                        {
                            cmd.AddParameter("@whtaxPercentage", System.Data.SqlDbType.Money, 0);
                            cmd.AddParameter("@WHTaxCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                        }
                        
                        cmd.AddParameter("@operator", System.Data.SqlDbType.VarChar, OperatorID);
                        cmd.AddParameter("@TaxNumber", System.Data.SqlDbType.VarChar, TaxNumber.SerialNo);
                        cmd.AddParameter("@TaxTransType", System.Data.SqlDbType.VarChar, TaxNumber.TransType);
                        cmd.AddParameter("@JenisFaktur", System.Data.SqlDbType.VarChar, TaxNumber.JenisFaktur);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Open();

                        cmd.BeginTransaction();
                        result = cmd.ExecuteNonQuery();

                        

                        if (TaxStatus)
                        {
                            foreach (var invDtl in InvDetail)
                            {
                                //var Listsummary = InvDetail.GroupBy(item => item.Counter).Select(t => new { ID = t.Key, Value = t.Sum(u => u.Amount) }).ToList();
                                //decimal summary = Listsummary[invDtl.Counter].Value;
                                //decimal summary = InvDetail.Sum(item => item.Amount);
                                //decimal summary = invDtl.Amount;
                                decimal summary = 0;
                                for (int i = 0; i < InvDetail.Count; i++)
                                {
                                    if (invDtl.Counter == InvDetail[i].Counter)
                                    {
                                        summary += InvDetail[i].Amount;
                                    }
                                }
                                //myList.Where(x => x.RecType == 1).Sum(x => x.Income);
                                cmd.CommandText = "SlsSalesD";
                                cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, nextInvNum.ToString());
                                cmd.AddParameter("@counter", System.Data.SqlDbType.Int, invDtl.Counter);
                                cmd.AddParameter("@subseq", System.Data.SqlDbType.Int, invDtl.SubCounter);
                                cmd.AddParameter("@remark", System.Data.SqlDbType.VarChar, invDtl.Remark);
                                cmd.AddParameter("@subamount", System.Data.SqlDbType.VarChar, invDtl.SubAmount);
                                cmd.AddParameter("@amount", System.Data.SqlDbType.Money, invDtl.Amount);
                                cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, invDtl.Amount * ExchangeRate);
                                cmd.AddParameter("@summary", System.Data.SqlDbType.Money, summary);

                                cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                                cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                                cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                                cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, Cust.CUSTCode);
                                cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                                cmd.AddParameter("@intaxinvoice", System.Data.SqlDbType.Bit, invDtl.TaxInvoice);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Open();

                                result = cmd.ExecuteNonQuery();
                            }

                            //int noppn = dt.Select("SubSeqNo=0").Length + 1; belum
                            cmd.CommandText = "SlsSalesD";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, nextInvNum.ToString());
                            cmd.AddParameter("@counter", System.Data.SqlDbType.Int, InvDetail.Where(x => x.SubCounter.ToString().Contains("0")).Count() + 1);
                            cmd.AddParameter("@subseq", System.Data.SqlDbType.Int, 0);
                            cmd.AddParameter("@subamount", System.Data.SqlDbType.VarChar, "");
                            cmd.AddParameter("@remark", System.Data.SqlDbType.VarChar, "PPN");//(IDS.GeneralTable.Syspar.GetInstance().VAT / 100)
                            //cmd.AddParameter("@amount", System.Data.SqlDbType.Money, Math.Round((InvoiceAmount * 0.1m)));
                            cmd.AddParameter("@amount", System.Data.SqlDbType.Money, Math.Round((InvoiceAmount * (IDS.GeneralTable.Syspar.GetInstance().VAT / 100))));
                            //cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, Math.Round(InvoiceAmount * 0.1m) * ExchangeRate);
                            cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, Math.Round(InvoiceAmount * (IDS.GeneralTable.Syspar.GetInstance().VAT / 100)) * ExchangeRate);

                            cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                            cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                            cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                            cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, Cust.CUSTCode);
                            cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                            cmd.AddParameter("@intaxinvoice", System.Data.SqlDbType.Bit, false);

                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            foreach (var invDtl in InvDetail)
                            {
                                //decimal summary = InvDetail.Sum(item => item.Amount);
                                decimal summary = 0;
                                for (int i = 0; i < InvDetail.Count; i++)
                                {
                                    if (invDtl.Counter == InvDetail[i].Counter)
                                    {
                                        summary += InvDetail[i].Amount;
                                    }
                                }
                                cmd.CommandText = "SlsSalesD";
                                cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, nextInvNum.ToString());
                                cmd.AddParameter("@counter", System.Data.SqlDbType.Int, invDtl.Counter);
                                cmd.AddParameter("@subseq", System.Data.SqlDbType.Int, invDtl.SubCounter);
                                cmd.AddParameter("@remark", System.Data.SqlDbType.VarChar, invDtl.Remark);
                                cmd.AddParameter("@subamount", System.Data.SqlDbType.VarChar, invDtl.SubAmount);
                                cmd.AddParameter("@amount", System.Data.SqlDbType.Money, invDtl.Amount);
                                cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, invDtl.Amount * ExchangeRate);
                                cmd.AddParameter("@summary", System.Data.SqlDbType.Money, summary);

                                cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                                cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                                cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                                cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, Cust.CUSTCode);
                                cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                                cmd.AddParameter("@intaxinvoice", System.Data.SqlDbType.Bit, invDtl.TaxInvoice);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Open();

                                result = cmd.ExecuteNonQuery();
                            }
                        }
                        cmd.CommitTransaction();
                    }
                    else if (ExecCode == (int)IDS.Tool.PageActivity.Edit)
                    {
                        foreach (var invDtl in InvDetail)
                        {
                            if (invDtl.Remark.ToUpper() != "PPN")
                            {
                                InvoiceAmount = InvoiceAmount + invDtl.Amount;
                            }
                        }

                        decimal oldInvoiceAmount = 0;
                        decimal oldDiscAmount = 0;
                        decimal oldPPnAmount = 0;
                        string oldCustcode = string.Empty;

                        cmd.CommandText = "SelSalesInvoiceList";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@inv", System.Data.SqlDbType.VarChar, InvoiceNumber);
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 6);
                        cmd.Open();

                        cmd.ExecuteReader();

                        using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    oldInvoiceAmount = Tool.GeneralHelper.NullToDecimal(dr["InvoiceAmount"],0);
                                    oldDiscAmount = Tool.GeneralHelper.NullToDecimal(dr["DiscountAmount"], 0);
                                    oldPPnAmount = Tool.GeneralHelper.NullToDecimal(dr["PPnAmount"], 0);
                                    oldCustcode = Tool.GeneralHelper.NullToString(dr["CustCode"]);
                                }
                            }

                            if (!dr.IsClosed)
                                dr.Close();
                        }

                        cmd.CommandText = "SalesCustOutstanding";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@InvAmount", System.Data.SqlDbType.Decimal, oldInvoiceAmount-oldDiscAmount+oldPPnAmount);
                        cmd.AddParameter("@period", System.Data.SqlDbType.VarChar, Tool.DateTimeExtension.ToPeriod(InvoiceDate));
                        cmd.AddParameter("@CustCODE", System.Data.SqlDbType.VarChar, oldCustcode);
                        cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 5);
                        cmd.Open();
                        cmd.BeginTransaction();
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SlsSalesD";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, InvoiceNumber);
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                        cmd.Open();
                        result = cmd.ExecuteNonQuery();

                        cmd.CommandText = "SlsSalesH";
                        cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, InvoiceNumber);
                        cmd.AddParameter("@invDate", System.Data.SqlDbType.DateTime, InvoiceDate);
                        cmd.AddParameter("@invTransDate", System.Data.SqlDbType.DateTime, InvoiceTransDate);
                        //if (rbTax.Checked == true)
                        //    db.AddParameter("@invAmount", SqlDbType.Money, (totalAmount + totalAmount * 0.1));
                        //else
                        cmd.AddParameter("@invAmount", System.Data.SqlDbType.Money, InvoiceAmount);
                        cmd.AddParameter("@payTerm", System.Data.SqlDbType.Int, TermOfPayment);
                        cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                        cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                        cmd.AddParameter("@contractNo", System.Data.SqlDbType.VarChar, ContractNo);
                        cmd.AddParameter("@refNo", System.Data.SqlDbType.VarChar, RefNo);
                        cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, Cust.CUSTCode);
                        cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                        //cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, Tool.GeneralHelper.NullToDecimal((InvoiceAmount * ExchangeRate) + Math.Round(InvoiceAmount * 0.1m * ExchangeRate), 0));
                        cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, Tool.GeneralHelper.NullToDecimal((InvoiceAmount * ExchangeRate) + Math.Round(InvoiceAmount * (IDS.GeneralTable.Syspar.GetInstance().VAT / 100) * ExchangeRate), 0));
                        cmd.AddParameter("@oldInvAmount", System.Data.SqlDbType.Money, 0);

                        cmd.AddParameter("@description", System.Data.SqlDbType.Text, Description);
                        //db.AddParameter("@discountAmount", SqlDbType.Money, Convert.ToDouble(txtDiscount.Value));
                        cmd.AddParameter("@isPPn", System.Data.SqlDbType.Bit, IsPPn);
                        if (IsPPn)
                        {
                            //cmd.AddParameter("@taxAmount", System.Data.SqlDbType.Money, Math.Round((InvoiceAmount * 0.1m)));
                            cmd.AddParameter("@taxAmount", System.Data.SqlDbType.Money, Math.Round((InvoiceAmount * (IDS.GeneralTable.Syspar.GetInstance().VAT / 100))));
                            cmd.AddParameter("@ppnpercentage", System.Data.SqlDbType.Money, IDS.GeneralTable.Syspar.GetInstance().VAT);
                        }
                        else
                        {
                            cmd.AddParameter("@taxAmount", System.Data.SqlDbType.Money, 0);
                            cmd.AddParameter("@ppnpercentage", System.Data.SqlDbType.Money, 0);
                        }

                        if (WHTax)
                        {
                            cmd.AddParameter("@WHTaxCode", System.Data.SqlDbType.VarChar, WHTaxCode);
                            cmd.AddParameter("@whtaxPercentage", System.Data.SqlDbType.Money, WHTaxPercent);
                        }
                        else
                        {
                            cmd.AddParameter("@whtaxPercentage", System.Data.SqlDbType.Money, 0);
                            cmd.AddParameter("@WHTaxCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                        }

                        cmd.AddParameter("@TaxNumber", System.Data.SqlDbType.VarChar, TaxNumber.SerialNo);
                        cmd.AddParameter("@TaxTransType", System.Data.SqlDbType.VarChar, TaxNumber.TransType);
                        cmd.AddParameter("@JenisFaktur", System.Data.SqlDbType.VarChar, TaxNumber.JenisFaktur);
                        cmd.AddParameter("@operator", System.Data.SqlDbType.VarChar, OperatorID);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Open();

                        result = cmd.ExecuteNonQuery();

                        //decimal summary = InvDetail.Sum(item => item.Amount);
                        decimal summary = 0;

                        if (TaxStatus)
                        {
                            foreach (var invDtl in InvDetail)
                            {
                                summary = 0;
                                for (int i = 0; i < InvDetail.Count; i++)
                                {
                                    if (invDtl.Counter == InvDetail[i].Counter)
                                    {
                                        summary += InvDetail[i].Amount;
                                    }
                                }

                                if (invDtl.Remark != "PPN")
                                {
                                    cmd.CommandText = "SlsSalesD";
                                    cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, InvoiceNumber);
                                    cmd.AddParameter("@counter", System.Data.SqlDbType.Int, invDtl.Counter);
                                    cmd.AddParameter("@subseq", System.Data.SqlDbType.Int, invDtl.SubCounter);
                                    cmd.AddParameter("@remark", System.Data.SqlDbType.VarChar, invDtl.Remark);
                                    cmd.AddParameter("@subamount", System.Data.SqlDbType.VarChar, invDtl.SubAmount);
                                    cmd.AddParameter("@amount", System.Data.SqlDbType.Money, invDtl.Amount);
                                    cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, invDtl.Amount * ExchangeRate);
                                    cmd.AddParameter("@summary", System.Data.SqlDbType.Money, summary);

                                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                                    cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                                    cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                                    cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, Cust.CUSTCode);
                                    cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                                    cmd.AddParameter("@intaxinvoice", System.Data.SqlDbType.Bit, invDtl.TaxInvoice);
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.Open();

                                    result = cmd.ExecuteNonQuery();
                                }
                            }

                            //int noppn = dt.Select("SubSeqNo=0").Length + 1; belum
                            cmd.CommandText = "SlsSalesD";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, InvoiceNumber);
                            cmd.AddParameter("@counter", System.Data.SqlDbType.Int, InvDetail.Where(x => x.SubCounter.ToString().Contains("0")).Count() + 1);
                            cmd.AddParameter("@subseq", System.Data.SqlDbType.Int, 0);
                            cmd.AddParameter("@subamount", System.Data.SqlDbType.VarChar, "");
                            cmd.AddParameter("@remark", System.Data.SqlDbType.VarChar, "PPN");//(IDS.GeneralTable.Syspar.GetInstance().VAT / 100)
                            //cmd.AddParameter("@amount", System.Data.SqlDbType.Money, Math.Round((InvoiceAmount * 0.1m)));
                            cmd.AddParameter("@amount", System.Data.SqlDbType.Money, Math.Round((InvoiceAmount * (IDS.GeneralTable.Syspar.GetInstance().VAT / 100))));
                            //cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, Math.Round(InvoiceAmount * 0.1m) * ExchangeRate);
                            cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, Math.Round(InvoiceAmount * (IDS.GeneralTable.Syspar.GetInstance().VAT / 100)) * ExchangeRate);

                            cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                            cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                            cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                            cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, Cust.CUSTCode);
                            cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                            cmd.AddParameter("@intaxinvoice", System.Data.SqlDbType.Bit, false);

                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            foreach (var invDtl in InvDetail)
                            {
                                if (!invDtl.Remark.Contains("PPN"))
                                {
                                    cmd.CommandText = "SlsSalesD";
                                    cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, InvoiceNumber);
                                    cmd.AddParameter("@counter", System.Data.SqlDbType.Int, invDtl.Counter);
                                    cmd.AddParameter("@subseq", System.Data.SqlDbType.Int, invDtl.SubCounter);
                                    cmd.AddParameter("@remark", System.Data.SqlDbType.VarChar, invDtl.Remark);
                                    cmd.AddParameter("@subamount", System.Data.SqlDbType.VarChar, invDtl.SubAmount);
                                    cmd.AddParameter("@amount", System.Data.SqlDbType.Money, invDtl.Amount);
                                    cmd.AddParameter("@eqAmount", System.Data.SqlDbType.Money, invDtl.Amount * ExchangeRate);
                                    cmd.AddParameter("@summary", System.Data.SqlDbType.Money, summary);

                                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                                    cmd.AddParameter("@acc", System.Data.SqlDbType.VarChar, Account.Account);
                                    cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                                    cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, Cust.CUSTCode);
                                    cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                                    cmd.AddParameter("@intaxinvoice", System.Data.SqlDbType.Bit, invDtl.TaxInvoice);
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.Open();

                                    result = cmd.ExecuteNonQuery();
                                }
                                
                            }
                        }
                        cmd.CommitTransaction();
                        IDS.Sales.CustomerOutstanding.Recalculate(Branch.BranchCode, Cust.CUSTCode, CCy.CurrencyCode, InvoiceDate, false,OperatorID);
                    }
                    
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Invoice Number is already exists. Please choose other Invoice Number.");
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

        public int InsUpDel(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SelSalesInvoiceList";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();
                    cmd.BeginTransaction();

                    string[] item;
                    string invNo = "";
                    decimal invAmt = 0;
                    string ccy = "";
                    string custCode = "";
                    string branchCode = "";

                    char separator = ';';

                    for (int i = 0; i < data.Length; i++)
                    {
                        item = data[i].Split(separator);

                        invNo = item[0];
                        branchCode = item[1];
                        custCode = item[2];
                        ccy = item[3];
                        invAmt = Tool.GeneralHelper.NullToDecimal(item[4],0);

                        cmd.CommandText = "SelSalesInvoiceList";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@inv", System.Data.SqlDbType.VarChar, invNo);
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 7);
                        cmd.Open();

                        cmd.ExecuteReader();

                        using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    if (Tool.GeneralHelper.NullToDecimal(dr["PaidAmount"], 0) != 0 || Tool.GeneralHelper.NullToInt(dr["StatusINV"], 0) != 0)
                                    {
                                        throw new Exception("Some Invoice can not be deleted. Because Its already paid.");
                                    }
                                    else
                                    {
                                        
                                    }
                                }
                                
                            }

                            if (!dr.IsClosed)
                                dr.Close();
                        }

                        cmd.CommandText = "SalesCustOutstanding";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@InvAmount", System.Data.SqlDbType.Decimal, invAmt);
                        cmd.AddParameter("@CustCODE", System.Data.SqlDbType.VarChar, custCode);
                        cmd.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo);
                        cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 4);
                        cmd.Open();
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SalesInvoiceList";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo);
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                        cmd.Open();
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SalesACFARAP";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@docNo", System.Data.SqlDbType.VarChar, invNo);
                        cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                        cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, custCode);
                        cmd.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                        cmd.Open();
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "UpdSUBACFARAP";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@docNo", System.Data.SqlDbType.VarChar, invNo);
                        cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                        cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, custCode);
                        cmd.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                        cmd.Open();
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
                            throw new Exception("Invoice Number is already exists. Please choose other Invoice Number.");
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

        public static string GetNewInvoiceNo(IDS.DataAccess.SqlServer cmd)
        {
            string result = "";



            return result;
        }

        public static List<System.Web.Mvc.SelectListItem> GetInvoiceNoForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> invNos = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 11);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        invNos = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem invNo = new System.Web.Mvc.SelectListItem();
                            invNo.Value = dr["InvoiceNumber"] as string;
                            invNo.Text = dr["InvoiceNumber"] as string;

                            invNos.Add(invNo);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return invNos;
        }

        public static List<System.Web.Mvc.SelectListItem> GetInvoiceNoForDataSource(string cust)
        {
            List<System.Web.Mvc.SelectListItem> invNos = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 13);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(cust));
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        invNos = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem invNo = new System.Web.Mvc.SelectListItem();
                            invNo.Value = dr["InvoiceNumber"] as string;
                            invNo.Text = dr["InvoiceNumber"] as string;

                            invNos.Add(invNo);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return invNos;
        }

        public static List<System.Web.Mvc.SelectListItem> GetInvoiceVoidNoForDataSource(string cust)
        {
            List<System.Web.Mvc.SelectListItem> invNos = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 15);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(cust));
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        invNos = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem invNo = new System.Web.Mvc.SelectListItem();
                            invNo.Value = dr["InvoiceNumber"] as string;
                            invNo.Text = dr["InvoiceNumber"] as string;

                            invNos.Add(invNo);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return invNos;
        }
        //list.Insert(0, new System.Web.Mvc.SelectListItem { Text = "All", Value = "" });
        public static List<System.Web.Mvc.SelectListItem> GetInvoiceNoForDataSource(string branch,string cust, string period)
        {
            List<System.Web.Mvc.SelectListItem> invNos = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 12);
                db.AddParameter("@Cust", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(cust));
                db.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        invNos = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem invNo = new System.Web.Mvc.SelectListItem();
                            invNo.Value = dr["InvoiceNumber"] as string;
                            invNo.Text = dr["InvoiceNumber"] as string;

                            invNos.Add(invNo);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return invNos;
        }

        public static List<System.Web.Mvc.SelectListItem> GetInvoiceNoForDataSource(string branch, string cust, string period,bool WithAll)
        {
            List<System.Web.Mvc.SelectListItem> invNos = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 12);
                db.AddParameter("@Cust", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(cust));
                db.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        invNos = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem invNo = new System.Web.Mvc.SelectListItem();
                            invNo.Value = dr["InvoiceNumber"] as string;
                            invNo.Text = dr["InvoiceNumber"] as string;

                            invNos.Add(invNo);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
            if (WithAll)
            {
                invNos.Insert(0, new System.Web.Mvc.SelectListItem { Text = "All", Value = "" });
            }
            
            return invNos;
        }

        public static List<System.Web.Mvc.SelectListItem> GetInvoiceNoForDataSource(int type, string branch,DateTime docdate, string custPrin)
        {
            List<System.Web.Mvc.SelectListItem> invNos = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "PayAllInvList";
                db.AddParameter("@pInit", System.Data.SqlDbType.TinyInt, type);
                db.AddParameter("@pBranchCode", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@docDate", System.Data.SqlDbType.DateTime, docdate);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        invNos = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem invNo = new System.Web.Mvc.SelectListItem();
                            if (!string.IsNullOrEmpty(custPrin))
                            {
                                if (dr["CUST_PRIN"] as string == custPrin)
                                {
                                    invNo.Value = dr["DOC_NO"] as string;
                                    invNo.Text = dr["DOC_NO"] as string;

                                    invNos.Add(invNo);
                                }
                            }
                            
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return invNos;
        }

        public static int RecalculatePaidAmount(string serialNo, string ccy,string cust, decimal paid)
        {
            int result = 0;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"UPDATE SLSInvH SET PaidAmount = @PAID, OutstandingAmount = ((InvoiceAmount - DiscountAmount) + PPnAmount) - @PAID 
        WHERE custCode = @CUST 
        AND CCY=@CCY 
        AND InvoiceNumber = @INVNO";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, serialNo);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@paid", System.Data.SqlDbType.Money, paid);

                db.Open();
                db.BeginTransaction();
                result = db.ExecuteNonQuery();
                db.CommitTransaction();

                db.Close();
            }

            return result;
        }

        public static List<Invoice> GetSalesVoidInvoice(string cust,string period,string ccy)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"SELECT *, (((InvoiceAmount - DiscountAmount) + PPnAmount) * -1) AS InvoiceTotal
		FROM SlsInvH AS i
		WHERE CONVERT(VARCHAR(6), i.InvoiceDate, 112) = CONVERT(VARCHAR(6), @PERIOD, 112) 
			AND i.CustCode = @CUST AND ISNULL(i.StatusINV, 0) = 3 AND i.CCY=@CCY";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                //db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            //invoice.RefNo = dr["RefNo"] as string;

                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            //invoice.Cust.CUSTName = IDS.Tool.GeneralHelper.NullToString(dr["Name"],"");

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            invoice.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            invoice.DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]);
                            invoice.InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.IsPPh = Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            invoice.PPhPercentage = Convert.ToDecimal(dr["PPhPercentage"]);
                            invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            invoice.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            invoice.PPnAmount = Convert.ToDecimal(dr["PPnAmount"]);
                            invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            //invoice.Account= new GLTable.ChartOfAccount();
                            //invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            invoice.Description = dr["Description"] as string;
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<Invoice> GetSalesInvoice(string cust, string period, string ccy)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"SELECT *, ((InvoiceAmount - DiscountAmount) + PPnAmount) AS InvoiceTotal
		FROM SlsInvH AS i
		WHERE CONVERT(VARCHAR(6), i.InvoiceDate, 112) = CONVERT(VARCHAR(6), @PERIOD, 112) 
			AND i.CustCode = @CUST AND ISNULL(i.StatusINV, 0) <= 2 AND i.CCY=@CCY;";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                //db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            //invoice.RefNo = dr["RefNo"] as string;

                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            //invoice.Cust.CUSTName = dr["Name"] as string;

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            invoice.InvoiceTotal = IDS.Tool.GeneralHelper.NullToDecimal(dr["InvoiceTotal"], 0);
                            invoice.DiscountAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["DiscountAmount"], 0);
                            invoice.InvoiceAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["InvoiceAmount"], 0);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            //invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.IsPPh = Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            invoice.PPhPercentage = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPhPercentage"], 0);
                            invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            invoice.PPhAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPhAmount"],0);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            invoice.PPnAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPnAmount"], 0);
                            invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            //invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            //invoice.Account= new GLTable.ChartOfAccount();
                            //invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            //invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            invoice.Description = dr["Description"] as string;
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<Invoice> GetNewVoidInvoice(string cust,string inv)
        {
            List<IDS.Sales.Invoice> list = new List<Invoice>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(cust));
                db.AddParameter("@inv", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(inv));
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 14);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Invoice invoice = new Invoice();
                            invoice.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            invoice.RefNo = dr["RefNo"] as string;

                            invoice.Branch = new GeneralTable.Branch();
                            invoice.Branch.BranchCode = dr["branchcode"] as string;
                            //department.BranchDepartment.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            //department.BranchDepartment.NPWP = dr["NPWP"] as string;

                            //department.BranchDepartment.BranchManagerName = dr["BranchManager"] as string;
                            //department.BranchDepartment.FinAccOfficer = dr["FinAccOfficer"] as string;

                            invoice.Cust = new GeneralTable.Customer();
                            invoice.Cust.CUSTCode = dr["CustCode"] as string;
                            //invoice.Cust.CUSTName = dr["Name"] as string;

                            //custProj.StartPeriod = Convert.ToDateTime(dr["StartPeriod"]);
                            //invoice.InvoiceTotal = Convert.ToDecimal(dr["InvoiceTotal"]);
                            invoice.DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]);
                            invoice.InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                            //invoice.TermOfPayment = Tool.GeneralHelper.NullToInt(dr["Remark"],0);
                            //invoice.PaidAmount = Convert.ToDecimal(dr["PaidAmount"]);
                            //invoice.OutstandingAmount = Convert.ToDecimal(dr["OutstandingAmount"]);
                            invoice.StatusInv = Tool.GeneralHelper.NullToInt(dr["StatusInv"], 0);
                            invoice.IsPPh = Tool.GeneralHelper.NullToString(dr["IsPPh"]);
                            //invoice.PPhPercentage = Convert.ToDecimal(dr["PPhPercentage"]);
                            //invoice.PPhNo = Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            //invoice.PPhAmount = Convert.ToDecimal(dr["PPhAmount"]);
                            //invoice.PPhDateReceive = Convert.ToDateTime(dr["PPhDateReceive"]);
                            //invoice.IsPPn = Tool.GeneralHelper.NullToBool(dr["IsPPn"]);
                            //invoice.PPnAmount = Convert.ToDecimal(dr["PPnAmount"]);
                            //invoice.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            //invoice.TaxStatus = Tool.GeneralHelper.NullToBool(dr["TaxStatus"]);
                            //invoice.PaymentStatus = Tool.GeneralHelper.NullToBool(dr["PaymentStatus"]);
                            //invoice.PayDate = Convert.ToDateTime(dr["PayDate"]);
                            invoice.Voucher = Tool.GeneralHelper.NullToString(dr["Voucher"]);

                            //invoice.Account= new GLTable.ChartOfAccount();
                            //invoice.Account.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            invoice.CCy = new GeneralTable.Currency();
                            invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);

                            ////invoice.ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]);
                            ////invoice.EquivAmount = Convert.ToDecimal(dr["EquivAmt"]);
                            invoice.StatusARAP = Tool.GeneralHelper.NullToBool(dr["StatusARAP"]);
                            //invoice.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"], 0);

                            //invoice.BuktiPotongPPh = dr["BuktiPotongPPh"] as string;
                            invoice.Description = dr["Description"] as string;
                            //invoice.CreatedBy = dr["CreatedBy"] as string;
                            //invoice.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            invoice.OperatorID = dr["OperatorID"] as string;
                            invoice.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(invoice);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static void ProcessVoid(string[] invNo, string[] invDate, string[] cust, string operatorID,string branch)
        {
            if (invNo == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "sp_voidInvoice";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < invNo.Length; i++)
                    {
                        cmd.CommandText = "sp_voidInvoice";
                        cmd.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo[i]);
                        cmd.AddParameter("@VoidDate", System.Data.SqlDbType.DateTime, Convert.ToDateTime(invDate[i]));
                        cmd.AddParameter("@userID", System.Data.SqlDbType.VarChar, operatorID);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();

                        IDS.Sales.OutstandingRecalculate.NewSPBasedCalculator.Recalculate(branch,cust[i], null, DateTime.Now);
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
                            throw new Exception("Void Invoice is already exists. Please choose other Void Invoice.");
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
        }

        //public string CheckExportFaktur(string invNo)
        //{
        //    string result = "";

        //    using (DataAccess.SqlServer db = new DataAccess.SqlServer())
        //    {
        //        db.CommandText = @"select DocumentNo from tblTaxNumberList where DocumentNo = @InvNo and ExportStatus = 1";
        //        db.CommandType = System.Data.CommandType.Text;
        //        db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo);
        //        db.Open();

        //        db.ExecuteReader();

        //        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
        //        {
        //            if (dr.HasRows)
        //            {
        //                result = dr["DocumentNo"] as string;
        //            }

        //            if (!dr.IsClosed)
        //                dr.Close();
        //        }

        //        db.Close();
        //    }

        //    return result;
        //}

        //        public static void UpdateSalesInvoice(string isPPh, object nobp, )
        //        {
        //            try
        //            {
        //                using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
        //                {
        //                    try
        //                    {
        //                        db.CommandText = @"UPDATE SLSInvH SET 
        //    isPPh = @TAX, PPhNo = @NOBP, PPhPercentage = @PERCENT,
        //    PPhAmount = @ALLO, PPhDateReceive = @DATE
        //WHERE InvoiceNumber = @INVNO";
        //                        db.AddParameter("@serial", System.Data.SqlDbType.VarChar, "RCV" + PayDate.ToString("yyMM"));
        //                        db.CommandType = System.Data.CommandType.Text;
        //                        db.Open();
        //                    }
        //                    catch
        //                    {
        //                        throw;
        //                    }
        //                    finally
        //                    {
        //                        db.Close();
        //                        GC.Collect();
        //                    }
        //                }
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //        }

        public string Validation()
        {
            StringBuilder sb = new StringBuilder();

            //if (string.IsNullOrEmpty(InvoiceNumber))
            //    sb.Append("Invoice Number is Required. Please Insert Invoice Number");

            //if (sb == null)
            //    return "";
            //else
            //    return sb.ToString();
            return "";
        }
    }

    public class InvoiceQR
    {
        public string InvoiceNo { get; set; }
        public DateTime InvDate { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
    }
}
