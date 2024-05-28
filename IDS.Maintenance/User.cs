using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Maintenance
{
    public enum UserStatus : int
    {
        Active = 1,
        InActive = 0
    }

    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public UserGroup UserGroup { get; set; }

        public string ExpiredCode { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityAnswer { get; set; }
        public int Akumulasi { get; set; }
        public UserStatus Status { get; set; }
        public IDS.GeneralTable.Branch Branch { get; set; }

        public string EntryUser { get; set; }
        public DateTime EntryDate { get; set; }
        public string OperatorID { get; set; }
        public DateTime LastUpdate { get; set; }

        public User()
        {

        }

        public static User UserLogin(string userID, string password)
        {
            Maintenance.User user = null;

            Tool.clsCryptho crypt = new Tool.clsCryptho();

            string passwd = crypt.Encrypt(password, "ids");

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MNTUserLogin";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@User", System.Data.SqlDbType.VarChar, userID);
                db.AddParameter("@Password", System.Data.SqlDbType.VarChar, password);
                db.Open();

                db.ExecuteReader();

                using (SqlDataReader dr = db.DbDataReader as SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        user = new User();
                        user.UserID = dr["UserId"] as string;
                        user.Password = dr["Password"] as string;
                        user.Branch = GeneralTable.Branch.GetBranch(dr["BranchCode"] as string);
                        user.UserName = dr["UserName"] as string;
                        user.UserGroup = Maintenance.UserGroup.GetUserGroup(dr["GroupCode"] as string);
                        user.Status = (UserStatus)Convert.ToInt32(dr["Status"]);
                        user.EmailAddress = dr["EmailAddress"] as string;
                        //user.SecurityCode = dr["SecurityCode"] as string;
                        //user.SecurityAnswer = dr["SecurityAnswer"] as string;
                        //user.Akumulasi = dr["Akumulasi"] == DBNull.Value ? 0 : Convert.ToInt16(dr["Akumulasi"]);
                        user.ExpiredCode = dr["expiredCode"] as string;
                        //user.EntryUser = dr["EntryUser"] as string;
                        //user.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                    }

                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }

                db.Close();
            }

            return user;
        }
        
        public static IList<User> GetUser()
        {
            List<User> users = new List<User>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelUser";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@UserID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            User user = new User();
                            user.UserID = dr["UserId"] as string;
                            user.Password = dr["Password"] as string;
                            user.Branch = new GeneralTable.Branch();
                            user.Branch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);
                            
                            user.UserName = dr["UserName"] as string;

                            user.UserGroup = new Maintenance.UserGroup();
                            user.UserGroup.GroupCode = Tool.GeneralHelper.NullToString(dr["GroupCode"]);
                            
                            user.Status = (UserStatus)Convert.ToInt32(dr["Status"]);
                            user.EmailAddress = dr["EmailAddress"] as string;
                            user.SecurityCode = dr["SecurityCode"] as string;
                            user.SecurityAnswer = dr["SecurityAnswer"] as string;
                            user.Akumulasi = dr["Akumulasi"] == DBNull.Value ? 0 : Convert.ToInt16(dr["Akumulasi"]);
                            user.ExpiredCode = dr["expiredCode"] as string;
                            user.EntryUser = dr["EntryUser"] as string;
                            user.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            user.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            user.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            users.Add(user);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return users;
        }

        public static IList<User> GetUserForGrid()
        {
            List<User> users = new List<User>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelUser";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@UserID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            User user = new User();
                            user.UserID = dr["UserId"] as string;
                            user.Password = dr["Password"] as string;

                            user.Branch = new GeneralTable.Branch();
                            user.Branch.BranchCode = dr["branchcode"] as string;

                            user.UserName = dr["UserName"] as string;
                            user.UserGroup = new UserGroup();
                            user.UserGroup.GroupCode = Tool.GeneralHelper.NullToString(dr["GroupCode"]);

                            user.Status = (UserStatus)Convert.ToInt32(dr["Status"]);
                            user.EmailAddress = dr["EmailAddress"] as string;
                            //user.SecurityCode = dr["SecurityCode"] as string;
                            //user.SecurityAnswer = dr["SecurityAnswer"] as string;
                            //user.Akumulasi = dr["Akumulasi"] == DBNull.Value ? 0 : Convert.ToInt16(dr["Akumulasi"]);
                            user.ExpiredCode = dr["expiredCode"] as string;
                            //user.EntryUser = dr["EntryUser"] as string;
                            //user.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            user.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            user.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            users.Add(user);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return users;
        }

        public static User GetUser(string userID)
        {
            User user = new User();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "MntSelUser";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@UserID", System.Data.SqlDbType.VarChar, userID);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        user.UserID = dr["UserId"] as string;
                        user.Password = dr["Password"] as string;

                        user.Branch = new IDS.GeneralTable.Branch();
                        user.Branch.BranchCode = Tool.GeneralHelper.NullToString(dr["BranchCode"]);

                        user.UserName = dr["UserName"] as string;

                        user.UserGroup = new Maintenance.UserGroup();
                        user.UserGroup.GroupCode = Tool.GeneralHelper.NullToString(dr["GroupCode"]);

                        user.Status = (UserStatus)Convert.ToInt32(dr["Status"]);
                        user.EmailAddress = dr["EmailAddress"] as string;
                        user.SecurityCode = dr["SecurityCode"] as string;
                        user.SecurityAnswer = dr["SecurityAnswer"] as string;
                        user.Akumulasi = dr["Akumulasi"] == DBNull.Value ? 0 : Convert.ToInt16(dr["Akumulasi"]);
                        user.ExpiredCode = dr["expiredCode"] as string;
                        //user.EntryUser = dr["EntryUser"] as string;
                        //user.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        //user.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                        //user.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return user;
        }

        public static bool UserPassChange(string userid, string oldpass, string newpass, string newpaaconfirms)
        {
            if (PassMatch(userid, oldpass))
            {
                uPDATEpASAS(newpass, userid);
                return true;
            }
            return false;

        }

        private static bool PassMatch(string userid, string pass)
        {
            bool exist_ = false;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "select userid,password from MntUser where userid=@UserId and Password=@Password";
                db.AddParameter("@UserId", System.Data.SqlDbType.VarChar, userid);
                db.AddParameter("@Password", System.Data.SqlDbType.VarChar, pass);
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        exist_ = true;
                    }
                    else
                    {
                        exist_ = false;
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return exist_;
        }//UserExist

        private static void uPDATEpASAS(string PASS, string UserId)
        {
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "update MntUser set password=@Password where userid=@UserId";
                db.AddParameter("@Password", System.Data.SqlDbType.VarChar, PASS);
                db.AddParameter("@UserId", System.Data.SqlDbType.VarChar, UserId);
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.BeginTransaction();
                int result = db.ExecuteNonQuery();
                db.CommitTransaction();
                db.Close();
            }
        }

        public static System.Data.DataTable GetWebMenuMaster()
        {
            System.Data.DataTable dt_ = new System.Data.DataTable();
            dt_.Clear();
            dt_.Columns.Add("MenuCode");
            dt_.Columns.Add("GroupCode");
            dt_.Columns.Add("FrmName");
            dt_.Columns.Add("ProjectName");
            dt_.Columns.Add("Akses");
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "select MenuCode,GroupCode,frmName,ProjectName,Akses from mntGroupAccess";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            dt_.Rows.Add(new object[] { dr["MenuCode"].ToString(), dr["GroupCode"].ToString(), dr["frmName"].ToString(), dr["ProjectName"].ToString(), dr["Akses"].ToString() });
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return dt_;
        }

        public static List<System.Web.Mvc.SelectListItem> GetMenuListForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "select ProjectName from MntMainProject union select NULL";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem acfcust = new System.Web.Mvc.SelectListItem();
                            acfcust.Value = IDS.Tool.GeneralHelper.NullToString(dr["ProjectName"]);
                            acfcust.Text = IDS.Tool.GeneralHelper.NullToString(dr["ProjectName"]);
                            list.Add(acfcust);
                        }


                    }
                }
                db.Close();
            }
            return list;
        }//GetMenuListForDataSource

        public static void GetUserFromBranch(string Branch, System.Data.DataTable dt)
        {

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("SELECT userid as username, username as fullname,");
                sb.AppendLine("emailaddress, groupcode, ENTRYDATE, expiredcode,");
                sb.AppendLine("akumulasi, status, branchcode");
                sb.AppendLine("FROM MntUser");
                sb.AppendLine("WHERE branchcode LIKE ISNULL (@Branch,'%') ORDER BY userid;");

                db.CommandText = sb.ToString();
                db.CommandType = System.Data.CommandType.Text;
                if (string.IsNullOrEmpty(Branch))
                {
                    db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Branch);
                }
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            dt.Rows.Add(new object[] { dr["username"].ToString(), dr["fullname"].ToString(), dr["emailaddress"].ToString(), dr["groupcode"].ToString(), dr["ENTRYDATE"].ToString(), dr["expiredcode"].ToString(), dr["akumulasi"].ToString(), dr["status"].ToString() });
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
        }

        public static List<System.Web.Mvc.SelectListItem> GetSecurityCode()
        {
            List<System.Web.Mvc.SelectListItem> groups = new List<System.Web.Mvc.SelectListItem>();
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SELECT SecurityCode, SecurityDesc FROM MntSec ORDER BY SecurityDesc";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem item = new System.Web.Mvc.SelectListItem();
                            item.Value = Tool.GeneralHelper.NullToString(dr["SecurityCode"]);
                            item.Text = Tool.GeneralHelper.NullToString(dr["SecurityDesc"]);
                            groups.Add(item);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }
            return groups;
        }

        public static List<System.Web.Mvc.SelectListItem> Getgroupcode()
        {
            List<System.Web.Mvc.SelectListItem> groups = new List<System.Web.Mvc.SelectListItem>();
            using (IDS.DataAccess.SqlServer db = new IDS.DataAccess.SqlServer())
            {
                db.CommandText = "SELECT groupcode, groupname FROM mntgroupuser ORDER BY groupcode";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem item = new System.Web.Mvc.SelectListItem();
                            item.Value = Tool.GeneralHelper.NullToString(dr["groupcode"]);
                            item.Text = Tool.GeneralHelper.NullToString(dr["groupname"]);
                            groups.Add(item);
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return groups;
        }

        public static bool UserExist(string userID)
        {
            bool exist_ = false;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SELECT userid FROM MntUser WHERE userid = @UserId";
                db.AddParameter("@UserId", System.Data.SqlDbType.VarChar, userID);
                db.CommandType = System.Data.CommandType.Text;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        exist_ = true;
                    }
                    else
                    {
                        exist_ = false;
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return exist_;
        }//UserExist

        public static bool SaveUser(string userID, string UserName, string Password, string EmailAddress, string GroupCode, System.DateTime createdDate, string expiredCode, string SecurityCode, string SecurityAnswer, int Akumulasi, bool Status, string BranchCode, string OperatorID)
        {
            int result = 0;
            bool success = false;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "MntSaveUser";
                    db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 0);
                    db.AddParameter("@UserId", System.Data.SqlDbType.VarChar, userID);
                    db.AddParameter("@UserName", System.Data.SqlDbType.VarChar, UserName);
                    db.AddParameter("@Password", System.Data.SqlDbType.VarChar, Password);
                    db.AddParameter("@Email", System.Data.SqlDbType.VarChar, EmailAddress);
                    db.AddParameter("@Group", System.Data.SqlDbType.VarChar, GroupCode);
                    db.AddParameter("@Date", System.Data.SqlDbType.DateTime, createdDate);
                    db.AddParameter("@exp", System.Data.SqlDbType.VarChar, expiredCode);
                    db.AddParameter("@SCode", System.Data.SqlDbType.VarChar, SecurityCode);
                    db.AddParameter("@SAnsw", System.Data.SqlDbType.VarChar, SecurityAnswer);
                    db.AddParameter("@Akumulasi", System.Data.SqlDbType.TinyInt, Akumulasi);
                    db.AddParameter("@Status", System.Data.SqlDbType.Bit, Status);
                    db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, BranchCode);
                    db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                    db.AddParameter("@ENTRYUSER", System.Data.SqlDbType.VarChar, OperatorID);
                    db.AddParameter("@LastUpdate", System.Data.SqlDbType.DateTime, System.DateTime.Now);
                    db.AddParameter("@AMGroupCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();
                    db.BeginTransaction();
                    result = db.ExecuteNonQuery();
                    db.CommitTransaction();
                    success = true;
                }
                catch (SqlException sex)
                {
                    success = false;
                    if (db.Transaction != null)
                        db.RollbackTransaction();
                }
                db.Close();
            }
            return success;
        }//UserExist

        public static bool UpdateUser(string userID, string UserName, string Password, string EmailAddress, string GroupCode, System.DateTime createdDate, string expiredCode, string SecurityCode, string SecurityAnswer, string Akumulasi, bool Status, string BranchCode, string AMGroupCode, string ENTRYUSER)
        {
            int result = 0;
            bool success = false;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "MntSaveUser";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                    db.AddParameter("@userid", System.Data.SqlDbType.VarChar, userID);
                    db.AddParameter("@username", System.Data.SqlDbType.VarChar, UserName);
                    db.AddParameter("@password", System.Data.SqlDbType.VarChar, Password);
                    db.AddParameter("@email", System.Data.SqlDbType.VarChar, EmailAddress);
                    db.AddParameter("@group", System.Data.SqlDbType.VarChar, GroupCode);
                    db.AddParameter("@date", System.Data.SqlDbType.DateTime, createdDate);
                    db.AddParameter("@exp", System.Data.SqlDbType.VarChar, expiredCode);
                    db.AddParameter("@scode", System.Data.SqlDbType.VarChar, SecurityCode);
                    db.AddParameter("@sansw", System.Data.SqlDbType.VarChar, SecurityAnswer);
                    db.AddParameter("@akumulasi", System.Data.SqlDbType.TinyInt, int.Parse(Akumulasi));
                    db.AddParameter("@status", System.Data.SqlDbType.Bit, Status);
                    db.AddParameter("@branch", System.Data.SqlDbType.VarChar, BranchCode);
                    db.AddParameter("@LastUpdate", System.Data.SqlDbType.DateTime, System.DateTime.Now);
                    db.AddParameter("@AMGroupCode", System.Data.SqlDbType.VarChar, AMGroupCode);
                    db.AddParameter("@ENTRYUSER", System.Data.SqlDbType.VarChar, ENTRYUSER);
                    db.Open();
                    db.BeginTransaction();
                    result = db.ExecuteNonQuery();
                    db.CommitTransaction();
                    success = true;
                }
                catch (SqlException sex)
                {
                    success = false;
                    if (db.Transaction != null)
                        db.RollbackTransaction();
                }
                db.Close();
            }
            return success;
        }//UserExist

        public static bool DeleteUserId(string userID)
        {
            int result = 0;
            bool success = false;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "DELETE FROM MntUser WHERE UserId=@UserId";
                    db.CommandType = System.Data.CommandType.Text;
                    db.AddParameter("@userid", System.Data.SqlDbType.VarChar, userID);
                    db.Open();
                    db.BeginTransaction();
                    result = db.ExecuteNonQuery();
                    db.CommitTransaction();
                    success = true;
                }
                catch (SqlException sex)
                {
                    success = false;
                    if (db.Transaction != null)
                        db.RollbackTransaction();
                }
                db.Close();
            }
            return success;
        }//UserExist

        public static System.Data.DataTable GetUserFromId(string userid)
        {
            System.Data.DataTable dt_ = new System.Data.DataTable();
            dt_.Clear();
            dt_.Columns.Add("userid");
            dt_.Columns.Add("username");
            dt_.Columns.Add("email");
            dt_.Columns.Add("group");
            dt_.Columns.Add("datecreated");
            dt_.Columns.Add("expired");
            dt_.Columns.Add("accum");
            dt_.Columns.Add("active");
            dt_.Columns.Add("scode");
            dt_.Columns.Add("branch");
            dt_.Columns.Add("sansw");
            IDS.Maintenance.User.GetUser(userid, dt_);
            return dt_;
        }

        private static void GetUser(string userid, System.Data.DataTable dt)
        {

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("SELECT userid as username, username as fullname,");
                sb.AppendLine("emailaddress, groupcode, ENTRYDATE, expiredcode,");
                sb.AppendLine("akumulasi, status, branchcode, SecurityCode, SecurityAnswer");
                sb.AppendLine("FROM MntUser");
                sb.AppendLine("WHERE userid =@userid ORDER BY userid;");

                db.CommandText = sb.ToString();
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@userid", System.Data.SqlDbType.VarChar, userid);
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dt.Rows.Add(new object[] { dr["username"].ToString(), dr["fullname"].ToString(), dr["emailaddress"].ToString(), dr["groupcode"].ToString(), dr["ENTRYDATE"].ToString(), dr["expiredcode"].ToString(), dr["akumulasi"].ToString(), dr["status"].ToString(), dr["SecurityCode"].ToString(), dr["branchcode"].ToString(), dr["SecurityAnswer"].ToString() });
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
        }

    }
}