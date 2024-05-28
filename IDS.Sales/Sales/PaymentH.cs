using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class PaymentH
    {
        [Display(Name = "Serial No")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Serial No is Required")]
        [MaxLength(50)]
        public string SerialNo { get; set; }
        public string ReffNo { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTo { get; set; }
        public DateTime PayDate { get; set; }
        public string PaymentMethod { get; set; }
        public bool PayMethod { get; set; }
        public IDS.GeneralTable.Customer Customer { get; set; }
        public IDS.GeneralTable.Bank Bank { get; set; }
        public IDS.GLTable.ChartOfAccount Account { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public IDS.GeneralTable.Currency Ccy { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public string Comment { get; set; }
        public string Voucher { get; set; }
        public string Flag { get; set; }
        public bool PPh23 { get; set; }
        public decimal PPh23Percent { get; set; }
        public IDS.GLTable.ChartOfAccount PPh23Account { get; set; }

        [Display(Name = "Branch")]
        public IDS.GeneralTable.Branch Branch { get; set; }

        public string SCode { get; set; }

        [Display(Name = "Operator ID")]
        [MaxLength(20), StringLength(20)]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public List<PaymentD> Detail { get; set; }

        public PaymentH()
        {

        }

        public static List<PaymentH> GetPaymentH(string serialNo, string reffNo, string type, DateTime dateFrom,DateTime dateTo,int chkByDate,string vendor,string flag,int arap)
        {
            List<PaymentH> list = new List<PaymentH>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "searchPayment";
                db.CommandType = System.Data.CommandType.StoredProcedure;

                db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(serialNo));
                db.AddParameter("@refNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(reffNo));
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, Tool.GeneralHelper.IntToDBNull(type));
                db.AddParameter("@dateFrom", System.Data.SqlDbType.DateTime, dateFrom);
                db.AddParameter("@dateTo", System.Data.SqlDbType.DateTime, dateTo);
                db.AddParameter("@chk", System.Data.SqlDbType.TinyInt, Tool.GeneralHelper.NullToInt(chkByDate,0));
                db.AddParameter("@vendor", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(vendor));
                db.AddParameter("@flag", System.Data.SqlDbType.TinyInt, Tool.GeneralHelper.NullToInt(flag, 0));
                db.AddParameter("@arap", System.Data.SqlDbType.TinyInt, arap);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            PaymentH paymentH = new PaymentH();
                            paymentH.SerialNo = IDS.Tool.GeneralHelper.NullToString(dr["SerialNo"]);
                            paymentH.ReffNo = IDS.Tool.GeneralHelper.NullToString(dr["reffNo"]);
                            paymentH.PaymentType = Enum.ToObject(typeof(Tool.PaymentType), Tool.GeneralHelper.NullToInt(dr["type"],0)).ToString();
                            paymentH.PaymentTo = Enum.ToObject(typeof(Tool.PaymentTo), Tool.GeneralHelper.NullToInt(dr["paymentTo"], 0)).ToString();
                            paymentH.PayDate = Convert.ToDateTime(dr["payDate"]);
                            paymentH.PaymentMethod = Enum.ToObject(typeof(Tool.PaymentMethod), Tool.GeneralHelper.NullToInt(dr["payMethod"], 0)).ToString();

                            paymentH.Customer = new IDS.GeneralTable.Customer();
                            paymentH.Customer.CUSTCode = IDS.Tool.GeneralHelper.NullToString(dr["custSuppCode"]);

                            paymentH.Bank = new GeneralTable.Bank();
                            paymentH.Bank.BankCode = IDS.Tool.GeneralHelper.NullToString(dr["bankCode"]);

                            paymentH.Account = new GLTable.ChartOfAccount();
                            paymentH.Account.Account = IDS.Tool.GeneralHelper.NullToString(dr["AccNo"]);

                            paymentH.Ccy = new GeneralTable.Currency();
                            paymentH.Ccy.CurrencyCode = IDS.Tool.GeneralHelper.NullToString(dr["ccycode"]);

                            paymentH.ChequeNo = IDS.Tool.GeneralHelper.NullToString(dr["chequeNo"]);

                            if (!string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["chequeDate"])))
                                paymentH.ChequeDate = Convert.ToDateTime(dr["chequeDate"]);

                            paymentH.TotalAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["totalAmount"],0);
                            paymentH.BaseAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["baseAmount"], 0);
                            paymentH.Comment = IDS.Tool.GeneralHelper.NullToString(dr["comment"]);
                            paymentH.Voucher = IDS.Tool.GeneralHelper.NullToString(dr["voucher"]);
                            paymentH.Flag = Enum.ToObject(typeof(Tool.PaymentFlag), Tool.GeneralHelper.NullToInt(dr["flag"], 0)).ToString();
                            paymentH.PPh23 = IDS.Tool.GeneralHelper.NullToBool(dr["Pph23"]);
                            paymentH.PPh23Percent = IDS.Tool.GeneralHelper.NullToDecimal(dr["Pph23Percent"], 0);

                            paymentH.PPh23Account = new GLTable.ChartOfAccount();
                            paymentH.PPh23Account.Account = IDS.Tool.GeneralHelper.NullToString(dr["Pph23Acc"]);

                            paymentH.Branch = new GeneralTable.Branch();
                            paymentH.Branch.BranchCode = dr["branchcode"] as string;

                            paymentH.SCode = IDS.Tool.GeneralHelper.NullToString(dr["scode"]);
                            paymentH.OperatorID = dr["OperatorID"] as string;
                            paymentH.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(paymentH);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static PaymentH GetSalesPaymentHWithDetail(string serialNo)
        {
            PaymentH paymentH = new PaymentH();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesPaymentH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, serialNo);
                db.AddParameter("@Init", System.Data.SqlDbType.VarChar, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            #region Fill Header
                            paymentH.SerialNo = IDS.Tool.GeneralHelper.NullToString(dr["SerialNo"]);
                            paymentH.ReffNo = IDS.Tool.GeneralHelper.NullToString(dr["reffNo"]);
                            //paymentH.PaymentType = Enum.ToObject(typeof(Tool.PaymentType), Tool.GeneralHelper.NullToInt(dr["type"], 0)).ToString();
                            paymentH.PaymentType = Tool.GeneralHelper.NullToInt(dr["type"], 0).ToString();
                            paymentH.PaymentTo = Enum.ToObject(typeof(Tool.PaymentTo), Tool.GeneralHelper.NullToInt(dr["paymentTo"], 0)).ToString();
                            paymentH.PayDate = Convert.ToDateTime(dr["payDate"]);
                            paymentH.PaymentMethod = Enum.ToObject(typeof(Tool.PaymentMethod), Tool.GeneralHelper.NullToInt(dr["payMethod"], 0)).ToString();

                            paymentH.Customer = new IDS.GeneralTable.Customer();
                            paymentH.Customer.CUSTCode = IDS.Tool.GeneralHelper.NullToString(dr["custSuppCode"]);

                            paymentH.Bank = new GeneralTable.Bank();
                            paymentH.Bank.BankCode = IDS.Tool.GeneralHelper.NullToString(dr["bankCode"]);

                            paymentH.Account = new GLTable.ChartOfAccount();
                            paymentH.Account.Account = IDS.Tool.GeneralHelper.NullToString(dr["AccNo"]);

                            paymentH.Ccy = new GeneralTable.Currency();
                            paymentH.Ccy.CurrencyCode = IDS.Tool.GeneralHelper.NullToString(dr["ccycode"]);

                            paymentH.ChequeNo = IDS.Tool.GeneralHelper.NullToString(dr["chequeNo"]);

                            if (!string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["chequeDate"])))
                                paymentH.ChequeDate = Convert.ToDateTime(dr["chequeDate"]);

                            paymentH.TotalAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["totalAmount"], 0);
                            paymentH.BaseAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["baseAmount"], 0);
                            paymentH.Comment = IDS.Tool.GeneralHelper.NullToString(dr["comment"]);
                            paymentH.Voucher = IDS.Tool.GeneralHelper.NullToString(dr["voucher"]);
                            //paymentH.Flag = Enum.ToObject(typeof(Tool.PaymentFlag), Tool.GeneralHelper.NullToInt(dr["flag"], 0)).ToString();
                            paymentH.Flag = IDS.Tool.GeneralHelper.NullToString(dr["flag"]);
                            paymentH.PPh23 = IDS.Tool.GeneralHelper.NullToBool(dr["Pph23"]);
                            paymentH.PPh23Percent = IDS.Tool.GeneralHelper.NullToDecimal(dr["Pph23Percent"], 0);

                            paymentH.PPh23Account = new GLTable.ChartOfAccount();
                            paymentH.PPh23Account.Account = IDS.Tool.GeneralHelper.NullToString(dr["Pph23Acc"]);

                            paymentH.Branch = new GeneralTable.Branch();
                            paymentH.Branch.BranchCode = dr["branchcode"] as string;

                            paymentH.SCode = IDS.Tool.GeneralHelper.NullToString(dr["scode"]);
                            paymentH.OperatorID = dr["OperatorID"] as string;
                            paymentH.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            #endregion

                            if (paymentH.Detail == null)
                                paymentH.Detail = new List<PaymentD>();

                            #region Fill Detail
                            PaymentD pd = new PaymentD();
                            pd.SerialNo = Tool.GeneralHelper.NullToString(dr["serialNo"]);
                            pd.SeqNo = Tool.GeneralHelper.NullToInt(dr["seq"], 0);
                            pd.AlloType = Tool.GeneralHelper.NullToInt(dr["alloType"], 0);
                            pd.Invoice = new Invoice();
                            pd.InvNo = Tool.GeneralHelper.NullToString(dr["invNo"]);
                            pd.Invoice.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["invNo"]);
                            if (!string.IsNullOrEmpty(dr["InvoiceDate"].ToString()))
                            {
                                pd.Invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            }
                            pd.Invoice.EquivAmount = Tool.GeneralHelper.NullToDecimal(dr["EquivAmt"],0);
                            pd.Invoice.OutstandingAmount = Tool.GeneralHelper.NullToDecimal(dr["OutstandingAmount"], 0);
                            pd.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            pd.AlloAmount = Tool.GeneralHelper.NullToDecimal(dr["alloAmount"], 0);
                            pd.EquivAmount = Tool.GeneralHelper.NullToDecimal(dr["equivAmount"], 0);
                            pd.ExchRate = Tool.GeneralHelper.NullToDecimal(dr["exchrate"], 0);

                            pd.Ccy = new GeneralTable.Currency();
                            pd.Ccy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["ccy"]);

                            pd.Acc = new GLTable.ChartOfAccount();
                            pd.Acc.Account = Tool.GeneralHelper.NullToString(dr["acc"]);
                            pd.Acc.AccountName = Tool.GeneralHelper.NullToString(dr["accname"]);

                            paymentH.Detail.Add(pd);
                            #endregion
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return paymentH;
        }

        public static PaymentH GetSalesPaymentH(string serialNo)
        {
            PaymentH paymentH = new PaymentH();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesPaymentH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, serialNo);
                db.AddParameter("@Init", System.Data.SqlDbType.VarChar, 4);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            #region Fill Header
                            paymentH.SerialNo = IDS.Tool.GeneralHelper.NullToString(dr["SerialNo"]);
                            paymentH.ReffNo = IDS.Tool.GeneralHelper.NullToString(dr["reffNo"]);
                            //paymentH.PaymentType = Enum.ToObject(typeof(Tool.PaymentType), Tool.GeneralHelper.NullToInt(dr["type"], 0)).ToString();
                            paymentH.PaymentType = Tool.GeneralHelper.NullToInt(dr["type"], 0).ToString();
                            paymentH.PaymentTo = Enum.ToObject(typeof(Tool.PaymentTo), Tool.GeneralHelper.NullToInt(dr["paymentTo"], 0)).ToString();
                            paymentH.PayDate = Convert.ToDateTime(dr["payDate"]);
                            paymentH.PaymentMethod = Enum.ToObject(typeof(Tool.PaymentMethod), Tool.GeneralHelper.NullToInt(dr["payMethod"], 0)).ToString();

                            paymentH.Customer = new IDS.GeneralTable.Customer();
                            paymentH.Customer.CUSTCode = IDS.Tool.GeneralHelper.NullToString(dr["custSuppCode"]);

                            paymentH.Bank = new GeneralTable.Bank();
                            paymentH.Bank.BankCode = IDS.Tool.GeneralHelper.NullToString(dr["bankCode"]);

                            paymentH.Account = new GLTable.ChartOfAccount();
                            paymentH.Account.Account = IDS.Tool.GeneralHelper.NullToString(dr["AccNo"]);

                            paymentH.Ccy = new GeneralTable.Currency();
                            paymentH.Ccy.CurrencyCode = IDS.Tool.GeneralHelper.NullToString(dr["ccycode"]);

                            paymentH.ChequeNo = IDS.Tool.GeneralHelper.NullToString(dr["chequeNo"]);

                            if (!string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["chequeDate"])))
                                paymentH.ChequeDate = Convert.ToDateTime(dr["chequeDate"]);

                            paymentH.TotalAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["totalAmount"], 0);
                            paymentH.BaseAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["baseAmount"], 0);
                            paymentH.Comment = IDS.Tool.GeneralHelper.NullToString(dr["comment"]);
                            paymentH.Voucher = IDS.Tool.GeneralHelper.NullToString(dr["voucher"]);
                            //paymentH.Flag = Enum.ToObject(typeof(Tool.PaymentFlag), Tool.GeneralHelper.NullToInt(dr["flag"], 0)).ToString();
                            paymentH.Flag = IDS.Tool.GeneralHelper.NullToString(dr["flag"]);
                            paymentH.PPh23 = IDS.Tool.GeneralHelper.NullToBool(dr["Pph23"]);
                            paymentH.PPh23Percent = IDS.Tool.GeneralHelper.NullToDecimal(dr["Pph23Percent"], 0);

                            paymentH.PPh23Account = new GLTable.ChartOfAccount();
                            paymentH.PPh23Account.Account = IDS.Tool.GeneralHelper.NullToString(dr["Pph23Acc"]);

                            paymentH.Branch = new GeneralTable.Branch();
                            paymentH.Branch.BranchCode = dr["branchcode"] as string;

                            paymentH.SCode = IDS.Tool.GeneralHelper.NullToString(dr["scode"]);
                            paymentH.OperatorID = dr["OperatorID"] as string;
                            paymentH.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            #endregion

                            if (paymentH.Detail == null)
                                paymentH.Detail = new List<PaymentD>();

                            #region Fill Detail
                            PaymentD pd = new PaymentD();
                            pd.SerialNo = Tool.GeneralHelper.NullToString(dr["serialNo"]);
                            pd.SeqNo = Tool.GeneralHelper.NullToInt(dr["seq"], 0);
                            pd.AlloType = Tool.GeneralHelper.NullToInt(dr["alloType"], 0);
                            pd.Invoice = new Invoice();
                            pd.InvNo = Tool.GeneralHelper.NullToString(dr["invNo"]);
                            pd.Invoice.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["invNo"]);
                            if (!string.IsNullOrEmpty(dr["InvoiceDate"].ToString()))
                            {
                                pd.Invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            }
                            pd.Invoice.EquivAmount = Tool.GeneralHelper.NullToDecimal(dr["EquivAmt"], 0);
                            pd.Invoice.OutstandingAmount = Tool.GeneralHelper.NullToDecimal(dr["OutstandingAmount"], 0);
                            pd.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            pd.AlloAmount = Tool.GeneralHelper.NullToDecimal(dr["alloAmount"], 0);
                            pd.EquivAmount = Tool.GeneralHelper.NullToDecimal(dr["equivAmount"], 0);
                            pd.ExchRate = Tool.GeneralHelper.NullToDecimal(dr["exchrate"], 0);

                            pd.Ccy = new GeneralTable.Currency();
                            pd.Ccy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["ccy"]);

                            pd.Acc = new GLTable.ChartOfAccount();
                            pd.Acc.Account = Tool.GeneralHelper.NullToString(dr["acc"]);
                            pd.Acc.AccountName = Tool.GeneralHelper.NullToString(dr["accname"]);

                            paymentH.Detail.Add(pd);
                            #endregion
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return paymentH;
        }

        public static List<System.Web.Mvc.SelectListItem> GetPaymentTypeForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> paymentType = new List<System.Web.Mvc.SelectListItem>();
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "Cash", Value = ((int)IDS.Tool.PaymentType.Cash).ToString().ToString() });
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "Transfer", Value = ((int)IDS.Tool.PaymentType.Transfer).ToString() });
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "Cheque", Value = ((int)IDS.Tool.PaymentType.Cheque).ToString() });
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "Giro", Value = ((int)IDS.Tool.PaymentType.Giro).ToString() });
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "Other", Value = ((int)IDS.Tool.PaymentType.Other).ToString() });

            return paymentType;
        }

        public int InsUpDel(int ExeCode, ref string newSerialNo)
        {
            int result = 0;
            List<PaymentD> paymentD = new List<PaymentD>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    if ((int)IDS.Tool.PageActivity.Insert == ExeCode)
                    {
                        SerialNo = GenerateSerialNumber();
                        newSerialNo = SerialNo;
                    }

                    db.CommandText = "SELECT * FROM paymentDetail WHERE serialNo = @serialNo AND (alloType = 1 OR alloType = 6)";
                    db.CommandType = System.Data.CommandType.Text;
                    db.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, SerialNo);
                    //db.AddParameter("@type", System.Data.SqlDbType.Int, 4);
                    db.CommandType = System.Data.CommandType.Text;
                    db.Open();

                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                PaymentD pd = new PaymentD();
                                pd.SerialNo = Tool.GeneralHelper.NullToString(dr["serialNo"]);
                                pd.SeqNo = Tool.GeneralHelper.NullToInt(dr["seq"], 0);
                                pd.AlloType = Tool.GeneralHelper.NullToInt(dr["alloType"], 0);
                                pd.InvNo = Tool.GeneralHelper.NullToString(dr["invNo"]);
                                pd.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                                pd.AlloAmount = Tool.GeneralHelper.NullToDecimal(dr["alloAmount"], 0);
                                pd.EquivAmount = Tool.GeneralHelper.NullToDecimal(dr["equivAmount"], 0);
                                pd.ExchRate = Tool.GeneralHelper.NullToDecimal(dr["exchrate"], 0);

                                pd.Acc = new GLTable.ChartOfAccount();
                                pd.Acc.Account = Tool.GeneralHelper.NullToString(dr["acc"]);

                                paymentD.Add(pd);
                            }
                        }

                        if (!dr.IsClosed)
                        {
                            dr.Close();
                        }
                    }

                    db.Open();
                    db.BeginTransaction();

                    foreach (var item in paymentD)
                    {
                        db.CommandText = "AdjustCustOutstanding";
                        db.DbCommand.CommandTimeout = 0;
                        db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Customer.CUSTCode));
                        db.AddParameter("@period", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(PayDate.ToString("yyyyMM")));
                        db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Ccy.CurrencyCode));
                        db.AddParameter("@Credit", System.Data.SqlDbType.Money, TotalAmount /*AlloAmount*/);
                        db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 3);

                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.Open();

                        
                        db.ExecuteNonQuery();

                        db.CommandText = "SalesUpdInvOutstanding";
                        db.DbCommand.CommandTimeout = 0;
                        db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(item.InvNo));
                        db.AddParameter("@AlloAmount", System.Data.SqlDbType.Money, TotalAmount * -1 /*AlloAmount*/);
                        db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 4);

                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.Open();
                        
                        db.ExecuteNonQuery();
                    }

                    db.CommandText = "paymentSaveHeader";
                    db.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SerialNo));
                    db.AddParameter("@ReffNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.NullToString(ReffNo));//Tool.GeneralHelper.StringToDBNull(ReffNo));
                    db.AddParameter("@type", System.Data.SqlDbType.TinyInt, IDS.Tool.GeneralHelper.IntToDBNull(PaymentType));
                    db.AddParameter("@paymentTo", System.Data.SqlDbType.TinyInt, DBNull.Value/*0*/);
                    db.AddParameter("@payDate", System.Data.SqlDbType.DateTime, PayDate);
                    db.AddParameter("@payMethod", System.Data.SqlDbType.Bit, PayMethod/*PaymentMethod*/);
                    db.AddParameter("@custSuppCode", System.Data.SqlDbType.VarChar, Customer.CUSTCode);
                    db.AddParameter("@bankCode", System.Data.SqlDbType.VarChar, Bank.BankCode/*Bank.BankCode*/);
                    db.AddParameter("@AccNo", System.Data.SqlDbType.VarChar, Account.Account);
                    if (PaymentType == "1" || PaymentType == "2")
                    {
                        db.AddParameter("@chequeNo", System.Data.SqlDbType.VarChar, ChequeNo);
                        db.AddParameter("@chequeDate", System.Data.SqlDbType.DateTime, ChequeDate);
                    }

                    db.AddParameter("@ccycode", System.Data.SqlDbType.VarChar, Ccy.CurrencyCode);
                    db.AddParameter("@totalAmount", System.Data.SqlDbType.Money, TotalAmount);
                    db.AddParameter("@baseAmount", System.Data.SqlDbType.Money, BaseAmount);
                    db.AddParameter("@comment", System.Data.SqlDbType.VarChar, Comment);
                    db.AddParameter("@voucher", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@Pph23", System.Data.SqlDbType.Bit, DBNull.Value);
                    db.AddParameter("@Pph23Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@PphPercent", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                    db.AddParameter("@SourceCode", System.Data.SqlDbType.VarChar, SCode);
                    db.AddParameter("@operatorID", System.Data.SqlDbType.VarChar, OperatorID);
                    db.AddParameter("@init", System.Data.SqlDbType.TinyInt, ExeCode);
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();
                    result = db.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                    }

                    db.CommandText = "paymentSaveDetail";
                    db.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SerialNo));
                    db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);//Hapus
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();
                    result = db.ExecuteNonQuery();

                    //db.CommandText = "paymentSaveDetail";
                    //db.DbCommand.CommandTimeout = 0;
                    //db.CommandType = System.Data.CommandType.StoredProcedure;
                    //db.Open();

                    for (int i = 0; i < Detail.Count; i++)
                    {
                        db.CommandText = "paymentSaveDetail";
                        db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SerialNo));
                        db.AddParameter("@Seq", System.Data.SqlDbType.Int, Tool.GeneralHelper.IntToDBNull(Detail[i].SeqNo));
                        db.AddParameter("@AlloType", System.Data.SqlDbType.SmallInt, Tool.GeneralHelper.IntToDBNull(Detail[i].AlloType));
                        db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].InvNo));
                        db.AddParameter("@amount", System.Data.SqlDbType.Money, Tool.GeneralHelper.DecimalToDBNull(Detail[i].Amount)); 
                        db.AddParameter("@alloAmount", System.Data.SqlDbType.Money, Tool.GeneralHelper.DecimalToDBNull(Detail[i].AlloAmount));
                        db.AddParameter("@exchRate", System.Data.SqlDbType.Money, Tool.GeneralHelper.DecimalToDBNull(Detail[i].ExchRate));
                        db.AddParameter("@equivAmount", System.Data.SqlDbType.Money, Tool.GeneralHelper.DecimalToDBNull(Detail[i].EquivAmount));
                        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Ccy?.CurrencyCode));
                        db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Acc.Account));
                        db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                        db.DbCommand.CommandTimeout = 0;
                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.Open();
                        result = db.ExecuteNonQuery();

                        if (result <= 0)
                        {
                            throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                        }

                        int alloType = Detail[i].AlloType;

                        if (alloType == 1 || alloType == 6)
                        {
                            db.CommandText = "SalesSelCustOutstanding";
                            db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, Customer.CUSTCode);
                            db.AddParameter("@period", System.Data.SqlDbType.VarChar, PayDate.ToString("yyyyMM"));
                            db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Ccy.CurrencyCode);
                            db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                            db.CommandType = System.Data.CommandType.StoredProcedure;
                            db.Open();

                            object outs = db.ExecuteScalar();

                            if (!(outs is DBNull || outs == null))
                            {
                                db.CommandText = "AdjustCustOutstanding";
                                db.DbCommand.CommandTimeout = 0;
                                db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Customer.CUSTCode));
                                db.AddParameter("@period", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(PayDate.ToString("yyyyMM")));
                                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Ccy.CurrencyCode));
                                db.AddParameter("@Credit", System.Data.SqlDbType.Money, Detail[i].AlloAmount * -1 /*AlloAmount*/);
                                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 3);

                                db.CommandType = System.Data.CommandType.StoredProcedure;
                                db.Open();
                                
                                db.ExecuteNonQuery();
                            }
                            else
                            {
                                db.CommandText = "SalesCustOutstanding";
                                db.DbCommand.CommandTimeout = 0;
                                db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Customer.CUSTCode));
                                db.AddParameter("@period", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(PayDate.ToString("yyyyMM")));
                                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Ccy.CurrencyCode));
                                db.AddParameter("@Credit", System.Data.SqlDbType.Money, Detail[i].AlloAmount * -1 /*AlloAmount*/);
                                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 1);//input custoutstanding

                                db.CommandType = System.Data.CommandType.StoredProcedure;
                                db.Open();

                                db.ExecuteNonQuery();
                            }

                            db.CommandText = "SalesUpdInvOutstanding";
                            db.DbCommand.CommandTimeout = 0;
                            db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].InvNo));
                            db.AddParameter("@AlloAmount", System.Data.SqlDbType.Money, Detail[i].AlloAmount /*AlloAmount*/);
                            db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 4);

                            db.CommandType = System.Data.CommandType.StoredProcedure;
                            db.Open();

                            db.ExecuteNonQuery();
                        }
                        else if (alloType == 2)
                        {
                            //if (true) TaxAble??
                            //{
                                IDS.GeneralTable.Tax tax = IDS.GeneralTable.Tax.GetTaxWithAcc(Detail[i].Acc.Account);
                                tax.CalculateOutstandingSalesTax(Detail[i].Amount, Customer.CUSTCode, PayDate, SerialNo);
                                tax.TaxTransaction(Detail[i].InvNo, Detail[i].AlloAmount, PayDate, Detail[i].Amount, true);
                                
                            //}
                        }
                    }
                    db.CommitTransaction();
                    IDS.Sales.CustomerOutstanding.Recalculate(Branch.BranchCode, Customer.CUSTCode, Ccy.CurrencyCode, PayDate,true,OperatorID);

                    //db.CommitTransaction();
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
                catch (Exception e)
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

        public int InsUpDelPayment(int ExeCode, ref string newSerialNo)
        {
            int result = 0;
            List<PaymentD> paymentD = new List<PaymentD>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    if ((int)IDS.Tool.PageActivity.Insert == ExeCode)
                    {
                        SerialNo = GenerateSerialNumber();
                        newSerialNo = SerialNo;
                    }

                    db.CommandText = "SELECT * FROM paymentDetail WHERE serialNo = @serialNo AND alloType = 1";
                    db.CommandType = System.Data.CommandType.Text;
                    db.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, SerialNo);
                    //db.AddParameter("@type", System.Data.SqlDbType.Int, 4);
                    db.CommandType = System.Data.CommandType.Text;
                    db.Open();

                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                PaymentD pd = new PaymentD();
                                pd.SerialNo = Tool.GeneralHelper.NullToString(dr["serialNo"]);
                                pd.SeqNo = Tool.GeneralHelper.NullToInt(dr["seq"], 0);
                                pd.AlloType = Tool.GeneralHelper.NullToInt(dr["alloType"], 0);
                                pd.InvNo = Tool.GeneralHelper.NullToString(dr["invNo"]);
                                pd.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                                pd.AlloAmount = Tool.GeneralHelper.NullToDecimal(dr["alloAmount"], 0);
                                pd.EquivAmount = Tool.GeneralHelper.NullToDecimal(dr["equivAmount"], 0);
                                pd.ExchRate = Tool.GeneralHelper.NullToDecimal(dr["exchrate"], 0);
                                pd.Type = Tool.GeneralHelper.NullToString(dr["Type"]);

                                pd.Acc = new GLTable.ChartOfAccount();
                                pd.Acc.Account = Tool.GeneralHelper.NullToString(dr["accountno"]);

                                paymentD.Add(pd);
                            }
                        }

                        if (!dr.IsClosed)
                        {
                            dr.Close();
                        }
                    }

                    db.Close();
                    db.BeginTransaction();

                    foreach (var item in paymentD)
                    {
                        db.CommandText = "AdjustCustOutstanding";
                        db.DbCommand.CommandTimeout = 0;
                        db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Customer.CUSTCode));
                        db.AddParameter("@period", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(PayDate.ToString("yyyyMM")));
                        db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Ccy.CurrencyCode));
                        db.AddParameter("@Credit", System.Data.SqlDbType.Money, TotalAmount /*AlloAmount*/);
                        db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 7);

                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.Open();


                        db.ExecuteNonQuery();

                        db.CommandText = "SalesUpdInvOutstanding";
                        db.DbCommand.CommandTimeout = 0;
                        db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(item.InvNo));
                        db.AddParameter("@AlloAmount", System.Data.SqlDbType.Money, TotalAmount * -1 /*AlloAmount*/);
                        db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 4);

                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.Open();

                        db.ExecuteNonQuery();
                    }

                    db.CommandText = "paymentSaveHeader";
                    db.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SerialNo));
                    db.AddParameter("@ReffNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.NullToString(ReffNo));//Tool.GeneralHelper.StringToDBNull(ReffNo));
                    db.AddParameter("@type", System.Data.SqlDbType.TinyInt, IDS.Tool.GeneralHelper.IntToDBNull(PaymentType));
                    db.AddParameter("@paymentTo", System.Data.SqlDbType.TinyInt, 1/*DBNull.Value*//*0*/);
                    db.AddParameter("@payDate", System.Data.SqlDbType.DateTime, PayDate);
                    db.AddParameter("@payMethod", System.Data.SqlDbType.Bit, PayMethod/*PaymentMethod*/);
                    db.AddParameter("@custSuppCode", System.Data.SqlDbType.VarChar, Customer.CUSTCode);
                    db.AddParameter("@bankCode", System.Data.SqlDbType.VarChar, Bank.BankCode/*Bank.BankCode*/);
                    db.AddParameter("@AccNo", System.Data.SqlDbType.VarChar, Account.Account);
                    if (PaymentType == "1" || PaymentType == "2")
                    {
                        db.AddParameter("@chequeNo", System.Data.SqlDbType.VarChar, ChequeNo);
                        db.AddParameter("@chequeDate", System.Data.SqlDbType.DateTime, ChequeDate);
                    }

                    db.AddParameter("@ccycode", System.Data.SqlDbType.VarChar, Ccy.CurrencyCode);
                    db.AddParameter("@totalAmount", System.Data.SqlDbType.Money, TotalAmount);
                    db.AddParameter("@baseAmount", System.Data.SqlDbType.Money, BaseAmount);
                    db.AddParameter("@comment", System.Data.SqlDbType.VarChar, Comment);
                    db.AddParameter("@voucher", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@Pph23", System.Data.SqlDbType.Bit, DBNull.Value);
                    db.AddParameter("@Pph23Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@PphPercent", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                    db.AddParameter("@SourceCode", System.Data.SqlDbType.VarChar, SCode);
                    db.AddParameter("@operatorID", System.Data.SqlDbType.VarChar, OperatorID);
                    db.AddParameter("@init", System.Data.SqlDbType.TinyInt, ExeCode);
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();
                    db.BeginTransaction();
                    result = db.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                    }

                    db.CommandText = "paymentSaveDetail";
                    db.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SerialNo));
                    db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);//Hapus
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();
                    //db.BeginTransaction();
                    result = db.ExecuteNonQuery();

                    db.CommandText = "paymentSaveDetail";
                    db.DbCommand.CommandTimeout = 0;
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();

                    for (int i = 0; i < Detail.Count; i++)
                    {
                        db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SerialNo));
                        db.AddParameter("@Seq", System.Data.SqlDbType.Int, Tool.GeneralHelper.IntToDBNull(Detail[i].SeqNo));
                        db.AddParameter("@AlloType", System.Data.SqlDbType.SmallInt, Tool.GeneralHelper.IntToDBNull(Detail[i].AlloType));
                        db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].InvNo));
                        db.AddParameter("@amount", System.Data.SqlDbType.Money, Tool.GeneralHelper.DecimalToDBNull(Detail[i].Amount));
                        db.AddParameter("@alloAmount", System.Data.SqlDbType.Money, Tool.GeneralHelper.DecimalToDBNull(Detail[i].AlloAmount));
                        db.AddParameter("@exchRate", System.Data.SqlDbType.Money, Tool.GeneralHelper.DecimalToDBNull(Detail[i].ExchRate));
                        db.AddParameter("@equivAmount", System.Data.SqlDbType.Money, Tool.GeneralHelper.DecimalToDBNull(Detail[i].EquivAmount));
                        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Ccy?.CurrencyCode));
                        db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Acc.Account));
                        db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                        db.Open();
                        result = db.ExecuteNonQuery();

                        if (result <= 0)
                        {
                            throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                        }

                        int alloType = Detail[i].AlloType;

                        if (alloType == 1)
                        {
                            db.CommandText = "SalesSelCustOutstanding";
                            db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, Customer.CUSTCode);
                            db.AddParameter("@period", System.Data.SqlDbType.VarChar, PayDate.ToString("yyyyMM"));
                            db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Ccy.CurrencyCode);
                            db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 4);
                            db.CommandType = System.Data.CommandType.StoredProcedure;
                            db.Open();

                            object outs = db.ExecuteScalar();

                            if (!(outs is DBNull/* || outs == null*/))
                            {
                                db.CommandText = "AdjustCustOutstanding";
                                db.DbCommand.CommandTimeout = 0;
                                db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Customer.CUSTCode));
                                db.AddParameter("@period", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(PayDate.ToString("yyyyMM")));
                                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Ccy.CurrencyCode));
                                db.AddParameter("@Credit", System.Data.SqlDbType.Money, Detail[i].AlloAmount * -1 /*AlloAmount*/);
                                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 7);

                                db.CommandType = System.Data.CommandType.StoredProcedure;
                                db.Open();

                                db.ExecuteNonQuery();
                            }
                            else
                            {
                                db.CommandText = "SalesCustOutstanding";
                                db.DbCommand.CommandTimeout = 0;
                                db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Customer.CUSTCode));
                                db.AddParameter("@period", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(PayDate.ToString("yyyyMM")));
                                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].Ccy.CurrencyCode));
                                db.AddParameter("@Credit", System.Data.SqlDbType.Money, Detail[i].AlloAmount * -1 /*AlloAmount*/);
                                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 2);//input custoutstanding

                                db.CommandType = System.Data.CommandType.StoredProcedure;
                                db.Open();

                                db.ExecuteNonQuery();
                            }

                            db.CommandText = "SalesUpdInvOutstanding";
                            db.DbCommand.CommandTimeout = 0;
                            db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Detail[i].InvNo));
                            db.AddParameter("@AlloAmount", System.Data.SqlDbType.Money, Detail[i].AlloAmount /*AlloAmount*/);
                            db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 4);

                            db.CommandType = System.Data.CommandType.StoredProcedure;
                            db.Open();

                            db.ExecuteNonQuery();
                        }
                        else if (alloType == 2)
                        {
                            //if (true) TaxAble??
                            //{
                            IDS.GeneralTable.Tax tax = IDS.GeneralTable.Tax.GetTaxWithAcc(Detail[i].Acc.Account);
                            tax.CalculateOutstandingSalesTax(Detail[i].Amount, Customer.CUSTCode, PayDate, SerialNo);
                            tax.TaxTransaction(Detail[i].InvNo, Detail[i].AlloAmount, PayDate, Detail[i].Amount, true);

                            //}
                        }
                    }
                    db.CommitTransaction();
                    //IDS.Sales.CustomerOutstanding.Recalculate(Branch.BranchCode, Customer.CUSTCode, Ccy.CurrencyCode, PayDate, true, OperatorID);

                    //db.CommitTransaction();
                    db.Close();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Payment already exists, please try to save again.");
                        default:
                            throw;
                    }
                }
                catch (Exception e)
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

        public int InsUpDel(int ExecCode, string[] serialNo)
        {
            int result = 0;

            if (serialNo == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    for (int i = 0; i < serialNo.Length; i++)
                    {
                        List<PaymentH> listPaymentH = new List<PaymentH>();
                        List<PaymentD> listPaymentD = new List<PaymentD>();

                        cmd.CommandText = "SalesSelPaymentH";
                        cmd.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, serialNo[i]);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Open();
                        cmd.ExecuteReader();

                        using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
                        {
                            if (dr.HasRows)
                            {
                                PaymentH payH = new PaymentH();
                                while (dr.Read())
                                {
                                    payH.SerialNo = IDS.Tool.GeneralHelper.NullToString(dr["SerialNo"],"");
                                    payH.TotalAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Total"], 0);

                                    payH.Ccy = new GeneralTable.Currency();
                                    payH.Ccy.CurrencyCode = IDS.Tool.GeneralHelper.NullToString(dr["ccy"]);

                                    payH.Customer = new GeneralTable.Customer();
                                    payH.Customer.CUSTCode = IDS.Tool.GeneralHelper.NullToString(dr["CustSuppCode"]);

                                    payH.PayDate = Convert.ToDateTime(dr["PayDate"]);

                                    listPaymentH.Add(payH);
                                }

                            }

                            if (!dr.IsClosed)
                                dr.Close();
                        }

                        foreach (var itemPayH in listPaymentH)
                        {
                            CustomerOutstanding.CalculateOutstandingCredit(itemPayH.Customer.CUSTCode, itemPayH.PayDate.ToString("yyyyMM"), itemPayH.Ccy.CurrencyCode, itemPayH.TotalAmount);
                        }

                        //
                        cmd.CommandText = "SalesSelPaymentD";
                        cmd.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, serialNo[i]);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.ExecuteReader();

                        using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
                        {
                            if (dr.HasRows)
                            {
                                PaymentD payD = new PaymentD();
                                while (dr.Read())
                                {
                                    payD.InvNo = IDS.Tool.GeneralHelper.NullToString(dr["InvNo"], "");
                                    payD.AlloAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["AlloAmount"], 0);

                                    listPaymentD.Add(payD);
                                }

                            }
                            if (!dr.IsClosed)
                                dr.Close();
                        }
                        //

                        foreach (var itemPayD in listPaymentD)
                        {
                            cmd.CommandText = "SalesUpdInvOutstanding";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 5);
                            cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, itemPayD.InvNo);
                            cmd.AddParameter("@AlloAmount", System.Data.SqlDbType.Money, itemPayD.AlloAmount);
                            cmd.Open();

                            cmd.BeginTransaction();
                            cmd.ExecuteNonQuery();
                            cmd.CommitTransaction();

                            cmd.Close();
                        }

                        cmd.CommandText = "paymentSaveHeader";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 3);
                        cmd.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, serialNo[i]);
                        cmd.Open();

                        cmd.BeginTransaction();
                        cmd.ExecuteNonQuery();
                        cmd.CommitTransaction();

                        cmd.Close();
                    }

                    //cmd.CommitTransaction();
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
                //catch (System.Data.SqlClient.SqlException e)
                //{
                //    if (cmd.Transaction != null)
                //        cmd.RollbackTransaction();

                //    throw;
                //}
                finally
                {
                    cmd.Close();
                }
            }

            return result;
        }

        public string ProcessJournal(string[] serialNo,string user,string branch)
        {
            string result = "";
            int resultCount = 0;
            
            if (serialNo == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    for (int i = 0; i < serialNo.Length; i++)
                    {
                        IDS.Sales.PaymentH paymentH = IDS.Sales.PaymentH.GetSalesPaymentHWithDetail(serialNo[i]);

                        if (string.IsNullOrEmpty(paymentH.Voucher))
                        {
                            string spacc = "";
                            string spaccCcy = "";
                            #region Process Header
                            Nullable<DateTime> date = paymentH.PayDate as Nullable<DateTime>;
                            string voc = this.GenerateVoucher(date.Value, paymentH.SCode);

                                #region Masukkan data ke ACFTRANH
                                cmd.CommandText = "SP_JOURNALRECEIVE";
                                cmd.AddParameter("@scode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(paymentH.SCode));
                                cmd.AddParameter("@voucher", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(voc));
                                cmd.AddParameter("@ENT_Date", System.Data.SqlDbType.DateTime, DateTime.Now);
                                cmd.AddParameter("@TRANS_DATE", System.Data.SqlDbType.DateTime, paymentH.PayDate);
                                cmd.AddParameter("@PAY_TERM", System.Data.SqlDbType.SmallInt, 0);
                                cmd.AddParameter("@ARAP_TRANS", System.Data.SqlDbType.SmallInt, 0);
                                cmd.AddParameter("@Office", System.Data.SqlDbType.SmallInt, 1);
                                cmd.AddParameter("@OPERATORID", System.Data.SqlDbType.VarChar, user);
                                cmd.AddParameter("@LASTUPD", System.Data.SqlDbType.DateTime, DateTime.Now);
                                cmd.AddParameter("@status", System.Data.SqlDbType.Bit, 0);
                                cmd.AddParameter("@STS_PAYMENT", System.Data.SqlDbType.SmallInt, 1);
                                cmd.AddParameter("@TYPE", System.Data.SqlDbType.SmallInt, 0);
                                cmd.AddParameter("@BRANCHCODE", System.Data.SqlDbType.VarChar, branch);
                                cmd.AddParameter("@COMMENT", System.Data.SqlDbType.VarChar, paymentH.Comment);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Open();
                                cmd.BeginTransaction();
                                resultCount = cmd.ExecuteNonQuery();
                            #endregion

                                #region Ambil Bank account dan currency
                                cmd.CommandText = @"SELECT 
                                b.BankCode AS[BANKCODE],
                                a.NAME AS[ACCNAME],
                                s.FR_ACC AS ACCOUNT,
                                s.FR_CCY AS[CCY],
                                s.TYPE_ACC AS TYPE
                                FROM ACFSPACC AS s LEFT JOIN tblBank AS b
                                ON s.FR_ACC = b.GelAcc LEFT JOIN ACFGLMH AS a
                                ON a.ACC = s.FR_ACC
                                WHERE s.FR_ACC = @ACCOUNT
                                ORDER BY ACCOUNT";
                                cmd.AddParameter("@ACCOUNT", System.Data.SqlDbType.VarChar, paymentH.Account.Account);
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.Open();
                                cmd.ExecuteReader();

                                using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
                                {
                                    if (dr.HasRows)
                                    {
                                        while (dr.Read())
                                        {
                                            spacc = dr["ACCOUNT"] as string;
                                            spaccCcy = dr["CCY"] as string;
                                        }

                                    }

                                    if (!dr.IsClosed)
                                        dr.Close();
                                }
                            #endregion

                            string baseccy = IDS.GeneralTable.Syspar.GetInstance().BaseCCy;
                            double totalAmount = Math.Abs(IDS.Tool.GeneralHelper.NullToDouble(paymentH.TotalAmount,0));
                            double baseAmount = Math.Abs(IDS.Tool.GeneralHelper.NullToDouble(paymentH.BaseAmount, 0));
                            bool differccy = spaccCcy.ToUpper().Trim() == baseccy;
                            if (differccy || IDS.Tool.GeneralHelper.NullToInt(paymentH.PaymentType,0) == 4) // cek payment type untuk pilihan Other
                            {
                                totalAmount = baseAmount;
                            }
                                #region Masukkan data header kedalam ACFTRANSD
                                cmd.CommandText = "SP_JOURNALRECEIVEDETAIL";
                                cmd.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 0);
                                cmd.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, paymentH.SCode);
                                cmd.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voc);
                                cmd.AddParameter("@COUNTER", System.Data.SqlDbType.SmallInt, 1);
                                cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToInt(paymentH.PaymentType, 0) == 4 ? paymentH.Account.Account : spacc); // Jika payment type selain other masukkan ke Spacc
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToInt(paymentH.PaymentType, 0) == 4 ? paymentH.Ccy.CurrencyCode : spaccCcy); // Jika payment type selain other masukkan ke SpaccCcy
                                cmd.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(IDS.GeneralTable.Syspar.GetInstance().Department,""));
                                cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, paymentH.SerialNo);
                                cmd.AddParameter("@Descrip", System.Data.SqlDbType.VarChar, paymentH.Comment);
                                cmd.AddParameter("@C_S", System.Data.SqlDbType.VarChar, paymentH.Customer.CUSTCode);
                                cmd.AddParameter("@UPAID", System.Data.SqlDbType.Bit, 0);
                                cmd.AddParameter("@Amount", System.Data.SqlDbType.Money, paymentH.Detail[0].AlloType == 6 ? (paymentH.TotalAmount * -1) : paymentH.TotalAmount);
                                cmd.AddParameter("@PayDate", System.Data.SqlDbType.DateTime, paymentH.PayDate);
                                cmd.AddParameter("@BRANCHCODE", System.Data.SqlDbType.VarChar, branch);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Open();
                                //cmd.BeginTransaction();
                                cmd.ExecuteNonQuery();
                                int k = 1;
                            #endregion

                                #region "Masukkan data Equivalent"
                                if (!differccy && IDS.Tool.GeneralHelper.NullToInt(paymentH.PaymentType, 0) != 4) // dan Bukan PayType Other
                                {
                                    cmd.CommandText = "SP_JOURNALRECEIVEDETAIL";
                                    cmd.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 0);
                                    cmd.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, paymentH.SCode);
                                    cmd.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voc);
                                    cmd.AddParameter("@COUNTER", System.Data.SqlDbType.SmallInt, 2);
                                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, spacc);
                                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, spaccCcy);
                                    cmd.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(IDS.GeneralTable.Syspar.GetInstance().Department, ""));
                                    cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, paymentH.SerialNo);
                                    cmd.AddParameter("@Descrip", System.Data.SqlDbType.VarChar, "Bank Received Equivalent "+paymentH.Comment);
                                    cmd.AddParameter("@C_S", System.Data.SqlDbType.VarChar, paymentH.Customer.CUSTCode);
                                    cmd.AddParameter("@UPAID", System.Data.SqlDbType.Bit, 0);
                                    cmd.AddParameter("@Amount", System.Data.SqlDbType.Money, paymentH.BaseAmount);
                                    cmd.AddParameter("@PayDate", System.Data.SqlDbType.DateTime, paymentH.PayDate);
                                    cmd.AddParameter("@BRANCHCODE", System.Data.SqlDbType.VarChar, branch);
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.Open();
                                    //cmd.BeginTransaction();
                                    cmd.ExecuteNonQuery();
                                    k = 2;
                                }
                            #endregion

                                #region "Process Detail"
                                if (paymentH.Detail.Count > 0)
                                {
                                    foreach (var item in paymentH.Detail)
                                    {
                                        string descript = "";
                                        int typedetail = 0;
                                        decimal amount = IDS.Tool.GeneralHelper.NullToDecimal(item.AlloAmount,0);
                                        decimal eqvamount = IDS.Tool.GeneralHelper.NullToDecimal(item.EquivAmount, 0);

                                        switch (item.AlloType)
                                        {
                                            case 1:
                                                descript = "Received from Invoice No : " + item.InvNo;
                                                typedetail = 1;

                                                cmd.CommandText = "UPDATE SLSInvH SET PAYDATE = @PAYDATE WHERE InvoiceNumber = @INVNO";
                                                cmd.AddParameter("@PayDate", System.Data.SqlDbType.DateTime, paymentH.PayDate);
                                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, item.InvNo);
                                                cmd.CommandType = System.Data.CommandType.Text;
                                                cmd.Open();
                                                //cmd.BeginTransaction();
                                                cmd.ExecuteNonQuery();
                                                amount = amount * -1;
                                                eqvamount = eqvamount * -1;
                                                break;

                                            case 2:
                                                descript = "PPh 23 from Invoice No : " + item.InvNo;
                                                typedetail = 2;
                                                amount = amount * -1;
                                                eqvamount = eqvamount * -1;
                                                break;

                                            case 3:
                                                descript = "Bank Charges from Receive No : " + paymentH.SerialNo;
                                                typedetail = 3;
                                                amount = amount * -1;
                                                eqvamount = eqvamount * -1;
                                                break;

                                            case 4:
                                                descript = "Other Fee from Invoice No : " + item.InvNo;
                                                typedetail = 4;
                                                amount = amount * -1;
                                                eqvamount = eqvamount * -1;
                                                break;

                                            case 5:
                                                descript = "Gain or Loss resulted from currency of Invoice No : " + item.InvNo;
                                                typedetail = 5;
                                                break;
                                            case 6:
                                                descript = "Received SSP of invoice No : " + item.InvNo;
                                                //amount = amount * -1;
                                                //eqvamount = eqvamount * -1;
                                                break;
                                            default:
                                                break;
                                    }
                                    #region "Masukkan data Detail ke ACFTRANSH"
                                    ++k;
                                    if (descript.Length > 100)
                                    {
                                        descript = descript.Substring(0, 100);
                                    }

                                    cmd.CommandText = "SP_JOURNALRECEIVEDETAIL";
                                    cmd.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 0);
                                    cmd.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, paymentH.SCode);
                                    cmd.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voc);
                                    cmd.AddParameter("@COUNTER", System.Data.SqlDbType.SmallInt, k);
                                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, item.Acc.Account);
                                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, item.Ccy.CurrencyCode);
                                    cmd.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(IDS.GeneralTable.Syspar.GetInstance().Department, ""));
                                    cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, paymentH.SerialNo);
                                    cmd.AddParameter("@Descrip", System.Data.SqlDbType.VarChar, descript + " " + paymentH.Customer.CUSTCode);
                                    cmd.AddParameter("@C_S", System.Data.SqlDbType.VarChar, paymentH.Customer.CUSTCode);
                                    cmd.AddParameter("@UPAID", System.Data.SqlDbType.Bit, 0);
                                    cmd.AddParameter("@Amount", System.Data.SqlDbType.Money, amount);
                                    cmd.AddParameter("@PayDate", System.Data.SqlDbType.DateTime, paymentH.PayDate);
                                    cmd.AddParameter("@BRANCHCODE", System.Data.SqlDbType.VarChar, branch);
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.Open();
                                    //cmd.BeginTransaction();
                                    cmd.ExecuteNonQuery();
                                    #endregion

                                    //Jika ada perbedaan ccy masukkan equivalent.
                                    differccy = item.Ccy.CurrencyCode.ToUpper().Trim() == baseccy;
                                    #region "Masukkan nilai Equivalent"
                                    if (!differccy)
                                    {
                                        ++k;
                                        descript += " Equivalent";
                                        if (descript.Length > 100)
                                        {
                                            descript = descript.Substring(0, 100);
                                        }
                                        cmd.CommandText = "SP_JOURNALRECEIVEDETAIL";
                                        cmd.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 0);
                                        cmd.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, paymentH.SCode);
                                        cmd.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voc);
                                        cmd.AddParameter("@COUNTER", System.Data.SqlDbType.SmallInt, k);
                                        cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, item.Acc.Account);
                                        cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
                                        cmd.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(IDS.GeneralTable.Syspar.GetInstance().Department, ""));
                                        cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, paymentH.SerialNo);
                                        cmd.AddParameter("@Descrip", System.Data.SqlDbType.VarChar, descript + " " + paymentH.Customer.CUSTCode);
                                        cmd.AddParameter("@C_S", System.Data.SqlDbType.VarChar, paymentH.Customer.CUSTCode);
                                        cmd.AddParameter("@UPAID", System.Data.SqlDbType.Bit, 0);
                                        cmd.AddParameter("@Amount", System.Data.SqlDbType.Money, eqvamount);
                                        cmd.AddParameter("@PayDate", System.Data.SqlDbType.DateTime, paymentH.PayDate);
                                        cmd.AddParameter("@BRANCHCODE", System.Data.SqlDbType.VarChar, branch);
                                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                        cmd.Open();
                                        //cmd.BeginTransaction();
                                        cmd.ExecuteNonQuery();
                                    }
                                    #endregion

                                    #region "Cek Payment"
                                    cmd.CommandText = "CEKPAYMENT";
                                    cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, item.InvNo);
                                    cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, paymentH.Customer.CUSTCode);
                                    cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, paymentH.Ccy.CurrencyCode);
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.Open();
                                    //cmd.BeginTransaction();
                                    cmd.ExecuteNonQuery();
                                    #endregion //Cek Payment
                                }
                            }
                            #endregion

                            #region "Cleanup Process"
                            cmd.CommandText = @"UPDATE PAYMENTHEADER SET VOUCHER = @voucher, flag=1
                                                        WHERE SERIALNO = @serialNo";
                            cmd.AddParameter("@voucher", System.Data.SqlDbType.VarChar, voc);
                            cmd.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, paymentH.SerialNo);
                            cmd.CommandType = System.Data.CommandType.Text;
                            //cmd.BeginTransaction();
                            cmd.ExecuteNonQuery();

                            cmd.CommitTransaction();
                            #endregion //Cleanup Process
                            #endregion
                        }
                    }
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    result = sex.Message;
                    resultCount = 0;
                }
                finally
                {
                    cmd.Close();
                }
            }

            if (resultCount > 0)
            {
                result = "Process Success";
            }

            return result;
        }

        public string ReverseJournal(string[] serialNo, string user, string branch)
        {
            string result = "";
            int resultCount = 0;

            if (serialNo == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    for (int i = 0; i < serialNo.Length; i++)
                    {
                        IDS.Sales.PaymentH paymentH = IDS.Sales.PaymentH.GetSalesPaymentHWithDetail(serialNo[i]);

                        if (!string.IsNullOrEmpty(paymentH.SerialNo))
                        {
                            if (Convert.ToInt16(paymentH.Flag) > 1)
                            {
                                result = "Payment with Serial No " + serialNo[i] + " is already canceled.";
                            }
                            else
                            {
                                cmd.CommandText = "reversePayment";
                                cmd.AddParameter("@scode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(paymentH.SCode));
                                cmd.AddParameter("@paymentNo", System.Data.SqlDbType.VarChar, serialNo[i]);
                                cmd.AddParameter("@user", System.Data.SqlDbType.VarChar, user);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Open();
                                cmd.BeginTransaction();
                                resultCount = cmd.ExecuteNonQuery();
                                cmd.CommitTransaction();

                            }

                            IDS.Sales.OutstandingRecalculate.NewSPBasedCalculator.Recalculate(paymentH.Branch.BranchCode, paymentH.Customer.CUSTCode,paymentH.Ccy.CurrencyCode,paymentH.PayDate, null);
                            resultCount = 1;
                        }
                    }

                    
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    result = sex.Message;
                    resultCount = 0;
                }
                finally
                {
                    cmd.Close();
                }
            }

            if (resultCount > 0)
            {
                if (string.IsNullOrEmpty(result))
                {
                    result = "Cancel Receive Success";
                }
            }

            return result;
        }

        public string GenerateSerialNumber()
        {
            string result = "";

            try
            {
                using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                {
                    try
                    {
                        db.CommandText = "SELECT dbo.GeneratePaymentSerialNo(@serial)";
                        db.AddParameter("@serial", System.Data.SqlDbType.VarChar, "RCV" + PayDate.ToString("yyMM"));
                        db.CommandType = System.Data.CommandType.Text;
                        db.Open();

                        result = db.ExecuteScalar().ToString();
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
            return result;
        }

        public string GenerateVoucher(DateTime date, string account)
        {
            string result = "";

            try
            {
                using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                {
                    try
                    {
                        db.CommandText = "SELECT dbo.GenerateVoucher(@acc, @date)";
                        db.AddParameter("@acc", System.Data.SqlDbType.VarChar, account);
                        db.AddParameter("@date", System.Data.SqlDbType.DateTime, date);
                        db.CommandType = System.Data.CommandType.Text;
                        db.Open();

                        result = date.ToString("yyMM") + Convert.ToInt64(db.ExecuteScalar()).ToString("000");
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
            return result;
        }
    }
}
