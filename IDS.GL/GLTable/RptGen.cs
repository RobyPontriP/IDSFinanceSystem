using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class RptGen
    {
        [Display(Name = "Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        [MaxLength(2), StringLength(2)]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "From Table")]
        public string FromTable { get; set; }

        [Display(Name = "Copy From Table")]
        public string CopyFromTable { get; set; }

        [Display(Name = "Report Type")]
        public string RptType { get; set; }

        [Display(Name = "Level")]
        public string Level { get; set; }

        [Display(Name = "Dept")]
        public int Dept { get; set; }

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

        public RptGen()
        {

        }

        public RptGen(string code)
        {
            Code = code;
        }

        public int InsUpDelRptGen(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTRptGen";
                    cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, Code);
                    cmd.AddParameter("@Level", System.Data.SqlDbType.Int, IDS.Tool.GeneralHelper.NullToInt(Level,0));
                    cmd.AddParameter("@Dept", System.Data.SqlDbType.TinyInt, IDS.Tool.GeneralHelper.NullToInt(FromTable,2));
                    cmd.AddParameter("@CT", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(CopyFromTable));
                    cmd.AddParameter("@OP", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.AddParameter("@pHasilProses", System.Data.SqlDbType.Int, 1);
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
                            throw new Exception("Report Generator Code is already exists. Please choose other Report Generator.");
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

        public int InsUpDelRptGen(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTRptGen";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GLSelACFGEN";
                        cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@code", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Report Generator Code is already exists. Please choose other Report Generator Code.");
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

        public static List<RptGen> GetRptGen()
        {
            List<IDS.GLTable.RptGen> list = new List<RptGen>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGEN";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 5);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RptGen RepGen = new RptGen();
                            RepGen.Code = dr["Code"] as string;
                            RepGen.Name = dr["codedesc"] as string;
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

        public static List<System.Web.Mvc.SelectListItem> GetRptGenForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> rptGens = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGEN";
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 5);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        rptGens = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem rptGen = new System.Web.Mvc.SelectListItem();
                            rptGen.Value = dr["code"] as string;
                            rptGen.Text = dr["codedesc"] as string;

                            rptGens.Add(rptGen);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return rptGens;
        }

        public static List<System.Web.Mvc.SelectListItem> GetReportTypeForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> rptType = new List<System.Web.Mvc.SelectListItem>();
            rptType.Add(new System.Web.Mvc.SelectListItem() { Text = "B - Balance Sheet", Value = "B" });
            rptType.Add(new System.Web.Mvc.SelectListItem() { Text = "P - Profit and Lost", Value = "P" });
            rptType.Add(new System.Web.Mvc.SelectListItem() { Text = "G - General Expense and Administrative", Value = "G" });

            return rptType;
        }

        public static List<System.Web.Mvc.SelectListItem> GetReportLevelForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> rptLevel = new List<System.Web.Mvc.SelectListItem>();
            rptLevel.Add(new System.Web.Mvc.SelectListItem() { Text = "9", Value = "9" });
            rptLevel.Add(new System.Web.Mvc.SelectListItem() { Text = "8", Value = "8" });
            rptLevel.Add(new System.Web.Mvc.SelectListItem() { Text = "7", Value = "7" });
            rptLevel.Add(new System.Web.Mvc.SelectListItem() { Text = "6", Value = "6" });
            rptLevel.Add(new System.Web.Mvc.SelectListItem() { Text = "5", Value = "5" });
            rptLevel.Add(new System.Web.Mvc.SelectListItem() { Text = "4", Value = "4" });
            rptLevel.Add(new System.Web.Mvc.SelectListItem() { Text = "3", Value = "3" });
            rptLevel.Add(new System.Web.Mvc.SelectListItem() { Text = "2", Value = "2" });
            rptLevel.Add(new System.Web.Mvc.SelectListItem() { Text = "1", Value = "1" });

            return rptLevel;
        }
    }
}
