using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GeneralTable
{
   public class tblJenisPenghasilan
    {
        public string KodePenghasilan { get; set; }
        public string KodePajak { get; set; }
        public string Desctiption { get; set; }
        public int JPID { get; set; }
        public string KodeForms { get; set; }
        public double TarifBruto { get; set; }
        public tblJenisPenghasilan() { }

        public static List<System.Web.Mvc.SelectListItem> Get_TaxObjectType()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "sel_TaxObjectType";
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt,1);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem penghasilan = new System.Web.Mvc.SelectListItem();
                            penghasilan.Value = IDS.Tool.GeneralHelper.NullToString(dr["KodePenghasilan"]);
                            penghasilan.Text = IDS.Tool.GeneralHelper.NullToString(dr["Description"]);
                            list.Add(penghasilan);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetKodePajakForDataSource(string jenisPPh)
        {
            List<System.Web.Mvc.SelectListItem> kodePajaks = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelJenisPenghasilan";
                db.AddParameter("@kodePajak", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(jenisPPh));
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        kodePajaks = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem kodePajak = new System.Web.Mvc.SelectListItem();
                            kodePajak.Value = dr["KodePajak"] as string;
                            kodePajak.Text = dr["Description"] as string;

                            kodePajaks.Add(kodePajak);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return kodePajaks;
        }

        public static List<System.Web.Mvc.SelectListItem> GetKodePajakForDataSource(string jenisPPh,string kodePenghasilan)
        {
            List<System.Web.Mvc.SelectListItem> kodePajaks = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelJenisPenghasilan";
                db.AddParameter("@kodePajak", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(jenisPPh));
                db.AddParameter("@kodePenghasilan", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(kodePenghasilan));
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 5);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        kodePajaks = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem kodePajak = new System.Web.Mvc.SelectListItem();
                            kodePajak.Value = dr["KodePajak"] as string;
                            kodePajak.Text = dr["Description"] as string;

                            kodePajaks.Add(kodePajak);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return kodePajaks;
        }

        public static string GetKodePenghasilan(string kodePajak, string desc)
        {
            string kodePenghasilan = "";

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelJenisPenghasilan";
                db.AddParameter("@kodePajak", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(kodePajak));
                db.AddParameter("@desc", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(desc));
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            kodePenghasilan = dr["KodePenghasilan"] as string;
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return kodePenghasilan;
        }
    }
}
