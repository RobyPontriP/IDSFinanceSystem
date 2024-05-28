using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GL.GLTransaction
{
    /// <summary>
    /// Voucher Header
    /// </summary>
    public class GLVoucherH
    {
        public GLTable.SourceCode SCode { get; set; }
        public string Voucher { get; set; }
        public GeneralTable.Branch VBranch { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime TransDate { get; set; }
        public int PayTerm { get; set; }
        public int Reversed { get; set; }
        public string ReversedVoucher { get; set; }
        public int ARAP_Trans { get; set; }
        public string OperatorID { get; set; }
        public DateTime LastUpdate { get; set; }
        public int StatusPayment { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }

        

        public GLVoucherH()
        {
        }
    }
}
