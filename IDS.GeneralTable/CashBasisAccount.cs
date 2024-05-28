using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IDS.GLTable
{
    public sealed class CashBasisAccount
    {
        [Display(Name = "Account No"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account no is required")]
        public string Account { get; set; }

        [Display(Name = "Currency"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Currency is required")]
        public IDS.GeneralTable.Currency CCy { get; set; }

        [Display(Name = "Name"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account name is required")]
        public string AccountName { get; set; }

        [Display(Name = "Level"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account level is required")]
        public int Level { get; set; }

        [Display(Name = "Total / Detail"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account total / detail is required")]
        public bool AccountTotalDetail { get; set; }

        [Display(Name = "Group"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account group is required")]
        public IDS.Tool.GLAccountGroup AccountGroup { get; set; }

        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT),
            Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [Display(Name = "Last Update"),
           DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime LastUpdate { get; set; }

        [Display(Name = "BI Code")]
        public string BICode { get; set; }

        public CashBasisAccount()
        {
            
        }

        public static List<CashBasisAccount> GetCashBasisAccountForGrid(string parent)
        {
            List<CashBasisAccount> list = new List<CashBasisAccount>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACCGLMH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                if (string.IsNullOrWhiteSpace(parent))
                {
                    db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, parent);
                }

                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 6);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        list = new List<CashBasisAccount>();

                        while (dr.Read())
                        {
                            CashBasisAccount coa = new CashBasisAccount();
                            coa.Account = dr["ACC"] as string;
                            coa.AccountName = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                            coa.CCy = new GeneralTable.Currency();
                            coa.CCy.CurrencyCode = IDS.Tool.GeneralHelper.NullToString(dr["CCY"]);

                            coa.AccountTotalDetail = Convert.ToBoolean(dr["AT"]);
                            coa.Level = dr["TL"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TL"]);
                            coa.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt16(dr["AG"]);
                            coa.BICode = Tool.GeneralHelper.NullToString(dr["BICODE"]);
                            
                            //coa.EntryUser = dr["EntryUser"] as string;
                            //coa.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            coa.OperatorID = dr["OperatorID"] as string;
                            coa.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(coa);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static IList<CashBasisAccount> GetCashBasisAccount()
        {
            IList<CashBasisAccount> list = new List<CashBasisAccount>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACCGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 0);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CashBasisAccount coa = new CashBasisAccount();
                            coa.Account = dr["ACC"] as string;
                            coa.AccountName = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                            coa.CCy = IDS.GeneralTable.Currency.GetCurrency(IDS.Tool.GeneralHelper.NullToString(dr["CCY"]));
                            coa.Level = dr["TL"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TL"]);
                            coa.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt16(dr["AG"]);
                            coa.AccountTotalDetail = Convert.ToBoolean(dr["AT"]);
                            coa.BICode = Tool.GeneralHelper.NullToString(dr["BICODE"]);

                            coa.EntryUser = dr["EntryUser"] as string;
                            coa.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            coa.OperatorID = dr["OperatorID"] as string;
                            coa.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(coa);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static IList<CashBasisAccount> GetCashBasisAccount(string currency)
        {
            IList<CashBasisAccount> list = new List<CashBasisAccount>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACCGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, currency);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CashBasisAccount coa = new CashBasisAccount();
                            coa.Account = dr["ACC"] as string;
                            coa.AccountName = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                            coa.CCy = IDS.GeneralTable.Currency.GetCurrency(IDS.Tool.GeneralHelper.NullToString(dr["CCY"]));
                            coa.Level = dr["TL"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TL"]);
                            coa.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt16(dr["AG"]);
                            coa.AccountTotalDetail = Convert.ToBoolean(dr["AT"]);
                            coa.BICode = Tool.GeneralHelper.NullToString(dr["BICODE"]);

                            coa.EntryUser = dr["EntryUser"] as string;
                            coa.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            coa.OperatorID = dr["OperatorID"] as string;
                            coa.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(coa);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static CashBasisAccount GetCashBasisAccount(string currency, string account)
        {
            CashBasisAccount coa = new CashBasisAccount();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACCGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, account);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, string.IsNullOrEmpty(account) ? DBNull.Value : (object)account);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        
                        coa.Account = dr["ACC"] as string;
                        coa.AccountName = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                        coa.CCy = IDS.GeneralTable.Currency.GetCurrency(IDS.Tool.GeneralHelper.NullToString(dr["CCY"]));
                        coa.Level = dr["TL"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TL"]);
                        coa.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt16(dr["AG"]);
                        coa.AccountTotalDetail = Convert.ToBoolean(dr["AT"]);
                        coa.BICode = Tool.GeneralHelper.NullToString(dr["BICODE"]);
                        
                        coa.EntryUser = dr["EntryUser"] as string;
                        coa.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        coa.OperatorID = dr["OperatorID"] as string;
                        coa.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return coa;
        }

        public static List<System.Web.Mvc.SelectListItem> GetCashBasisAccountForDatasource(string currencyCode)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACCGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, currencyCode);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                            coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                            coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(coa);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public int InsUpDel(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLUpdateCashBasis";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Account);
                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                    cmd.AddParameter("@NAME", System.Data.SqlDbType.VarChar, AccountName);
                    cmd.AddParameter("@TL", System.Data.SqlDbType.TinyInt, Level);
                    cmd.AddParameter("@AT", System.Data.SqlDbType.TinyInt, AccountTotalDetail);
                    cmd.AddParameter("@AG", System.Data.SqlDbType.TinyInt, (int)AccountGroup);
                    cmd.AddParameter("@BICode", System.Data.SqlDbType.VarChar, BICode);
                    cmd.AddParameter("@OPERATORID", System.Data.SqlDbType.VarChar, OperatorID);
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
                            throw new Exception("Combination of Account Number and Currency data are already exists. Please choose other Account No and Currency.");
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

        public int InsUpDel(int ExecCode, string[] datas)
        {
            int result = 0;

            if (datas == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLUpdateCashBasis";
                    cmd.Open();
                    cmd.BeginTransaction();

                    string[] data;
                    string coa = "";
                    string ccy = "";

                    for (int i = 0; i < datas.Length; i++)
                    {
                        data = datas[i].Split(new char[] { ';' });

                        coa = data[0];
                        ccy = data[1];

                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, coa);
                        cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                        cmd.AddParameter("@NAME", System.Data.SqlDbType.VarChar, DBNull.Value);
                        cmd.AddParameter("@TL", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        cmd.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        cmd.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        cmd.AddParameter("@BICode", System.Data.SqlDbType.VarChar, DBNull.Value);
                        cmd.AddParameter("@OPERATORID", System.Data.SqlDbType.VarChar, OperatorID);
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
                            throw new Exception("Combination of Account Number and Currency data are already exists. Please choose other Account No and Currency.");
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