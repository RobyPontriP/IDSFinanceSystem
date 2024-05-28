using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class CustomerOutstanding
    {
        [Display(Name = "Customer")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer Code is required")]
        public GeneralTable.Customer Customer { get; set; }

        [Display(Name = "Period")]
        public string Period { get; set; }

        [Display(Name = "Month")]
        public string Month { get; set; }

        [Display(Name = "Year")]
        public string Year { get; set; }

        [Display(Name = "Currency Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        public IDS.GeneralTable.Currency CCy { get; set; }

        [Display(Name = "Beginning Balance")]
        public decimal BegBal { get; set; }

        [Display(Name = "Debit")]
        public decimal Debit { get; set; }

        [Display(Name = "Credit")]
        public decimal Credit { get; set; }

        [Display(Name = "Ending Balance")]
        public decimal EndBal { get; set; }

        [Display(Name = "Closing")]
        public bool Closing { get; set; }

        public CustomerOutstanding()
        {

        }

        public static List<CustomerOutstanding> GetCustOutstanding(string period)
        {
            List<IDS.Sales.CustomerOutstanding> list = new List<CustomerOutstanding>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelCustOutstanding";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            CustomerOutstanding custOuts = new CustomerOutstanding();

                            custOuts.Customer = new IDS.GeneralTable.Customer();
                            custOuts.Customer.CUSTCode = dr["custCode"] as string;

                            custOuts.Period = dr["Period"] as string;
                            custOuts.Year = dr["Period"].ToString().Substring(0, 4);
                            custOuts.Month = dr["Period"].ToString().Substring(4,2);
                            
                            custOuts.CCy = new GeneralTable.Currency();
                            custOuts.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            custOuts.BegBal = Tool.GeneralHelper.NullToDecimal(dr["BeginningBalance"],0);
                            custOuts.Debit = Tool.GeneralHelper.NullToDecimal(dr["Debit"], 0);
                            custOuts.Credit = Tool.GeneralHelper.NullToDecimal(dr["Credit"], 0);
                            custOuts.EndBal = Tool.GeneralHelper.NullToDecimal(dr["EndingBalance"], 0);
                            custOuts.Closing = Tool.GeneralHelper.NullToBool(dr["Closing"]);

                            list.Add(custOuts);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static void Recalculate(string branch, string cust, string ccy, DateTime periodDate,bool sp,string operatorID)
        {
            List<IDS.Sales.CustomerOutstanding> listCustOuts = new List<CustomerOutstanding>();
            string period = periodDate.ToString("yyyyMM");
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    if (sp)
                    {
                    db.CommandText = "SlsProcReCalcCustOut";
                    //db.DbCommand.CommandTimeout = 0;
                    db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                    db.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
                    db.AddParameter("@vdate", System.Data.SqlDbType.DateTime, periodDate);
                    db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, operatorID);
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    db.Open();

                    db.BeginTransaction();
                    db.ExecuteNonQuery();
                    db.CommitTransaction();
                    }
                    else
                    {
                    db.CommandText = "SalesSelCustOutstanding";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                    db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                    db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, cust);
                    db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                    db.Open();

                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {

                            while (dr.Read())
                            {
                                CustomerOutstanding custOuts = new CustomerOutstanding();

                                custOuts.Customer = new IDS.GeneralTable.Customer();
                                custOuts.Customer.CUSTCode = dr["custCode"] as string;

                                custOuts.Period = dr["Period"] as string;
                                custOuts.Year = dr["Period"].ToString().Substring(0, 4);
                                custOuts.Month = dr["Period"].ToString().Substring(4, 2);

                                custOuts.CCy = new GeneralTable.Currency();
                                custOuts.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                                custOuts.BegBal = Tool.GeneralHelper.NullToDecimal(dr["BeginningBalance"], 0);
                                custOuts.Debit = Tool.GeneralHelper.NullToDecimal(dr["Debit"], 0);
                                custOuts.Credit = Tool.GeneralHelper.NullToDecimal(dr["Credit"], 0);
                                //custOuts.EndBal = Tool.GeneralHelper.NullToDecimal(dr["EndingBalance"], 0);
                                custOuts.Closing = Tool.GeneralHelper.NullToBool(dr["Closing"]);

                                listCustOuts.Add(custOuts);
                            }
                        }

                        if (!dr.IsClosed)
                            dr.Close();
                    }
                    db.Close();
                    //
                    
                    db.Close();

                    if (listCustOuts.Count >0)
                    {
                        foreach (var item in listCustOuts)
                        {
                            #region Calculate Credit Data
                            db.CommandText = "AdjustCustOutstanding";
                            db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 4);
                            db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, item.Customer.CUSTCode);
                            db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, item.CCy.CurrencyCode);
                            db.AddParameter("@period", System.Data.SqlDbType.VarChar, item.Period);
                            db.CommandType = System.Data.CommandType.StoredProcedure;
                            db.Open();

                            db.BeginTransaction();
                            db.ExecuteNonQuery();
                            db.CommitTransaction();

                            #region Calculate Payment
                            List<IDS.Sales.PaymentD> listPaymentD = IDS.Sales.PaymentD.GetPaymentDetail(item.Customer.CUSTCode, item.CCy.CurrencyCode, item.Period);// new List<IDS.Sales.PaymentD>();
                            foreach (var itemPayment in listPaymentD)
                            {
                                decimal paid = IDS.Sales.PaymentD.GetTotalAllocationAmount(itemPayment.SerialNo, item.CCy.CurrencyCode);

                                IDS.Sales.Invoice.RecalculatePaidAmount(itemPayment.SerialNo, item.CCy.CurrencyCode, item.Customer.CUSTCode, paid);

                                RecalculateARAPPaidAmount(itemPayment.SerialNo, item.CCy.CurrencyCode, item.Customer.CUSTCode, paid);

                                CalculateOutstandingCredit(item.Customer.CUSTCode, item.Period,item.CCy.CurrencyCode, itemPayment.AlloAmount);
                            }
                            #endregion

                            #region Calculate Void Invoice
                            List<IDS.Sales.Invoice> listVoidInvoice = IDS.Sales.Invoice.GetSalesVoidInvoice(item.Customer.CUSTCode, item.Period, item.CCy.CurrencyCode);// new List<IDS.Sales.PaymentD>();

                            foreach (var itemVoidInvoice in listVoidInvoice)
                            {
                                decimal paid = IDS.Sales.PaymentD.GetTotalAllocationAmount(itemVoidInvoice.InvoiceNumber, item.CCy.CurrencyCode);

                                IDS.Sales.Invoice.RecalculatePaidAmount(itemVoidInvoice.InvoiceNumber, item.CCy.CurrencyCode, item.Customer.CUSTCode, paid);

                                RecalculateARAPPaidAmount(itemVoidInvoice.InvoiceNumber, item.CCy.CurrencyCode, item.Customer.CUSTCode, paid);

                                CalculateOutstandingCredit(item.Customer.CUSTCode, item.Period, item.CCy.CurrencyCode, itemVoidInvoice.InvoiceTotal);
                            }

                            #endregion

                            #region Calculate Debit Data
                            ClearOutstandingDebit(item.Customer.CUSTCode,item.Period,item.CCy.CurrencyCode);

                            #region Calculate New AR
                            List<IDS.Sales.Invoice> listInvoice = IDS.Sales.Invoice.GetSalesInvoice(item.Customer.CUSTCode, item.Period, item.CCy.CurrencyCode);// new List<IDS.Sales.PaymentD>();

                            foreach (var itemSalesInvoice in listInvoice)
                            {
                                decimal paid = IDS.Sales.PaymentD.GetTotalAllocationAmount(itemSalesInvoice.InvoiceNumber, item.CCy.CurrencyCode);

                                IDS.Sales.Invoice.RecalculatePaidAmount(itemSalesInvoice.InvoiceNumber, item.CCy.CurrencyCode, item.Customer.CUSTCode, paid);

                                RecalculateARAPPaidAmount(itemSalesInvoice.InvoiceNumber, item.CCy.CurrencyCode, item.Customer.CUSTCode, paid);

                                CalculateOutstandingDebit(item.Customer.CUSTCode, item.Period, item.CCy.CurrencyCode, itemSalesInvoice.InvoiceTotal);
                            }
                            #endregion
                            #endregion


                            #endregion


                        }

                    }
                    }

                }
                catch (SqlException sex)
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();
                }
            }
        }

        public static int RecalculateARAPPaidAmount(string serialNo, string ccy, string cust, decimal paid)
        {
            int result = 0;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"UPDATE ACFARAP SET PAYMENT = @PAID
		WHERE CUST_PRIN = @CUST 
        AND CCY=@CCY 
        AND DOC_NO = @INVNO";
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

        public static int CalculateOutstandingCredit(string cust, string period,string ccy, decimal paid)
        {
            int result = 0;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "AdjustCustOutstanding";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 5);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@paid", System.Data.SqlDbType.Money, paid);
                db.Open();

                db.BeginTransaction();
                result = db.ExecuteNonQuery();
                db.CommitTransaction();

                db.Close();
            }

            return result;
        }

        public static int CalculateOutstandingDebit(string cust, string period, string ccy, decimal outs)
        {
            int result = 0;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "AdjustCustOutstanding";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 8);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@outs", System.Data.SqlDbType.Money, outs);
                db.Open();
                db.BeginTransaction();
                result = db.ExecuteNonQuery();
                db.CommitTransaction();

                db.Close();
            }

            return result;
        }

        public static int ClearOutstandingDebit(string cust, string period, string ccy)
        {
            int result = 0;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "AdjustCustOutstanding";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 6);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.Open();
                db.BeginTransaction();
                result = db.ExecuteNonQuery();
                db.CommitTransaction();
                db.Close();
            }

            return result;
        }
    }
}
