using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class SpecialAccount
    {
        [Display(Name = "Special Account ID")]
        public int ID { get; set; }

        [Display(Name = "Account Type")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Account Type is required")]
        [MaxLength(2)]
        public string TypeACC { get; set; }

        [Display(Name = "From CCY")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "From CCY is required")]
        [MaxLength(3)]
        public string FromCCY { get; set; }

        [Display(Name = "From ACC")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "From ACC is required")]
        [MaxLength(10)]
        public string FromACC { get; set; }

        [Display(Name = "To CCY")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "To CCY is required")]
        [MaxLength(3)]
        public string ToCCY { get; set; }

        [Display(Name = "To ACC")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "To ACC is required")]
        [MaxLength(10)]
        public string ToACC { get; set; }

        [Display(Name = "Office")]
        public int Office { get; set; }
        
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

        public SpecialAccount()
        {

        }

        public SpecialAccount(int spaccID, string typeAcc)
        {
            ID = spaccID;
            TypeACC = typeAcc;
        }

        public static SpecialAccount GetSpecialAccount(string spaccID)
        {
            SpecialAccount specialAccount = new SpecialAccount();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSpecAcc";
                db.AddParameter("@ID", System.Data.SqlDbType.VarChar, spaccID);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        
                        specialAccount.ID = Convert.ToInt16(dr["ID"]);
                        specialAccount.TypeACC = dr["TYPE_ACC"] as string;
                        specialAccount.FromCCY = dr["FR_CCY"] as string;
                        specialAccount.FromACC = dr["FR_ACC"] as string;
                        specialAccount.ToCCY = dr["TO_CCY"] as string; ;
                        specialAccount.ToACC = dr["TO_ACC"] as string;
                        if (dr["OFFICE"] != DBNull.Value)
                        {
                            specialAccount.Office = Convert.ToInt16(dr["OFFICE"]);
                        }
                        specialAccount.EntryUser = dr["EntryUser"] as string;
                        specialAccount.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        specialAccount.OperatorID = dr["OperatorID"] as string;
                        specialAccount.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return specialAccount;
        }

        public static SpecialAccount GetSpecialAccountWithAccount(string account)
        {
            SpecialAccount specialAccount = new SpecialAccount();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSpecAcc";
                db.AddParameter("@acc", System.Data.SqlDbType.VarChar, account);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 9);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        
                        specialAccount.FromCCY = dr["Account"] as string;
                        specialAccount.FromACC = dr["CCY"] as string;
                        specialAccount.TypeACC = dr["TYPE"] as string;

                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return specialAccount;
        }

        /// <summary>
        /// Retrieve semua daftar Special Account
        /// </summary>
        /// <returns></returns>
        public static List<SpecialAccount> GetSpecialAccount()
        {
            List<IDS.GLTable.SpecialAccount> list = new List<SpecialAccount>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSpecAcc";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@ID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            SpecialAccount specialAccount = new SpecialAccount();
                            specialAccount.ID = Convert.ToInt16(dr["ID"]);
                            specialAccount.TypeACC = dr["TYPE_ACC"] as string;
                            specialAccount.FromCCY = dr["FR_CCY"] as string;
                            specialAccount.FromACC = dr["FR_ACC"] as string;
                            specialAccount.ToCCY = dr["TO_CCY"] as string; ;
                            specialAccount.ToACC = dr["TO_ACC"] as string;
                            if (dr["OFFICE"] != DBNull.Value)
                            {
                                specialAccount.Office = Convert.ToInt16(dr["OFFICE"]);
                            }
                            specialAccount.EntryUser = dr["EntryUser"] as string;
                            specialAccount.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            specialAccount.OperatorID = dr["OperatorID"] as string;
                            specialAccount.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(specialAccount);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<SpecialAccount> GetSpecialAccountWithTypeAcc(string typeAcc)
        {
            List<IDS.GLTable.SpecialAccount> list = new List<SpecialAccount>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSpecAcc";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@ID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@typeAcc", System.Data.SqlDbType.VarChar, typeAcc);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            SpecialAccount specialAccount = new SpecialAccount();
                            specialAccount.ID = Convert.ToInt16(dr["ID"]);
                            specialAccount.TypeACC = Tool.GeneralHelper.NullToString(dr["TYPE_ACC"],"");
                            specialAccount.FromCCY = Tool.GeneralHelper.NullToString(dr["FR_CCY"], "");
                            specialAccount.FromACC = Tool.GeneralHelper.NullToString(dr["FR_ACC"], "");
                            specialAccount.ToCCY = Tool.GeneralHelper.NullToString(dr["TO_CCY"], "");
                            specialAccount.ToACC = Tool.GeneralHelper.NullToString(dr["TO_ACC"], "");
                            specialAccount.Office = Tool.GeneralHelper.NullToInt(dr["OFFICE"], 0);
                            specialAccount.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"], "");
                            specialAccount.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            specialAccount.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"], "");
                            specialAccount.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(specialAccount);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public int InsUpDelSpecialAccount(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLSpecAcc";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@ID", System.Data.SqlDbType.Int, ID);
                    cmd.AddParameter("@acctype", System.Data.SqlDbType.VarChar, TypeACC);
                    cmd.AddParameter("@fr_acc", System.Data.SqlDbType.VarChar, FromACC);
                    cmd.AddParameter("@to_acc", System.Data.SqlDbType.VarChar, ToACC);
                    cmd.AddParameter("@fr_ccy", System.Data.SqlDbType.VarChar, FromCCY);
                    cmd.AddParameter("@to_ccy", System.Data.SqlDbType.VarChar, ToCCY);
                    cmd.AddParameter("@office", System.Data.SqlDbType.TinyInt, Office);
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
                            throw new Exception("Special Account code is already exists. Please choose other Special Account.");
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

        public int InsUpDelSpecialAccount(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLSpecAcc";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GLSpecAcc";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@ID", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Special Account Code is already exists. Please choose other Special Account Code.");
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

        public static List<System.Web.Mvc.SelectListItem> GetTypeAccForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> typeAcc = new List<System.Web.Mvc.SelectListItem>();
            typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "PL - Profit Lost", Value = IDS.Tool.GLSpecialAccount.PL.ToString() });
            typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "AR - Account Receivable", Value = IDS.Tool.GLSpecialAccount.AR.ToString() });
            typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "AP - Account Payable", Value = IDS.Tool.GLSpecialAccount.AP.ToString() });
            typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "DL - Daily Balance Account", Value = IDS.Tool.GLSpecialAccount.DL.ToString() });
            typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "KS - Cash Account", Value = IDS.Tool.GLSpecialAccount.KS.ToString() });
            typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "BN - Bank Account", Value = IDS.Tool.GLSpecialAccount.BN.ToString() });
            typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "RE - Revaluation Account", Value = IDS.Tool.GLSpecialAccount.RE.ToString() });
            typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "BY - Expenses / Income Account", Value = IDS.Tool.GLSpecialAccount.BY.ToString() });


            return typeAcc;
        }

        public static List<System.Web.Mvc.SelectListItem> GetProfitOrLostTypeForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> profitOrLostType = new List<System.Web.Mvc.SelectListItem>();
            profitOrLostType.Add(new System.Web.Mvc.SelectListItem() { Text = "CE - Current Earning", Value = "CE" });
            profitOrLostType.Add(new System.Web.Mvc.SelectListItem() { Text = "EX - Expense", Value = "EX" });
            profitOrLostType.Add(new System.Web.Mvc.SelectListItem() { Text = "IC - Income", Value = "IC" });
            profitOrLostType.Add(new System.Web.Mvc.SelectListItem() { Text = "PL - PL-Summary", Value = "PL" });
            profitOrLostType.Add(new System.Web.Mvc.SelectListItem() { Text = "RE - Retained Earning", Value = "RE" });
            
            return profitOrLostType;
        }

        public static List<System.Web.Mvc.SelectListItem> GetSPACCForDatasource(string rptOf)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSpecAcc";
                db.AddParameter("@ID", System.Data.SqlDbType.VarChar, DBNull.Value);

                if (rptOf=="BOTH")
                {
                    db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                }
                else
                {
                    db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                    db.AddParameter("@typeAcc", System.Data.SqlDbType.VarChar, rptOf);
                }

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem spacc = new System.Web.Mvc.SelectListItem();
                            spacc.Value = IDS.Tool.GeneralHelper.NullToString(dr["FR_ACC"]);
                            spacc.Text = IDS.Tool.GeneralHelper.NullToString(dr["ACCNAME"]);

                            list.Add(spacc);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetSPACCForDatasource(string rptOf,string ccy)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSpecAcc";
                db.AddParameter("@ID", System.Data.SqlDbType.VarChar, DBNull.Value);

                if (rptOf == "BOTH")
                {
                    db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                }
                else
                {
                    db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                    db.AddParameter("@typeAcc", System.Data.SqlDbType.VarChar, rptOf);
                    db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                }

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem spacc = new System.Web.Mvc.SelectListItem();
                            spacc.Value = IDS.Tool.GeneralHelper.NullToString(dr["FR_ACC"]);
                            spacc.Text = IDS.Tool.GeneralHelper.NullToString(dr["ACCNAME"]);

                            list.Add(spacc);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetSPACCWithCcyForDataSource(string ccy)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSpecAcc";
                db.AddParameter("@ID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 6);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem spacc = new System.Web.Mvc.SelectListItem();
                            spacc.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACCOUNT"]);
                            spacc.Text = IDS.Tool.GeneralHelper.NullToString(dr["ACCOUNT"]) + " - " +IDS.Tool.GeneralHelper.NullToString(dr["ACCNAME"]);

                            list.Add(spacc);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetSPACCWithCcyForDataSource(string ccy,int payType)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSpecAcc";
                db.AddParameter("@ID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, payType == 0 ? 7 : 8);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem spacc = new System.Web.Mvc.SelectListItem();
                            spacc.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACCOUNT"]);
                            spacc.Text = IDS.Tool.GeneralHelper.NullToString(dr["ACCOUNT"]) + " - " + IDS.Tool.GeneralHelper.NullToString(dr["ACCNAME"]);

                            list.Add(spacc);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }
    }
}
