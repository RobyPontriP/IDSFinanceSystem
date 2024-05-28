using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
   public class AccrualCustProject
    {
        [Display(Name = "Invoice Number")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Invoice Number")]
        [MaxLength(20)]
        public string InvoiceNo { get; set; }

        [Display(Name = "Name")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Branch")]
        [MaxLength(5)]
        public string Branch { get; set; }

        [Display(Name = "Customer")]
        public IDS.GeneralTable.Customer Customer { get; set; }

        [Display(Name = "Ccy")]
        public string Ccy { get; set; }

        [Display(Name = "Period")]
        public string Period { get; set; }

        [Display(Name = "SEQNo")]
        public int SEQNo { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "GLStatus")]
        public bool GLStatus { get; set; }

        [Display(Name = "CreatedBy")]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "CreatedDate")]
        [MaxLength(20)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "OperatorID")]
        [MaxLength(20)]
        public string OperatorID { get; set; }

        [Display(Name = "LastUpdate")]
        public DateTime LastUpdate { get; set; }

        public AccrualCustProject()
        {
        }

        public static AccrualCustProject GetAccrualCustProject(string period)
        {
            AccrualCustProject accrCustProj = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelAccrualAR";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        accrCustProj = new IDS.Sales.AccrualCustProject();
                        accrCustProj.Period = dr["Period"] as string;
                        accrCustProj.Total = Tool.GeneralHelper.NullToDecimal(dr["Total"], 0);
                        accrCustProj.Name = dr["Name"] as string;
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return accrCustProj;
        }

        public static AccrualCustProject GetAccrualCustProject(string period,string invno, string branch)
        {
            AccrualCustProject accrualCustProj = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelAccrualAR";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@invNo", System.Data.SqlDbType.VarChar, invno);
                db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        accrualCustProj = new IDS.Sales.AccrualCustProject();
                        accrualCustProj.InvoiceNo = dr["InvoiceNo"] as string;
                        accrualCustProj.Branch = dr["Branch"] as string;
                        accrualCustProj.Period = dr["Period"] as string;
                        accrualCustProj.SEQNo = Tool.GeneralHelper.NullToInt(dr["SEQNo"], 0);
                        accrualCustProj.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                        accrualCustProj.GLStatus = Tool.GeneralHelper.NullToBool(dr["GLStatus"]);

                        accrualCustProj.Customer = new IDS.GeneralTable.Customer();
                        accrualCustProj.Customer.CUSTCode = dr["CUST_PRIN"] as string;

                        accrualCustProj.Ccy = dr["Ccy"] as string;
                        accrualCustProj.OperatorID = dr["OperatorID"] as string;
                        accrualCustProj.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return accrualCustProj;
        }


        public static List<IDS.Sales.AccrualCustProject> GetListAccrualCustProject(string period)
        {
            List<IDS.Sales.AccrualCustProject> list = new List<IDS.Sales.AccrualCustProject>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelAccrualAR";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            IDS.Sales.AccrualCustProject accrualCustProj = new IDS.Sales.AccrualCustProject();
                            accrualCustProj.Period = dr["Period"] as string;
                            accrualCustProj.Total= Tool.GeneralHelper.NullToDecimal(dr["Total"],0);
                            accrualCustProj.Name= dr["Name"] as string;
                            list.Add(accrualCustProj);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<IDS.Sales.AccrualCustProject> GetAccrualCustProjectDetail(string period)
        {
            List<IDS.Sales.AccrualCustProject> list = new List<IDS.Sales.AccrualCustProject>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelAccrualAR";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            IDS.Sales.AccrualCustProject accrualCustProj = new IDS.Sales.AccrualCustProject();
                            accrualCustProj.InvoiceNo = dr["InvoiceNo"] as string;
                            accrualCustProj.Branch = dr["Branch"] as string;
                            accrualCustProj.Period = dr["Period"] as string;
                            accrualCustProj.SEQNo = Tool.GeneralHelper.NullToInt(dr["SEQNo"],0);
                            accrualCustProj.Amount = Tool.GeneralHelper.NullToDecimal(dr["Amount"], 0);
                            accrualCustProj.GLStatus = Tool.GeneralHelper.NullToBool(dr["GLStatus"]);

                            accrualCustProj.Customer = new IDS.GeneralTable.Customer();
                            accrualCustProj.Customer.CUSTCode = dr["CUST_PRIN"] as string;

                            accrualCustProj.Ccy= dr["Ccy"] as string;
                            accrualCustProj.OperatorID = dr["OperatorID"] as string;
                            accrualCustProj.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            list.Add(accrualCustProj);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }
        
        public int InsUpDelAccrCustProj(Tool.PageActivity ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SalesAccrualAR";
                    cmd.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, InvoiceNo);
                    cmd.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Branch);
                    cmd.AddParameter("@Period", System.Data.SqlDbType.VarChar, Period);
                    cmd.AddParameter("@SeqNo", System.Data.SqlDbType.TinyInt, SEQNo);
                    cmd.AddParameter("@Amount", System.Data.SqlDbType.Money, Amount);
                    cmd.AddParameter("@GLStatus", System.Data.SqlDbType.Bit, GLStatus);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, (int)ExecCode);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Accrual Customer Project code is already exists. Please choose other Accrual Customer Project code.");
                        case 547:
                            throw new Exception("Data can not be delete while data used for reference.");
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
