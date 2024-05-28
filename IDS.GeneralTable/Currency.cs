using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GeneralTable
{
    public class Currency
    {
        [Display(Name = "Currency Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency code is required")]
        [MaxLength(3), StringLength(3)]
        public string CurrencyCode { get; set; }

        [Display(Name = "Currency Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency name is required")]
        [MaxLength(20)]
        public string CurrencyName { get; set; }

        [Display(Name = "Country Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country Name is required")]
        public Country CountryCurrency { get; set; }

        [Display(Name = "Decimal Places")]
        [Range(0,10)]
        public int DecimalPlaces { get; set; }

        [Display(Name = "Rounding Up")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public bool RoundingUp { get; set; }

        [Display(Name = "Multiply")]
        public bool MultiplyDivided { get; set; }

        [Display(Name = "Variance Limit")]
        [Range(0,100)]
        public decimal VarianceLimit { get; set; }

        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        [Display(Name = "Rounding")]
        public int Rounding { get; set; }

        public Currency()
        {

        }

        public Currency(string currencyCode, string currencyName)
        {
            CurrencyCode = currencyCode;
            CurrencyName = currencyName;
        }

        public static Currency GetCurrency(string currencyCode)
        {
            Currency currency = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelCurrency";
                db.AddParameter("@CurrencyCode", System.Data.SqlDbType.VarChar, currencyCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        currency = new Currency();
                        currency.CurrencyCode = dr["CurrencyCode"] as string;
                        currency.CurrencyName = dr["CurrencyName"] as string;
                        currency.CountryCurrency = IDS.GeneralTable.Country.GetCountry(dr["CountryCode"] as string);
                        currency.DecimalPlaces = Convert.ToInt16(dr["DecimalPlaces"]);
                        currency.RoundingUp = Convert.ToBoolean(dr["RoundingUp"]);
                        currency.MultiplyDivided = Convert.ToBoolean(dr["MultiplyDivided"]);
                        currency.VarianceLimit = Convert.ToDecimal(dr["VarianceLimit"]);
                        currency.EntryUser = dr["EntryUser"] as string;
                        currency.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        currency.OperatorID = dr["OperatorID"] as string;
                        currency.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                        currency.Rounding = IDS.Tool.GeneralHelper.NullToInt16(dr["Rounding"], 0);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return currency;
        }

        /// <summary>
        /// Retrieve semua daftar Country
        /// </summary>
        /// <returns></returns>
        public static List<Currency> GetCurrencyList()
        {
            List<IDS.GeneralTable.Currency> list = new List<Currency>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelCurrency";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@CurrencyCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Currency currency = new Currency();
                            currency = new Currency();
                            currency.CurrencyCode = dr["CurrencyCode"] as string;
                            currency.CurrencyName = dr["CurrencyName"] as string;
                            currency.CountryCurrency = IDS.GeneralTable.Country.GetCountry(dr["CountryCode"] as string);
                            currency.DecimalPlaces = Convert.ToInt16(dr["DecimalPlaces"]);
                            currency.RoundingUp = Convert.ToBoolean(dr["RoundingUp"]);
                            currency.MultiplyDivided = Convert.ToBoolean(dr["MultiplyDivided"]);
                            currency.VarianceLimit = Convert.ToDecimal(dr["VarianceLimit"]);
                            currency.EntryUser = dr["EntryUser"] as string;
                            currency.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            currency.OperatorID = dr["OperatorID"] as string;
                            currency.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            currency.Rounding = IDS.Tool.GeneralHelper.NullToInt16(dr["Rounding"],0);

                            list.Add(currency);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetCurrencyForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> currencies = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelCurrency";
                db.AddParameter("@CurrencyCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        currencies = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem currency = new System.Web.Mvc.SelectListItem();
                            currency.Value = dr["CurrencyCode"] as string;
                            currency.Text = dr["CurrencyCode"] as string;

                            currencies.Add(currency);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return currencies;
        }

        public static List<System.Web.Mvc.SelectListItem> GetCurrencyForDataSource(string currencyCode)
        {
            List<System.Web.Mvc.SelectListItem> currencies = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelCurrency";
                db.AddParameter("@CurrencyCode", System.Data.SqlDbType.VarChar, currencyCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        currencies = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem currency = new System.Web.Mvc.SelectListItem();
                            currency.Value = dr["CurrencyCode"] as string;
                            currency.Text = dr["CurrencyCode"] as string;

                            currencies.Add(currency);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return currencies;
        }

        public static List<System.Web.Mvc.SelectListItem> GetCurrencyBaseOnChartOfAccountForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> currencies = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelCurrency";
                db.AddParameter("@CurrencyCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        currencies = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem currency = new System.Web.Mvc.SelectListItem();
                            currency.Value = Tool.GeneralHelper.NullToString(dr["CCY"]);
                            currency.Text = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            currencies.Add(currency);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return currencies;
        }

        public int InsUpDelCurrency(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdateCurrency";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@CurrencyCode", System.Data.SqlDbType.VarChar, CurrencyCode.ToString());
                    cmd.AddParameter("@CountryCode", System.Data.SqlDbType.VarChar, CountryCurrency.CountryCode.ToString());
                    cmd.AddParameter("@CurrencyName", System.Data.SqlDbType.VarChar, CurrencyName.ToString());
                    cmd.AddParameter("@DecimalPlaces", System.Data.SqlDbType.Money, DecimalPlaces);
                    cmd.AddParameter("@RoundingUp", System.Data.SqlDbType.Bit, RoundingUp);
                    cmd.AddParameter("@Rounding", System.Data.SqlDbType.Int, Rounding);
                    cmd.AddParameter("@MultiplyDivided", System.Data.SqlDbType.TinyInt, MultiplyDivided);
                    cmd.AddParameter("@VarianceLimit", System.Data.SqlDbType.Float, VarianceLimit);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID.ToString());
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
                            throw new Exception("Currency code is already exists. Please choose other Currency code.");
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

        public int InsUpDelCurrency(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdateCurrency";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GTUpdateCurrency";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@CurrencyCode", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Currency Code is already exists. Please choose other Currency Code.");
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

        public static decimal SetMidRateFormat(decimal midRate,string ccy)
        {
            Currency Ccy = GetCurrency(ccy);
            if (Ccy.Rounding == 1)
            {
                if (Ccy.RoundingUp == true)
                {
                    midRate = Math.Ceiling(midRate);
                }
                else
                {
                    midRate = Math.Round(midRate, Ccy.DecimalPlaces);
                }
            }
            else
            {
                midRate = Math.Round(midRate, Ccy.DecimalPlaces);
            }
            

            return midRate;
        }
    }
}
