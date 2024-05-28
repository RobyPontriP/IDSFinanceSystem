using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using IDS.Tool;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDS.FixedAsset
{
    public class FATrans
    {
        [Display(Name = "Trans No"), Required(AllowEmptyStrings = false, ErrorMessage = "Trans No is required")]
        public string TransNo { get; set; }

        [Display(Name = "Trans. Code"), Required(AllowEmptyStrings = false, ErrorMessage = "Transaction code is required")]
        public string TransCode { get; set; }

        [Display(Name = "Asset No"), Required(AllowEmptyStrings = false, ErrorMessage = "Asset No is required")]
        public string AssetNo { get; set; }

        [Display(Name = "Trans. Date"), Required(AllowEmptyStrings = false, ErrorMessage = "Transaction date is required")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "dd/MMM/yyyy", ApplyFormatInEditMode = true)]
        public DateTime TransDate { get; set; }

        [Display(Name = "Description"), Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "From Branch"), Required(AllowEmptyStrings = false, ErrorMessage = "Branch is required")]
        public string FromBranch { get; set; }

        #region Sold
        [RequiredIf("TransCode", "SO", ErrorMessage = "Currency is required")]
        [Display(Name = "Currency"), Required(AllowEmptyStrings = false, ErrorMessage = "Currency")]
        public string Currency { get; set; }

        [RequiredIf("TransCode", "SO", ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity"), Required(AllowEmptyStrings = false, ErrorMessage = "Quantity is required")]
        public decimal Qty { get; set; }

        [RequiredIf("TransCode", "SO", ErrorMessage = "Unit price is required")]
        [Display(Name = "Unit Price"), Required(AllowEmptyStrings = false, ErrorMessage = "Unit price is required")]
        public decimal UnitPrice { get; set; }

        [RequiredIf("TransCode", "SO", ErrorMessage = "Total price is required")]
        [Display(Name = "Total Price"), Required(AllowEmptyStrings = false, ErrorMessage = "Total price is required")]
        public decimal TotalPrice { get; set; }

        [RequiredIf("TransCode", "SO", ErrorMessage = "Base total price is required")]
        [Display(Name = "Base Total Price"), Required(AllowEmptyStrings = false, ErrorMessage = "Base total price is required")]
        public decimal BaseTotalPrice { get; set; }

        [RequiredIf("TransCode", "SO", ErrorMessage = "From Acc is required")]
        [Display(Name = "Account")]
        public string FromAcc { get; set; }

        [RequiredIf("TransCode", "SO", ErrorMessage = "Book value is required")]
        [Display(Name = "Book Value"), Required(AllowEmptyStrings = false, ErrorMessage = "Book value is required")]
        public decimal BookValue { get; set; }
        #endregion
        
        #region Move Transaction
        [RequiredIf("TransCode", "MO", ErrorMessage = "To Acc is required while trasaction is Move")]
        [Display(Name = "To Acc")]
        public string ToAcc { get; set; }

        [RequiredIf("TransCode", "MO", ErrorMessage = "To Branch is required while transaction is Move")]
        [Display(Name = "To Branch")]
        public string ToBranch { get; set; }
        #endregion

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Serial No")]
        public string SerialNo { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }
        
        [Display(Name = "Voucher From")]
        public string VoucherNoFrom { get; set; }

        [Display(Name = "Voucher To")]
        [RequiredIf("TransCode", 2, ErrorMessage = "Voucher To is required while transaction code is Move")]
        public string VoucherNoTo { get; set; }

        [Display(Name = "Cancelled Voucher")]
        public string CancelledVoucher { get; set; }

        [Display(Name = "Cancelled Voucher To")]
        public string CancelledVoucherTo { get; set; }

        [Display(Name = "Accum. Depreciation")]
        public decimal AccumDepre { get; set; }

        [Display(Name = "Move Out")]
        public int MoveOut { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [Display(Name = "Last Update")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "dd/MMM/yyyy", ApplyFormatInEditMode = true)]
        public DateTime LastUpdate { get; set; }

        [Display(Name = "Entry User")]
        public string EntryUser { get; set; }

        [Display(Name = "Entry Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = "dd/MMM/yyyy", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; }

        public FATrans()
        {

        }

        public static List<FATrans> GetFATrans(string branchCode)
        {
            List<FATrans> items = new List<FATrans>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFATrans";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TransNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        items = new List<FATrans>();

                        while (dr.Read())
                        {
                            FATrans item = new FATrans();
                            item.TransNo = Tool.GeneralHelper.NullToString(dr["TransNo"]);
                            item.TransCode = Tool.GeneralHelper.NullToString(dr["TransCode"]);
                            item.AssetNo = Tool.GeneralHelper.NullToString(dr["AssetNo"]);
                            item.Currency = Tool.GeneralHelper.NullToString(dr["Ccy"]);
                            item.Qty = Tool.GeneralHelper.NullToDecimal(dr["Qty"], 0);
                            item.UnitPrice = Tool.GeneralHelper.NullToDecimal(dr["Price"], 0);
                            item.TotalPrice = Tool.GeneralHelper.NullToDecimal(dr["TotalPrice"], 0);
                            item.BaseTotalPrice = Tool.GeneralHelper.NullToDecimal(dr["BaseTotPrice"], 0);
                            item.FromAcc = Tool.GeneralHelper.NullToString(dr["FrAcc"]);
                            item.ToAcc = Tool.GeneralHelper.NullToString(dr["ToAcc"]);
                            item.TransDate = Tool.GeneralHelper.NullToDateTime(dr["TransDate"], DateTime.Now.Date);
                            item.BookValue = Tool.GeneralHelper.NullToDecimal(dr["BookValue"], 0);
                            item.Description = Tool.GeneralHelper.NullToString(dr["Descript"]);
                            item.FromBranch = Tool.GeneralHelper.NullToString(dr["FromBranch"]);
                            item.ToBranch = Tool.GeneralHelper.NullToString(dr["ToBranch"]);
                            item.Department = Tool.GeneralHelper.NullToString(dr["DeptCode"]);
                            item.Location = Tool.GeneralHelper.NullToString(dr["Location"]);
                            item.SerialNo = Tool.GeneralHelper.NullToString(dr["SerialNo"]);
                            item.Status = Tool.GeneralHelper.NullToInt(dr["Status"], 0);

                            item.VoucherNoFrom = Tool.GeneralHelper.NullToString(dr["VoucherNoFrom"]);
                            item.VoucherNoTo = Tool.GeneralHelper.NullToString(dr["VoucherNoTo"]);
                            item.CancelledVoucher = Tool.GeneralHelper.NullToString(dr["CancelledVoucher"]);
                            item.CancelledVoucherTo = Tool.GeneralHelper.NullToString(dr["CancelledVoucherTo"]);
                            item.AccumDepre = Tool.GeneralHelper.NullToDecimal(dr["AccumDepr"], 0);
                            item.MoveOut = Tool.GeneralHelper.NullToInt(dr["MoveOut"], 0);
                            item.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            item.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now.Date);
                            item.EntryUser = Tool.GeneralHelper.NullToString(dr["ENTRYUSER"]);
                            item.EntryDate = Tool.GeneralHelper.NullToDateTime(dr["ENTRYDATE"], DateTime.Now.Date);

                            items.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return items;
        }

        public static FATrans GetFATrans(string branchCode, string transNo)
        {
            FATrans item = new FATrans();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFATrans";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TransNo", System.Data.SqlDbType.VarChar, transNo);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        
                        item.TransNo = Tool.GeneralHelper.NullToString(dr["TransNo"]);
                        item.TransCode = Tool.GeneralHelper.NullToString(dr["TransCode"]);
                        item.AssetNo = Tool.GeneralHelper.NullToString(dr["AssetNo"]);
                        item.Currency = Tool.GeneralHelper.NullToString(dr["Ccy"]);
                        item.Qty = Tool.GeneralHelper.NullToDecimal(dr["Qty"], 0);
                        item.UnitPrice = Tool.GeneralHelper.NullToDecimal(dr["Price"], 0);
                        item.TotalPrice = Tool.GeneralHelper.NullToDecimal(dr["TotalPrice"], 0);
                        item.BaseTotalPrice = Tool.GeneralHelper.NullToDecimal(dr["BaseTotPrice"], 0);
                        item.FromAcc = Tool.GeneralHelper.NullToString(dr["FrAcc"]);
                        item.ToAcc = Tool.GeneralHelper.NullToString(dr["ToAcc"]);
                        item.TransDate = Tool.GeneralHelper.NullToDateTime(dr["TransDate"], DateTime.Now.Date);
                        item.BookValue = Tool.GeneralHelper.NullToDecimal(dr["BookValue"], 0);
                        item.Description = Tool.GeneralHelper.NullToString(dr["Descript"]);
                        item.FromBranch = Tool.GeneralHelper.NullToString(dr["FromBranch"]);
                        item.ToBranch = Tool.GeneralHelper.NullToString(dr["ToBranch"]);
                        item.Department = Tool.GeneralHelper.NullToString(dr["DeptCode"]);
                        item.Location = Tool.GeneralHelper.NullToString(dr["Location"]);
                        item.SerialNo = Tool.GeneralHelper.NullToString(dr["SerialNo"]);
                        item.Status = Tool.GeneralHelper.NullToInt(dr["Status"], 0);

                        item.VoucherNoFrom = Tool.GeneralHelper.NullToString(dr["VoucherNoFrom"]);
                        item.VoucherNoTo = Tool.GeneralHelper.NullToString(dr["VoucherNoTo"]);
                        item.CancelledVoucher = Tool.GeneralHelper.NullToString(dr["CancelledVoucher"]);
                        item.CancelledVoucherTo = Tool.GeneralHelper.NullToString(dr["CancelledVoucherTo"]);
                        item.AccumDepre = Tool.GeneralHelper.NullToDecimal(dr["AccumDepr"], 0);
                        item.MoveOut = Tool.GeneralHelper.NullToInt(dr["MoveOut"], 0);
                        item.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                        item.LastUpdate = Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now.Date);
                        item.EntryUser = Tool.GeneralHelper.NullToString(dr["ENTRYUSER"]);
                        item.EntryDate = Tool.GeneralHelper.NullToDateTime(dr["ENTRYDATE"], DateTime.Now.Date);

                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return item;
        }

        public static int GetFATransStatus(string branchCode, string transNo)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFATrans";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TransNo", System.Data.SqlDbType.VarChar, transNo);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                result = Tool.GeneralHelper.NullToInt(db.ExecuteScalar(), 0);

                db.Close();
            }

            return result;
        }

        public static List<SelectListItem> GetFATransCodeList()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem() { Value = "MO", Text = "Move" },
                new SelectListItem() { Value = "SO", Text = "Sold" },
                new SelectListItem() { Value = "WO", Text = "Write Off" }
            };
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
                        if (TransDate != null && TransDate != DateTime.MinValue && !string.IsNullOrWhiteSpace(FromBranch) &&
                            !string.IsNullOrWhiteSpace(TransCode))
                        {
                            db.CommandText = "SELECT ISNULL(RIGHT(LEFT(MAX(TransNo), 5), 3), 0) AS Res FROM FATrans WHERE CONVERT(VARCHAR(6),TransDate,112) = @year AND FromBranch = @branch AND TransCode=@transCode";
                            db.AddParameter("@year", System.Data.SqlDbType.VarChar, TransDate.ToPeriod());
                            db.AddParameter("@branch", System.Data.SqlDbType.VarChar, FromBranch);
                            db.AddParameter("@transCode", System.Data.SqlDbType.VarChar, TransCode);
                            db.CommandType = System.Data.CommandType.Text;
                            db.Open();

                            TransNo = Tool.GeneralHelper.NullToString(db.ExecuteScalar());
                            TransNo = (Convert.ToInt32(TransNo) + 1).ToString("000");
                            TransNo = TransCode + TransNo + "/" + TransDate.ToString("yyMM") + "/" + FromBranch;
                        }
                        else
                        {
                            throw new Exception("Failed to generate new transaction number. Please re-login and retry. If problem occured, please contact your administrator.");
                        }
                    }

                    db.CommandText = "FAInsUpDelAssetTrans";
                    db.AddParameter("@ExecCode", System.Data.SqlDbType.TinyInt, ExecCode);
                    db.AddParameter("@TransNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(TransNo));
                    db.AddParameter("@TransCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(TransCode));
                    db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(AssetNo));
                    db.AddParameter("@Ccy", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Currency));
                    db.AddParameter("@Qty", System.Data.SqlDbType.Float, Tool.GeneralHelper.NullToDecimal(Qty, 0));
                    db.AddParameter("@Price", System.Data.SqlDbType.Money, Tool.GeneralHelper.NullToDecimal(UnitPrice, 0));
                    db.AddParameter("@TotPric", System.Data.SqlDbType.Money, Tool.GeneralHelper.NullToDecimal(TotalPrice, 0));
                    db.AddParameter("@BaseTotPric", System.Data.SqlDbType.Money, Tool.GeneralHelper.NullToDecimal(BaseTotalPrice, 0));
                    db.AddParameter("@FrAcc", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(FromAcc));
                    db.AddParameter("@TransDate", System.Data.SqlDbType.DateTime, Tool.GeneralHelper.NullToDateTime(TransDate, DateTime.Now.Date));
                    db.AddParameter("@BookValue", System.Data.SqlDbType.Money, Tool.GeneralHelper.NullToDecimal(BookValue, 0));
                    db.AddParameter("@Descript", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Description));
                    db.AddParameter("@DeptCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Department));
                    db.AddParameter("@Location", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Location));
                    db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(SerialNo));
                    db.AddParameter("@Status", System.Data.SqlDbType.VarChar, Status);
                    db.AddParameter("@FromBranch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(FromBranch));
                    db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(OperatorID));
                    db.AddParameter("@AccumDepr", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.NullToDecimal(AccumDepre, 0));

                    // Move
                    db.AddParameter("@ToBranch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(ToBranch));
                    db.AddParameter("@ToAcc", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(ToAcc));
                    
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
                            throw new Exception("Asset transaction No is already exists. Please retry or contact your administrator.");
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
                    db.Open();
                    db.BeginTransaction();

                    int processed = 0;

                    for (int i = 0; i < data.Length; i++)
                    {
                        string[] trans = data[i].Split(';');

                        db.CommandText = "SelFATrans";
                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.AddParameter("@TransNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(trans[0]));
                        db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, trans[1]);
                        db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);

                        processed = Tool.GeneralHelper.NullToInt(db.ExecuteScalar(), 0);

                        if (processed > 0)
                        {
                            throw new Exception("One or more fixed asset transaction are processed or cancelled, then can not be delete. Delete process will be terminated");
                        }

                        db.CommandText = "FAInsUpDelAssetTrans";
                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.AddParameter("@ExecCode", System.Data.SqlDbType.TinyInt, 3);
                        db.AddParameter("@TransNo", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(trans[0]));
                        db.AddParameter("@TransCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@AssetNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@Ccy", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@Qty", System.Data.SqlDbType.Float, DBNull.Value);
                        db.AddParameter("@Price", System.Data.SqlDbType.Money, DBNull.Value);
                        db.AddParameter("@TotPric", System.Data.SqlDbType.Money, DBNull.Value);
                        db.AddParameter("@BaseTotPric", System.Data.SqlDbType.Money, DBNull.Value);
                        db.AddParameter("@FrAcc", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@TransDate", System.Data.SqlDbType.DateTime, DBNull.Value);
                        db.AddParameter("@BookValue", System.Data.SqlDbType.Money, DBNull.Value);
                        db.AddParameter("@Descript", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DeptCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@Location", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@Status", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@FromBranch", System.Data.SqlDbType.VarChar, trans[1]);
                        db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                        db.AddParameter("@AccumDepr", System.Data.SqlDbType.VarChar, DBNull.Value);

                        // Move
                        db.AddParameter("@ToBranch", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@ToAcc", System.Data.SqlDbType.VarChar, DBNull.Value);
                        

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
    }
}