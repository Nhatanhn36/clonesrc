using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class PoliciesReqDetail
{
    public int RequestId { get; set; }

    public DateTime? RequestDate { get; set; }

    public int EmployeeId { get; set; }

    public string? Status { get; set; }

    public decimal? Emi { get; set; }

    public int? PolicyId { get; set; }

    public string? PolicyName { get; set; }

    public decimal? PolicyAmount { get; set; }

    public int? CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Policy? Policy { get; set; }
}
