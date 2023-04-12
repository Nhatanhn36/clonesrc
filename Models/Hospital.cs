using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class Hospital
{
    public int HospitalId { get; set; }

    public string HospitalName { get; set; } = null!;

    public string? Location { get; set; }

    public string? Phone { get; set; }

    public string? HospitalUrl { get; set; }
}
