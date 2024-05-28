using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GeneralTable
{
    public class Staff
    {
        [Display(Name = "Staff Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Staff code is required")]
        [MaxLength(20), StringLength(20)]
        public string StaffCode { get; set; }

        [Display(Name = "Staff Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Staff name is required")]
        [MaxLength(50)]
        public string StaffName { get; set; }

        [Display(Name = "Department Name")]
        public Department Department { get; set; }

        [Display(Name = "Division Name")]
        public Division Division { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public IDS.Tool.Gender Gender { get; set; }

        [Display(Name = "Created By")]
        [MaxLength(20), StringLength(20)]
        public string EntryUser { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        [MaxLength(20), StringLength(20)]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public Staff()
        {
            //this.Country = new Country();
        }

        public Staff(string staffCode, string staffName) : this()
        {
            StaffCode = staffCode;
            StaffName = staffName;
        }

        public static Staff GetStaff(string staffCode)
        {
            Staff staff = new Staff();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelStaff";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, staffCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            staff.StaffCode = IDS.Tool.GeneralHelper.NullToString(dr["Code"]);
                            staff.StaffName = IDS.Tool.GeneralHelper.NullToString(dr["Name"]);
                            staff.Department = new Department(IDS.Tool.GeneralHelper.NullToString(dr["deptcode"]), IDS.Tool.GeneralHelper.NullToString(dr["deptname"]));
                            staff.Division = new Division(IDS.Tool.GeneralHelper.NullToString(dr["divcode"]), IDS.Tool.GeneralHelper.NullToString(dr["divdesc"]));
                            staff.Gender = (IDS.Tool.Gender)IDS.Tool.GeneralHelper.NullToInt(dr["Sex"], 0);
                            staff.OperatorID = IDS.Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            staff.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return staff;
        }

        /// <summary>
        /// Retrieve semua daftar Staff
        /// </summary>
        /// <returns></returns>
        public static List<Staff> GetStaff()
        {
            List<IDS.GeneralTable.Staff> list = new List<Staff>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelStaff";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@name", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        list = new List<Staff>();

                        while (dr.Read())
                        {
                            Staff staff = new Staff();
                            staff.StaffCode = IDS.Tool.GeneralHelper.NullToString(dr["Code"]);
                            staff.StaffName = IDS.Tool.GeneralHelper.NullToString(dr["Name"]);
                            staff.Department = new Department(IDS.Tool.GeneralHelper.NullToString(dr["deptcode"]), IDS.Tool.GeneralHelper.NullToString(dr["deptname"]));
                            staff.Division = new Division(IDS.Tool.GeneralHelper.NullToString(dr["divisicode"]), IDS.Tool.GeneralHelper.NullToString(dr["divdesc"]));
                            staff.Gender = (IDS.Tool.Gender)IDS.Tool.GeneralHelper.NullToInt(dr["Sex"], 0);
                            staff.OperatorID = IDS.Tool.GeneralHelper.NullToString(dr["OperatorID"]);
                            staff.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(staff);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            //if (list.Count > 0)
            //    list = list.OrderBy(x => x.Country.CountryName).ThenBy(x => x.CityName).ToList();

            return list;
        }

        /// <summary>
        /// Retrieve Staff berdasarkan kode parameter Staff
        /// </summary>
        /// <param name="staffCode"></param>
        /// <returns></returns>
        //public static List<Staff> GetStaff(string staffCode)
        //{
        //    List<Staff> list = new List<Staff>();

        //    using (DataAccess.SqlServer db = new DataAccess.SqlServer())
        //    {
        //        db.CommandText = "GTSelStaff";
        //        db.AddParameter("@code", System.Data.SqlDbType.VarChar, staffCode);
        //        db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
        //        db.CommandType = System.Data.CommandType.StoredProcedure;
        //        db.Open();

        //        db.ExecuteReader();

        //        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
        //        {
        //            if (dr.HasRows)
        //            {
        //                while (dr.Read())
        //                {
        //                    Staff staff = new Staff();
        //                    staff.StaffCode = IDS.Tool.GeneralHelper.NullToString(dr["Code"]);
        //                    staff.StaffName = IDS.Tool.GeneralHelper.NullToString(dr["Name"]);
        //                    staff.Department = new Department(IDS.Tool.GeneralHelper.NullToString(dr["deptcode"]), IDS.Tool.GeneralHelper.NullToString(dr["deptname"]));
        //                    staff.Division = new Division(IDS.Tool.GeneralHelper.NullToString(dr["divcode"]), IDS.Tool.GeneralHelper.NullToString(dr["divdesc"]));
        //                    staff.Gender = (IDS.Tool.Gender)(dr["Sex"]);
        //                    staff.OperatorID = IDS.Tool.GeneralHelper.NullToString(dr["OperatorID"]);
        //                    staff.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

        //                    list.Add(staff);
        //                }

        //            }

        //            if (!dr.IsClosed)
        //                dr.Close();
        //        }

        //        db.Close();
        //    }

        //    //if (list.Count > 0)
        //    //    list = list.OrderBy(x => x.Country.CountryName).ThenBy(x => x.CityName).ToList();

        //    return list;
        //}

        /// <summary>
        /// Untuk Insert, Update, Delete Data
        /// </summary>
        /// <param name="ExecCode"></param>
        /// <returns></returns>
        public int InsUpDelStaff(Tool.PageActivity ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdateStaff";
                    cmd.AddParameter("@code", System.Data.SqlDbType.VarChar, StaffCode);
                    cmd.AddParameter("@Name", System.Data.SqlDbType.VarChar, StaffName);
                    cmd.AddParameter("@deptcode", System.Data.SqlDbType.VarChar, Department.DepartmentCode);
                    cmd.AddParameter("@divisicode", System.Data.SqlDbType.VarChar, Division.DivisiCode);
                    cmd.AddParameter("@sex", System.Data.SqlDbType.TinyInt, (int)Gender);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID.ToString());
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, (int)ExecCode);
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
                            throw new Exception("Staff code is already exists. Please choose other staff code.");
                        case 547:
                            throw new Exception("Data can not be delete while data used for reference.");
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

        /// <summary>
        /// Untuk delete data
        /// </summary>
        /// <param name="ExecCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int InsUpDelStaff(Tool.PageActivity ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdateStaff";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GTUpdateStaff";
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, (int)ExecCode);
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
                            throw new Exception("Staff Code is already exists. Please choose other Staff Code.");
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

        //public static List<System.Web.Mvc.SelectListItem> GetTypeAccForDatasource()
        //{
        //    List<System.Web.Mvc.SelectListItem> typeAcc = new List<System.Web.Mvc.SelectListItem>();
        //    typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "Male", Value = IDS.Tool.GLSpecialAccount.PL.ToString() });
        //    typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "AR - Account Receivable", Value = IDS.Tool.GLSpecialAccount.AR.ToString() });
        //    typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "AP - Account Payable", Value = IDS.Tool.GLSpecialAccount.AP.ToString() });
        //    typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "DL - Daily Balance Account", Value = IDS.Tool.GLSpecialAccount.DL.ToString() });
        //    typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "KS - Cash Account", Value = IDS.Tool.GLSpecialAccount.KS.ToString() });
        //    typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "BN - Bank Account", Value = IDS.Tool.GLSpecialAccount.BN.ToString() });
        //    typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "RE - Revaluation Account", Value = IDS.Tool.GLSpecialAccount.RE.ToString() });
        //    typeAcc.Add(new System.Web.Mvc.SelectListItem() { Text = "BY - Expenses / Income Account", Value = IDS.Tool.GLSpecialAccount.BY.ToString() });


        //    return typeAcc;
        //}
    }
}