using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class UserLogin
{
    public int IdAccount { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte IsAdmin { get; set; }



    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();
}
