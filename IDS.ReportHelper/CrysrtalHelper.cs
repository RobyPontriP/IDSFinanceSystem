using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.ReportHelper
{
    public class CrystalHelper
    {
        public CrystalHelper()
        {
        }

        public CrystalDecisions.CrystalReports.Engine.ReportDocument SetDefaultFormulaField(CrystalDecisions.CrystalReports.Engine.ReportDocument rpt)
        {
            if (rpt == null)
                return rpt;

            if (rpt.DataDefinition.FormulaFields.Count == 0)
                return rpt;

            IDS.GeneralTable.Syspar syspar = IDS.GeneralTable.Syspar.GetInstance();

            if (syspar == null)
                return rpt;

            foreach (CrystalDecisions.CrystalReports.Engine.FormulaFieldDefinition field in rpt.DataDefinition.FormulaFields)
            {
                switch (field.Name.ToUpper())
                {
                    case "NAMA":
                    case "NAME":
                        rpt.DataDefinition.FormulaFields[field.Name].Text = syspar.PrintName ? "'" + syspar.Name + "'" : string.Empty;
                        break;                    
                    case "ADD1":
                        rpt.DataDefinition.FormulaFields[field.Name].Text = syspar.PrintAddress ? "'" + syspar.Address1 + "'" : string.Empty;
                        break;
                    case "ADD2":
                        rpt.DataDefinition.FormulaFields[field.Name].Text = syspar.PrintAddress ? "'" + syspar.Address2 + "'" : string.Empty;
                        break;
                    case "ADD3": // City
                        rpt.DataDefinition.FormulaFields[field.Name].Text = syspar.PrintCity ? "'" + syspar.Address3 + "'" : string.Empty;
                        break;
                    case "COUNTRY":
                        rpt.DataDefinition.FormulaFields[field.Name].Text = syspar.PrintCountry ? "'" + syspar.CountryCode + "'" : string.Empty;
                        break;
                    case "CHKTIME":
                        rpt.DataDefinition.FormulaFields[field.Name].Text = "'" + syspar.PrintTime.ToString() + "'";
                        break;
                    case "CHKPAGE":
                        rpt.DataDefinition.FormulaFields[field.Name].Text = "'" + syspar.PrintPageNumber.ToString() + "'";
                        break;
                    case "BASECCY":
                        rpt.DataDefinition.FormulaFields[field.Name].Text = "'" + syspar.BaseCCy.ToString() + "'";
                        break;
                    case "BAHASA":
                        rpt.DataDefinition.FormulaFields[field.Name].Text = "'" + syspar.Language.Trim() + "'";
                        break;
                }
            }

            return rpt;
        }

        public CrystalDecisions.CrystalReports.Engine.ReportDocument SetLogOn(CrystalDecisions.CrystalReports.Engine.ReportDocument rpt)
        {
            if (rpt != null)
            {
                CrystalDecisions.Shared.ConnectionInfo conInfo = PopulateConnectionInfo();
                CrystalDecisions.Shared.TableLogOnInfo rptTblLogonInfo = new CrystalDecisions.Shared.TableLogOnInfo();
                CrystalDecisions.CrystalReports.Engine.Tables rptTables = rpt.Database.Tables;

                foreach (CrystalDecisions.CrystalReports.Engine.Table tbl in rpt.Database.Tables)
                {
                    rptTblLogonInfo = tbl.LogOnInfo;
                    rptTblLogonInfo.ConnectionInfo = conInfo;
                    tbl.ApplyLogOnInfo(rptTblLogonInfo);
                }
            }

            return rpt;
        }

        private CrystalDecisions.Shared.ConnectionInfo PopulateConnectionInfo()
        {
            if (System.Web.HttpRuntime.Cache["CrystalReport.ConnectionInfo"] == null)
            {
                CrystalDecisions.Shared.ConnectionInfo crInfo = new CrystalDecisions.Shared.ConnectionInfo();

                using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
                {
                    crInfo.ServerName = db.GetServerName();
                    crInfo.DatabaseName = db.GetDatabaseName();
                    crInfo.UserID = System.Configuration.ConfigurationManager.ConnectionStrings["CrU"].ConnectionString;
                    crInfo.Password = System.Configuration.ConfigurationManager.ConnectionStrings["CrP"].ConnectionString;

                    System.Web.HttpRuntime.Cache.Add("CrystalReport.ConnectionInfo", crInfo, null, DateTime.Now.AddSeconds(60), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);

                    db.Close();
                    db.Dispose();
                }
            }

            return System.Web.HttpRuntime.Cache["CrystalReport.ConnectionInfo"] as CrystalDecisions.Shared.ConnectionInfo;
        }
    }
}
