using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class PaymentD
    {
        [Display(Name = "Serial No")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Serial No is Required")]
        [MaxLength(50)]
        public string SerialNo { get; set; }

        public int SeqNo { get; set; }

        public int AlloType { get; set; }

        public string InvNo { get; set; }
        //public DateTime InvoiceDate { get; set; }
        public Sales.Invoice Invoice { get; set; }

        public decimal Amount { get; set; }

        public decimal AlloAmount { get; set; }

        public decimal EquivAmount { get; set; }

        public decimal ExchRate { get; set; }

        public IDS.GLTable.ChartOfAccount Acc { get; set; }

        public IDS.GeneralTable.Currency Ccy { get; set; }

        public string Type { get; set; }

        public PaymentD()
        {

        }

        public static List<PaymentD> GetPaymentDetail(string serialNo, int type)
        {
            List<PaymentD> paymentD = new List<PaymentD>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "findPaymentDetail";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@paymentNo", System.Data.SqlDbType.VarChar, serialNo);
                db.AddParameter("@type", System.Data.SqlDbType.Int, type);
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
                            pd.SeqNo = Tool.GeneralHelper.NullToInt(dr["seq"],0);
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
            }

            return paymentD;
        }

        public static List<PaymentD> GetPaymentDetail(string cust,string ccy,string period)
        {
            List<PaymentD> paymentD = new List<PaymentD>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelPaymentD";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@type", System.Data.SqlDbType.Int, 3);
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
                            //pd.Type = Tool.GeneralHelper.NullToString(dr["Type"]);

                            //pd.Acc = new GLTable.ChartOfAccount();
                            //pd.Acc.Account = Tool.GeneralHelper.NullToString(dr["accountno"]);

                            paymentD.Add(pd);
                        }
                    }

                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }

                db.Close();
            }

            return paymentD;
        }

        public static List<System.Web.Mvc.SelectListItem> GetAllocationTypeForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> paymentType = new List<System.Web.Mvc.SelectListItem>();
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "Receivable", Value = ((int)IDS.Tool.AllocationType.Receivable).ToString().ToString() });
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "PPh", Value = ((int)IDS.Tool.AllocationType.Tax).ToString() });
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "Bank Charges", Value = ((int)IDS.Tool.AllocationType.BankCharges).ToString() });
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "Others", Value = ((int)IDS.Tool.AllocationType.Others).ToString() });
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "Gain/Loss", Value = ((int)IDS.Tool.AllocationType.GainLoss).ToString() });
            paymentType.Add(new System.Web.Mvc.SelectListItem() { Text = "PPn", Value = ((int)IDS.Tool.AllocationType.PPn).ToString() });

            return paymentType;
        }

        public static decimal GetTotalAllocationAmount(string serialNo, string ccy)
        {
            decimal result = 0;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"SELECT SUM(d.alloAmount)
		FROM paymentDetail AS d
		WHERE d.invNo = @INVNO 
			AND (d.alloType = 1 OR d.alloType = 6) AND d.CCY=@CCY";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, serialNo);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.Open();

                result =  IDS.Tool.GeneralHelper.NullToDecimal(db.ExecuteScalar(),0);

                

                db.Close();
            }

            return result;
        }
    }

    
}
