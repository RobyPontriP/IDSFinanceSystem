using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
  public  class YearlyInvoiceCharts
    {
        public YearlyInvoiceCharts()
        {

        }

        public static List<System.Web.Mvc.SelectListItem> GetYearFromSlsInvh()
        {
            List<System.Web.Mvc.SelectListItem> jps = new List<System.Web.Mvc.SelectListItem>();
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "select year(getdate())+1 as year union Select distinct year(Invoicedate) from SLSInvH";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        jps = new List<System.Web.Mvc.SelectListItem>();
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem jp = new System.Web.Mvc.SelectListItem();
                            jp.Value = dr["year"].ToString();
                            jp.Text = dr["year"].ToString();
                            jps.Add(jp);
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return jps;
        }

        public static System.Data.DataTable GetByYear(string year)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();
            dt.Columns.Add("JenisInvoice");
            dt.Columns.Add("Period");
            dt.Columns.Add("Bulan");
            dt.Columns.Add("Nilaisales");
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("			select iif(PROJECTCODE IS NULL, 'CASI', 'SMI') AS JENISINVOICE, convert(varchar(6),invoicedate,112)as period, ");
                sb.AppendLine("			(CASE");
                sb.AppendLine("				WHEN MONTH(invoicedate)= 1 THEN 'Jan'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 2 THEN 'Feb'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 3 THEN 'Mart'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 4 THEN 'Apr'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 5 THEN 'Mei'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 6 THEN 'Jun'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 7 THEN 'Jul'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 8 THEN 'Agust'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 9 THEN 'Sept'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 10 THEN 'Okt'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 11 THEN 'Nov'");
                sb.AppendLine("				WHEN MONTH(invoicedate) = 12 THEN 'Des'");
                sb.AppendLine("			END) AS Bulan,");
                sb.AppendLine("			");
                sb.AppendLine("		   sum(InvoiceAmount) as NilaiSales");
                sb.AppendLine("			 from SLSInvH ");
                sb.AppendLine("			 where year(invoicedate) = @year");
                sb.AppendLine("			 group by iif(PROJECTCODE IS NULL, 'CASI', 'SMI'), convert(varchar(6),invoicedate,112),MONTH(invoicedate)		");
                db.CommandText = sb.ToString();
                db.AddParameter("@year", System.Data.SqlDbType.VarChar, year);
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                       while (dr.Read())
                        {
                            dt.Rows.Add(new object[] { dr["JENISINVOICE"].ToString(), dr["period"].ToString() , dr["bulan"].ToString() , dr["Nilaisales"].ToString() });
                           // System.Diagnostics.Debug.WriteLine(dr["Nilaisales"].ToString());
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return dt;
        }


        public static System.Data.DataTable GetByYearFromTo(string YearFrom, string YearTO)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();
            dt.Columns.Add("JenisInvoice");
            dt.Columns.Add("Period");
            dt.Columns.Add("Nilaisales");
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("		select iif(PROJECTCODE IS NULL, 'CASI', 'SMI') AS JENISINVOICE, convert(varchar(4),invoicedate,112)as period,");
                sb.AppendLine("	           	   CAST(sum(InvoiceAmount) AS BIGINT) AS NilaiSales ");
                sb.AppendLine("	          			 from SLSInvH ");
                sb.AppendLine("	           		 where year(invoicedate) BETWEEN @from and @to");
                sb.AppendLine("	            	 group by iif(PROJECTCODE IS NULL, 'CASI', 'SMI'), convert(varchar(4),invoicedate,112)		");
                db.CommandText = sb.ToString();
                db.AddParameter("@from", System.Data.SqlDbType.VarChar, YearFrom);
                db.AddParameter("@to", System.Data.SqlDbType.VarChar, YearTO);
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            dt.Rows.Add(new object[] { dr["JENISINVOICE"].ToString(), dr["period"].ToString(),dr["Nilaisales"].ToString() });
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return dt;
        }

    }
}
