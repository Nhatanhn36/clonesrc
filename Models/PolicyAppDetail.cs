using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class PolicyAppDetail
{
    public int PolicyId { get; set; }

    public int? RequestId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Date { get; set; }

    public string? Status { get; set; }

    public string? Reason { get; set; }

    public virtual PoliciesReqDetail? Request { get; set; }
}
