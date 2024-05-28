using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class ReportGenerator
    {
        [Display(Name = "Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        [MaxLength(2), StringLength(2)]
        public string Code { get; set; }

        [Display(Name = "Line")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Line is required")]
        public int Line { get; set; }

        [Display(Name = "Department")]
        public string DepartmentACFGEN { get; set; }

        [Display(Name = "Account")]
        public string AccACFGEN { get; set; }

        [Display(Name = "Currency")]
        public string CurrencyACFGEN { get; set; }

        [Display(Name = "Description")]
        [MaxLength(100)]
        public string Description { get; set; }

        [Display(Name = "Print Status")]
        public bool PrintStatus { get; set; }

        [Display(Name = "From ACC")]
        public int FromACC { get; set; }

        [Display(Name = "C1")]
        [MaxLength(1)]
        public string C1 { get; set; }

        [Display(Name = "C2")]
        [MaxLength(1)]
        public string C2 { get; set; }

        [Display(Name = "C3")]
        [MaxLength(1)]
        public string C3 { get; set; }

        [Display(Name = "C4")]
        [MaxLength(1)]
        public string C4 { get; set; }

        [Display(Name = "C5")]
        [MaxLength(1)]
        public string C5 { get; set; }

        [Display(Name = "C6")]
        [MaxLength(1)]
        public string C6 { get; set; }

        [Display(Name = "C7")]
        [MaxLength(1)]
        public string C7 { get; set; }

        [Display(Name = "Multiply")]
        public int Multiply { get; set; }

        [Display(Name = "Divide")]
        public int Divide { get; set; }

        [Display(Name = "Skip")]
        public int Skip { get; set; }

        [Display(Name = "Debit Credit")]
        public int DebitCredit { get; set; }

        [Display(Name = "Is Right")]
        public bool IsRight { get; set; }

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

        public ReportGenerator()
        {

        }

        public ReportGenerator(string code,int line)
        {
            Code = code;
            Line = line;
        }

        public int InsUpDelRepGen(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLRepSaveGEN";
                    cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@code", System.Data.SqlDbType.VarChar, Code);
                    cmd.AddParameter("@line", System.Data.SqlDbType.Int, Line);
                    cmd.AddParameter("@dept", System.Data.SqlDbType.VarChar, DepartmentACFGEN);
                    cmd.AddParameter("@Acc", System.Data.SqlDbType.VarChar, AccACFGEN);
                    cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CurrencyACFGEN);
                    cmd.AddParameter("@descrip", System.Data.SqlDbType.VarChar, Description);
                    cmd.AddParameter("@Print", System.Data.SqlDbType.Bit, PrintStatus);
                    cmd.AddParameter("@from_acc", System.Data.SqlDbType.TinyInt, FromACC);
                    cmd.AddParameter("@txt1", System.Data.SqlDbType.VarChar, C1);
                    cmd.AddParameter("@txt2", System.Data.SqlDbType.VarChar, C2);
                    cmd.AddParameter("@txt3", System.Data.SqlDbType.VarChar, C3);
                    cmd.AddParameter("@txt4", System.Data.SqlDbType.VarChar, C4);
                    cmd.AddParameter("@txt5", System.Data.SqlDbType.VarChar, C5);
                    cmd.AddParameter("@txt6", System.Data.SqlDbType.VarChar, C6);
                    cmd.AddParameter("@txt7", System.Data.SqlDbType.VarChar, C7);
                    cmd.AddParameter("@multi", System.Data.SqlDbType.TinyInt, Multiply);
                    cmd.AddParameter("@divide", System.Data.SqlDbType.TinyInt, Divide);
                    cmd.AddParameter("@skip", System.Data.SqlDbType.TinyInt, Skip);
                    cmd.AddParameter("@DbtCrd", System.Data.SqlDbType.TinyInt, DebitCredit);
                    cmd.AddParameter("@isRight", System.Data.SqlDbType.Bit, IsRight);
                    cmd.AddParameter("@LogUser", System.Data.SqlDbType.VarChar, OperatorID);
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
                            throw new Exception("Report Generator Code is already exists. Please choose other Report Generator Code.");
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

        //public int InsUpDelRepGen(int ExecCode, string code, string line)
        //{
        //    int result = 0;

        //    using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
        //    {
        //        try
        //        {
        //            cmd.CommandText = "GLRepSaveGEN";
        //            cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, ExecCode);
        //            cmd.AddParameter("@code", System.Data.SqlDbType.VarChar, code);
        //            cmd.AddParameter("@line", System.Data.SqlDbType.Int, line);
        //            cmd.AddParameter("@dept", System.Data.SqlDbType.VarChar, DepartmentACFGEN.DepartmentCode);
        //            cmd.AddParameter("@Acc", System.Data.SqlDbType.VarChar, AccACFGEN.Account);
        //            cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CurrencyACFGEN.CurrencyCode);
        //            cmd.AddParameter("@descrip", System.Data.SqlDbType.VarChar, Description);
        //            cmd.AddParameter("@Print", System.Data.SqlDbType.Bit, PrintStatus);
        //            cmd.AddParameter("@from_acc", System.Data.SqlDbType.TinyInt, FromACC);
        //            cmd.AddParameter("@txt1", System.Data.SqlDbType.VarChar, C1);
        //            cmd.AddParameter("@txt2", System.Data.SqlDbType.VarChar, C2);
        //            cmd.AddParameter("@txt3", System.Data.SqlDbType.VarChar, C3);
        //            cmd.AddParameter("@txt4", System.Data.SqlDbType.VarChar, C4);
        //            cmd.AddParameter("@txt5", System.Data.SqlDbType.VarChar, C5);
        //            cmd.AddParameter("@txt6", System.Data.SqlDbType.VarChar, C6);
        //            cmd.AddParameter("@txt7", System.Data.SqlDbType.VarChar, C7);
        //            cmd.AddParameter("@multi", System.Data.SqlDbType.TinyInt, Multiply);
        //            cmd.AddParameter("@divide", System.Data.SqlDbType.TinyInt, Divide);
        //            cmd.AddParameter("@skip", System.Data.SqlDbType.TinyInt, Skip);
        //            cmd.AddParameter("@DbtCrd", System.Data.SqlDbType.TinyInt, DebitCredit);
        //            cmd.AddParameter("@LogUser", System.Data.SqlDbType.VarChar, OperatorID);
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.Open();

        //            cmd.BeginTransaction();
        //            result = cmd.ExecuteNonQuery();
        //            cmd.CommitTransaction();
        //        }
        //        catch (System.Data.SqlClient.SqlException sex)
        //        {
        //            if (cmd.Transaction != null)
        //                cmd.RollbackTransaction();

        //            switch (sex.Number)
        //            {
        //                case 2627:
        //                    throw new Exception("Report Generator Code is already exists. Please choose other Report Generator Code.");
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

        public int InsUpDelRepGen(int ExecCode, string[] dataCode, string[] dataLine)
        {
            int result = 0;

            if (dataCode == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLRepSaveGEN";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < dataCode.Length; i++)
                    {
                        cmd.CommandText = "GLRepSaveGEN";
                        cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@code", System.Data.SqlDbType.VarChar, dataCode[i]);
                        cmd.AddParameter("@line", System.Data.SqlDbType.VarChar, dataLine[i]);
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
                            throw new Exception("Report Generator Code is already exists. Please choose other Bank Code.");
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

        public static List<ReportGenerator> GetRepGen()
        {
            List<IDS.GLTable.ReportGenerator> list = new List<ReportGenerator>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGEN";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ReportGenerator RepGen = new ReportGenerator();
                            RepGen.Code = dr["Code"] as string;
                            RepGen.Line = IDS.Tool.GeneralHelper.NullToInt(dr["Line"],0);
                            RepGen.DepartmentACFGEN = IDS.Tool.GeneralHelper.NullToString(dr["Dept"]);
                            
                            RepGen.AccACFGEN = IDS.Tool.GeneralHelper.NullToString(dr["Acc"]);
                            
                            RepGen.CurrencyACFGEN = IDS.Tool.GeneralHelper.NullToString(dr["Ccy"]);

                            RepGen.Description = IDS.Tool.GeneralHelper.NullToString(dr["Descrip"]);
                            RepGen.PrintStatus = IDS.Tool.GeneralHelper.NullToBool(dr["PrintStatus"]);
                            RepGen.FromACC = IDS.Tool.GeneralHelper.NullToInt(dr["FromACC"], 0);
                            RepGen.C1 = IDS.Tool.GeneralHelper.NullToString(dr["C1"]);
                            RepGen.C2 = IDS.Tool.GeneralHelper.NullToString(dr["C2"]);
                            RepGen.C3 = IDS.Tool.GeneralHelper.NullToString(dr["C3"]);
                            RepGen.C4 = IDS.Tool.GeneralHelper.NullToString(dr["C4"]);
                            RepGen.C5 = IDS.Tool.GeneralHelper.NullToString(dr["C5"]);
                            RepGen.C6 = IDS.Tool.GeneralHelper.NullToString(dr["C6"]);
                            RepGen.C7 = IDS.Tool.GeneralHelper.NullToString(dr["C7"]);
                            RepGen.Multiply = IDS.Tool.GeneralHelper.NullToInt(dr["Multiply"], 0);
                            RepGen.Divide = IDS.Tool.GeneralHelper.NullToInt(dr["Divide"], 0);
                            RepGen.Skip = IDS.Tool.GeneralHelper.NullToInt(dr["Skip"], 0);
                            RepGen.DebitCredit = IDS.Tool.GeneralHelper.NullToInt(dr["DbtCrd"], 0);
                            RepGen.IsRight = IDS.Tool.GeneralHelper.NullToBool(dr["IsRight"]);
                            RepGen.EntryUser = dr["EntryUser"] as string;
                            RepGen.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            RepGen.OperatorID = dr["OperatorID"] as string;
                            RepGen.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(RepGen);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<ReportGenerator> GetRepGen(string repGenCode)
        {
            List<IDS.GLTable.ReportGenerator> list = new List<ReportGenerator>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGEN";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, repGenCode);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ReportGenerator RepGen = new ReportGenerator();
                            RepGen.Code = dr["Code"] as string;
                            RepGen.Line = IDS.Tool.GeneralHelper.NullToInt(dr["Line"], 0);
                            RepGen.DepartmentACFGEN = IDS.Tool.GeneralHelper.NullToString(dr["Dept"]);
                            
                            RepGen.AccACFGEN = IDS.Tool.GeneralHelper.NullToString(dr["Acc"]);
                            
                            RepGen.CurrencyACFGEN = IDS.Tool.GeneralHelper.NullToString(dr["Ccy"]);

                            RepGen.Description = IDS.Tool.GeneralHelper.NullToString(dr["Descrip"]);
                            RepGen.PrintStatus = IDS.Tool.GeneralHelper.NullToBool(dr["PrintStatus"]);
                            RepGen.FromACC = IDS.Tool.GeneralHelper.NullToInt(dr["FromACC"], 0);
                            RepGen.C1 = IDS.Tool.GeneralHelper.NullToString(dr["C1"]);
                            RepGen.C2 = IDS.Tool.GeneralHelper.NullToString(dr["C2"]);
                            RepGen.C3 = IDS.Tool.GeneralHelper.NullToString(dr["C3"]);
                            RepGen.C4 = IDS.Tool.GeneralHelper.NullToString(dr["C4"]);
                            RepGen.C5 = IDS.Tool.GeneralHelper.NullToString(dr["C5"]);
                            RepGen.C6 = IDS.Tool.GeneralHelper.NullToString(dr["C6"]);
                            RepGen.C7 = IDS.Tool.GeneralHelper.NullToString(dr["C7"]);
                            RepGen.Multiply = IDS.Tool.GeneralHelper.NullToInt(dr["Multiply"], 0);
                            RepGen.Divide = IDS.Tool.GeneralHelper.NullToInt(dr["Divide"], 0);
                            RepGen.Skip = IDS.Tool.GeneralHelper.NullToInt(dr["Skip"], 0);
                            RepGen.DebitCredit = IDS.Tool.GeneralHelper.NullToInt(dr["DbtCrd"], 0);
                            RepGen.IsRight = IDS.Tool.GeneralHelper.NullToBool(dr["IsRight"]);
                            RepGen.EntryUser = dr["EntryUser"] as string;
                            RepGen.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            RepGen.OperatorID = dr["OperatorID"] as string;
                            RepGen.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(RepGen);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static ReportGenerator GetReportGen(string repGenCode, string repGenLine)
        {
            ReportGenerator RepGen = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGEN";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, repGenCode);
                db.AddParameter("@line", System.Data.SqlDbType.Int, IDS.Tool.GeneralHelper.NullToInt(repGenLine, 0));
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 4);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        RepGen = new ReportGenerator();
                        RepGen.Code = dr["Code"] as string;
                        RepGen.Line = IDS.Tool.GeneralHelper.NullToInt(dr["Line"], 0);
                        RepGen.DepartmentACFGEN = IDS.Tool.GeneralHelper.NullToString(dr["Dept"]);
                        
                        RepGen.AccACFGEN = IDS.Tool.GeneralHelper.NullToString(dr["Acc"]);
                        
                        RepGen.CurrencyACFGEN = IDS.Tool.GeneralHelper.NullToString(dr["Ccy"]);

                        RepGen.Description = IDS.Tool.GeneralHelper.NullToString(dr["Descrip"]);
                        RepGen.PrintStatus = IDS.Tool.GeneralHelper.NullToBool(dr["PrintStatus"]);
                        RepGen.FromACC = IDS.Tool.GeneralHelper.NullToInt(dr["FromACC"], 0);
                        RepGen.C1 = IDS.Tool.GeneralHelper.NullToString(dr["C1"]);
                        RepGen.C2 = IDS.Tool.GeneralHelper.NullToString(dr["C2"]);
                        RepGen.C3 = IDS.Tool.GeneralHelper.NullToString(dr["C3"]);
                        RepGen.C4 = IDS.Tool.GeneralHelper.NullToString(dr["C4"]);
                        RepGen.C5 = IDS.Tool.GeneralHelper.NullToString(dr["C5"]);
                        RepGen.C6 = IDS.Tool.GeneralHelper.NullToString(dr["C6"]);
                        RepGen.C7 = IDS.Tool.GeneralHelper.NullToString(dr["C7"]);
                        RepGen.Multiply = IDS.Tool.GeneralHelper.NullToInt(dr["Multiply"], 0);
                        RepGen.Divide = IDS.Tool.GeneralHelper.NullToInt(dr["Divide"], 0);
                        RepGen.Skip = IDS.Tool.GeneralHelper.NullToInt(dr["Skip"], 0);
                        RepGen.DebitCredit = IDS.Tool.GeneralHelper.NullToInt(dr["DbtCrd"], 0);
                        RepGen.IsRight = IDS.Tool.GeneralHelper.NullToBool(dr["IsRight"]);
                        RepGen.EntryUser = dr["EntryUser"] as string;
                        RepGen.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        RepGen.OperatorID = dr["OperatorID"] as string;
                        RepGen.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return RepGen;
        }
        //public static List<ReportGenerator> GetRepGen(string repGenCode, string repGenLine)
        //{
        //    List<IDS.GLTable.ReportGenerator> list = new List<ReportGenerator>();

        //    using (DataAccess.SqlServer db = new DataAccess.SqlServer())
        //    {
        //        db.CommandText = "GLSelACFGEN";
        //        db.CommandType = System.Data.CommandType.StoredProcedure;
        //        db.AddParameter("@code", System.Data.SqlDbType.VarChar, repGenCode);
        //        db.AddParameter("@line", System.Data.SqlDbType.Int, IDS.Tool.GeneralHelper.NullToInt(repGenLine,0));
        //        db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 3);
        //        db.Open();

        //        db.ExecuteReader();

        //        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
        //        {
        //            if (dr.HasRows)
        //            {
        //                while (dr.Read())
        //                {
        //                    ReportGenerator RepGen = new ReportGenerator();
        //                    RepGen.Code = dr["Code"] as string;
        //                    RepGen.Line = IDS.Tool.GeneralHelper.NullToInt(dr["Line"], 0);
        //                    RepGen.DepartmentACFGEN = new IDS.GeneralTable.Department();
        //                    RepGen.DepartmentACFGEN.DepartmentCode = IDS.Tool.GeneralHelper.NullToString(dr["Dept"]);

        //                    RepGen.AccACFGEN = new IDS.GLTable.ChartOfAccount();
        //                    RepGen.AccACFGEN.Account = IDS.Tool.GeneralHelper.NullToString(dr["Acc"]);

        //                    RepGen.CurrencyACFGEN = new IDS.GeneralTable.Currency();
        //                    RepGen.CurrencyACFGEN.CurrencyCode = IDS.Tool.GeneralHelper.NullToString(dr["Ccy"]);

        //                    RepGen.Description = IDS.Tool.GeneralHelper.NullToString(dr["Descrip"]);
        //                    RepGen.PrintStatus = IDS.Tool.GeneralHelper.NullToBool(dr["PrintStatus"]);
        //                    RepGen.FromACC = IDS.Tool.GeneralHelper.NullToInt(dr["FromACC"], 0);
        //                    RepGen.C1 = IDS.Tool.GeneralHelper.NullToString(dr["C1"]);
        //                    RepGen.C2 = IDS.Tool.GeneralHelper.NullToString(dr["C2"]);
        //                    RepGen.C3 = IDS.Tool.GeneralHelper.NullToString(dr["C3"]);
        //                    RepGen.C4 = IDS.Tool.GeneralHelper.NullToString(dr["C4"]);
        //                    RepGen.C5 = IDS.Tool.GeneralHelper.NullToString(dr["C5"]);
        //                    RepGen.C6 = IDS.Tool.GeneralHelper.NullToString(dr["C6"]);
        //                    RepGen.C7 = IDS.Tool.GeneralHelper.NullToString(dr["C7"]);
        //                    RepGen.Multiply = IDS.Tool.GeneralHelper.NullToInt(dr["Multiply"], 0);
        //                    RepGen.Divide = IDS.Tool.GeneralHelper.NullToInt(dr["Divide"], 0);
        //                    RepGen.Skip = IDS.Tool.GeneralHelper.NullToInt(dr["Skip"], 0);
        //                    RepGen.DebitCredit = IDS.Tool.GeneralHelper.NullToInt(dr["DbtCrd"], 0);
        //                    RepGen.IsRigh = IDS.Tool.GeneralHelper.NullToBool(dr["IsRight"]);
        //                    RepGen.EntryUser = dr["EntryUser"] as string;
        //                    RepGen.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
        //                    RepGen.OperatorID = dr["OperatorID"] as string;
        //                    RepGen.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

        //                    list.Add(RepGen);
        //                }
        //            }

        //            if (!dr.IsClosed)
        //                dr.Close();
        //        }

        //        db.Close();
        //    }

        //    return list;
        //}

        public static List<System.Web.Mvc.SelectListItem> GetCodeForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> codes = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGEN";
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        codes = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem code = new System.Web.Mvc.SelectListItem();
                            code.Value = dr["Code"] as string;
                            code.Text = dr["Code"] as string;

                            codes.Add(code);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return codes;
        }

        public static List<System.Web.Mvc.SelectListItem> GetDbtCrdForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> DbtCrd = new List<System.Web.Mvc.SelectListItem>();
            DbtCrd.Add(new System.Web.Mvc.SelectListItem() { Text = "Debit (+)", Value = "0" });
            DbtCrd.Add(new System.Web.Mvc.SelectListItem() { Text = "Credit (-)", Value = "1" });

            return DbtCrd;
        }

        public static List<System.Web.Mvc.SelectListItem> GetFromAccForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> fromAcc = new List<System.Web.Mvc.SelectListItem>();
            fromAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "None", Value = "0" });
            fromAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "Register-1", Value = "1" });
            fromAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "Register-2", Value = "2" });
            fromAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "Register-3", Value = "3" });
            fromAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "Register-4", Value = "4" });
            fromAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "Register-5", Value = "5" });
            fromAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "Register-6", Value = "6" });
            fromAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "Register-7", Value = "7" });
            fromAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "File / Table", Value = "8" });

            return fromAcc;
        }

        public static List<System.Web.Mvc.SelectListItem> GetColForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> Col = new List<System.Web.Mvc.SelectListItem>();
            Col.Add(new System.Web.Mvc.SelectListItem() { Text = "+", Value = "+" });
            Col.Add(new System.Web.Mvc.SelectListItem() { Text = "-", Value = "-" });
            Col.Add(new System.Web.Mvc.SelectListItem() { Text = "0", Value = "0" });

            return Col;
        }
    }
}
