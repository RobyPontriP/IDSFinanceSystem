using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDS.GLTable
{
    public class Project
    {
        [Display(Name = "Project Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project code is required")]
        [MaxLength(15), StringLength(15)]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project name is required")]
        [MaxLength(50)]
        public string ProjectName { get; set; }

        [Display(Name = "Group")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Group name is required")]
        [MaxLength(6)]
        public string Group { get; set; }

        [Display(Name = "Branch Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch Name is required")]
        public IDS.GeneralTable.Branch BranchProject { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Begining Date")]
        public DateTime BegDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Leader")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Leader name is required")]
        [MaxLength(50)]
        public string Leader { get; set; }

        [Display(Name = "Description")]
        [MaxLength(500)]
        public string Description { get; set; }

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

        public Project()
        {

        }

        public Project(string projectCode, string projectName)
        {
            ProjectCode = projectCode;
            ProjectName = projectName;
        }

        public static Project GetProject(string projectCode)
        {
            Project project = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelProject";
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, projectCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        project = new Project();
                        project.ProjectCode = dr["CODE"] as string;
                        project.ProjectName = dr["Name"] as string;
                        project.Group = dr["GRP"] as string;

                        project.BranchProject = new IDS.GeneralTable.Branch();
                        project.BranchProject.BranchCode = dr["branchcode"] as string;
                        project.BranchProject.BranchName = dr["branchname"] as string;
                        project.BranchProject.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                        project.BranchProject.NPWP = dr["NPWP"] as string;

                        project.BranchProject.BranchManagerName = dr["BranchManager"] as string;
                        project.BranchProject.FinAccOfficer = dr["FinAccOfficer"] as string;

                        project.BranchProject.Address1 = dr["Addr1"] as string;
                        project.BranchProject.Address2 = dr["Addr2"] as string;
                        project.BranchProject.Address3 = dr["Addr3"] as string;

                        project.BranchProject.PostalCode = dr["PostalCode"] as string;
                        project.BranchProject.Phone1 = dr["Phone1"] as string;
                        project.BranchProject.Phone2 = dr["Phone2"] as string;
                        project.BranchProject.Phone3 = dr["Phone3"] as string;
                        project.BranchProject.Fax = dr["Fax"] as string;

                        // Print Option
                        project.BranchProject.PrintBranchName = Convert.ToBoolean(dr["Chknama"]);
                        project.BranchProject.PrintBranchAddress = Convert.ToBoolean(dr["Chkaddress"]);
                        project.BranchProject.PrintBranchCity = Convert.ToBoolean(dr["Chkcity"]);
                        project.BranchProject.PrintBranchCountry = Convert.ToBoolean(dr["Chkcountry"]);
                        project.BranchProject.PrintDate = Convert.ToBoolean(dr["ChkPrintDate"]);
                        project.BranchProject.PrintTime = Convert.ToBoolean(dr["ChkPrintTime"]);
                        project.BranchProject.PrintPage = Convert.ToBoolean(dr["ChkPage"]);
                        //project.BranchProject.Language = Convert.ToBoolean(dr["OptIndex"]);

                        // Log
                        project.BranchProject.EntryUser = dr["EntryUser"] as string;
                        project.BranchProject.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        project.BranchProject.OperatorID = dr["OperatorID"] as string;
                        project.BranchProject.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                        project.BegDate = Convert.ToDateTime(dr["BegDate"]);
                        project.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        project.Leader = dr["Leader"] as string;
                        project.Description = dr["Descrip"] as string;
                        project.EntryUser = dr["EntryUser"] as string;
                        project.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        project.OperatorID = dr["OperatorID"] as string;
                        project.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return project;
        }

        public static Project GetProject(string projectCode, string branchCode)
        {
            Project project = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelProject";
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, projectCode);
                db.AddParameter("@branchcode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        project = new Project();
                        project.ProjectCode = dr["CODE"] as string;
                        project.ProjectName = dr["Name"] as string;
                        project.Group = dr["GRP"] as string;

                        project.BranchProject = new IDS.GeneralTable.Branch();
                        project.BranchProject.BranchCode = dr["branchcode"] as string;
                        project.BranchProject.BranchName = dr["branchname"] as string;
                        project.BranchProject.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                        project.BranchProject.NPWP = dr["NPWP"] as string;

                        project.BranchProject.BranchManagerName = dr["BranchManager"] as string;
                        project.BranchProject.FinAccOfficer = dr["FinAccOfficer"] as string;

                        project.BranchProject.Address1 = dr["Addr1"] as string;
                        project.BranchProject.Address2 = dr["Addr2"] as string;
                        project.BranchProject.Address3 = dr["Addr3"] as string;

                        project.BranchProject.PostalCode = dr["PostalCode"] as string;
                        project.BranchProject.Phone1 = dr["Phone1"] as string;
                        project.BranchProject.Phone2 = dr["Phone2"] as string;
                        project.BranchProject.Phone3 = dr["Phone3"] as string;
                        project.BranchProject.Fax = dr["Fax"] as string;

                        // Print Option
                        project.BranchProject.PrintBranchName = Convert.ToBoolean(dr["Chknama"]);
                        project.BranchProject.PrintBranchAddress = Convert.ToBoolean(dr["Chkaddress"]);
                        project.BranchProject.PrintBranchCity = Convert.ToBoolean(dr["Chkcity"]);
                        project.BranchProject.PrintBranchCountry = Convert.ToBoolean(dr["Chkcountry"]);
                        project.BranchProject.PrintDate = Convert.ToBoolean(dr["ChkPrintDate"]);
                        project.BranchProject.PrintTime = Convert.ToBoolean(dr["ChkPrintTime"]);
                        project.BranchProject.PrintPage = Convert.ToBoolean(dr["ChkPage"]);
                        //project.BranchProject.Language = Convert.ToBoolean(dr["OptIndex"]);

                        // Log
                        project.BranchProject.EntryUser = dr["EntryUser"] as string;
                        project.BranchProject.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        project.BranchProject.OperatorID = dr["OperatorID"] as string;
                        project.BranchProject.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                        project.BegDate = Convert.ToDateTime(dr["BegDate"]);
                        project.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        project.Leader = dr["Leader"] as string;
                        project.Description = dr["Descrip"] as string;
                        project.EntryUser = dr["EntryUser"] as string;
                        project.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        project.OperatorID = dr["OperatorID"] as string;
                        project.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return project;
        }

        /// <summary>
        /// Retrieve semua daftar Project
        /// </summary>
        /// <returns></returns>
        public static List<Project> GetProject()
        {
            List<IDS.GLTable.Project> list = new List<Project>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelProject";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            Project project = new Project();
                            project.ProjectCode = dr["CODE"] as string;
                            project.ProjectName = dr["Name"] as string;
                            project.Group = dr["GRP"] as string;

                            project.BranchProject = new IDS.GeneralTable.Branch();
                            project.BranchProject.BranchCode = dr["branchcode"] as string;
                            project.BranchProject.BranchName = dr["branchname"] as string;
                            project.BranchProject.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            project.BranchProject.NPWP = dr["NPWP"] as string;

                            project.BranchProject.BranchManagerName = dr["BranchManager"] as string;
                            project.BranchProject.FinAccOfficer = dr["FinAccOfficer"] as string;

                            project.BranchProject.Address1 = dr["Addr1"] as string;
                            project.BranchProject.Address2 = dr["Addr2"] as string;
                            project.BranchProject.Address3 = dr["Addr3"] as string;

                            project.BranchProject.PostalCode = dr["PostalCode"] as string;
                            project.BranchProject.Phone1 = dr["Phone1"] as string;
                            project.BranchProject.Phone2 = dr["Phone2"] as string;
                            project.BranchProject.Phone3 = dr["Phone3"] as string;
                            project.BranchProject.Fax = dr["Fax"] as string;

                            // Print Option
                            project.BranchProject.PrintBranchName = Convert.ToBoolean(dr["Chknama"]);
                            project.BranchProject.PrintBranchAddress = Convert.ToBoolean(dr["Chkaddress"]);
                            project.BranchProject.PrintBranchCity = Convert.ToBoolean(dr["Chkcity"]);
                            project.BranchProject.PrintBranchCountry = Convert.ToBoolean(dr["Chkcountry"]);
                            project.BranchProject.PrintDate = Convert.ToBoolean(dr["ChkPrintDate"]);
                            project.BranchProject.PrintTime = Convert.ToBoolean(dr["ChkPrintTime"]);
                            project.BranchProject.PrintPage = Convert.ToBoolean(dr["ChkPage"]);
                            //project.BranchProject.Language = Convert.ToBoolean(dr["OptIndex"]);

                            project.BegDate = Convert.ToDateTime(dr["BegDate"]);
                            project.EndDate = Convert.ToDateTime(dr["EndDate"]);
                            project.Leader = dr["Leader"] as string;
                            project.Description = dr["Descrip"] as string;
                            project.EntryUser = dr["EntryUser"] as string;
                            project.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            project.OperatorID = dr["OperatorID"] as string;
                            project.LastUpdate = Convert.ToDateTime(dr["prjLastUpdate"]);

                            list.Add(project);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        /// <summary>
        /// Retrieve semua daftar country untuk datasource
        /// </summary>
        /// <returns></returns>

        public int InsUpDelProject(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLupdateProject";
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, ProjectCode);
                    cmd.AddParameter("@Name", System.Data.SqlDbType.VarChar, ProjectName);
                    cmd.AddParameter("@Grp", System.Data.SqlDbType.VarChar, Group);
                    cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchProject.BranchCode);
                    cmd.AddParameter("@BegDate", System.Data.SqlDbType.DateTime, BegDate);
                    cmd.AddParameter("@EndDate", System.Data.SqlDbType.DateTime, EndDate);
                    cmd.AddParameter("@Leader", System.Data.SqlDbType.VarChar, Leader);
                    cmd.AddParameter("@Descrip", System.Data.SqlDbType.VarChar, Description);
                    cmd.AddParameter("@operatorID", System.Data.SqlDbType.VarChar, OperatorID);
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
                            throw new Exception("Project code is already exists. Please choose other Project code.");
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

        public int InsUpDelProject(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLupdateProject";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GLupdateProject";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@Code", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Project Code is already exists. Please choose other Project Code.");
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

        public static List<SelectListItem> GetProjectForDataSource()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelProject";
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string projectCode = dr["CODE"].ToString();
                            string projectName = dr["Name"].ToString();
                            list.Add(new SelectListItem() { Text = projectName, Value = projectCode });
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<SelectListItem> GetProjectForDataSourceWithBranch(string branchCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelProject";
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 4);
                db.AddParameter("@branchcode", System.Data.SqlDbType.VarChar, branchCode);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string projectCode = dr["CODE"].ToString();
                            string projectName = dr["Name"].ToString();
                            list.Add(new SelectListItem() { Text = projectName, Value = projectCode });
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
