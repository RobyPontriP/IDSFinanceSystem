using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class PPhDetail
    {
        public IDS.GeneralTable.Supplier Supplier { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int SeqNo { get; set; }
        public string Description { get; set; }
        public decimal TaxRate { get; set; }
        public decimal Amount { get; set; }
        public bool IsProcess { get; set; }
        public decimal DasarPemotongan { get; set; }
        public decimal DasarPemotonganKumulatif { get; set; }
        public decimal Tarif { get; set; }
        public decimal TarifNonNPWP { get; set; }
        public decimal PPhTerutang { get; set; }
        public DateTime TanggalSetor { get; set; }
        public DateTime TanggalLapor { get; set; }
        public string NoBuktiPotong { get; set; }

        public PPhDetail()
        {

        }

        public static PPhDetail GetPPhDetail(string sup, int year,int month,int seqNo, string type)
        {
            PPhDetail pphDetail = new PPhDetail();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelPPhDetail";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@supcode", System.Data.SqlDbType.VarChar, sup);
                db.AddParameter("@year", System.Data.SqlDbType.Int, year);
                db.AddParameter("@month", System.Data.SqlDbType.Int, month);
                db.AddParameter("@seqNo", System.Data.SqlDbType.TinyInt, seqNo);
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 4);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            pphDetail.SeqNo = IDS.Tool.GeneralHelper.NullToInt(dr["SeqNo"], 0);
                            pphDetail.Supplier = new GeneralTable.Supplier();
                            pphDetail.Supplier.SupCode = IDS.Tool.GeneralHelper.NullToString(dr["CustSuppCode"], "");
                            //pphDetail.Supplier.SupName = IDS.Tool.GeneralHelper.NullToString(dr["CustName"], "");
                            pphDetail.Year = IDS.Tool.GeneralHelper.NullToInt(dr["Year"], 0);
                            pphDetail.Month = IDS.Tool.GeneralHelper.NullToInt(dr["Month"], 0);
                            pphDetail.MonthName = IDS.Tool.GeneralHelper.NullToString(dr["MonthName"], "");
                            pphDetail.SeqNo = IDS.Tool.GeneralHelper.NullToInt(dr["SeqNo"], 0);
                            pphDetail.Description = IDS.Tool.GeneralHelper.NullToString(dr["Description"], "");
                            pphDetail.TaxRate = IDS.Tool.GeneralHelper.NullToDecimal(dr["TaxRate"], 0);
                            pphDetail.Amount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            pphDetail.IsProcess = IDS.Tool.GeneralHelper.NullToBool(dr["IsProcess"]);
                            pphDetail.DasarPemotongan = IDS.Tool.GeneralHelper.NullToDecimal(dr["DasarPemotongan"], 0);
                            pphDetail.DasarPemotonganKumulatif = IDS.Tool.GeneralHelper.NullToDecimal(dr["DasarPemotonganKumulatif"], 0);
                            pphDetail.Tarif = IDS.Tool.GeneralHelper.NullToDecimal(dr["Tarif"], 0);
                            pphDetail.TarifNonNPWP = IDS.Tool.GeneralHelper.NullToDecimal(dr["TarifNonNPWP"], 0);
                            pphDetail.PPhTerutang = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPhTerutang"], 0);
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return pphDetail;
        }

        public static List<PPhDetail> GetPPhDetail(string sup,int year,string type)
        {
            List<PPhDetail> list = new List<PPhDetail>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "findPPhDetail";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, sup);
                db.AddParameter("@year", System.Data.SqlDbType.Int, year);
                db.AddParameter("@type", System.Data.SqlDbType.VarChar, type);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            PPhDetail pphDetail = new PPhDetail();
                            //pphDetail.Customer = new GeneralTable.Customer();
                            //pphDetail.Customer.CUSTCode = IDS.Tool.GeneralHelper.NullToString(dr["CustSuppCode"], "");
                            //pphDetail.Customer.CUSTName = IDS.Tool.GeneralHelper.NullToString(dr["CustName"], "");

                            pphDetail.SeqNo = IDS.Tool.GeneralHelper.NullToInt(dr["SeqNo"], 0);
                            pphDetail.MonthName = IDS.Tool.GeneralHelper.NullToString(dr["Month"], "");
                            pphDetail.TaxRate = IDS.Tool.GeneralHelper.NullToDecimal(dr["TaxRate"], 0);
                            pphDetail.Amount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            pphDetail.DasarPemotongan = IDS.Tool.GeneralHelper.NullToDecimal(dr["DasarPemotongan"], 0);
                            pphDetail.DasarPemotonganKumulatif = IDS.Tool.GeneralHelper.NullToDecimal(dr["DasarPemotonganKumulatif"], 0);
                            pphDetail.Tarif = IDS.Tool.GeneralHelper.NullToDecimal(dr["Tarif"], 0);
                            pphDetail.TarifNonNPWP = IDS.Tool.GeneralHelper.NullToDecimal(dr["TarifNonNPWP"], 0);
                            pphDetail.PPhTerutang = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPhTerutang"], 0);
                            list.Add(pphDetail);
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return list;
        }

        public static List<PPhDetail> GetPPhDetail(string sup, int year)
        {
            List<PPhDetail> list = new List<PPhDetail>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelPPhDetail";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@supcode", System.Data.SqlDbType.VarChar, sup);
                db.AddParameter("@year", System.Data.SqlDbType.Int, year);
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            PPhDetail pphDetail = new PPhDetail();
                            pphDetail.SeqNo = IDS.Tool.GeneralHelper.NullToInt(dr["SeqNo"], 0);
                            pphDetail.Supplier = new GeneralTable.Supplier();
                            pphDetail.Supplier.SupCode = IDS.Tool.GeneralHelper.NullToString(dr["CustSuppCode"], "");
                            //pphDetail.Supplier.SupName = IDS.Tool.GeneralHelper.NullToString(dr["CustName"], "");
                            pphDetail.Year = IDS.Tool.GeneralHelper.NullToInt(dr["Year"], 0);
                            pphDetail.Month = IDS.Tool.GeneralHelper.NullToInt(dr["Month"], 0);
                            pphDetail.MonthName = IDS.Tool.GeneralHelper.NullToString(dr["MonthName"], "");
                            pphDetail.SeqNo = IDS.Tool.GeneralHelper.NullToInt(dr["SeqNo"], 0);
                            pphDetail.Description = IDS.Tool.GeneralHelper.NullToString(dr["Description"], "");
                            pphDetail.TaxRate = IDS.Tool.GeneralHelper.NullToDecimal(dr["TaxRate"], 0);
                            pphDetail.Amount = IDS.Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            pphDetail.IsProcess = IDS.Tool.GeneralHelper.NullToBool(dr["IsProcess"]);
                            pphDetail.DasarPemotongan = IDS.Tool.GeneralHelper.NullToDecimal(dr["DasarPemotongan"], 0);
                            pphDetail.DasarPemotonganKumulatif = IDS.Tool.GeneralHelper.NullToDecimal(dr["DasarPemotonganKumulatif"], 0);
                            pphDetail.Tarif = IDS.Tool.GeneralHelper.NullToDecimal(dr["Tarif"], 0);
                            pphDetail.TarifNonNPWP = IDS.Tool.GeneralHelper.NullToDecimal(dr["TarifNonNPWP"], 0);
                            pphDetail.PPhTerutang = IDS.Tool.GeneralHelper.NullToDecimal(dr["PPhTerutang"], 0);
                            list.Add(pphDetail);
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
