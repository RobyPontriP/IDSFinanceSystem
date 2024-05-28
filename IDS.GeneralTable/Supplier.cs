using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GeneralTable
{
    public class Supplier
    {
        [Display(Name = "Sup Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Supplier code is required")]
        [MaxLength(20), StringLength(20)]
        public string SupCode { get; set; }

        [Display(Name = "Sup Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Supplier name is required")]
        [MaxLength(50), StringLength(50)]
        public string SupName { get; set; }

        ////public IDS.GeneralTable.Ind Industry { get; set; }

        //[Display(Name = "Contact Person")]
        //[MaxLength(50), StringLength(50)]
        //public string ContactPerson { get; set; }

        //[Display(Name = "Address")]
        //[MaxLength(100), StringLength(100)]
        //public string Address1 { get; set; }

        //[Display(Name = "Country")]
        //public IDS.GeneralTable.Country SupCountry { get; set; }

        //[Display(Name = "City")]
        ////[MaxLength(10), StringLength(10)]
        //public IDS.GeneralTable.City SupCity { get; set; }

        //[Display(Name = "Phone")]
        //[MaxLength(20), StringLength(20)]
        //public string Phone { get; set; }

        //[Display(Name = "FAX")]
        //[MaxLength(20), StringLength(20)]
        //public string Fax { get; set; }

        //[Display(Name = "NPWP")]
        //[MaxLength(30), StringLength(30)]
        //public string NPWP { get; set; }

        //[Display(Name = "NPPKP")]
        //[MaxLength(20), StringLength(20)]
        //public string NPPKP { get; set; }

        //[Display(Name = "Charts of Account Supplier")]
        //public IDS.GLTable.ChartOfAccount SupAcc { get; set; }

        //[Display(Name = "Beneficiary Name")]
        //[MaxLength(50), StringLength(50)]
        //public string BenName { get; set; }

        //[Display(Name = "Beneficiary Address 1")]
        //[MaxLength(50), StringLength(50)]
        //public string BenAddress1 { get; set; }

        //[Display(Name = "Beneficiary Address 2")]
        //[MaxLength(50), StringLength(50)]
        //public string BenAddress2 { get; set; }

        //[Display(Name = "Beneficiary Bank")]
        //[MaxLength(20), StringLength(20)]
        //public string BenBank { get; set; }

        //[Display(Name = "Acc No Beneficiary Bank")]
        ////[MaxLength(10), StringLength(10)]
        //public IDS.GLTable.ChartOfAccount BenBankAcc { get; set; }

        //[Display(Name = "Beneficiary Bank Address 1")]
        //[MaxLength(50), StringLength(50)]
        //public string BenBankAddress1 { get; set; }

        //[Display(Name = "Beneficiary Bank Address 2")]
        //[MaxLength(50), StringLength(50)]
        //public string BenBankAddress2 { get; set; }

        //[Display(Name = "Acc")]
        //public IDS.GLTable.ChartOfAccount Acc { get; set; }

        //[Display(Name = "Currency")]
        //public IDS.GeneralTable.Currency CCy { get; set; }

        //[Display(Name = "Type")]
        //public int Type { get; set; }

        //[Display(Name = "VAT Acc")]
        //public IDS.GLTable.ChartOfAccount VATAcc { get; set; }

        //[Display(Name = "Outstanding")]
        //public decimal Outstanding { get; set; }
        [Display(Name = "Contact Person")]
        [MaxLength(50), StringLength(50)]
        public string ContactPerson { get; set; }

        [Display(Name = "Address")]
        [MaxLength(100), StringLength(100)]
        public string Address1 { get; set; }

        [Display(Name = "Country")]
        //[MaxLength(3), StringLength(3)]
        public IDS.GeneralTable.Country SupCountry { get; set; }

        [Display(Name = "City")]
        //[MaxLength(10), StringLength(10)]
        public IDS.GeneralTable.City SupCity { get; set; }

        [Display(Name = "Phone")]
        [MaxLength(20), StringLength(20)]
        public string Phone { get; set; }

        [Display(Name = "FAX")]
        [MaxLength(20), StringLength(20)]
        public string Fax { get; set; }

        [Display(Name = "NPWP")]
        [MaxLength(30), StringLength(30)]
        public string NPWP { get; set; }

        [Display(Name = "NPPKP")]
        [MaxLength(20), StringLength(20)]
        public string NPPKP { get; set; }

        [Display(Name = "Charts of Account Supplier")]
        //[MaxLength(10), StringLength(10)]
        public IDS.GLTable.ChartOfAccount SupAcc { get; set; }

        [Display(Name = "Beneficiary Name")]
        [MaxLength(100), StringLength(100)]
        public string BenName { get; set; }

        [Display(Name = "Beneficiary Address 1")]
        [MaxLength(50), StringLength(50)]
        public string BenAddress1 { get; set; }

        [Display(Name = "Beneficiary Address 2")]
        [MaxLength(50), StringLength(50)]
        public string BenAddress2 { get; set; }

        [Display(Name = "Beneficiary Bank")]
        [MaxLength(20), StringLength(20)]
        public string BenBank { get; set; }

        [Display(Name = "Acc No Beneficiary Bank")]
        //[MaxLength(10), StringLength(10)]
        //public IDS.GLTable.ChartOfAccount BenBankAcc { get; set; }
        public string BenBankAcc { get; set; }

        [Display(Name = "Beneficiary Bank Address 1")]
        [MaxLength(50), StringLength(50)]
        public string BenBankAddress1 { get; set; }

        [Display(Name = "Beneficiary Bank Address 2")]
        [MaxLength(50), StringLength(50)]
        public string BenBankAddress2 { get; set; }

        [Display(Name = "Bill Name")]
        [MaxLength(100), StringLength(100)]
        public string BillName { get; set; }

        [Display(Name = "Bill Address")]
        [MaxLength(300), StringLength(300)]
        public string BillAdd { get; set; }

        [Display(Name = "Bill Country")]
        //[MaxLength(3), StringLength(3)]
        public IDS.GeneralTable.Country BillCountry { get; set; }

        [Display(Name = "Bill City")]
        //[MaxLength(10), StringLength(10)]
        public IDS.GeneralTable.City BillCity { get; set; }

        [Display(Name = "Tax Name")]
        [MaxLength(100), StringLength(100)]
        public string TaxName { get; set; }

        [Display(Name = "Tax Address")]
        [MaxLength(300), StringLength(300)]
        public string TaxAdd { get; set; }

        [Display(Name = "Tax Country")]
        //[MaxLength(3), StringLength(3)]
        public IDS.GeneralTable.Country TaxCountry { get; set; }

        [Display(Name = "Tax City")]
        //[MaxLength(10), StringLength(10)]
        public IDS.GeneralTable.City TaxCity { get; set; }

        [Display(Name = "Acc")]
        //[MaxLength(10), StringLength(10)]
        public IDS.GLTable.ChartOfAccount Acc { get; set; }

        [Display(Name = "Currency")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Currency is required")]
        public IDS.GeneralTable.Currency CCy { get; set; }

        [Display(Name = "Type")]
        public int Type { get; set; }

        [Display(Name = "Active / Passive")]
        public bool ActivePassive { get; set; }

        [Display(Name = "Flag Group")]
        public bool FlagGroup { get; set; }

        [Display(Name = "VAT Acc")]
        public IDS.GLTable.ChartOfAccount VATAcc { get; set; }

        [Display(Name = "Sales Acc")]
        public IDS.GLTable.ChartOfAccount SalesAcc { get; set; }

        [Display(Name = "Outstanding")]
        public decimal Outstanding { get; set; }

        [Display(Name = "Gov Private")]
        public bool GovPrivate { get; set; }

        [Display(Name = "Supplier Type")]
        [MaxLength(7), StringLength(7)]
        public string SupType { get; set; }

        [Display(Name = "BUMN Tax")]
        public IDS.GLTable.ChartOfAccount BUMNTax { get; set; }

        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [Display(Name = "Last Update")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime LastUpdate { get; set; }

        public Supplier()
        {
        }

        public Supplier(string supCode, string supName)
        {
            SupCode = supCode;
            SupName = supName;
        }

        public static List<Supplier> GetSupplier()
        {
            List<IDS.GeneralTable.Supplier> list = new List<Supplier>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelACFVEND";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Supplier sup = new Supplier();
                            sup.SupCode = dr["VEND"] as string;
                            sup.SupName = dr["NAME"] as string;
                            sup.ContactPerson = dr["ContactPerson"] as string;
                            sup.Address1 = dr["ADDR_1"] as string;

                            sup.SupCountry = new Country();
                            sup.SupCountry.CountryCode = Tool.GeneralHelper.NullToString(dr["CountryCode"]);
                            sup.SupCountry.CountryName = Tool.GeneralHelper.NullToString(dr["CountryName"]);

                            sup.SupCity = new City();
                            sup.SupCity.CityCode = Tool.GeneralHelper.NullToString(dr["CityCode"]);
                            sup.SupCity.CityName = dr["CityName"] as string;
                            //Sup.SupCity.BICode = dr["BICode"] as string;
                            //Sup.SupCity.OJKCode = dr["OJKCode"] as string;
                            //Sup.SupCity.SLIKCode = dr["SLIKCode"] as string;
                            //Sup.SupCity.Remark = dr["Remark"] as string;
                            sup.SupCity.Country = sup.SupCountry;

                            sup.Phone = dr["PHONE"] as string;
                            sup.Fax = dr["FAX"] as string;
                            sup.NPWP = dr["NPWP"] as string;
                            sup.NPPKP = dr["NPPKP"] as string;
                            
                            sup.SupAcc = new GLTable.ChartOfAccount();
                            sup.SupAcc.Account = Tool.GeneralHelper.NullToString(dr["SupAcc"]);

                            sup.Acc = new GLTable.ChartOfAccount();
                            sup.Acc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            sup.CCy = new GeneralTable.Currency();
                            sup.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            sup.VATAcc = new GLTable.ChartOfAccount();
                            sup.VATAcc.Account = Tool.GeneralHelper.NullToString(dr["VATACC"]);

                            sup.Type = Tool.GeneralHelper.NullToInt(dr["Type"], 0);
                            sup.EntryUser = dr["EntryUser"] as string;
                            sup.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            sup.OperatorID = dr["OperatorID"] as string;
                            sup.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(sup);
                        }

                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static Supplier GetSupplier(string supCode)
        {
            Supplier sup = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelACFVEND";
                db.AddParameter("@VENDCODE", System.Data.SqlDbType.VarChar, supCode);
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        sup = new Supplier();
                        sup.SupCode = dr["VEND"] as string;
                        sup.SupType = dr["SupType"] as string;
                        sup.SupName = dr["NAME"] as string;
                        sup.ContactPerson = dr["ContactPerson"] as string;
                        sup.Address1 = dr["ADDR_1"] as string;

                        sup.SupCountry = new Country();
                        sup.SupCountry.CountryCode = Tool.GeneralHelper.NullToString(dr["CountryCode"]);

                        sup.SupCity = new City();
                        sup.SupCity.CityCode = Tool.GeneralHelper.NullToString(dr["CityCode"]);
                        sup.SupCity.Country = sup.SupCountry;

                        sup.Phone = dr["PHONE"] as string;
                        sup.Fax = dr["FAX"] as string;
                        sup.NPWP = dr["NPWP"] as string;
                        sup.NPPKP = dr["NPPKP"] as string;

                        sup.SupAcc = new GLTable.ChartOfAccount();
                        sup.SupAcc.Account = Tool.GeneralHelper.NullToString(dr["SupAcc"]);

                        sup.SalesAcc = new GLTable.ChartOfAccount();
                        sup.SalesAcc.Account = Tool.GeneralHelper.NullToString(dr["SalesAcc"]);

                        sup.VATAcc = new GLTable.ChartOfAccount();
                        sup.VATAcc.Account = Tool.GeneralHelper.NullToString(dr["VATAcc"]);

                        sup.BUMNTax = new GLTable.ChartOfAccount();
                        sup.BUMNTax.Account = Tool.GeneralHelper.NullToString(dr["BUMNTax"]);

                        sup.Outstanding = Convert.ToDecimal(dr["outstanding"]);
                        sup.BenName = dr["BENNAME"] as string;
                        sup.BenAddress1 = dr["BENADD1"] as string;
                        sup.BenAddress2 = dr["BENADD2"] as string;
                        sup.BenBank = dr["BENBANK"] as string;

                        //sup.BenBankAcc = new GLTable.ChartOfAccount();
                        //sup.BenBankAcc.Account = Tool.GeneralHelper.NullToString(dr["BENBANKACC"]);
                        sup.BenBankAcc = Tool.GeneralHelper.NullToString(dr["BENBANKACC"]);

                        sup.BenBankAddress1 = dr["BENBANKADD1"] as string;
                        sup.BenBankAddress2 = dr["BENBANKADD2"] as string;

                        //Bill
                        sup.BillName = dr["BillName"] as string;
                        sup.BillCountry = new Country();
                        sup.BillCountry.CountryCode = Tool.GeneralHelper.NullToString(dr["BillCountry"]);
                        sup.BillAdd = dr["BILLADD"] as string;

                        sup.BillCity = new City();
                        sup.BillCity.CityCode = Tool.GeneralHelper.NullToString(dr["BillCity"]);
                        //

                        //Tax
                        sup.TaxName = dr["TaxName"] as string;
                        sup.TaxCountry = new Country();
                        sup.TaxCountry.CountryCode = Tool.GeneralHelper.NullToString(dr["TaxCountry"]);
                        sup.TaxAdd = dr["TAXADD"] as string;

                        sup.TaxCity = new City();
                        sup.TaxCity.CityCode = Tool.GeneralHelper.NullToString(dr["TaxCity"]);
                        //Tax

                        sup.Acc = new GLTable.ChartOfAccount();
                        sup.Acc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                        sup.CCy = new GeneralTable.Currency();
                        sup.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                        sup.Type = Tool.GeneralHelper.NullToInt(dr["Type"], 0);
                        sup.ActivePassive = Tool.GeneralHelper.NullToBool(dr["ActivePasive"]);
                        sup.FlagGroup = Tool.GeneralHelper.NullToBool(dr["FlagGroup"]);
                        sup.GovPrivate = Tool.GeneralHelper.NullToBool(dr["GovPrivate"]);
                        sup.EntryUser = dr["EntryUser"] as string;
                        sup.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        sup.OperatorID = dr["OperatorID"] as string;
                        sup.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return sup;
        }

        public int InsUpDelSupplier(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdateVEND";
                    cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@VEND", System.Data.SqlDbType.VarChar, SupCode);
                    //cmd.AddParameter("@SupType", System.Data.SqlDbType.Bit, Convert.ToBoolean(Convert.ToInt16(SupType)));
                    cmd.AddParameter("@NAME", System.Data.SqlDbType.VarChar, SupName);
                    cmd.AddParameter("@CONTACT", System.Data.SqlDbType.VarChar, ContactPerson);
                    cmd.AddParameter("@ADD1", System.Data.SqlDbType.VarChar, Address1);
                    cmd.AddParameter("@CITY", System.Data.SqlDbType.VarChar, SupCity.CityCode);
                    cmd.AddParameter("@COUNTRY", System.Data.SqlDbType.VarChar, SupCountry.CountryCode);
                    cmd.AddParameter("@MOBILE", System.Data.SqlDbType.VarChar, Phone);
                    cmd.AddParameter("@FAX", System.Data.SqlDbType.VarChar, Fax);
                    cmd.AddParameter("@NPWP", System.Data.SqlDbType.VarChar, NPWP);
                    cmd.AddParameter("@NPPKP", System.Data.SqlDbType.VarChar, NPPKP);
                    //cmd.AddParameter("@IndusCode", System.Data.SqlDbType.VarChar, IndustrySup.IndusCode);
                    cmd.AddParameter("@SupAcc", System.Data.SqlDbType.VarChar, SupAcc.Account);
                    //cmd.AddParameter("@SlsAcc", System.Data.SqlDbType.VarChar, SalesAcc.Account);
                    cmd.AddParameter("@VATAcc", System.Data.SqlDbType.VarChar, VATAcc.Account);
                    //cmd.AddParameter("@BUMNAcc", System.Data.SqlDbType.VarChar, BUMNTax.Account);
                    cmd.AddParameter("@Outstanding", System.Data.SqlDbType.Money, Outstanding);
                    cmd.AddParameter("@BenName", System.Data.SqlDbType.VarChar, BenName);
                    cmd.AddParameter("@BenAddr1", System.Data.SqlDbType.VarChar, BenAddress1);
                    cmd.AddParameter("@BenAddr2", System.Data.SqlDbType.VarChar, BenAddress2);
                    cmd.AddParameter("@BenBank", System.Data.SqlDbType.VarChar, BenBank);
                    cmd.AddParameter("@BenBankAcc", System.Data.SqlDbType.VarChar, BenBankAcc);
                    cmd.AddParameter("@BenBankAdd1", System.Data.SqlDbType.VarChar, BenBankAddress1);
                    cmd.AddParameter("@BenBankAdd2", System.Data.SqlDbType.VarChar, BenBankAddress2);
                    //cmd.AddParameter("@Acc", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(Acc.Account));
                    //cmd.AddParameter("@Ccy", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(CCy.CurrencyCode));
                    // Bill
                    cmd.AddParameter("@BillName", System.Data.SqlDbType.VarChar, BillName);
                    cmd.AddParameter("@BillCOUNTRY", System.Data.SqlDbType.VarChar, BillCountry.CountryCode);
                    cmd.AddParameter("@BillCITY", System.Data.SqlDbType.VarChar, BillCity.CityCode);
                    cmd.AddParameter("@BillAdd", System.Data.SqlDbType.VarChar, BillAdd);
                    //
                    // Tax
                    cmd.AddParameter("@TaxName", System.Data.SqlDbType.VarChar, TaxName);
                    cmd.AddParameter("@TaxCOUNTRY", System.Data.SqlDbType.VarChar, TaxCountry.CountryCode);
                    cmd.AddParameter("@TaxCITY", System.Data.SqlDbType.VarChar, TaxCity.CityCode);
                    cmd.AddParameter("@TaxAdd", System.Data.SqlDbType.VarChar, TaxAdd);
                    //
                    cmd.AddParameter("@Type", System.Data.SqlDbType.TinyInt, Type);
                    cmd.AddParameter("@ActivePassive", System.Data.SqlDbType.Bit, Convert.ToBoolean(Convert.ToInt16(ActivePassive)));
                    cmd.AddParameter("@FlagGroup", System.Data.SqlDbType.Bit, Convert.ToBoolean(Convert.ToInt16(FlagGroup)));
                    cmd.AddParameter("@GovPrivate", System.Data.SqlDbType.Bit, Convert.ToBoolean(Convert.ToInt16(GovPrivate)));
                    if (!string.IsNullOrEmpty(EntryUser))
                    {
                        cmd.AddParameter("@EntryUser", System.Data.SqlDbType.VarChar, EntryUser);
                    }
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
                            throw new Exception("Supplier code is already exists. Please choose other Supplier code.");
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

        public int InsUpDelSupplier(Tool.PageActivity ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdateVEND";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GTUpdateVEND";
                        cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, (int)ExecCode);
                        cmd.AddParameter("@VEND", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Supplier Code is already exists. Please choose other Supplier Code.");
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

        public static List<System.Web.Mvc.SelectListItem> GetACFVENDForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelACFVEND";
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 6);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem acfvend = new System.Web.Mvc.SelectListItem();
                            acfvend.Value = IDS.Tool.GeneralHelper.NullToString(dr["VEND"]);
                            acfvend.Text = acfvend.Value + " - " + IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(acfvend);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetACFVENDForDataSource(string vend)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelACFVEND";
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 5);
                db.AddParameter("@VENDCODE", System.Data.SqlDbType.VarChar, vend);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem acfvend = new System.Web.Mvc.SelectListItem();
                            acfvend.Value = IDS.Tool.GeneralHelper.NullToString(dr["SUPACC"]);
                            acfvend.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(acfvend);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetACFVENDForDataSource(bool withAll)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelACFVEND";
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 6);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem acfvend = new System.Web.Mvc.SelectListItem();
                            acfvend.Value = IDS.Tool.GeneralHelper.NullToString(dr["VEND"]);
                            acfvend.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(acfvend);
                        }
                    }
                }

                db.Close();
            }
            if (withAll)
            {
                list.Insert(0, new System.Web.Mvc.SelectListItem { Text = "All", Value = "" });
            }
            return list;
        }
    }
}
