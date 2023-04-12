using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNo { get; set; }

    public string? CompanyUrl { get; set; }

    public virtual ICollection<PoliciesReqDetail> PoliciesReqDetails { get; } = new List<PoliciesReqDetail>();
}
