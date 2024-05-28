using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class ExchangeRate
    {
        [Display(Name = "Exchange Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Exchange Date required")]
        public DateTime ExchangeDate { get; set; }

        [Display(Name = "Currency Code 1")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code 1 is required")]
        //[MaxLength(3)]
        public IDS.GeneralTable.Currency Currency1 { get; set; }

        [Display(Name = "Currency Code 2")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code 2 is required")]
        //[MaxLength(3)]
        public IDS.GeneralTable.Currency Currency2 { get; set; }

        [Display(Name = "Bid Rate")]
        //[Range(0, 255)]
        public decimal BidRate { get; set; }

        [Display(Name = "Offer Rate")]
        //[Range(0, 255)]
        public decimal OfferRate { get; set; }

        [Display(Name = "Mid Rate")]
        //[Range(0, 255)]
        public decimal MidRate { get; set; }
        
        [Display(Name = "Created By")]
        [MaxLength(20), StringLength(20)]
        public string EntryUser { get; set; }

        [Display(Name = "Created Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        [MaxLength(20), StringLength(20)]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public ExchangeRate()
        {

        }

        //public ExchangeRate(DateTime exchRateCode,string currency1)
        //{
        //    ExchangeDate = exchRateCode;
        //    Currency1.CurrencyCode = currency1;
        //}

        public static ExchangeRate GetExchangeRate(DateTime exchCode,string currencyCode1, string currencyCode2)
        {
            ExchangeRate exchRate = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelExchRate";
                db.AddParameter("@exchCode", System.Data.SqlDbType.DateTime,exchCode);
                db.AddParameter("@currency1", System.Data.SqlDbType.VarChar, currencyCode1);
                db.AddParameter("@currency2", System.Data.SqlDbType.VarChar, currencyCode2);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        exchRate = new ExchangeRate();
                        exchRate.ExchangeDate = Convert.ToDateTime(dr["ExchangeDate"]);
                        exchRate.Currency1 = IDS.GeneralTable.Currency.GetCurrency(dr["CurrencyCode1"].ToString());
                        exchRate.Currency2 = IDS.GeneralTable.Currency.GetCurrency(dr["CurrencyCode2"].ToString());
                        exchRate.BidRate = Convert.ToDecimal(dr["BidRate"]);
                        exchRate.OfferRate = Convert.ToDecimal(dr["OfferRate"]);
                        exchRate.MidRate = Convert.ToDecimal(dr["MidRate"]);
                        exchRate.EntryUser = dr["EntryUser"] as string;
                        exchRate.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        exchRate.OperatorID = dr["OperatorID"] as string;
                        exchRate.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return exchRate;
        }

        /// <summary>
        /// Retrieve semua daftar Area
        /// </summary>
        /// <returns></returns>
        public static List<ExchangeRate> GetExchangeRate()
        {
            List<ExchangeRate> list = new List<ExchangeRate>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelExchRate";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@exchCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ExchangeRate exchRate = new ExchangeRate();
                            exchRate.ExchangeDate = Convert.ToDateTime(dr["ExchangeDate"]);
                            exchRate.Currency1 = IDS.GeneralTable.Currency.GetCurrency(dr["CurrencyCode1"].ToString());
                            exchRate.Currency2 = IDS.GeneralTable.Currency.GetCurrency(dr["CurrencyCode2"].ToString());
                            exchRate.BidRate = Convert.ToDecimal(dr["BidRate"]);
                            exchRate.OfferRate = Convert.ToDecimal(dr["OfferRate"]);
                            exchRate.MidRate = Convert.ToDecimal(dr["MidRate"]);
                            exchRate.EntryUser = dr["EntryUser"] as string;
                            exchRate.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            exchRate.OperatorID = dr["OperatorID"] as string;
                            exchRate.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(exchRate);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static decimal GetMidRate(string ccy1, string ccy2, DateTime exchDate)
        {
            decimal result = 0;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelExchRate";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@currency1", System.Data.SqlDbType.VarChar, ccy1);
                db.AddParameter("@currency2", System.Data.SqlDbType.VarChar, ccy2);
                db.AddParameter("@exchCode", System.Data.SqlDbType.DateTime, exchDate);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = IDS.Tool.GeneralHelper.NullToDecimal(dr["MidRate"], 0);
                        }
                    }
                    else
                    {
                        result = 1;
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return result;
        }

        public static decimal GetMidRate(string ccy1, string ccy2)
        {
            decimal result = 0;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelExchRate";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@currency1", System.Data.SqlDbType.VarChar, ccy1);
                db.AddParameter("@currency2", System.Data.SqlDbType.VarChar, ccy2);
                db.AddParameter("@exchCode", System.Data.SqlDbType.DateTime, DateTime.Now);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = IDS.Tool.GeneralHelper.NullToDecimal(dr["MidRate"], 0);
                        }
                    }
                    else
                    {
                        result = 1;
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return result;
        }

        public int InsUpDelExchangeRate(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLExchRate";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@exchDate", System.Data.SqlDbType.DateTime, ExchangeDate);
                    cmd.AddParameter("@Currency1", System.Data.SqlDbType.VarChar, Currency1.CurrencyCode);
                    cmd.AddParameter("@Currency2", System.Data.SqlDbType.VarChar, Currency2.CurrencyCode);
                    cmd.AddParameter("@BidRate", System.Data.SqlDbType.Money, BidRate);
                    cmd.AddParameter("@OfferRate", System.Data.SqlDbType.Money, OfferRate);
                    cmd.AddParameter("@MidRate", System.Data.SqlDbType.Money, MidRate);
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
                            throw new Exception("Exchange code is already exists. Please choose other Exchange code.");
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

        public int InsUpDelExchangeRate(int ExecCode, string[] data, string[] currency1, string[] currency2)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLExchRate";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GLExchRate";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@exchDate", System.Data.SqlDbType.DateTime, Convert.ToDateTime(data[i]));
                        cmd.AddParameter("@currency1", System.Data.SqlDbType.VarChar, currency1[i]);
                        cmd.AddParameter("@currency2", System.Data.SqlDbType.VarChar, currency2[i]);
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
                            throw new Exception("Exchange Code is already exists. Please choose other Exchange Code.");
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
