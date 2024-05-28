using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class ACFMCTR
    {
        [Display(Name = "Period")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Period is required")]
        [MaxLength(8), StringLength(8)]
        public string Period { get; set; }

        [Display(Name = "Branch")]
        public IDS.GeneralTable.Branch Branch { get; set; }

        [Display(Name = "Closing")]
        public bool Closing { get; set; }
        
        public ACFMCTR()
        {

        }

        public static bool GetClosingStatus(string period, string branch)
        {
            bool result = false;
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFMCTR";
                db.AddParameter("@period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@branch", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = IDS.Tool.GeneralHelper.NullToBool(dr["Closing"], false);
                        }
                            
                    }
                    else
                    {
                        result = true;
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return result;
        }
    }
}
