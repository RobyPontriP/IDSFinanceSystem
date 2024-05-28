using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class OutstandingRecalculate
    {
           
        #region SQL Commands
        private SqlCommand CUSTOMER_LIST =  new SqlCommand(@"SELECT * FROM CustomerOutstanding AS c
		WHERE c.custCode LIKE ISNULL(@CUST, '%') 
        AND c.CCY LIKE ISNULL(@CCY, '%')
        AND c.Period = @PERIOD);");
        //AND c.Period = CONVERT(VARCHAR(6), @PERIOD, 112);");

        //    private SqlCommand PAYMENT_LIST = SQLHelper.CreateCommand(@"SELECT d.*
        //		FROM paymentDetail AS d LEFT JOIN paymentHeader AS h
        //			ON d.serialNo = h.serialNo LEFT JOIN SLSInvH AS i
        //            ON d.invNo = i.InvoiceNumber
        //		WHERE CONVERT(VARCHAR(6), h.PayDate, 112) = CONVERT(VARCHAR(6), @PERIOD, 112) 
        //			AND d.alloType = 1 AND ISNULL(i.StatusINV, 0) < 2 AND h.CustSuppCode = @CUST AND d.CCY=@CCY;", System.Data.CommandType.Text);

        //      private SqlCommand PAYMENT_LIST = new SqlCommand(@"SELECT d.*
        //FROM paymentDetail AS d LEFT JOIN paymentHeader AS h
        //	ON d.serialNo = h.serialNo LEFT JOIN SLSInvH AS i
        //          ON d.invNo = i.InvoiceNumber
        //WHERE CONVERT(VARCHAR(6), h.PayDate, 112) = CONVERT(VARCHAR(6), @PERIOD, 112) 
        //	AND (d.alloType = 1 OR d.alloType = 6) AND ISNULL(i.StatusINV, 0) < 2 AND h.CustSuppCode = @CUST AND d.CCY=@CCY;");
        private SqlCommand PAYMENT_LIST = new SqlCommand(@"SELECT d.*
		FROM paymentDetail AS d LEFT JOIN paymentHeader AS h
			ON d.serialNo = h.serialNo LEFT JOIN SLSInvH AS i
            ON d.invNo = i.InvoiceNumber
		WHERE CONVERT(VARCHAR(6), h.PayDate, 112) =  @PERIOD
			AND (d.alloType = 1 OR d.alloType = 6) AND ISNULL(i.StatusINV, 0) < 2 AND h.CustSuppCode = @CUST AND d.CCY=@CCY;");

        //      private SqlCommand INVOICE_LIST = new SqlCommand(@"SELECT *, ((InvoiceAmount - DiscountAmount) + PPnAmount) AS TotalInvoice
        //FROM SlsInvH AS i
        //WHERE CONVERT(VARCHAR(6), i.InvoiceDate, 112) = CONVERT(VARCHAR(6), @PERIOD, 112) 
        //	AND i.CustCode = @CUST AND ISNULL(i.StatusINV, 0) <= 2 AND i.CCY=@CCY;");
        private SqlCommand INVOICE_LIST = new SqlCommand(@"SELECT *, ((InvoiceAmount - DiscountAmount) + PPnAmount) AS TotalInvoice
		FROM SlsInvH AS i
		WHERE CONVERT(VARCHAR(6), i.InvoiceDate, 112) = @PERIOD
			AND i.CustCode = @CUST AND ISNULL(i.StatusINV, 0) <= 2 AND i.CCY=@CCY;");

        //      private SqlCommand VOID_INVOICE_LIST = new SqlCommand(@"SELECT *, (((InvoiceAmount - DiscountAmount) + PPnAmount) * -1) AS TotalInvoice
        //FROM SlsInvH AS i
        //WHERE CONVERT(VARCHAR(6), i.InvoiceDate, 112) = CONVERT(VARCHAR(6), @PERIOD, 112) 
        //	AND i.CustCode = @CUST AND ISNULL(i.StatusINV, 0) = 3 AND i.CCY=@CCY;");
        private SqlCommand VOID_INVOICE_LIST = new SqlCommand(@"SELECT *, (((InvoiceAmount - DiscountAmount) + PPnAmount) * -1) AS TotalInvoice
		FROM SlsInvH AS i
		WHERE CONVERT(VARCHAR(6), i.InvoiceDate, 112) = @PERIOD
			AND i.CustCode = @CUST AND ISNULL(i.StatusINV, 0) = 3 AND i.CCY=@CCY;");

        private SqlCommand INVOICE_PAYMENT = new SqlCommand(@"SELECT SUM(d.alloAmount)
		FROM paymentDetail AS d
		WHERE d.invNo = @INVNO 
			AND (d.alloType = 1 OR d.alloType = 6) AND d.CCY=@CCY;");

        private SqlCommand CLEAR_OUTSTANDING_CREDIT_DATA =
            new SqlCommand(@"UPDATE CustomerOutstanding SET Credit = $0 
WHERE custCode = @CUST AND CCY=@CCY AND Period = @PERIOD");
        //WHERE custCode = @CUST AND CCY=@CCY AND Period = CONVERT(VARCHAR(6), @PERIOD, 112)");

        private SqlCommand CLEAR_OUTSTANDING_DEBIT_DATA =
            new SqlCommand(@"UPDATE CustomerOutstanding SET Debit = $0 
WHERE custCode = @CUST AND CCY=@CCY AND Period = @PERIOD)");
        //WHERE custCode = @CUST AND CCY=@CCY AND Period = CONVERT(VARCHAR(6), @PERIOD, 112)");

        private SqlCommand CALC_OUTSTANDING_CREDIT =
            new SqlCommand(@"UPDATE CustomerOutstanding SET Credit = Credit + @PAID 
WHERE custCode = @CUST AND CCY=@CCY AND Period = @PERIOD");
        //WHERE custCode = @CUST AND CCY=@CCY AND Period = CONVERT(VARCHAR(6), @PERIOD, 112)");

        private SqlCommand CALC_OUTSTANDING_DEBIT =
            new SqlCommand(@"UPDATE CustomerOutstanding SET Debit = Debit + @OUTS 
WHERE custCode = @CUST AND CCY=@CCY AND Period = @PERIOD");
        //WHERE custCode = @CUST AND CCY=@CCY AND Period = CONVERT(VARCHAR(6), @PERIOD, 112)");

        private SqlCommand RECALC_PAIDAMOUNT_DATA = new SqlCommand(@"UPDATE SLSInvH SET PaidAmount = @PAID, OutstandingAmount = ((InvoiceAmount - DiscountAmount) + PPnAmount) - @PAID WHERE custCode = @CUST  AND CCY=@CCY  AND InvoiceNumber = @INVNO");


        private SqlCommand RECALC_ARAP_PAIDAMOUNT_DATA =
            new SqlCommand(@"UPDATE ACFARAP SET PAYMENT = @PAID
		WHERE CUST_PRIN = @CUST 
        AND CCY=@CCY 
        AND DOC_NO = @INVNO ");
        #endregion

        private SqlCommand SPCall = new SqlCommand("SlsProcReCalcCustOut");

        private DbTransaction TRANS = null;

        private bool SP = false;

        public static OutstandingRecalculate NewCalculator
        {
            get
            {
                return new OutstandingRecalculate(false);
            }
        }

        public static OutstandingRecalculate NewSPBasedCalculator
        {
            get
            {
                return new OutstandingRecalculate(true);
            }
        }

        private OutstandingRecalculate(bool SP)
        {
            this.SP = SP;
        }

        public void Recalculate(object branch, object ccy)
        {
            this.Recalculate(branch, null, ccy, DateTime.Now);
        }

        public void Recalculate(object branch, object cust, object ccy)
        {
            this.Recalculate(branch, cust, ccy, DateTime.Now);
        }

        public void Recalculate(object branch, object cust, object ccy, object period)
        {
            DateTime datePeriod = DateTime.Now;

            if (string.IsNullOrEmpty(period.ToString()))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }

            using (IDS.DataAccess.SqlServer cmd = new DataAccess.SqlServer())
            {
                if (TRANS != null)
                {
                    cmd.Transaction = TRANS;
                }
                else
                {
                    cmd.Open();
                    cmd.BeginTransaction();
                }

                try
                {
                    if (SP)
                    {
                        cmd.CommandText = SPCall.CommandText;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.AddParameter("@Cust",System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(IDS.Tool.GeneralHelper.NullToString(cust,"")));
                        cmd.AddParameter("@Branch", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(IDS.Tool.GeneralHelper.NullToString(branch,"")));
                        cmd.AddParameter("@vDate", System.Data.SqlDbType.DateTime, Convert.ToDateTime(datePeriod));
                        cmd.Open();
                        cmd.ExecuteNonQuery();
                        //cmd.Close();
                    //    TRANS.Execute(SPCall,
                    //        new SqlParameter("@Cust", cust == null ? DBNull.Value : cust),
                    //        new SqlParameter("@Branch", branch == null ? DBNull.Value : branch),
                    //        new SqlParameter("@vDate", period == null ? DBNull.Value : period));
                    }
                    else
                    {
                        cmd.CommandText = CUSTOMER_LIST.CommandText;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(IDS.Tool.GeneralHelper.NullToString(cust, "")));
                        cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(IDS.Tool.GeneralHelper.NullToString(ccy, "")));
                        cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, period == null ? "" : Convert.ToDateTime(period).ToString("yyyyMM"));
                        cmd.Open();
                        System.Data.DataTable dt = cmd.GetDataTable();

                            
                            //TRANS.FillData(ref dt, CUSTOMER_LIST,
                            //new SqlParameter("@CUST", cust == null ? DBNull.Value : cust),
                            //new SqlParameter("@CCY", ccy == null ? DBNull.Value : ccy),
                            //new SqlParameter("@PERIOD", period == null ? DBNull.Value : period));
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            #region Calculate Credit Data
                            cmd.CommandText = CLEAR_OUTSTANDING_CREDIT_DATA.CommandText;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"],""));
                            cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                            cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, period == null ? "" : Convert.ToDateTime(dr["Period"]).ToString("yyyyMM"));
                            cmd.Open();
                            cmd.ExecuteNonQuery();

                            //TRANS.Execute(CLEAR_OUTSTANDING_CREDIT_DATA,
                            //    new SqlParameter("@CUST", dr["custCode"]),
                            //    new SqlParameter("@CCY", dr["CCY"]),
                            //    new SqlParameter("@PERIOD", dr["Period"]));

                            #region Calculate Payment
                            cmd.CommandText = CUSTOMER_LIST.CommandText;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(IDS.Tool.GeneralHelper.NullToString(dr["custCode"], "")));
                            cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(IDS.Tool.GeneralHelper.NullToString(dr["CCY"], "")));
                            cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, period == null ? "" : Convert.ToDateTime(dr["Period"]).ToString("yyyyMM"));
                            cmd.Open();
                            System.Data.DataTable dtp = cmd.GetDataTable();

                            //DataTable dtp = new DataTable();
                            //TRANS.FillData(ref dtp, PAYMENT_LIST,
                            //    new SqlParameter("@CUST", dr["custCode"]),
                            //    new SqlParameter("@CCY", dr["CCY"]),
                            //    new SqlParameter("@PERIOD", dr["Period"]));
                            foreach (System.Data.DataRow drp in dtp.Rows)
                            {
                                cmd.CommandText = INVOICE_PAYMENT.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(drp["invNo"], ""));
                                cmd.Open();
                                object paid = cmd.ExecuteScalar();

                                //object paid = TRANS.ExecuteScalar(INVOICE_PAYMENT,
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@INVNO", drp["invNo"]));

                                cmd.CommandText = RECALC_PAIDAMOUNT_DATA.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(drp["invNo"], ""));
                                cmd.AddParameter("@PAID", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToDecimal(paid, 0m));
                                cmd.Open();
                                cmd.ExecuteNonQuery();

                                //TRANS.Execute(RECALC_PAIDAMOUNT_DATA,
                                //    new SqlParameter("@CUST", dr["custCode"]),
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@INVNO", drp["invNo"]),
                                //    new SqlParameter("@PAID", paid == null || paid is DBNull ? 0m : paid));

                                cmd.CommandText = RECALC_ARAP_PAIDAMOUNT_DATA.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(drp["invNo"], ""));
                                cmd.AddParameter("@PAID", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToDecimal(paid, 0m));
                                cmd.Open();
                                cmd.ExecuteNonQuery();

                                //TRANS.Execute(RECALC_ARAP_PAIDAMOUNT_DATA,
                                //    new SqlParameter("@CUST", dr["custCode"]),
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@INVNO", drp["invNo"]),
                                //    new SqlParameter("@PAID", paid == null || paid is DBNull ? 0m : paid));

                                cmd.CommandText = CALC_OUTSTANDING_CREDIT.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@PAID", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToDecimal(drp["alloAmount"], 0m));
                                cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["Period"], ""));
                                cmd.Open();
                                cmd.ExecuteNonQuery();

                                //TRANS.Execute(CALC_OUTSTANDING_CREDIT,
                                //    new SqlParameter("@CUST", dr["custCode"]),
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@PAID", drp["alloAmount"]),
                                //    new SqlParameter("@PERIOD", dr["Period"]));
                            }
                            #endregion

                            #region Calculate Void Invoice
                            cmd.CommandText = VOID_INVOICE_LIST.CommandText;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                            cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                            cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, period == null ? "" : Convert.ToDateTime(dr["Period"]).ToString("yyyyMM"));
                            cmd.Open();
                            System.Data.DataTable dtv = cmd.GetDataTable();
                            //DataTable dtv = new DataTable();
                            //TRANS.FillData(ref dtv, VOID_INVOICE_LIST,
                            //    new SqlParameter("@CUST", dr["custCode"]),
                            //    new SqlParameter("@CCY", dr["CCY"]),
                            //    new SqlParameter("@PERIOD", dr["Period"]));
                            foreach (System.Data.DataRow drv in dtv.Rows)
                            {
                                cmd.CommandText = INVOICE_PAYMENT.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(drv["InvoiceNumber"], ""));
                                cmd.Open();
                                object paid = cmd.ExecuteScalar();
                                //object paid = TRANS.ExecuteScalar(INVOICE_PAYMENT,
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@INVNO", drv["InvoiceNumber"]));

                                cmd.CommandText = RECALC_PAIDAMOUNT_DATA.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(drv["InvoiceNumber"], ""));
                                cmd.AddParameter("@PAID", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToDecimal(paid, 0m));
                                cmd.Open();
                                cmd.ExecuteNonQuery();
                                //TRANS.Execute(RECALC_PAIDAMOUNT_DATA,
                                //    new SqlParameter("@CUST", dr["custCode"]),
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@INVNO", drv["InvoiceNumber"]),
                                //    new SqlParameter("@PAID", paid == null || paid is DBNull ? 0m : paid));

                                cmd.CommandText = RECALC_ARAP_PAIDAMOUNT_DATA.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(drv["InvoiceNumber"], ""));
                                cmd.AddParameter("@PAID", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToDecimal(paid, 0m));
                                cmd.Open();
                                cmd.ExecuteNonQuery();
                                //TRANS.Execute(RECALC_ARAP_PAIDAMOUNT_DATA,
                                //    new SqlParameter("@CUST", dr["custCode"]),
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@INVNO", drv["InvoiceNumber"]),
                                //    new SqlParameter("@PAID", paid == null || paid is DBNull ? 0m : paid));

                                cmd.CommandText = CALC_OUTSTANDING_CREDIT.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@PAID", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToDecimal(drv["TotalInvoice"], 0m));
                                cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["Period"], ""));
                                cmd.Open();
                                cmd.ExecuteNonQuery();
                                //TRANS.Execute(CALC_OUTSTANDING_CREDIT,
                                //    new SqlParameter("@CUST", dr["custCode"]),
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@PAID", drv["TotalInvoice"]),
                                //    new SqlParameter("@PERIOD", dr["Period"]));
                            }
                            #endregion

                            #endregion

                            #region Calculate Debit Data
                            cmd.CommandText = CLEAR_OUTSTANDING_DEBIT_DATA.CommandText;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                            cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                            cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["Period"], ""));
                            cmd.Open();
                            cmd.ExecuteNonQuery();
                            //TRANS.Execute(CLEAR_OUTSTANDING_DEBIT_DATA,
                            //    new SqlParameter("@CUST", dr["custCode"]),
                            //    new SqlParameter("@CCY", dr["CCY"]),
                            //    new SqlParameter("@PERIOD", dr["Period"]));

                            #region Calculate New AR
                            cmd.CommandText = INVOICE_LIST.CommandText;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                            cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                            cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, period == null ? "" : Convert.ToDateTime(dr["Period"]).ToString("yyyyMM"));
                            cmd.Open();
                            System.Data.DataTable dti = cmd.GetDataTable();
                            //DataTable dti = new DataTable();
                            //TRANS.FillData(ref dti, INVOICE_LIST,
                            //    new SqlParameter("@CUST", dr["custCode"]),
                            //    new SqlParameter("@CCY", dr["CCY"]),
                            //    new SqlParameter("@PERIOD", dr["Period"]));
                            foreach (System.Data.DataRow dri in dti.Rows)
                            {
                                cmd.CommandText = INVOICE_PAYMENT.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dri["InvoiceNumber"], ""));
                                cmd.Open();
                                object paid = cmd.ExecuteScalar();
                                //object paid = TRANS.ExecuteScalar(INVOICE_PAYMENT,
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@INVNO", dri["InvoiceNumber"]));

                                cmd.CommandText = RECALC_PAIDAMOUNT_DATA.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dri["InvoiceNumber"], ""));
                                cmd.AddParameter("@PAID", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToDecimal(paid, 0m));
                                cmd.Open();
                                cmd.ExecuteNonQuery();
                                //TRANS.Execute(RECALC_PAIDAMOUNT_DATA,
                                //    new SqlParameter("@CUST", dr["custCode"]),
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@INVNO", dri["InvoiceNumber"]),
                                //    new SqlParameter("@PAID", paid == null || paid is DBNull ? 0m : paid));

                                cmd.CommandText = RECALC_ARAP_PAIDAMOUNT_DATA.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dri["InvoiceNumber"], ""));
                                cmd.AddParameter("@PAID", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToDecimal(paid, 0m));
                                cmd.Open();
                                cmd.ExecuteNonQuery();
                                //TRANS.Execute(RECALC_ARAP_PAIDAMOUNT_DATA,
                                //    new SqlParameter("@CUST", dr["custCode"]),
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@INVNO", dri["InvoiceNumber"]),
                                //    new SqlParameter("@PAID", paid == null || paid is DBNull ? 0m : paid));

                                cmd.CommandText = CALC_OUTSTANDING_DEBIT.CommandText;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.AddParameter("@CUST", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["custCode"], ""));
                                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["CCY"], ""));
                                cmd.AddParameter("@OUTS", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dri["TotalInvoice"], ""));
                                cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.NullToString(dr["Period"], ""));
                                cmd.Open();
                                cmd.ExecuteNonQuery();
                                //TRANS.Execute(CALC_OUTSTANDING_DEBIT,
                                //    new SqlParameter("@CUST", dr["custCode"]),
                                //    new SqlParameter("@CCY", dr["CCY"]),
                                //    new SqlParameter("@OUTS", dri["TotalInvoice"]),
                                //    new SqlParameter("@PERIOD", dr["Period"]));
                            }
                            #endregion
                            #endregion
                        }
                    }
                    if (TRANS!= null)
                    {
                        TRANS.Commit();
                    }
                    else
                    {
                        cmd.CommitTransaction();
                    }
                }
                catch (Exception ex)
                {
                    TRANS.Rollback();
                    TRANS = null;

                    cmd.RollbackTransaction();

                    throw new Exception(ex.Message, ex);
                }
                TRANS = null;
            }
            
        }

        public void Recalculate(object branch, object ccy, DbTransaction trans)
        {
            this.Recalculate(branch, null, ccy, DateTime.Now, trans);
        }

        public void Recalculate(object branch, object cust, object ccy, DbTransaction trans)
        {
            this.Recalculate(branch, cust, ccy, DateTime.Now, trans);
        }

        public void Recalculate(object branch, object cust, object ccy, object period, DbTransaction trans)
        {
            TRANS = trans;
            this.Recalculate(branch, cust, ccy, period);
        }
    }


}
