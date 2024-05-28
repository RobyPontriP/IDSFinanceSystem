using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class EndOfPeriod
    {
        public string OperatorID { get; set; }
        public DateTime LastUpdate { get; set; }
        public string MessageError { get; set; }

        public EndOfPeriod()
        {

        }

        public string SalesProcess(DateTime dtPeriod, string branch, string ChkProcess)
        {
            string strResult = "";
            string[] dataToProcess = ChkProcess.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            using (DataAccess.SqlServer cmd = new DataAccess.SqlServer())
            {
                //cmd.Open();
                //cmd.BeginTransaction();
                for (int i = 0; i < dataToProcess.Length; i++)
                {
                    string value = dataToProcess[i];

                    switch (value)
                    {
                        case "1":
                            if (RecalculateOutstanding(dtPeriod, branch, cmd) >= 1)
                            {
                                strResult = strResult + "1 1,";
                            }
                            else
                            {
                                strResult = strResult + "1 0,";
                            }
                            break;

                        case "2":
                            if (EndOfPeriodProcess(dtPeriod, cmd) >= 1)
                            {
                                strResult = strResult + "2 1,";
                            }
                            else
                            {
                                strResult = strResult + "2 0,";
                            }
                            break;
                    }
                }
                return strResult;
            }
        }

        private int EndOfPeriodProcess(DateTime FromPeriod, DataAccess.SqlServer cmd)
        {
            int result = 0;

            try
            {
                cmd.CommandText = "slsOutstanding";
                cmd.AddParameter("@period", System.Data.SqlDbType.DateTime, FromPeriod);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.DbCommand.CommandTimeout = 0;

                cmd.Open();

                cmd.BeginTransaction();
                //result = cmd.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                result = 1;
                cmd.CommitTransaction();
            }
            catch (Exception ex)
            {
                MessageError = string.IsNullOrEmpty(ex.Message) ? ex.Message : "* End of Period \n" + "    - " + ex.Message + "\n \n";
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();
                throw;
            }
            finally
            {
                cmd.Close();
            }

            return result;
        }

        private int RecalculateOutstanding(DateTime FromPeriod,string branch, DataAccess.SqlServer cmd)
        {
            int result = 0;

            try
            {
                IDS.Sales.OutstandingRecalculate.NewSPBasedCalculator.Recalculate(branch, null, null, FromPeriod);

                result = 1;
            }
            catch (Exception ex)
            {
                MessageError = string.IsNullOrEmpty(ex.Message) ? ex.Message : "* Recalculate Outstanding \n" + "    - " + ex.Message + "\n \n";
            }
            finally
            {
                cmd.Close();
            }

            return result;
        }
    }
}
