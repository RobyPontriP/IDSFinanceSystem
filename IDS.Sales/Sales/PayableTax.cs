using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class PayableTax
    {
        [Display(Name = "Invoice Number")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Invoice Number")]
        [MaxLength(20)]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Serial No")]
        [MaxLength(50)]
        public string SerialNo { get; set; }

        [Display(Name = "Customer")]
        public IDS.GeneralTable.Customer Customer { get; set; }

        [Display(Name = "Tax Object ID")]
        public IDS.Sales.JenisPenghasilan TaxObjectID { get; set; }

        [Display(Name = "Komoditi ID")]
        public string KomoditiID { get; set; }

        [Display(Name = "No Bukti Potong")]
        public string NoBuktiPotong { get; set; }

        [Display(Name = "Tax Type")]
        [MaxLength(10)]
        public string TaxType { get; set; }

        [Display(Name = "Tax Object Type")]
        [MaxLength(10)]
        public string TaxObjectType { get; set; }

        [Display(Name = "Description")]
        [MaxLength(100)]
        public string Description { get; set; }

        [Display(Name = "Tax Rate")]
        public decimal TaxRate { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Is Process")]
        public bool IsProcess { get; set; }

        [Display(Name = "Dasar Pemotongan")]
        public decimal DasarPemotongan { get; set; }

        [Display(Name = "Dasar Pemotongan Kumulatif")]
        public decimal DasarPemotonganKumulatif { get; set; }

        [Display(Name = "Tarif")]
        public decimal Tarif { get; set; }

        [Display(Name = "Tarif Non NPWP")]
        public decimal TarifNonNPWP { get; set; }

        [Display(Name = "PPh Terutang")]
        public decimal PPhTerutang { get; set; }

        [Display(Name = "Tanggal Setor")]
        public DateTime TanggalSetor { get; set; }

        [Display(Name = "Tanggal Lapor")]
        public DateTime TanggalLapor { get; set; }

        public PayableTax()
        {

        }

        public static List<PayableTax> GetPayableTax(string custCode,string period, string taxType)
        {
            List<PayableTax> list = new List<PayableTax>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelPayableTax";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@CustCode", System.Data.SqlDbType.VarChar, custCode);
                db.AddParameter("@TaxType", System.Data.SqlDbType.VarChar, taxType);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            PayableTax payableTax = new PayableTax();
                            payableTax.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["InvoiceNo"]);
                            payableTax.SerialNo = Tool.GeneralHelper.NullToString(dr["SerialNo"]);

                            payableTax.Customer = new GeneralTable.Customer();
                            payableTax.Customer.CUSTCode = Tool.GeneralHelper.NullToString(dr["CustSuppCode"]);

                            payableTax.NoBuktiPotong = Tool.GeneralHelper.NullToString(dr["NoBuktiPotong"]);
                            payableTax.TaxType = Tool.GeneralHelper.NullToString(dr["TaxType"]);

                            payableTax.TaxObjectID = new Sales.JenisPenghasilan();
                            payableTax.TaxObjectID.JPID= Tool.GeneralHelper.NullToInt(dr["TaxObjectID"],0);
                            payableTax.TaxObjectID.Description = Tool.GeneralHelper.NullToString(dr["descripjp"]);

                            payableTax.KomoditiID = Tool.GeneralHelper.NullToString(dr["KomoditiID"]);
                            payableTax.Description = Tool.GeneralHelper.NullToString(dr["Description"]);
                            payableTax.TaxRate = Tool.GeneralHelper.NullToDecimal(dr["TaxRate"],0);
                            payableTax.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            payableTax.IsProcess = Tool.GeneralHelper.NullToBool(dr["isProcess"]);
                            payableTax.DasarPemotongan = Tool.GeneralHelper.NullToDecimal(dr["DasarPemotongan"], 0);
                            payableTax.DasarPemotonganKumulatif = Tool.GeneralHelper.NullToDecimal(dr["DasarPemotonganKumulatif"], 0);
                            payableTax.Tarif = Tool.GeneralHelper.NullToDecimal(dr["Tarif"], 0);
                            payableTax.TarifNonNPWP = Tool.GeneralHelper.NullToDecimal(dr["TarifNonNPWP"], 0);
                            payableTax.PPhTerutang = Tool.GeneralHelper.NullToDecimal(dr["PPhTerutang"], 0);
                            payableTax.TanggalSetor = Convert.ToDateTime(dr["TanggalSetor"]);
                            payableTax.TanggalLapor = Convert.ToDateTime(dr["TanggalLapor"]);

                            list.Add(payableTax);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static PayableTax GetPayableTax(string invNo, string serialNo)
        {
            PayableTax payableTax = new PayableTax();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelPayableTax";
                db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo);
                db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, serialNo);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        payableTax.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["InvoiceNo"]);
                        payableTax.SerialNo = Tool.GeneralHelper.NullToString(dr["SerialNo"]);

                        payableTax.Customer = new GeneralTable.Customer();
                        payableTax.Customer.CUSTCode = Tool.GeneralHelper.NullToString(dr["CustSuppCode"]);

                        payableTax.NoBuktiPotong = Tool.GeneralHelper.NullToString(dr["NoBuktiPotong"]);
                        payableTax.TaxType = Tool.GeneralHelper.NullToString(dr["TaxType"]);

                        payableTax.TaxObjectID = new Sales.JenisPenghasilan();
                        payableTax.TaxObjectID.JPID = Tool.GeneralHelper.NullToInt(dr["TaxObjectID"], 0);
                        payableTax.TaxObjectID.Description = Tool.GeneralHelper.NullToString(dr["descripjp"]);

                        payableTax.KomoditiID = Tool.GeneralHelper.NullToString(dr["KomoditiID"]);
                        payableTax.Description = Tool.GeneralHelper.NullToString(dr["Description"]);
                        payableTax.TaxRate = Tool.GeneralHelper.NullToDecimal(dr["TaxRate"], 0);
                        payableTax.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                        payableTax.IsProcess = Tool.GeneralHelper.NullToBool(dr["isProcess"]);
                        payableTax.DasarPemotongan = Tool.GeneralHelper.NullToDecimal(dr["DasarPemotongan"], 0);
                        payableTax.DasarPemotonganKumulatif = Tool.GeneralHelper.NullToDecimal(dr["DasarPemotonganKumulatif"], 0);
                        payableTax.Tarif = Tool.GeneralHelper.NullToDecimal(dr["Tarif"], 0);
                        payableTax.TarifNonNPWP = Tool.GeneralHelper.NullToDecimal(dr["TarifNonNPWP"], 0);
                        payableTax.PPhTerutang = Tool.GeneralHelper.NullToDecimal(dr["PPhTerutang"], 0);
                        payableTax.TanggalSetor = Convert.ToDateTime(dr["TanggalSetor"]);
                        payableTax.TanggalLapor = Convert.ToDateTime(dr["TanggalLapor"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return payableTax;
        }

        public int InsUpDelPayableTax(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SalesPayableTax";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@InvoiceNo", System.Data.SqlDbType.VarChar, InvoiceNumber);
                    cmd.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, SerialNo);
                    cmd.AddParameter("@NoBuktiPotong", System.Data.SqlDbType.VarChar, NoBuktiPotong);
                    cmd.AddParameter("@TaxObjID", System.Data.SqlDbType.Int, TaxObjectID.JPID);
                    cmd.AddParameter("@Description", System.Data.SqlDbType.VarChar, Description);
                    cmd.AddParameter("@Amt", System.Data.SqlDbType.Money, Amount);
                    cmd.AddParameter("@TanggalSetor", System.Data.SqlDbType.Date, TanggalSetor);
                    cmd.AddParameter("@TanggalLapor", System.Data.SqlDbType.Date, TanggalLapor);
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
                            throw new Exception("Payable Tax is already exists. Please choose other Payable Tax.");
                        default:
                            throw;
                    }
                }
                catch (Exception ex)
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

        public int InsUpDelPayableTax(Tool.PageActivity ExecCode, string[] dataInv, string[] dataSerNo)
        {
            int result = 0;

            if (dataInv == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SalesPayableTax";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < dataInv.Length; i++)
                    {
                        cmd.CommandText = "SalesPayableTax";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, (int)ExecCode);
                        cmd.AddParameter("@InvoiceNo", System.Data.SqlDbType.VarChar, dataInv[i]);
                        cmd.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, dataSerNo[i]);
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
                            throw new Exception("Invoice No is already exists. Please choose other Invoice No.");
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
    }
}
