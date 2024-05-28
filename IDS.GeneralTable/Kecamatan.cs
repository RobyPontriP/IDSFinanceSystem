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
    public class Kecamatan
    {
        [Display(Name = "Kecamatan Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Kecamatan code is required")]
        [MaxLength(10), StringLength(10)]
        public string KecamatanCode { get; set; }

        [Display(Name = "Kecamatan Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Kecamatan name is required")]
        [MaxLength(50)]
        public string KecamatanName { get; set; }

        [Display(Name = "Country Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country Name is required")]
        public Country CountryKecamatan { get; set; }

        [Display(Name = "City Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "City Name is required")]
        public City CityKecamatan { get; set; }

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

        public Kecamatan()
        {
        }

        public Kecamatan(string kecamatanCode, string kecamatanName)
        {
            KecamatanCode = kecamatanCode;
            KecamatanName = kecamatanName;
        }

        public static List<Kecamatan> GetKecamatan()
        {
            List<IDS.GeneralTable.Kecamatan> list = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelKecamatan";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        list = new List<Kecamatan>();

                        while (dr.Read())
                        {
                            Kecamatan kecamatan = new Kecamatan();
                            kecamatan.KecamatanCode = dr["KecamatanCode"] as string;
                            kecamatan.KecamatanName = dr["KecamatanName"] as string;

                            kecamatan.CountryKecamatan = new Country();
                            kecamatan.CountryKecamatan.CountryCode = dr["CountryCode"] as string;
                            kecamatan.CountryKecamatan.CountryName = dr["CountryName"] as string;

                            kecamatan.CityKecamatan = new City();
                            kecamatan.CityKecamatan.CityCode = dr["CityCode"] as string;
                            kecamatan.CityKecamatan.CityName = dr["CityName"] as string;
                            kecamatan.CityKecamatan.Country = kecamatan.CountryKecamatan;
                            kecamatan.EntryUser = dr["EntryUser"] as string;
                            kecamatan.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            kecamatan.OperatorID = dr["OperatorID"] as string;
                            kecamatan.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(kecamatan);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static Kecamatan GetKecamatan(string kecamatanCode)
        {
            Kecamatan kecamatan = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelKecamatan";
                db.AddParameter("@KecamatanCode", System.Data.SqlDbType.VarChar, kecamatanCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        kecamatan = new Kecamatan();
                        kecamatan.KecamatanCode = dr["KecamatanCode"] as string;
                        kecamatan.KecamatanName = dr["KecamatanName"] as string;

                        kecamatan.CountryKecamatan = new Country();
                        kecamatan.CountryKecamatan.CountryCode = dr["CountryCode"] as string;
                        kecamatan.CountryKecamatan.CountryName = dr["CountryName"] as string;

                        kecamatan.CityKecamatan = new City();
                        kecamatan.CityKecamatan.CityCode = dr["CityCode"] as string;
                        kecamatan.CityKecamatan.CityName = dr["CityName"] as string;
                        kecamatan.CityKecamatan.Country = kecamatan.CountryKecamatan;
                        kecamatan.EntryUser = dr["EntryUser"] as string;
                        kecamatan.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        kecamatan.OperatorID = dr["OperatorID"] as string;
                        kecamatan.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                        kecamatan.EntryUser = dr["EntryUser"] as string;
                        kecamatan.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        kecamatan.OperatorID = dr["OperatorID"] as string;
                        kecamatan.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return kecamatan;
        }

        public static Kecamatan GetKecamatan(string kecamatanCode, string countryCode, string cityCode)
        {
            Kecamatan kecamatan = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelKecamatan";
                db.AddParameter("@CountryCode", System.Data.SqlDbType.VarChar, countryCode);
                db.AddParameter("@CityCode", System.Data.SqlDbType.VarChar, cityCode);
                db.AddParameter("@KecamatanCode", System.Data.SqlDbType.VarChar, kecamatanCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        kecamatan = new Kecamatan();
                        kecamatan.KecamatanCode = dr["KecamatanCode"] as string;
                        kecamatan.KecamatanName = dr["KecamatanName"] as string;

                        kecamatan.CountryKecamatan = new Country();
                        kecamatan.CountryKecamatan.CountryCode = dr["CountryCode"] as string;
                        kecamatan.CountryKecamatan.CountryName = dr["CountryName"] as string;

                        kecamatan.CityKecamatan = new City();
                        kecamatan.CityKecamatan.CityCode = dr["CityCode"] as string;
                        kecamatan.CityKecamatan.CityName = dr["CityName"] as string;
                        kecamatan.CityKecamatan.Country = kecamatan.CountryKecamatan;
                        kecamatan.EntryUser = dr["EntryUser"] as string;
                        kecamatan.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        kecamatan.OperatorID = dr["OperatorID"] as string;
                        kecamatan.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                        kecamatan.EntryUser = dr["EntryUser"] as string;
                        kecamatan.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        kecamatan.OperatorID = dr["OperatorID"] as string;
                        kecamatan.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return kecamatan;
        }

        public int InsUpDelKecamatan(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUPDKecamatan";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@KecamatanCode", System.Data.SqlDbType.VarChar, KecamatanCode);
                    cmd.AddParameter("@KecamatanName", System.Data.SqlDbType.VarChar, KecamatanName);
                    cmd.AddParameter("@CountryCode", System.Data.SqlDbType.VarChar, CountryKecamatan.CountryCode);
                    cmd.AddParameter("@CityCode", System.Data.SqlDbType.VarChar, CityKecamatan.CityCode);

                    if (!string.IsNullOrEmpty(EntryUser))
                    {
                        cmd.AddParameter("@EntryUser", System.Data.SqlDbType.VarChar, EntryUser);
                    }

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
                            throw new Exception("Kecamatan code is already exists. Please choose other kecamatan code.");
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

        public int InsUpDelKecamatan(int ExecCode, string[] data, string[] dataCountry, string[] dataCity)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUPDKecamatan";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GTUPDKecamatan";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@KecamatanCode", System.Data.SqlDbType.VarChar, data[i]);
                        cmd.AddParameter("@CountryCode", System.Data.SqlDbType.VarChar, dataCountry[i]);
                        cmd.AddParameter("@CityCode", System.Data.SqlDbType.VarChar, dataCity[i]);
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
                            throw new Exception("Kecamatan Code is already exists. Please choose other Kecamatan Code.");
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

        public static List<SelectListItem> GetKecamatanForDataSource()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelKecamatan";
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 5);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string kecamatanCode = dr["KecamatanCode"].ToString();
                            string kecamatanName = dr["KecamatanName"].ToString();
                            list.Add(new SelectListItem() { Text = kecamatanName, Value = kecamatanCode });
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<SelectListItem> GetKecamatanForDataSource(string cityCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelKecamatan";
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 6);
                db.AddParameter("@CityCode", System.Data.SqlDbType.VarChar, cityCode);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string kecamatanCode = dr["KecamatanCode"].ToString();
                            string kecamatanName = dr["KecamatanName"].ToString();
                            list.Add(new SelectListItem() { Text = kecamatanName, Value = kecamatanCode });
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
