﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace IDS.GLTable
{
    public class ChartOfAccount
    {
        [Display(Name = "Account No"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account no is required")]
        public string Account { get; set; }

        [Display(Name = "Currency"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Currency is required")]
        public IDS.GeneralTable.Currency CCy { get; set; }

        [Display(Name = "Chinese Name")]
        public string CHName { get; set; }

        [Display(Name = "Name"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account name is required")]
        public string AccountName { get; set; }

        [Display(Name = "Level"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account level is required")]
        [Range(1,9, ErrorMessage = "Rating must between 1 to 9")]
        public int Level { get; set; }

        [Display(Name = "Total / Detail"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account total / detail is required")]
        public bool AccountTotalDetail { get; set; }

        [Display(Name = "Group"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Account group is required")]
        public IDS.Tool.GLAccountGroup AccountGroup { get; set; }

        [Display(Name = "Proctected Account")]
        public bool ProtectAccount { get; set; }

        [Display(Name = "Cash Acc")]
        public bool CashAccount { get; set; }

        [Display(Name = "Created By")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT),
            Display(Name = "Created Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [Display(Name = "Last Update"),
           DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime LastUpdate { get; set; }

        public ChartOfAccount()
        {
        }

        /// <summary>
        /// Retrieve semua daftar Country
        /// </summary>
        /// <returns></returns>
        public static List<ChartOfAccount> GetCOA()
        {
            List<ChartOfAccount> list = new List<ChartOfAccount>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        list = new List<ChartOfAccount>();

                        while (dr.Read())
                        {
                            ChartOfAccount coa = new ChartOfAccount();
                            coa.Account = dr["ACC"] as string;
                            coa.AccountName = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                            coa.CHName = IDS.Tool.GeneralHelper.NullToString(dr["CHName"]);
                            coa.CCy = IDS.GeneralTable.Currency.GetCurrency(IDS.Tool.GeneralHelper.NullToString(dr["CCY"]));
                            coa.Level = dr["TL"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TL"]);
                            coa.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt16(dr["AG"]);
                            coa.AccountTotalDetail = Convert.ToBoolean(dr["AT"]);
                            coa.ProtectAccount = Convert.ToBoolean(dr["PROTECTACC"]);
                            coa.CashAccount = Convert.ToBoolean(dr["CASHACC"]);

                            coa.EntryUser = dr["EntryUser"] as string;
                            coa.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            coa.OperatorID = dr["OperatorID"] as string;
                            coa.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(coa);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<ChartOfAccount> GetCOAForGrid(string parent)
        {
            List<ChartOfAccount> list = new List<ChartOfAccount>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                if (string.IsNullOrWhiteSpace(parent))
                {
                    db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, parent);
                }
               
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 6);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        list = new List<ChartOfAccount>();

                        while (dr.Read())
                        {
                            ChartOfAccount coa = new ChartOfAccount();
                            coa.Account = dr["ACC"] as string;
                            coa.AccountName = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                            //coa.CHName = IDS.Tool.GeneralHelper.NullToString(dr["CHName"]);
                            coa.CCy = new GeneralTable.Currency();
                            coa.CCy.CurrencyCode = IDS.Tool.GeneralHelper.NullToString(dr["CCY"]);

                            coa.AccountTotalDetail = Convert.ToBoolean(dr["AT"]);
                            coa.Level = dr["TL"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TL"]);
                            coa.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt16(dr["AG"]);
                            //coa.ProtectAccount = Convert.ToBoolean(dr["PROTECTACC"]);
                            //coa.CashAccount = Convert.ToBoolean(dr["CASHACC"]);

                            //coa.EntryUser = dr["EntryUser"] as string;
                            //coa.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                            coa.OperatorID = dr["OperatorID"] as string;
                            coa.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(coa);
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
        /// Retrieve Daftar Chart of Account berdasarkan kode Account
        /// </summary>
        /// <param name="countryCode">Account No</param>
        /// <returns></returns>
        public static ChartOfAccount GetCOA(string accountNo)
        {
            ChartOfAccount coa = new ChartOfAccount();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, accountNo);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        coa.Account = dr["ACC"] as string;
                        coa.AccountName = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                        coa.CHName = IDS.Tool.GeneralHelper.NullToString(dr["CHName"]);
                        coa.CCy = IDS.GeneralTable.Currency.GetCurrency(IDS.Tool.GeneralHelper.NullToString(dr["CCY"]));
                        coa.Level = dr["TL"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TL"]);
                        coa.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt16(dr["AG"]);
                        coa.ProtectAccount = Convert.ToBoolean(dr["PROTECTACC"]);
                        coa.CashAccount = Convert.ToBoolean(dr["CASHACC"]);
                        coa.AccountTotalDetail = Convert.ToBoolean(dr["AT"]);

                        coa.EntryUser = dr["EntryUser"] as string;
                        coa.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        coa.OperatorID = dr["OperatorID"] as string;
                        coa.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return coa;
        }

        /// <summary>
        /// Retrieve Daftar Chart of Account berdasarkan kode Account and currency
        /// </summary>
        /// <param name="accountNo"></param>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        public static ChartOfAccount GetCOA(string accountNo, string ccyCode)
        {
            ChartOfAccount coa = new ChartOfAccount();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, accountNo);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, ccyCode);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        coa.Account = dr["ACC"] as string;
                        coa.AccountName = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                        coa.CHName = IDS.Tool.GeneralHelper.NullToString(dr["CHName"]);
                        coa.CCy = IDS.GeneralTable.Currency.GetCurrency(IDS.Tool.GeneralHelper.NullToString(dr["CCY"]));
                        coa.Level = dr["TL"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TL"]);
                        coa.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt16(dr["AG"]);
                        coa.ProtectAccount = Convert.ToBoolean(dr["PROTECTACC"]);
                        coa.CashAccount = Convert.ToBoolean(dr["CASHACC"]);
                        coa.AccountTotalDetail = Convert.ToBoolean(dr["AT"]);

                        coa.EntryUser = dr["EntryUser"] as string;
                        coa.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        coa.OperatorID = dr["OperatorID"] as string;
                        coa.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return coa;
        }

        /// <summary>
        /// Retrieve Daftar Chart of Account berdasarkan parameter
        /// </summary>
        /// <param name="accountNo"></param>
        /// <param name="currencyCode"></param>
        /// <param name="accountGroup"></param>
        /// <param name="accountTotal"></param>
        /// <returns></returns>
        public static List<ChartOfAccount> GetCOA(string accountNo, string currencyCode, int? accountGroup, bool? accountTotal)
        {
            List<ChartOfAccount> list = new List<ChartOfAccount>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, accountNo);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, currencyCode == null ? DBNull.Value : (object)currencyCode);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, accountGroup == null ? DBNull.Value : (object)accountGroup);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, accountTotal == null ? DBNull.Value : (object)accountTotal);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        ChartOfAccount coa = new ChartOfAccount();
                        coa.Account = dr["ACC"] as string;
                        coa.AccountName = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                        coa.CHName = IDS.Tool.GeneralHelper.NullToString(dr["CHName"]);
                        coa.CCy = IDS.GeneralTable.Currency.GetCurrency(IDS.Tool.GeneralHelper.NullToString(dr["CCY"]));
                        coa.Level = dr["TL"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TL"]);
                        coa.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt16(dr["AG"]);
                        coa.ProtectAccount = Convert.ToBoolean(dr["PROTECTACC"]);
                        coa.CashAccount = Convert.ToBoolean(dr["CASHACC"]);
                        coa.AccountTotalDetail = Convert.ToBoolean(dr["AT"]);

                        coa.EntryUser = dr["EntryUser"] as string;
                        coa.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                        coa.OperatorID = dr["OperatorID"] as string;
                        coa.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                        list.Add(coa);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            if (list.Count > 0)
                list.OrderBy(x => x.Account);

            return list;
        }

        /// <summary>
        /// Retrieve Chart of Account untuk datasource
        /// </summary>
        /// <returns></returns>
        public static List<System.Web.Mvc.SelectListItem> GetCOAForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                // Di ganti dari 8 ke 7
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 7);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                            coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                            coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(coa);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        /// <summary>
        /// Retrieve Chart of Account untuk datasource dengan AT=0 tanpa Account Group
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public static List<System.Web.Mvc.SelectListItem> GetCOAForDatasource(string currencyCode)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, currencyCode);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
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
                            System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                            coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                            coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(coa);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetCOAForDataSource(string ccy)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 7);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                            coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                            coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(coa);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetCOAForSalesDataSource(string ccy,string typeAcc)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 12);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (string.IsNullOrEmpty(typeAcc))
                            {
                                System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                                coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                                coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                                list.Add(coa);
                            }
                            else
                            {
                                if (IDS.Tool.GeneralHelper.NullToString(dr["TaxACC"]) != null || IDS.Tool.GeneralHelper.NullToString(dr["TaxACC"]) != "")
                                {
                                    if (IDS.Tool.GeneralHelper.NullToString(dr["TaxACC"]) == typeAcc)
                                    {
                                        System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                                        coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                                        coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                                        list.Add(coa);
                                    }
                                }
                            }
                            
                            
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        /// <summary>
        /// Retrieve Chart of Account untuk datasource with Account Group
        /// </summary>
        /// <returns></returns>
        public static System.Web.Mvc.SelectList GetCOAForDatasourceWithAccountGroup(string currencyCode)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, currencyCode);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 5);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                            coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                            coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);
                            coa.Group = new System.Web.Mvc.SelectListGroup() { Name = IDS.Tool.GeneralHelper.NullToString(dr["AGName"]), Disabled = true };
                            list.Add(coa);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            System.Web.Mvc.SelectList selList = null;

            if (list.Count > 0)
            {
                selList = new System.Web.Mvc.SelectList(list, "Value", "Text", "Group.Name", null, null);
            }
            else
            {
                selList = new System.Web.Mvc.SelectList(new List<System.Web.Mvc.SelectListItem>());
            }

            return selList;
        }
                      
        /// <summary>
        /// Retrieve Chart of Account untuk datasource dengan tanpa Account Group
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public static List<System.Web.Mvc.SelectListItem> GetCOAExProtAccForDatasource(string currencyCode)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, currencyCode);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 11);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                            coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                            coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(coa);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        /// <summary>
        /// Retrieve Chart of Account untuk datasource exclude Prot Acc
        /// </summary>
        /// <returns></returns>
        public static List<System.Web.Mvc.SelectListItem> GetCOAExcludeProtAccForDatasource()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 9);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                            coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                            coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(coa);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        /// <summary>
        /// Retrieve Chart of Account untuk datasource exclude Prot Acc
        /// </summary>
        /// <returns></returns>
        public static List<System.Web.Mvc.SelectListItem> GetCOAWithTypeExcludeProtAccForDataSource(string ccy, string typeAcc)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, DBNull.Value);
                if (string.IsNullOrEmpty(ccy))
                {
                    db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, ccy);
                }
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);

                if (string.IsNullOrEmpty(typeAcc))
                {
                    db.AddParameter("@TypeACC", System.Data.SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    db.AddParameter("@TypeACC", System.Data.SqlDbType.VarChar, typeAcc);
                }
                
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 8);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem coa = new System.Web.Mvc.SelectListItem();
                            coa.Value = IDS.Tool.GeneralHelper.NullToString(dr["ACC"]);
                            coa.Text = IDS.Tool.GeneralHelper.NullToString(dr["NAME"]);

                            list.Add(coa);
                        }
                    }
                }

                db.Close();
            }

            return list;
        }

        public static bool IsCashAccount(string currency, string account)
        {
            if (string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(account))
            {
                return true;
            }

            bool result = true;

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GLSelACFGLMH";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, account);
                db.AddParameter("@CCy", System.Data.SqlDbType.VarChar, currency);
                db.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 10);
                db.Open();

                result = Convert.ToBoolean(db.ExecuteScalar());

                db.Close();
            }

            return result;
        }

        public virtual int InsUpDel(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLTUpdateCOA";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Account);
                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                    cmd.AddParameter("@NAME", System.Data.SqlDbType.VarChar, AccountName);
                    cmd.AddParameter("@CHName", System.Data.SqlDbType.NVarChar, CHName);
                    cmd.AddParameter("@TL", System.Data.SqlDbType.TinyInt, Level);
                    cmd.AddParameter("@AT", System.Data.SqlDbType.TinyInt, AccountTotalDetail);
                    cmd.AddParameter("@AG", System.Data.SqlDbType.TinyInt, (int)AccountGroup);
                    cmd.AddParameter("@OPERATORID", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.AddParameter("@CASHACC", System.Data.SqlDbType.Bit, CashAccount);
                    cmd.AddParameter("@protect", System.Data.SqlDbType.Bit, ProtectAccount);
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
                            throw new Exception("Combination of Account Number and Currency data are already exists. Please choose other Account No and Currency.");
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

        public virtual int InsUpDel(int ExecCode, string[] datas)
        {
            int result = 0;

            if (datas == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GLTUpdateCOA";
                    cmd.Open();
                    cmd.BeginTransaction();

                    string[] data;
                    string coa = "";
                    string ccy = "";

                    for (int i = 0; i < datas.Length; i++)
                    {
                        data = datas[i].Split(new char[] { ';' });

                        coa = data[0];
                        ccy = data[1];

                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, coa);
                        cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                        cmd.AddParameter("@NAME", System.Data.SqlDbType.VarChar, DBNull.Value);
                        cmd.AddParameter("@CHName", System.Data.SqlDbType.NVarChar, DBNull.Value);
                        cmd.AddParameter("@TL", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        cmd.AddParameter("@AT", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        cmd.AddParameter("@AG", System.Data.SqlDbType.TinyInt, DBNull.Value);
                        cmd.AddParameter("@OPERATORID", System.Data.SqlDbType.VarChar, OperatorID);
                        cmd.AddParameter("@CASHACC", System.Data.SqlDbType.Bit, DBNull.Value);
                        cmd.AddParameter("@protect", System.Data.SqlDbType.Bit, DBNull.Value);
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
                            throw new Exception("Combination of Account Number and Currency data are already exists. Please choose other Account No and Currency.");
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
    }

    public class ChartOfAccountTreeNode
    {
        public string Account { get; set; }
        public IList<string> Child { get; set; }

        public ChartOfAccountTreeNode(string account)
        {
            Account = account;
            Child = new List<string>();
        }

        public ChartOfAccountTreeNode(string account, string childAccount) : this(account)
        {
            Child.Add(childAccount);
        }

        public void AddChild(string ChildAccount)
        {
            if (Child != null && !Child.Contains(ChildAccount))
            {
                Child.Add(ChildAccount);
                List<string> child = Child.OrderBy(x => x).ToList();
            }
        }

        public void SortChild()
        {
            if (Child != null && Child.Count > 1)
            {
                List<string> child = Child.OrderBy(x => x).ToList();
            }
        }

        public static IList<ChartOfAccountTreeNode> GetChartAccountAllNode()
        {
            IList<ChartOfAccountTreeNode> node = new List<ChartOfAccountTreeNode>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SELECT * FROM dbo.vwAccTreeNode";
                db.CommandType = System.Data.CommandType.Text;
                db.Open();

                db.ExecuteReader();

                using (SqlDataReader rd = db.DbDataReader as SqlDataReader)
                {
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            switch (node.Where(x => x.Account == rd["Account"].ToString()).Count() > 0)
                            {
                                case false:
                                    node.Add(new ChartOfAccountTreeNode(rd["Account"].ToString(), rd["ChildAccount"].ToString()));
                                    break;
                                default:
                                    //node.Add(new ChartOfAccountTreeNode(rd["Account"].ToString(), rd["ChildAccount"].ToString()));
                                    ChartOfAccountTreeNode obj = node.Where(x => x.Account == rd["Account"].ToString()).FirstOrDefault();
                                    if (obj != null)
                                    {
                                        if (!obj.Child.Contains(rd["ChildAccount"]))
                                        {
                                            obj.AddChild(rd["ChildAccount"].ToString());
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    if (!rd.IsClosed)
                        rd.Close();
                }

                db.Close();
            }

            if (node.Count > 1)
            {
                node = node.OrderBy(x => x.Account).ToList();

                foreach (ChartOfAccountTreeNode parent in node)
                {
                    if (parent.Child != null && parent.Child.Count > 1)
                    {
                        parent.Child = parent.Child.OrderBy(x => x).ToList();
                    }
                }
            }

            return node;
        }

        public static string GetChartAccountAllNodeRawHtml()
        {
            StringBuilder sb = new StringBuilder();
            IList<ChartOfAccountTreeNode> items = GetChartAccountAllNode();

            if (items != null && items.Count > 0)
            {
                //sb.Append("<div class=\"tree\"");
                sb.Append("\n\t<ul>");
                for (int i = 0; i < items.Count; i++)
                {
                    sb.Append("\n\t<li class=\"folder\"><a href=\"#\" onClick=\"setH('" + items[i].Account + "')\">" + items[i].Account + "</a>");

                    if (items[i].Child != null && items[i].Child.Count > 0)
                    {
                        sb.Append("\n\t<ul>");

                        for (int j = 0; j < items[i].Child.Count; j++)
                        {
                            sb.Append("\n\t<li class=\"file\"><a href=\"#\" onClick=\"setH('" + items[i].Child[j] + "')\">" + items[i].Child[j] + "</a><li>");
                        }

                        sb.Append("\n</ul>");
                    }
                }

                sb.Append("\n</ul>\n");
                //sb.Append("</div>");
            }

            return sb.ToString();            
        }
    }

    /// <summary>
    /// Untuk Chart of Account View Tree
    /// </summary>
    public class COAView
    {
        [JsonProperty(Order = 1)]
        public string Account { get; set; }
        [JsonProperty(Order = 2)]
        public IDS.GeneralTable.Currency CCy { get; set; }
        //public string CHName { get; set; }
        [JsonProperty(Order = 3)]
        public string ParentDummy { get; set; }
        [JsonProperty(Order = 4)]
        public string AccountName { get; set; }
        [JsonProperty(Order = 5)]
        public int Level { get; set; }
        [JsonProperty(Order = 6)]
        public int AccountTotal { get; set; }
        [JsonProperty(Order = 7)]
        public bool ProtectedAcc { get; set; }
        [JsonProperty(Order = 8)]
        public string COAID { get; set; }
        [JsonProperty(Order = 9)]
        public string ParentID { get; set; }
        [JsonProperty(Order = 10)]
        public IDS.Tool.GLAccountGroup AccountGroup { get; set; }


        //public bool ProtectAccount { get; set; }
        //public bool CashAccount { get; set; }


        //[JsonProperty(Order = 11)]
        //public string EntryUser { get; set; }
        //[JsonProperty(Order = 12)]
        //public DateTime EntryDate { get; set; }
        [JsonProperty(Order = 13)]
        public string OperatorID { get; set; }

        [JsonProperty(Order = 14)]
        public DateTime LastUpdate { get; set; }

        [JsonProperty(Order = 15, NullValueHandling = NullValueHandling.Ignore)]
        public List<COAView> children = new List<COAView>();


        public COAView()
        {
        }

        public List<COAView> GetCOAViewForGrid()
        {
            List<COAView> items = new List<COAView>();
            List<COAView> root = new List<COAView>();

            #region DataTable
            System.Data.DataSet dsNew = new System.Data.DataSet("dummer");


            int i = 0, g = 0;

            string strContent1 = "";
            string strContent2 = "";
            string strContent3 = "";
            string strContent4 = "";
            string strContent5 = "";
            string strContent6 = "";
            string strContent7 = "";
            string strContent8 = "";
            string strContent9 = "";
            string strCurrentCursor;

            int Level = 0;
            int LevelBefore = 0;
            bool TotDet = false;
            bool TotDetBefore = false;

            System.Web.UI.WebControls.SqlDataSource sdSrc = new System.Web.UI.WebControls.SqlDataSource();
            sdSrc.ID = "dtSrc";
            #endregion

            try
            {
                using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                {
                    db.CommandText = "SELECT " +
                        " ACC, CCY, NAME, ACC + CCY AS ParentDummy, TL, [AT], AG, PROTECTACC, OperatorID, LastUpdate, " +
                        " ACC+(CONVERT(VARCHAR(1), TL)) AS CoAID, '' AS ParentID " +
                        " FROM ACFGLMH " +
                        " ORDER BY ACC, CCY, TL, [AT]";
                    db.CommandType = System.Data.CommandType.Text;
                    db.Open();

                    #region Datatable
                    dsNew = db.GetDataSet();
                    dsNew.CaseSensitive = false;
                    dsNew.AcceptChanges();
                    #endregion

                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            COAView item = null;

                            while (dr.Read())
                            {
                                #region DataTable
                                TotDet = Convert.ToBoolean(dr["at"]);
                                Level = Convert.ToInt16(dr["tl"]);

                                if (i != 0)
                                {
                                    TotDetBefore = Convert.ToBoolean(dsNew.Tables[0].Rows[i - 1]["at"]);
                                    LevelBefore = Convert.ToInt16(dsNew.Tables[0].Rows[i - 1]["tl"]);
                                }

                                switch (Level)
                                {
                                    case 9:
                                        strContent8 = "";
                                        strContent7 = "";
                                        strContent6 = "";
                                        strContent5 = "";
                                        strContent4 = "";
                                        strContent3 = "";
                                        strContent2 = "";
                                        strContent1 = "";
                                        strContent9 = Convert.ToString(dr["parentdummy"]);
                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent9;

                                        item = new COAView();
                                        item.Account = dr["ACC"] as string;
                                        item.CCy = new IDS.GeneralTable.Currency();
                                        item.CCy.CurrencyCode = dr["CCY"] as string;
                                        item.ParentDummy = dr["ParentDummy"] as string;
                                        item.AccountName = dr["NAME"] as string;
                                        item.Level = Convert.ToInt32(dr["TL"]);
                                        item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                        item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                        item.COAID = dr["CoAID"] as string;
                                        item.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt32(dr["AG"]);
                                        item.ParentID = strContent9;

                                        items.Add(item);
                                        break;
                                    case 8:
                                        strContent8 = Convert.ToString(dr["parentdummy"]);

                                        item = new COAView();
                                        item.Account = dr["ACC"] as string;
                                        item.CCy = new IDS.GeneralTable.Currency();
                                        item.CCy.CurrencyCode = dr["CCY"] as string;
                                        item.ParentDummy = dr["ParentDummy"] as string;
                                        item.AccountName = dr["NAME"] as string;
                                        item.Level = Convert.ToInt32(dr["TL"]);
                                        item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                        item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                        item.COAID = dr["CoAID"] as string;
                                        item.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt32(dr["AG"]);
                                        item.ParentID = strContent8;

                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent8;

                                        if (i != 0)
                                        {
                                            if (LevelBefore > Level)
                                            {
                                                if (TotDetBefore == true)
                                                {
                                                    item.ParentID = items[i - 1].ParentDummy;

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentdummy"];
                                                }
                                            }
                                            else
                                            {
                                                if (LevelBefore == Level)
                                                {
                                                    item.ParentID = items[i - 1].ParentID;

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentid"];
                                                }
                                                else
                                                {
                                                    item.ParentID = strContent9;

                                                    //XXX
                                                    if (Convert.ToString(strContent9).Trim() != "")
                                                    {
                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent9;
                                                    }
                                                }
                                            }
                                        }

                                        items.Add(item);
                                        break;
                                    case 7:
                                        strContent7 = Convert.ToString(dr["parentdummy"]);

                                        item = new COAView();
                                        item.Account = dr["ACC"] as string;
                                        item.CCy = new IDS.GeneralTable.Currency();
                                        item.CCy.CurrencyCode = dr["CCY"] as string;
                                        item.ParentDummy = dr["ParentDummy"] as string;
                                        item.AccountName = dr["NAME"] as string;
                                        item.Level = Convert.ToInt32(dr["TL"]);
                                        item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                        item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                        item.COAID = dr["CoAID"] as string;
                                        item.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt32(dr["AG"]);
                                        item.ParentID = strContent7;

                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent7;
                                        if (i != 0)
                                        {
                                            if (LevelBefore > Level)
                                            {
                                                if (TotDetBefore == true)
                                                {
                                                    item.ParentID = items[i - 1].ParentDummy;

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentdummy"];
                                                }
                                            }
                                            else
                                            {
                                                if (LevelBefore == Level)
                                                {
                                                    item.ParentID = items[i - 1].ParentID;

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentid"];
                                                }
                                                else
                                                {
                                                    //XXX
                                                    if (Convert.ToString(strContent8).Trim() != "")
                                                    {
                                                        item.ParentID = strContent8;

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent8;

                                                    }
                                                    else
                                                    {
                                                        item.ParentID = strContent9;

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent9;
                                                    }

                                                }
                                            }
                                        }

                                        items.Add(item);
                                        break;
                                    case 6:
                                        strContent6 = Convert.ToString(dr["parentdummy"]);

                                        item = new COAView();
                                        item.Account = dr["ACC"] as string;
                                        item.CCy = new IDS.GeneralTable.Currency();
                                        item.CCy.CurrencyCode = dr["CCY"] as string;
                                        item.ParentDummy = dr["ParentDummy"] as string;
                                        item.AccountName = dr["NAME"] as string;
                                        item.Level = Convert.ToInt32(dr["TL"]);
                                        item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                        item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                        item.COAID = dr["CoAID"] as string;
                                        item.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt32(dr["AG"]);
                                        item.ParentID = strContent6;

                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent6;

                                        if (i != 0)
                                        {
                                            if (LevelBefore > Level)
                                            {
                                                if (TotDetBefore == true)
                                                {
                                                    item.ParentID = items[i - 1].ParentDummy;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentdummy"];
                                                }
                                            }
                                            else
                                            {
                                                if (LevelBefore == Level)
                                                {
                                                    item.ParentID = items[i - 1].ParentDummy;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentid"];
                                                }
                                                else
                                                {
                                                    //XXX
                                                    if (Convert.ToString(strContent7).Trim() != "")
                                                    {
                                                        if (strContent7.Substring(0, 3) != Convert.ToString(dsNew.Tables[0].Rows[i]["parentdummy"]).Substring(0, 3))
                                                        {
                                                            item.ParentID = strContent9;
                                                            

                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent9;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        item.ParentID = strContent8;
                                                        

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent8;
                                                    }

                                                }
                                            }
                                        }

                                        items.Add(item);
                                        break;
                                    case 5:
                                        strContent5 = Convert.ToString(dr["parentdummy"]);

                                        item = new COAView();
                                        item.Account = dr["ACC"] as string;
                                        item.CCy = new IDS.GeneralTable.Currency();
                                        item.CCy.CurrencyCode = dr["CCY"] as string;
                                        item.ParentDummy = dr["ParentDummy"] as string;
                                        item.AccountName = dr["NAME"] as string;
                                        item.Level = Convert.ToInt32(dr["TL"]);
                                        item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                        item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                        item.COAID = dr["CoAID"] as string;
                                        item.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt32(dr["AG"]);
                                        item.ParentID = strContent5;

                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent5;

                                        if (i != 0)
                                        {
                                            if (LevelBefore > Level)
                                            {
                                                if (TotDetBefore == true)
                                                {
                                                    item.ParentID = items[i - 1].ParentDummy;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentdummy"];
                                                }
                                            }
                                            else
                                            {
                                                if (LevelBefore == Level)
                                                {
                                                    item.ParentID = items[i - 1].ParentID;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentid"];
                                                }
                                                else
                                                {
                                                    //XXX
                                                    if (Convert.ToString(strContent6).Trim() != "")
                                                    {
                                                        item.ParentID = strContent6;
                                                        

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent6;

                                                        if (strContent6.Substring(0, 3) != Convert.ToString(dsNew.Tables[0].Rows[i]["parentdummy"]).Substring(0, 3))
                                                        {
                                                            item.ParentID = strContent8;
                                                            

                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent8;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        item.ParentID = strContent7;
                                                        

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent7;


                                                        if (strContent7.Trim() == "")
                                                        {
                                                            if (strContent8.Trim() == "")
                                                            {
                                                                item.ParentID = strContent9;
                                                                

                                                                dsNew.Tables[0].Rows[i]["parentid"] = strContent9;
                                                            }
                                                            else
                                                            {
                                                                item.ParentID = strContent8;
                                                                

                                                                dsNew.Tables[0].Rows[i]["parentid"] = strContent8;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        items.Add(item);
                                        break;
                                    case 4:
                                        strContent4 = Convert.ToString(dr["parentdummy"]);

                                        item = new COAView();
                                        item.Account = dr["ACC"] as string;
                                        item.CCy = new IDS.GeneralTable.Currency();
                                        item.CCy.CurrencyCode = dr["CCY"] as string;
                                        item.ParentDummy = dr["ParentDummy"] as string;
                                        item.AccountName = dr["NAME"] as string;
                                        item.Level = Convert.ToInt32(dr["TL"]);
                                        item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                        item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                        item.COAID = dr["CoAID"] as string;
                                        item.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt32(dr["AG"]);
                                        item.ParentID = strContent4;

                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent4;
                                        if (i != 0)
                                        {
                                            if (LevelBefore > Level)
                                            {
                                                if (TotDetBefore == true)
                                                {
                                                    item.ParentID = items[i - 1].ParentDummy;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentdummy"];
                                                }
                                            }
                                            else
                                            {
                                                if (LevelBefore == Level)
                                                {
                                                    item.ParentID = items[i - 1].ParentID;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentid"];
                                                }
                                                else
                                                {
                                                    //XXX
                                                    if (Convert.ToString(strContent5).Trim() != "")
                                                    {
                                                        item.ParentID = strContent5;
                                                        

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent5;
                                                        if (strContent5.Substring(0, 3) != Convert.ToString(dsNew.Tables[0].Rows[i]["parentdummy"]).Substring(0, 3))
                                                        {
                                                            item.ParentID = strContent7;
                                                            

                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent7;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        item.ParentID = strContent6;
                                                        

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent6;

                                                        if (strContent6.Trim() == "")
                                                        {
                                                            if (strContent7.Trim() == "")
                                                            {
                                                                if (strContent8.Trim() == "")
                                                                {
                                                                    item.ParentID = strContent9;
                                                                    

                                                                    dsNew.Tables[0].Rows[i]["parentid"] = strContent9;
                                                                }
                                                                else
                                                                {
                                                                    item.ParentID = strContent8;
                                                                    

                                                                    dsNew.Tables[0].Rows[i]["parentid"] = strContent8;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (strContent6.Trim() == "")
                                                                {
                                                                    if (strContent7.Trim() == "")
                                                                    {
                                                                        if (strContent8.Trim() == "")
                                                                        {
                                                                            item.ParentID = strContent9;
                                                                            

                                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent9;
                                                                        }
                                                                        else
                                                                        {
                                                                            item.ParentID = strContent8;
                                                                            

                                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent8;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        item.ParentID = strContent7;
                                                                        

                                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent7;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    item.ParentID = strContent6;
                                                                    

                                                                    dsNew.Tables[0].Rows[i]["parentid"] = strContent6;
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        items.Add(item);
                                        break;
                                    case 3:
                                        strContent3 = Convert.ToString(dr["parentdummy"]);

                                        item = new COAView();
                                        item.Account = dr["ACC"] as string;
                                        item.CCy = new IDS.GeneralTable.Currency();
                                        item.CCy.CurrencyCode = dr["CCY"] as string;
                                        item.ParentDummy = dr["ParentDummy"] as string;
                                        item.AccountName = dr["NAME"] as string;
                                        item.Level = Convert.ToInt32(dr["TL"]);
                                        item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                        item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                        item.COAID = dr["CoAID"] as string;
                                        item.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt32(dr["AG"]);
                                        item.ParentID = strContent3;

                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent3;
                                        if (i != 0)
                                        {
                                            if (LevelBefore > Level)
                                            {
                                                if (TotDetBefore == true)
                                                {
                                                    item.ParentID = items[i - 1].ParentDummy;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentdummy"];
                                                }
                                            }
                                            else
                                            {
                                                if (LevelBefore == Level)
                                                {
                                                    item.ParentID = items[i - 1].ParentID;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentid"];
                                                }
                                                else
                                                {
                                                    //XXX
                                                    if (Convert.ToString(strContent4).Trim() != "")
                                                    {
                                                        item.ParentID = strContent4;
                                                        

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent4;
                                                        if (strContent4.Substring(0, 3) != Convert.ToString(dsNew.Tables[0].Rows[i]["parentdummy"]).Substring(0, 3))
                                                        {
                                                            item.ParentID = strContent6;
                                                            

                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent6;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (strContent5.Trim() == "")
                                                        {
                                                            if (strContent6.Trim() == "")
                                                            {
                                                                if (strContent7.Trim() == "")
                                                                {
                                                                    if (strContent8.Trim() == "")
                                                                    {
                                                                        item.ParentID = strContent9;
                                                                        

                                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent9;
                                                                    }
                                                                    else
                                                                    {
                                                                        item.ParentID = strContent8;
                                                                        

                                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent8;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    item.ParentID = strContent7;
                                                                    

                                                                    dsNew.Tables[0].Rows[i]["parentid"] = strContent7;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                item.ParentID = strContent6;
                                                                

                                                                dsNew.Tables[0].Rows[i]["parentid"] = strContent6;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            item.ParentID = strContent5;
                                                            

                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent5;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        items.Add(item);
                                        break;
                                    case 2:
                                        strContent2 = Convert.ToString(dr["parentdummy"]);

                                        item = new COAView();
                                        item.Account = dr["ACC"] as string;
                                        item.CCy = new IDS.GeneralTable.Currency();
                                        item.CCy.CurrencyCode = dr["CCY"] as string;
                                        item.ParentDummy = dr["ParentDummy"] as string;
                                        item.AccountName = dr["NAME"] as string;
                                        item.Level = Convert.ToInt32(dr["TL"]);
                                        item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                        item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                        item.COAID = dr["CoAID"] as string;
                                        item.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt32(dr["AG"]);
                                        item.ParentID = strContent2;

                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent2;
                                        if (i != 0)
                                        {
                                            if (LevelBefore > Level)
                                            {
                                                if (TotDetBefore == true)
                                                {
                                                    item.ParentID = items[i - 1].ParentDummy;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentdummy"];
                                                }
                                            }
                                            else
                                            {
                                                if (LevelBefore == Level)
                                                {
                                                    item.ParentID = items[i - 1].ParentID;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentid"];
                                                }
                                                else
                                                {
                                                    //XXX
                                                    if (Convert.ToString(strContent3).Trim() != "")
                                                    {
                                                        item.ParentID = strContent3;
                                                        

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent3;

                                                        if (strContent3.Substring(0, 3) != Convert.ToString(dsNew.Tables[0].Rows[i]["parentdummy"]).Substring(0, 3))
                                                        {
                                                            item.ParentID = strContent5;
                                                            

                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent5;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (strContent4.Trim() == "")
                                                        {
                                                            if (strContent5.Trim() == "")
                                                            {
                                                                if (strContent6.Trim() == "")
                                                                {
                                                                    if (strContent7.Trim() == "")
                                                                    {
                                                                        if (strContent8.Trim() == "")
                                                                        {
                                                                            item.ParentID = strContent9;
                                                                            

                                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent9;
                                                                        }
                                                                        else
                                                                        {
                                                                            item.ParentID = strContent8;
                                                                            

                                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent8;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        item.ParentID = strContent7;
                                                                        

                                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent7;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    item.ParentID = strContent6;
                                                                    

                                                                    dsNew.Tables[0].Rows[i]["parentid"] = strContent6;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                item.ParentID = strContent5;
                                                                

                                                                dsNew.Tables[0].Rows[i]["parentid"] = strContent5;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            item.ParentID = strContent4;
                                                            

                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent4;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        items.Add(item);
                                        break;
                                    case 1:
                                        strContent1 = Convert.ToString(dr["parentdummy"]);

                                        item = new COAView();
                                        item.Account = dr["ACC"] as string;
                                        item.CCy = new IDS.GeneralTable.Currency();
                                        item.CCy.CurrencyCode = dr["CCY"] as string;
                                        item.ParentDummy = dr["ParentDummy"] as string;
                                        item.AccountName = dr["NAME"] as string;
                                        item.Level = Convert.ToInt32(dr["TL"]);
                                        item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                        item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                        item.COAID = dr["CoAID"] as string;
                                        item.AccountGroup = (Tool.GLAccountGroup)Convert.ToInt32(dr["AG"]);
                                        item.ParentID = strContent1;

                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent1;
                                        if (i != 0)
                                        {
                                            if (LevelBefore > Level)
                                            {
                                                if (TotDetBefore == true)
                                                {
                                                    item.ParentID = items[i - 1].ParentDummy;
                                                    
                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentdummy"];
                                                }
                                            }
                                            else
                                            {
                                                if (LevelBefore == Level)
                                                {
                                                    item.ParentID = items[i - 1].ParentID;
                                                    

                                                    dsNew.Tables[0].Rows[i]["parentid"] = dsNew.Tables[0].Rows[i - 1]["parentid"];
                                                }
                                                else
                                                {
                                                    //XXX
                                                    if (Convert.ToString(strContent2).Trim() != "")
                                                    {
                                                        item.ParentID = strContent2;
                                                        

                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent2;

                                                        if (strContent2.Substring(0, 3) != Convert.ToString(dsNew.Tables[0].Rows[i]["parentdummy"]).Substring(0, 3))
                                                        {
                                                            item.ParentID = strContent4;
                                                            

                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent4;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (strContent3.Trim() == "")
                                                        {
                                                            if (strContent4.Trim() == "")
                                                            {
                                                                if (strContent5.Trim() == "")
                                                                {
                                                                    if (strContent6.Trim() == "")
                                                                    {
                                                                        if (strContent7.Trim() == "")
                                                                        {
                                                                            if (strContent8.Trim() == "")
                                                                            {
                                                                                item.ParentID = strContent9;
                                                                                

                                                                                dsNew.Tables[0].Rows[i]["parentid"] = strContent9;
                                                                            }
                                                                            else
                                                                            {
                                                                                item.ParentID = strContent8;
                                                                                

                                                                                dsNew.Tables[0].Rows[i]["parentid"] = strContent8;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            item.ParentID = strContent7;
                                                                            

                                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent7;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        item.ParentID = strContent6;
                                                                        

                                                                        dsNew.Tables[0].Rows[i]["parentid"] = strContent6;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    item.ParentID = strContent5;
                                                                    

                                                                    dsNew.Tables[0].Rows[i]["parentid"] = strContent5;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                item.ParentID = strContent4;
                                                                

                                                                dsNew.Tables[0].Rows[i]["parentid"] = strContent4;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            item.ParentID = strContent3;
                                                            

                                                            dsNew.Tables[0].Rows[i]["parentid"] = strContent3;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        items.Add(item);
                                        break;
                                }
                                i++;
                                #endregion

                                //COAView item = new COAView();
                                //item.Account = dr[""] as string;
                                //item.CCy = new IDS.GeneralTable.Currency();
                                //item.CCy.CurrencyCode = dr["CCY"] as string;

                                //item.ParentDummy = dr["ParentDummy"] as string;
                                //item.AccountName = dr["NAME"] as string;
                                //item.Level = Convert.ToInt32(dr["TL"]);
                                //item.AccountTotal = Convert.ToInt32(dr["TL"]);
                                //item.ProtectedAcc = Convert.ToBoolean(dr["PROTECTACC"]);
                                //item.COAID = dr["CoAID"] as string;
                                //item.AccountGroup = (Tool.GLAccountGroup) Convert.ToInt32(dr["AG"]);
                            }
                            
                            dr.Close();
                        }
                    }

                    db.Close();
                }

                

                if (items != null && items.Count > 0)
                {
                    root = items.Where(x => x.Level == 9).ToList();

                    foreach (COAView item in root)
                    {
                        List<COAView> children = new List<COAView>();

                        GenerateChildren(item, ref items);

                        if (children != null && children.Count == 0)
                        {
                            item.children.AddRange(children);
                        }
                    }
                }

                //string hasil = Newtonsoft.Json.JsonConvert.SerializeObject( root );
                
            }
            catch
            {
            }

            return root;
        }

        private void GenerateChildren(COAView parent, ref List<COAView> AllAccount)
        {
            if (AllAccount == null || AllAccount.Count == 0 || parent == null)
                return;

            List<COAView> temp = new List<COAView>();
            temp.AddRange(AllAccount.Where(x => x.ParentID.Replace(x.CCy.CurrencyCode, "") == parent.Account));
            temp.Remove(parent);
            temp = temp.OrderBy(x => x.Account).ThenBy(x => x.CCy.CurrencyCode).ThenBy(x => x.Level).ThenBy(x => x.AccountTotal).ToList();

            if (temp != null && temp.Count > 0)
            {
                parent.children = new List<COAView>(temp);

                foreach (COAView child in parent.children)
                {
                    GenerateChildren(child, ref AllAccount);
                }
            }
        }
    }
}