using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class TaxNumber
    {
        [Display(Name = "Serial Number")]
        public string SerialNo { get; set; }
        public string TaxNoTo { get; set; }
        public bool IsUsed { get; set; }
        public string DocumentNo { get; set; }
        public string TransType { get; set; }
        public string JenisFaktur { get; set; }
        public string CustName { get; set; }
        public bool Cancel { get; set; }

        public bool ExportStatus { get; set; }

        [Display(Name = "Entry User")]
        [MaxLength(20), StringLength(20)]
        public string EntryUser { get; set; }

        [Display(Name = "Entry Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Operator ID")]
        [MaxLength(20), StringLength(20)]
        public string OperatorID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

        public TaxNumber()
        {

        }

        public static TaxNumber GetTaxNumber(string serailNo)
        {
            TaxNumber tn = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelTaxNumber";
                db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, serailNo);
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 2);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        tn = new TaxNumber();
                        tn.SerialNo = IDS.Tool.GeneralHelper.NullToString(dr["SerialNo"], "");
                        tn.IsUsed = IDS.Tool.GeneralHelper.NullToBool(dr["IsUsed"], false);
                        tn.DocumentNo = IDS.Tool.GeneralHelper.NullToString(dr["DocumentNo"], "");
                        tn.TransType = IDS.Tool.GeneralHelper.NullToString(dr["TransType"], "");
                        tn.JenisFaktur = IDS.Tool.GeneralHelper.NullToString(dr["JenisFaktur"], "");
                        tn.Cancel = IDS.Tool.GeneralHelper.NullToBool(dr["Cancel"], false);
                        tn.OperatorID = IDS.Tool.GeneralHelper.NullToString(dr["OperatorID"], "");
                        tn.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return tn;
        }

        public static List<TaxNumber> GetListTaxNumber()
        {
            List<TaxNumber> list = new List<TaxNumber>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelTaxNumber";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            TaxNumber tn = new TaxNumber();
                            tn.SerialNo = IDS.Tool.GeneralHelper.NullToString(dr["SerialNo"],"");
                            tn.IsUsed = IDS.Tool.GeneralHelper.NullToBool(dr["IsUsed"], false);
                            tn.DocumentNo = IDS.Tool.GeneralHelper.NullToString(dr["DocumentNo"], "");
                            tn.TransType = IDS.Tool.GeneralHelper.NullToString(dr["TransType"], "");
                            tn.JenisFaktur = IDS.Tool.GeneralHelper.NullToString(dr["JenisFaktur"], "");
                            tn.Cancel = IDS.Tool.GeneralHelper.NullToBool(dr["Cancel"], false);
                            tn.ExportStatus = IDS.Tool.GeneralHelper.NullToBool(dr["ExportStatus"], false);
                            tn.OperatorID = IDS.Tool.GeneralHelper.NullToString(dr["OperatorID"], "");
                            tn.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            list.Add(tn);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<TaxNumber> GetListTaxNumber(string year)
        {
            List<TaxNumber> list = new List<TaxNumber>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelTaxNumber";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@year", System.Data.SqlDbType.VarChar, year);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            TaxNumber tn = new TaxNumber();
                            tn.SerialNo = IDS.Tool.GeneralHelper.NullToString(dr["SerialNo"], "");
                            tn.IsUsed = IDS.Tool.GeneralHelper.NullToBool(dr["IsUsed"], false);
                            tn.DocumentNo = IDS.Tool.GeneralHelper.NullToString(dr["DocumentNo"], "");
                            tn.TransType = IDS.Tool.GeneralHelper.NullToString(dr["TransType"], "");
                            tn.JenisFaktur = IDS.Tool.GeneralHelper.NullToString(dr["TaxInvoiceType"], "");
                            tn.Cancel = IDS.Tool.GeneralHelper.NullToBool(dr["Cancel"], false);
                            tn.ExportStatus = IDS.Tool.GeneralHelper.NullToBool(dr["ExportStatus"], false);
                            tn.CustName = IDS.Tool.GeneralHelper.NullToString(dr["Name"], "");
                            tn.OperatorID = IDS.Tool.GeneralHelper.NullToString(dr["OperatorID"], "");
                            tn.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                            list.Add(tn);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public int InsUpDelTaxNumber(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SalesSelTaxNumber";
                    cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, SerialNo);
                    cmd.AddParameter("@isUsed", System.Data.SqlDbType.Bit, IsUsed);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Tax Number is already exists. Please choose other Tax Number.");
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

        public int InsUpDelTaxNumber(int ExecCode,string taxNoTo)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SalesTaxNumber";
                    cmd.Open();
                    cmd.BeginTransaction();
                    
                    for (int taxNoFrom = IDS.Tool.GeneralHelper.NullToInt(SerialNo.Substring(7), 0); taxNoFrom <= Convert.ToInt64(taxNoTo); taxNoFrom++)
                    {
                        
                        cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, SerialNo.Substring(0,7) + taxNoFrom.ToString("00000000"));
                        cmd.AddParameter("@isUsed", System.Data.SqlDbType.Bit, IsUsed);
                        cmd.AddParameter("@TransType", System.Data.SqlDbType.VarChar, DBNull.Value);
                        cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        result = cmd.ExecuteNonQuery();
                    }
                    cmd.CommitTransaction();
                    cmd.Close();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Tax Number is already exists. Please choose other Tax Number.");
                        default:
                            throw;
                    }
                }
                catch(Exception e)
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

        public int InsUpDelTaxNumber(int ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SalesTaxNumber";
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < data.Length; i++)
                    {
                        cmd.CommandText = "SalesSelTaxNumber";
                        cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);
                        cmd.AddParameter("@serialNo", System.Data.SqlDbType.VarChar, data[i]);
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
                            throw new Exception("Tax Number is already exists. Please choose other Tax Number.");
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

        public static List<System.Web.Mvc.SelectListItem> GetTaxNumberForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> tns = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelTaxNumber";
                db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@year", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        tns = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem tn = new System.Web.Mvc.SelectListItem();
                            tn.Value = dr["SerialNo"] as string;
                            tn.Text = dr["SerialNo"] as string;

                            tns.Add(tn);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                //
                //db.CommandText = "select TaxNumber from SLSInvH WHERE TaxNumber is not null or TaxNumber <> ''";
                //db.CommandType = System.Data.CommandType.Text;
                //db.Open();

                //db.ExecuteReader();
                
                //using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                //{
                //    if (dr.HasRows)
                //    {
                //        while (dr.Read())
                //        {
                //            tns = new List<System.Web.Mvc.SelectListItem>(tns.Where(x => x.Value != dr["TaxNumber"] as string).ToList());
                //        }
                //    }

                //    if (!dr.IsClosed)
                //        dr.Close();
                //}

                db.Close();
            }

            return tns;
        }

        public static List<System.Web.Mvc.SelectListItem> GetTransType()
        {
            List<System.Web.Mvc.SelectListItem> TransType = new List<System.Web.Mvc.SelectListItem>();
            TransType.Add(new System.Web.Mvc.SelectListItem() { Text = "01 - Kepada Pihak Yang Bukan Pemungut Pajak", Value = "01" });
            TransType.Add(new System.Web.Mvc.SelectListItem() { Text = "02 - Kepada Pemungut Bendaharawan", Value = "02" });
            TransType.Add(new System.Web.Mvc.SelectListItem() { Text = "03 - Kepada Pemungut Selain Bendaharawan", Value = "03" });
            TransType.Add(new System.Web.Mvc.SelectListItem() { Text = "04 - DPP Nilai Lain", Value = "04" });
            TransType.Add(new System.Web.Mvc.SelectListItem() { Text = "06 - Penyerahan Lainnya", Value = "06" });
            TransType.Add(new System.Web.Mvc.SelectListItem() { Text = "07 - Penyerahan yang PPN-nya Tidak Dipungut", Value = "07" });
            TransType.Add(new System.Web.Mvc.SelectListItem() { Text = "08 - Penyerahan yang PPN-nya Dibebaskan", Value = "08" });
            TransType.Add(new System.Web.Mvc.SelectListItem() { Text = "09 - Penyerahan Aktiva (Pasal 16D UU PPN)", Value = "09" });

            return TransType;
        }

        public static List<System.Web.Mvc.SelectListItem> GetTaxType()
        {
            List<System.Web.Mvc.SelectListItem> TaxType = new List<System.Web.Mvc.SelectListItem>();
            TaxType.Add(new System.Web.Mvc.SelectListItem() { Text = "Faktur Pajak", Value = "0" });
            TaxType.Add(new System.Web.Mvc.SelectListItem() { Text = "Faktur Pajak Pengganti", Value = "1" });

            return TaxType;
        }

        public static string[] CheckExportFaktur(string[] invNo,int init)
        {
            List<string> invNoList = new List<string>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                for (int i = 0; i < invNo.Length; i++)
                {
                    db.CommandText = "CheckExportFaktur";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, init);
                    db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo[i]);
                    db.Open();

                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                invNoList.Add(dr["DocumentNo"] as string);
                            }
                                
                        }

                        if (!dr.IsClosed)
                            dr.Close();
                    }
                }


                db.Close();
            }

            return invNoList.ToArray();
        }
        
        public static bool CheckStatusExportFaktur(string invNo)
        {
            bool result = false;
            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = @"select DocumentNo,ExportStatus,JenisFaktur from tblTaxNumberList where DocumentNo = @InvNo";
                db.CommandType = System.Data.CommandType.Text;
                db.AddParameter("@InvNo", System.Data.SqlDbType.VarChar, invNo);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = Tool.GeneralHelper.NullToBool(dr["ExportStatus"],false);

                            if (Tool.GeneralHelper.NullToInt(dr["JenisFaktur"],0) == 1)
                            {
                                result = true;
                            }
                        }

                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }


                db.Close();
            }

            return result;
        }

        public static void UpdateStatusExport(string invNo)
        {
            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "update tblTaxNumberList set ExportStatus = 1 where DocumentNo = @invNo";
                    cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, invNo);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Open();

                    cmd.BeginTransaction();
                    cmd.ExecuteNonQuery();
                    cmd.CommitTransaction();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("Tax Number is already exists. Please choose other Tax Number.");
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
        }

        public int UpdateTaxNumber(string invNo)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = @"UPDATE tblTaxNumberList SET IsUsed = 1, DocumentNo=@invNo,TransType=@TaxTransType, JenisFaktur = @JenisFaktur
					, OperatorID = @operator, LastUpdate = GETDATE() where SerialNo = @SerialNo";
                    cmd.AddParameter("@invNo", System.Data.SqlDbType.VarChar, invNo);
                    cmd.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, SerialNo);
                    cmd.AddParameter("@TaxTransType", System.Data.SqlDbType.VarChar, TransType);
                    cmd.AddParameter("@JenisFaktur", System.Data.SqlDbType.VarChar, JenisFaktur);
                    cmd.AddParameter("@operator", System.Data.SqlDbType.VarChar, OperatorID);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();

                    if (JenisFaktur == "1" && result > 0)
                    {
                        cmd.CommandText = @"UPDATE tblTaxNumberList set ExportStatus = 0 WHERE SerialNo = @SerialNo";
                        cmd.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, SerialNo);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Open();
                        result = cmd.ExecuteNonQuery();
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
                            throw new Exception("Tax Number is already exists. Please choose other Tax Number.");
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

        //public static List<System.Web.Mvc.SelectListItem> GetTaxNumberForDataSource(string taxNo)
        //{
        //    List<System.Web.Mvc.SelectListItem> tns = new List<System.Web.Mvc.SelectListItem>();

        //    using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
        //    {
        //        db.CommandText = "SalesSelTaxNumber";
        //        db.AddParameter("@SerialNo", System.Data.SqlDbType.VarChar, DBNull.Value);
        //        db.AddParameter("@year", System.Data.SqlDbType.VarChar, DBNull.Value);
        //        db.AddParameter("@init", System.Data.SqlDbType.TinyInt, 4);
        //        db.CommandType = System.Data.CommandType.StoredProcedure;
        //        db.Open();

        //        db.ExecuteReader();

        //        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
        //        {
        //            if (dr.HasRows)
        //            {
        //                tns = new List<System.Web.Mvc.SelectListItem>();

        //                while (dr.Read())
        //                {
        //                    System.Web.Mvc.SelectListItem tn = new System.Web.Mvc.SelectListItem();
        //                    tn.Value = dr["SerialNo"] as string;
        //                    tn.Text = dr["SerialNo"] as string;

        //                    tns.Add(tn);
        //                }
        //            }

        //            if (!dr.IsClosed)
        //                dr.Close();
        //        }

        //        //
        //        db.CommandText = "select TaxNumber from SLSInvH WHERE TaxNumber is not null or TaxNumber <> ''";
        //        db.CommandType = System.Data.CommandType.Text;
        //        db.Open();

        //        db.ExecuteReader();

        //        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
        //        {
        //            if (dr.HasRows)
        //            {
        //                while (dr.Read())
        //                {
        //                    if (dr["TaxNumber"] as string != taxNo)
        //                    {
        //                        tns = new List<System.Web.Mvc.SelectListItem>(tns.Where(x => x.Value != dr["TaxNumber"] as string).ToList());
        //                    }

        //                }
        //            }

        //            if (!dr.IsClosed)
        //                dr.Close();
        //        }

        //        db.Close();
        //    }

        //    return tns;
        //}
    }
}
