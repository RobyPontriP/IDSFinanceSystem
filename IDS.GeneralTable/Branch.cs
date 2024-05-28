using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace IDS.GeneralTable
{
    public class Branch
    {
        #region Properties
        [Display(Name = "Branch Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch code is required")]
        [MaxLength(5), StringLength(5)]
        public string BranchCode { get; set; }

        [Display(Name = "Branch Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch name is required")]
        [MaxLength(30), StringLength(30)]
        public string BranchName { get; set; }

        public decimal ContractLimit { get; set; }

        [Display(Name = "Branch Manager Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch manager name is required")]
        [MaxLength(100), StringLength(100)]
        /// <summary>
        /// Nama Branch Manager Cabang
        /// </summary>

        public string BranchManagerName { get; set; }

        [Display(Name = "Financial Officer")]
        [MaxLength(30), StringLength(30)]
        /// <summary>
        /// Nama Financial Account Officer Cabang
        /// </summary>
        public string FinAccOfficer { get; set; }

        [Display(Name = "NPWP")]
        [MaxLength(20), StringLength(20)]
        /// <summary>
        /// Nomor NPWP Cabang
        /// </summary>
        public string NPWP { get; set; }

        [Display(Name = "HO Status")]
        /// <summary>
        /// Status cabang atau kantor pusat.
        /// True = Kantor Pusat, False = cabang
        /// </summary>
        public bool HOStatus { get; set; }

        public BranchLeaseDefaultValue BranchLease { get; set; }

        // Branch Contact
        [Display(Name = "Address 1")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address 1 is required")]
        [MaxLength(50), StringLength(50)]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        [MaxLength(50), StringLength(50)]
        public string Address2 { get; set; }

        [Display(Name = "Address 3")]
        [MaxLength(50), StringLength(50)]
        public string Address3 { get; set; }

        [Display(Name = "Branch City")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch city is required")]
        public City BranchCity { get; set; }

        [Display(Name = "Branch Country")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch country is required")]
        public Country BranchCountry { get; set; }

        [NotMapped]
        public string FullAddress
        {
            get
            {
                return (Address1 ?? "" + Address2 ?? "" + Address3 ?? "" + BranchCity?.CityCode ?? "" + BranchCountry?.CountryCode ?? "");
            }
        }

        [Display(Name = "Postal Code")]
        [Required(AllowEmptyStrings = true)]
        [MaxLength(5), StringLength(5)]
        public string PostalCode { get; set; }

        [Display(Name = "Phone 1")]
        [Required(AllowEmptyStrings = true)]
        [MaxLength(15), StringLength(15)]
        public string Phone1 { get; set; }

        [Display(Name = "Phone 2")]
        [MaxLength(15), StringLength(15)]
        public string Phone2 { get; set; }

        [Display(Name = "Phone 3")]
        [MaxLength(15), StringLength(15)]
        public string Phone3 { get; set; }

        [Display(Name = "Fax")]
        [MaxLength(15), StringLength(15)]
        public string Fax { get; set; }

        public string Telex { get; set; }

        //public string FinAccOfficer { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATE_FORMAT, ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime TaxEstablishDate { get; set; }
        public string BranchManager { get; set; }

        public string Repno { get; set; }
        public string Hopno { get; set; }
        public string TaxSignBy { get; set; }
        public string TaxOccupation { get; set; }
        public string InvSignBy { get; set; }
        public string InvOccupation { get; set; }


        // Report Print Setting
        [Display(Name = "Print Branch Name")]
        public bool PrintBranchName { get; set; }
        [Display(Name = "Print Branch Address")]
        public bool PrintBranchAddress { get; set; }
        [Display(Name = "Print Branch City")]
        public bool PrintBranchCity { get; set; }
        [Display(Name = "Print Branch Country")]
        public bool PrintBranchCountry { get; set; } // Old
        [Display(Name = "print Page Number")]
        public bool PrintPage { get; set; }
        [Display(Name = "Print Date")]
        public bool PrintDate { get; set; }
        [Display(Name = "Print Time")]
        public bool PrintTime { get; set; }
        [Display(Name = "Language")]
        public string Language { get; set; } //OptIndex

        //[Display(Name = "Language")]
        //public bool Language { get; set; } //OptIndex

        public bool OptIndex { get; set; }
        // Log
        [Display(Name = "Created By")]
        public string EntryUser { get; set; }
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }
        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }
        #endregion // Properties


        public Branch()
        {
            BranchLease = new BranchLeaseDefaultValue();
        }

        public Branch(string branchCode, string branchName)
        {
            BranchCode = branchCode;
            BranchName = BranchName;
        }

        public int InsUpDel(IDS.Tool.PageActivity FormState)
        {
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                try
                {
                    db.CommandText = "GTUpdateBranch";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();

                    db.BeginTransaction();

                    db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, BranchCode);
                    db.AddParameter("@BranchName", System.Data.SqlDbType.VarChar, BranchName);

                    db.AddParameter("@Addr1", System.Data.SqlDbType.VarChar, Address1);
                    db.AddParameter("@Addr2", System.Data.SqlDbType.VarChar, Address2);
                    db.AddParameter("@Addr3", System.Data.SqlDbType.VarChar, Address3);
                    db.AddParameter("@CityCode", System.Data.SqlDbType.VarChar, BranchCity?.CityCode);
                    db.AddParameter("@CountryCode", System.Data.SqlDbType.VarChar, BranchCountry?.CountryCode); // New
                    db.AddParameter("@PostalCode", System.Data.SqlDbType.VarChar, PostalCode);
                    db.AddParameter("@Phone1", System.Data.SqlDbType.VarChar, Phone1);
                    db.AddParameter("@Phone2", System.Data.SqlDbType.VarChar, Phone2);
                    db.AddParameter("@Phone3", System.Data.SqlDbType.VarChar, Phone3);
                    db.AddParameter("@Fax", System.Data.SqlDbType.VarChar, Fax);

                    db.AddParameter("@FinAccOfficer", System.Data.SqlDbType.VarChar, FinAccOfficer);
                    db.AddParameter("@LastReceiptNo", System.Data.SqlDbType.Int, 0);

                    db.AddParameter("@HOStatus", System.Data.SqlDbType.Bit, HOStatus);
                    db.AddParameter("@Chknama", System.Data.SqlDbType.Bit, PrintBranchName);
                    db.AddParameter("@Chkaddress", System.Data.SqlDbType.Bit, PrintBranchAddress);
                    db.AddParameter("@Chkcity", System.Data.SqlDbType.Bit, PrintBranchCity);
                    db.AddParameter("@Chkcountry", System.Data.SqlDbType.Bit, PrintBranchCountry);
                    db.AddParameter("@ChkPrintDate", System.Data.SqlDbType.Bit, PrintDate);
                    db.AddParameter("@ChkPrintTime", System.Data.SqlDbType.Bit, PrintTime);
                    db.AddParameter("@ChkPage", System.Data.SqlDbType.Bit, bool.Parse(Language));
                    db.AddParameter("@OptIndex", System.Data.SqlDbType.Bit, OptIndex);

                    db.AddParameter("@NPWP", System.Data.SqlDbType.VarChar, NPWP);
                    db.AddParameter("@BranchManager", System.Data.SqlDbType.VarChar, BranchManagerName);

                    db.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);

                    db.AddParameter("@TaxSignBy", System.Data.SqlDbType.VarChar, TaxSignBy);//
                    db.AddParameter("@InvSignBy", System.Data.SqlDbType.VarChar, InvSignBy);
                    db.AddParameter("@InvOccupation", System.Data.SqlDbType.VarChar, InvOccupation);
                    db.AddParameter("@TaxOccupation", System.Data.SqlDbType.VarChar, TaxOccupation);
                    db.AddParameter("@Telex", System.Data.SqlDbType.VarChar, Telex);
                    db.AddParameter("@TaxEstablishDate", System.Data.SqlDbType.DateTime, TaxEstablishDate);

                    db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, FormState);

                    db.AddParameter("@ContractLimit", System.Data.SqlDbType.Money, Convert.ToDouble(ContractLimit));
                    //db.AddParameter("@OverRate", System.Data.SqlDbType.Float, Convert.ToDouble(OverRate));
                    //db.AddParameter("@OverRateRetail", System.Data.SqlDbType.Float, Convert.ToDouble(OverRateRetail));

                    //db.AddParameter("@NUPApprovalLimit", System.Data.SqlDbType.Money, NUPApprovalLimit);
                    //db.AddParameter("@NUPExpiredDay", System.Data.SqlDbType.Int, Convert.ToInt32(NUPExpiredDays));
                    ////db.AddParameter("@officeDesc", System.Data.SqlDbType.VarChar, mdl.NullParamStr(txtBM.Text).Value);

                    //db.AddParameter("@PenaltyRate", System.Data.SqlDbType.Float, Convert.ToDouble(PenaltyRate));
                    //db.AddParameter("@PenaltyRateRetail", System.Data.SqlDbType.Float, Convert.ToDouble(PenaltyRateRetail));
                    //db.AddParameter("@SLIKCode", System.Data.SqlDbType.VarChar, SLIKCode);

                    db.AddParameter("@RepNo", System.Data.SqlDbType.VarChar, Repno);
                    db.AddParameter("@HopNo", System.Data.SqlDbType.VarChar, Hopno);

                    db.ExecuteNonQuery();
                    db.CommitTransaction();

                    return 1;
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Branch code is already exists. Please choose other branch code");
                        default:
                            throw;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public int InsUpDel(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdateBranch";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GTUpdateBranch";
                        cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, data[i]);
                        cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommitTransaction();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Brand ID is already exists. Please choose other Brand ID.");
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

        /// <summary>
        /// Retrieve data branch berdasarkan branchCode
        /// </summary>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        public static Branch GetBranch(string branchCode)
        {
            Branch branch = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBranch";
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();


                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        branch = new Branch();
                        branch.BranchCode = dr["branchcode"] as string;
                        branch.BranchName = dr["branchname"] as string;
                        branch.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                        branch.NPWP = dr["NPWP"] as string;

                        branch.BranchManagerName = dr["BranchManager"] as string;
                        branch.FinAccOfficer = dr["FinAccOfficer"] as string;

                        // Contact
                        branch.Address1 = Tool.GeneralHelper.NullToString(dr["Addr1"]);
                        branch.Address2 = Tool.GeneralHelper.NullToString(dr["Addr2"]);
                        branch.Address3 = Tool.GeneralHelper.NullToString(dr["Addr3"]);

                        branch.BranchCountry = new Country();
                        branch.BranchCountry.CountryCode = Tool.GeneralHelper.NullToString(dr["CountryCode"]);
                        branch.BranchCountry.CountryName = Tool.GeneralHelper.NullToString(dr["CountryName"]);

                        branch.BranchCity = new City();
                        branch.BranchCity.CityCode = Tool.GeneralHelper.NullToString(dr["CityCode"]);
                        branch.BranchCity.CityName = Tool.GeneralHelper.NullToString(dr["CityName"]);
                        branch.BranchCity.Country = branch.BranchCountry;
                        branch.PostalCode = Tool.GeneralHelper.NullToString(dr["PostalCode"]);

                        branch.Phone1 = Tool.GeneralHelper.NullToString(dr["Phone1"]);
                        branch.Phone2 = Tool.GeneralHelper.NullToString(dr["Phone2"]);
                        branch.Phone3 = Tool.GeneralHelper.NullToString(dr["Phone3"]);
                        branch.Fax = Tool.GeneralHelper.NullToString(dr["Fax"]);

                        branch.PrintBranchName = Convert.ToBoolean(dr["Chknama"]);
                        branch.PrintBranchAddress = Convert.ToBoolean(dr["Chkaddress"]);
                        branch.PrintBranchCity = Convert.ToBoolean(dr["Chkcity"]);
                        branch.PrintBranchCountry = Convert.ToBoolean(dr["Chkcountry"]);
                        branch.PrintPage = Convert.ToBoolean(dr["ChkPage"]);
                        branch.PrintDate = Convert.ToBoolean(dr["ChkPrintDate"]);
                        branch.PrintTime = Convert.ToBoolean(dr["ChkPrintTime"]);
                        branch.OptIndex = Convert.ToBoolean(dr["OptIndex"]);

                        branch.Hopno = Tool.GeneralHelper.NullToString(dr["HopNo"]);
                        branch.Repno = Tool.GeneralHelper.NullToString(dr["RepNo"]);
                        branch.TaxEstablishDate = Tool.GeneralHelper.NullToDateTime(dr["TaxEstablishDate"], System.DateTime.Now);
                        branch.InvSignBy = Tool.GeneralHelper.NullToString(dr["InvSignBy"]);
                        branch.InvOccupation = Tool.GeneralHelper.NullToString(dr["InvOccupation"]);
                        branch.TaxSignBy = Tool.GeneralHelper.NullToString(dr["TaxSignBy"]);
                        branch.TaxOccupation = Tool.GeneralHelper.NullToString(dr["TaxOccupation"]);
                        branch.Telex = Tool.GeneralHelper.NullToString(dr["Telex"]);
                        // Log
                        branch.EntryUser = dr["EntryUser"] as string;
                        branch.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        branch.OperatorID = dr["OperatorID"] as string;
                        branch.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }
                }

                db.Close();
            }

            return branch;
        }

        /// <summary>
        /// Retrieve semua data branch
        /// </summary>
        /// <returns></returns>
        public static List<Branch> GetBranch()
        {
            List<Branch> branches = new List<Branch>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBranch";
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();


                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Branch branch = new Branch();
                            branch = new Branch();
                            branch.BranchCode = dr["branchcode"] as string;
                            branch.BranchName = dr["branchname"] as string;
                            branch.HOStatus = dr["HOStatus"] is DBNull ? false : Convert.ToBoolean(dr["HOStatus"]);
                            branch.NPWP = dr["NPWP"] as string;

                            branch.BranchManagerName = dr["BranchManager"] as string;
                            branch.FinAccOfficer = dr["FinAccOfficer"] as string;

                            branch.Address1 = dr["Addr1"] as string;
                            branch.Address2 = dr["Addr2"] as string;
                            branch.Address3 = dr["Addr3"] as string;
                            branch.BranchCountry = Country.GetCountry(Tool.GeneralHelper.NullToString(dr["CountryCode"]));
                            branch.BranchCity = City.GetCity(Tool.GeneralHelper.NullToString(dr["CityCode"]), Tool.GeneralHelper.NullToString(dr["CountryCode"]));

                            branch.PostalCode = dr["PostalCode"] as string;
                            branch.Phone1 = dr["Phone1"] as string;
                            branch.Phone2 = dr["Phone2"] as string;
                            branch.Phone3 = dr["Phone3"] as string;
                            branch.Fax = dr["Fax"] as string;

                            // Print Option
                            branch.PrintBranchName = Convert.ToBoolean(dr["Chknama"]);
                            branch.PrintBranchAddress = Convert.ToBoolean(dr["Chkaddress"]);
                            branch.PrintBranchCity = Convert.ToBoolean(dr["Chkcity"]);
                            branch.PrintBranchCountry = Convert.ToBoolean(dr["Chkcountry"]);
                            branch.PrintDate = Convert.ToBoolean(dr["ChkPrintDate"]);
                            branch.PrintTime = Convert.ToBoolean(dr["ChkPrintTime"]);
                            branch.PrintPage = Convert.ToBoolean(dr["ChkPage"]);
                            branch.OptIndex = Convert.ToBoolean(dr["OptIndex"]);

                            // Log
                            branch.EntryUser = dr["EntryUser"] as string;
                            branch.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            branch.OperatorID = dr["OperatorID"] as string;
                            branch.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            branches.Add(branch);
                        }
                    }
                }

                db.Close();
            }

            return branches;
        }

        public static List<SelectListItem> GetBranchForDatasource()
        {
            List<SelectListItem> branches = new List<SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBranch";
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SelectListItem branch = new SelectListItem(); //<string, string>(dr["BranchCode"].ToString(), dr["BranchName"].ToString());
                            branch.Text = dr["BranchName"].ToString();
                            branch.Value = dr["BranchCode"].ToString();
                            branches.Add(branch);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            branches = branches.OrderBy(x => x.Text).ToList();

            return branches;
        }

        public static object GetItem_from_Branch(string itemName, string branccode)
        {
            object return_ = "";
            string Query = "select top 1 @item from tblBranch where BranchCode=@branch";
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = Query;
                db.AddParameter("@item", System.Data.SqlDbType.VarChar, itemName);
                db.AddParameter("@branch", System.Data.SqlDbType.VarChar, branccode);
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        return_ = dr[0];
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
            return return_;
        }

        public static Decimal GetContract_limit(string itemName, string branccode)
        {
            Decimal return_ = 0;
            string Query = "select top 1 " + itemName + " from tblBranch where BranchCode='" + branccode + "'";
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = Query;
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        return_ = Tool.GeneralHelper.NullToDecimal(dr[0], 0);
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
            return return_;
        }

        public static DateTime GetTaxEstablishDate(string itemName, string branccode)
        {
            DateTime return_ = System.DateTime.Now;
            string Query = "select top 1 " + itemName + " from tblBranch where BranchCode='" + branccode + "'";
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = Query;
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        return_ = Tool.GeneralHelper.NullToDateTime(dr[0], System.DateTime.Now);
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
            return return_;
        }

        public static bool GetHOStatus()
        {
            bool return_ = false;
            string Query = "select top 1 HOStatus from tblBranch where HOStatus=1";
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = Query;
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        return_ = true;
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
            return return_;
        }

        public static List<SelectListItem> GetLanguage()
        {
            List<System.Web.Mvc.SelectListItem> RP = new List<System.Web.Mvc.SelectListItem>();
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "English", Value = "True" });
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "Indonesia", Value = "False" });
            return RP;
        }

        public static List<SelectListItem> GetSignBY()
        {
            List<SelectListItem> l = new List<SelectListItem>();
            StringBuilder b = new StringBuilder();
            b.AppendLine("SELECT mntUser.UserId, mntUser.username FROM mntUser ORDER BY mntUser.username;");
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = b.ToString();
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SelectListItem lchild = new SelectListItem(); //<string, string>(dr["BranchCode"].ToString(), dr["BranchName"].ToString());
                            lchild.Text = dr["username"].ToString();
                            lchild.Value = dr["UserId"].ToString();
                            l.Add(lchild);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
            return l;
        }

        public static List<SelectListItem> GetRepNo()
        {
            List<SelectListItem> l = new List<SelectListItem>();
            StringBuilder b = new StringBuilder();
            b.AppendLine("select acc, acc+' - '+ name as nama from ACFGLMH where ccy='IDR' and at=0");
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = b.ToString();
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SelectListItem lchild = new SelectListItem(); //<string, string>(dr["BranchCode"].ToString(), dr["BranchName"].ToString());
                            lchild.Text = dr["nama"].ToString();
                            lchild.Value = dr["acc"].ToString();
                            l.Add(lchild);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
            return l;
        }

        public static List<SelectListItem> GetBranchForDatasource(string branchCode)
        {
            List<SelectListItem> branches = new List<SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBranch";
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
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
                            SelectListItem branch = new SelectListItem(); //<string, string>(dr["BranchCode"].ToString(), dr["BranchName"].ToString());
                            branch.Text = dr["BranchName"].ToString();
                            branch.Value = dr["BranchCode"].ToString();
                            branches.Add(branch);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            branches = branches.OrderBy(x => x.Text).ToList();

            return branches;
        }

        public static List<System.Web.Mvc.SelectListItem> GetSignByForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> signBys = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBranch";
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        signBys = new List<System.Web.Mvc.SelectListItem>();
                        string value = "";
                        while (dr.Read())
                        {

                            value = IDS.Tool.GeneralHelper.NullToString(dr["InvSignBy"]) + " - " + IDS.Tool.GeneralHelper.NullToString(dr["InvOccupation"] + ",") + IDS.Tool.GeneralHelper.NullToString(dr["TaxSignBy"]) + " - " + IDS.Tool.GeneralHelper.NullToString(dr["TaxOccupation"] + ",");
                            //value = IDS.Tool.GeneralHelper.NullToString(dr["SignBy1"]) + "," + IDS.Tool.GeneralHelper.NullToString(dr["SignBy2"]) + ",";

                            string[] valuesCode = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            for (int i = 0; i < valuesCode.Length; i++)
                            {
                                System.Web.Mvc.SelectListItem signBy = new System.Web.Mvc.SelectListItem();
                                if (valuesCode[i] != " - ")
                                {
                                    signBy.Value = valuesCode[i];
                                    signBy.Text = valuesCode[i];

                                    signBys.Add(signBy);
                                }
                            }


                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return signBys;
        }
    }








    /// <summary>
    /// Class untuk default value leasing per branch
    /// </summary>
    public class BranchLeaseDefaultValue
    {
        [Display(Name = "Contract Limit")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contract Limit is required")]
        public decimal ContractLimit { get; set; }

        [Display(Name = "NUP Approval Limit")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "NUP Approval Limit is required")]
        public decimal NUPApprovalLimit { get; set; }

        [Display(Name = "NUP Expired Days")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "NUP Expired Days is required")]
        public int NUPExpiredDays { get; set; }

        [Display(Name = "Penalty Rate")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Penalty Rate for corporate funding is required")]
        public decimal PenaltyRate { get; set; } // Untuk Pembiayaan Corporate

        [Display(Name = "Penalty Rate Retail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Penalty Rate for retail funding is required")]
        public decimal PenaltyRateRetail { get; set; } // Untuk Pembiayaan Retail

        [Display(Name = "Over Rate")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Over rate for corporate funding is required")]
        public decimal OverRate { get; set; } // Untuk Pembiayaan Corporate

        [Display(Name = "Over Rate Retail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contract Limit is required")]
        public decimal OverRateRetail { get; set; } // Untuk Pembiayaan Retail

        [NotMapped]
        [Display(Name = "Last Receipt No")]
        [Required(AllowEmptyStrings = false)]
        [DefaultValue(0)]
        public int LastReceiptNo { get; set; }

        [NotMapped]
        [Display(Name = "Last Contract Count")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Contract Count is required")]
        [DefaultValue(0)]
        public int LastCount { get; set; } // Nomor Urut terakhir nomor kontrak 

        [Display(Name = "SLIK Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "SLIK code is required")]
        [MaxLength(100), StringLength(100)]
        /// <summary>
        /// Kode Cabang untuk pelaporan SLIK
        /// </summary>
        public string SLIKCode { get; set; }

        public BranchLeaseDefaultValue()
        {
        }

        public void GetBranchLeaseDefaultValue(string branchCode)
        {
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelBranchLeaseDefaultValue";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ContractLimit = Convert.ToDecimal(dr["ContractLimit"]);
                            NUPApprovalLimit = Convert.ToDecimal(dr["NUPApprovalLimit"]);
                            NUPExpiredDays = Convert.ToInt32(dr["NUPExpiredDay"]);
                            OverRate = Convert.ToDecimal(dr["OverRate"]);
                            OverRateRetail = Convert.ToDecimal(dr["OverRateRetail"]);
                            PenaltyRate = Convert.ToDecimal(dr["PenaltyRate"]);
                            PenaltyRateRetail = Convert.ToDecimal(dr["PenaltyRateRetail"]);
                            LastCount = Convert.ToInt32(dr["LastCount"]);
                            LastReceiptNo = Convert.ToInt32(dr["LastReceiptNo"]);
                            SLIKCode = Tool.GeneralHelper.NullToString(dr["SLIKCode"]);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
        }
    }

    public class GL
    {
        // COA untuk perpindahan Aset Pembiayaan antar cabang
        [Display(Name = "HOP No")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "HOP No is required")]
        public IDS.GLTable.ChartOfAccount HopNo { get; set; }

        [Display(Name = "Rep No")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Rep No is required")]
        public IDS.GLTable.ChartOfAccount RepNo { get; set; }

        public GL()
        {
        }

        public void GetBranchAsetMovementAccount()
        {
            //using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            //{
            //    db.CommandText = "GTSelBranchAccount";
            //    db.CommandType = System.Data.CommandType.StoredProcedure;
            //    db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
            //    db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
            //    db.Open();

            //    db.ExecuteReader();

            //    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
            //    {
            //        if (dr.HasRows)
            //        {
            //            items = new List<IDS.GL.ChartOfAccount>();

            //            while (dr.Read())
            //            {
            //                IDS.GL.ChartOfAccount item = new IDS.GL.ChartOfAccount();

            //                items.Add(item);
            //            }
            //        }

            //        if (!dr.IsClosed)
            //            dr.Close();
            //    }
            //}

            //return items;
        }
    }

    ///// <summary>
    ///// Class untuk print setting pada branch
    ///// </summary>
    //public class BranchReportSetting
    //{
    //    public BranchReportSetting()
    //    {
    //    }

    //    public BranchReportSetting(bool printName, bool printAddress, bool printCity, bool printCountry, bool printPage, bool printDate, bool printTime, bool language) : this()
    //    {
    //        PrintBranchName = printName;
    //        PrintBranchAddress = printAddress;
    //        PrintBranchCity = printCity;
    //        PrintBranchCountry = printCountry;
    //        PrintPage = printPage;
    //        PrintDate = printDate;
    //        PrintTime = printTime;
    //        Language = language;
    //    }

    //    public void GetBranchReportSetting()
    //    {
    //        List<BranchReportSetting> items = null;

    //        using (DataAccess.SqlServer db = new DataAccess.SqlServer())
    //        {
    //            db.CommandText = "GTSelBranchReportSetting";
    //            db.CommandType = System.Data.CommandType.StoredProcedure;
    //            db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
    //            db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
    //            db.Open();

    //            db.ExecuteReader();

    //            using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
    //            {
    //                if (dr.HasRows)
    //                {
    //                    items = new List<BranchReportSetting>();

    //                    while (dr.Read())
    //                    {
    //                        PrintBranchName = Convert.ToBoolean(dr["Chknama"]);
    //                        PrintBranchAddress = Convert.ToBoolean(dr["Chkaddress"]);
    //                        PrintBranchCity = Convert.ToBoolean(dr["Chkcity"]);
    //                        PrintBranchCountry = Convert.ToBoolean(dr["Chkcountry"]);
    //                        PrintPage = Convert.ToBoolean(dr["ChkPage"]);
    //                        PrintDate = Convert.ToBoolean(dr["ChkPrintDate"]);
    //                        PrintTime = Convert.ToBoolean(dr["ChkPrintTime"]);
    //                        Language = Convert.ToBoolean(dr["OptIndex"]);
    //                    }
    //                }

    //                if (!dr.IsClosed)
    //                    dr.Close();
    //            }

    //            db.Close();
    //        }
    //    }
    //}

    //public class BrachContact
    //{

    //    public BrachContact()
    //    {
    //    }

    //    public void GetBranchReportSetting()
    //    {
    //        List<BranchReportSetting> items = null;

    //        using (DataAccess.SqlServer db = new DataAccess.SqlServer())
    //        {
    //            db.CommandText = "GTSelBranchReportSetting";
    //            db.CommandType = System.Data.CommandType.StoredProcedure;
    //            db.AddParameter("@code", System.Data.SqlDbType.VarChar, DBNull.Value);
    //            db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
    //            db.Open();

    //            db.ExecuteReader();

    //            using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
    //            {
    //                if (dr.HasRows)
    //                {
    //                    items = new List<BranchReportSetting>();

    //                    while (dr.Read())
    //                    {
    //                        Address1 = Tool.GeneralHelper.NullToString(dr["Addr1"]);
    //                        Address2 = Tool.GeneralHelper.NullToString(dr["Addr2"]);
    //                        Address3 = Tool.GeneralHelper.NullToString(dr["Addr3"]);

    //                        BranchCountry = new Country();
    //                        BranchCountry.CountryCode = Tool.GeneralHelper.NullToString(dr["CountryCode"]);
    //                        BranchCountry.CountryName = Tool.GeneralHelper.NullToString(dr["CountryName"]);

    //                        BranchCity = new City();
    //                        BranchCity.CityCode = Tool.GeneralHelper.NullToString(dr["CityCode"]);
    //                        BranchCity.CityName = Tool.GeneralHelper.NullToString(dr["CityName"]);
    //                        BranchCity.Country = BranchCountry;

    //                        PostalCode = Tool.GeneralHelper.NullToString(dr["PostalCode"]);

    //                        Phone1 = Tool.GeneralHelper.NullToString(dr["Phone1"]);
    //                        Phone2 = Tool.GeneralHelper.NullToString(dr["Phone2"]);
    //                        Phone3 = Tool.GeneralHelper.NullToString(dr["Phone3"]);
    //                        Fax = Tool.GeneralHelper.NullToString(dr["Fax"]);
    //                    }
    //                }

    //                if (!dr.IsClosed)
    //                    dr.Close();
    //            }

    //            db.Close();
    //        }
    //    }
    //}
}