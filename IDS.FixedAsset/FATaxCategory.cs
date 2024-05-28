using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IDS.FixedAsset
{
    public class FATaxCategory
    {
        public string TaxID { get; set; }
        public string TaxDescription { get; set; }
        public FADepreMethod DepreMethod { get; set; }
        public int Year { get; set; }
        public decimal Rate { get; set; }

        public string EntryUser { get; set; }
        public DateTime EntryDate { get; set; }
        public string OperatorID { get; set; }
        public DateTime LastUpdate { get; set; }

        public string DepreMethodToString { get; set; }

        public FATaxCategory()
        {

        }

        public static List<SelectListItem> GetFATaxCategoryForDatasource()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFATaxCategory";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TaxID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        items = new List<SelectListItem>();

                        while (dr.Read())
                        {
                            SelectListItem item = new SelectListItem();
                            item.Value = Tool.GeneralHelper.NullToString(dr["TaxCatID"]);
                            item.Text = dr["taxCatDesc"].ToString();
                            items.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return items;
        }//GetFATaxCategoryForDatasource

        public static List<IDS.FixedAsset.FATaxCategory> GetData(string TaxId, string Method)
        {
            List<IDS.FixedAsset.FATaxCategory> list = new List<IDS.FixedAsset.FATaxCategory>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFATaxCategory";
                db.AddParameter("@TaxID", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(TaxId));
                db.AddParameter("@Method", System.Data.SqlDbType.VarChar, Method);
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
                            IDS.FixedAsset.FATaxCategory c = new IDS.FixedAsset.FATaxCategory()
                            {
                                OperatorID = dr["OperatorID"].ToString(),
                                DepreMethodToString = dr["TaxCatDepMethod"].ToString(),
                                LastUpdate = System.Convert.ToDateTime(dr["LastUpdate"].ToString()),
                                Rate = Tool.GeneralHelper.NullToDecimal(dr["TaxCatDepRate"], 0),
                                TaxDescription = dr["TaxCatDesc"].ToString(),
                                TaxID = dr["TaxCatID"].ToString(),
                                Year = Tool.GeneralHelper.NullToInt(dr["TaxCatDepYear"], 0)
                            };
                            list.Add(c);
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static IDS.FixedAsset.FATaxCategory GetDataGroup(string GrouP)
        {
            IDS.FixedAsset.FATaxCategory l = new IDS.FixedAsset.FATaxCategory();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFATaxCategory";
                db.AddParameter("@TaxID", System.Data.SqlDbType.VarChar, GrouP);
                db.AddParameter("@Method", System.Data.SqlDbType.VarChar, "");
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        l = new IDS.FixedAsset.FATaxCategory()
                        {
                            OperatorID = dr["OperatorID"].ToString(),
                            DepreMethodToString = dr["TaxCatDepMethod"].ToString(),
                            LastUpdate = System.Convert.ToDateTime(dr["LastUpdate"].ToString()),
                            Rate = Tool.GeneralHelper.NullToDecimal(dr["TaxCatDepRate"], 0),
                            TaxDescription = dr["TaxCatDesc"].ToString(),
                            TaxID = dr["TaxCatID"].ToString(),
                            Year = Tool.GeneralHelper.NullToInt(dr["TaxCatDepYear"], 0)
                        };
                    }// end if
                    if (!dr.IsClosed)
                        dr.Close();
                }// end using
                db.Close();
            }
            return l;
        }

        public static string SaveFATaxCategory(IDS.FixedAsset.FATaxCategory s, Decimal T)
        {
            string retur_ = "";
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFATaxCategory";
                db.AddParameter("@TaxID", System.Data.SqlDbType.VarChar, s.TaxID);
                db.AddParameter("@TaxCatID", System.Data.SqlDbType.VarChar, s.TaxID);
                db.AddParameter("@TaxCatDesc", System.Data.SqlDbType.VarChar, s.TaxDescription);
                db.AddParameter("@TaxCatDepMethod", System.Data.SqlDbType.TinyInt, s.DepreMethodToString);
                db.AddParameter("@TaxCatDepYear", System.Data.SqlDbType.Int, s.Year);
                db.AddParameter("@TaxCatDepRate", System.Data.SqlDbType.Float, s.Rate);
                db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, s.OperatorID);
                db.AddParameter("@LastUpdate", System.Data.SqlDbType.DateTime, s.LastUpdate);
                db.AddParameter("@ENTRYUSER", System.Data.SqlDbType.VarChar, s.EntryUser);
                db.AddParameter("@ENTRYDATE", System.Data.SqlDbType.DateTime, s.EntryDate);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, T);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        retur_ = dr["Msg"].ToString();
                    }
                    if (!dr.IsClosed) dr.Close();
                }

                db.Close();
            }
            return retur_;
        }
    }
}