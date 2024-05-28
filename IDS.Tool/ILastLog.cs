using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IDS.Tool
{
    public interface ILastLog
    {
        string EntryUser { get; set; }
        DateTime EntryDate { get; set; }
        string OperatorID { get; set; }
        DateTime LastUpdate { get; set; }
    }
}
