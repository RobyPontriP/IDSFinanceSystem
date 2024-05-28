using IDS.GLTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTransaction
{
    public class GLVoucherD
    {
        public GLTable.SourceCode SCode { get; set; }
        public string Voucher { get; set; }
        public int Counter { get; set; }
        public GeneralTable.Branch VBranch { get; set; }
        public GeneralTable.Currency CCy { get; set; }
        public GLTable.ChartOfAccount COA { get; set; }
        public string CashAccount { get; set; }
        public GeneralTable.Department Dept { get; set; }
        public GLTable.Project Proj { get; set; }
        public string DocumentNo { get; set; }
        public string Descrip { get; set; }
        public string CustSupp { get; set; }
        public double Amount { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string UPD { get; set; }
        public string InstrumentNo { get; set; }
        public bool MatchStatus { get; set; }

        public GLVoucherD()
        {
        }

        public static List<GLVoucherD> GetVoucherDetailByAccount(string accNo, string branchCode, string period)
        {
            List<GLVoucherD> items = new List<GLVoucherD>();

            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "GLSelVoucherDetail";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@VoucherNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@SCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 0);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        items = new List<GLVoucherD>();

                        while (dr.Read())
                        {
                            GLVoucherD item = new GLVoucherD();
                            item.SCode = new GLTable.SourceCode();
                            item.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCode"]);

                            item.Voucher = IDS.Tool.GeneralHelper.NullToString(dr["Voucher"]);
                            item.Counter = Convert.ToInt32(dr["COUNTER"]);

                            item.COA = new GLTable.ChartOfAccount();
                            item.COA.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            item.VBranch = new IDS.GeneralTable.Branch();
                            item.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            item.CCy = new GeneralTable.Currency();
                            item.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["Code"]);

                            item.CashAccount = Tool.GeneralHelper.NullToString(dr["CASHACC"]);

                            item.Dept = new IDS.GeneralTable.Department();
                            item.Dept.DepartmentCode = Tool.GeneralHelper.NullToString(dr["DEPT"]);

                            item.Proj = new IDS.GLTable.Project();
                            item.Proj.ProjectCode = Tool.GeneralHelper.NullToString(dr["PROJ"]);

                            item.DocumentNo = IDS.Tool.GeneralHelper.NullToString(dr["DOC_NO"]);
                            item.Descrip = Tool.GeneralHelper.NullToString(dr["DESCRIP"]);
                            item.CustSupp = IDS.Tool.GeneralHelper.NullToString(dr["C_S"]);
                            item.Amount = Convert.ToDouble(dr["AMOUNT"]);
                            item.UPD = IDS.Tool.GeneralHelper.NullToString(dr["UPD"]);
                            item.InstrumentNo = IDS.Tool.GeneralHelper.NullToString(dr["INSTRUMENT_NO"]);
                            item.MatchStatus = Tool.GeneralHelper.NullToBool(dr["MATCHSTATUS"], false);

                            items.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return items;
        }

        public static List<GLVoucherD> GetVoucherDetail(string branch, string scode, string voucher, int execCode)
        {
            List<GLVoucherD> voucherD = new List<GLVoucherD>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLTransView";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@scode", System.Data.SqlDbType.VarChar, scode);
                db.AddParameter("@voucher", System.Data.SqlDbType.VarChar, voucher);
                db.AddParameter("@ARAP", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@dtFrom", System.Data.SqlDbType.DateTime, DBNull.Value);
                db.AddParameter("@dtTo", System.Data.SqlDbType.DateTime, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, execCode);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GLVoucherD vd = new GLVoucherD();
                            vd.SCode = new GLTable.SourceCode();
                            vd.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCODE"]);
                            vd.Voucher = Tool.GeneralHelper.NullToString(dr["VOUCHER"]);
                            vd.Counter = Tool.GeneralHelper.NullToInt(dr["COUNTER"], 0);

                            vd.VBranch = new GeneralTable.Branch();
                            vd.VBranch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                            vd.COA = new GLTable.ChartOfAccount();
                            vd.COA.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            vd.CCy = new GeneralTable.Currency();
                            vd.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            vd.CashAccount = Tool.GeneralHelper.NullToString(dr["CASHACC"]);

                            vd.Dept = new GeneralTable.Department();
                            vd.Dept.DepartmentCode = Tool.GeneralHelper.NullToString(dr["DEPT"]);

                            vd.Proj = new GLTable.Project();
                            vd.Proj.ProjectCode = Tool.GeneralHelper.NullToString(dr["PROJ"]);

                            vd.DocumentNo = Tool.GeneralHelper.NullToString(dr["DOC_NO"]);
                            vd.Descrip = Tool.GeneralHelper.NullToString(dr["DESCRIP"]);
                            vd.CustSupp = Tool.GeneralHelper.NullToString(dr["C_S"]);
                            vd.Amount = Convert.ToDouble(dr["AMOUNT"]);
                            vd.Debit = IDS.Tool.GeneralHelper.NullToDecimal(dr["Debit"], 0);
                            vd.Credit = IDS.Tool.GeneralHelper.NullToDecimal(dr["Credit"], 0);
                            vd.UPD = Tool.GeneralHelper.NullToString(dr["UPD"]);
                            vd.InstrumentNo = Tool.GeneralHelper.NullToString(dr["INSTRUMENT_NO"]);
                            vd.MatchStatus = Tool.GeneralHelper.NullToBool(dr["MATCHSTATUS"], false);

                            voucherD.Add(vd);
                        }
                    }

                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }

                db.Close();
            }

            return voucherD;
        }

        public static bool CheckBalanceNewVoucher(string scode, string voucher, string branch, IDS.DataAccess.SqlServer cmd)
        {
            bool result = false;

            cmd.CommandText = "select sum(amount) as amount from ACFTRAND where SCODE =@scode and VOUCHER = @voucher and BranchCode = @branch";
            cmd.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, scode);
            cmd.AddParameter("@voucher", System.Data.SqlDbType.VarChar, voucher);
            cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Open();

            cmd.ExecuteReader();

            using (System.Data.SqlClient.SqlDataReader dr = cmd.DbDataReader as System.Data.SqlClient.SqlDataReader)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (Tool.GeneralHelper.NullToDecimal(dr["amount"], 0) == 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                if (!dr.IsClosed)
                    dr.Close();

                return result;
            }

            //public static List<GLVoucherD> GetVoucherDetail(string invNo)
            //{
            //    List<IDS.Sales.InvoiceDetail> list = new List<InvoiceDetail>();

            //    using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            //    {
            //        db.CommandText = "SP_GetDetailSalesInvoice";
            //        db.CommandType = System.Data.CommandType.StoredProcedure;
            //        db.AddParameter("@InvoiceNumber", System.Data.SqlDbType.VarChar, invNo);
            //        db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
            //        db.Open();

            //        db.ExecuteReader();

            //        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
            //        {
            //            if (dr.HasRows)
            //            {

            //                while (dr.Read())
            //                {
            //                    InvoiceDetail invoiceDetail = new InvoiceDetail();
            //                    invoiceDetail.InvoiceNumber = dr["InvoiceNumber"] as string;
            //                    invoiceDetail.Counter = Tool.GeneralHelper.NullToInt(dr["Counter"], 0);
            //                    invoiceDetail.SubCounter = Tool.GeneralHelper.NullToInt(dr["SubCounter"], 0);
            //                    invoiceDetail.SubAmount = Tool.GeneralHelper.NullToString(dr["SubAmount"], "");
            //                    invoiceDetail.Remark = Tool.GeneralHelper.NullToString(dr["Remark"], "");
            //                    invoiceDetail.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
            //                    invoiceDetail.TaxInvoice = Tool.GeneralHelper.NullToBool(dr["TaxInvoice"]);

            //                    list.Add(invoiceDetail);
            //                }
            //            }

            //            if (!dr.IsClosed)
            //                dr.Close();
            //        }

            //        db.Close();
            //    }

            //    return list;
            //}
        }
    }
}