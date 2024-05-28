using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDS.Maintenance
{
    public class UserMenuProject
    {
        public string ProjectName { get; set; }
        public string EntryUser { get; set; }
        public DateTime EntryDate { get; set; }
        public string OperatorID { get; set; }
        public DateTime LastUpdate { get; set; }

        public UserMenuProject()
        {
        }

        public static List<UserMenuProject> GetUserMenuProject()
        {
            List<UserMenuProject> list = new List<UserMenuProject>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelProject";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            UserMenuProject prj = new UserMenuProject();
                            prj.ProjectName = Tool.GeneralHelper.NullToString(dr["ProjectName"]);
                            prj.EntryUser = Tool.GeneralHelper.NullToString(dr["EntryUser"]);
                            prj.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            prj.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            prj.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            list.Add(prj);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<SelectListItem> GetUserMenuProjectForDatasource()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelProject";
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SelectListItem item = new SelectListItem();
                            item.Value = item.Text = Tool.GeneralHelper.NullToString(dr["ProjectName"]);

                            list.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }
    }
}