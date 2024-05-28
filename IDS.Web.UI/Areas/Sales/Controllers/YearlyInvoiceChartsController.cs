using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;


namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class YearlyInvoiceChartsController : Controller
    {
        // GET: Sales/YearlyInvoiceCharts
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getyearfromslsinvh()
        {
            return Json(IDS.Sales.YearlyInvoiceCharts.GetYearFromSlsInvh(), JsonRequestBehavior.AllowGet);
        }


        public string GetChartYearFromTo()
        {
            string return_ = "";

            dynamic obj = new Newtonsoft.Json.Linq.JObject();
            obj.Name = "Satinder Singh";
            obj.Location = "Mumbai";
            obj.blog = "Codepedia.info";

            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var yearform = o.SelectToken("yearfrom").ToString();
                var yearto = o.SelectToken("yearto").ToString();

                System.Data.DataTable dt = IDS.Sales.YearlyInvoiceCharts.GetByYearFromTo(yearform, yearto);

                List<LineChart.Dataset> ds = new List<LineChart.Dataset>();

                LineChart.Dataset CASI_ = new LineChart.Dataset();
                CASI_.label = "CASI";
                CASI_.backgroundColor = "transparent";
                CASI_.borderColor = "#2986cc";
                CASI_.cubicInterpolationMode = "monotone";
                CASI_.fill = false;
                CASI_.tension = 0.4;
                CASI_.pointBackgroundColor = "#09fb05";
                CASI_.pointBorderColor = "#078ef3";
                CASI_.pointBorderWidth = 4;
                CASI_.pointHitRadius = 30;
                CASI_.pointHoverRadius = 10;
                CASI_.pointStyle = "rectRounded";
                CASI_.pointRadius = 5;//
                CASI_.data = GetNilaisales(dt, "CASI");
                ds.Add(CASI_);

                LineChart.Dataset SMI_ = new LineChart.Dataset();
                SMI_.label = "SMI";
                SMI_.backgroundColor = "transparent";
                SMI_.borderColor = "#E64A19";
                SMI_.cubicInterpolationMode = "monotone";
                SMI_.fill = false;
                SMI_.tension = 0.4;
                SMI_.pointBackgroundColor = "#FFA726";// bulat mouse focused
                SMI_.pointBorderColor = "#E64A19";//lapis bulat Mouse focused
                SMI_.pointBorderWidth = 4;
                SMI_.pointHitRadius = 30;
                SMI_.pointHoverRadius = 10;
                SMI_.pointStyle = "rectRounded";
                SMI_.pointRadius = 5;//
                SMI_.data = GetNilaisales(dt, "SMI");
                ds.Add(SMI_);

                LineChart.ChartYearly carts_root = new LineChart.ChartYearly()
                {
                    datasets = ds,
                    labels = GetPeriod(dt, "CASI")// null// new List<string> { "Jan ", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
                };

                return_ = Newtonsoft.Json.JsonConvert.SerializeObject(carts_root);
            }
            return return_;
        }

        public string GetChartYearFromToBarChart()
        {
            string return_ = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var yearform = o.SelectToken("yearfrom").ToString();
                var yearto = o.SelectToken("yearto").ToString();

                System.Data.DataTable dt = IDS.Sales.YearlyInvoiceCharts.GetByYearFromTo(yearform, yearto);

                List<BarChart.Dataset> ds = new List<BarChart.Dataset>();

                BarChart.Dataset CASI_ = new BarChart.Dataset();
                CASI_.label = "CASI";
                CASI_.backgroundColor = "rgba(60,141,188,0.9)";
                CASI_.borderColor = "rgba(60,141,188,0.8)";
                CASI_.pointRadius = false;
                CASI_.pointColor = "#3b8bba";
                CASI_.pointStrokeColor = "rgba(60,141,188,1)";
                CASI_.pointHighlightFill = "#fff";
                CASI_.pointHighlightStroke = "rgba(60,141,188,1)";
                CASI_.data = GetNilaisales(dt, "CASI");
                ds.Add(CASI_);

                BarChart.Dataset SMI_ = new BarChart.Dataset();
                SMI_.label = "SMI";
                SMI_.backgroundColor = "#E64A19";
                SMI_.borderColor = "rgba(210, 214, 222, 1)";
                SMI_.pointRadius = false;
                SMI_.pointColor = "rgba(210, 214, 222, 1)";
                SMI_.pointStrokeColor = "#E64A19";
                SMI_.pointHighlightFill = "#fff";
                SMI_.pointHighlightStroke = "rgba(220,220,220,1)";
                SMI_.data = GetNilaisales(dt, "SMI");
                ds.Add(SMI_);

                BarChart.Root carts_root = new BarChart.Root()
                {
                    datasets = ds,
                    labels = GetPeriod(dt, "CASI")// null// new List<string> { "Jan ", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
                };

                return_ = Newtonsoft.Json.JsonConvert.SerializeObject(carts_root);
            }
            return return_;
        }


        public string GetChartYear()
        {
            string return_ = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var year_ = o.SelectToken("year").ToString();
                System.Data.DataTable dt = IDS.Sales.YearlyInvoiceCharts.GetByYear(year_);

                List<LineChart.Dataset> ds = new List<LineChart.Dataset>();

                LineChart.Dataset CASI_ = new LineChart.Dataset();
                CASI_.label = "CASI";
                CASI_.backgroundColor = "transparent";
                CASI_.borderColor = "#2986cc";
                CASI_.cubicInterpolationMode = "monotone";
                CASI_.fill = false;
                CASI_.tension = 0.4;
                CASI_.pointBackgroundColor = "#09fb05";
                CASI_.pointBorderColor = "#078ef3";
                CASI_.pointBorderWidth = 4;
                CASI_.pointHitRadius = 30;
                CASI_.pointHoverRadius = 10;
                CASI_.pointStyle = "rectRounded";
                CASI_.pointRadius = 5;//
                CASI_.data = GetNilaisales(dt, "CASI");
                ds.Add(CASI_);

                LineChart.Dataset SMI_ = new LineChart.Dataset();
                SMI_.label = "SMI";
                SMI_.backgroundColor = "transparent";
                SMI_.borderColor = "#E64A19";
                SMI_.cubicInterpolationMode = "monotone";
                SMI_.fill = false;
                SMI_.tension = 0.4;
                SMI_.pointBackgroundColor = "#FFA726";// bulat mouse focused
                SMI_.pointBorderColor = "#E64A19";//lapis bulat Mouse focused
                SMI_.pointBorderWidth = 4;
                SMI_.pointHitRadius = 30;
                SMI_.pointHoverRadius = 10;
                SMI_.pointStyle = "rectRounded";
                SMI_.pointRadius = 5;//
                SMI_.data = GetNilaisales(dt, "SMI");
                ds.Add(SMI_);


                LineChart.ChartYearly carts_root = new LineChart.ChartYearly()
                {
                    datasets = ds,
                    labels = new List<string> { "Jan ", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
                };

                return_ = Newtonsoft.Json.JsonConvert.SerializeObject(carts_root);
            }
            return return_;
        }

        private List<double> GetNilaisales(System.Data.DataTable d, string JenisInvoice)
        {
            List<double> m = new List<double>();


            if (d.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                   from order in d.AsEnumerable()
                   where order.Field<String>("JenisInvoice") == JenisInvoice
                   select order;
                if (query.Any())
                {
                    System.Data.DataTable boundTable = query.CopyToDataTable<System.Data.DataRow>();
                    List<double> data1 = new List<double>();
                    foreach (System.Data.DataRow row in boundTable.Rows)
                    {
                        string nilaiAsli = "";
                        string sNumber = row["Nilaisales"].ToString();
                        if (sNumber.Contains("."))
                        {
                            char delim = '.';
                            string[] values = sNumber.Split(delim);
                            nilaiAsli = values[0];
                        }
                        else
                        {
                            nilaiAsli = sNumber;
                        }
                        m.Add(Convert.ToDouble(nilaiAsli));
                    }
                }

            }
            return m;
        }

        private List<string> GetPeriod(System.Data.DataTable d, string casi_smi)
        {
            List<string> m = new List<string>();


            if (d.Rows.Count > 0)
            {
                IEnumerable<DataRow> query =
                   from order in d.AsEnumerable()
                   where order.Field<String>("JenisInvoice") == casi_smi
                   select order;
                if (query.Any())
                {
                    System.Data.DataTable boundTable = query.CopyToDataTable<System.Data.DataRow>();
                    List<double> data1 = new List<double>();
                    foreach (System.Data.DataRow row in boundTable.Rows)
                    {
                        string sNumber = row["Period"].ToString();

                        m.Add(sNumber);
                    }
                }

            }
            return m;
        }
    }


}

namespace LineChart
{
    public class Dataset
    {
        public string label { get; set; }
        public List<double> data { get; set; }
        public string cubicInterpolationMode { get; internal set; }
        public string backgroundColor { get; internal set; }
        public string borderColor { get; internal set; }
        public bool fill { get; internal set; }
        public double tension { get; internal set; }
        public string pointBackgroundColor { get; internal set; }
        public string pointBorderColor { get; internal set; }
        public int pointBorderWidth { get; internal set; }
        public int pointHitRadius { get; internal set; }
        public int pointHoverRadius { get; internal set; }
        public string pointStyle { get; internal set; }
        public int pointRadius { get; internal set; }
    }

    public class ChartYearly
    {
        public List<string> labels { get; set; }
        public List<Dataset> datasets { get; set; }
    }
}

namespace BarChart
{
    public class Dataset
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public bool pointRadius { get; set; }
        public string pointColor { get; set; }
        public string pointStrokeColor { get; set; }
        public string pointHighlightFill { get; set; }
        public string pointHighlightStroke { get; set; }
        public List<double> data { get; set; }
    }

    public class Root
    {
        public List<string> labels { get; set; }
        public List<Dataset> datasets { get; set; }
    }
}