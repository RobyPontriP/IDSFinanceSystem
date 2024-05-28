using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Tool
{
    public class ExportToCSV
    {
        private System.Web.HttpResponse _Response;
        private StringBuilder _sb = new StringBuilder();

        public ExportToCSV(System.Web.HttpResponse response)
        {
            _Response = response;
        }

        public void setColumn(string[] Column)
        {
            _sb.Append("\"").Append(string.Join("\",\"", Column.ToArray())).Append("\"\n");
        }

        public void setData(System.Data.DataTable dataTable)
        {
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                setData(row);
            }
        }
        //object item;
        public void setData(System.Data.DataRow dataRow)
        {

            object[] arr;
            arr = dataRow.ItemArray;
            //_sb.Append("\n");
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = arr[i].GetType() == typeof(decimal) ? Convert.ToDouble(arr[i]).ToString("F2") :
                    arr[i].GetType() == typeof(int) ? Convert.ToDouble(arr[i]).ToString("F0") : arr[i].ToString();
                if (i == 0)
                    _sb.Append("\"").Append(arr[i].ToString());
                else
                    _sb.Append("\",\"").Append(arr[i].ToString());
            }

            _sb.Append("\"\n");
        }

        public static StringBuilder setData(System.Data.DataTable dataTable,StringBuilder _sb)
        {
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                object[] arr;
                arr = row.ItemArray;
                //_sb.Append("\n");
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = arr[i].GetType() == typeof(decimal) ? Convert.ToDouble(arr[i]).ToString("F2") :
                        arr[i].GetType() == typeof(int) ? Convert.ToDouble(arr[i]).ToString("F0") : arr[i].ToString();
                    if (i == 0)
                        _sb.Append("\"").Append(arr[i].ToString());
                    else
                        _sb.Append("\",\"").Append(arr[i].ToString());
                }
                _sb.Append("\"\n");
            }

            //_sb.Append("\"\n");

            return _sb;
        }

        public void CreateCsv()
        {
            _Response.ContentType = "text/csv";
            _Response.AddHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
            _Response.Write(_sb.ToString());
            //_Response.Flush();
            _Response.End();
        }

        // Add by Anthony
        public void CreateCsv(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return;

            _Response.ContentType = "text/csv";
            _Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
            _Response.Write(_sb.ToString());
            //_Response.Flush();
            _Response.End();
        }
        // End add by Anthony
    }
}
