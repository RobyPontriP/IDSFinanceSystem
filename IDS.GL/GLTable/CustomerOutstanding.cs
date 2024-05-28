using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class CustomerOutstanding
    {
        public string CustCode { get; set; }
        public string Period { get; set; }
        public IDS.GeneralTable.Currency Ccy { get; set; }
        public decimal BegBal { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public bool Closing { get; set; }
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

        public CustomerOutstanding()
        {

        }

        public int AdjustCreditOutstanding(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "AdjustCustOutstanding";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@custCode", System.Data.SqlDbType.VarChar, CustCode);
                    cmd.AddParameter("@period", System.Data.SqlDbType.VarChar, Period);
                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Ccy.CurrencyCode);
                    cmd.AddParameter("@Debit", System.Data.SqlDbType.VarChar, Debit);
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
                            throw new Exception("Customer Project code is already exists. Please choose other Customer Project code.");
                        default:
                            throw;
                    }
                }

                finally
                {
                    cmd.Close();
                }
            }

            return result;
        }

        public int AdjustCreditOutstanding(int ExecCode, IDS.DataAccess.SqlServer cmd)
        {
            int result = 0;

            try
            {
                cmd.CommandText = "AdjustCustOutstanding";
                cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                cmd.AddParameter("@custCode", System.Data.SqlDbType.VarChar, CustCode);
                cmd.AddParameter("@period", System.Data.SqlDbType.VarChar, Period);
                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Ccy.CurrencyCode);
                cmd.AddParameter("@Debit", System.Data.SqlDbType.VarChar, Debit);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Open();

                //cmd.BeginTransaction();
                result = cmd.ExecuteNonQuery();
                //cmd.CommitTransaction();
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();

                switch (sex.Number)
                {
                    case 2627:
                        throw new Exception("Error");
                    default:
                        throw;
                }
            }            
            return result;
        }
    }
}
