using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.ReportHelper.GeneralTable
{
    public class KelurahanReport
    {
        private static IDS.GeneralTable.Kelurahan data = new IDS.GeneralTable.Kelurahan();

        public static List<object> GetReportForKelurahan<T>()
        {
            return IDS.GeneralTable.Kelurahan.GetKelurahan().Select(x => new
            {
                KelurahanCode = x.KelurahanCode,
                KelurahanName = x.KelurahanName,
                KecamatanCode = x.KecamatanKelurahan.KecamatanCode,
                KecamatanName = x.KecamatanKelurahan.KecamatanName,
                CityCode = x.CityKelurahan.CityCode,
                CityName = x.CityKelurahan.CityName,
                CounttyCode = x.CountryKelurahan.CountryCode,
                CountryName = x.CountryKelurahan.CountryName
            }).ToList<object>();
        }
    }
}
