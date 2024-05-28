using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Web;

namespace IDS.Tool
{
    /// <summary>
    /// Class helper untuk membaca file Rekening Koran (.csv)
    /// </summary>
    public class RekeningKoranDetail
    {
        /// <summary>
        /// Deskripsi pada rekening koran (digabung semua)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Tanggal transaksi di rekening koran
        /// </summary>
        public DateTime TransDate { get; set; }

        /// <summary>
        /// Debit atau Credit
        /// </summary>
        public string DBCR { get; set; }

        /// <summary>
        /// Nilai transaksi
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Ending balance per transaksi
        /// </summary>
        public double EndingBalance { get; set; }

        public RekeningKoranDetail()
        {
            Amount = 0;
            EndingBalance = 0;
        }
        
    }
}
