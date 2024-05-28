using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IDS.GLTransaction
{
    public class BankStatement
    {
        [Display(Name = "No")]
        public int? Counter { get; set; }

        [Display(Name = "Account No."), Required(AllowEmptyStrings = false, ErrorMessage = "Account No is required")]
        public string AccountNo { get; set; }

        [Display(Name = "Currency"), Required(AllowEmptyStrings = false, ErrorMessage = "Currency is required")]
        public string Currency { get; set; }

        [Display(Name = "Period")]
        public string Period { get; set; }

        [Display(Name = "Doc. Bank")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Doc. bank is required")]
        public string DocBank { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        [Display(Name = "Amount"), Required(AllowEmptyStrings = false, ErrorMessage = "Amount is required")]
        public double AmountBank { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Bank Balance")]
        public double SaldoBank { get; set; }

        [Display(Name = "Remark"), Required(AllowEmptyStrings = false, ErrorMessage = "Remark is required")]
        public string Remark { get; set; }

        [Display(Name = "Trans. Date"), Required(AllowEmptyStrings = false, ErrorMessage = "Trans. date is required")]
        public DateTime TransDate { get; set; }

        [Display(Name = "Branch"), Required(AllowEmptyStrings = false, ErrorMessage = "Branch is required")]
        public string BranchCode { get; set; }

        [Display(Name = "Status")]
        public int MatchStatus { get; set; } // 0 = Match, 1 = Unmatch, 2 = All, 


        public BankStatement()
        {

        }

        public static List<BankStatement> GetBankStatement(string period, string ccy, string accNo, string branchCode, int? status)
        {
            List<BankStatement> bankStatement = new List<BankStatement>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFStatBank";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Period", System.Data.SqlDbType.Text, period);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, accNo);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@MatchStatus", System.Data.SqlDbType.TinyInt, status == null ? DBNull.Value : (object)status);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);

                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BankStatement item = new BankStatement();
                            item.Counter = Convert.ToInt32(dr["ctr"]);
                            item.AccountNo = Tool.GeneralHelper.NullToString(dr["Acc"]);
                            item.Currency = Tool.GeneralHelper.NullToString(dr["Ccy"]);
                            item.Period = Tool.GeneralHelper.NullToString(dr["Mn"]);
                            item.DocBank = Tool.GeneralHelper.NullToString(dr["DocBank"]);
                            item.MatchStatus = Convert.ToInt16(Tool.GeneralHelper.NullToBool(dr["MatchStatus"], false));
                            item.AmountBank = Tool.GeneralHelper.NullToDouble(dr["AmountBank"], 0);
                            item.SaldoBank = Tool.GeneralHelper.NullToDouble(dr["SaldoBank"], 0);
                            item.Remark = Tool.GeneralHelper.NullToString(dr["Remark"]);
                            item.TransDate = Tool.GeneralHelper.NullToDateTime(dr["TransDate"], DateTime.Now.Date);
                            item.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            bankStatement.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return bankStatement;
        }

        public static BankStatement GetBankStatement(int? ctrNo, string branchCode)
        {
            BankStatement item = null;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFStatBank";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Ctrl", System.Data.SqlDbType.Int, (object)ctrNo ?? DBNull.Value);
                db.AddParameter("@Period", System.Data.SqlDbType.Text, DBNull.Value);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@MatchStatus", System.Data.SqlDbType.Bit, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);

                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        item = new BankStatement();
                        item.Counter = Convert.ToInt32(dr["ctr"]);
                        item.AccountNo = Tool.GeneralHelper.NullToString(dr["Acc"]);
                        item.Currency = Tool.GeneralHelper.NullToString(dr["Ccy"]);
                        item.Period = Tool.GeneralHelper.NullToString(dr["Mn"]);
                        item.DocBank = Tool.GeneralHelper.NullToString(dr["DocBank"]);
                        item.MatchStatus = Tool.GeneralHelper.NullToInt(dr["MatchStatus"], 0);
                        item.AmountBank = Tool.GeneralHelper.NullToDouble(dr["AmountBank"], 0);
                        item.SaldoBank = Tool.GeneralHelper.NullToDouble(dr["SaldoBank"], 0);
                        item.Remark = Tool.GeneralHelper.NullToString(dr["Remark"]);
                        item.TransDate = Tool.GeneralHelper.NullToDateTime(dr["TransDate"], DateTime.Now.Date);
                        item.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return item;
        }

        public static bool CheckMatchDataExists(string branchCode, string period, string AccNo, string ccy)
        {
            bool result = false;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFStatBank";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Period", System.Data.SqlDbType.Text, period);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, AccNo);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);

                db.Open();

                result = Convert.ToBoolean(db.ExecuteScalar());
            }

            return result;
        }

        public static string GetBeginningAndEndingBalance(string branchCode, string period, string accNo, string ccy)
        {
            string result = "|";

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFStatBank";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Period", System.Data.SqlDbType.Text, period);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, accNo);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);

                db.Open();

                db.ExecuteReader();

                double begBal = 0;
                double endBal = 0;

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        begBal = Convert.ToDouble(dr["BegBal"]);
                        endBal = Convert.ToDouble(dr["EndBal"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                result = begBal.ToString("N2") + "|" + endBal.ToString("N2");
            }

            return result;
        }

        public int InsUpDel(int ExeCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "GLUpdAcfBankStat";
                    db.AddParameter("@Type", System.Data.SqlDbType.Int, ExeCode);
                    //db.AddParameter("@CtrNo", System.Data.SqlDbType.Int, Counter);
                    db.AddParameter("@AccNo", System.Data.SqlDbType.VarChar, AccountNo);
                    db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, Currency);
                    db.AddParameter("@Period", System.Data.SqlDbType.VarChar, Period);
                    db.AddParameter("@DocBank", System.Data.SqlDbType.VarChar, DocBank);
                    db.AddParameter("@AmountBank", System.Data.SqlDbType.VarChar, AmountBank);
                    db.AddParameter("@SaldoBank", System.Data.SqlDbType.Money, 0);
                    db.AddParameter("@Remark", System.Data.SqlDbType.VarChar, Remark);
                    db.AddParameter("@TransDate", System.Data.SqlDbType.DateTime, TransDate);
                    db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchCode);
                    db.AddParameter("@MatchStatus", System.Data.SqlDbType.Bit, MatchStatus);

                    db.AddParameter("@CtrNo", System.Data.SqlDbType.Int, Counter);

                    if (ExeCode == 1)
                    {
                        (db.DbCommand.Parameters["@CtrNo"] as System.Data.SqlClient.SqlParameter).Direction = System.Data.ParameterDirection.Output;
                        (db.DbCommand.Parameters["@CtrNo"] as System.Data.SqlClient.SqlParameter).IsNullable = false;
                        //(db.DbCommand.Parameters["@CtrNo"] as System.Data.SqlClient.SqlParameter).Size = 50;
                    }



                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();

                    db.BeginTransaction();
                    result = db.ExecuteNonQueryWithParamOutput();
                    db.CommitTransaction();

                    if (result >= 1 && (db.DbCommand.Parameters["@CtrNo"] as System.Data.SqlClient.SqlParameter).Value != null && (db.DbCommand.Parameters["@CtrNo"] as System.Data.SqlClient.SqlParameter).Value != DBNull.Value)
                    {
                        result = Convert.ToInt32((db.DbCommand.Parameters["@CtrNo"] as System.Data.SqlClient.SqlParameter).Value);
                    }
                }
                catch (Exception)
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    throw;
                }
                finally
                {
                    db.Close();
                }
            }

            return result;
        }

        public int InsUpDel(int ExeCode, string[] data)
        {
            int result = 0;

            if (data == null || data.Length == 0)
                return result;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "GLUpdAcfBankStat";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();

                    db.BeginTransaction();
                    db.AddParameter("@AccNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@Period", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@DocBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@AmountBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@SaldoBank", System.Data.SqlDbType.Money, DBNull.Value);
                    db.AddParameter("@Remark", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.AddParameter("@TransDate", System.Data.SqlDbType.DateTime, DBNull.Value);
                    db.AddParameter("@MatchStatus", System.Data.SqlDbType.Bit, DBNull.Value);

                    string[] temp;
                    int counter = -1;
                    string branch = "";

                    for (int i = 0; i < data.Length; i++)
                    {
                        temp = data[i].Split(';');
                        counter = Convert.ToInt32(temp[0]);
                        branch = temp[1];


                        db.AddParameter("@Type", System.Data.SqlDbType.Int, 3);
                        db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                        db.AddParameter("@CtrNo", System.Data.SqlDbType.Int, counter);

                        result = db.ExecuteNonQuery();
                    }


                    db.CommitTransaction();
                }
                catch (Exception)
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    throw;
                }
                finally
                {
                    db.Close();
                }
            }

            return result;
        }

        public int UpdateMatchStatus(string bankStatementData, string tranD)
        {
            int result = 1;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.Open();
                    db.BeginTransaction();

                    //// Set Unmatch untuk data per 1 period data
                    BankStatement item = new BankStatement();
                    //item.MatchStatus = 0;
                    //item.UpdateMatchStatusPerPeriod(db);

                    // Update Match Status
                    item.MatchStatus = 1;

                    if (!string.IsNullOrWhiteSpace(bankStatementData))
                    {
                        string[] dataBank = bankStatementData.Split(',');

                        if (dataBank.Length > 0)
                        {
                            for (int i = 0; i < dataBank.Length; i++)
                            {
                                result = item.SetMatchStatus(db, dataBank[i]);
                            }
                        }
                    }

                    if (tranD != null)
                    {
                        string[] dataTranD = tranD.Split(',');

                        if (dataTranD.Length > 0)
                        {
                            for (int i = 0; i < dataTranD.Length; i++)
                            {
                                result = item.SetTranDMatchStatus(db, dataTranD[i]);
                            }
                        }
                    }

                    db.CommitTransaction();

                    return result;
                }
                catch (Exception ex)
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    throw ex;
                }
            }
        }

        private int SetMatchStatus(IDS.DataAccess.SqlServer db, string key)
        {
            int result = 0;

            if (db == null)
                throw new Exception("Database connection is lost or close please contact your administrator");

            try
            {
                db.CommandText = "GLUpdAcfBankStat";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                db.AddParameter("@MatchStatus", System.Data.SqlDbType.TinyInt, MatchStatus);
                db.AddParameter("@CombineKey", System.Data.SqlDbType.VarChar, key);

                //db.AddParameter("@CtrNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@AccNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@Period", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@DocBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@AmountBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@SaldoBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@Remark", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@TransDate", System.Data.SqlDbType.VarChar, DBNull.Value);


                result = db.ExecuteNonQuery();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int SetTranDMatchStatus(IDS.DataAccess.SqlServer db, string key)
        {
            int result = 0;

            if (db == null)
                throw new Exception("Database connection is lost or close please contact your administrator");

            try
            {
                db.CommandText = "GLUpdAcfBankStat";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 6);
                db.AddParameter("@MatchStatus", System.Data.SqlDbType.TinyInt, MatchStatus);
                db.AddParameter("@CombineKey", System.Data.SqlDbType.VarChar, key);

                //db.AddParameter("@CtrNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@AccNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@Period", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@DocBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@AmountBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@SaldoBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@Remark", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@TransDate", System.Data.SqlDbType.VarChar, DBNull.Value);


                result = db.ExecuteNonQuery();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Set Match / UnMatch sesuai parameter status match untuk 1 periode per Account dan per cabang
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private int UpdateMatchStatusPerPeriod(IDS.DataAccess.SqlServer db)
        {
            int result = 0;

            if (db == null)
                throw new Exception("Database connection is lost or close please contact your administrator");

            try
            {
                db.CommandText = "GLUpdAcfBankStat";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.AddParameter("@AccNo", System.Data.SqlDbType.VarChar, AccountNo);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, Currency);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, Period);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchCode);
                db.AddParameter("@MatchStatus", System.Data.SqlDbType.TinyInt, MatchStatus);
                db.AddParameter("@CtrNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@DocBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AmountBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@SaldoBank", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Remark", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@TransDate", System.Data.SqlDbType.VarChar, DBNull.Value);

                result = db.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}