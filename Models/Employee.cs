using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? Designation { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? ContactNo { get; set; }

    public DateTime? Joindate { get; set; }

    public string? Address { get; set; }

    public int? IdAccount { get; set; }

    public int? PolicyId { get; set; }

    public string? PolicyStatus { get; set; }

    public virtual UserLogin? IdAccountNavigation { get; set; }

    public virtual Policy? Policy { get; set; }
}
