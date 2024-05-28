using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GeneralTable
{
    public class Bank
    {
        [Display(Name = "Bank Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bank Code is required")]
        [MaxLength(20), StringLength(20)]
        public string BankCode { get; set; }

        [Display(Name = "Bank Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bank name is required")]
        [MaxLength(50)]
        public string BankName { get; set; }
        
        [Display(Name = "Bank Ccy")]
        public IDS.GeneralTable.Currency BankCcy { get; set; }

        [Display(Name = "Gel Account")]
        public IDS.GLTable.ChartOfAccount GelAccount { get; set; }

        [Display(Name = "Beneficiary")]
        [MaxLength(100), StringLength(100)]
        public string Beneficiary { get; set; }

        [Display(Name = "Rek. No")]
        [MaxLength(50), StringLength(50)]
        public string NoRek { get; set; }

        [Display(Name = "Address")]
        [MaxLength(100), StringLength(100)]
        public string Address { get; set; }

        public IDS.GeneralTable.City City { get; set; }

        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public Bank()
        {

        }

        public Bank(string bankCode)
        {
            BankCode = bankCode;
        }

        public static List<Bank> GetBank()
        {
            List<IDS.GeneralTable.Bank> list = new List<Bank>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBank";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@bankCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Bank bank = new Bank();
                            bank.BankCode = dr["BankCode"] as string;
                            bank.BankName = dr["BankName"] as string;
                            bank.Beneficiary = dr["Beneficiary"] as string;

                            bank.GelAccount = new IDS.GLTable.ChartOfAccount();
                            bank.GelAccount.Account = dr["GelAcc"] as string;

                            bank.BankCcy = new IDS.GeneralTable.Currency();
                            bank.BankCcy.CurrencyCode = dr["Ccy"] as string;

                            bank.NoRek = IDS.Tool.GeneralHelper.NullToString(dr["NoRek"], "");

                            bank.City = new City();
                            bank.City.CityCode = IDS.Tool.GeneralHelper.NullToString(dr["CityCode"], "");
                            bank.City.CityName = IDS.Tool.GeneralHelper.NullToString(dr["CityName"], "");

                            bank.Address = IDS.Tool.GeneralHelper.NullToString(dr["Address"], "");

                            bank.EntryUser = dr["EntryUser"] as string;
                            bank.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            bank.OperatorID = dr["OperatorID"] as string;
                            bank.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(bank);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static Bank GetBank(string bankCode)
        {
            Bank bank = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBank";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@bankCode", System.Data.SqlDbType.VarChar, bankCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        bank = new Bank();
                        bank.BankCode = dr["BankCode"] as string;
                        bank.BankName = dr["BankName"] as string;
                        bank.Beneficiary = dr["Beneficiary"] as string;

                        bank.GelAccount = new IDS.GLTable.ChartOfAccount();
                        bank.GelAccount.Account = dr["GelAcc"] as string;

                        bank.BankCcy = new IDS.GeneralTable.Currency();
                        bank.BankCcy.CurrencyCode = dr["Ccy"] as string;

                        bank.NoRek = IDS.Tool.GeneralHelper.NullToString(dr["NoRek"], "");

                        bank.City = new City();
                        bank.City.CityCode = IDS.Tool.GeneralHelper.NullToString(dr["CityCode"], "");
                        bank.City.CityName = IDS.Tool.GeneralHelper.NullToString(dr["CityName"], "");

                        bank.Address = IDS.Tool.GeneralHelper.NullToString(dr["Address"], "");

                        bank.EntryUser = dr["EntryUser"] as string;
                        bank.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        bank.OperatorID = dr["OperatorID"] as string;
                        bank.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return bank;
        }

        public int InsUpDelBank(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdtblBANK";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@BankCode", System.Data.SqlDbType.VarChar, BankCode);
                    cmd.AddParameter("@BankName", System.Data.SqlDbType.VarChar, BankName);
                    cmd.AddParameter("@Ccy", System.Data.SqlDbType.VarChar, BankCcy.CurrencyCode);
                    cmd.AddParameter("@Beneficiary", System.Data.SqlDbType.VarChar, Beneficiary);
                    cmd.AddParameter("@GelAcc", System.Data.SqlDbType.VarChar, GelAccount.Account);
                    cmd.AddParameter("@NoRek", System.Data.SqlDbType.VarChar, NoRek);
                    cmd.AddParameter("@CityCode", System.Data.SqlDbType.VarChar, City.CityCode);
                    cmd.AddParameter("@Address", System.Data.SqlDbType.VarChar, Address);
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
                            throw new Exception("Bank Code is already exists. Please choose other Bank Code.");
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

        public int InsUpDelBank(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdtblBANK";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GTUpdtblBANK";
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@BankCode", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Bank Code is already exists. Please choose other Bank Code.");
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

        public static List<System.Web.Mvc.SelectListItem> GetBankForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> banks = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBank";
                db.AddParameter("@bankCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        banks = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem bank = new System.Web.Mvc.SelectListItem();
                            bank.Value = dr["BankCode"] as string;
                            bank.Text = dr["BankName"] as string;

                            banks.Add(bank);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return banks;
        }

        public static string GetBankCodeWithAcc(string acc)
        {
            string result =""; 

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBank";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@bankCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@GelAcc", System.Data.SqlDbType.VarChar, acc);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = dr["BankCode"] as string;
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return result;
        }
    }
}
