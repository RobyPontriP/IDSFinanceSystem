using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.GeneralReport.Controllers
{
    public class RptCustomerSupplierController : Controller
    {
        // GET: GeneralReport/RptCustomerSupplier
        public ActionResult Index()
        {
            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/Report/GeneralTable/CR/RptCustomerSupplier.rpt"));

            IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "FISCUS_CustomerSupplierList_" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".pdf");
        }
    }
}