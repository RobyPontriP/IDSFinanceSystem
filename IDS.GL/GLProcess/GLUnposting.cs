using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLProcess
{
    public class GLUnposting
    {
        public string OperatorID { get; set; }
        public DateTime LastUpdate { get; set; }
        public string MessageError { get; set; }

        public GLUnposting()
        {

        }

        public string GLUnpostingProcess(string dtPeriod, string branch)
        {
            MessageError = "";

            int result = 0;

            dtPeriod = Convert.ToDateTime(dtPeriod).ToString("yyyyMM");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLUnPostTrans";
                    cmd.AddParameter("@pBranch", System.Data.SqlDbType.VarChar, branch);
                    cmd.AddParameter("@TDate", System.Data.SqlDbType.VarChar, dtPeriod);
                    cmd.AddParameter("@CurrDate", System.Data.SqlDbType.DateTime, DateTime.Now);
                    cmd.AddParameter("@CurrUser", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                        default:
                            throw;
                    }

                    MessageError = sex.Message;
                }
                catch (Exception ex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    MessageError = ex.Message;
                    throw;
                }
                finally
                {
                    cmd.Close();
                }

                if (string.IsNullOrEmpty(MessageError))
                {
                    MessageError = "Process Completed";
                }
            }

            return MessageError;
        }
    }
}
