using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDS.Tool;
using System.ComponentModel.DataAnnotations;

namespace IDS.GLTransaction
{
    public class CashBankD
    {
        public string CashBankNumber { get; set; }
        public int Counter { get; set; }
        public int SubCounter { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }

        public CashBankD()
        {

        }

        public static List<CashBankD> GetCashBankD(string cbNo)
        {
            List<CashBankD> cbdList = new List<CashBankD>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelCashBankD";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@cbNo", System.Data.SqlDbType.VarChar, cbNo);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CashBankD cbD = new CashBankD();
                            cbD.CashBankNumber = Tool.GeneralHelper.NullToString(dr["CashBankNumber"],"");
                            cbD.Counter = Tool.GeneralHelper.NullToInt(dr["Counter"], 0);
                            cbD.SubCounter = Tool.GeneralHelper.NullToInt(dr["SubCounter"], 0);
                            cbD.Type = Tool.GeneralHelper.NullToInt(dr["Type"],0);
                            cbD.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            cbD.Remark = Tool.GeneralHelper.NullToString(dr["Remark"],"");
                            cbdList.Add(cbD);
                        }
                    }

                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }

                db.Close();
            }

            return cbdList;
        }

        public static List<System.Web.Mvc.SelectListItem> GetTypeCBD()
        {
            List<System.Web.Mvc.SelectListItem> cbdType = new List<System.Web.Mvc.SelectListItem>();
            cbdType.Add(new System.Web.Mvc.SelectListItem() { Text = "Invoice", Value = "1" });
            cbdType.Add(new System.Web.Mvc.SelectListItem() { Text = "PPN", Value = "2" });
            cbdType.Add(new System.Web.Mvc.SelectListItem() { Text = "PPh21", Value = "3" });
            cbdType.Add(new System.Web.Mvc.SelectListItem() { Text = "PPh23", Value = "4" });
            cbdType.Add(new System.Web.Mvc.SelectListItem() { Text = "PPh 4 Ayat 2", Value = "5" });
            cbdType.Add(new System.Web.Mvc.SelectListItem() { Text = "Bank Charges", Value = "6" });

            return cbdType;
        }

        public static decimal GetBankCharges(string cbNo)
        {
            decimal result = 0;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelCashBankD";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@cbNo", System.Data.SqlDbType.VarChar, cbNo);
                db.AddParameter("@init", System.Data.SqlDbType.Int, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return result;
        }
    }
}
