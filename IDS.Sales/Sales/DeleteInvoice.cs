using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class DeleteInvoice
    {
        public string InvoiceNo { get; set; }
        public DeleteInvoice()
        {
        }

        public DeleteInvoice(string InvoiceNo)
        {
            this.InvoiceNo = InvoiceNo;
        }

        public static string ProcessDeleteInvoice()
        {
            return "asdasd";
        }

        public static string Confirm_Delete_Invoice(string inv_, string cust_, string periode_, string branct_)
        {
            string return_string = "";
            if (string.IsNullOrEmpty(periode_))
                periode_ = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(periode_);
                periode_ = datePeriod.ToString("yyyyMM");
            }

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "SlsPaymentValidasi";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 2);
                    db.AddParameter("@Period", System.Data.SqlDbType.VarChar, periode_);
                    db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branct_);
                    db.AddParameter("@invNo", System.Data.SqlDbType.VarChar, inv_);
                    db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust_);
                    db.Open();
                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            //IDS.Sales.Invoice InvToDelete = new IDS.Sales.Invoice()
                            //{
                            //    CCy = new GeneralTable.Currency() { CurrencyCode = dr["ccy"].ToString() },
                            //    InvoiceNumber = dr["InvoiceNumber"].ToString(),
                            //    InvoiceRole = System.Convert.ToInt16(dr["InvoiceRole"]),
                            //    PPnNo = dr["PPnNo"].ToString(),
                            //    InvoiceDate = System.Convert.ToDateTime(dr["InvoiceDate"].ToString()),
                            //    InvoiceAmount = System.Convert.ToDecimal(dr["InvoiceAmount"].ToString()),
                            //    PPnAmount = System.Convert.ToDecimal(dr["PPnAmount"].ToString()),
                            //    PPhAmount = System.Convert.ToDecimal(dr["PPhAmount"].ToString()),
                            //    EquivAmount = System.Convert.ToDecimal(dr["EquipAmt"].ToString()),
                            //    RP = dr["RP"].ToString(),
                            //    Voucher = dr["VOUCHERNO"].ToString()
                            //};
                            IDS.Sales.Invoice Inv = new IDS.Sales.Invoice();

                            Inv.CCy = new IDS.GeneralTable.Currency();
                            Inv.CCy.CurrencyCode = dr["ccy"].ToString();

                            Inv.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["InvoiceNumber"],"");
                            Inv.InvoiceRole = Tool.GeneralHelper.NullToInt(dr["InvoiceRole"],-1);
                            Inv.PPnNo = Tool.GeneralHelper.NullToString(dr["PPnNo"], "");
                            Inv.InvoiceDate = dr["InvoiceDate"] == null ? DateTime.Now : Convert.ToDateTime(dr["InvoiceDate"]);
                            Inv.InvoiceAmount = Tool.GeneralHelper.NullToDecimal(dr["InvoiceAmount"], 0);
                            Inv.PPnAmount = Tool.GeneralHelper.NullToDecimal(dr["PPnAmount"], 0);
                            Inv.PPhAmount = Tool.GeneralHelper.NullToDecimal(dr["PPhAmount"], 0);
                            Inv.EquivAmount = Tool.GeneralHelper.NullToDecimal(dr["EquivAmt"], 0);
                            Inv.RP = Tool.GeneralHelper.NullToString(dr["RP"], "");
                            Inv.Voucher = Tool.GeneralHelper.NullToString(dr["VOUCHERNO"], "");
                            return_string = Return_Finish_HTML_ToDelete(Inv);
                        }
                        if (!dr.IsClosed)
                            dr.Close();
                    }
                    db.Close();
                }
                catch (Exception ex)
                {
                    return_string = WebMsgApp_WebMsgBox_Show(ex.Message);
                }
            }
            return return_string;
        }

        public static string Finish_Delete_Invoice_With_SP(string cust, string inv, string sCode, string voucher, string branch, string ccy, string invoiceDate, string pphamount, string invoiceamount)
        {
            string return_string = "";
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "slsDeleteInvoice";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@inv", System.Data.SqlDbType.VarChar, inv);
                db.AddParameter("@sCode", System.Data.SqlDbType.VarChar, sCode);
                db.AddParameter("@voucher", System.Data.SqlDbType.VarChar, voucher);
                db.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@invoiceTotal", System.Data.SqlDbType.Money, Convert.ToDouble(pphamount) + Convert.ToDouble(invoiceamount));
                db.AddParameter("@invoiceDate", System.Data.SqlDbType.DateTime, Convert.ToDateTime(invoiceDate));
                db.Open();
                db.BeginTransaction();
                db.ExecuteNonQuery();
                db.CommitTransaction();
                db.Close();
                return_string = WebMsgApp_WebMsgBox_Show("Invoice has been deleted.");
            }
            return return_string;
        }
        public static bool Get_dtPayment(string tInvoice)
        {
            bool _return = false;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SlsPaymentH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 2);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, null);
                db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, null);
                db.AddParameter("@invNo", System.Data.SqlDbType.VarChar, tInvoice);
                db.Open();
                db.ExecuteReader();

                //@TYPE tinyint,
                //@Period varchar(6) = null,
                //@branchCode varchar(5) = null,
                //@invNo varchar(20) = null

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        if (dr["serialNo"] != DBNull.Value)
                        {
                            _return = true;
                        }
                        else
                        {
                            _return = false;
                        }
                    }
                    if (!dr.IsClosed) dr.Close();
                }
                db.Close();
            }
            return _return;
        }
        //SlsPaymentH
        public static bool Get_slsSSPPrint(string inv_, string cust_)
        {
            //@TYPE tinyint,
            //@inv varchar(20) = null,
            //@cust varchar(20) = null
            bool _return = false;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "slsSelSSPPrint";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 4);
                db.AddParameter("@inv", System.Data.SqlDbType.VarChar, inv_);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust_);
                db.Open();
                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {

                    if (dr.HasRows)
                    {
                        dr.Read();
                        if (dr["invoiceNumber"] != DBNull.Value)
                        {
                            _return = true;
                        }
                        else
                        {
                            _return = false;
                        }
                    }
                    if (!dr.IsClosed) dr.Close();
                }
                db.Close();
            }
            return _return;
        }
        //slsSSPPrint
        public static bool Get_Inv_From_ACFARAP_And_SLSInvH(string inv_, string cust_, string periode_)
        {
            //@period as varchar(12) = null,
            //@cust as varchar(20) = null,
            //@type as INT,
            //@inv as varchar(20) = null
            bool _return = false;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesInvoiceList";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.AddParameter("@inv", System.Data.SqlDbType.VarChar, inv_);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust_);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, periode_);
                db.Open();
                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        //dr["SUBACC"].ToString()
                        if (dr["status"].ToString().Contains("1"))
                        {
                            _return = true;
                        }
                        else
                        {
                            _return = false;
                        }
                    }
                    if (!dr.IsClosed) dr.Close();
                }
                db.Close();
            }
            return _return;
        }
        //SelSalesInvoiceList

        private static string WebMsgApp_WebMsgBox_Show(string comment)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='ConfirmForm' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>");
            b.AppendLine(" <div class='modal-dialog modal-dialog-centered'>");
            b.AppendLine("     <div class='modal-content'>");
            b.AppendLine("       <div class='modal-header'><h5 class='modal-title' id='exampleModalLongTitle'> Confirm </h5>");
            b.AppendLine("           <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='goClose()'>");
            b.AppendLine("               <span aria-hidden='true'>&times;</span>");
            b.AppendLine("           </button>");
            b.AppendLine("      </div>");
            b.AppendLine("      <div class='modal-body'>");
            b.AppendLine("         <h6>" + comment + "</h6>");
            b.AppendLine("     </div>");
            b.AppendLine("      <div class='modal-footer'>");
            b.AppendLine("          <div class='container my-3 bg-light'>");
            b.AppendLine("              <div class='col-md-12 text-center'>");
            b.AppendLine("                  <button type='button' class='btn btn-primary btn-block' data-dismiss='modal' onclick='goClose()'>Close</button>");
            b.AppendLine("               </div>");
            b.AppendLine("           </div>");
            b.AppendLine("       </div>");
            b.AppendLine("     </div>");
            b.AppendLine(" </div>");
            b.AppendLine("</div>");
            return b.ToString();
        }

        private static string Return_Finish_HTML_ToDelete(IDS.Sales.Invoice idl)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='ConfirmForm' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>");
            b.AppendLine("    <div class='modal-dialog modal-dialog-centered'>");
            b.AppendLine("       <div class='modal-content'>");
            b.AppendLine("           <div class='modal-header'><h5 class='modal-title' id='exampleModalLongTitle'>You are about to delete");
            b.AppendLine("               data:</h5>");
            b.AppendLine("              <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='goClose()'>");
            b.AppendLine("                   <span aria-hidden='true'>&times;</span>");
            b.AppendLine("               </button>");
            b.AppendLine("          </div>");
            b.AppendLine("          <div class='modal-body'>");
            b.AppendLine("              <table id='tbldelete'>");
            b.AppendLine("                  <tr>");
            b.AppendLine("                      <th></th>");
            b.AppendLine("                      <th></th>");
            b.AppendLine("                 </tr>");
            b.AppendLine("                 <tr>");
            b.AppendLine("                    <td><b>Invoice Number</b></td>");
            b.AppendLine("                     <td>:" + idl.InvoiceNumber + "</td>");
            b.AppendLine("               </tr>");
            b.AppendLine("                 <tr>");
            b.AppendLine("                     <td><b>CCY</b></td>");
            b.AppendLine("                    <td>:" + idl.CCy.CurrencyCode + "</td>");
            b.AppendLine("                 </tr>");
            b.AppendLine("                <tr>");
            b.AppendLine("                   <td><b>Invoice Role</b></td>");
            b.AppendLine("                   <td>:" + idl.InvoiceRole + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                 <tr>");
            b.AppendLine("                     <td><b>PpnNo</b></td>");
            b.AppendLine("                     <td>:" + idl.PPnNo + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                <tr>");
            b.AppendLine("                    <td><b>Invoice Date</b></td>");
            b.AppendLine("                   <td>:" + idl.InvoiceDate + "</td>");
            b.AppendLine("                 </tr>");
            b.AppendLine("               <tr>");
            b.AppendLine("                    <td><b>Invoice Amount</b></td>");
            b.AppendLine("                    <td>:" + idl.InvoiceAmount + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                <tr>");
            b.AppendLine("                    <td><b>Ppn Amount</b></td>");
            b.AppendLine("                    <td>:" + idl.PPnAmount + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                 <tr>");
            b.AppendLine("                    <td><b>Pph Amount</b></td>");
            b.AppendLine("                   <td>:" + idl.PPhAmount + "</td>");
            b.AppendLine("                </tr>");

            b.AppendLine("                <tr>");
            b.AppendLine("                    <td><b>EquipAmt</b></td>");
            b.AppendLine("                    <td>:" + idl.EquivAmount + "</td>");
            b.AppendLine("                 </tr>");
            b.AppendLine("               <tr>");
            b.AppendLine("                    <td><b>RP</b></td>");
            b.AppendLine("                    <td>:" + idl.RP + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                <tr>");
            b.AppendLine("                    <td><b>VoucherNo</b></td>");
            b.AppendLine("                    <td>:" + idl.Voucher + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                <tr>");
            b.AppendLine("                    <td></td>");
            b.AppendLine("                   <td><b>Are you sure want to delete the data?</b></td>");
            b.AppendLine("                </tr>");
            b.AppendLine("             </table>");
            b.AppendLine("        </div>");
            b.AppendLine("        <div class='modal-footer'>");
            b.AppendLine("           <div class='container my-3 bg-light'>");
            b.AppendLine("                <div class='col-md-12 text-center'>");
            b.AppendLine("                    <button type='button' class='btn btn-primary' data-dismiss='modal' onclick='goClose()'>No</button>");
            b.AppendLine("                    <button type='button' class='btn btn-danger' onclick='getDataForms()'>Yes</button>");
            b.AppendLine("                </div>");
            b.AppendLine("            </div>");
            b.AppendLine("        </div>");
            b.AppendLine("    </div>");
            b.AppendLine("</div>");
            b.AppendLine("</div>");
            return b.ToString();
        }
    }
}
