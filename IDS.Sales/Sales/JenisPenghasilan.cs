using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class JenisPenghasilan
    {
        public int JPID { get; set; }

        public string KodePajak { get; set; }

        public string KodePenghasilan { get; set; }

        public string KodeForm { get; set; }

        public decimal TarifBruto { get; set; }

        public string Description { get; set; }

        public JenisPenghasilan()
        {

        }

        public static List<System.Web.Mvc.SelectListItem> GetJPForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> jps = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelJenisPenghasilan";
                db.AddParameter("@JPID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@KodePajak", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        jps = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem jp = new System.Web.Mvc.SelectListItem();
                            jp.Value = Tool.GeneralHelper.NullToString(dr["jpid"]);
                            jp.Text = dr["description"] as string;

                            jps.Add(jp);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return jps;
        }
    }
}
