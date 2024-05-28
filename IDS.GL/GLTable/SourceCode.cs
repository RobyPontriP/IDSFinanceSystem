using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDS.GLTable
{
    public class SourceCode
    {
        [Display(Name = "Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Source Code is required")]
        [MaxLength(4), StringLength(4)]
        public string Code { get; set; }

        [Display(Name = "Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Source Code Description is required")]
        [MaxLength(30), StringLength(30)]
        public string Description { get; set; }

        [Display(Name = "Created By")]
        [MaxLength(20), StringLength(20)]
        public string EntryUser { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        [MaxLength(20), StringLength(20)]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public SourceCode()
        {
        }

        public SourceCode(string code,string scodedesc)
        {
            Code = code;
            Description = scodedesc;
        }

        public static List<SourceCode> GetSourceCode()
        {
            List<IDS.GLTable.SourceCode> list = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSourceCode";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        list = new List<SourceCode>();

                        while (dr.Read())
                        {
                            SourceCode sourceCode = new SourceCode();
                            sourceCode.Code = dr["Code"] as string;
                            sourceCode.Description = dr["ScodeDesc"] as string;
                            sourceCode.EntryUser = dr["EntryUser"] as string;
                            sourceCode.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            sourceCode.OperatorID = dr["OperatorID"] as string;
                            sourceCode.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(sourceCode);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        /// <summary>
        /// Retrieve semua daftar Brand untuk datasource
        /// </summary>
        /// <returns></returns>

        public static SourceCode GetSourceCode(string sourceCode)
        {
            SourceCode sourceCodeObj = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSourceCode";
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, sourceCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        sourceCodeObj = new SourceCode();
                        sourceCodeObj.Code = dr["Code"] as string;
                        sourceCodeObj.Description = dr["ScodeDesc"] as string;
                        sourceCodeObj.EntryUser = dr["EntryUser"] as string;
                        sourceCodeObj.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        sourceCodeObj.OperatorID = dr["OperatorID"] as string;
                        sourceCodeObj.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return sourceCodeObj;
        }

        /// <summary>
        /// Retrieve semua daftar Brand
        /// </summary>
        /// <returns></returns>
        public int InsUpDelSourceCode(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLSourceCode";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, Code);
                    cmd.AddParameter("@ScodeDesc", System.Data.SqlDbType.VarChar, Description);
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
                            throw new Exception("Source Code is already exists. Please choose other Source Code.");
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

        public int InsUpDelSourceCode(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLSourceCode";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GLSourceCode";
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Source Code is already exists. Please choose other Source Code.");
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

        public static List<SelectListItem> GetSourceCodeForDataSource()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelSourceCode";
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        //list.Add(new SelectListItem() { Text = "ALL", Value = "ALL" });
                        while (dr.Read())
                        {
                            string sCode = dr["Code"].ToString();
                            string ScodeDesc = dr["ScodeDesc"].ToString();
                            list.Add(new SelectListItem() { Text = sCode +" - "+ ScodeDesc, Value = sCode });
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }
    }
}
