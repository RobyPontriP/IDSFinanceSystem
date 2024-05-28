using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class ProcessAccrual
    {
        public string OperatorID { get; set; }
        public DateTime LastUpdate { get; set; }
        public string MessageError { get; set; }

        public ProcessAccrual()
        {

        }

        public string Process(string period,string branch)
        {
            string strResult = "";

            using(DataAccess.SqlServer cmd = new DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "ProcessAcrualAR";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.Int, 2);
                    cmd.AddParameter("@docNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                    cmd.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                    cmd.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
                    cmd.AddParameter("@payTerm", System.Data.SqlDbType.Int, DBNull.Value);
                    cmd.AddParameter("@amount", System.Data.SqlDbType.Money, DBNull.Value);
                    cmd.AddParameter("@Operator", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();

                    strResult = "Process Done";
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    strResult = sex.Message;

                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                }

                finally
                {
                    cmd.Close();
                }
            }

            return strResult;
        }

        public static List<System.Web.Mvc.SelectListItem> GetPeriodForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> periods = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SELECT DISTINCT tblAcrualAR.period, " +
                " (SELECT CASE substring(tblAcrualAR.period, 5, 2) " +
                " WHEN '01' THEN 'Januari' " +
                " WHEN '02' THEN 'Februari' " +
                " WHEN '03' THEN 'Maret' " +
                " WHEN '04' THEN 'April' " +
                " WHEN '05' THEN 'Mei' " +
                " WHEN '06' THEN 'Juni' " +
                " WHEN '07' THEN 'Juli' " +
                " WHEN '08' THEN 'Agustus' " +
                " WHEN '09' THEN 'September' " +
                " WHEN '10' THEN 'Oktober' " +
                " WHEN '11' THEN 'November' " +
                " WHEN '12' THEN 'Desember' END) " +
                " + ', ' + " +
                " substring (tblAcrualAR.period, 1, 4) " +
                " AS Bulan FROM tblAcrualAr WHERE tblAcrualAR.GLStatus = 0 ORDER BY tblAcrualAr.period";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        periods = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem period = new System.Web.Mvc.SelectListItem();
                            period.Value = dr["period"] as string;
                            period.Text = dr["Bulan"] as string;

                            periods.Add(period);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return periods;
        }
    }
}
