using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTransaction
{
    public class GLStandingOrderD
    {
        public int Counter { get; set; }
        public GeneralTable.Currency CCy { get; set; }
        public GLTable.ChartOfAccount COA { get; set; }
        public GLTable.CashBasisAccount CashAccount { get; set; }
        public GeneralTable.Department Dept { get; set; }
        public GLTable.Project Proj { get; set; }
        public string DocumentNo { get; set; }
        public GeneralTable.CustomerSupplier CustSupp { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }

        public GLStandingOrderD()
        {
        }

        public static List<GLStandingOrderD> GetGLSODetail(string branchCode, string SONumber)
        {
            if (string.IsNullOrWhiteSpace(branchCode) || string.IsNullOrWhiteSpace(SONumber))
                return new List<GLStandingOrderD>();

            List<GLStandingOrderD> detail = new List<GLStandingOrderD>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelGLSOD";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@SONo", System.Data.SqlDbType.VarChar, SONumber);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            GLStandingOrderD d = new GLStandingOrderD();
                            d.Counter = Tool.GeneralHelper.NullToInt(dr["SODCtr"], 0);

                            d.CCy = new GeneralTable.Currency();
                            d.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["SODCcy"]);

                            if (dr["SODAcc"] == DBNull.Value)
                            {
                                d.COA = new GLTable.ChartOfAccount();
                                d.COA.Account = Tool.GeneralHelper.NullToString(dr["SODAcc"]);
                            }                            

                            if (dr["CASHACC"] != DBNull.Value)
                            {
                                d.CashAccount = new GLTable.CashBasisAccount();
                                d.CashAccount.Account = Tool.GeneralHelper.NullToString(dr["CASHACC"]);
                            }

                            if (dr["SODDept"] != DBNull.Value)
                            {
                                d.Dept = new GeneralTable.Department();
                                d.Dept.DepartmentCode = Tool.GeneralHelper.NullToString(dr["SODDept"]);
                            }

                            if (dr["SODProj"] != DBNull.Value)
                            {
                                d.Proj = new GLTable.Project();
                                d.Proj.ProjectCode = Tool.GeneralHelper.NullToString(dr["SODProj"]);
                            }
                            
                            d.DocumentNo = Tool.GeneralHelper.NullToString(dr["SODDocNo"]);
                            d.Amount = Tool.GeneralHelper.NullToDouble(dr["SODAmount"], 0);
                            d.Description = Tool.GeneralHelper.NullToString(dr["SODDesc"]);

                            detail.Add(d);
                        }

                        dr.Close();
                    }
                }

                db.Close();
            }

            return detail;
        }

        public int InsUpDel(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GL_UpdACFSORDD";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();
                    cmd.BeginTransaction();

                    string[] item;
                    string sOHNo = "";
                    string branchCode = "";

                    char separator = ';';

                    for (int i = 0; i < data.Length; i++)
                    {
                        item = data[i].Split(separator);

                        sOHNo = item[0];
                        branchCode = item[1];

                        cmd.AddParameter("@SODNo", System.Data.SqlDbType.VarChar, sOHNo);
                        cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);


                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommitTransaction();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception(" Standing Order is already exists. Please choose other Standing Order Code.");
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
                finally
                {
                    cmd.Close();
                }
            }

            return result;
        }
    }
}
