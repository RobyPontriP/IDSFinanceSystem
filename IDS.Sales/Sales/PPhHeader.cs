using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Sales
{
    public class PPhHeader
    {
        public IDS.GeneralTable.Supplier Supplier { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public string PPhType { get; set; }
        public decimal TotalAmount { get; set; }
        public bool NPWP { get; set; }
        public string JenisPenghasilan { get; set; }
        public List<PPhDetail> Detail { get; set; }
        public PPhDetail PPhDetail { get; set; }

        public PPhHeader()
        {

        }

        public static List<PPhHeader> GetPPhHeader()
        {
            List<PPhHeader> list = new List<PPhHeader>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelPPhHeader";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 3);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            PPhHeader pphHeader = new PPhHeader();
                            pphHeader.Supplier = new GeneralTable.Supplier();
                            pphHeader.Supplier.SupCode = IDS.Tool.GeneralHelper.NullToString(dr["CustSuppCode"],"");
                            pphHeader.Supplier.SupName = IDS.Tool.GeneralHelper.NullToString(dr["SupName"], "");
                            pphHeader.Supplier.NPWP = IDS.Tool.GeneralHelper.NullToString(dr["NPWP"], "");
                            pphHeader.Type = IDS.Tool.GeneralHelper.NullToString(dr["Type"], "");
                            pphHeader.Year = IDS.Tool.GeneralHelper.NullToInt(dr["Year"], 0);

                            pphHeader.PPhType = IDS.Tool.GeneralHelper.NullToString(dr["PPhType"]);
                            pphHeader.TotalAmount = Tool.GeneralHelper.NullToDecimal(dr["TotalAmount"], 0);
                            pphHeader.NPWP = Tool.GeneralHelper.NullToBool(dr["NPWP"], false);
                            pphHeader.JenisPenghasilan = IDS.Tool.GeneralHelper.NullToString(dr["JenisPenghasilan"]);
                            list.Add(pphHeader);
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return list;
        }// GetData

        public static PPhHeader GetPPhHeader(string sup, int year, string type)
        {
            PPhHeader pphHeader = new PPhHeader();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelPPhHeader";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@supcode", System.Data.SqlDbType.VarChar, sup);
                db.AddParameter("@Year", System.Data.SqlDbType.Int, year);
                db.AddParameter("@Type", System.Data.SqlDbType.VarChar, type);
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 4);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            pphHeader.Supplier = new GeneralTable.Supplier();
                            pphHeader.Supplier.SupCode = IDS.Tool.GeneralHelper.NullToString(dr["CustSuppCode"], "");
                            pphHeader.Supplier.SupName = IDS.Tool.GeneralHelper.NullToString(dr["SupName"], "");
                            pphHeader.Supplier.NPWP = IDS.Tool.GeneralHelper.NullToString(dr["NPWP"], "");
                            pphHeader.Type = IDS.Tool.GeneralHelper.NullToString(dr["Type"], "");
                            pphHeader.Year = IDS.Tool.GeneralHelper.NullToInt(dr["Year"], 0);

                            pphHeader.PPhType = IDS.Tool.GeneralHelper.NullToString(dr["PPhType"]);
                            pphHeader.TotalAmount = Tool.GeneralHelper.NullToDecimal(dr["TotalAmount"], 0);
                            pphHeader.NPWP = Tool.GeneralHelper.NullToBool(dr["NPWP"], false);
                            pphHeader.JenisPenghasilan = IDS.Tool.GeneralHelper.NullToString(dr["JenisPenghasilan"]);
                        }
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }
                db.Close();
            }
            return pphHeader;
        }

        public int InsUpDelPPh(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {

                    cmd.CommandText = "SalesPPhHeader";
                    cmd.AddParameter("@CustSuppCode", System.Data.SqlDbType.VarChar, Supplier.SupCode);
                    cmd.AddParameter("@Year", System.Data.SqlDbType.Int, Year);
                    string[] type = Type.Split('-');
                    if (type.Length > 1)
                    {
                        Type = type[1].Trim();
                    }
                    else
                    {
                        Type = type[0].Trim();
                    }
                    cmd.AddParameter("@Type", System.Data.SqlDbType.VarChar, Type);
                    cmd.AddParameter("@PPhType", System.Data.SqlDbType.VarChar, PPhType);
                    cmd.AddParameter("@TotalAmount", System.Data.SqlDbType.Money, TotalAmount);
                    cmd.AddParameter("@NPWP", System.Data.SqlDbType.Bit, NPWP);
                    cmd.AddParameter("@JenisPenghasilan", System.Data.SqlDbType.VarChar, IDS.GeneralTable.tblJenisPenghasilan.GetKodePenghasilan(PPhType,Type));
                    cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, (int)ExecCode);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();

                    cmd.BeginTransaction();
                    result = cmd.ExecuteNonQuery();

                    cmd.CommandText = "SalesPPhDetail";
                    cmd.AddParameter("@Sup", System.Data.SqlDbType.VarChar, Supplier.SupCode);
                    cmd.AddParameter("@Year", System.Data.SqlDbType.Int, Year);
                    cmd.AddParameter("@Desc", System.Data.SqlDbType.VarChar, Type);
                    cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, 3);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();
                    cmd.ExecuteNonQuery();
                    //cmd.CommitTransaction();

                    //cmd.BeginTransaction();
                    if (Detail.Count > 0 )
                    {
                        int seqNo = 1;
                        foreach (var item in Detail)
                        {
                            cmd.CommandText = "SalesPPhDetail";
                            cmd.AddParameter("@Sup", System.Data.SqlDbType.VarChar, Supplier.SupCode);
                            cmd.AddParameter("@Year", System.Data.SqlDbType.Int, Year);
                            cmd.AddParameter("@Month", System.Data.SqlDbType.Int, item.Month);
                            cmd.AddParameter("@SeqNo", System.Data.SqlDbType.TinyInt, seqNo);
                            cmd.AddParameter("@Desc", System.Data.SqlDbType.VarChar, item.Description);
                            cmd.AddParameter("@TaxRate", System.Data.SqlDbType.Money, item.TaxRate);
                            cmd.AddParameter("@Amount", System.Data.SqlDbType.Money, item.Amount);
                            cmd.AddParameter("@DasarPemotongan", System.Data.SqlDbType.Money, item.DasarPemotongan);
                            cmd.AddParameter("@DasarPemotonganKumulatif", System.Data.SqlDbType.Money, item.DasarPemotonganKumulatif);
                            cmd.AddParameter("@Tarif", System.Data.SqlDbType.Money, item.Tarif);
                            cmd.AddParameter("@TarifNonNPWP", System.Data.SqlDbType.Money, item.TarifNonNPWP);
                            cmd.AddParameter("@PPhTerutang", System.Data.SqlDbType.Money, item.PPhTerutang);
                            cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, 1);
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Open();
                            cmd.ExecuteNonQuery();

                            seqNo++;
                        }
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
                            throw new Exception("PPh code is already exists. Please choose other staff code.");
                        case 547:
                            throw new Exception("Data can not be delete while data used for reference.");
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

        public int InsUpDel(int ExecCode, string[] dataSup, int year, string[] type)
        {
            int result = 0;

            if (dataSup == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "SalesPPhHeader";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Open();
                    cmd.BeginTransaction();

                    for (int i = 0; i < dataSup.Length; i++)
                    {
                        cmd.AddParameter("@CustSuppCode", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(dataSup[i]));
                        cmd.AddParameter("@Year", System.Data.SqlDbType.Int, year);
                        cmd.AddParameter("@Type", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(type[i]));
                        cmd.AddParameter("@init", System.Data.SqlDbType.TinyInt, 3);


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

        public PPhHeader CalculateDetail(PPhHeader pphHeader,int FormActionDetail)
        {
            int seqNo = 0;
            //pphHeader.Detail.Add(pphHeader.PPhDetail);
            if (pphHeader.Detail.Count > 0)
            {
                if (FormActionDetail != 3)
                {
                    int sameMonth = 0;
                    foreach (var item in pphHeader.Detail)
                    {
                        
                        if (item.Month == pphHeader.PPhDetail.Month)
                        {
                            seqNo = item.SeqNo;
                            sameMonth = 1;
                        }

                        if (sameMonth == 0)
                        {
                            if (pphHeader.PPhDetail.Month > item.Month)
                            {
                                seqNo = item.SeqNo;
                            }
                            else if (pphHeader.PPhDetail.Month < item.Month)
                            {
                                seqNo = item.SeqNo - 1;
                                sameMonth = 1;
                            }
                        }
                    }

                    pphHeader.PPhDetail.Supplier = pphHeader.Supplier;
                    pphHeader.PPhDetail.Year = pphHeader.Year;
                    pphHeader.PPhDetail.DasarPemotongan = pphHeader.PPhDetail.Amount / 2;
                    string[] type = pphHeader.Type.Split('-');
                    if (type.Length > 1)
                    {
                        pphHeader.PPhDetail.Description = type[1].Trim();
                    }
                    else
                    {
                        pphHeader.PPhDetail.Description = type[0].Trim();
                    }

                    pphHeader.PPhDetail.MonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(pphHeader.PPhDetail.Month);
                }
                

                

                if (FormActionDetail == 1 || FormActionDetail == 2)
                {
                    pphHeader.Detail.Insert(seqNo, pphHeader.PPhDetail);
                }
                //else if (FormAction == 3)
                //{
                //    pphHeader.Detail.RemoveAll(x => x.Supplier.SupCode == pphHeader.PPhDetail.Supplier.SupCode && x.Year == pphHeader.PPhDetail.Year && x.Description == pphHeader.Type && x.SeqNo == pphHeader.PPhDetail.SeqNo && x.Month == pphHeader.PPhDetail.Month);
                //}
                

                int index = 0;
                pphHeader.TotalAmount = 0;

                //decimal dpkTemp = 0;
                foreach (var item in pphHeader.Detail)
                {
                    //dpkTemp += pphHeader.Detail[index].DasarPemotongan;
                    #region Kalkulasi Tarif
                    //if (pphHeader.Detail[index].DasarPemotonganKumulatif == 0)
                    //{
                    //    pphHeader.Detail[index].DasarPemotonganKumulatif = dpkTemp;
                    //}

                    //if (pphHeader.Detail[index].DasarPemotonganKumulatif <= 60000000)
                    //{
                    //    pphHeader.Detail[index].Tarif = Convert.ToDecimal(0.05);
                    //}
                    //else if (pphHeader.Detail[index].DasarPemotonganKumulatif > 60000000 && pphHeader.Detail[index].DasarPemotonganKumulatif <= 250000000)
                    //{
                    //    pphHeader.Detail[index].Tarif = Convert.ToDecimal(0.15);
                    //}
                    //else if (pphHeader.Detail[index].DasarPemotonganKumulatif > 250000000 && pphHeader.Detail[index].DasarPemotonganKumulatif <= 500000000)
                    //{
                    //    pphHeader.Detail[index].Tarif = Convert.ToDecimal(0.25);
                    //}
                    //else if (pphHeader.Detail[index].DasarPemotonganKumulatif > 500000000 && pphHeader.Detail[index].DasarPemotonganKumulatif <= 5000000000)
                    //{
                    //    pphHeader.Detail[index].Tarif = Convert.ToDecimal(0.30);
                    //}
                    //else if (pphHeader.Detail[index].DasarPemotonganKumulatif > 5000000000)
                    //{
                    //    pphHeader.Detail[index].Tarif = Convert.ToDecimal(0.35);
                    //}
                    #endregion

                    if (FormActionDetail != 3)
                    {
                        if (index >= seqNo)
                        {
                            if (index <= 0)
                            {
                                pphHeader.Detail[index].DasarPemotonganKumulatif = pphHeader.Detail[index].DasarPemotongan;
                            }
                            else
                            {
                                pphHeader.Detail[index].DasarPemotonganKumulatif = pphHeader.Detail[index - 1].DasarPemotonganKumulatif + pphHeader.Detail[index].DasarPemotongan;
                            }
                            
                            pphHeader.Detail[index].PPhTerutang = pphHeader.Detail[index].DasarPemotongan * pphHeader.Detail[index].Tarif * pphHeader.Detail[index].TarifNonNPWP;
                        }
                    }
                    else
                    {
                        if (index <= 0)
                        {
                            pphHeader.Detail[index].DasarPemotonganKumulatif = pphHeader.Detail[index].DasarPemotongan;
                        }
                        else
                        {
                            pphHeader.Detail[index].DasarPemotonganKumulatif = pphHeader.Detail[index - 1].DasarPemotonganKumulatif + pphHeader.Detail[index].DasarPemotongan;
                        }
                        pphHeader.Detail[index].PPhTerutang = pphHeader.Detail[index].DasarPemotongan * pphHeader.Detail[index].Tarif * pphHeader.Detail[index].TarifNonNPWP;
                    }
                    

                    pphHeader.TotalAmount = pphHeader.TotalAmount + pphHeader.Detail[index].PPhTerutang;
                    index++;

                    //
                    
                }
            }
            else
            {
                pphHeader.PPhDetail.Supplier = pphHeader.Supplier;
                pphHeader.PPhDetail.Year = pphHeader.Year;
                pphHeader.PPhDetail.DasarPemotongan = pphHeader.PPhDetail.Amount / 2;
                string[] type = pphHeader.Type.Split('-');
                if (type.Length > 1)
                {
                    pphHeader.PPhDetail.Description = type[1].Trim();
                }
                else
                {
                    pphHeader.PPhDetail.Description = type[0].Trim();
                }

                pphHeader.PPhDetail.SeqNo = 1;
                pphHeader.PPhDetail.MonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(pphHeader.PPhDetail.Month);
                pphHeader.PPhDetail.DasarPemotonganKumulatif = pphHeader.PPhDetail.DasarPemotongan;
                pphHeader.PPhDetail.PPhTerutang = pphHeader.PPhDetail.DasarPemotongan * pphHeader.PPhDetail.Tarif * pphHeader.PPhDetail.TarifNonNPWP;
                
                pphHeader.Detail.Insert(seqNo, pphHeader.PPhDetail);
            }
            return pphHeader;
        }

        public static List<System.Web.Mvc.SelectListItem> GetMonthForDataSource()
        {
            List<System.Web.Mvc.SelectListItem> month = new List<System.Web.Mvc.SelectListItem>();
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "January", Value = "1" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "February", Value = "2" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "March", Value = "3" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "April", Value = "4" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "May", Value = "5" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "Juny", Value = "6" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "July", Value = "7" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "August", Value = "8" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "September", Value = "9" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "October", Value = "10" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "November", Value = "11" });
            month.Add(new System.Web.Mvc.SelectListItem() { Text = "December", Value = "12" });

            return month;
        }
    }
}
