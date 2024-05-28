using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class SUBACFARAP
    {
        [Display(Name = "RP")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RP")]
        [MaxLength(1)]
        public string RP { get; set; }

        [Display(Name = "Charts of Account")]
        public IDS.GLTable.ChartOfAccount Acc { get; set; }

        //[Display(Name = "Currency Code")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        //public IDS.GeneralTable.Currency CCy { get; set; }

        [Display(Name = "Currency Code")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        public IDS.GeneralTable.Currency CCySUBACFARAP { get; set; }

        [Display(Name = "Customer Prin")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer Prin")]
        [MaxLength(20)]
        public string CustPrin { get; set; }

        public string SupCode { get; set; }

        [Display(Name = "Doc No")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Doc No")]
        [MaxLength(15)]
        public string DocNo { get; set; }

        [Display(Name = "Branch Name")]
        public IDS.GeneralTable.Branch Branch { get; set; }

        [Display(Name = "Seq No")]
        public int SeqNo { get; set; }

        [Display(Name = "Dept Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Dept Code")]
        [MaxLength(3)]
        public string DeptCode { get; set; }

        [Display(Name = "Sub Charts of Account")]
        public IDS.GLTable.ChartOfAccount SubAcc { get; set; }

        [Display(Name = "Sub Amount")]
        public decimal SubAmount { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Equiv Amount")]
        public decimal EquivAmt { get; set; }

        public SUBACFARAP()
        {

        }

        public static List<SUBACFARAP> GetSUBACFARAP(string docno, string custprin, string ccy, string branchcode)
        {
            List<IDS.GLTable.SUBACFARAP> list = new List<SUBACFARAP>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSUBACFARAP";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@docno", System.Data.SqlDbType.VarChar, docno);
                db.AddParameter("@custprin", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(custprin));
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@branchcode", System.Data.SqlDbType.VarChar, branchcode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            SUBACFARAP subacfarap = new SUBACFARAP();
                            subacfarap.RP = dr["RP"] as string;

                            subacfarap.Acc = new GLTable.ChartOfAccount();
                            subacfarap.Acc.Account = Tool.GeneralHelper.NullToString(dr["subacfarapacc"]);
                            subacfarap.Acc.AccountName = Tool.GeneralHelper.NullToString(dr["AccName"]);
                            //subacfarap.Acc = new GLTable.ChartOfAccount();
                            //subacfarap.Acc = ChartOfAccount.GetCOA(Tool.GeneralHelper.NullToString(dr["ACC"]), Tool.GeneralHelper.NullToString(dr["CCY"]));

                            subacfarap.CCySUBACFARAP = new GeneralTable.Currency();
                            subacfarap.CCySUBACFARAP.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            subacfarap.CustPrin = dr["CUST_PRIN"] as string;

                            subacfarap.DocNo = dr["DOC_NO"] as string;
                            subacfarap.SeqNo = Tool.GeneralHelper.NullToInt(dr["SEQNO"],0);

                            subacfarap.Branch = new GeneralTable.Branch();
                            subacfarap.Branch.BranchCode = dr["branchcode"] as string;
                            
                            subacfarap.DeptCode = dr["DEPTCODE"] as string;

                            subacfarap.SubAcc = new GLTable.ChartOfAccount();
                            subacfarap.Acc.Account = Tool.GeneralHelper.NullToString(dr["SUBACC"]);
                            
                            subacfarap.SubAmount = Tool.GeneralHelper.NullToDecimal(dr["SUBAMT"], 0);
                            subacfarap.Description = dr["Description"] as string;
                            subacfarap.EquivAmt= Tool.GeneralHelper.NullToDecimal(dr["EquivAmt"], 0);

                            list.Add(subacfarap);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<SUBACFARAP> GetSUBACFARAP(string docno)
        {
            List<IDS.GLTable.SUBACFARAP> list = new List<SUBACFARAP>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelSUBACFARAP";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@docno", System.Data.SqlDbType.VarChar, docno);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            SUBACFARAP subacfarap = new SUBACFARAP();
                            subacfarap.RP = dr["RP"] as string;

                            subacfarap.Acc = new GLTable.ChartOfAccount();
                            subacfarap.Acc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            subacfarap.CCySUBACFARAP = new GeneralTable.Currency();
                            subacfarap.CCySUBACFARAP.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            subacfarap.CustPrin = dr["CUST_PRIN"] as string;

                            subacfarap.DocNo = dr["DOC_NO"] as string;
                            subacfarap.SeqNo = Tool.GeneralHelper.NullToInt(dr["SEQNO"], 0);

                            subacfarap.Branch = new GeneralTable.Branch();
                            subacfarap.Branch.BranchCode = dr["branchcode"] as string;

                            subacfarap.DeptCode = dr["DEPTCODE"] as string;

                            subacfarap.SubAcc = new GLTable.ChartOfAccount();
                            subacfarap.Acc.Account = Tool.GeneralHelper.NullToString(dr["SUBACC"]);

                            subacfarap.SubAmount = Tool.GeneralHelper.NullToDecimal(dr["SUBAMT"], 0);
                            subacfarap.Description = dr["Description"] as string;
                            subacfarap.EquivAmt = Tool.GeneralHelper.NullToDecimal(dr["EquivAmt"], 0);

                            list.Add(subacfarap);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public int DelSUBACFARAP(int ExecCode, IDS.DataAccess.SqlServer cmd)
        {
            int result = 0;

            try
            {
                cmd.CommandText = "UpdSUBACFARAP";
                cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, RP);
                cmd.AddParameter("@docNo", System.Data.SqlDbType.VarChar, DocNo);
                cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, CustPrin);
                cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                cmd.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCySUBACFARAP.CurrencyCode);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sex)
            {
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();

                switch (sex.Number)
                {
                    case 2627:
                        throw new Exception("SUBACFARAP is already exists. Please choose other SUBACFARAP.");
                    case 547:
                        throw new Exception("One or more data can not be delete while data used for reference.");
                    default:
                        throw;
                }
            }
            catch
            {
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();

                throw;
            }

            return result;
        }

        public int InsUpSUBACFARAP(int ExecCode, IDS.DataAccess.SqlServer cmd)
        {
            int result = 0;

            try
            {
                cmd.CommandText = "ARAPSaveDetail";
                cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);
                cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, RP);
                cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Acc.Account);
                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CCySUBACFARAP.CurrencyCode);
                cmd.AddParameter("@CUST_PRIN", System.Data.SqlDbType.VarChar, CustPrin);
                cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DocNo);
                cmd.AddParameter("@SeqNo", System.Data.SqlDbType.Int, SeqNo);
                cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                cmd.AddParameter("@DEPTCODE", System.Data.SqlDbType.VarChar, DeptCode);
                cmd.AddParameter("@SUBACC", System.Data.SqlDbType.VarChar, SubAcc.Account);
                cmd.AddParameter("@SUBAMT", System.Data.SqlDbType.Money, SubAmount);
                cmd.AddParameter("@Description", System.Data.SqlDbType.VarChar, Description);
                cmd.AddParameter("@EQUIVAMT", System.Data.SqlDbType.Money, EquivAmt);
                cmd.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException sex)
            {
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();

                switch (sex.Number)
                {
                    case 2627:
                        throw new Exception("SUBACFARAP is already exists. Please choose other SUBACFARAP.");
                    default:
                        throw;
                }
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();

                throw;
            }
            finally
            {
                cmd.Close();
            }

            return result;
        }

        public static int UpdateSUBACC(int ExecCode, List<SUBACFARAP> subacfarap,string scode)
        {
            int result = 0;
            using (DataAccess.SqlServer cmd = new DataAccess.SqlServer())
            {
                try
                {
                    //cmd.CommandText = "APSaveHeader";
                    //cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, ExecCode);
                    //cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, item.RP);
                    //cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, item.Acc.Account);
                    //cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, item.CCySUBACFARAP.CurrencyCode);
                    //cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, item.DocNo);
                    //cmd.AddParameter("@SeqNo", System.Data.SqlDbType.Int, item.SeqNo);
                    //cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, item.Branch.BranchCode);
                    //cmd.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, scode);
                    //cmd.Open();
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmd.BeginTransaction();

                    //result = cmd.ExecuteNonQuery();
                    //cmd.CommitTransaction();
                    foreach (var item in subacfarap)
                    {
                        cmd.CommandText = "APSaveDetail";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, item.RP);
                        cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, item.Acc.Account);
                        cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, item.CCySUBACFARAP.CurrencyCode);
                        cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, item.DocNo);
                        cmd.AddParameter("@SeqNo", System.Data.SqlDbType.Int, item.SeqNo);
                        cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, item.Branch.BranchCode);
                        cmd.AddParameter("@SUBACC", System.Data.SqlDbType.VarChar, item.SubAcc.Account);
                        cmd.AddParameter("@SCode", System.Data.SqlDbType.VarChar, scode);
                        cmd.Open();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.BeginTransaction();

                        result = cmd.ExecuteNonQuery();
                        cmd.CommitTransaction();
                    }
                    
                }
                catch (SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("SUBACFARAP is already exists. Please choose other SUBACFARAP.");
                        default:
                            throw;
                    }
                }
                catch (Exception ex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    throw;
                }
                finally
                {
                    cmd.Close();
                }

                return result;
            }
        }

        public static int InsUpSUBACFARAP(int ExecCode, IDS.DataAccess.SqlServer cmd, SUBACFARAP subacfarap)
        {
            int result = 0;

            try
            {
                cmd.CommandText = "ARAPSaveDetail";
                cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, subacfarap.RP);
                cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, subacfarap.Acc.Account);
                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, subacfarap.CCySUBACFARAP.CurrencyCode);
                cmd.AddParameter("@CUST_PRIN", System.Data.SqlDbType.VarChar, subacfarap.CustPrin);
                cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, subacfarap.DocNo);
                cmd.AddParameter("@SeqNo", System.Data.SqlDbType.Int, subacfarap.SeqNo);
                cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, subacfarap.Branch.BranchCode);
                cmd.AddParameter("@DEPTCODE", System.Data.SqlDbType.VarChar, subacfarap.DeptCode);
                cmd.AddParameter("@SUBACC", System.Data.SqlDbType.VarChar, subacfarap.SubAcc.Account);
                cmd.AddParameter("@SUBAMT", System.Data.SqlDbType.Money, subacfarap.SubAmount);
                cmd.AddParameter("@Description", System.Data.SqlDbType.VarChar, subacfarap.Description);
                cmd.AddParameter("@EQUIVAMT", System.Data.SqlDbType.Money, subacfarap.EquivAmt);
                cmd.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException sex)
            {
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();

                switch (sex.Number)
                {
                    case 2627:
                        throw new Exception("SUBACFARAP is already exists. Please choose other SUBACFARAP.");
                    default:
                        throw;
                }
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();

                throw;
            }

            return result;
        }
    }
}
