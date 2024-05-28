using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GeneralTable
{
    public class Location
    {
        [Display(Name = "Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Location code is required")]
        [MaxLength(10), StringLength(10)]
        public string LocationCode { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Location name is required")]
        [MaxLength(50)]
        public string LocationName { get; set; }

        //[Display(Name = "Country Name")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Country Name is required")]
        //public Country Country { get; set; }

        [Display(Name = "Operator")]
        public string OperatorID { get; set; }

        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        //[Display(Name = "Remark")]
        //[MaxLength(500)]
        //public string Remark { get; set; }

        //[Display(Name = "BI Code")]
        //[MaxLength(10)]
        //public string BICode { get; set; }

        //[Display(Name = "OJK Code")]
        //[MaxLength(10)]
        //public string OJKCode { get; set; }

        //[Display(Name = "SLIK Code")]
        //[MaxLength(10)]
        //public string SLIKCode { get; set; }

        public Location()
        {
        }

        public Location(string locationCode, string locationName)
        {
            LocationCode = locationCode;
            LocationName = locationName;
        }

        public static List<Location> GetLocation()
        {
            List<IDS.GeneralTable.Location> list = new List<Location>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelLocation";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        list = new List<Location>();

                        while (dr.Read())
                        {
                            Location location = new Location();
                            location.LocationCode = dr["LocationCode"] as string;
                            location.LocationName = dr["LocationName"] as string;
                            //location.Country = IDS.GeneralTable.Country.GetCountry(dr["CountryCode"] as string);
                            location.OperatorID = dr["OperatorID"] as string;
                            location.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            //location.Remark = dr["Remark"] as string;
                            //location.BICode = dr["BICode"] as string;
                            //location.OJKCode = dr["OJKCode"] as string;
                            //location.SLIKCode = dr["SLIKCode"] as string;

                            list.Add(location);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static Location GetLocation(string locationCode)
        {
            Location location = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelLocation";
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, locationCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        location = new Location();
                        location.LocationCode = dr["LocationCode"] as string;
                        location.LocationName = dr["LocationName"] as string;
                        //location.Country = IDS.GeneralTable.Country.GetCountry(dr["CountryCode"] as string);
                        location.OperatorID = dr["OperatorID"] as string;
                        location.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                        //location.Remark = dr["Remark"] as string;
                        //location.BICode = dr["BICode"] as string;
                        //location.OJKCode = dr["OJKCode"] as string;
                        //location.SLIKCode = dr["SLIKCode"] as string;
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return location;
        }

        public static List<System.Web.Mvc.SelectListItem> GetLocationDatasource()
        {
            List<System.Web.Mvc.SelectListItem> location = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelLocation";
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        location = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem loc = new System.Web.Mvc.SelectListItem();
                            loc.Value = dr["LocationCode"] as string;
                            loc.Text = dr["LocationName"] as string;

                            location.Add(loc);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return location;
        }

        /// <summary>
        /// Untuk Insert, Update, Delete Data
        /// </summary>
        /// <param name="ExecCode"></param>
        /// <returns></returns>
        public int InsUpDelLocation(Tool.PageActivity ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTLocation";
                    cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, LocationCode);
                    cmd.AddParameter("@Name", System.Data.SqlDbType.VarChar, LocationName);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID.ToString());
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, (int)ExecCode);
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
                            throw new Exception("Location code is already exists. Please choose other location code.");
                        case 547:
                            throw new Exception("Data can not be delete while data used for reference.");
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

        /// <summary>
        /// Untuk delete data
        /// </summary>
        /// <param name="ExecCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int InsUpDelLocation(Tool.PageActivity ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTLocation";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, (int)ExecCode);
                        cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Location code is already exists. Please choose other location code.");
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
    }
}
