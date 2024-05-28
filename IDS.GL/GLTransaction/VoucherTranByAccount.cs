using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTransaction
{
    public class VoucherTranByAccount
    {
        public string SCode { get; set; }
        public string Voucher { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransDate { get; set; }
        public string Account { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public double Debet { get; set; }
        public double Credit { get; set; }

        public VoucherTranByAccount()
        {
        }

        public static List<VoucherTranByAccount> GetVoucherTransByAccount(string period, string branchCode, string account)
        {
            List<VoucherTranByAccount> items = new List<VoucherTranByAccount>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelVoucherHeader";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@VoucherNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@SCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@Account", System.Data.SqlDbType.VarChar, account);
                db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 7);

                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while(dr.Read())
                        {
                            VoucherTranByAccount v = new VoucherTranByAccount();
                            v.SCode = Tool.GeneralHelper.NullToString(dr["SCODE"]);
                            v.Voucher = Tool.GeneralHelper.NullToString(dr["VOUCHER"]);
                            v.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);
                            v.TransDate = Convert.ToDateTime(dr["TRANS_DATE"]);
                            v.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);
                            v.Currency = Tool.GeneralHelper.NullToString(dr["CCY"]);
                            v.Description = Tool.GeneralHelper.NullToString(dr["DESCRIP"]);

                            if (dr["AMOUNT"] == System.DBNull.Value)
                            {
                                v.Debet = 0;
                                v.Credit = 0;
                            }
                            else
                            {
                                double amount = Convert.ToDouble(dr["AMOUNT"]);

                                if (amount >= 0)
                                {
                                    v.Debet = Tool.GeneralHelper.NullToDouble(dr["AMOUNT"], 0);
                                    v.Credit = 0;
                                }
                                else
                                {
                                    v.Debet = 0;
                                    v.Credit = Tool.GeneralHelper.NullToDouble(dr["AMOUNT"], 0);
                                }
                            }

                            items.Add(v);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return items;
        }
    }
}