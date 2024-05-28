using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
   public class INVPRINT
    {
        public Invoice Invoice { get; set; }
        public IDS.GeneralTable.Customer Customer { get; set; }
        public PaymentH PaymentH { get; set; }
        public PaymentD PaymentD { get; set; }

        public INVPRINT() {
        }

        public static List<INVPRINT> GetINVPRINT()
        {
            List<INVPRINT> list = new List<INVPRINT>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"SELECT        h.InvoiceNumber, h.RefNo, h.InvoiceDate, h.ProjectCode, h.BranchCode, h.CustCode, h.DiscountAmount, h.InvoiceAmount, h.TermOfPayment, h.PaidAmount, h.OutstandingAmount, h.StatusINV, h.isPPh, h.PPhPercentage, 
                         h.PPhNo, h.PPhAmount, h.PPhDateReceive, h.isPPn, h.PPnAmount, h.PPnNo, h.TaxStatus, h.PaymentStatus, h.PayDate, h.Voucher, h.ACC, h.CCY, h.ExchangeRate, h.EquivAmt, h.StatusARAP, h.OperatorID, h.LastUPDATE, 
                         h.InvoiceRole, h.BuktiPotongPPh, h.Description, c.GOVPRIVATE, d.serialNo, (CASE ISNULL(c.GOVPRIVATE, 0) WHEN 0 THEN p.baseAmount ELSE h.InvoiceAmount - h.DiscountAmount + h.PPnAmount END) AS baseAmount
FROM            dbo.SLSInvH AS h LEFT OUTER JOIN
                         dbo.ACFCUST AS c ON h.CustCode = c.CUST AND c.GOVPRIVATE = 1 LEFT OUTER JOIN
                         dbo.paymentDetail AS d ON d.invNo = h.InvoiceNumber AND d.alloType = 1 LEFT OUTER JOIN
                         dbo.paymentHeader AS p ON d.serialNo = p.serialNo
WHERE        (c.GOVPRIVATE = 1) OR
                         (NOT (d.serialNo IS NULL)) AND (h.StatusINV <= 1)";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            INVPRINT invprint = new INVPRINT();
                            invprint = new INVPRINT();
                            invprint.Invoice = new Invoice();
                            invprint.Invoice.RefNo = dr["RefNo"] as string;
                            invprint.Invoice.InvoiceDate = IDS.Tool.GeneralHelper.NullToDateTime(dr["InvoiceDate"], DateTime.Now);
                            invprint.Invoice.CustProject = new CustProject();
                            invprint.Invoice.CustProject.CustProjCode = IDS.Tool.GeneralHelper.NullToString(dr["ProjectCode"]);
                            invprint.Invoice.Branch = new GeneralTable.Branch();
                            invprint.Invoice.Branch.BranchCode = IDS.Tool.GeneralHelper.NullToString(dr["BranchCode"]);
                            invprint.Invoice.Cust = new GeneralTable.Customer();
                            invprint.Invoice.Cust.CUSTCode = IDS.Tool.GeneralHelper.NullToString(dr["CustCode"]);
                            invprint.Invoice.DiscountAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["DiscountAmount"],0);
                            invprint.Invoice.InvoiceAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["InvoiceAmount"], 0);
                            invprint.Invoice.TermOfPayment = IDS.Tool.GeneralHelper.NullToInt(dr["TermOfPayment"],0);
                            invprint.Invoice.PaidAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["PaidAmount"], 0);
                            invprint.Invoice.OutstandingAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["OutstandingAmount"], 0);
                            invprint.Invoice.StatusInv = IDS.Tool.GeneralHelper.NullToInt(dr["StatusINV"], 0);
                            invprint.Invoice.IsPPh = IDS.Tool.GeneralHelper.NullToString(dr["isPPh"]);
                            invprint.Invoice.PPhPercentage = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPhPercentage"], 0);
                            invprint.Invoice.PPhNo = IDS.Tool.GeneralHelper.NullToString(dr["PPhNo"]);
                            invprint.Invoice.PPhAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPhAmount"], 0);
                            invprint.Invoice.PPhDateReceive = IDS.Tool.GeneralHelper.NullToDateTime(dr["PPhDateReceive"], DateTime.Now);
                            invprint.Invoice.IsPPn = IDS.Tool.GeneralHelper.NullToBool(dr["isPPn"]);
                            invprint.Invoice.PPnAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPnAmount"], 0);
                            invprint.Invoice.PPnNo = IDS.Tool.GeneralHelper.NullToString(dr["PPnNo"]);
                            invprint.Invoice.Cust.GovPrivate = IDS.Tool.GeneralHelper.NullToBool(dr["GovPrivate"]);
                            list.Add(invprint);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<INVPRINT> GetINVPRINT(string custCode,string invno)
        {
            List<INVPRINT> list = new List<INVPRINT>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"SELECT InvoiceNumber,custcode, serialNo, baseAmount, ISNULL(GOVPRIVATE, 0) AS GOVPRIVATE FROM INVPRINT WHERE CustCode = @custCode and InvoiceNumber = @invno";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@custCode", System.Data.SqlDbType.VarChar, custCode);
                db.AddParameter("@invno", System.Data.SqlDbType.VarChar, invno);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            INVPRINT invprint = new INVPRINT();
                            invprint = new INVPRINT();
                            invprint.Invoice = new Invoice();
                            invprint.Invoice.InvoiceNumber = IDS.Tool.GeneralHelper.NullToString(dr["InvoiceNumber"]);
                            invprint.PaymentH = new PaymentH();
                            invprint.PaymentH.SerialNo = IDS.Tool.GeneralHelper.NullToString(dr["serialNo"]);
                            invprint.PaymentH.BaseAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["baseAmount"], 0);
                            invprint.Customer = new GeneralTable.Customer();
                            invprint.Customer.CUSTCode = IDS.Tool.GeneralHelper.NullToString(dr["custcode"]);
                            invprint.Customer.GovPrivate = IDS.Tool.GeneralHelper.NullToBool(dr["GOVPRIVATE"]);
                            list.Add(invprint);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetINVPRINTForDatasource(string cust)
        {
            List<System.Web.Mvc.SelectListItem> groups = new List<System.Web.Mvc.SelectListItem>();
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = @"SELECT InvoiceNumber  +'='+ cast(isnull(serialNo,'') as varchar) +'='+ cast(baseAmount AS varchar)+'='+ cast(ISNULL(GOVPRIVATE, 0) as varchar) as splitforgetdata
,  InvoiceNumber 
FROM INVPRINT WHERE CustCode ='" + cust + "'";
                //db.CommandText = @"SELECT InvoiceNumber +'='+  serialNo +'='+ cast(baseAmount AS varchar)+'='+ cast(ISNULL(GOVPRIVATE, 0) as varchar) as splitforgetdata,  InvoiceNumber FROM INVPRINT WHERE CustCode ='" + cust + "'";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem item = new System.Web.Mvc.SelectListItem();
                            item.Value = Tool.GeneralHelper.NullToString(dr["splitforgetdata"]);
                            //item.Value = Tool.GeneralHelper.NullToString(dr["serialNo"]);
                            item.Text = Tool.GeneralHelper.NullToString(dr["InvoiceNumber"]);
                            groups.Add(item);
                        }
                    }
                    if (!dr.IsClosed) dr.Close();
                }
            }
            return groups;
        }//GetINVPRINTForDatasource


    }
}
