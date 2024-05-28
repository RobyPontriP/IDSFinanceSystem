using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GeneralTable
{
    public class Tax
    {
        [Display(Name = "Tax ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax id is required")]
        [MaxLength(10), StringLength(10)]
        public string TaxID { get; set; }

        [Display(Name = "Tax Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax name is required")]
        [MaxLength(20)]
        public string TaxName { get; set; }

        [Display(Name = "Tax Value")]
        public decimal TaxValue { get; set; }

        [Display(Name = "Prepaid Account")]
        public IDS.GLTable.ChartOfAccount PrepaidAccount { get; set; }

        [Display(Name = "Payable Account")]
        public IDS.GLTable.ChartOfAccount PayableAccount { get; set; }

        [Display(Name = "Dasar Pemotongan")]
        public decimal DSPPercent { get; set; }

        [Display(Name = "Tarif Non NPWP")]
        public decimal TarfNonNPWP { get; set; }

        [Display(Name = "Entry User")]
        public string EntryUser { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public Tax()
        {

        }

        public Tax(string taxID)
        {
            TaxID = taxID;
        }

        public static List<Tax> GetTax()
        {
            List<IDS.GeneralTable.Tax> list = new List<Tax>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelTax";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@taxID", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Tax tax = new Tax();
                            tax.TaxID = dr["TaxID"] as string;
                            tax.TaxName = dr["TaxName"] as string;
                            tax.TaxValue = string.IsNullOrEmpty(dr["TaxValue"].ToString()) ? 0 : Convert.ToDecimal(dr["TaxValue"]);

                            tax.PrepaidAccount = new IDS.GLTable.ChartOfAccount();
                            tax.PrepaidAccount.Account = dr["PrepaidAccount"] as string;

                            tax.PayableAccount = new IDS.GLTable.ChartOfAccount();
                            tax.PayableAccount.Account = dr["PayableAccount"] as string;

                            tax.DSPPercent = string.IsNullOrEmpty(dr["dsppercent"].ToString()) ? 0 : Convert.ToDecimal(dr["dsppercent"]);
                            tax.TarfNonNPWP = string.IsNullOrEmpty(dr["TarifNonSPWP"].ToString()) ? 0 : Convert.ToDecimal(dr["TarifNonSPWP"]);
                            tax.EntryUser = dr["EntryUser"] as string;
                            tax.EntryDate = IDS.Tool.GeneralHelper.NullToDateTime(dr["EntryDate"], DateTime.Now); //Convert.ToDateTime(dr["EntryDate"]);
                            tax.OperatorID = dr["OperatorID"] as string;
                            tax.LastUpdate = IDS.Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now); //Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(tax);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static Tax GetTax(string taxID)
        {
            Tax tax = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelTax";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@taxID", System.Data.SqlDbType.VarChar, taxID);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        tax = new Tax();
                        tax.TaxID = dr["TaxID"] as string;
                        tax.TaxName = dr["TaxName"] as string;
                        tax.TaxValue = string.IsNullOrEmpty(dr["TaxValue"].ToString()) ? 0 : Convert.ToDecimal(dr["TaxValue"]);

                        tax.PrepaidAccount = new IDS.GLTable.ChartOfAccount();
                        tax.PrepaidAccount.Account = dr["PrepaidAccount"] as string;

                        tax.PayableAccount = new IDS.GLTable.ChartOfAccount();
                        tax.PayableAccount.Account = dr["PayableAccount"] as string;

                        tax.DSPPercent = string.IsNullOrEmpty(dr["dsppercent"].ToString()) ? 0 : Convert.ToDecimal(dr["dsppercent"]);
                        tax.TarfNonNPWP = string.IsNullOrEmpty(dr["TarifNonSPWP"].ToString()) ? 0 : Convert.ToDecimal(dr["TarifNonSPWP"]);
                        tax.EntryUser = dr["EntryUser"] as string;
                        tax.EntryDate = IDS.Tool.GeneralHelper.NullToDateTime(dr["EntryDate"], DateTime.Now); //Convert.ToDateTime(dr["EntryDate"]);
                        tax.OperatorID = dr["OperatorID"] as string;
                        tax.LastUpdate = IDS.Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now); //Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return tax;
        }

        public static Tax GetTaxWithAcc(string Acc)
        {
            Tax tax = null;
            string taxId = "";

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
                            if (IDS.Tool.GeneralHelper.NullToString(dr["ACC"]) != null || IDS.Tool.GeneralHelper.NullToString(dr["ACC"]) != "")
                            {
                                if (IDS.Tool.GeneralHelper.NullToString(dr["ACC"]) == Acc)
                                {
                                    taxId = IDS.Tool.GeneralHelper.NullToString(dr["taxID"]);
                                    tax = new Tax();
                                    tax.TaxID = IDS.Tool.GeneralHelper.NullToString(dr["taxID"]); 
                                    tax.TaxName = IDS.Tool.GeneralHelper.NullToString(dr["TaxName"]);
                                    tax.TaxValue = string.IsNullOrEmpty(dr["TaxValue"].ToString()) ? 0 : Convert.ToDecimal(dr["TaxValue"]);

                                    tax.PrepaidAccount = new IDS.GLTable.ChartOfAccount();
                                    tax.PrepaidAccount.Account = IDS.Tool.GeneralHelper.NullToString(dr["PrepaidAccount"]);

                                    tax.PayableAccount = new IDS.GLTable.ChartOfAccount();
                                    tax.PayableAccount.Account = IDS.Tool.GeneralHelper.NullToString(dr["PayableAccount"]);

                                    tax.DSPPercent = IDS.Tool.GeneralHelper.NullToDecimal(dr["dsppercent"],0);
                                    tax.TarfNonNPWP = IDS.Tool.GeneralHelper.NullToDecimal(dr["TarifNonSPWP"], 0);
                                }
                            }

                        }
                    }
                }

                if (!string.IsNullOrEmpty(taxId))
                {
                    db.CommandText = "GTSelTax";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@taxID", System.Data.SqlDbType.VarChar, taxId);
                    db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                    db.Open();

                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();

                            tax = new Tax();
                            tax.TaxID = dr["TaxID"] as string;
                            tax.TaxName = dr["TaxName"] as string;
                            tax.TaxValue = string.IsNullOrEmpty(dr["TaxValue"].ToString()) ? 0 : Convert.ToDecimal(dr["TaxValue"]);

                            tax.PrepaidAccount = new IDS.GLTable.ChartOfAccount();
                            tax.PrepaidAccount.Account = dr["PrepaidAccount"] as string;

                            tax.PayableAccount = new IDS.GLTable.ChartOfAccount();
                            tax.PayableAccount.Account = dr["PayableAccount"] as string;

                            tax.DSPPercent = string.IsNullOrEmpty(dr["dsppercent"].ToString()) ? 0 : Convert.ToDecimal(dr["dsppercent"]);
                            tax.TarfNonNPWP = string.IsNullOrEmpty(dr["TarifNonSPWP"].ToString()) ? 0 : Convert.ToDecimal(dr["TarifNonSPWP"]);
                            tax.EntryUser = dr["EntryUser"] as string;
                            tax.EntryDate = IDS.Tool.GeneralHelper.NullToDateTime(dr["EntryDate"], DateTime.Now); //Convert.ToDateTime(dr["EntryDate"]);
                            tax.OperatorID = dr["OperatorID"] as string;
                            tax.LastUpdate = IDS.Tool.GeneralHelper.NullToDateTime(dr["LastUpdate"], DateTime.Now); //Convert.ToDateTime(dr["LastUpdate"]);
                        }

                        if (!dr.IsClosed)
                            dr.Close();
                    }
                }
                

                db.Close();
            }

            return tax;
        }

        public int InsUpDelTax(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdateTax";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@taxID", System.Data.SqlDbType.VarChar, TaxID);
                    cmd.AddParameter("@taxName", System.Data.SqlDbType.VarChar, TaxName);
                    cmd.AddParameter("@taxValue", System.Data.SqlDbType.Money, TaxValue);
                    cmd.AddParameter("@PREPAIDACC", System.Data.SqlDbType.VarChar, PrepaidAccount.Account);
                    cmd.AddParameter("@PAYABLEACC", System.Data.SqlDbType.VarChar, PayableAccount.Account);
                    cmd.AddParameter("@DSPPERCENT", System.Data.SqlDbType.Money, DSPPercent);
                    cmd.AddParameter("@TNONSPWP", System.Data.SqlDbType.Money, TarfNonNPWP);
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
                            throw new Exception("Tax id is already exists. Please choose other Tax id.");
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

        public int InsUpDelTax(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTUpdateTax";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "GTUpdateTax";
                        cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@taxID", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Tax ID is already exists. Please choose other Tax ID.");
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

        public static List<System.Web.Mvc.SelectListItem> GetTaxForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> taxs = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelTax";
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        taxs = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem tax = new System.Web.Mvc.SelectListItem();
                            tax.Value = dr["TaxID"] as string;
                            tax.Text = dr["TaxName"] as string;

                            taxs.Add(tax);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return taxs;
        }

        public static List<System.Web.Mvc.SelectListItem> GetTaxIDValueForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> taxs = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "GTSelTax";
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        taxs = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem tax = new System.Web.Mvc.SelectListItem();
                            tax.Value = dr["TaxID"] as string;
                            tax.Text = dr["TaxName"] as string;

                            taxs.Add(tax);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return taxs;
        }

        public static List<System.Web.Mvc.SelectListItem> GetTaxPPhForDataSource(string taxType)
        {
            List<System.Web.Mvc.SelectListItem> tax = new List<System.Web.Mvc.SelectListItem>();

            if (taxType == "PPh21")
            {
                tax.Add(new System.Web.Mvc.SelectListItem() { Text = "F 113301", Value = "F113301" });
                tax.Add(new System.Web.Mvc.SelectListItem() { Text = "F 113302", Value = "F113302" });
                tax.Add(new System.Web.Mvc.SelectListItem() { Text = "Referensi Lawan Transaksi", Value = "RLT" });
            }
            else if(taxType == "PPh23" || taxType == "PPh4")
            {
                tax.Add(new System.Web.Mvc.SelectListItem() { Text = "Bukti Potong", Value = "BP" });
                tax.Add(new System.Web.Mvc.SelectListItem() { Text = "Surat Setoran Pajak", Value = "SSP" });
                tax.Add(new System.Web.Mvc.SelectListItem() { Text = "Pemindah Bukuan", Value = "PBK" });
                tax.Add(new System.Web.Mvc.SelectListItem() { Text = "Referensi Lawan Transaksi", Value = "RLT" });
            }

            return tax;
        }

        public decimal CalculateOutstandingSalesTax(decimal amount,string cust, DateTime transDate, string invNo)
        {
            decimal outstanding = 0;
            decimal dspk = 0;
            decimal spwp = 0;
            decimal dpp = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "GTSelTax";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 5);
                    cmd.AddParameter("@taxID", System.Data.SqlDbType.VarChar, TaxID);
                    cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                    cmd.AddParameter("@transDate", System.Data.SqlDbType.DateTime, transDate);
                    cmd.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, invNo);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    //dspk = IDS.Tool.GeneralHelper.NullToDecimal(cmd.ExecuteScalar(),0) + (amount * DSPPercent);
                    dspk = IDS.Tool.GeneralHelper.NullToDecimal(cmd.ExecuteScalar(), 0);

                    cmd.CommandText = "GTSelTax";
                    cmd.AddParameter("@type", System.Data.SqlDbType.TinyInt, 6);
                    cmd.AddParameter("@taxID", System.Data.SqlDbType.VarChar, TaxID);
                    cmd.AddParameter("@cust", System.Data.SqlDbType.VarChar, cust);
                    cmd.AddParameter("@NONPWP", System.Data.SqlDbType.VarChar, TarfNonNPWP);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    spwp = IDS.Tool.GeneralHelper.NullToDecimal(cmd.ExecuteScalar(), 0);

                    dpp = amount * DSPPercent;

                    outstanding = (TaxValue * dpp * spwp) * -1;
                    //outstanding = (DSPPercent * dpp * spwp) * -1;
                }
                catch (SqlException sex)
                {
                }
                finally
                {
                    cmd.Close();
                }
            }

            return outstanding;
        }

        public void TaxTransaction(string invNo,decimal alloAmt,DateTime payDate, decimal amt,bool receive)
        {
            // amt=dpp
            amt = amt * DSPPercent;
            object nomorBp = DBNull.Value;

            if (!receive)
            {

            }
            else
            {
                using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                {
                    try
                    {
                        db.CommandText = @"UPDATE SLSInvH SET 
    isPPh = @TAX, PPhNo = @NOBP, PPhPercentage = @PERCENT,
    PPhAmount = @ALLO, PPhDateReceive = @DATE
WHERE InvoiceNumber = @INVNO";
                        db.AddParameter("@INVNO", System.Data.SqlDbType.VarChar, invNo);
                        db.AddParameter("@tax", System.Data.SqlDbType.VarChar, TaxID);
                        db.AddParameter("@ALLO", System.Data.SqlDbType.Money, Math.Abs(alloAmt));
                        db.AddParameter("@PERCENT", System.Data.SqlDbType.Money, DSPPercent);
                        db.AddParameter("@Date", System.Data.SqlDbType.DateTime, payDate);
                        db.AddParameter("@nobp", System.Data.SqlDbType.VarChar, nomorBp);
                        
                        db.CommandType = System.Data.CommandType.Text;
                        db.Open();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        db.Close();
                        GC.Collect();
                    }
                }
            }
        }
    }
}
