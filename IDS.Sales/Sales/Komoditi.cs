using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class Komoditi
    {
        public string Code { get; set; }
        public string TaxID { get; set; }
        public string Name { get; set; }
        public int JPID { get; set; }
        public decimal Rate { get; set; }

        public Komoditi()
        {

        }

        public static List<System.Web.Mvc.SelectListItem> GetKomoditiForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> komoditis = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelKomoditi";
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@TaxID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        komoditis = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem komoditi = new System.Web.Mvc.SelectListItem();
                            komoditi.Value = dr["code"] as string;
                            komoditi.Text = dr["name"] as string;

                            komoditis.Add(komoditi);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return komoditis;
        }
    }
}
