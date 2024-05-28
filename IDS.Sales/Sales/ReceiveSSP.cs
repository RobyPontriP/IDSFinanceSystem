using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
   public class ReceiveSSP
    {
        public IDS.GeneralTable.Branch Branch { get; set; }
        public IDS.Sales.Invoice Invoice { get; set; }
        public IDS.GeneralTable.Customer Customer { get; set; }
        public string Remark { get; set; }
        public DateTime PrintDate { get; set; }
        public string Signature { get; set; }
        public int PrintCounter { get; set; }
        public bool ReceiveStatus { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ReceiveOperator { get; set; }
        public DateTime ReceiveEntryDate { get; set; }

        [Display(Name = "Entry User")]
        [MaxLength(20), StringLength(20)]
        public string EntryUser { get; set; }

        [Display(Name = "Entry Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        [MaxLength(20), StringLength(20)]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public ReceiveSSP()
        {

        }

        public static List<ReceiveSSP> GetReceiveSSP(string cust, string period)
        {
            List<ReceiveSSP> List = new List<ReceiveSSP>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSalesSSP";
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ReceiveSSP SSP = new ReceiveSSP();

                            SSP.Branch = new IDS.GeneralTable.Branch();
                            SSP.Branch.BranchCode =Tool.GeneralHelper.NullToString(dr["branch"]);

                            SSP.Customer = new GeneralTable.Customer();
                            SSP.Customer.CUSTCode = Tool.GeneralHelper.NullToString(dr["CustCode"]);

                            SSP.PrintDate = Convert.ToDateTime(dr["PrintDate"]);

                            if (!string.IsNullOrEmpty(Tool.GeneralHelper.NullToString(dr["ReceiveDate"])))
                            {
                                SSP.ReceiveDate = Convert.ToDateTime(dr["ReceiveDate"]);
                            }

                            if (!string.IsNullOrEmpty(Tool.GeneralHelper.NullToString(dr["ReceiveEntryDate"])))
                            {
                                SSP.ReceiveEntryDate = Convert.ToDateTime(dr["ReceiveEntryDate"]);
                            }

                            SSP.ReceiveStatus = Tool.GeneralHelper.NullToBool(dr["ReceiveStatus"]);
                            SSP.ReceiveOperator = Tool.GeneralHelper.NullToString(dr["ReceiveOperator"]);

                            SSP.Invoice = new IDS.Sales.Invoice();
                            SSP.Invoice.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["InvoiceNumber"]);
                            SSP.Invoice.CCy = new GeneralTable.Currency();
                            SSP.Invoice.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCy"]);
                            SSP.Invoice.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]);
                            SSP.Invoice.InvoiceAmount = Tool.GeneralHelper.NullToDecimal(dr["InvoiceAmount"],0);
                            SSP.Invoice.PPnAmount = Tool.GeneralHelper.NullToDecimal(dr["PPnAmount"], 0);

                            List.Add(SSP);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return List;
        }

        public int SaveSSP(string rcvDate, string[] invNo, string[] branch, string[] custCode)
        {
            int result = 0;

            if (invNo == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SalesSSP";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < invNo.Length; i++)
                    {
                        cmd.CommandText = "SalesSSP";
                        cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch[i]);
                        cmd.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo[i]);
                        cmd.AddParameter("@CustCode", System.Data.SqlDbType.VarChar, custCode[i]);
                        cmd.AddParameter("@RcvDate", System.Data.SqlDbType.DateTime, Convert.ToDateTime(rcvDate));
                        cmd.AddParameter("@RcvOperator", System.Data.SqlDbType.VarChar, ReceiveOperator);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
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
                        case 2627:
                            throw new Exception("Receive SSP is already exists. Please choose other Receive SSP.");
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

        public static int SaveSSP(int ExecCode, string branch,string invno,string custCode,string operatorID)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    //@branch, @InvNo, @CustCode, @remark, GETDATE(), @signature, 1, 0, NULL, NULL, NULL, 
                    //@OperatorID, GETDATE(),@OperatorID, GETDATE())
                    cmd.CommandText = "SalesSSP";
                    cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
                    cmd.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invno);
                    cmd.AddParameter("@CustCode", System.Data.SqlDbType.VarChar, custCode);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, operatorID);
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
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
                            throw new Exception("SSP id is already exists. Please choose other SSP id.");
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

        public static int GetBUMNLimit(string mapPath)
        {
            int result = 0;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(mapPath))
            {
                string line;
                while (!string.IsNullOrEmpty((line = reader.ReadToEnd())))
                {
                    result = IDS.Tool.GeneralHelper.NullToInt(line, 0);
                }
            }
            //IEnumerable<string> lines = System.IO.File.ReadLines(mapPath);

            return result;
        }
    }
}
