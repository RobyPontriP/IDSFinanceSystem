using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDS.GeneralTable
{
    public class Division
    {
        [Display(Name = "Division Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Divisi Code is required")]
        [MaxLength(20), StringLength(20)]
        public string DivisiCode { get; set; }

        [Display(Name = "Division Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Divisi Name is required")]
        [MaxLength(100), StringLength(100)]
        public string DivisiName { get; set; }

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

        public Division()
        {

        }

        public Division(string divisiCode, string divisiName)
        {
            DivisiCode = divisiCode;
            DivisiName = divisiName;
        }

        public static List<Division> GetDivisi()
        {
            List<IDS.GeneralTable.Division> list = new List<Division>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSeltblDivisi";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@DivisiCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Division divisi = new Division();
                            divisi = new Division();
                            divisi.DivisiCode = dr["DivisiCode"] as string;
                            divisi.DivisiName = dr["Description"] as string;
                            divisi.EntryUser = dr["EntryUser"] as string;
                            divisi.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            divisi.OperatorID = dr["OperatorID"] as string;
                            divisi.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(divisi);
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
        /// Retrieve semua daftar Divisi untuk datasource
        /// </summary>
        /// <returns></returns>
        public static Division GetDivisi(string divisiCode)
        {
            Division divisi = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSeltblDivisi";
                db.AddParameter("@DivisiCode", System.Data.SqlDbType.VarChar, divisiCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        divisi = new Division();
                        divisi.DivisiCode = dr["DivisiCode"] as string;
                        divisi.DivisiName = dr["Description"] as string;
                        divisi.EntryUser = dr["EntryUser"] as string;
                        divisi.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        divisi.OperatorID = dr["OperatorID"] as string;
                        divisi.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return divisi;
        }

        /// <summary>
        /// Retrieve semua daftar Division
        /// </summary>
        /// <returns></returns>        
        public static List<KeyValuePair<string, string>> GetDivisiForDatasource()
        {
            List<KeyValuePair<string, string>> divisies = new List<KeyValuePair<string, string>>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTUpdtblDivisi";
                db.AddParameter("@type", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@DivisiCode", System.Data.SqlDbType.TinyInt, 4);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        divisies = new List<KeyValuePair<string, string>>();

                        while (dr.Read())
                        {
                            KeyValuePair<string, string> divisi = new KeyValuePair<string, string>(dr["DivisiCode"] as string, dr["DivisiName"] as string);
                            divisies.Add(divisi);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }

            return divisies;
        }

        public int InsUpDelDivisi(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdtblDivisi";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@DivisiCode", System.Data.SqlDbType.VarChar, DivisiCode.ToString());
                    cmd.AddParameter("@Description", System.Data.SqlDbType.VarChar, DivisiName.ToString());
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID.ToString());
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
                            throw new Exception("Divisi code is already exists. Please choose other divisi code.");
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

        public int InsUpDelDivisi(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdtblDivisi";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GTUpdtblDivisi";
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@DivisiCode", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Divisi Code is already exists. Please choose other Divisi Code.");
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

        public static List<SelectListItem> GetDivisionForDataSource()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "select DivisiCode, Description from tblDivisi";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string divisiCode = dr["DivisiCode"].ToString();
                            string divisiName = dr["Description"].ToString();
                            list.Add(new SelectListItem() { Text = divisiName, Value = divisiCode });
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
