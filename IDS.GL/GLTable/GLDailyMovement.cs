using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class GLDailyMovement
    {
        [Display(Name = "Account")]
        public IDS.GLTable.ChartOfAccount Acc { get; set; }

        [Display(Name = "Currency")]
        public IDS.GeneralTable.Currency Ccy { get; set; }

        [Display(Name = "Cash Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime CashDate { get; set; }

        [Display(Name = "Branch")]
        public IDS.GeneralTable.Branch Branch { get; set; }

        [Display(Name = "Cash DB")]
        public decimal CashDB { get; set; }

        [Display(Name = "Cash CR")]
        public decimal CashCR { get; set; }

        [Display(Name = "Cash Net")]
        public decimal CashNET { get; set; }

        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [Display(Name = "Last Update")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime LastUpdate { get; set; }

        public GLDailyMovement()
        {

        }
        
        public static List<GLDailyMovement> GetDailyMovement()
        {
            List<IDS.GLTable.GLDailyMovement> list = new List<GLDailyMovement>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelGLDailyMovement";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            GLDailyMovement dm = new GLDailyMovement();
                            dm.Acc = new GLTable.ChartOfAccount();
                            dm.Acc.Account = Tool.GeneralHelper.NullToString(dr["dmacc"]);
                            dm.Acc.AccountName = Tool.GeneralHelper.NullToString(dr["name"]);

                            dm.Ccy = new GeneralTable.Currency();
                            dm.Ccy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            dm.Branch = new GeneralTable.Branch();
                            dm.Branch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            dm.CashDate = Convert.ToDateTime((dr["CashDate"]));

                            dm.CashDB = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashDB"], 0);
                            dm.CashCR = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashCR"], 0);
                            dm.CashNET = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashNET"], 0);
                            dm.OperatorID = dr["OperatorID"] as string;
                            dm.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(dm);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<GLDailyMovement> GetDailyMovement(DateTime cashdate)
        {
            List<IDS.GLTable.GLDailyMovement> list = new List<GLDailyMovement>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelGLDailyMovement";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@cashdate", System.Data.SqlDbType.DateTime, cashdate);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            GLDailyMovement dm = new GLDailyMovement();
                            dm.Acc = new GLTable.ChartOfAccount();
                            dm.Acc.Account = Tool.GeneralHelper.NullToString(dr["dmacc"]);
                            dm.Acc.AccountName = Tool.GeneralHelper.NullToString(dr["name"]);

                            dm.Ccy = new GeneralTable.Currency();
                            dm.Ccy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            dm.Branch = new GeneralTable.Branch();
                            dm.Branch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            dm.CashDate = Convert.ToDateTime((dr["CashDate"]));

                            dm.CashDB = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashDB"], 0);
                            dm.CashCR = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashCR"], 0);
                            dm.CashNET = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashNET"], 0);
                            dm.OperatorID = dr["OperatorID"] as string;
                            dm.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(dm);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static GLDailyMovement GetDailyMovement(string acc,string ccy,DateTime cashdate,string branch)
        {
            GLDailyMovement dm = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelGLDailyMovement";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, acc);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@cashdate", System.Data.SqlDbType.DateTime, cashdate);
                db.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        dm = new GLDailyMovement();
                        dm.Acc = new GLTable.ChartOfAccount();
                        dm.Acc.Account = Tool.GeneralHelper.NullToString(dr["dmacc"]);
                        dm.Acc.AccountName = Tool.GeneralHelper.NullToString(dr["name"]);

                        dm.Ccy = new GeneralTable.Currency();
                        dm.Ccy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                        dm.Branch = new GeneralTable.Branch();
                        dm.Branch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                        dm.CashDate = Convert.ToDateTime((dr["CashDate"]));

                        dm.CashDB = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashDB"], 0);
                        dm.CashCR = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashCR"], 0);
                        dm.CashNET = IDS.Tool.GeneralHelper.NullToDecimal(dr["CashNET"], 0);
                        dm.OperatorID = dr["OperatorID"] as string;
                        dm.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return dm;
        }
        
        //public int InsUpDelFinancialRatio(int ExecCode)
        //{
        //    int result = 0;

        //    using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
        //    {
        //        try
        //        {
        //            cmd.CommandText = "GLUpdateFinancialRatio";
        //            cmd.AddParameter("@Type", System.Data.SqlDbType.Int, ExecCode);
        //            cmd.AddParameter("@RatioCode", System.Data.SqlDbType.VarChar, RatioCode);
        //            cmd.AddParameter("@Name", System.Data.SqlDbType.VarChar, RatioName);
        //            cmd.AddParameter("@InPercent", System.Data.SqlDbType.Bit, InPercent);
        //            cmd.AddParameter("@Formula", System.Data.SqlDbType.VarChar, Formula);
        //            cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.Open();

        //            cmd.BeginTransaction();
        //            result = cmd.ExecuteNonQuery();
        //            cmd.CommitTransaction();
        //        }
        //        catch (SqlException sex)
        //        {
        //            if (cmd.Transaction != null)
        //                cmd.RollbackTransaction();

        //            switch (sex.Number)
        //            {
        //                case 2627:
        //                    throw new Exception("Financial Ratio Code is already exists. Please choose other Financial Ratio.");
        //                default:
        //                    throw;
        //            }
        //        }
        //        catch
        //        {
        //            if (cmd.Transaction != null)
        //                cmd.RollbackTransaction();

        //            throw;
        //        }
        //        finally
        //        {
        //            cmd.Close();
        //        }
        //    }

        //    return result;
        //}

        //public int DeleteFinancialRatios(string[] data)
        //{
        //    int result = 0;

        //    if (data == null)
        //        throw new Exception("No data found");

        //    using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
        //    {
        //        try
        //        {
        //            cmd.CommandText = "GLUpdateFinancialRatio";
        //            cmd.Open();
        //            cmd.BeginTransaction();

        //            for (int i = 0; i < data.Length; i++)
        //            {
        //                cmd.AddParameter("@Type", System.Data.SqlDbType.Int, IDS.Tool.PageActivity.Delete);
        //                cmd.AddParameter("@RatioCode", System.Data.SqlDbType.VarChar, data[i]);
        //                cmd.AddParameter("@Name", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                cmd.AddParameter("@InPercent", System.Data.SqlDbType.Bit, DBNull.Value);
        //                cmd.AddParameter("@Formula", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);

        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //                cmd.ExecuteNonQuery();
        //            }

        //            cmd.CommitTransaction();
        //        }
        //        catch (SqlException sex)
        //        {
        //            if (cmd.Transaction != null)
        //                cmd.RollbackTransaction();

        //            switch (sex.Number)
        //            {
        //                case 2627:
        //                    throw new Exception("Country code is already exists. Please choose other country code.");
        //                case 547:
        //                    throw new Exception("One or more data can not be delete while data used for reference.");
        //                default:
        //                    throw;
        //            }
        //        }
        //        catch
        //        {
        //            if (cmd.Transaction != null)
        //                cmd.RollbackTransaction();

        //            throw;
        //        }
        //        finally
        //        {
        //            cmd.Close();
        //        }
        //    }

        //    return result;
        //}
    }
}
