using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class Policy
{
    public int PolicyId { get; set; }

    public string? PolicyName { get; set; }

    public int? CompanyId { get; set; }

    public string? PolicyDescription { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Emi { get; set; }

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<PoliciesReqDetail> PoliciesReqDetails { get; } = new List<PoliciesReqDetail>();

    public Company Company { get; set; }
}
