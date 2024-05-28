using System.ComponentModel;

namespace IDS.FixedAsset
{
    public enum FADepreMethod
    {
        [Description("Straight Line")]
        Straight_Line = 0,
        [Description("Double Declining")]
        Double_Declining = 1,
        [Description("Not Depreciate")]
        Not_Depreciate = 2
    }

    public enum FAAssetStatus
    {
        [Description("Active")]
        Active = 0,
        [Description("Sold")]
        Sold = 1,
        [Description("Move")]
        Move = 2,
        [Description("Write Off")]
        Write_Off = 3
    }
}