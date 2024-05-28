using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDS.Maintenance
{
    public class UserGroup
    {
        [Display(Name = "User Group Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Group Code is required")]
        [MaxLength(20), StringLength(20)]
        public string GroupCode { get; set; }

        [Display(Name = "User Group Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Group Name is required")]
        [MaxLength(50), StringLength(50)]
        public string GroupName { get; set; }
        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public UserGroup()
        {
        }

        public UserGroup(string code, string name)
        {
            GroupCode = code;
            GroupName = name;
        }
        
        /// <summary>
        /// Mengambil user userGroup berdasarkan parameter kode group user
        /// </summary>
        /// <param name="userGroupCode">Kode Group User</param>
        /// <returns></returns>
        public static UserGroup GetUserGroup(string userGroupCode)
        {
            UserGroup group = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelUserGroup";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.AddParameter("@GroupCode", System.Data.SqlDbType.VarChar, userGroupCode);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        group = new UserGroup();
                        group.GroupCode = dr["GroupCode"] as string;
                        group.GroupName = dr["GroupName"] as string;
                        group.EntryUser = dr["EntryUser"] as string;
                        //group.EntryDate = dConvert.ToDateTime(dr["EntryDate"]);
                        group.EntryDate = IDS.Tool.GeneralHelper.NullToDateTime(dr["EntryDate"],DateTime.Now);
                        group.OperatorID = dr["OperatorID"] as string;
                        //group.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                        group.LastUpdate = IDS.Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return group;
        }

        public static List<UserGroup> GetUserGroup()
        {
            List<IDS.Maintenance.UserGroup> list = new List<UserGroup>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelUserGroup";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@GroupCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            UserGroup userGroup = new UserGroup();
                            userGroup.GroupCode = dr["GroupCode"] as string;
                            userGroup.GroupName = dr["GroupName"] as string;
                            userGroup.EntryUser = dr["EntryUser"] as string;
                            userGroup.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            userGroup.OperatorID = dr["OperatorID"] as string;
                            userGroup.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(userGroup);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<SelectListItem> GetUserGroupForDatasource()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "";
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
                            item.Value = Tool.GeneralHelper.NullToString(dr["GroupCode"]);
                            item.Text = Tool.GeneralHelper.NullToString(dr["GroupName"]);

                            items.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return items;
        }

        public int InsUpDelUserGroup(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "MntUserGroup";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@GroupCode", System.Data.SqlDbType.VarChar, GroupCode);
                    cmd.AddParameter("@GroupName", System.Data.SqlDbType.VarChar, GroupName);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();
                }
                catch (SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("User Group code is already exists. Please choose other User Group code.");
                        default:
                            throw;
                    }
                }
                catch (Exception ex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    throw;
                }
                finally
                {
                    cmd.Close();
                }
            }

            return result;
        }

        public int InsUpDelUserGroup(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "MntUserGroup";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "MntUserGroup";
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@GroupCode", System.Data.SqlDbType.VarChar, data[i]);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommitTransaction();
                }
                catch (SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("User Group Code is already exists. Please choose other User Group Code.");
                        case 547:
                            throw new Exception("One or more data can not be delete while data used for reference.");
                        default:
                            throw;
                    }
                }
                catch
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    throw;
                }
                finally
                {
                    cmd.Close();
                }
            }

            return result;
        }

        public override string ToString()
        {
            return Convert.ToString(GroupName);
        }
    }
}