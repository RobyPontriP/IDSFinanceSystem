using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class ProcessInvoice
    {
        public IDS.Sales.Invoice Invoice { get; set; }
        public GeneralTable.Customer Customer { get; set; }
        public string OperatorID { get; set; }

        public ProcessInvoice()
        {

        }

        public string InvoiceProcess(string period, string invNo, DateTime invDate)
        {
            string strResult = "";
            string PeriodProcess = "";
            if (string.IsNullOrEmpty(period))
            {
                period = DateTime.Now.ToString("yyyyMM");
                PeriodProcess = DateTime.Now.ToString("yyyyMM");
            }
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
                PeriodProcess = datePeriod.ToString("yyyyMM");
            }

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                if (string.IsNullOrEmpty(invNo))
                {
                    db.CommandText = "SelSalesProjInvoice";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 5);
                    db.AddParameter("@period", System.Data.SqlDbType.VarChar, period.Substring(0, 4));
                    db.Open();

                    invNo = (Tool.GeneralHelper.NullToInt(Convert.ToInt32(db.ExecuteScalar()), 0) + 1).ToString();
                }

                db.CommandText = "SalesSelCustProject";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 4);
                db.AddParameter("@NextPeriod", System.Data.SqlDbType.VarChar, period);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["CUSTACC"])) || string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["VATACC"])) || string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["SALESACC"])))
                            {
                                strResult = "Some customer account has not been set. Please set customer account. Process will be terminate.";
                                return strResult;
                            }
                        }
                        
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }

                object WillBeProcess = 0;

                db.CommandText = "SalesSelCustProject";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 5);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            WillBeProcess = Tool.GeneralHelper.NullToInt(dr["ProjectCode"], 0);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                string month = GetPeriod(period.Substring(4, 2)); //huruf romawi
                string year = period.Substring(0, 4);
                period = "/" + month + "/" + year;
                //SelProjInvoiceList
                db.CommandText = "SelSalesProjInvoice";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 6);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, year);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        string DuplicateInvNo = string.Empty;
                        while (dr.Read())
                        {
                            if (invNo == dr["InvNo"] as string)
                            {
                                DuplicateInvNo = DuplicateInvNo + invNo + period + ", ";
                            }
                        }

                        if (!string.IsNullOrEmpty(DuplicateInvNo))
                        {
                            strResult = "Duplicate invoice number: " + DuplicateInvNo + ". Process will be terminate.";
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                try
                {
                    db.CommandText = "ProcessInvoiceProject";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@Period", System.Data.SqlDbType.VarChar, PeriodProcess);
                    db.AddParameter("@Operator", System.Data.SqlDbType.VarChar, OperatorID);
                    db.AddParameter("@EditInvoiceDate", System.Data.SqlDbType.DateTime, invDate);
                    db.AddParameter("@EditInvoiceNo", System.Data.SqlDbType.Int, Tool.GeneralHelper.NullToInt(invNo,0));
                    db.Open();

                    db.BeginTransaction();
                    db.ExecuteNonQuery();

                    if (string.IsNullOrEmpty(strResult))
                    {
                        strResult = "Process Done";
                    }
                    db.CommitTransaction();
                }
                catch (Exception e)
                {
                    db.RollbackTransaction();
                    strResult = e.Message;
                }
                finally
                {
                    db.Close();
                }
            }
            return strResult;
        }

        public string GetNextInvoiceNumber(int offset, string period)
        {
            if (string.IsNullOrEmpty(period))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }

            string nextPeriod = period;

            object WillBeProcess;
            object CurrentNumber;
            string NextNumber = "";
            string month = GetPeriod(period.Substring(4, 2));
            string year = period.Substring(0, 4);
            period = "/" + month + "/" + year;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelCustProject";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 5);
                db.AddParameter("@NextPeriod", System.Data.SqlDbType.VarChar, nextPeriod);
                db.Open();

                db.ExecuteReader();

                int countProjectCode = 0;

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            countProjectCode = Tool.GeneralHelper.NullToInt(dr["ProjectCode"],0);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                WillBeProcess = countProjectCode;
                // SelProjInvoiceList
                db.CommandText = "SelSalesProjInvoice";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 5);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, year);
                db.Open();

                CurrentNumber = Tool.GeneralHelper.NullToInt(Convert.ToInt32(db.ExecuteScalar()), 0);

                NextNumber = (Convert.ToInt32(CurrentNumber) + 1).ToString("00000") + "/" + month + "/" + year;
                string lastNumber = (Convert.ToInt32(CurrentNumber) + (Convert.ToInt32(WillBeProcess) == 0 ? 1 : Convert.ToInt32(WillBeProcess))).ToString("00000") + "/" + month + "/" + year;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("<strong>Invoice will be process : </strong> " + WillBeProcess.ToString() + "<br /><br />")
                    .Append("<strong>Next invoice number : </strong><br />" + NextNumber + " <strong>to</strong> " + lastNumber);

                db.Close();

                return sb.ToString();
            }
        }

        private string GetPeriod(string month)
        {
            string bln = "";

            try
            {
                if (month.Length > 0)
                {
                    switch (month)
                    {
                        case "01":
                            bln = "I";
                            break;
                        case "02":
                            bln = "II";
                            break;
                        case "03":
                            bln = "III";
                            break;
                        case "04":
                            bln = "IV";
                            break;
                        case "05":
                            bln = "V";
                            break;
                        case "06":
                            bln = "VI";
                            break;
                        case "07":
                            bln = "VII";
                            break;
                        case "08":
                            bln = "VIII";
                            break;
                        case "09":
                            bln = "IX";
                            break;
                        case "10":
                            bln = "X";
                            break;
                        case "11":
                            bln = "XI";
                            break;
                        case "12":
                            bln = "XII";
                            break;
                        default:
                            bln = "";
                            break;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {

            }

            return bln;
        }
    }
}
