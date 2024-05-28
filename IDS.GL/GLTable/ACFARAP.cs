using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.GLTable
{
    public class ACFARAP
    {
        [Display(Name = "RP")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RP")]
        [MaxLength(1)]
        public string RP { get; set; }

        public IDS.GLTable.SourceCode SCode { get; set; }

        [Display(Name = "Charts of Account AR/AP")]
        public IDS.GLTable.ChartOfAccount Acc { get; set; }

        [Display(Name = "Currency Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency Code is required")]
        public IDS.GeneralTable.Currency CCy { get; set; }

        [Display(Name = "Customer Prin")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer Prin")]
        [MaxLength(20)]
        public string CustPrin { get; set; }

        [Display(Name = "Doc No")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Doc No")]
        [MaxLength(15)]
        public string DocNo { get; set; }

        [Display(Name = "Branch Name")]
        public IDS.GeneralTable.Branch Branch { get; set; }

        [Display(Name = "Dept Code")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Dept Code")]
        [MaxLength(3)]
        public string DeptCode { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Received Date")]
        public DateTime ReceivedDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "", DataFormatString = IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Payment Term")]
        public int PaymentTerm { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Outstanding")]
        public decimal Outstanding { get; set; }

        [Display(Name = "Exchange Rate")]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Payment")]
        public decimal Payment { get; set; }

        [Display(Name = "Instrument No")]
        [MaxLength(50)]
        public string InstrumentNo { get; set; }

        [Display(Name = "Process Invoice")]
        public bool ProcessInv { get; set; }

        [Display(Name = "To Be Process")]
        public bool ToBeProcess { get; set; }

        [Display(Name = "Remark")]
        [MaxLength(2500)]
        public string Remark { get; set; }

        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }

        [Display(Name = "Equiv Amt")]
        public decimal EquivAmt { get; set; }

        //[Display(Name = "Customer")]
        //[MaxLength(20), StringLength(20)]
        //public GeneralTable.Customer Customer { get; set; }

        [Display(Name = "Customer")]
        //[MaxLength(20), StringLength(20)]
        public GeneralTable.Customer CustomerACFARAP { get; set; }

        [Display(Name = "Supplier")]
        //[MaxLength(20), StringLength(20)]
        public GeneralTable.Supplier Supplier { get; set; }

        [Display(Name = "Invoice")]
        public Sales.Invoice Invoice { get; set; }

        public GLTransaction.CashBankH CashBank { get; set; }

        [Display(Name = "SalesType")]
        public string SalesType { get; set; }

        [Display(Name = "Cash/Bank Type")]
        public string CBType { get; set; }

        public string OldDocNo { get; set; }
        public string OldRP { get; set; }
        public string OldAcc { get; set; }
        public string OldCcy { get; set; }
        public string OldCust { get; set; }
        public string OldBranch { get; set; }

        public List<SUBACFARAP> ListSUBACFARAP { get; set; }

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

        public ACFARAP()
        {

        }

        public string ARAPProcess(int totalData, ACFARAP procARAP)
        {
            string strResult = "";

            if  (totalData> 0)
            {
                using (DataAccess.SqlServer db = new DataAccess.SqlServer())
                {
                    db.CommandText = "GLSelACFCUST";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 4);
                    db.AddParameter("@CUSTCODE", System.Data.SqlDbType.VarChar, procARAP.CustomerACFARAP.CUSTCode);
                    db.Open();

                    db.ExecuteReader();

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            strResult += "Invoice with number " + procARAP.DocNo + " cannot be process, because Account Receivable Account, Sales Account or VAT Account for this customer is not set.";
                            //return strResult;
                        }
                        if (!dr.IsClosed)
                            dr.Close();
                    }

                    db.CommandText = "SelSUBACFARAP";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 4);
                    db.AddParameter("@RP", System.Data.SqlDbType.VarChar, procARAP.RP);
                    db.AddParameter("@acc", System.Data.SqlDbType.VarChar, procARAP.Acc.Account);
                    db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, procARAP.CCy.CurrencyCode);
                    db.AddParameter("@custprin", System.Data.SqlDbType.VarChar, procARAP.CustPrin);
                    db.AddParameter("@docno", System.Data.SqlDbType.VarChar, procARAP.DocNo);
                    db.AddParameter("@branchcode", System.Data.SqlDbType.VarChar, procARAP.Branch.BranchCode);

                    db.Open();

                    db.ExecuteReader();

                    if (procARAP.ProcessInv)
                    {
                        strResult += "Data already process";
                        //return strResult;
                    }

                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                if (string.IsNullOrEmpty(dr["SUBACC"].ToString()))
                                {
                                    strResult = "Sub Account for this invoice number (" + dr["SUBACC"].ToString() + ") has not been set.";
                                    return strResult;
                                }
                                //else
                                //{
                                //    return "";
                                //}
                            }
                        }
                        else
                        {
                            strResult = "Invoice with number " + procARAP.DocNo + " Can't be process " + "\n" + "\r" + " because the Sub Account not yet set.";

                            if (!dr.IsClosed)
                                dr.Close();

                            return strResult;
                            
                        }

                        if (!dr.IsClosed)
                            dr.Close();
                    }

                    try
                    {
                        db.CommandText = "ProcessARAP";
                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.AddParameter("@RP", System.Data.SqlDbType.VarChar,"A" + procARAP.RP);
                        db.AddParameter("@Acc", System.Data.SqlDbType.VarChar, procARAP.Acc.Account);
                        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, procARAP.CCy.CurrencyCode);
                        db.AddParameter("@Cust", System.Data.SqlDbType.VarChar, procARAP.CustomerACFARAP.CUSTCode);
                        db.AddParameter("@Doc", System.Data.SqlDbType.VarChar, procARAP.DocNo);
                        db.AddParameter("@PayTerm", System.Data.SqlDbType.Int, procARAP.PaymentTerm);
                        db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, procARAP.Branch.BranchCode);
                        db.AddParameter("@Operator", System.Data.SqlDbType.VarChar, OperatorID);
                        db.Open();

                        db.BeginTransaction();
                        db.ExecuteNonQuery();

                        if (string.IsNullOrEmpty(strResult))
                        {
                            strResult = "Process Done";
                        }
                        db.CommitTransaction();
                    }
                    catch (Exception e)
                    {
                        db.RollbackTransaction();
                        strResult = e.Message;
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }
            return strResult;
        }

        public string APProcess()
        {
            string strResult = "";

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                IDS.GLTransaction.GLVoucherH vh = new GLTransaction.GLVoucherH();
                vh.SCode = new SourceCode();
                vh.SCode.Code = SCode.Code;
                vh.VBranch = new GeneralTable.Branch();
                vh.VBranch.BranchCode = Branch.BranchCode;
                vh.TransDate = DocDate;
                VoucherNo = vh.GenerateNewVoucher(Branch.BranchCode, SCode.Code, DocDate);

                db.CommandText = "SelSUBACFARAP";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@TYPE", System.Data.SqlDbType.TinyInt, 4);
                db.AddParameter("@RP", System.Data.SqlDbType.VarChar, RP);
                db.AddParameter("@acc", System.Data.SqlDbType.VarChar, Acc.Account);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                db.AddParameter("@custprin", System.Data.SqlDbType.VarChar, CustPrin);
                db.AddParameter("@docno", System.Data.SqlDbType.VarChar, DocNo);
                db.AddParameter("@branchcode", System.Data.SqlDbType.VarChar, Branch.BranchCode);

                db.Open();

                db.ExecuteReader();

                if (ProcessInv)
                {
                    strResult += "Data already process, cannot process again";
                    return strResult;
                }

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (string.IsNullOrEmpty(dr["SUBACC"].ToString()))
                            {
                                strResult = "Sub Account for this Cash/Bank Number (" + DocNo + ") has not been set.";
                                return strResult;
                            }
                        }
                    }
                    else
                    {
                        strResult = "Cash/Bank Number " + DocNo + " Can't be process " + "\n" + "\r" + " because the Sub Account not yet set.";

                        if (!dr.IsClosed)
                            dr.Close();

                        return strResult;

                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.CommandText = "ProcessAP";
                db.DbCommand.CommandTimeout = 0;
                db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, SCode.Code);
                db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, VoucherNo); 
                db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Branch.BranchCode));
                db.AddParameter("@Sup", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Supplier.SupCode));
                db.AddParameter("@TRANS_DATE", System.Data.SqlDbType.DateTime, DocDate);
                db.AddParameter("@PAY_TERM", System.Data.SqlDbType.TinyInt, PaymentTerm);
                //db.AddParameter("@ARAP_TRANS", System.Data.SqlDbType.TinyInt, 0);
                db.AddParameter("@OPERATORID", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(OperatorID));
                db.AddParameter("@Sup", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Supplier.SupCode));
                db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Branch.BranchCode));
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Acc.Account));
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(CCy.CurrencyCode));
                //db.AddParameter("@PROJ", System.Data.SqlDbType.VarChar, DBNull.Value);
                db.AddParameter("@DOC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(DocNo));
                db.AddParameter("@UTYPE", System.Data.SqlDbType.TinyInt, 1);

                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.BeginTransaction();
                db.ExecuteNonQuery();

                int i = 1;
                foreach (var item in ListSUBACFARAP)
                {
                    db.CommandText = "ProcessAP";
                    db.DbCommand.CommandTimeout = 0;
                    db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, SCode.Code);
                    db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(VoucherNo));
                    db.AddParameter("@COUNTER", System.Data.SqlDbType.SmallInt, i);
                    db.AddParameter("@Sup", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Supplier.SupCode));
                    db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Branch.BranchCode));
                    db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(item.SubAcc.Account));
                    db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(item.CCySUBACFARAP.CurrencyCode));
                    db.AddParameter("@DOC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(DocNo));
                    db.AddParameter("@DESC_ACFTRANH", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(item.Description + "( "+DocNo+" )"));
                    db.AddParameter("@AMOUNT", System.Data.SqlDbType.Money, item.SubAmount);
                    db.AddParameter("@MATCHSTATUS", System.Data.SqlDbType.Bit, 0);
                    db.AddParameter("@UTYPE", System.Data.SqlDbType.TinyInt, 2);
                    db.CommandType = System.Data.CommandType.StoredProcedure;

                    int resultd = db.ExecuteNonQuery();

                    if (resultd <= 0)
                    {
                        db.RollbackTransaction();
                        throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                    }
                    i++;
                }

                db.CommandText = "ProcessAP";
                db.DbCommand.CommandTimeout = 0;
                db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, SCode.Code);
                db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(VoucherNo));
                db.AddParameter("@COUNTER", System.Data.SqlDbType.SmallInt, i);
                db.AddParameter("@Sup", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Supplier.SupCode));
                db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Branch.BranchCode));
                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(Acc.Account));
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(CCy.CurrencyCode));
                db.AddParameter("@DOC", System.Data.SqlDbType.VarChar, Tool.GeneralHelper.StringToDBNull(DocNo));
                db.AddParameter("@AMOUNT", System.Data.SqlDbType.Money, Amount);
                db.AddParameter("@MATCHSTATUS", System.Data.SqlDbType.Bit, 0);
                db.AddParameter("@UTYPE", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;

                int resulth = db.ExecuteNonQuery();

                if (resulth <= 0)
                {
                    db.RollbackTransaction();
                    throw new Exception("Failed to insert data. Please retry or contact your administrator.");
                }
                i++;

                if (!IDS.GLTransaction.GLVoucherD.CheckBalanceNewVoucher(SCode.Code, VoucherNo, Branch.BranchCode,db))
                {
                    db.RollbackTransaction();
                    throw new Exception("Debit and Credit Not Balance Please Check Your Data Again");
                }

                if (string.IsNullOrEmpty(strResult))
                {
                    db.CommitTransaction();
                    strResult = "Process Done";
                }
                else
                {
                    db.RollbackTransaction();
                }
            }


                return strResult;
        }

        public static ACFARAP GetACFARAP(string arap, string custCode, string ccy, string specialAcc, string docNo, string branchCode)
        {
            ACFARAP procARAP = null;

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelACFARAP";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@RP", System.Data.SqlDbType.VarChar, arap);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, custCode);
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@specialAccount", System.Data.SqlDbType.VarChar, specialAcc);
                db.AddParameter("@docNo", System.Data.SqlDbType.VarChar, docNo);
                db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 5);
                db.Open();
                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        dr.Read();

                        procARAP = new ACFARAP();
                        procARAP.RP = dr["RP"] as string;
                        procARAP.DocNo = dr["DOC_NO"] as string;
                        procARAP.DocDate = Convert.ToDateTime(dr["DOC_DATE"]);

                        procARAP.Branch = new GeneralTable.Branch();
                        procARAP.Branch.BranchCode = dr["branchcode"] as string;

                        procARAP.CustPrin = dr["CUST_PRIN"] as string;

                        procARAP.CCy = new GeneralTable.Currency();
                        procARAP.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                        procARAP.Acc = new GLTable.ChartOfAccount();
                        procARAP.Acc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                        procARAP.Invoice = new Sales.Invoice();
                        procARAP.Invoice.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["RelatedInvoice"]);

                        procARAP.ProcessInv = Tool.GeneralHelper.NullToBool(dr["Processed"]);
                        procARAP.DeptCode = dr["DEPTCODE"] as string;
                        procARAP.PaymentTerm = Tool.GeneralHelper.NullToInt(dr["PAYTERM"], 0);
                        procARAP.ReceivedDate = Convert.ToDateTime(dr["RECEIVEDDATE"]);
                        procARAP.Amount = Tool.GeneralHelper.NullToDecimal(dr["AMOUNT"], 0);
                        procARAP.Payment = Tool.GeneralHelper.NullToDecimal(dr["PAYMENT"], 0);
                        procARAP.Outstanding = Tool.GeneralHelper.NullToDecimal(dr["OUTSTANDING"], 0);
                        procARAP.InstrumentNo = dr["INSTRUMENT_NO"] as string;
                        procARAP.ToBeProcess = Tool.GeneralHelper.NullToBool(dr["tobeproces"]);
                        procARAP.Remark = dr["Remark"] as string;
                        procARAP.VoucherNo = dr["voucherNo"] as string;
                        procARAP.ExchangeRate = Tool.GeneralHelper.NullToDecimal(dr["ExchangeRate"], 0);
                        procARAP.EquivAmt = Convert.ToDecimal(dr["EQUIVAMT"]);

                        procARAP.CustomerACFARAP = new GeneralTable.Customer();
                        procARAP.CustomerACFARAP.CUSTCode = dr["CUST_PRIN"] as string;
                        procARAP.CustomerACFARAP.CUSTName = dr["NAME"] as string;

                        procARAP.SalesType = dr["SalesType"] as string;
                        procARAP.OperatorID = dr["OperatorID"] as string;
                        procARAP.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return procARAP;
        }

        //public static ACFARAP GetACFARAPVEND(string arap, string supCode, string ccy, string specialAcc, string docNo, string branchCode)
        //{
        //    ACFARAP procARAP = null;

        //    using (DataAccess.SqlServer db = new DataAccess.SqlServer())
        //    {
        //        db.CommandText = "SalesSelACFARAPVEND";
        //        db.CommandType = System.Data.CommandType.StoredProcedure;
        //        db.AddParameter("@RP", System.Data.SqlDbType.VarChar, arap);
        //        db.AddParameter("@sup", System.Data.SqlDbType.VarChar, supCode);
        //        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
        //        db.AddParameter("@specialAccount", System.Data.SqlDbType.VarChar, specialAcc);
        //        db.AddParameter("@docNo", System.Data.SqlDbType.VarChar, docNo);
        //        db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
        //        db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
        //        db.Open();
        //        db.ExecuteReader();

        //        using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
        //        {
        //            if (dr.HasRows)
        //            {
        //                dr.Read();

        //                procARAP = new ACFARAP();
        //                procARAP.RP = dr["RP"] as string;
        //                procARAP.DocNo = dr["DOC_NO"] as string;
        //                procARAP.DocDate = Convert.ToDateTime(dr["DOC_DATE"]);

        //                procARAP.Branch = new GeneralTable.Branch();
        //                procARAP.Branch.BranchCode = dr["branchcode"] as string;

        //                procARAP.Supplier.SupCode = dr["SupCode"] as string;

        //                procARAP.CCy = new GeneralTable.Currency();
        //                procARAP.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

        //                procARAP.Acc = new GLTable.ChartOfAccount();
        //                procARAP.Acc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

        //                procARAP.Invoice = new Sales.Invoice();
        //                procARAP.Invoice.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["RelatedInvoice"]);

        //                procARAP.ProcessInv = Tool.GeneralHelper.NullToBool(dr["Processed"]);
        //                procARAP.DeptCode = dr["DEPTCODE"] as string;
        //                procARAP.PaymentTerm = Tool.GeneralHelper.NullToInt(dr["PAYTERM"], 0);
        //                procARAP.ReceivedDate = Convert.ToDateTime(dr["RECEIVEDDATE"]);
        //                procARAP.Amount = Tool.GeneralHelper.NullToDecimal(dr["AMOUNT"], 0);
        //                procARAP.Payment = Tool.GeneralHelper.NullToDecimal(dr["PAYMENT"], 0);
        //                procARAP.Outstanding = Tool.GeneralHelper.NullToDecimal(dr["OUTSTANDING"], 0);
        //                procARAP.InstrumentNo = dr["INSTRUMENT_NO"] as string;
        //                procARAP.ToBeProcess = Tool.GeneralHelper.NullToBool(dr["tobeproces"]);
        //                procARAP.Remark = dr["Remark"] as string;
        //                procARAP.VoucherNo = dr["voucherNo"] as string;
        //                procARAP.ExchangeRate = Tool.GeneralHelper.NullToDecimal(dr["ExchangeRate"], 0);
        //                procARAP.EquivAmt = Convert.ToDecimal(dr["EQUIVAMT"]);

        //                procARAP.Supplier = new GeneralTable.Supplier();
        //                procARAP.Supplier.SupCode = dr["SupCode"] as string;
        //                procARAP.Supplier.SupName = dr["NAME"] as string;

        //                procARAP.SalesType = dr["SalesType"] as string;
        //                procARAP.OperatorID = dr["OperatorID"] as string;
        //                procARAP.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);
        //            }

        //            if (!dr.IsClosed)
        //                dr.Close();
        //        }

        //        db.Close();
        //    }

        //    return procARAP;
        //}

        public static ACFARAP GetACFARAPWithDetail(string arap, string custCode, string ccy, string specialAcc, string docNo, string branchCode)
        {
            ACFARAP procARAP = new ACFARAP();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelACFARAP";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@RP", System.Data.SqlDbType.VarChar, arap);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, custCode);
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@specialAccount", System.Data.SqlDbType.VarChar, specialAcc);
                db.AddParameter("@docNo", System.Data.SqlDbType.VarChar, docNo);
                db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 6);
                db.Open();
                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            procARAP.RP = dr["RP"] as string;
                            procARAP.DocNo = dr["DOC_NO"] as string;
                            procARAP.DocDate = Convert.ToDateTime(dr["DOC_DATE"]);
                            procARAP.ReceivedDate = Convert.ToDateTime(dr["RECEIVEDDATE"]);

                            procARAP.Branch = new GeneralTable.Branch();
                            procARAP.Branch.BranchCode = dr["branchcode"] as string;

                            procARAP.CustPrin = dr["CUST_PRIN"] as string;

                            procARAP.CCy = new GeneralTable.Currency();
                            procARAP.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            procARAP.Acc = new GLTable.ChartOfAccount();
                            procARAP.Acc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            procARAP.Invoice = new Sales.Invoice();
                            procARAP.Invoice.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["RelatedInvoice"]);

                            procARAP.ProcessInv = Tool.GeneralHelper.NullToBool(dr["Processed"]);
                            procARAP.DeptCode = dr["DEPTCODE"] as string;
                            procARAP.PaymentTerm = Tool.GeneralHelper.NullToInt(dr["PAYTERM"], 0);
                            procARAP.ReceivedDate = Convert.ToDateTime(dr["RECEIVEDDATE"]);
                            procARAP.Amount = Tool.GeneralHelper.NullToDecimal(dr["AMOUNT"], 0);
                            procARAP.Payment = Tool.GeneralHelper.NullToDecimal(dr["PAYMENT"], 0);
                            procARAP.Outstanding = Tool.GeneralHelper.NullToDecimal(dr["OUTSTANDING"], 0);
                            procARAP.InstrumentNo = dr["INSTRUMENT_NO"] as string;
                            procARAP.ToBeProcess = Tool.GeneralHelper.NullToBool(dr["tobeproces"]);
                            procARAP.Remark = dr["Remark"] as string;
                            procARAP.VoucherNo = dr["voucherNo"] as string;
                            procARAP.ExchangeRate = Tool.GeneralHelper.NullToDecimal(dr["ExchangeRate"], 0);
                            procARAP.EquivAmt = Convert.ToDecimal(dr["EQUIVAMT"]);

                            procARAP.CustomerACFARAP = new GeneralTable.Customer();
                            procARAP.CustomerACFARAP.CUSTCode = dr["CUST_PRIN"] as string;
                            procARAP.CustomerACFARAP.CUSTName = dr["NAME"] as string;

                            procARAP.SalesType = dr["SalesType"] as string;
                            procARAP.OperatorID = dr["OperatorID"] as string;
                            procARAP.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            #region Fill Detail
                            if (procARAP.ListSUBACFARAP == null)
                                procARAP.ListSUBACFARAP = new List<SUBACFARAP>();

                            SUBACFARAP subacfarap = new SUBACFARAP();
                            subacfarap.RP = dr["subRP"] as string;
                            subacfarap.SeqNo = IDS.Tool.GeneralHelper.NullToInt(dr["SEQNO"], 0);

                            subacfarap.Acc = new ChartOfAccount();
                            subacfarap.Acc.Account = dr["subacfarapACC"] as string;

                            subacfarap.SubAcc = new ChartOfAccount();
                            subacfarap.SubAcc.Account = dr["SUBACFARAPSUBACC"] as string;

                            subacfarap.CCySUBACFARAP = new GeneralTable.Currency();
                            subacfarap.CCySUBACFARAP.CurrencyCode = dr["subCCY"] as string;

                            subacfarap.Description = dr["subDesc"] as string;
                            subacfarap.SubAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["amtDetail"], 0);
                            subacfarap.EquivAmt = IDS.Tool.GeneralHelper.NullToDecimal(dr["subEQUIVAMT"], 0);

                            procARAP.ListSUBACFARAP.Add(subacfarap);
                            #endregion
                        }


                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return procARAP;
        }

        public static ACFARAP GetACFARAPVendWithDetail(string arap, string supCode, string ccy, string specialAcc, string docNo, string branchCode)
        {
            ACFARAP procARAP = new ACFARAP();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelACFARAPVEND";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@RP", System.Data.SqlDbType.VarChar, arap);
                db.AddParameter("@sup", System.Data.SqlDbType.VarChar, supCode);
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@specialAccount", System.Data.SqlDbType.VarChar, specialAcc);
                db.AddParameter("@docNo", System.Data.SqlDbType.VarChar, docNo);
                db.AddParameter("@branchCode", System.Data.SqlDbType.VarChar, branchCode);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.Open();
                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            procARAP.RP = dr["RP"] as string;
                            procARAP.DocNo = dr["DOC_NO"] as string;
                            procARAP.DocDate = Convert.ToDateTime(dr["DOC_DATE"]);
                            procARAP.ReceivedDate = Convert.ToDateTime(dr["RECEIVEDDATE"]);

                            procARAP.Branch = new GeneralTable.Branch();
                            procARAP.Branch.BranchCode = dr["branchcode"] as string;

                            procARAP.CCy = new GeneralTable.Currency();
                            procARAP.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            procARAP.Acc = new GLTable.ChartOfAccount();
                            procARAP.Acc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);

                            procARAP.Invoice = new Sales.Invoice();
                            procARAP.Invoice.InvoiceNumber = Tool.GeneralHelper.NullToString(dr["RelatedInvoice"]);

                            procARAP.SCode = new SourceCode();
                            procARAP.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCode"]);

                            procARAP.ProcessInv = Tool.GeneralHelper.NullToBool(dr["Processed"]);
                            procARAP.DeptCode = dr["DEPTCODE"] as string;
                            procARAP.PaymentTerm = Tool.GeneralHelper.NullToInt(dr["PAYTERM"], 0);
                            procARAP.ReceivedDate = Convert.ToDateTime(dr["RECEIVEDDATE"]);
                            procARAP.Amount = Tool.GeneralHelper.NullToDecimal(dr["AMOUNT"], 0);
                            procARAP.Payment = Tool.GeneralHelper.NullToDecimal(dr["PAYMENT"], 0);
                            procARAP.Outstanding = Tool.GeneralHelper.NullToDecimal(dr["OUTSTANDING"], 0);
                            procARAP.InstrumentNo = dr["INSTRUMENT_NO"] as string;
                            procARAP.ToBeProcess = Tool.GeneralHelper.NullToBool(dr["tobeproces"]);
                            procARAP.Remark = dr["Remark"] as string;
                            procARAP.VoucherNo = dr["voucherNo"] as string;
                            procARAP.ExchangeRate = Tool.GeneralHelper.NullToDecimal(dr["ExchangeRate"], 0);
                            procARAP.EquivAmt = Convert.ToDecimal(dr["EQUIVAMT"]);

                            procARAP.Supplier = new GeneralTable.Supplier();
                            procARAP.Supplier.SupCode = dr["SupCode"] as string;
                            procARAP.Supplier.SupName = dr["NAME"] as string;

                            procARAP.CBType = dr["CBType"] as string;
                            procARAP.OperatorID = dr["OperatorID"] as string;
                            procARAP.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            procARAP.CashBank = new GLTransaction.CashBankH();
                            procARAP.CashBank.EntryUser = Tool.GeneralHelper.NullToString(dr["ENTRYUSER"]);
                            procARAP.CashBank.Approval1 = Tool.GeneralHelper.NullToString(dr["Approval1"]);
                            procARAP.CashBank.Approval2 = Tool.GeneralHelper.NullToString(dr["Approval2"]);
                            procARAP.CashBank.Approval3 = Tool.GeneralHelper.NullToString(dr["Approval3"]);

                            #region Fill Detail
                            if (procARAP.ListSUBACFARAP == null)
                                procARAP.ListSUBACFARAP = new List<SUBACFARAP>();

                            SUBACFARAP subacfarap = new SUBACFARAP();
                            subacfarap.RP = dr["subRP"] as string;
                            subacfarap.SeqNo = IDS.Tool.GeneralHelper.NullToInt(dr["SEQNO"], 0);

                            subacfarap.Acc = new ChartOfAccount();
                            subacfarap.Acc.Account = dr["subacfarapACC"] as string;

                            subacfarap.SubAcc = new ChartOfAccount();
                            subacfarap.SubAcc.Account = dr["SUBACFARAPSUBACC"] as string;

                            subacfarap.CCySUBACFARAP = new GeneralTable.Currency();
                            subacfarap.CCySUBACFARAP.CurrencyCode = dr["subCCY"] as string;

                            subacfarap.Description = dr["subDesc"] as string;
                            subacfarap.SubAmount = IDS.Tool.GeneralHelper.NullToDecimal(dr["amtDetail"], 0);
                            subacfarap.EquivAmt = IDS.Tool.GeneralHelper.NullToDecimal(dr["subEQUIVAMT"], 0);

                            procARAP.ListSUBACFARAP.Add(subacfarap);
                            #endregion
                        }


                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return procARAP;
        }

        public static List<ACFARAP> GetACFARAP(string arap,string custCode, string ccy,string specialAcc)
        {
            List<IDS.GLTable.ACFARAP> list = new List<ACFARAP>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelACFARAP";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@RP", System.Data.SqlDbType.VarChar, string.IsNullOrEmpty(arap) ? "" : arap.Substring(1,1));
                //db.AddParameter("@cust", System.Data.SqlDbType.VarChar, custCode);
                db.AddParameter("@cust", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(custCode));
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@specialAccount", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(specialAcc));
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            ACFARAP procARAP = new ACFARAP();
                            procARAP.RP = Tool.GeneralHelper.NullToString(dr["RP"]);
                            procARAP.DocNo = Tool.GeneralHelper.NullToString(dr["DOC_NO"]);

                            if (!string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["DOC_DATE"])))
                                procARAP.DocDate = Convert.ToDateTime(dr["DOC_DATE"]);

                            procARAP.Branch = new GeneralTable.Branch();
                            procARAP.Branch.BranchCode = IDS.Tool.GeneralHelper.NullToString(dr["branchcode"]);

                            procARAP.CustPrin = Tool.GeneralHelper.NullToString(dr["CUST_PRIN"]);

                            procARAP.CCy = new GeneralTable.Currency();
                            procARAP.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            procARAP.Acc = new GLTable.ChartOfAccount();
                            procARAP.Acc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);
                            procARAP.Acc.AccountName = Tool.GeneralHelper.NullToString(dr["ACCNAME"]);

                            //procARAP.Acc = new GLTable.ChartOfAccount();
                            //procARAP.Acc = ChartOfAccount.GetCOA(Tool.GeneralHelper.NullToString(dr["ACC"]), Tool.GeneralHelper.NullToString(dr["CCY"]));

                            procARAP.ProcessInv = Tool.GeneralHelper.NullToBool(dr["Processed"]);
                            procARAP.DeptCode = Tool.GeneralHelper.NullToString(dr["DEPTCODE"]);
                            procARAP.PaymentTerm = Tool.GeneralHelper.NullToInt(dr["PAYTERM"],0);
                            procARAP.ReceivedDate = Convert.ToDateTime(dr["RECEIVEDDATE"]);
                            procARAP.Amount = Tool.GeneralHelper.NullToDecimal(dr["AMOUNT"],0);
                            procARAP.Payment = Tool.GeneralHelper.NullToDecimal(dr["PAYMENT"], 0);
                            procARAP.Outstanding = Tool.GeneralHelper.NullToDecimal(dr["OUTSTANDING"], 0);
                            procARAP.InstrumentNo = Tool.GeneralHelper.NullToString(dr["INSTRUMENT_NO"]);
                            procARAP.ToBeProcess = Tool.GeneralHelper.NullToBool(dr["tobeproces"]);
                            procARAP.Remark = Tool.GeneralHelper.NullToString(dr["Remark"]);
                            procARAP.VoucherNo = Tool.GeneralHelper.NullToString(dr["voucherNo"]);
                            procARAP.EquivAmt = Convert.ToDecimal(dr["EQUIVAMT"]);

                            procARAP.CustomerACFARAP = new GeneralTable.Customer();
                            procARAP.CustomerACFARAP.CUSTCode = Tool.GeneralHelper.NullToString(dr["CUST_PRIN"]);
                            procARAP.CustomerACFARAP.CUSTName = Tool.GeneralHelper.NullToString(dr["NAME"]);

                            procARAP.SalesType = Tool.GeneralHelper.NullToString(dr["SalesType"]);
                            procARAP.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);

                            if (!string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["LastUpdate"])))
                                procARAP.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(procARAP);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<ACFARAP> GetACFARAPVEND(string arap, string supCode, string ccy, string specialAcc)
        {
            List<IDS.GLTable.ACFARAP> list = new List<ACFARAP>();

            using (DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelACFARAPVEND";
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.AddParameter("@RP", System.Data.SqlDbType.VarChar, string.IsNullOrEmpty(arap) ? "" : arap.Substring(1, 1));
                db.AddParameter("@sup", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(supCode));
                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@specialAccount", System.Data.SqlDbType.VarChar, IDS.Tool.GeneralHelper.StringToDBNull(specialAcc));
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 1);
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            ACFARAP procARAP = new ACFARAP();
                            procARAP.RP = Tool.GeneralHelper.NullToString(dr["RP"]);
                            procARAP.DocNo = Tool.GeneralHelper.NullToString(dr["DOC_NO"]);

                            if (!string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["DOC_DATE"])))
                                procARAP.DocDate = Convert.ToDateTime(dr["DOC_DATE"]);

                            procARAP.Branch = new GeneralTable.Branch();
                            procARAP.Branch.BranchCode = IDS.Tool.GeneralHelper.NullToString(dr["branchcode"]);

                            procARAP.Supplier = new GeneralTable.Supplier();
                            procARAP.Supplier.SupCode = Tool.GeneralHelper.NullToString(dr["SupCode"]);
                            procARAP.Supplier.SupName = Tool.GeneralHelper.NullToString(dr["Name"]);

                            procARAP.CCy = new GeneralTable.Currency();
                            procARAP.CCy.CurrencyCode = Tool.GeneralHelper.NullToString(dr["CCY"]);

                            procARAP.Acc = new GLTable.ChartOfAccount();
                            procARAP.Acc.Account = Tool.GeneralHelper.NullToString(dr["ACC"]);
                            procARAP.Acc.AccountName = Tool.GeneralHelper.NullToString(dr["ACCNAME"]);

                            //procARAP.Acc = new GLTable.ChartOfAccount();
                            //procARAP.Acc = ChartOfAccount.GetCOA(Tool.GeneralHelper.NullToString(dr["ACC"]), Tool.GeneralHelper.NullToString(dr["CCY"]));
                            procARAP.SCode = new SourceCode();
                            procARAP.SCode.Code = Tool.GeneralHelper.NullToString(dr["SCode"]);

                            procARAP.ProcessInv = Tool.GeneralHelper.NullToBool(dr["Processed"]);
                            procARAP.DeptCode = Tool.GeneralHelper.NullToString(dr["DEPTCODE"]);
                            procARAP.PaymentTerm = Tool.GeneralHelper.NullToInt(dr["PAYTERM"], 0);
                            procARAP.ReceivedDate = Convert.ToDateTime(dr["RECEIVEDDATE"]);
                            procARAP.Amount = Tool.GeneralHelper.NullToDecimal(dr["AMOUNT"], 0);
                            procARAP.Payment = Tool.GeneralHelper.NullToDecimal(dr["PAYMENT"], 0);
                            procARAP.Outstanding = Tool.GeneralHelper.NullToDecimal(dr["OUTSTANDING"], 0);
                            procARAP.InstrumentNo = Tool.GeneralHelper.NullToString(dr["INSTRUMENT_NO"]);
                            procARAP.ToBeProcess = Tool.GeneralHelper.NullToBool(dr["tobeproces"]);
                            procARAP.Remark = Tool.GeneralHelper.NullToString(dr["Remark"]);
                            procARAP.VoucherNo = Tool.GeneralHelper.NullToString(dr["voucherNo"]);
                            procARAP.EquivAmt = Convert.ToDecimal(dr["EQUIVAMT"]);

                            procARAP.CBType = Tool.GeneralHelper.NullToString(dr["cbType"]);
                            procARAP.OperatorID = Tool.GeneralHelper.NullToString(dr["OperatorID"]);

                            if (!string.IsNullOrEmpty(IDS.Tool.GeneralHelper.NullToString(dr["LastUpdate"])))
                                procARAP.LastUpdate = Convert.ToDateTime(dr["LastUpdate"]);

                            list.Add(procARAP);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return list;
        }

        public static List<System.Web.Mvc.SelectListItem> GetACFARAPForDataSource(string arap)
        {
            List<System.Web.Mvc.SelectListItem> acfaraps = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelACFARAP";
                db.AddParameter("@RP", System.Data.SqlDbType.VarChar, arap);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        acfaraps = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem acfarap = new System.Web.Mvc.SelectListItem();
                            acfarap.Value = Tool.GeneralHelper.NullToString(dr["CUST_PRIN"]);
                            acfarap.Text = Tool.GeneralHelper.NullToString(dr["NAME"]);

                            acfaraps.Add(acfarap);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return acfaraps;
        }

        public static List<System.Web.Mvc.SelectListItem> GetACFARAPVENDForDataSource(string arap)
        {
            List<System.Web.Mvc.SelectListItem> acfaraps = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelACFARAPVEND";
                db.AddParameter("@RP", System.Data.SqlDbType.VarChar, arap);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 3);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        acfaraps = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem acfarap = new System.Web.Mvc.SelectListItem();
                            acfarap.Value = Tool.GeneralHelper.NullToString(dr["SupCode"]);
                            acfarap.Text = Tool.GeneralHelper.NullToString(dr["NAME"]);

                            acfaraps.Add(acfarap);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return acfaraps;
        }

        public static List<System.Web.Mvc.SelectListItem> GetAccACFARAPForDataSource(string arap, string ccy)
        {
            List<System.Web.Mvc.SelectListItem> acfaraps = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SalesSelACFARAP";
                db.AddParameter("@RP", System.Data.SqlDbType.VarChar, arap);
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        acfaraps = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            System.Web.Mvc.SelectListItem acfarap = new System.Web.Mvc.SelectListItem();
                            acfarap.Value = Tool.GeneralHelper.NullToString(dr["ACC"]);
                            acfarap.Text = Tool.GeneralHelper.NullToString(dr["NAME"]);

                            acfaraps.Add(acfarap);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return acfaraps;
        }

        public static List<System.Web.Mvc.SelectListItem> GetAccACFARAPForDataSource(string ccy)
        {
            List<System.Web.Mvc.SelectListItem> acfaraps = new List<System.Web.Mvc.SelectListItem>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelACFARAPVEND";
                db.AddParameter("@ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@Type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        acfaraps = new List<System.Web.Mvc.SelectListItem>();

                        while (dr.Read())
                        {
                            if (Tool.GeneralHelper.NullToString(dr["TYPE_ACC"]) == "BN" || Tool.GeneralHelper.NullToString(dr["TYPE_ACC"]) == "KS" || Tool.GeneralHelper.NullToString(dr["TYPE_ACC"]) == "AP")
                            {
                                System.Web.Mvc.SelectListItem acfarap = new System.Web.Mvc.SelectListItem();
                                acfarap.Value = Tool.GeneralHelper.NullToString(dr["ACC"]);
                                acfarap.Text = Tool.GeneralHelper.NullToString(dr["NAME"]);

                                acfaraps.Add(acfarap);
                            }
                            
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }

            return acfaraps;
        }


        public int InsUpDelAP(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.Open();
                    cmd.BeginTransaction();

                    #region Adjust Outstanding  
                    // Adjust Outstanding
                    if (RP == "P")
                    {
                        List<SUBACFARAP> listSUBACFARAP = new List<SUBACFARAP>();
                        listSUBACFARAP = IDS.GLTable.SUBACFARAP.GetSUBACFARAP(DocNo);
                        IDS.GLTable.CustomerOutstanding custOuts = new GLTable.CustomerOutstanding();
                        foreach (var item in listSUBACFARAP)
                        {
                            custOuts.Period = DocDate.ToString("yyyyMM");
                            custOuts.CustCode = CustomerACFARAP.CUSTCode;
                            custOuts.Ccy = new GeneralTable.Currency();
                            custOuts.Ccy.CurrencyCode = item.CCySUBACFARAP.CurrencyCode;
                            custOuts.Debit = item.SubAmount;

                            custOuts.AdjustCreditOutstanding(1, cmd);
                        }
                    }
                    #endregion

                    #region Update ACFARAP
                    InsUpDelACFARAP(ExecCode, cmd);
                    #endregion

                    cmd.Open();
                    cmd.BeginTransaction();
                    #region Delete SUBACFARAP
                    SUBACFARAP subacfarap = new SUBACFARAP();
                    subacfarap.DocNo = DocNo;
                    subacfarap.RP = RP;
                    subacfarap.CCySUBACFARAP = CCy;
                    subacfarap.CustPrin = CustomerACFARAP.CUSTCode;
                    subacfarap.Branch = Branch;
                    subacfarap.DelSUBACFARAP(4, cmd);
                    #endregion

                    #region Update SUBACFARAP
                    foreach (var item in ListSUBACFARAP)
                    {
                        item.RP = RP;
                        item.DocNo = DocNo;
                        item.CustPrin = CustomerACFARAP.CUSTCode;
                        item.Branch = Branch;
                        item.Acc = Acc;
                        IDS.GLTable.SUBACFARAP.InsUpSUBACFARAP(1, cmd,item);

                        // Adjust Outstanding
                        if (RP == "P")
                        {
                            IDS.GLTable.CustomerOutstanding custOuts = new GLTable.CustomerOutstanding();
                            custOuts.Period = DocDate.ToString("yyyyMM");
                            custOuts.CustCode = CustomerACFARAP.CUSTCode;
                            custOuts.Ccy = new GeneralTable.Currency();
                            custOuts.Ccy.CurrencyCode = item.CCySUBACFARAP.CurrencyCode;
                            custOuts.Debit = item.SubAmount;

                            custOuts.AdjustCreditOutstanding(2, cmd);
                        }
                    }
                    #endregion
                    
                    cmd.CommitTransaction();
                }
                catch (System.Data.SqlClient.SqlException sex)
                {
                    if (cmd.Transaction != null)
                        cmd.RollbackTransaction();

                    switch (sex.Number)
                    {
                        case 2627:
                            throw new Exception("ACFARAP is already exists. Please choose other ACFARAP.");
                        default:
                            throw;
                    }
                }

                finally
                {
                    cmd.Close();
                    result = 1;
                }
            }

            return result;
        }

        public int InsUpDelACFARAP(int ExecCode)
        {
            int result = 0;

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "ARAPSaveHeader";
                    cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);
                    cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, RP);
                    cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Acc.Account);
                    cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                    cmd.AddParameter("@CUST_PRIN", System.Data.SqlDbType.VarChar, CustomerACFARAP.CUSTCode);
                    cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DocNo);
                    cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                    cmd.AddParameter("@DEPTCODE", System.Data.SqlDbType.VarChar, DeptCode);
                    cmd.AddParameter("@DOC_DATE", System.Data.SqlDbType.DateTime, DocDate);
                    cmd.AddParameter("@RECEIVEDDATE", System.Data.SqlDbType.DateTime, ReceivedDate);
                    cmd.AddParameter("@PAYTERM", System.Data.SqlDbType.SmallInt, PaymentTerm);
                    cmd.AddParameter("@AMOUNT", System.Data.SqlDbType.Money, Amount);
                    cmd.AddParameter("@ExchangeRate", System.Data.SqlDbType.Money, ExchangeRate);
                    cmd.AddParameter("@REMARK", System.Data.SqlDbType.VarChar,Remark);
                    cmd.AddParameter("@EQUIVAMT", System.Data.SqlDbType.Money, EquivAmt);
                    cmd.AddParameter("@RelatedInvoice", System.Data.SqlDbType.VarChar, Invoice.InvoiceNumber);
                    // OLD?
                    cmd.AddParameter("@OLDRP", System.Data.SqlDbType.VarChar, RP);
                    cmd.AddParameter("@OLDACC", System.Data.SqlDbType.VarChar, Acc.Account);
                    cmd.AddParameter("@OLDCCY", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                    cmd.AddParameter("@OLDCUST_PRIN", System.Data.SqlDbType.VarChar, CustomerACFARAP.CUSTCode);
                    cmd.AddParameter("@OLDDOC_NO", System.Data.SqlDbType.VarChar, DocNo);
                    cmd.AddParameter("@OLDBranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                    cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID.ToString());
                    //cmd.AddParameter("@LastUpd", System.Data.SqlDbType.DateTime, DateTime.Now);
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
                            throw new Exception("ACFARAP is already exists. Please choose other ACFARAP.");
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

        public int InsUpDelACFARAP(int ExecCode, IDS.DataAccess.SqlServer cmd)
        {
            int result = 0;

            try
            {
                cmd.CommandText = "ARAPSaveHeader";
                cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);
                cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, RP);
                cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Acc.Account);
                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                cmd.AddParameter("@CUST_PRIN", System.Data.SqlDbType.VarChar, CustomerACFARAP.CUSTCode);
                cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DocNo);
                cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                cmd.AddParameter("@DEPTCODE", System.Data.SqlDbType.VarChar, DeptCode);
                cmd.AddParameter("@DOC_DATE", System.Data.SqlDbType.DateTime, DocDate);
                cmd.AddParameter("@RECEIVEDDATE", System.Data.SqlDbType.DateTime, ReceivedDate);
                cmd.AddParameter("@PAYTERM", System.Data.SqlDbType.SmallInt, PaymentTerm);
                cmd.AddParameter("@AMOUNT", System.Data.SqlDbType.Money, Amount);
                cmd.AddParameter("@ExchangeRate", System.Data.SqlDbType.Money, ExchangeRate);
                cmd.AddParameter("@REMARK", System.Data.SqlDbType.VarChar, Remark);
                cmd.AddParameter("@EQUIVAMT", System.Data.SqlDbType.Money, EquivAmt);
                cmd.AddParameter("@RelatedInvoice", System.Data.SqlDbType.VarChar, Invoice.InvoiceNumber);
                // OLD?
                cmd.AddParameter("@OLDRP", System.Data.SqlDbType.VarChar, RP);
                cmd.AddParameter("@OLDACC", System.Data.SqlDbType.VarChar, Acc.Account);
                cmd.AddParameter("@OLDCCY", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                cmd.AddParameter("@OLDCUST_PRIN", System.Data.SqlDbType.VarChar, CustomerACFARAP.CUSTCode);
                cmd.AddParameter("@OLDDOC_NO", System.Data.SqlDbType.VarChar, DocNo);
                cmd.AddParameter("@OLDBranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID.ToString());
                //cmd.AddParameter("@LastUpd", System.Data.SqlDbType.DateTime, DateTime.Now);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Open();
                
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
                        throw new Exception("ACFARAP is already exists. Please choose other ACFARAP.");
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

            return result;
        }

        public int InsUpDelACFARAPWithOld(int ExecCode, IDS.DataAccess.SqlServer cmd)
        {
            int result = 0;

            try
            {
                cmd.CommandText = "ARAPSaveHeader";
                cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);
                cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, RP);
                cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Acc.Account);
                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                cmd.AddParameter("@CUST_PRIN", System.Data.SqlDbType.VarChar, Supplier.SupCode);
                cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DocNo);
                cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                cmd.AddParameter("@DEPTCODE", System.Data.SqlDbType.VarChar, DeptCode);
                cmd.AddParameter("@DOC_DATE", System.Data.SqlDbType.DateTime, DocDate);
                cmd.AddParameter("@RECEIVEDDATE", System.Data.SqlDbType.DateTime, ReceivedDate);
                cmd.AddParameter("@PAYTERM", System.Data.SqlDbType.SmallInt, PaymentTerm);
                cmd.AddParameter("@AMOUNT", System.Data.SqlDbType.Money, Amount);
                cmd.AddParameter("@ExchangeRate", System.Data.SqlDbType.Money, ExchangeRate);
                cmd.AddParameter("@REMARK", System.Data.SqlDbType.VarChar, Remark);
                cmd.AddParameter("@EQUIVAMT", System.Data.SqlDbType.Money, EquivAmt);
                if (Invoice != null)
                {
                    cmd.AddParameter("@RelatedInvoice", System.Data.SqlDbType.VarChar, Invoice.InvoiceNumber);
                }
                
                // OLD?
                cmd.AddParameter("@OLDRP", System.Data.SqlDbType.VarChar, OldRP);
                cmd.AddParameter("@OLDACC", System.Data.SqlDbType.VarChar, OldAcc);
                cmd.AddParameter("@OLDCCY", System.Data.SqlDbType.VarChar, OldCcy);
                cmd.AddParameter("@OLDCUST_PRIN", System.Data.SqlDbType.VarChar, OldCust);
                cmd.AddParameter("@OLDDOC_NO", System.Data.SqlDbType.VarChar, OldDocNo);
                cmd.AddParameter("@OLDBranchCode", System.Data.SqlDbType.VarChar, OldBranch);
                cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID.ToString());
                //cmd.AddParameter("@LastUpd", System.Data.SqlDbType.DateTime, DateTime.Now);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Open();

                result = cmd.ExecuteNonQuery();
                //cmd.CommitTransaction();
            }
            catch (SqlException sex)
            {
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();

                switch (sex.Number)
                {
                    case 2627:
                        throw new Exception("ACFARAP is already exists. Please choose other ACFARAP.");
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

            return result;
        }

        public int InsUpDelACFARAPSupWithOld(int ExecCode, IDS.DataAccess.SqlServer cmd)
        {
            int result = 0;

            try
            {
                cmd.CommandText = "ARAPSUPSaveHeader";
                cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, ExecCode);
                cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, RP);
                cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, Acc.Account);
                cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, CCy.CurrencyCode);
                cmd.AddParameter("@SUP", System.Data.SqlDbType.VarChar, Supplier.SupCode);
                cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DocNo);
                cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, Branch.BranchCode);
                cmd.AddParameter("@DEPTCODE", System.Data.SqlDbType.VarChar, DeptCode);
                cmd.AddParameter("@DOC_DATE", System.Data.SqlDbType.DateTime, DocDate);
                cmd.AddParameter("@RECEIVEDDATE", System.Data.SqlDbType.DateTime, ReceivedDate);
                cmd.AddParameter("@PAYTERM", System.Data.SqlDbType.SmallInt, PaymentTerm);
                cmd.AddParameter("@AMOUNT", System.Data.SqlDbType.Money, Amount);
                cmd.AddParameter("@ExchangeRate", System.Data.SqlDbType.Money, ExchangeRate);
                cmd.AddParameter("@REMARK", System.Data.SqlDbType.VarChar, Remark);
                cmd.AddParameter("@EQUIVAMT", System.Data.SqlDbType.Money, EquivAmt);
                if (Invoice != null)
                {
                    cmd.AddParameter("@RelatedInvoice", System.Data.SqlDbType.VarChar, Invoice.InvoiceNumber);
                }

                // OLD?
                cmd.AddParameter("@OLDRP", System.Data.SqlDbType.VarChar, OldRP);
                cmd.AddParameter("@OLDACC", System.Data.SqlDbType.VarChar, OldAcc);
                cmd.AddParameter("@OLDCCY", System.Data.SqlDbType.VarChar, OldCcy);
                cmd.AddParameter("@OLDSUP", System.Data.SqlDbType.VarChar, OldCust);
                cmd.AddParameter("@OLDDOC_NO", System.Data.SqlDbType.VarChar, OldDocNo);
                cmd.AddParameter("@OLDBranchCode", System.Data.SqlDbType.VarChar, OldBranch);
                cmd.AddParameter("@OperatorID", System.Data.SqlDbType.VarChar, OperatorID.ToString());
                //cmd.AddParameter("@LastUpd", System.Data.SqlDbType.DateTime, DateTime.Now);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Open();

                result = cmd.ExecuteNonQuery();
                //cmd.CommitTransaction();
            }
            catch (SqlException sex)
            {
                if (cmd.Transaction != null)
                    cmd.RollbackTransaction();

                switch (sex.Number)
                {
                    case 2627:
                        throw new Exception("ACFARAP is already exists. Please choose other ACFARAP.");
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

            return result;
        }

        public int DelACFARAP(Tool.PageActivity ExecCode, string[] data)
        {
            int result = 0;

            if (data == null)
                throw new Exception("No data found");

            using (IDS.DataAccess.SqlServer cmd = new IDS.DataAccess.SqlServer())
            {
                try
                {
                    cmd.CommandText = "ARAPSaveHeader";
                    cmd.Open();
                    cmd.BeginTransaction();

                    string[] item;
                    string rp = "";
                    string acc = "";
                    string ccy = "";
                    string custCode = "";
                    string docno = "";
                    string branchCode = "";

                    char separator = ';';

                    for (int i = 0; i < data.Length; i++)
                    {
                        item = data[i].Split(separator);

                        rp = item[0];
                        acc = item[1];
                        ccy = item[2];
                        custCode = item[3];
                        docno = item[4];
                        branchCode = item[5];                      
                        
                        cmd.CommandText = "ARAPSaveHeader";
                        cmd.AddParameter("@Init", System.Data.SqlDbType.TinyInt, (int)ExecCode);
                        cmd.AddParameter("@RP", System.Data.SqlDbType.VarChar, rp);
                        cmd.AddParameter("@ACC", System.Data.SqlDbType.VarChar, acc);
                        cmd.AddParameter("@CCY", System.Data.SqlDbType.VarChar, ccy);
                        cmd.AddParameter("@CUST_PRIN", System.Data.SqlDbType.VarChar, custCode);
                        cmd.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, docno);
                        cmd.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branchCode);
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
                            throw new Exception("ACFARAP is already exists. Please choose other ACFARAP.");
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
}
