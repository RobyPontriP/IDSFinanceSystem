using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GeneralTable
{
    public class PPhTrans
    {
        public string SerialNo { get; set; }

        public string InvoiceNo { get; set; }

        public int TaxObjectID { get; set; }

        public string Komoditi { get; set; }

        public IDS.GeneralTable.Customer Customer { get; set; }

        public string NoBuktiPotong { get; set; }

        public string TaxType { get; set; }

        public string TaxObjectType { get; set; }

        public string Description { get; set; }

        public decimal TaxRate { get; set; }

        public decimal Amount { get; set; }

        public bool isProcess { get; set; }

        public decimal DasarPemotongan { get; set; }

        public decimal DasarPemotonganKumulatif { get; set; }

        public decimal Tarif { get; set; }

        public decimal TarifNonNPWP { get; set; }

        public decimal PPhTerutang { get; set; }

        public DateTime TanggalSetor { get; set; }

        public DateTime TanggalLapor { get; set; }

        public PPhTrans()
        {

        }

        public static int DeletePPhTrans(string serialNo, string invNo, string taxType)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "UpdPPh";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 5);
                    cmd.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, serialNo);
                    cmd.AddParameter("@InvoiceNo", System.Data.SqlDbType.VarChar, invNo);
                    cmd.AddParameter("@TaxType", System.Data.SqlDbType.VarChar, taxType);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();
                }
                catch (SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("PPh Trans is already exists. Please choose other PPh Trans.");
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

    }
}
