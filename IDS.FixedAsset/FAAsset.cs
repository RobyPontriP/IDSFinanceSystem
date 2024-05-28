using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using IDS.Tool;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.FixedAsset
{
    public class FAAsset
    {
        [Display(Name = "Asset No"), Required(AllowEmptyStrings = false, ErrorMessage = "Asset no is required")]
        public string AssetNo { get; set; }

        [Display(Name = "Branch Code"), Required(AllowEmptyStrings = false, ErrorMessage = "Branch Code is required")]
        public string BranchCode { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Group"), Required(AllowEmptyStrings = false, ErrorMessage = "Asset group is required")]
        public FAGroup Group { get; set; }

        [Display(Name = "Part Of")]
        public string PartOf { get; set; }

        [Display(Name = "Tax Category"), Required(AllowEmptyStrings = false, ErrorMessage = "Asset tax category is required")]
        public string TaxCategoryID { get; set; }

        [Display(Name = "Description"), Required(AllowEmptyStrings = false, ErrorMessage = "Asset description is required")]
        public string Description { get; set; }

        [Display(Name = "Asset Voucher")]
        public string AssetVoucher { get; set; }

        [Display(Name = "Serial No")]
        public string SerialNo { get; set; }

        [Display(Name = "Vendor")]
        public string Vendor { get; set; }

        [Display(Name = "Qty.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Qty { get; set; }

        [Display(Name = "Currency"), Required(AllowEmptyStrings = false, ErrorMessage = "Currency is required")]
        public string Currency { get; set; }

        [Display(Name = "Unit Price"), DisplayFormat(ApplyFormatInEditMode = true, NullDisplayText ="0", DataFormatString = "{0:N2}")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Residual Value")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal ResidualValue { get; set; }

        [Display(Name = "Total Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Base Total Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal BaseTotalPrice { get; set; }

        [Display(Name = "Business Use"), Range(0, 100, ErrorMessage = "Business use range between 0 and 100")]
        public decimal BusinessUse { get; set; }

        [Display(Name = "Acquisition Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "dd/MMM/yyyy", ApplyFormatInEditMode = true)]
        public DateTime AcquisitionDate { get; set; }

        [Display(Name = "Starting Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "dd/MMM/yyyy", ApplyFormatInEditMode = true)]
        public DateTime StartingDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "dd/MMM/yyyy", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = " Status"), Required(AllowEmptyStrings = false, ErrorMessage = "Asset status is required")]
        public FAAssetStatus Status { get; set; }

        [Display(Name = "Status Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "dd/MMM/yyyy", ApplyFormatInEditMode = true)]
        public DateTime StatusDate { get; set; }

        [Display(Name = "Exchage Rate")]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [Display(Name = "Last Update")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "dd/MMM/yyyy HH:mm:ss")]
        public DateTime LastUpdate { get; set; }

        [Display(Name = "Entry User")]
        public string EntryUser { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        public FAAsset()
        {

        }

        public static List<FAAsset> GetFAAsset(string branch)
        {
            List<FAAsset> assets = new List<FAAsset>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SelFAAsset";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            FAAsset asset = new FAAsset();
                            asset.AssetNo = Tool.GeneralHelper.NullToString(dr["AssetNo"]);
                            asset.Description = Tool.GeneralHelper.NullToString(dr["Description"]);
                            asset.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            asset.Department = Tool.GeneralHelper.NullToString(dr["DeptCode"]);
                            asset.Location = Tool.GeneralHelper.NullToString(dr["Location"]);

                            if (dr["ItemGroup"] != DBNull.Value)
                            {
                                asset.Group = new FAGroup();
                                asset.Group.Code = Tool.GeneralHelper.NullToString(dr["AssetGroup"]);
                                asset.Group.Description = Tool.GeneralHelper.NullToString(dr["AssetGroupDesc"]);

                                asset.Group.DepreMethod = (FADepreMethod)Convert.ToInt32(dr["DepMethod"]);
                                asset.Group.DepreYear = Tool.GeneralHelper.NullToInt(dr["DepYear"], 1);
                                asset.Group.DepreRate = Tool.GeneralHelper.NullToDecimal(dr["DepRate"], 0);

                                asset.Group.GLAccItemGroup = Tool.GeneralHelper.NullToString(dr["GLAccItemGrp"]);
                                asset.Group.GLAccumDepre = Tool.GeneralHelper.NullToString(dr["GLAccAccumDep"]);
                                asset.Group.GLAccDepreExpense = Tool.GeneralHelper.NullToString(dr["GLAccDepExp"]);
                                asset.Group.GLAccGainLoss = Tool.GeneralHelper.NullToString(dr["GLGainLoss"]);

                                asset.Group.EntryUser = Tool.GeneralHelper.NullToString(dr["GroupEntryUser"]);
                                asset.Group.EntryDate = Tool.GeneralHelper.NullToDateTime(dr["GroupEntryDate"], DateTime.Now.Date);
                                asset.Group.OperatorID = Tool.GeneralHelper.NullToString(dr["GroupOperatorID"]);
                                asset.Group.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["GroupLastUpdate"], DateTime.Now.Date);
                            }

                            asset.PartOf = Tool.GeneralHelper.NullToString(dr["PartOf"]);
                            asset.TaxCategoryID = Tool.GeneralHelper.NullToString(dr["TaxCategoryID"]);
                            asset.AssetVoucher = Tool.GeneralHelper.NullToString(dr["AssetVoucher"]);
                            asset.SerialNo = Tool.GeneralHelper.NullToString(dr["SerialNo"]);
                            asset.Vendor = Tool.GeneralHelper.NullToString(dr["Vendor"]);
                            asset.Qty= Tool.GeneralHelper.NullToDecimal(dr["Qty"], 1);
                            asset.Currency = Tool.GeneralHelper.NullToString(dr["CCY"]);
                            asset.UnitPrice = Tool.GeneralHelper.NullToDecimal(dr["UnitPrice"], 0);
                            asset.ResidualValue = Tool.GeneralHelper.NullToDecimal(dr["ResidualValue"], 0);
                            asset.TotalPrice = Tool.GeneralHelper.NullToDecimal(dr["TotalPrice"], 0);
                            asset.BaseTotalPrice = Tool.GeneralHelper.NullToDecimal(dr["BaseTotalPrice"], 0);
                            asset.BusinessUse = Tool.GeneralHelper.NullToDecimal(dr["BusinessUse"], 0);
                            asset.AcquisitionDate = Tool.GeneralHelper.NullToDateTime(dr["AcquisitionDate"], DateTime.Now.Date);
                            asset.StartingDate = Tool.GeneralHelper.NullToDateTime(dr["StartingDate"], DateTime.Now.Date);
                            asset.EndDate = Tool.GeneralHelper.NullToDateTime(dr["EndDate"], DateTime.Now.Date);
                            asset.Status = ((IDS.FixedAsset.FAAssetStatus) Convert.ToInt16(dr["Status"]));
                            asset.StatusDate = Tool.GeneralHelper.NullToDateTime(dr["StatusDate"], DateTime.Now.Date);
                            asset.ExchangeRate = Tool.GeneralHelper.NullToDecimal(dr["ExChangeRate"], 1);
                            
                            asset.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            asset.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now.Date);
                            asset.EntryUser = Tool.GeneralHelper.NullToString(dr["ENTRYUSER"]);
                            asset.EntryDate = Tool.GeneralHelper.NullToDateTime(dr["ENTRYDATE"], DateTime.Now.Date);

                            assets.Add(asset);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return assets;
        }

        public static FAAsset GetFAAsset(string assetNo, string branchCode)
        {
            FAAsset asset = new FAAsset();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SelFAAsset";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, assetNo);
                db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        asset.AssetNo = Tool.GeneralHelper.NullToString(dr["AssetGroup"]);
                        asset.Description = Tool.GeneralHelper.NullToString(dr["Description"]);
                        asset.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                        asset.Department = Tool.GeneralHelper.NullToString(dr["DeptCode"]);
                        asset.Location = Tool.GeneralHelper.NullToString(dr["Location"]);

                        if (dr["ItemGroup"] != DBNull.Value)
                        {
                            asset.Group = new FAGroup();
                            asset.Group.Code = Tool.GeneralHelper.NullToString(dr["AssetGroup"]);
                            asset.Group.Description = Tool.GeneralHelper.NullToString(dr["AssetGroupDesc"]);

                            asset.Group.DepreMethod = (FADepreMethod)Convert.ToInt32(dr["DepMethod"]);
                            asset.Group.DepreYear = Tool.GeneralHelper.NullToInt(dr["DepYear"], 1);
                            asset.Group.DepreRate = Tool.GeneralHelper.NullToDecimal(dr["DepRate"], 0);

                            asset.Group.GLAccItemGroup = Tool.GeneralHelper.NullToString(dr["GLAccItemGrp"]);
                            asset.Group.GLAccumDepre = Tool.GeneralHelper.NullToString(dr["GLAccAccumDep"]);
                            asset.Group.GLAccDepreExpense = Tool.GeneralHelper.NullToString(dr["GLAccDepExp"]);
                            asset.Group.GLAccGainLoss = Tool.GeneralHelper.NullToString(dr["GLGainLoss"]);

                            asset.Group.EntryUser = Tool.GeneralHelper.NullToString(dr["GroupEntryUser"]);
                            asset.Group.EntryDate = Tool.GeneralHelper.NullToDateTime(dr["GroupEntryDate"], DateTime.Now.Date);
                            asset.Group.OperatorID = Tool.GeneralHelper.NullToString(dr["GroupOperatorID"]);
                            asset.Group.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["GroupLastUpdate"], DateTime.Now.Date);
                        }

                        asset.PartOf = Tool.GeneralHelper.NullToString(dr["PartOf"]);
                        asset.TaxCategoryID = Tool.GeneralHelper.NullToString(dr["TaxCategoryID"]);
                        asset.AssetVoucher = Tool.GeneralHelper.NullToString(dr["AssetVoucher"]);
                        asset.SerialNo = Tool.GeneralHelper.NullToString(dr["SerialNo"]);
                        asset.Vendor = Tool.GeneralHelper.NullToString(dr["Vendor"]);
                        asset.Qty = Tool.GeneralHelper.NullToDecimal(dr["Qty"], 1);
                        asset.Currency = Tool.GeneralHelper.NullToString(dr["CCY"]);
                        asset.UnitPrice = Tool.GeneralHelper.NullToDecimal(dr["UnitPrice"], 0);
                        asset.ResidualValue = Tool.GeneralHelper.NullToDecimal(dr["ResidualValue"], 0);
                        asset.TotalPrice = Tool.GeneralHelper.NullToDecimal(dr["TotalPrice"], 0);
                        asset.BaseTotalPrice = Tool.GeneralHelper.NullToDecimal(dr["BaseTotalPrice"], 0);
                        asset.BusinessUse = Tool.GeneralHelper.NullToDecimal(dr["BusinessUse"], 0);
                        asset.AcquisitionDate = Tool.GeneralHelper.NullToDateTime(dr["AcquisitionDate"], DateTime.Now.Date);
                        asset.StartingDate = Tool.GeneralHelper.NullToDateTime(dr["StartingDate"], DateTime.Now.Date);
                        asset.EndDate = Tool.GeneralHelper.NullToDateTime(dr["EndDate"], DateTime.Now.Date);
                        asset.Status = ((IDS.FixedAsset.FAAssetStatus)Convert.ToInt16(dr["Status"]));
                        asset.StatusDate = Tool.GeneralHelper.NullToDateTime(dr["StatusDate"], DateTime.Now.Date);
                        asset.ExchangeRate = Tool.GeneralHelper.NullToDecimal(dr["ExChangeRate"], 1);

                        asset.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                        asset.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now.Date);
                        asset.EntryUser = Tool.GeneralHelper.NullToString(dr["ENTRYUSER"]);
                        asset.EntryDate = Tool.GeneralHelper.NullToDateTime(dr["ENTRYDATE"], DateTime.Now.Date);
                    }
                    
                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return asset;
        }

        public static List<System.Web.Mvc.SelectListItem> getFAAssetForDatasource(string branchCode)
        {
            List<System.Web.Mvc.SelectListItem> groups = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SelFAAsset";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
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
                            item.Value = Tool.GeneralHelper.NullToString(dr["AssetNo"]);
                            item.Text = Tool.GeneralHelper.NullToString(dr["Description"]);

                            groups.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return groups;
        }

        public static List<System.Web.Mvc.SelectListItem> getFAAssetNoForDatasource(string branchCode)
        {
            List<System.Web.Mvc.SelectListItem> groups = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SelFAAsset";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
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
                            item.Value = Tool.GeneralHelper.NullToString(dr["AssetNo"]);
                            item.Text = Tool.GeneralHelper.NullToString(dr["AssetNo"]) + " - " +Tool.GeneralHelper.NullToString(dr["Description"]);

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
                    if (ExecCode == 1) // New
                    {
                        // Get Last Asset No
                        db.CommandText = "SELECT MAX(AssetNo) AS Res FROM FAAsset WHERE Right(AssetNo,4)=@year AND BranchCode=@branchCode AND ItemGroup=@itemGroup";
                        db.AddParameter("@year", System.Data.SqlDbType.VarChar, AcquisitionDate.ToString("yyyy"));
                        db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, BranchCode);
                        db.AddParameter("@itemGroup", System.Data.SqlDbType.VarChar, Group.Code);
                        db.CommandType = System.Data.CommandType.Text;
                        db.Open();

                        string lastAssetNo = Tool.GeneralHelper.NullToString(db.ExecuteScalar());
                        int lastNo = 1;

                        if (lastAssetNo != "")
                        {
                            lastNo = Convert.ToInt32(lastAssetNo.Split('/')[1]);
                            lastNo++;
                        }

                        AssetNo = string.Format("{0}/{1}/{2}/{3}", Group.Code,  ("000" + lastNo.ToString()).Right(3), BranchCode, AcquisitionDate.ToString("yyyy"));
                    }
                    
                    
                    db.CommandText = "FAInsUpDelAsset";
                    db.AddParameter("@Type", System.Data.SqlDbType.VarChar, ExecCode);
                    db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, AssetNo);
                    db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchCode);
                    db.AddParameter("@DeptCode", System.Data.SqlDbType.VarChar, Department);
                    db.AddParameter("@Location", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Location));
                    db.AddParameter("@ItemGrp", System.Data.SqlDbType.VarChar, (object)Group?.Code ?? DBNull.Value);
                    db.AddParameter("@Part", System.Data.SqlDbType.VarChar, PartOf);
                    db.AddParameter("@TaxCatID", System.Data.SqlDbType.VarChar, TaxCategoryID);
                    db.AddParameter("@Desc", System.Data.SqlDbType.VarChar, Description);
                    db.AddParameter("@AssetVoucher", System.Data.SqlDbType.VarChar, AssetVoucher);
                    db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, SerialNo);
                    db.AddParameter("@Vend", System.Data.SqlDbType.VarChar, Vendor);
                    db.AddParameter("@Qty", System.Data.SqlDbType.Float, Qty);
                    db.AddParameter("@Unit", System.Data.SqlDbType.Money, UnitPrice);
                    db.AddParameter("@Resi", System.Data.SqlDbType.Money, ResidualValue);
                    db.AddParameter("@Tot", System.Data.SqlDbType.Money, TotalPrice);
                    db.AddParameter("@BaseTot", System.Data.SqlDbType.Money, BaseTotalPrice);
                    db.AddParameter("@Business", System.Data.SqlDbType.Float, BusinessUse);
                    db.AddParameter("@Acq", System.Data.SqlDbType.DateTime, AcquisitionDate);
                    db.AddParameter("@Start", System.Data.SqlDbType.DateTime, StartingDate);
                    db.AddParameter("@End", System.Data.SqlDbType.DateTime, EndDate);
                    db.AddParameter("@Stat", System.Data.SqlDbType.TinyInt, (int)Status);
                    db.AddParameter("@StatDate", System.Data.SqlDbType.DateTime, StatusDate);
                    db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Currency);
                    db.AddParameter("@OP", System.Data.SqlDbType.VarChar, OperatorID);
                    db.AddParameter("@ExChangeRate", System.Data.SqlDbType.Money, ExchangeRate);


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
                            throw new Exception("Asset No is already exists. Please retry or contact your administrator.");
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
        /// Untuk menghapus data FA Asset lebih dari satu kode
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
                    db.CommandText = "FAInsUpDelAsset";
                    db.Open();
                    db.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        string[] asset = data[i].Split(';');
                        
                        db.AddParameter("@Type", System.Data.SqlDbType.VarChar, 3);
                        db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, asset[0]);
                        db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, asset[1]);
                        db.AddParameter("@DeptCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@Location", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@ItemGrp", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@Part", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@TaxCatID", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@Desc", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@AssetVoucher", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@Vend", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@Qty", System.Data.SqlDbType.Float, DBNull.Value);
                        db.AddParameter("@Unit", System.Data.SqlDbType.Money, DBNull.Value);
                        db.AddParameter("@Resi", System.Data.SqlDbType.Money, DBNull.Value);
                        db.AddParameter("@Tot", System.Data.SqlDbType.Money, DBNull.Value);
                        db.AddParameter("@BaseTot", System.Data.SqlDbType.Money, DBNull.Value);
                        db.AddParameter("@Business", System.Data.SqlDbType.Float, DBNull.Value);
                        db.AddParameter("@Acq", System.Data.SqlDbType.DateTime, DBNull.Value);
                        db.AddParameter("@Start", System.Data.SqlDbType.DateTime, DBNull.Value);
                        db.AddParameter("@End", System.Data.SqlDbType.DateTime, DBNull.Value);
                        db.AddParameter("@Stat", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        db.AddParameter("@StatDate", System.Data.SqlDbType.DateTime, DBNull.Value);
                        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@OP", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@ExChangeRate", System.Data.SqlDbType.Money, DBNull.Value);
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
                            throw new Exception("Fixed asset code is already exists. Please retry or contact your administrator.");
                        case 547:
                            throw new Exception("One or more asset can not be delete while asset depreciation is journal.");
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

        public static List<System.Web.Mvc.SelectListItem> getFAAssetForDatasourceOPT(string branchCode)
        {
            List<System.Web.Mvc.SelectListItem> groups = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SelFAAsset";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
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
                            item.Value = Tool.GeneralHelper.NullToString(dr["AssetNo"]);
                            item.Text = Tool.GeneralHelper.NullToString(dr["Description"]);

                            groups.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }
            return groups;
        }

    }
}