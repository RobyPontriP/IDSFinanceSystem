using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class FinancialRatio
    {
        [Display(Name = "Ratio Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ratio Code is required")]
        [MaxLength(10), StringLength(10)]
        public string RatioCode { get; set; }

        [Display(Name = "Ratio Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ratio Name is required")]
        [MaxLength(50), StringLength(50)]
        public string RatioName { get; set; }

        [Display(Name = "In Percent")]
        public bool InPercent { get; set; }

        [Display(Name = "Formula")]
        [MaxLength(255), StringLength(255)]
        public string Formula { get; set; }

        [Display(Name = "Dummy")]
        public decimal Dummy { get; set; }

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

        public FinancialRatio()
        {

        }

        public FinancialRatio(string ratioCode)
        {
            RatioCode = ratioCode;
        }

        public static List<FinancialRatio> GetFinancialRatio()
        {
            List<IDS.GLTable.FinancialRatio> list = new List<FinancialRatio>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelFinancialRatio";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@RatioCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            FinancialRatio finRatio = new FinancialRatio();
                            finRatio.RatioCode = dr["RatioCode"] as string;
                            finRatio.RatioName = dr["Name"] as string;
                            finRatio.InPercent = IDS.Tool.GeneralHelper.NullToBool(dr["InPercent"]);
                            finRatio.Formula = dr["Formula"] as string;
                            finRatio.Dummy = string.IsNullOrEmpty(dr["Dummy"].ToString()) ? 0 : Convert.ToDecimal(dr["Dummy"]);
                            finRatio.EntryUser = dr["EntryUser"] as string;
                            finRatio.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            finRatio.OperatorID = dr["OperatorID"] as string;
                            finRatio.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(finRatio);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static FinancialRatio GetFinancialRatio(string finRatioCode)
        {
            FinancialRatio finRatio = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelFinancialRatio";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@RatioCode", System.Data.SqlDbType.VarChar, finRatioCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        finRatio = new FinancialRatio();
                        finRatio.RatioCode = dr["RatioCode"] as string;
                        finRatio.RatioName = dr["Name"] as string;
                        finRatio.InPercent = IDS.Tool.GeneralHelper.NullToBool(dr["InPercent"]);
                        finRatio.Formula = dr["Formula"] as string;
                        finRatio.Dummy = string.IsNullOrEmpty(dr["Dummy"].ToString()) ? 0 : Convert.ToDecimal(dr["Dummy"]);
                        finRatio.EntryUser = dr["EntryUser"] as string;
                        finRatio.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        finRatio.OperatorID = dr["OperatorID"] as string;
                        finRatio.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return finRatio;
        }

        public static List<System.Web.Mvc.SelectListItem> GetFinancialRatioForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> ratioCodes = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelFinancialRatio";
                db.AddParameter("@RatioCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@Name", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@InPercent", System.Data.SqlDbType.Bit, DBNull.Value);
                //db.AddParameter("@Formula", System.Data.SqlDbType.VarChar, DBNull.Value);
                //db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        ratioCodes = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem ratioCode = new System.Web.Mvc.SelectListItem();
                            ratioCode.Value = dr["RatioCode"] as string;
                            ratioCode.Text = dr["RatioCode"] as string;

                            ratioCodes.Add(ratioCode);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return ratioCodes;
        }

        public int InsUpDelFinancialRatio(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLUpdateFinancialRatio";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.Int, ExecCode);
                    cmd.AddParameter("@RatioCode", System.Data.SqlDbType.VarChar, RatioCode);
                    cmd.AddParameter("@Name", System.Data.SqlDbType.VarChar, RatioName);
                    cmd.AddParameter("@InPercent", System.Data.SqlDbType.Bit, InPercent);
                    cmd.AddParameter("@Formula", System.Data.SqlDbType.VarChar, Formula);
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
                            throw new Exception("Financial Ratio Code is already exists. Please choose other Financial Ratio.");
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

        public int DeleteFinancialRatios(string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLUpdateFinancialRatio";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.AddParameter("@Type", System.Data.SqlDbType.Int, IDS.Tool.PageActivity.Delete);
                        cmd.AddParameter("@RatioCode", System.Data.SqlDbType.VarChar, data[i]);
                        cmd.AddParameter("@Name", System.Data.SqlDbType.VarChar, DBNull.Value);
                        cmd.AddParameter("@InPercent", System.Data.SqlDbType.Bit, DBNull.Value);
                        cmd.AddParameter("@Formula", System.Data.SqlDbType.VarChar, DBNull.Value);
                        cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                        
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
                            throw new Exception("Country code is already exists. Please choose other country code.");
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

        public static void UpdateRatioTemp(string periodFrom,string periodTo, string branchCode)
        {
            int readYear = Convert.ToInt16(periodTo.Substring(0,4)) - Convert.ToInt16(periodFrom.Substring(0,4));
            double[] val1 = new double[5];
            double[] val2 = new double[5];

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLRepRatioHasil";
                db.AddParameter("@vcPeriod1", System.Data.SqlDbType.VarChar, periodFrom+ "12");
                db.AddParameter("@vcPeriod2", System.Data.SqlDbType.VarChar, periodTo + "12");
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar,branchCode);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (readYear >= 0)
                            {
                                val1[0] = EvalFormula(Convert.ToString(dr["FM1_YEAR1"]));
                                val2[0] = EvalFormula(Convert.ToString(dr["FM2_YEAR1"]));
                            }

                            if (readYear >= 1)
                            {
                                val1[1] = EvalFormula(Convert.ToString(dr["FM1_YEAR2"]));
                                val2[1] = EvalFormula(Convert.ToString(dr["FM2_YEAR2"]));
                            }

                            if (readYear >= 2)
                            {
                                val1[2] = EvalFormula(Convert.ToString(dr["FM1_YEAR3"]));
                                val2[2] = EvalFormula(Convert.ToString(dr["FM2_YEAR3"]));
                            }

                            if (readYear >= 3)
                            {
                                val1[3] = EvalFormula(Convert.ToString(dr["FM1_YEAR4"]));
                                val2[3] = EvalFormula(Convert.ToString(dr["FM2_YEAR4"]));
                            }

                            if (readYear >= 4)
                            {
                                val1[4] = EvalFormula(Convert.ToString(dr["FM1_YEAR5"]));
                                val2[4] = EvalFormula(Convert.ToString(dr["FM2_YEAR5"]));
                            }

                            string strRatioCode = Convert.ToString(dr["RatioCode"]);
                            string strRatioName = Convert.ToString(dr["Name"]);
                            bool bPer = Convert.ToBoolean(dr["InPercent"]);
                            double dblA = 0;
                            double dblB = 0;
                            double mHasil = 0;
                            int iPer = 0;
                            int PeriodDummy = Convert.ToInt16(periodFrom);
                            if (bPer)
                            {
                                iPer = 1;
                            }

                            using (IDS.DataAccess.SqlServer dbDel = new DataAccess.SqlServer())
                            {
                                dbDel.CommandText = "Delete from tblfinancialRatioTemp";
                                dbDel.CommandType = System.Data.CommandType.Text;
                                dbDel.Open();
                                dbDel.BeginTransaction();
                                dbDel.ExecuteNonQuery();
                                dbDel.CommitTransaction();
                                dbDel.Close();
                            }

                            if (readYear >= 0)
                            {
                                PeriodDummy = Convert.ToInt16(periodFrom.Trim());
                                string strYear = Convert.ToString(PeriodDummy);
                                dblA = 0;
                                dblB = 0;
                                dblA = val1[0];
                                dblB = val2[0];

                                mHasil = 0;
                                if (dblA != 0 && dblB != 0)
                                {
                                    mHasil = dblA / dblB;

                                    DelInsRatioTemp(strRatioCode, strRatioName, mHasil, iPer, strYear);
                                }
                            }

                            if (readYear >= 1)
                            {
                                PeriodDummy++;
                                string strYear = Convert.ToString(PeriodDummy);
                                dblA = 0;
                                dblB = 0;
                                dblA = val1[1];
                                dblB = val2[1];

                                mHasil = 0;
                                if (dblA != 0 && dblB != 0)
                                {
                                    mHasil = dblA / dblB;

                                    DelInsRatioTemp(strRatioCode, strRatioName, mHasil, iPer, strYear);
                                }
                            }

                            if (readYear >= 2)
                            {
                                PeriodDummy++;
                                string strYear = Convert.ToString(PeriodDummy);
                                dblA = 0;
                                dblB = 0;
                                dblA = val1[2];
                                dblB = val2[2];

                                mHasil = 0;
                                if (dblA != 0 && dblB != 0)
                                {
                                    mHasil = dblA / dblB;

                                    DelInsRatioTemp(strRatioCode, strRatioName, mHasil, iPer, strYear);
                                }
                            }

                            if (readYear >= 3)
                            {
                                PeriodDummy++;
                                string strYear = Convert.ToString(PeriodDummy);
                                dblA = 0;
                                dblB = 0;
                                dblA = val1[3];
                                dblB = val2[3];

                                mHasil = 0;
                                if (dblA != 0 && dblB != 0)
                                {
                                    mHasil = dblA / dblB;

                                    DelInsRatioTemp(strRatioCode, strRatioName, mHasil, iPer, strYear);
                                }
                            }

                            if (readYear >= 4)
                            {
                                PeriodDummy++;
                                string strYear = Convert.ToString(PeriodDummy);
                                dblA = 0;
                                dblB = 0;
                                dblA = val1[4];
                                dblB = val2[4];

                                mHasil = 0;
                                if (dblA != 0 && dblB != 0)
                                {
                                    mHasil = dblA / dblB;

                                    DelInsRatioTemp(strRatioCode, strRatioName, mHasil, iPer, strYear);
                                }
                            }
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
        }

        private static double EvalFormula(string strFormula)
        {
            double dblRes = 0;
            if (strFormula.Trim() != "")
            {
                using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                {
                    db.CommandText = "SELECT @Formula";
                    db.AddParameter("@Formula", System.Data.SqlDbType.VarChar, strFormula);
                    db.CommandType = System.Data.CommandType.Text;
                    db.Open();
                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                dblRes = Convert.ToDouble(dr[0]);
                            }
                        }

                        if (!dr.IsClosed)
                            dr.Close();
                    }
                }
            }
            return dblRes;
        }

        private static void DelInsRatioTemp(string ratioCode,string ratioName,double mhasil,int iper,string stryaer)
        {
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "Insert INTO tblFinancialRatioTemp (RatioCode, Name, Result, INPercent, YearRatio) VALUES (@RatioCode,@RatioName,@mHasil,@iPer,@Year)";
                db.AddParameter("@RatioCode", System.Data.SqlDbType.VarChar, ratioCode);
                db.AddParameter("@RatioName", System.Data.SqlDbType.VarChar, ratioName);
                db.AddParameter("@mHasil", System.Data.SqlDbType.Decimal, mhasil);
                db.AddParameter("@iPer", System.Data.SqlDbType.Int, iper);
                db.AddParameter("@Year", System.Data.SqlDbType.VarChar, stryaer);
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.BeginTransaction();
                db.ExecuteNonQuery();
                db.CommitTransaction();
            }
        }

        public string Validate(string strFormula, long lngFormulaLen)
        {
            string msgError = "";
            char[] arrFormula;
            int iMaxLen;
            int x = 0;
            int iOp = 0;
            int iCl = 0;
            int iDivider = 0;
            string strOperand = "";
            bool bolSucceed = false;
            //int i, op = 0, cl = 0, j = 0;
            iMaxLen = Convert.ToInt16(lngFormulaLen);
            arrFormula = strFormula.ToCharArray(0, iMaxLen);

            try
            {
                if (iMaxLen >= 3)
                {
                    strOperand = Convert.ToString(arrFormula[0]) + Convert.ToString(arrFormula[1]) + Convert.ToString(arrFormula[2]);
                }

                for (x = 0; x < lngFormulaLen; x++)
                {
                    if (Convert.ToString(arrFormula[x]) == "(")
                    {
                        iOp++;
                    }

                    if (Convert.ToString(arrFormula[x]) == ")")
                    {
                        iCl++;
                    }

                    if (x == 0)
                    {
                        if (arrFormula[0] == '+' || arrFormula[0] == '-' || arrFormula[0] == '/' || arrFormula[0] == '*'
                            || arrFormula[0] == ')' || strOperand == "#/#")
                        {
                            msgError ="Invalid operator placement";
                            return msgError;
                        }
                    }
                    else
                    {
                        if ((iMaxLen - x) >= 3)
                        {
                            strOperand = Convert.ToString(arrFormula[x]) + Convert.ToString(arrFormula[x + 1]) + Convert.ToString(arrFormula[x + 2]);
                            //if (strOperand == "#/#")
                            if (strOperand == "#/#" || strOperand == "}+{" || strOperand == "}-{" || strOperand == "}*{" || strOperand == "}/{")
                            {
                                iDivider++;
                            }
                        }

                        if (arrFormula[x - 1] == '+' || arrFormula[x - 1] == '-' || arrFormula[x - 1] == '/' || arrFormula[x - 1] == '*' || arrFormula[x - 1] == '(')
                        {
                            if (arrFormula[x] == '+' || arrFormula[x] == '-' || arrFormula[x] == '/' || arrFormula[x] == '*'
                                || arrFormula[x] == ')')
                            {
                                if (arrFormula[x - 1] == '(')
                                {
                                    msgError = "Cannot place operator after opened bracket";
                                    return msgError;
                                }
                                else
                                {
                                    msgError = "Invalid Placement, Double operator";
                                    return msgError;
                                }
                                //return false;
                            }
                            else if (strOperand == "#/#")
                            {
                                if (arrFormula[x - 1] == '(')
                                {
                                    msgError = "Invalid divider after opened bracket";
                                    return msgError;
                                }
                                else
                                {
                                    msgError = "Invalid Placement, Cannot Place divider after operator";
                                    return msgError;
                                }
                                //return false;
                            }
                        }

                        if (Convert.ToString(arrFormula[x - 1]) == "{" || Convert.ToString(arrFormula[x - 1]) == "}")
                        {
                            if (Convert.ToString(arrFormula[x]) == "{" || Convert.ToString(arrFormula[x]) == "}")
                            {
                                msgError = "Invalid brace bracket placement";
                                return msgError;
                            }
                        }



                        if (x == (lngFormulaLen - 1))
                        {
                            if (iOp - iCl != 0)
                            {
                                msgError = "Bracket has no pair";
                                return msgError;
                            }

                            if (iMaxLen >= 3)
                            {
                                strOperand = Convert.ToString(arrFormula[x - 3]) + Convert.ToString(arrFormula[x - 2]) + Convert.ToString(arrFormula[x - 1]);
                            }

                            if (arrFormula[iMaxLen - 1] == '+' || arrFormula[iMaxLen - 1] == '-' ||
                                arrFormula[iMaxLen - 1] == '/' || arrFormula[iMaxLen - 1] == '*' || strOperand == "#/#")
                            {
                                msgError = "Invalid Placement, Operator at the end";
                                return msgError;
                            }
                            else if (arrFormula[iMaxLen - 1] == '{' || arrFormula[iMaxLen - 1] == '(')
                            {
                                msgError = "Invalid Opened Brace Placement at The End";
                                return msgError;
                            }

                            if (iDivider == 0)
                            {
                                msgError = "No Account Divider Detected";
                                return msgError;
                            }
                            else if (iDivider != 1)
                            {
                                msgError = "Multiple Divider is not allowed";
                                return msgError;
                            }
                        }
                    }
                }
            }
            catch (Exception exxx)
            {
                msgError = Convert.ToString(exxx);
                return msgError;
            }

            msgError = string.IsNullOrEmpty(msgError) ? "Success" : msgError;

            return msgError;
        }
    }
}
