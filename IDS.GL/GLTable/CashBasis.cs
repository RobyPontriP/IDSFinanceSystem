using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class CashBasis
    {
        [Display(Name = "Account")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Account No is required")]
        //[MaxLength(10), StringLength(10)]
        public IDS.GLTable.ChartOfAccount COACashBasis { get; set; }

        [Display(Name = "Currency Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        //[MaxLength(3)]
        public IDS.GeneralTable.Currency CurrencyCashBasis { get; set; }

        [Display(Name = "CHName")]
        [MaxLength(200), StringLength(200)]
        public string CHName { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [MaxLength(200), StringLength(200)]
        public string Name { get; set; }

        public int TL { get; set; }
        public int AT { get; set; }
        public int AG { get; set; }

        [Display(Name = "BI Code")]
        [MaxLength(10), StringLength(10)]
        public string BICode { get; set; }

        [Display(Name = "Created By")]
        [MaxLength(20), StringLength(20)]
        public string EntryUser { get; set; }

        [Display(Name = "Created Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public CashBasis()
        {

        }

        public static List<CashBasis> GetCashBasis()
        {
            List<IDS.GLTable.CashBasis> list = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelCashBasis";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        list = new List<CashBasis>();

                        while (dr.Read())
                        {
                            CashBasis cashBasis = new CashBasis();

                            cashBasis.COACashBasis = new IDS.GLTable.ChartOfAccount();
                            cashBasis.COACashBasis.Account = dr["ACC"] as string;
                            cashBasis.COACashBasis.CCy = new GeneralTable.Currency();
                            cashBasis.COACashBasis.CCy.CurrencyCode = dr["CCY"] as string;
                            cashBasis.COACashBasis.CCy.CurrencyName = dr["CurrencyName"] as string;
                            cashBasis.COACashBasis.CCy.CountryCurrency = new GeneralTable.Country();
                            cashBasis.COACashBasis.CCy.CountryCurrency.CountryCode = dr["CountryCode"] as string;
                            cashBasis.COACashBasis.CCy.CountryCurrency.CountryName = dr["CountryName"] as string;
                            //cashBasis.COACashBasis.CCy.CountryCurrency.SLIKCode = dr["SLIKCode"] as string; // Untuk multifinance aktifkan

                            cashBasis.CurrencyCashBasis = new GeneralTable.Currency();
                            cashBasis.CurrencyCashBasis.CurrencyCode = dr["CCY"] as string;
                            cashBasis.CurrencyCashBasis.CurrencyName = dr["CurrencyName"] as string;
                            cashBasis.CurrencyCashBasis.CountryCurrency = new GeneralTable.Country();
                            cashBasis.CurrencyCashBasis.CountryCurrency.CountryCode = dr["CountryCode"] as string;
                            cashBasis.CurrencyCashBasis.CountryCurrency.CountryName = dr["CountryName"] as string;
                            //cashBasis.CurrencyCashBasis.CountryCurrency.SLIKCode = dr["SLIKCode"] as string; // Untuk multifinance aktifkan

                            cashBasis.Name = dr["NAME"] as string;
                            cashBasis.EntryUser = dr["OperatorID"] as string;
                            cashBasis.EntryDate = Convert.ToDateTime(dr["LastUpdate"]);
                            cashBasis.OperatorID = dr["OperatorID"] as string;
                            cashBasis.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(cashBasis);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static CashBasis GetCashBasis(string acc,string ccy)
        {
            CashBasis cashBasis = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelCashBasis";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, acc);
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        cashBasis = new CashBasis();

                        cashBasis.COACashBasis = new IDS.GLTable.ChartOfAccount();
                        cashBasis.COACashBasis.Account = dr["ACC"] as string;

                        cashBasis.CurrencyCashBasis = new GeneralTable.Currency();
                        cashBasis.CurrencyCashBasis.CurrencyCode = dr["CCY"] as string;

                        cashBasis.Name = dr["NAME"] as string;
                        cashBasis.EntryUser = dr["OperatorID"] as string;
                        cashBasis.EntryDate = Convert.ToDateTime(dr["LastUpdate"]);
                        cashBasis.OperatorID = dr["OperatorID"] as string;
                        cashBasis.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return cashBasis;
        }

        public int InsUpDelCashBasis(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLUpdateCashBasis";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, COACashBasis.Account);
                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CurrencyCashBasis.CurrencyCode);
                    cmd.AddParameter("@Name", System.Data.SqlDbType.VarChar, Name);
                    cmd.AddParameter("@TL", System.Data.SqlDbType.TinyInt, TL);
                    cmd.AddParameter("@AT", System.Data.SqlDbType.TinyInt, AT);
                    cmd.AddParameter("@AG", System.Data.SqlDbType.TinyInt, AG);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
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
                            throw new Exception("Cash Basis is already exists. Please choose other Cash Basis.");
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

        public int InsUpDelCashBasis(int ExecCode, string[] dataACC, string[] dataCCY)
        {
            int result = 0;

            if (dataACC == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLUpdateCashBasis";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < dataACC.Length; i++)
                    {
                        cmd.CommandText = "GLUpdateCashBasis";
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, dataACC[i]);
                        cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, dataCCY[i]);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommitTransaction();
                }
                catch (SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Cash Basis is already exists. Please choose other Cash Basis.");
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
