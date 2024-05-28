using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.FixedAsset
{
   public class FAAssetGroupExpense
    {
        public string AssetGroup { get; set; }
        public string AssetGroupDesc { get; set; }
        public string GLGainLoss { get; set; }
        public string DepMethod { get; set; }
        public string DepYear { get; set; }
        public string DepRate { get; set; }
        public string OperatorID { get; set; }
        public string LastUpdate { get; set; }
        public string ENTRYUSER { get; set; }
        public string ENTRYDATE { get; set; }

        public FAAssetGroupExpense() { }

        public static List<System.Web.Mvc.SelectListItem> FAAssetGroupExpenseForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> groups = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SelFAAssetGroupExpense";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem item = new System.Web.Mvc.SelectListItem();
                            item.Value = Tool.GeneralHelper.NullToString(dr["AssetGroup"]);
                            item.Text = Tool.GeneralHelper.NullToString(dr["AssetGroupDesc"]);
                            groups.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return groups;
        }//FAAssetGroupExpenseForDatasource

        public static List<System.Web.Mvc.SelectListItem> GetCOAForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                // Di ganti dari 8 ke 7
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 12);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                            coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                            coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(coa);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static List<IDS.FixedAsset.FAAssetGroupExpense> GetData(string grup)
        {
            List<IDS.FixedAsset.FAAssetGroupExpense> list = new List<IDS.FixedAsset.FAAssetGroupExpense>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFAAssetGroupExpense";
                db.AddParameter("@AsstGroup", System.Data.SqlDbType.VarChar, grup);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            IDS.FixedAsset.FAAssetGroupExpense c = new IDS.FixedAsset.FAAssetGroupExpense() {
                                AssetGroup = dr["AssetGroup"].ToString(),
                                AssetGroupDesc = dr["AssetGroupDesc"].ToString(),
                                DepMethod = dr["DepMethod"].ToString(),
                                DepRate = dr["DepRate"].ToString(),
                                DepYear =dr["DepYear"].ToString(),
                                GLGainLoss = dr["GLGainLoss"].ToString(),
                                OperatorID = dr["OperatorID"].ToString(),
                                 LastUpdate=dr["LastUpdate"].ToString()
                            };
                            list.Add(c);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }



    }
}
