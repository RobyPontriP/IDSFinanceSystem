using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.FixedAsset
{
    public class FAJournal
    {
        public string Period { get; set; }
        public string BranchCode { get; set; }
        public string BegVal { get; set; }
        public string Increment { get; set; }
        public string Decrement { get; set; }
        public string Depreciation { get; set; }
        public string AccumDepr { get; set; }
        public string EndVal { get; set; }
        public string Journal { get; set; }
        public string Voucher { get; set; }
        public string OperatorID { get; set; }
        public string LastUpdate { get; set; }
        public string ENTRYUSER { get; set; }
        public string ENTRYDATE { get; set; }
        public FAJournal() { }



        public static List<IDS.FixedAsset.FAJournalSelect> GetFAJournal(string period,string ccy, string branch)
        {
            List<IDS.FixedAsset.FAJournalSelect> SelFAJournal = new List<IDS.FixedAsset.FAJournalSelect>();

            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFAJournal";
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar,period );
                db.AddParameter("@Ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@Branch", System.Data.SqlDbType.VarChar,branch);
                db.AddParameter("@Init", System.Data.SqlDbType.TinyInt, 1);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();

                db.ExecuteReader();

                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            IDS.FixedAsset.FAJournalSelect b = new IDS.FixedAsset.FAJournalSelect() {
                                 AccumAccName=dr["AccumAccName"].ToString(),
                                 AccumAccNo= dr["AccumAccNo"].ToString(),
                                 AccumDepr= dr["AccumDepr"].ToString(),
                                 BegVal= dr["BegVal"].ToString(),
                                 BranchCode= dr["BranchCode"].ToString(),
                                 Ccy= dr["Ccy"].ToString(),
                                 DepAccName= dr["DepAccName"].ToString(),
                                 DepAccNo= dr["DepAccNo"].ToString(),
                                 Depreciation= dr["Depreciation"].ToString(),
                                 EndVal= dr["EndVal"].ToString(),
                                 Increment= dr["Increment"].ToString(),
                                 ItemGroup= dr["ItemGroup"].ToString(),
                                 Journal= dr["Journal"].ToString(),
                                 KEY= dr["KEY"].ToString(),
                                 Period= dr["Period"].ToString(),
                                 Voucher= dr["Voucher"].ToString()
                            };
                            SelFAJournal.Add(b);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
            return SelFAJournal;
        }

        public static IDS.FixedAsset.FAGroup GetFaGroup(string AssetGroup)
        {
            IDS.FixedAsset.FAGroup f = new IDS.FixedAsset.FAGroup();
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFAGroup";
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 2);
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, AssetGroup);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader){
                    if (dr.HasRows){
                        dr.Read();
                        f.Code = dr["AssetGroup"].ToString();
                        f.DepreMethod = (FADepreMethod)(System.Convert.ToInt16( dr["DepMethod"]));
                        f.DepreRate =IDS.Tool.GeneralHelper.NullToDecimal(dr["DepRate"].ToString(),0);
                        f.DepreYear = System.Convert.ToInt16(dr["DepYear"].ToString());
                        f.Description =dr["AssetGroupDesc"].ToString();
                        f.EntryDate = System.Convert.ToDateTime(dr["ENTRYDATE"]);
                        f.EntryUser = dr["ENTRYUSER"].ToString();
                        f.GLAccDepreExpense =dr["GLAccDepExp"].ToString();
                        f.GLAccGainLoss =dr["GLGainLoss"].ToString();
                        f.GLAccItemGroup = dr["GLAccItemGrp"].ToString();
                        f.GLAccumDepre = dr["GLAccAccumDep"].ToString();
                        f.LastUpdate = System.Convert.ToDateTime(dr["LastUpdate"].ToString());
                        f.OperatorID = dr["OperatorID"].ToString();
                    }
                    if (!dr.IsClosed)dr.Close();
                }
                db.Close();
            }
            return f;
        }

        public static List<FAAssetGroupDetail> GetFAAssetGroupDetail(string ccy, string period, string group, string branch)
        {
            List<IDS.FixedAsset.FAAssetGroupDetail> list_ = new List<IDS.FixedAsset.FAAssetGroupDetail>();
            using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
            {
                db.CommandText = "SelFAGroup";
                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, period);
                db.AddParameter("@Code", System.Data.SqlDbType.VarChar, group);
                db.AddParameter("@ItemGroup", System.Data.SqlDbType.VarChar, group);
                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, branch);
                db.AddParameter("@Ccy", System.Data.SqlDbType.VarChar, ccy);
                db.AddParameter("@type", System.Data.SqlDbType.TinyInt, 4);
                db.CommandType = System.Data.CommandType.StoredProcedure;
                db.Open();
                db.ExecuteReader();
                using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            IDS.FixedAsset.FAAssetGroupDetail b = new IDS.FixedAsset.FAAssetGroupDetail()
                            {
                                AssetGroup = dr["ItemGroup"].ToString(),
                                AssetNo= dr["AssetNo"].ToString(),
                                //Decrement= dr["Decrement"].ToString(),
                                Decrement = String.Format("{0:n}", IDS.Tool.GeneralHelper.NullToDecimal(dr["Decrement"], 0)),
                                //BegVal = dr["BegVal"].ToString(),
                                BegVal = String.Format("{0:n}", IDS.Tool.GeneralHelper.NullToDecimal(dr["BegVal"], 0)),
                                //Depreciation= dr["Depreciation"].ToString(),
                                Depreciation = String.Format("{0:n}", IDS.Tool.GeneralHelper.NullToDecimal(dr["Depreciation"], 0)),
                                //EndVal= dr["EndVal"].ToString(),
                                EndVal = String.Format("{0:n}", IDS.Tool.GeneralHelper.NullToDecimal(dr["EndVal"], 0)),
                                //Increment = dr["Increment"].ToString()
                                Increment = String.Format("{0:n}", IDS.Tool.GeneralHelper.NullToDecimal(dr["Increment"], 0))
                            };
                            list_.Add(b);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                db.Close();
            }
            return list_;
        }

        //public bool ProccessGo(List<FAProsessCmd> l)
        //{
        //    bool b = false;
        //    string result = "";
        //    string SQLText = "";
        //    string voucher = "";

        //    foreach (var x in l)
        //    {
        //        string strMonth = x.Period.Substring(4, 2);
        //        string strYear = x.Period.Substring(0, 4);
        //        //string d = "ItemGroup==>"+x.ItemGroup + ", Period==>"+x.Period + ", CCY==>"+x.IDR;
        //        //System.Diagnostics.Debug.WriteLine(d);

        //        SQLText = "SELECT (FAA.CCY + ';' + FAS.Period + ';' + ItemGroup + ';' + FAS.BranchCode) [KEY], FAA.CCY [Ccy], FAS.BranchCode [BranchCode], ACFG.ACC [AccumAccNo], ACFG.[NAME] [AccumAccName], ACFGL.ACC [DepAccNo], ACFGL.[NAME] [DepAccName], FAS.Period, ItemGroup, SUM(BegVal) [BegVal], SUM(Increment) [Increment], SUM(Decrement) [Decrement], SUM(Depreciation) [Depreciation], SUM(AccumDepr) [AccumDepr], SUM(EndVal) [EndVal], Journal, Voucher " +
        //                                  "FROM (FASchedule FAS INNER JOIN FAAsset FAA ON FAS.AssetNo = FAA.AssetNo AND FAA.BranchCode = FAS.BranchCode) " +
        //                                  "INNER JOIN FAAssetGroup FAAG ON FAA.ItemGroup = FAAG.AssetGroup " +
        //                                  "LEFT JOIN ACFGLMH ACFG ON FAAG.GLAccAccumDep = ACFG.ACC " +
        //                                  "LEFT JOIN ACFGLMH ACFGL ON FAAG.GLAccDepExp = ACFGL.ACC " +
        //                                  "WHERE ItemGroup =@ItemGroup AND FAS.Period =@Period AND FAA.CCY =@CCY AND FAA.BranchCode =@Branch AND Journal = 0 AND Status IN (0, 4) " +
        //                                  "GROUP BY FAS.Period, FAS.BranchCode, ItemGroup, Journal, Voucher, ACFG.ACC, ACFG.[Name], ACFGL.ACC, ACFGL.[NAME], FAA.CCY";

        //        //System.Diagnostics.Debug.WriteLine(SQLText+"\n\n"+"============================");
        //        using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
        //        {
        //            db.CommandText = "FAInsertJournal";
        //            db.CommandType = System.Data.CommandType.StoredProcedure;
        //            db.AddParameter("@scode", System.Data.SqlDbType.VarChar, "FA");
        //            db.AddParameter("@brcode", System.Data.SqlDbType.VarChar, x.Branch);
        //            db.AddParameter("@ent_date", System.Data.SqlDbType.DateTime, DateTime.Now.Date);
        //            //db.AddParameter("@trans_date", System.Data.SqlDbType.DateTime, Convert.ToDateTime(Convert.ToDateTime(x.Period).Month + "/" + "1" + "/" + Convert.ToDateTime(x.Period).Year).Date);
        //            db.AddParameter("@trans_date", System.Data.SqlDbType.DateTime, x.Period.Substring(4, 2) + "/" + "1" + "/" + x.Period.Substring(0, 4));
        //            db.AddParameter("@operatorId", System.Data.SqlDbType.VarChar, OperatorID);
        //            db.AddParameter("@lastUpd", System.Data.SqlDbType.DateTime, DateTime.Now);
        //            db.AddParameter("@ARAP", System.Data.SqlDbType.TinyInt, 0);
                    
        //            //db.AddParameter("@vcr", System.Data.SqlDbType.VarChar, voucher);
        //            //db.DbCommand.Parameters["@vcr"]

        //            db.AddParameter("@vcr", System.Data.SqlDbType.VarChar,12, System.Data.ParameterDirection.Output);
        //            db.CommandType = System.Data.CommandType.StoredProcedure;
        //            db.Open();
        //            db.BeginTransaction();
        //            db.ExecuteNonQueryWithParamOutput();
        //            db.CommitTransaction();
        //            voucher = (db.DbCommand.Parameters["@vcr"] as System.Data.SqlClient.SqlParameter).Value.ToString();
        //            db.Close();
        //        //}//end using
        //        string AccDepr = "";
        //        string AccExp = "";
        //        string Ccy = "";
        //        double TotalAmount = 0;
        //        string Key = "";

        //        //using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
        //        //{
        //            db.CommandText = SQLText;
        //            db.AddParameter("@ItemGroup", System.Data.SqlDbType.VarChar, x.ItemGroup);
        //            db.AddParameter("@Period", System.Data.SqlDbType.VarChar, x.Period);
        //            db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, x.IDR);
        //            db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, x.Branch);
        //            db.CommandType = System.Data.CommandType.Text;
        //            db.Open();
        //            db.ExecuteReader();
        //            using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
        //            {
        //                if (dr.HasRows)
        //                {
        //                    dr.Read();
        //                    AccDepr = Convert.ToString(dr["AccumAccNo"]);
        //                    AccExp = Convert.ToString(dr["DepAccNo"]);
        //                    TotalAmount = Convert.ToDouble(dr["Depreciation"]);
        //                    Ccy = Convert.ToString(dr["Ccy"]);
        //                    Key = Convert.ToString(dr["KEY"]);
        //                }
        //                if (!dr.IsClosed) dr.Close();
        //            }
        //            db.Close();
        //        //}//end using

        //        if (AccDepr.Trim() == "" || AccExp.Trim() == "")
        //        {
        //            b = false;
        //            result = "Please Insert Depreciation Accumulation Account and Depreciaton Expense Account";
        //                db.RollbackTransaction();
        //                db.Close();
        //            //WebMsgApp.WebMsgBox.Show("Please Insert Depreciation Accumulation Account and Depreciaton Expense Account");
        //        }
        //        else
        //        {
        //            //using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
        //            //{
        //                db.CommandText = "FAInsertJournalD";
        //                db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, "FA");
        //                db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voucher);
        //                db.AddParameter("@COUNTER", System.Data.SqlDbType.VarChar, 1);
        //                db.AddParameter("@BRCODE", System.Data.SqlDbType.VarChar, "DTN");
        //                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, AccExp);
        //                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Ccy);
        //                db.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                db.AddParameter("@PROJ", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                db.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                db.AddParameter("@DESCRIP", System.Data.SqlDbType.VarChar, "Fixed Asset Depreciation of : "+x.ItemGroup);//belum
        //                db.AddParameter("@AMOUNT", System.Data.SqlDbType.VarChar, TotalAmount);
        //                db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, "0000000000");
        //                db.AddParameter("@INSTRUMENT_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                db.CommandType = System.Data.CommandType.StoredProcedure;
        //                db.Open();
        //                db.BeginTransaction();
        //                db.ExecuteNonQuery();

        //                db.CommandText = "FAInsertJournalD";
        //                db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, "FA");
        //                db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voucher);
        //                db.AddParameter("@COUNTER", System.Data.SqlDbType.VarChar, 2);
        //                db.AddParameter("@BRCODE", System.Data.SqlDbType.VarChar, "DTN");
        //                db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, AccDepr);
        //                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Ccy);
        //                db.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                db.AddParameter("@PROJ", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                db.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                db.AddParameter("@DESCRIP", System.Data.SqlDbType.VarChar, "Fixed Asset Depreciation of : " + x.ItemGroup);//belum
        //                db.AddParameter("@AMOUNT", System.Data.SqlDbType.VarChar, TotalAmount);
        //                db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, "0000000000");
        //                db.AddParameter("@INSTRUMENT_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
        //                db.CommandType = System.Data.CommandType.StoredProcedure;
        //                db.Open();
        //                //db.BeginTransaction();
        //                db.ExecuteNonQuery();


        //                string upQuery = "SELECT FAA.AssetNo "
        //                                   + "FROM FASchedule FAS INNER JOIN FAAsset FAA "
        //                                   + "ON FAS.AssetNo = FAA.AssetNo AND FAS.BranchCode = FAA.BranchCode "
        //                                   + "WHERE ItemGroup = @ItemGroup AND Period = @Period AND FAA.BranchCode = @BranchCode AND FAA.Status IN (0, 4) ";

        //                string query = string.Format("UPDATE FASchedule SET Journal = 1, Voucher = @VOUCHER WHERE Period = @Period AND BranchCode = @BranchCode AND AssetNo IN ({0})", upQuery);

        //                db.CommandText = query;
        //                db.AddParameter("@Period", System.Data.SqlDbType.VarChar, x.Period);
        //                db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voucher);
        //                db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, x.Branch);
        //                db.AddParameter("@ItemGroup", System.Data.SqlDbType.VarChar, x.ItemGroup);
        //                db.AddParameter("@Key", System.Data.SqlDbType.VarChar, Key);
        //                db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, x.IDR);
        //                db.CommandType = System.Data.CommandType.Text;
        //                //db.BeginTransaction();
        //                db.Open();
        //                db.ExecuteNonQuery();

        //                //db.Close();
        //                db.CommitTransaction();
        //            }
        //        }


        //    }//enf for
        //    return b;
        //}

        public string ProccessGo(List<FAProsessCmd> l)
        {
            bool b = false;
            string result = "";
            string SQLText = "";
            string voucher = "";

            foreach (var x in l)
            {
                string strMonth = x.Period.Substring(4, 2);
                string strYear = x.Period.Substring(0, 4);
                //string d = "ItemGroup==>"+x.ItemGroup + ", Period==>"+x.Period + ", CCY==>"+x.IDR;
                //System.Diagnostics.Debug.WriteLine(d);

                SQLText = "SELECT (FAA.CCY + ';' + FAS.Period + ';' + ItemGroup + ';' + FAS.BranchCode) [KEY], FAA.CCY [Ccy], FAS.BranchCode [BranchCode], ACFG.ACC [AccumAccNo], ACFG.[NAME] [AccumAccName], ACFGL.ACC [DepAccNo], ACFGL.[NAME] [DepAccName], FAS.Period, ItemGroup, SUM(BegVal) [BegVal], SUM(Increment) [Increment], SUM(Decrement) [Decrement], SUM(Depreciation) [Depreciation], SUM(AccumDepr) [AccumDepr], SUM(EndVal) [EndVal], Journal, Voucher " +
                                          "FROM (FASchedule FAS INNER JOIN FAAsset FAA ON FAS.AssetNo = FAA.AssetNo AND FAA.BranchCode = FAS.BranchCode) " +
                                          "INNER JOIN FAAssetGroup FAAG ON FAA.ItemGroup = FAAG.AssetGroup " +
                                          "LEFT JOIN ACFGLMH ACFG ON FAAG.GLAccAccumDep = ACFG.ACC " +
                                          "LEFT JOIN ACFGLMH ACFGL ON FAAG.GLAccDepExp = ACFGL.ACC " +
                                          "WHERE ItemGroup =@ItemGroup AND FAS.Period =@Period AND FAA.CCY =@CCY AND FAA.BranchCode =@Branch AND Journal = 0 AND Status IN (0, 4) " +
                                          "GROUP BY FAS.Period, FAS.BranchCode, ItemGroup, Journal, Voucher, ACFG.ACC, ACFG.[Name], ACFGL.ACC, ACFGL.[NAME], FAA.CCY";

                //System.Diagnostics.Debug.WriteLine(SQLText+"\n\n"+"============================");
                using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                {
                    db.CommandText = "FAInsertJournal";
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.AddParameter("@scode", System.Data.SqlDbType.VarChar, "FA");
                    db.AddParameter("@brcode", System.Data.SqlDbType.VarChar, x.Branch);
                    db.AddParameter("@ent_date", System.Data.SqlDbType.DateTime, DateTime.Now.Date);
                    //db.AddParameter("@trans_date", System.Data.SqlDbType.DateTime, Convert.ToDateTime(Convert.ToDateTime(x.Period).Month + "/" + "1" + "/" + Convert.ToDateTime(x.Period).Year).Date);
                    db.AddParameter("@trans_date", System.Data.SqlDbType.DateTime, x.Period.Substring(4, 2) + "/" + "1" + "/" + x.Period.Substring(0, 4));
                    db.AddParameter("@operatorId", System.Data.SqlDbType.VarChar, OperatorID);
                    db.AddParameter("@lastUpd", System.Data.SqlDbType.DateTime, DateTime.Now);
                    db.AddParameter("@ARAP", System.Data.SqlDbType.TinyInt, 0);

                    //db.AddParameter("@vcr", System.Data.SqlDbType.VarChar, voucher);
                    //db.DbCommand.Parameters["@vcr"]

                    db.AddParameter("@vcr", System.Data.SqlDbType.VarChar, 12, System.Data.ParameterDirection.Output);
                    db.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Open();
                    db.BeginTransaction();
                    db.ExecuteNonQueryWithParamOutput();
                    db.CommitTransaction();
                    voucher = (db.DbCommand.Parameters["@vcr"] as System.Data.SqlClient.SqlParameter).Value.ToString();
                    db.Close();
                    //}//end using
                    string AccDepr = "";
                    string AccExp = "";
                    string Ccy = "";
                    double TotalAmount = 0;
                    string Key = "";

                    //using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                    //{
                    db.CommandText = SQLText;
                    db.AddParameter("@ItemGroup", System.Data.SqlDbType.VarChar, x.ItemGroup);
                    db.AddParameter("@Period", System.Data.SqlDbType.VarChar, x.Period);
                    db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, x.IDR);
                    db.AddParameter("@Branch", System.Data.SqlDbType.VarChar, x.Branch);
                    db.CommandType = System.Data.CommandType.Text;
                    db.Open();
                    db.ExecuteReader();
                    using (System.Data.SqlClient.SqlDataReader dr = db.DbDataReader as System.Data.SqlClient.SqlDataReader)
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            AccDepr = Convert.ToString(dr["AccumAccNo"]);
                            AccExp = Convert.ToString(dr["DepAccNo"]);
                            TotalAmount = Convert.ToDouble(dr["Depreciation"]);
                            Ccy = Convert.ToString(dr["Ccy"]);
                            Key = Convert.ToString(dr["KEY"]);
                        }
                        if (!dr.IsClosed) dr.Close();
                    }
                    db.Close();
                    //}//end using

                    if (AccDepr.Trim() == "" || AccExp.Trim() == "")
                    {
                        b = false;
                        result = "Please Insert Depreciation Accumulation Account and Depreciaton Expense Account";
                        db.RollbackTransaction();
                        db.Close();
                        //WebMsgApp.WebMsgBox.Show("Please Insert Depreciation Accumulation Account and Depreciaton Expense Account");
                    }
                    else
                    {
                        //using (IDS.DataAccess.SqlServer db = new DataAccess.SqlServer())
                        //{
                        db.CommandText = "FAInsertJournalD";
                        db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, "FA");
                        db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voucher);
                        db.AddParameter("@COUNTER", System.Data.SqlDbType.VarChar, 1);
                        db.AddParameter("@BRCODE", System.Data.SqlDbType.VarChar, "DTN");
                        db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, AccExp);
                        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Ccy);
                        db.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@PROJ", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DESCRIP", System.Data.SqlDbType.VarChar, "Fixed Asset Depreciation of : " + x.ItemGroup);//belum
                        db.AddParameter("@AMOUNT", System.Data.SqlDbType.VarChar, TotalAmount);
                        db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, "0000000000");
                        db.AddParameter("@INSTRUMENT_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.Open();
                        db.BeginTransaction();
                        db.ExecuteNonQuery();

                        db.CommandText = "FAInsertJournalD";
                        db.AddParameter("@SCODE", System.Data.SqlDbType.VarChar, "FA");
                        db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voucher);
                        db.AddParameter("@COUNTER", System.Data.SqlDbType.VarChar, 2);
                        db.AddParameter("@BRCODE", System.Data.SqlDbType.VarChar, "DTN");
                        db.AddParameter("@ACC", System.Data.SqlDbType.VarChar, AccDepr);
                        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, Ccy);
                        db.AddParameter("@DEPT", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@PROJ", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DOC_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.AddParameter("@DESCRIP", System.Data.SqlDbType.VarChar, "Fixed Asset Depreciation of : " + x.ItemGroup);//belum
                        db.AddParameter("@AMOUNT", System.Data.SqlDbType.VarChar, -1 * TotalAmount);
                        db.AddParameter("@UPD", System.Data.SqlDbType.VarChar, "0000000000");
                        db.AddParameter("@INSTRUMENT_NO", System.Data.SqlDbType.VarChar, DBNull.Value);
                        db.CommandType = System.Data.CommandType.StoredProcedure;
                        db.Open();
                        //db.BeginTransaction();
                        db.ExecuteNonQuery();


                        string upQuery = "SELECT FAA.AssetNo "
                                           + "FROM FASchedule FAS INNER JOIN FAAsset FAA "
                                           + "ON FAS.AssetNo = FAA.AssetNo AND FAS.BranchCode = FAA.BranchCode "
                                           + "WHERE ItemGroup = @ItemGroup AND Period = @Period AND FAA.BranchCode = @BranchCode AND FAA.Status IN (0, 4) ";

                        string query = string.Format("UPDATE FASchedule SET Journal = 1, Voucher = @VOUCHER WHERE Period = @Period AND BranchCode = @BranchCode AND AssetNo IN ({0})", upQuery);

                        db.CommandText = query;
                        db.AddParameter("@Period", System.Data.SqlDbType.VarChar, x.Period);
                        db.AddParameter("@VOUCHER", System.Data.SqlDbType.VarChar, voucher);
                        db.AddParameter("@BranchCode", System.Data.SqlDbType.VarChar, x.Branch);
                        db.AddParameter("@ItemGroup", System.Data.SqlDbType.VarChar, x.ItemGroup);
                        db.AddParameter("@Key", System.Data.SqlDbType.VarChar, Key);
                        db.AddParameter("@CCY", System.Data.SqlDbType.VarChar, x.IDR);
                        db.CommandType = System.Data.CommandType.Text;
                        //db.BeginTransaction();
                        db.Open();
                        db.ExecuteNonQuery();

                        //db.Close();
                        db.CommitTransaction();
                    }
                }


            }//enf for
            return result;
        }
        //public bool ProccessGo(List<FAProsessCmd> l)
        //{
        //    bool b = false;



        //    //enf for
        //    return b;
        //}
    }

    public class FAJournalSelect
    {
        public string KEY { get; set; }
        public string Ccy { get; set; }
        public string BranchCode { get; set; }
        public string AccumAccNo { get; set; }
        public string AccumAccName { get; set; }
        public string DepAccNo { get; set; }
        public string DepAccName { get; set; }
        public string Period { get; set; }
        public string ItemGroup { get; set; }
        public string BegVal { get; set; }
        public string Increment { get; set; }
        public string Depreciation { get; set; }
        public string AccumDepr { get; set; }
        public string EndVal { get; set; }
        public string Journal { get; set; }
        public string Voucher { get; set; }
        public FAJournalSelect() { }

    }

    public class FAAssetGroupDetail
    {
        public string AssetNo { get; set; }
        public string AssetGroup { get; set; }
        public string BegVal { get; set; }
        public string Increment { get; set; }
        public string Decrement { get; set; }
        public string Depreciation { get; set; }
        public string EndVal { get; set; }
        public FAAssetGroupDetail() { }
    }

    public class FAProsessCmd
    {
        public string ItemGroup { get; set; }
        public string Period { get; set; }
        public string IDR { get; set; }
        public string Branch { get; set; }
        public FAProsessCmd() { }
    }

}
