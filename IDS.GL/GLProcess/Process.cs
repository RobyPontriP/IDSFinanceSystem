using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLProcess
{
    public class Process
    {
        public string OperatorID { get; set; }
        public DateTime LastUpdate { get; set; }
        public string MessageError { get; set; }

        public Process()
        {

        }

        public string GLProcess(DateTime dtPeriod, string branch, string ChkProcess)
        {
            string strResult = "";
            string[] dataToProcess = ChkProcess.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            using(DataAccess.SqlServer cmd = new DataAccess.SqlServer())
            {
                //cmd.Open();
                //cmd.BeginTransaction();
                for (int i = 0; i < dataToProcess.Length; i++)
                {
                    string value = dataToProcess[i];

                    switch (value)
                    {
                        case "1":
                            if (SOProcCreateJournal(dtPeriod, branch,cmd) >= 1)
                            {
                                strResult = strResult + "1 1,";
                            }
                            else
                            {
                                strResult = strResult + "1 0,";
                            }
                            break;

                        case "2":
                            if (PostGLTrans(dtPeriod, branch,cmd) >= 1)
                            {
                                strResult = strResult + "2 1,";
                            }
                            else
                            {
                                strResult = strResult + "2 0,";
                            }
                            break;

                        case "3":
                            if (PostRevaluation(dtPeriod, branch,cmd) >= 1)
                            {
                                strResult = strResult + "3 1,";
                            }
                            else
                            {
                                strResult = strResult + "3 0,";
                            }
                            break;

                        case "4":
                            if (CalcUpdateCA(dtPeriod, branch,cmd) >= 1)
                            {
                                strResult = strResult + "4 1,";
                            }
                            else
                            {
                                strResult = strResult + "4 0,";
                            }
                            break;

                        case "5":
                            if (PostingPL(dtPeriod, branch,cmd) >= 1)
                            {
                                strResult = strResult + "5 1,";
                            }
                            else
                            {
                                strResult = strResult + "5 0,";
                            }
                            break;

                        case "6":
                            if (EndOfPeriod(dtPeriod, branch,cmd) >= 1)
                            {
                                strResult = strResult + "6 1,";
                            }
                            else
                            {
                                strResult = strResult + "6 0,";
                            }
                            break;

                        case "7":
                            if (MonthlyClosing(dtPeriod, branch,cmd) >= 1)
                            {
                                strResult = strResult + "7 1,";
                            }
                            else
                            {
                                strResult = strResult + "7 0,";
                            }
                            break;

                        case "8":
                            if (BeginningYear(dtPeriod, branch, cmd) >= 1)
                            {
                                strResult = strResult + "8 1,";
                            }
                            else
                            {
                                strResult = strResult + "8 0,";
                            }
                            break;
                    }
                }
                return strResult;
            }
        }

        private int SOProcCreateJournal(DateTime mparm,string branch, DataAccess.SqlServer cmd)
        {
            string strDate = "";
            string mPeriod = "";
            string MNPeriod;
            bool i = false;
            int result = 0;

            strDate = Convert.ToString(mparm.Date.Day);

            mPeriod = SetPeriod(mparm);

            MNPeriod = SetPeriod(new DateTime(mparm.Year, mparm.Month, 1).AddMonths(1));
            
            try
            {
                cmd.CommandText = "GLProcActiveStandingOrder";
                cmd.AddParameter("@pExpDate", System.Data.SqlDbType.DateTime, mparm); //DateTime
                cmd.AddParameter("@pPeriod", System.Data.SqlDbType.VarChar, mPeriod); //string , "202110"
                cmd.AddParameter("@pExecD", System.Data.SqlDbType.TinyInt, strDate); // string, tanggal ("6")
                cmd.AddParameter("@pCurrUser", System.Data.SqlDbType.VarChar, OperatorID);
                cmd.AddParameter("@pCurrDate", System.Data.SqlDbType.DateTime, DateTime.Now);
                cmd.AddParameter("@pMNPeriod", System.Data.SqlDbType.VarChar, MNPeriod); // string, mPeriod + 1 ="202111"
                cmd.AddParameter("@pBranchCode", System.Data.SqlDbType.VarChar, branch);// string kode branch
                cmd.AddParameter("@pOut", System.Data.SqlDbType.Bit, i);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.DbCommand.CommandTimeout = 0;
                cmd.Open();

                cmd.BeginTransaction();
                //result = cmd.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                result = 1;
                cmd.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageError = string.IsNullOrEmpty(ex.Message) ? ex.Message : "* Automatic Journal from Standing Order \n" + "    - "+ ex.Message + "\n \n";
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();
                throw;
            }
            finally
            {
                //string test = "teesstttttt";
                //MessageError += string.IsNullOrEmpty(test) ? test : "* Automatic Journal from Standing Order \n" + "    - " + test + "\n \n";
                cmd.Close();
            }  
            return result;
        }

        private int PostGLTrans(DateTime mPeriod,string branch, DataAccess.SqlServer cmd)
        {
            string strBln = "";
            string strThn = "";
            string strMonth;
            string strB;
            string strT;
            int result = 0;
            strBln = Convert.ToString(mPeriod.Date.Month);
            if (Convert.ToInt16(strBln) > 0 && Convert.ToInt16(strBln) < 10)
            {
                strBln = Convert.ToString('0') + strBln;
            }
            strThn = Convert.ToString(mPeriod.Date.Year);
            strMonth = strThn + strBln;
            strB = strBln;
            strT = Convert.ToString(mPeriod.Date.Year);
            bool i = false;

            try
            {
                cmd.CommandText = "GLProcPostGLTrans";
                cmd.AddParameter("@pBranch", System.Data.SqlDbType.VarChar, branch); // branchCode "KPNO"
                cmd.AddParameter("@pTDate", System.Data.SqlDbType.DateTime,mPeriod); // DateTime
                cmd.AddParameter("@mmonth", System.Data.SqlDbType.VarChar, strMonth); // "202110"
                cmd.AddParameter("@pCurrUser", System.Data.SqlDbType.VarChar, OperatorID);
                cmd.AddParameter("@pCurrDate", System.Data.SqlDbType.DateTime, DateTime.Now);
                cmd.AddParameter("@pOut", System.Data.SqlDbType.Bit,i);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.DbCommand.CommandTimeout = 0;
                cmd.Open();

                cmd.BeginTransaction();
                //result = cmd.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                result = 1;
                cmd.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageError = string.IsNullOrEmpty(ex.Message) ? ex.Message : "* Post Transaction \n" + "    - " + ex.Message + "\n \n";
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();
                //throw;
            }
            finally
            {
                cmd.Close();
            }
            
            return result;
        }

        private int PostRevaluation(DateTime dtPeriod,string branch, DataAccess.SqlServer cmd)
        {
            int result = 0;
            DateTime dtBln, dtDate;
            DateTime dtDummy;
            string strB, strThn, strMonth;
            bool boolTrigger = false;
            strB = Convert.ToString(dtPeriod.Date.Month);
            if (Convert.ToInt16(strB) > 0 && Convert.ToInt16(strB) < 10)
            {
                strB = "0" + strB;
            }
            
            strThn = Convert.ToString(dtPeriod.Date.Year);
            strMonth = strThn + strB; ;
            dtDummy = dtPeriod.AddMonths(1);
            dtBln = new DateTime(dtPeriod.Year, dtPeriod.Month, 1).AddMonths(1).AddDays(-1);
            dtDate = dtBln;

            try
            {
                cmd.CommandText = "GLProcPostReval";
                cmd.AddParameter("@pBranch", System.Data.SqlDbType.VarChar, branch);
                cmd.AddParameter("@pPeriod", System.Data.SqlDbType.DateTime, dtBln);
                cmd.AddParameter("@mrDate", System.Data.SqlDbType.VarChar, dtBln);
                cmd.AddParameter("@pCurrUser", System.Data.SqlDbType.VarChar, OperatorID);
                cmd.AddParameter("@pCurrDate", System.Data.SqlDbType.DateTime, DateTime.Now);
                cmd.AddParameter("@pOut", System.Data.SqlDbType.Bit, boolTrigger);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.DbCommand.CommandTimeout = 0;

                cmd.Open();

                cmd.BeginTransaction();
                //result = cmd.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                result = 1;
                cmd.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageError = string.IsNullOrEmpty(ex.Message) ? ex.Message : "* Calculate and Post Revaluation Transactions \n" + "    - " + ex.Message + "\n \n";
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();
                throw;
            }
            finally
            {
                cmd.Close();
            }
            return result;
        }

        private int CalcUpdateCA(DateTime dtPeriod,string branch,DataAccess.SqlServer cmd)
        {
            int result = 0;
            try
            {
                cmd.CommandText = "GLProcCalcUpdCA";
                cmd.AddParameter("@DT", System.Data.SqlDbType.DateTime, dtPeriod);
                cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.DbCommand.CommandTimeout = 0;

                cmd.Open();

                cmd.BeginTransaction();
                //result = cmd.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                result = 1;
                cmd.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageError = string.IsNullOrEmpty(ex.Message) ? ex.Message : "* Calculate and Update Control Account \n" + "    - " + ex.Message + "\n \n";
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();
                throw;
            }
            finally
            {
                cmd.Close();
            }
            
            return result;
        }

        private int PostingPL(DateTime mPeriod,string branch,DataAccess.SqlServer cmd)
        {
            int result = 0;
            string MDay = "";
            string MBln = "";
            string Mthn = "";
            string mMonth = "";
            bool i = false;
            
            DateTime mtdate, mDummy, mTrdate;

            MDay = Convert.ToString(mPeriod.Date.Day);
            MBln = Convert.ToString(mPeriod.Date.Month);
            Mthn = Convert.ToString(mPeriod.Date.Year);
            mMonth = Mthn + MBln;
            mDummy = mPeriod.AddMonths(1);
            mtdate = mPeriod.AddDays(Convert.ToDouble(DateDiff(mPeriod, mDummy) - mPeriod.Day));
            mTrdate = mPeriod;

            try
            {
                cmd.CommandText = "sp_PostPLTrans";
                cmd.AddParameter("@pPeriod", System.Data.SqlDbType.DateTime, mTrdate);
                cmd.AddParameter("@mTdate", System.Data.SqlDbType.DateTime, mtdate);
                cmd.AddParameter("@pBranch", System.Data.SqlDbType.VarChar, branch);
                cmd.AddParameter("@pError", System.Data.SqlDbType.VarChar, i);
                cmd.AddParameter("@pCurrUser", System.Data.SqlDbType.VarChar, OperatorID);
                cmd.AddParameter("@pCurrDate", System.Data.SqlDbType.DateTime, DateTime.Now);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.DbCommand.CommandTimeout = 0;

                cmd.Open();

                cmd.BeginTransaction();
                //result = cmd.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                result = 1;
                cmd.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageError = string.IsNullOrEmpty(ex.Message) ? ex.Message : "* Calculate and Post P/L Transactions \n" + "    - " + ex.Message + "\n \n";
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();
                throw;
            }
            finally
            {
                cmd.Close();
            }
            
            return result;
        }

        private int EndOfPeriod(DateTime FromPeriod,string branch, DataAccess.SqlServer cmd)
        {
            int result = 0;
            int errorGLProcEndOfPeriod = 0;
            string strPeriod = "";
            strPeriod = SetPeriod(FromPeriod);

            string strPeriodNext = "";
            DateTime dtPeriodNext;
            dtPeriodNext = FromPeriod.AddMonths(1);
            strPeriodNext = SetPeriod(dtPeriodNext.Date);

            try
            {
                cmd.CommandText = "GLProcEndOfPeriod";
                cmd.AddParameter("@pBranch", System.Data.SqlDbType.VarChar, branch);
                cmd.AddParameter("@mMonth", System.Data.SqlDbType.VarChar, strPeriod);
                cmd.AddParameter("@MNMonth", System.Data.SqlDbType.VarChar, strPeriodNext);
                cmd.AddParameter("@mOperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                cmd.AddParameter("@mLastUpDate", System.Data.SqlDbType.DateTime, DateTime.Now);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.DbCommand.CommandTimeout = 0;
                cmd.Open();

                cmd.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (Convert.ToSByte(dr["Code"]) == 1)
                            {
                                errorGLProcEndOfPeriod = Convert.ToSByte(dr["Code"]);
                            }
                        }
                        dr.Close();
                    }
                }

                // -- Sebelumnya Walaupun Error Tetap di jalankan tidak ada validasi
                //cmd.CommandText = "GLProcCrtPeriod";
                //cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, strPeriod);
                //cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Add - Roby - 20211012
                if (errorGLProcEndOfPeriod != 1)
                {
                    cmd.CommandText = "GLProcCrtPeriod";
                    cmd.AddParameter("@PERIOD", System.Data.SqlDbType.VarChar, strPeriod);
                    cmd.AddParameter("@pBranch", System.Data.SqlDbType.VarChar, branch);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.DbCommand.CommandTimeout = 0;

                    cmd.Open();

                    cmd.BeginTransaction();
                    //result = cmd.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    result = 1;
                    cmd.CommitTransaction();
                }
                //End Add - Roby - 20211012

            }
            catch (Exception ex)
            {
                MessageError = string.IsNullOrEmpty(ex.Message) ? ex.Message : "* End of Period \n" + "    - " + ex.Message + "\n \n";
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();
                throw;
            }
            finally
            {
                cmd.Close();
            }
            
            return result;
        }

        private int MonthlyClosing(DateTime TglMonthly,string branch, DataAccess.SqlServer cmd)
        {
            int result = 0;
            //string strInsertCounter = "";
            string strPeriod = "";

            try
            {
                using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                {
                    strPeriod = Convert.ToString(TglMonthly.Date.Year) + Right("0" + Convert.ToString(TglMonthly.Date.Month), 2);
                    db.CommandText = "select * from ACFMCTR  where period=@period";
                    db.AddParameter("@period", System.Data.SqlDbType.VarChar, strPeriod);
                    db.CommandType = System.Data.CommandType.Text;
                    cmd.DbCommand.CommandTimeout = 0;

                    db.Open();
                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (!dr.HasRows)
                        {
                            cmd.CommandText = "insert into ACFMCTR values (@period,@branch,1)";
                            cmd.AddParameter("@period", System.Data.SqlDbType.VarChar, strPeriod);
                            cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.DbCommand.CommandTimeout = 0;

                            cmd.Open();
                            cmd.BeginTransaction();
                            //result = cmd.ExecuteNonQuery();
                            cmd.ExecuteNonQuery();
                            result = 1;
                            cmd.CommitTransaction();
                        }
                        else
                        {
                            while (dr.Read())
                            {
                                cmd.CommandText = "UPDATE ACFMCTR SET Closing=@closing  WHERE Period=@period";
                                cmd.AddParameter("@period", System.Data.SqlDbType.VarChar, strPeriod);
                                cmd.AddParameter("@closing", System.Data.SqlDbType.TinyInt, Convert.ToBoolean(dr["Closing"]) == true ? 0 : 1);
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.DbCommand.CommandTimeout = 0;

                                cmd.Open();
                                cmd.BeginTransaction();
                                //result = cmd.ExecuteNonQuery();
                                cmd.ExecuteNonQuery();
                                result = 1;
                                cmd.CommitTransaction();
                            }
                            dr.Close();
                        }
                        
                    }

                    db.Close();
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
            
            return result;
        }

        private int BeginningYear(DateTime mPeriod, string branch, DataAccess.SqlServer cmd)
        {
            int result = 0;
            int i = 0;

            string msgError = "";

            do
            {
                switch (i)
                {
                    case 0:
                        msgError = CheckExistingData(cmd, "select CoNVERT(VARCHAR(6),StartFiscalYear,112) from SYSPAR where Code=1", 0);
                        break;
                    case 1:
                        msgError = CheckExistingData(cmd, "SELECT * FROM SYSPAR WHERE MONTH(StartFiscalYear) = MONTH('" + mPeriod + "')", i);
                        break;
                    case 2:
                        msgError = CheckExistingData(cmd, "SELECT * FROM ACFSPACC WHERE Type_Acc='BY'", i);
                        break;
                    case 3:
                        msgError = CheckExistingData(cmd, "Select ACC From ACFGLMH Where ACFGLMH.ACC Between (Select FR_ACC From ACFSPACC WHere Type_Acc='BY') And (Select To_ACC From ACFSPACC WHere Type_Acc='BY')", i);
                        break;
                    case 4:
                        msgError = CheckExistingData(cmd, "SELECT * FROM ACFGLMD WHERE ACC=(Select FR_ACC From ACFSPACC WHere Type_Acc='BY') AND CCY='IDR' " +
                " AND MN='" + SetPeriod(mPeriod, true) + "' AND BranchCode='" + branch + "'", i);
                        break;
                    case 5:
                        msgError = CheckExistingData(cmd, "select ID From ACFSPACC Where Fr_Acc='CE'", i);
                        break;
                    case 6:
                        msgError = CheckExistingData(cmd, "select ID From ACFSPACC Where Fr_Acc='RE'", i);
                        break;
                    case 7:
                        msgError = CheckExistingData(cmd, "select ID From ACFSPACC Where Fr_Acc='PL'", i);
                        break;
                }
                i++;
            } while (string.IsNullOrEmpty(msgError) && i < 8);

            if (string.IsNullOrEmpty(msgError))
            {
                try
                {
                    cmd.CommandText = "GLProcBegYear";
                    cmd.AddParameter("@mTransDate", System.Data.SqlDbType.DateTime, mPeriod);
                    cmd.AddParameter("@mProcDate", System.Data.SqlDbType.DateTime, DateTime.Now);
                    cmd.AddParameter("@mBranchCode", System.Data.SqlDbType.VarChar, branch);
                    cmd.AddParameter("@mUsr", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.DbCommand.CommandTimeout = 0;

                    cmd.Open();

                    cmd.BeginTransaction();
                    //result = cmd.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    result = 1;
                    cmd.CommitTransaction();
                }
                catch (Exception ex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();
                    throw;
                }
            }
            else
            {
                MessageError += "* Beginning of Year \n"+ msgError;
            }
            return result;
        }

        private string CheckExistingData(DataAccess.SqlServer cmd, string cmdText, int cmdTextIndex)
        {
            string result = "";

            cmd.CommandText = cmdText;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.DbCommand.CommandTimeout = 0;

            cmd.Open();
            cmd.ExecuteReader();

            using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
            {
                if (!dr.HasRows)
                {
                    switch (cmdTextIndex)
                    {
                        case 0:
                            result = result + "No Fiscal Year";
                            break;

                        case 1:
                            result = result + "Fiscal Year start in wrong month";
                            break;

                        case 2:
                            result = result + "No Expense Account Defined";
                            break;

                        case 3:
                            result = result + "No Expense Account Defined";
                            break;

                        case 4:
                            result = result + "No Monthly Balance With Expense Account in Special Account";
                            break;

                        case 5:
                            result = result + "No Current Earning Account Defined";
                            break;

                        case 6:
                            result = result + "No Retained Earning Account Defined";
                            break;

                        case 7:
                            result = result + "No Profit / Loss Account Defined";
                            break;
                    }
                }
            }

            cmd.Close();

            result = string.IsNullOrEmpty(result) ? result : "    - " + result + "\n";

            return result;
        }

        private string Right(string Text, int length)
        {
            string result = Text.Substring(Text.Length - length, length);
            return result;
        }

        private string SetPeriod(DateTime Date)
        {
            int mn = 0;
            int yr = 0;
            string period;
            mn = Convert.ToInt16(Date.Month);
            yr = Convert.ToInt16(Date.Year);
            if (mn < 10)
            {
                period = "0" + Convert.ToString(mn);
            }
            else
            {
                period = Convert.ToString(mn);
            }
            period = Convert.ToString(yr) + period;
            return period;
        }

        public string SetPeriod(DateTime Date, bool ReturnYear)
        {
            int mn = 0;

            string period;

            if (ReturnYear)
            {
                return SetPeriod(Date);
            }
            else
            {
                mn = Convert.ToInt16(Date.Month);

                if (mn < 10)
                {
                    period = "0" + Convert.ToString(mn);
                }
                else
                {
                    period = Convert.ToString(mn);
                }
                return period;
            }
        }

        private static Double DateDiff(DateTime dt1, DateTime dt2)
        {
            TimeSpan ts = dt2 - dt1;
            return Math.Round(ts.TotalDays);
        }
    }
}
