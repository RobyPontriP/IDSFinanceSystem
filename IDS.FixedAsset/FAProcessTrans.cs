using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.FixedAsset
{
   public  class FAProcessTrans
    {
        public string TransNo { get; set; }
        public string TransCode { get; set; }
        public string AssetNo { get; set; }
        public string Ccy { get; set; }
        public string Qty { get; set; }
        public string Price { get; set; }
        public string TotalPrice { get; set; }
        public string BaseTotPrice { get; set; }
        public string FrAcc { get; set; }
        public string ToAcc { get; set; }
        public string TransDate { get; set; }
        public string TransEntryDate { get; set; }
        public string BookValue { get; set; }
        public string Descript { get; set; }
        public string FromBranch { get; set; }
        public string ToBranch { get; set; }
        public string DeptCode { get; set; }
        public string Location { get; set; }
        public string SerialNo { get; set; }
        public string Status { get; set; }
        public string VoucherNoFrom { get; set; }
        public string VoucherNoTo { get; set; }
        public string CancelledVoucher { get; set; }
        public string CancelledVoucherTo { get; set; }
        public string AccumDepr { get; set; }
        public string MoveOut { get; set; }
        public string OperatorID { get; set; }
        public string LastUpdate { get; set; }
        public string ENTRYUSER { get; set; }
        public string ENTRYDATE { get; set; }
        public FAProcessTrans()       {     }

        public static List<System.Web.Mvc.SelectListItem> getFATransForDatasource(string branch)
        {
            List<System.Web.Mvc.SelectListItem> groups = new List<System.Web.Mvc.SelectListItem>();
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "selFATrans";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Branch", System.Data.SqlDbType.VarChar,branch);
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 1);
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem item = new System.Web.Mvc.SelectListItem();
                            item.Value = Tool.GeneralHelper.NullToString(dr["TransNo"]);
                            item.Text = Tool.GeneralHelper.NullToString(dr["Descript"]);
                            groups.Add(item);
                        }
                    }
                    if (!dr.IsClosed) dr.Close();
                }
                db.Close();
            }
            return groups;
        }//getFATransForDatasource

        public static List<IDS.FixedAsset.FAProcessTrans> getFAProcessTrans(string branch)
        {
            List<IDS.FixedAsset.FAProcessTrans> list = new List<IDS.FixedAsset.FAProcessTrans>();
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "selFATrans";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            IDS.FixedAsset.FAProcessTrans item = new IDS.FixedAsset.FAProcessTrans()
                            {
                                 AccumDepr=dr["AccumDepr"].ToString(),
                                  AssetNo=dr["AssetNo"].ToString(),
                                   BaseTotPrice= dr["BaseTotPrice"].ToString(),
                                    BookValue= dr["BookValue"].ToString(),
                                     CancelledVoucher= dr["CancelledVoucher"].ToString(),
                                      CancelledVoucherTo= dr["CancelledVoucherTo"].ToString(),
                                        Ccy= dr["Ccy"].ToString(), 
                                         DeptCode= dr["DeptCode"].ToString(),
                                          Descript= dr["Descript"].ToString(),
                                           ENTRYDATE= dr["ENTRYDATE"].ToString(),
                                            ENTRYUSER= dr["ENTRYUSER"].ToString(),
                                             FrAcc= dr["FrAcc"].ToString(),
                                              FromBranch= dr["FromBranch"].ToString(),
                                               LastUpdate= dr["LastUpdate"].ToString(),
                                                Location= dr["Location"].ToString(),
                                                 MoveOut= dr["MoveOut"].ToString(),
                                                  OperatorID= dr["OperatorID"].ToString(),
                                                   Price= dr["Price"].ToString(),
                                                    Qty= dr["Qty"].ToString(),
                                                     SerialNo= dr["SerialNo"].ToString(),
                                                      Status= dr["Status"].ToString(),
                                                       ToAcc= dr["ToAcc"].ToString(),
                                                        ToBranch= dr["ToBranch"].ToString(),
                                                         TotalPrice= dr["TotalPrice"].ToString(),
                                                          TransCode= dr["TransCode"].ToString(),
                                                           TransDate= dr["TransDate"].ToString(),
                                                            TransEntryDate= dr["TransEntryDate"].ToString(),
                                                             TransNo= dr["TransNo"].ToString(),
                                                              VoucherNoFrom= dr["VoucherNoFrom"].ToString(),
                                                               VoucherNoTo= dr["VoucherNoTo"].ToString()

                            };
                            list.Add(item);
                        }
                    }
                    if (!dr.IsClosed) dr.Close();
                }
                db.Close();
            }
            return list;
        }//getFATransForDatasource

        public static string GoProsessTrans(string TransNo,string FromBranch,string OperatorID,DateTime LastUpdate,string Message)
        {
            string return_ = "";
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
               try
                {
                    db.CommandText = "FATransProc";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@TransNo", System.Data.SqlDbType.VarChar, TransNo);
                    db.AddParameter("@FromBranch", System.Data.SqlDbType.VarChar, FromBranch);
                    db.AddParameter("@LastUpdate", System.Data.SqlDbType.DateTime, LastUpdate);
                    db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                    db.AddParameter("@Message", System.Data.SqlDbType.VarChar, Message);
                    db.Open();
                    db.ExecuteReader();
                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            return_ = dr[0].ToString();
                        }

                        if (!dr.IsClosed) dr.Close();
                    }
                    db.Close();
                }
                catch (Exception msg)
                {
                    System.Diagnostics.Debug.WriteLine(msg);
                }
            }
           return return_;
        }// GoProsessTrans

        public static string GoProsessCancell(string TransNo, string FromBranch, string OperatorID, DateTime LastUpdate)
        {
            string return_ = "";
            
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "FATransCancel";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TransNo", System.Data.SqlDbType.VarChar, TransNo);
                db.AddParameter("@FromBranch", System.Data.SqlDbType.VarChar, FromBranch);
                db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                db.AddParameter("@LastUpdate", System.Data.SqlDbType.DateTime, LastUpdate);
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        return_ = dr[0].ToString();
                    }
                    if (!dr.IsClosed) dr.Close();
                }
                db.Close();

            }
            return return_;
        }// GoProsessTrans

        public static string getStatus(string branch,string TransNo)
        {
            string return_ = "";
            System.Text.StringBuilder b = new StringBuilder();
            b.AppendLine("SELECT [status] from FATrans WHERE TransNo = '"+TransNo+ "' AND FromBranch = '" + branch + "'");
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = b.ToString();
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        return_ = dr["Status"].ToString();
                    }
                    if (!dr.IsClosed) dr.Close();
                }
                db.Close();
            }
            return return_;
        }

        public static IDS.FixedAsset.FAProcessTrans GetDataTRx(string branch, string TransNo)
        {
            IDS.FixedAsset.FAProcessTrans xlist = new IDS.FixedAsset.FAProcessTrans();
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "selFATrans";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@TransNo", System.Data.SqlDbType.VarChar, TransNo);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        xlist.AccumDepr = dr["AccumDepr"].ToString();
                        xlist.AssetNo = dr["AssetNo"].ToString();
                        xlist.BaseTotPrice = dr["BaseTotPrice"].ToString();
                        xlist.BookValue = dr["BookValue"].ToString();
                        xlist.CancelledVoucher = dr["CancelledVoucher"].ToString();
                        xlist.CancelledVoucherTo = dr["CancelledVoucherTo"].ToString();
                        xlist.Ccy = dr["Ccy"].ToString();
                        xlist.DeptCode = dr["DeptCode"].ToString();
                        xlist.Descript = dr["Descript"].ToString();
                        xlist.ENTRYDATE = dr["ENTRYDATE"].ToString();
                        xlist.ENTRYUSER = dr["ENTRYUSER"].ToString();
                        xlist.FrAcc = dr["FrAcc"].ToString();
                        xlist.FromBranch = dr["FromBranch"].ToString();
                        xlist.LastUpdate = dr["LastUpdate"].ToString();
                        xlist.Location = dr["Location"].ToString();
                        xlist.MoveOut = dr["MoveOut"].ToString();
                        xlist.OperatorID = dr["OperatorID"].ToString();
                        xlist.Price = dr["Price"].ToString();
                        xlist.Qty = dr["Qty"].ToString();
                        xlist.SerialNo = dr["SerialNo"].ToString();
                        xlist.Status = dr["Status"].ToString();
                        xlist.ToAcc = dr["ToAcc"].ToString();
                        xlist.ToBranch = dr["ToBranch"].ToString();
                        xlist.TotalPrice = dr["TotalPrice"].ToString();
                        xlist.TransCode = dr["TransCode"].ToString();
                        xlist.TransDate = dr["TransDate"].ToString();
                        xlist.TransEntryDate = dr["TransEntryDate"].ToString();
                        xlist.TransNo = dr["TransNo"].ToString();
                        xlist.VoucherNoFrom = dr["VoucherNoFrom"].ToString();
                        xlist.VoucherNoTo = dr["VoucherNoTo"].ToString();
                    }
                    if (!dr.IsClosed) dr.Close();
                }
                db.Close();
            }
            return xlist;
        }//getFATransForDatasource


    }
}
