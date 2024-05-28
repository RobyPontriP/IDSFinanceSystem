using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class InvoiceDetail
    {
        [Display(Name = "Invoice Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Invoice Number")]
        [MaxLength(20)]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Counter")]
        public int Counter { get; set; }

        [Display(Name = "SubCounter")]
        public int SubCounter { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }

        [Display(Name = "SubAmount")]
        [MaxLength(20)]
        public string SubAmount { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Tax Invoice")]
        public bool TaxInvoice { get; set; }

        public decimal ExchangeRate { get; set; }

        public InvoiceDetail()
        {

        }

        public static List<InvoiceDetail> GetInvoiceDetail(string invNo)
        {
            List<IDS.Sales.InvoiceDetail> list = new List<InvoiceDetail>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SP_GetDetailSalesInvoice";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@InvoiceNumber", System.Data.SqlDbType.VarChar, invNo);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            InvoiceDetail invoiceDetail = new InvoiceDetail();
                            invoiceDetail.InvoiceNumber = dr["InvoiceNumber"] as string;
                            invoiceDetail.Counter = Tool.GeneralHelper.NullToInt(dr["Counter"], 0);
                            invoiceDetail.SubCounter = Tool.GeneralHelper.NullToInt(dr["SubCounter"], 0);
                            invoiceDetail.SubAmount = Tool.GeneralHelper.NullToString(dr["SubAmount"], "");
                            invoiceDetail.Remark = Tool.GeneralHelper.NullToString(dr["Remark"], "");
                            invoiceDetail.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            invoiceDetail.TaxInvoice = Tool.GeneralHelper.NullToBool(dr["TaxInvoice"]);

                            list.Add(invoiceDetail);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }
    }
}
