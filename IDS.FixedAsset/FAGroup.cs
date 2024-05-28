using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.FixedAsset
{
    public class FAGroup
    {
        [Display(Name = "Group Code")]
        [MaxLength(20), StringLength(20)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Group code is required")]
        public string Code { get; set; }

        [Display(Name = "Group Name")]
        [MaxLength(20), StringLength(20)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Group name is required")]
        public string Description { get; set; }

        // Depreciation
        [Display(Name = "Depre. Method")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Depreciation method is required")]
        public FADepreMethod DepreMethod { get; set; }
                
        [Display(Name = "Depre. Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Depreciation year is required")]
        public int DepreYear { get; set; }
        [Display(Name = "Depre. Rate")]
        public decimal DepreRate { get; set; }

        // GL Account
        [Display(Name = "Item Group Account")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Item group account is required")]
        public string GLAccItemGroup { get; set; }
        [Display(Name = "Accum. Depre. Account")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Depreciation accumulation account is required")]
        public string GLAccumDepre { get; set; }
        [Display(Name = "Depre. Expense Account")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Depreciation expense account is required")]
        public string GLAccDepreExpense { get; set; }
        [Display(Name = "Gain Loss Account")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gain/Loss account is required")]
        public string GLAccGainLoss { get; set; } 
        
        // Log
        [Display(Name = "Operartor ID")]
        public string OperatorID { get; set; }
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }
        [Display(Name = "Entry Name")]
        public string EntryUser { get; set; }
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        public FAGroup()
        {
        }

        public static List<FAGroup> GetFAGroup()
        {
            List<FAGroup> groups = new List<FAGroup>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SelFAGroup";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            FAGroup item = new FAGroup();
                            item.Code = Tool.GeneralHelper.NullToString(dr["AssetGroup"]);
                            item.Description = Tool.GeneralHelper.NullToString(dr["AssetGroupDesc"]);

                            item.DepreMethod = (FADepreMethod)Convert.ToInt32(dr["DepMethod"]);
                            item.DepreYear = Tool.GeneralHelper.NullToInt(dr["DepYear"], 1);
                            item.DepreRate = Tool.GeneralHelper.NullToDecimal(dr["DepRate"], 0);

                            item.GLAccItemGroup = Tool.GeneralHelper.NullToString(dr["GLAccItemGrp"]);
                            item.GLAccumDepre = Tool.GeneralHelper.NullToString(dr["GLAccAccumDep"]);
                            item.GLAccDepreExpense = Tool.GeneralHelper.NullToString(dr["GLAccDepExp"]);
                            item.GLAccGainLoss = Tool.GeneralHelper.NullToString(dr["GLGainLoss"]);

                            item.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            item.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now.Date);
                            item.EntryUser = Tool.GeneralHelper.NullToString(dr["ENTRYUSER"]);
                            item.EntryDate = Tool.GeneralHelper.NullToDateTime(dr["ENTRYDATE"], DateTime.Now.Date);                            

                            groups.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return groups;
        }

        public static FAGroup GetFAGroup(string groupCode)
        {
            FAGroup group = new FAGroup();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SelFAGroup";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, groupCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        group.Code = Tool.GeneralHelper.NullToString(dr["AssetGroup"]);
                        group.Description = Tool.GeneralHelper.NullToString(dr["AssetGroupDesc"]);

                        group.DepreMethod = (FADepreMethod)Convert.ToInt32(dr["DepMethod"]);
                        group.DepreYear = Tool.GeneralHelper.NullToInt(dr["DepYear"], 1);
                        group.DepreRate = Tool.GeneralHelper.NullToDecimal(dr["DepRate"], 0);

                        group.GLAccItemGroup = Tool.GeneralHelper.NullToString(dr["GLAccItemGrp"]);
                        group.GLAccumDepre = Tool.GeneralHelper.NullToString(dr["GLAccAccumDep"]);
                        group.GLAccDepreExpense = Tool.GeneralHelper.NullToString(dr["GLAccDepExp"]);
                        group.GLAccGainLoss = Tool.GeneralHelper.NullToString(dr["GLGainLoss"]);

                        group.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                        group.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now.Date);
                        group.EntryUser = Tool.GeneralHelper.NullToString(dr["ENTRYUSER"]);
                        group.EntryDate = Tool.GeneralHelper.NullToDateTime(dr["ENTRYDATE"], DateTime.Now.Date);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return group;
        }

        public static List<System.Web.Mvc.SelectListItem> getFAGroupForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> groups = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SelFAGroup";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
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
        }

        public int InsUpDel(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "FAInsUpDelAssetGroup";
                    db.AddParameter("@Flag", System.Data.SqlDbType.TinyInt, ExecCode);
                    db.AddParameter("@AssetGroup", System.Data.SqlDbType.VarChar, Code);
                    db.AddParameter("@AssetGroupDesc", System.Data.SqlDbType.VarChar, Description);
                    db.AddParameter("@GLAccItemGrp", System.Data.SqlDbType.VarChar, GLAccItemGroup);
                    db.AddParameter("@GLAccAccumDep", System.Data.SqlDbType.VarChar, GLAccumDepre);
                    db.AddParameter("@GLAccDepExp", System.Data.SqlDbType.VarChar, GLAccDepreExpense);
                    db.AddParameter("@GLGainLoss", System.Data.SqlDbType.VarChar, GLAccGainLoss);
                    db.AddParameter("@DepMethod", System.Data.SqlDbType.TinyInt, (int)DepreMethod);
                    db.AddParameter("@DepYear", System.Data.SqlDbType.Int, DepreYear);
                    db.AddParameter("@DepRate", System.Data.SqlDbType.Float, DepreRate);
                    db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);

                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();

                    db.BeginTransaction();
                    result = db.ExecuteNonQuery();
                    db.CommitTransaction();
                }
                catch (SqlException sex)
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Group code is already exists. Please choose other group code.");
                        default:
                            throw;
                    }
                }
                catch
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    throw;
                }
                finally
                {
                    db.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// Untuk menghapus data FA group lebih dari satu kode
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int InsUpDel(string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "FAInsUpDelAssetGroup";
                    db.Open();
                    db.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        db.AddParameter("@Flag", System.Data.SqlDbType.TinyInt, 3);
                        db.AddParameter("@AssetGroup", System.Data.SqlDbType.VarChar, data[i]);
                        db.AddParameter("@AssetGroupDesc", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@GLAccItemGrp", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@GLAccAccumDep", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@GLAccDepExp", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@GLGainLoss", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DepMethod", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        db.AddParameter("@DepYear", System.Data.SqlDbType.Int, DBNull.Value);
                        db.AddParameter("@DepRate", System.Data.SqlDbType.Float, DBNull.Value);
                        db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.CommandType = System.Data.CommandType.StoredProcedure;

                        db.ExecuteNonQuery();
                    }

                    db.CommitTransaction();
                }
                catch (SqlException sex)
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Fixed asset group code is already exists. Please choose other fixed asset group code.");
                        case 547:
                            throw new Exception("One or more data can not be delete while data used for reference.");
                        default:
                            throw;
                    }
                }
                catch
                {
                    if (db.Transaction != null)
                        db.RollbackTransaction();

                    throw;
                }
                finally
                {
                    db.Close();
                }
            }

            return result;
        }
    }
}