using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace IDS.Tool
{
    /// <summary>
    /// Untuk CRUD (Create, Read, Update, Delete) pada form / page
    /// </summary>
    public enum PageActivity : int
    {
        none = 0,
        Insert = 1,
        Edit = 2,
        Delete = 3,
        Process = 4
    }

    /// <summary>
    /// Untuk User Group Access
    /// </summary>
    public enum GroupAccess
    {
        [Description("None")]
        None = 0,
        [Description("Read Only")]
        Read = 1,
        [Description("Create")]
        Create = 2,
        [Description("Edit")]
        Edit = 3,
        [Description("Delete")]
        Delete = 4,
        [Description("Create Edit")]
        Create_Edit = 5,
        [Description("Create Delete")]
        Create_Delete = 6,
        [Description("Edit Delete")]
        Edit_Delete = 7,
        [Description("Create Edit Delete")]
        Create_Edit_Delete = 8        
    }


    #region GL Enum
    public enum GLAccountTotalDetail
    {
        AccountTotal = 1,
        AccountDetail = 0
    }

    public enum GLAccountGroup
    {
        [Description("Asset")]
        Asset = 1,

        [System.ComponentModel.DataAnnotations.Display(Name = "Liabilities and Capital")]
        [Description("Liabilities and Capital")]
        Liabilities_and_Capital = 2,
        [Description("Income")]
        Income = 3,
        [Description("Expense")]
        Expense = 4,

        [System.ComponentModel.DataAnnotations.Display(Name = "P/L Summary")]
        [Description("P/L Summary")]
        PL_Summary = 5
    }

    public enum GLCashBank
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Cash")]
        [Description("Cash")]
        Cash = 0,
        [System.ComponentModel.DataAnnotations.Display(Name = "Bank")]
        [Description("Bank")]
        Bank = 1
    }

    public enum GLSpecialAccount
    {
        PL = 1,
        AR = 2,
        AP = 3,
        DL = 4,
        KS = 5,
        BN = 6,
        RE = 7,
        BY = 8
    }
    #endregion

    #region Sales Enum
    //public enum PaymentType
    //{
    //    Cash = 0,
    //    [DescriptionAttribute("Credit Card")]
    //    CreditCard = 1,
    //    Cheque = 2,
    //    [DescriptionAttribute("Giro/Transfer")]
    //    Giro = 3
    //}
    public enum PaymentType
    {
        Cash = 0,
        Cheque = 1,
        Giro = 2,
        Transfer = 3,
        Other = 4
    }

    public enum PaymentTo
    {
        Supplier = 1,
        Customer = 0,
        General = 2
    }

    public enum PaymentMethod
    {
        [DescriptionAttribute("Cash Payment")]
        Cash = 0,
        [DescriptionAttribute("Non-Cash")]
        NonCash = 1
    }

    public enum Gender
    {
        [DescriptionAttribute("Male")]
        Male = 0,
        [DescriptionAttribute("Female")]
        Female = 1
    }

    public enum PaymentFlag
    {
        Canceled = 2,
        Processed = 1,
        Unprocessed = 0
    }

    public enum GLBankStatementMatchStatus
    {
        [DescriptionAttribute("Unmatch")]
        Unmatch = 0,

        [DescriptionAttribute("Match")]
        Match = 1,

        [DescriptionAttribute("All")]
        All = 2
    }

    public enum AllocationType
    {
        [DescriptionAttribute("Receivable")]
        Receivable = 1,

        [DescriptionAttribute("Tax")]
        Tax = 2,

        [DescriptionAttribute("Bank Charges")]
        BankCharges = 3,

        [DescriptionAttribute("Others")]
        Others = 4,

        [DescriptionAttribute("Gain/Loss")]
        GainLoss = 5,

        [DescriptionAttribute("PPn")]
        PPn = 6
    }
    #endregion
}
